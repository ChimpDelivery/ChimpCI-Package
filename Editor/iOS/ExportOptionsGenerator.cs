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
               $"        <string>{CIDefinitions.ProvisioningProfileName}</string>",
                "    </dict>",
                "    <key>method</key>",
                "    <string>app-store</string>",
                "    <key>signingCertificate</key>",
               $"    <string>{CIDefinitions.SigningCertificateName}</string>",
                "    <key>signingStyle</key>",
                "    <string>manual</string>",
                "    <key>stripSwiftSymbols</key>",
                "    <true/>",
                "    <key>teamID</key>",
               $"    <string>{CIDefinitions.TeamID}</string>",
                "    <key>uploadSymbols</key>",
                "    <false/>",
                "</dict>",
                "</plist>"
            };

            if (!Directory.Exists(CIDefinitions.ExportOptionsPath))
            {
                Directory.CreateDirectory(CIDefinitions.ExportOptionsPath);
            }

            string exportOptionsPath = Path.Combine(CIDefinitions.ExportOptionsPath, "exportOptions.plist");
            File.WriteAllLines(exportOptionsPath, fileContents.ToArray());

            Debug.Log($"[Unity-CI-Package] exportOptions.plist created at {exportOptionsPath}");
        }
    }
}
