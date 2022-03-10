using System;
using System.Collections.Generic;
using System.Linq;

using TalusCI.Runtime.AppApi;

using UnityEditor;
using UnityEditor.Build.Reporting;

namespace TalusCI.Editor
{
    public class BuildActions
    {
        private static AppInfo _AppInfo = new AppInfo();

        public static void IOSDevelopment()
        {
            EditorUserBuildSettings.development = true;
            EditorApplication.Exit(CreateBuild().summary.result == BuildResult.Succeeded ? 0 : 1);
        }

        public static void IOSRelease()
        {
            EditorApplication.Exit(CreateBuild().summary.result == BuildResult.Succeeded ? 0 : 1);
        }

        private static string[] GetScenes()
        {
            return (from t in EditorBuildSettings.scenes where t.enabled select t.path).ToArray();
        }

        private static BuildReport CreateBuild()
        {
            if (PlayerSettings.SplashScreen.showUnityLogo)
            {
                PlayerSettings.SplashScreen.showUnityLogo = false;
            }

            //
            GenerateExportOptions();

            //
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);

            return BuildPipeline.BuildPlayer(GetScenes(), _AppInfo.IOSFolder, BuildTarget.iOS, BuildOptions.CompressWithLz4HC);
        }

        private static void GenerateExportOptions()
        {
            var fileContents = new List<string>
            {
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>",
                "<!DOCTYPE plist PUBLIC \"-//Apple//DTD PLIST 1.0//EN\" \"http://www.apple.com/DTDs/PropertyList-1.0.dtd\">",
                "<plist version=\"1.0\">",
                "<dict>",
                "    <key>compileBitcode</key>",
                "    <false/>",
                "    <key>provisioningProfiles</key>",
                "    <dict>",
               $"        <key>{_AppInfo.Identifier}</key>",
               $"        <string>{_AppInfo.ProvisioningProfileName}</string>",
                "    </dict>",
                "    <key>method</key>",
                "    <string>app-store</string>",
                "    <key>signingCertificate</key>",
               $"    <string>{_AppInfo.SigningCertificateName}</string>",
                "    <key>signingStyle</key>",
                "    <string>manual</string>",
                "    <key>stripSwiftSymbols</key>",
                "    <true/>",
                "    <key>teamID</key>",
               $"    <string>{_AppInfo.TeamID}</string>",
                "    <key>uploadSymbols</key>",
                "    <false/>",
                "</dict>",
                "</plist>"
            };

            Console.WriteLine("[TalusBuild] exportOptions.plist created at " + _AppInfo.ExportOptionsPath);
            System.IO.File.WriteAllLines(_AppInfo.ExportOptionsPath, fileContents.ToArray());
        }
    }
}
