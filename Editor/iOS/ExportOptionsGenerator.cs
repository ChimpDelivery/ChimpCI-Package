using System;
using System.IO;
using System.Collections.Generic;

using UnityEditor;
using UnityEditor.Callbacks;

namespace TalusCI.Editor.iOS
{
    public class ExportOptionsGenerator : UnityEditor.Editor
    {
        [PostProcessBuild]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string pathToBuild)
        {
            if (buildTarget != BuildTarget.iOS && buildTarget != BuildTarget.tvOS) { return; }

            Generate();
        }

        private static void Generate()
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
               $"        <key>{PlayerSettings.applicationIdentifier}</key>",
               $"        <string>{iOSAppBuildInfo.ProvisioningProfileName}</string>",
                "    </dict>",
                "    <key>method</key>",
                "    <string>app-store</string>",
                "    <key>signingCertificate</key>",
               $"    <string>{iOSAppBuildInfo.SigningCertificateName}</string>",
                "    <key>signingStyle</key>",
                "    <string>manual</string>",
                "    <key>stripSwiftSymbols</key>",
                "    <true/>",
                "    <key>teamID</key>",
               $"    <string>{iOSAppBuildInfo.TeamID}</string>",
                "    <key>uploadSymbols</key>",
                "    <false/>",
                "</dict>",
                "</plist>"
            };

            Console.WriteLine("[TalusBuild] exportOptions.plist created at " + iOSAppBuildInfo.ExportOptionsPath);
            File.WriteAllLines(iOSAppBuildInfo.ExportOptionsPath, fileContents.ToArray());
        }
    }
}
