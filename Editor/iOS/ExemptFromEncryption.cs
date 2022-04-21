#if UNITY_IOS
using System;
using System.IO;

using UnityEditor;
using UnityEditor.Callbacks;

using UnityEditor.iOS.Xcode;

namespace TalusCI.Editor.iOS
{
    public class ExemptFromEncryption : UnityEditor.Editor
    {
        private readonly static string EncryptionKey = "ITSAppUsesNonExemptEncryption";
        private readonly static string EncryptionValue = "false";

        [PostProcessBuild(9999)]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string pathToBuild)
        {
            if (buildTarget != BuildTarget.iOS && buildTarget != BuildTarget.tvOS) { return; }

            string plistPath = Path.Combine(pathToBuild, "Info.plist");
            Console.WriteLine($"[TalusBuild] Info.plist path: '{plistPath}'");

            var plist = new PlistDocument();
            plist.ReadFromFile(plistPath);

            PlistElementDict root = plist.root;
            root.SetString(EncryptionKey, EncryptionValue);

            File.WriteAllText(plistPath, plist.WriteToString());
        }
    }
}
#endif