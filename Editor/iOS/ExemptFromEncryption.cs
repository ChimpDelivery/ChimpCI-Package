using System;
using System.IO;

using UnityEditor;
using UnityEditor.Callbacks;

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

namespace TalusCI.Editor.iOS
{
    public class ExemptFromEncryption : UnityEditor.Editor
    {
        private readonly static string EncryptionKey = "ITSAppUsesNonExemptEncryption";
        private readonly static string EncryptionValue = "false";

        [PostProcessBuild]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string pathToBuild)
        {
            if (buildTarget != BuildTarget.iOS && buildTarget != BuildTarget.tvOS) { return; }

#if UNITY_IOS
            string plistPath = Path.Combine(pathToBuild, "Info.plist");
            Console.WriteLine("[TalusBuild] Info.plist path: " + plistPath);

            var plist = new PlistDocument();
            plist.ReadFromFile(plistPath);

            PlistElementDict root = plist.root;
            root.SetString(EncryptionKey, EncryptionValue);

            File.WriteAllText(plistPath, plist.WriteToString());
#endif
        }
    }
}