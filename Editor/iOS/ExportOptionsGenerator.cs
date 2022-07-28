using System.IO;
using System.Collections.Generic;

using UnityEditor;
using UnityEditor.Callbacks;

using Debug = UnityEngine.Debug;

namespace TalusCI.Editor.iOS
{
    internal class ExportOptionsGenerator
    {
        [PostProcessBuild]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string pathToBuild)
        {
            if (buildTarget != BuildTarget.iOS && buildTarget != BuildTarget.tvOS) { return; }

            Generate();
        }

        private static void Generate()
        {
            var settingsHolder = iOSSettingsHolder.instance;

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
               $"        <key>{PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.iOS)}</key>",
               $"        <string>{settingsHolder.ProvisioningProfileName}</string>",
                "    </dict>",
                "    <key>method</key>",
                "    <string>app-store</string>",
                "    <key>signingCertificate</key>",
               $"    <string>{settingsHolder.SigningCertificateName}</string>",
                "    <key>signingStyle</key>",
                "    <string>manual</string>",
                "    <key>stripSwiftSymbols</key>",
                "    <true/>",
                "    <key>teamID</key>",
               $"    <string>{settingsHolder.TeamID}</string>",
                "    <key>uploadSymbols</key>",
                "    <false/>",
                "</dict>",
                "</plist>"
            };

            var exportOptionsRootPath = Path.Combine(iOSSettingsHolder.ProjectFolder, settingsHolder.ExportOptionsPath);
            Debug.Log($"[TalusCI-Package] Export Options Root Path: {exportOptionsRootPath}");

            if (!Directory.Exists(exportOptionsRootPath))
            {
                Directory.CreateDirectory(exportOptionsRootPath);
            }

            string exportOptionsPath = Path.Combine(exportOptionsRootPath, "exportOptions.plist");
            File.WriteAllLines(exportOptionsPath, fileContents.ToArray());

            Debug.Log($"[TalusCI-Package] exportOptions.plist created at {exportOptionsRootPath}");
        }
    }
}
