using System;
using System.Collections.Generic;
using System.Linq;

using Unity.EditorCoroutines.Editor;

using UnityEditor;
using UnityEditor.Build.Reporting;

namespace TalusCI.Editor
{
    public class BuildActions
    {
        private static FetchAppInfo _FetchedAppInfo = new FetchAppInfo();
        private static JenkinsAppInfo _JenkinsAppInfo = new JenkinsAppInfo();

        public static void IOSDevelopment()
        {
            EditorCoroutineUtility.StartCoroutineOwnerless(_FetchedAppInfo.GetAppInfo(app => {
                EditorUserBuildSettings.development = true;
                CreateBuild(app);
            }));
        }

        public static void IOSRelease()
        {
            EditorCoroutineUtility.StartCoroutineOwnerless(_FetchedAppInfo.GetAppInfo(app => {
                CreateBuild(app);
            }));
        }

        private static string[] GetScenes()
        {
            return (from t in EditorBuildSettings.scenes where t.enabled select t.path).ToArray();
        }

        private static void CreateBuild(AppModel app)
        {
            //
            GenerateExportOptions(app);

            // Populate app data with fetched model.
            if (PlayerSettings.SplashScreen.showUnityLogo) { PlayerSettings.SplashScreen.showUnityLogo = false; }
            PlayerSettings.applicationIdentifier = app.app_bundle;
            PlayerSettings.productName = app.app_name;

            PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
            
            // Save settings.
            Console.WriteLine("[TalusBuild] Assets Saved!");

            BuildPipeline.BuildPlayer(GetScenes(), _JenkinsAppInfo.IOSFolder, BuildTarget.iOS, BuildOptions.CompressWithLz4HC);
        }

        private static void GenerateExportOptions(AppModel appModel)
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
               $"        <key>{appModel.app_bundle}</key>",
               $"        <string>{_JenkinsAppInfo.ProvisioningProfileName}</string>",
                "    </dict>",
                "    <key>method</key>",
                "    <string>app-store</string>",
                "    <key>signingCertificate</key>",
               $"    <string>{_JenkinsAppInfo.SigningCertificateName}</string>",
                "    <key>signingStyle</key>",
                "    <string>manual</string>",
                "    <key>stripSwiftSymbols</key>",
                "    <true/>",
                "    <key>teamID</key>",
               $"    <string>{_JenkinsAppInfo.TeamID}</string>",
                "    <key>uploadSymbols</key>",
                "    <false/>",
                "</dict>",
                "</plist>"
            };

            Console.WriteLine("[TalusBuild] exportOptions.plist created at " + _JenkinsAppInfo.ExportOptionsPath);
            System.IO.File.WriteAllLines(_JenkinsAppInfo.ExportOptionsPath, fileContents.ToArray());
        }
    }
}
