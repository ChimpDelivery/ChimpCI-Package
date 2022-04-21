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

            string plistPath = pathToBuild + "/Info.plist";

            var plist = new PlistDocument();
            plist.ReadFromFile(plistPath);

            PlistElementDict root = plist.root;
            if (root.values.ContainsKey(EncryptionKey))
            {
                root.SetString(EncryptionKey, EncryptionValue);

                Console.WriteLine($"[TalusBuild] setting {EncryptionKey} in {plistPath} => NEW VALUE: {EncryptionValue}");
            }
            else
            {
                Console.WriteLine($"[TalusBuild] key: {EncryptionKey} can not found in {plistPath}!");
            }

            File.WriteAllText(plistPath, plist.WriteToString());
        }
    }
}
#endif