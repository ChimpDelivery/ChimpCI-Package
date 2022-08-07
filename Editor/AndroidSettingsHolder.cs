using UnityEditor;

using UnityEngine;

namespace TalusCI.Editor
{
    /// <summary>
    ///     AndroidSettingsHolder provides information about Google Play building & signing.
    /// </summary>
    [FilePath("ProjectSettings/TalusAndroid.asset", FilePathAttribute.Location.ProjectFolder)]
    public class AndroidSettingsHolder : ScriptableSingleton<AndroidSettingsHolder>
    {
        // TalusCI.asset path
        public string Path => GetFilePath();

        // Unity3D - CI Layout Panel Path
        private const string _ProviderPath = "Talus Studio/Android Layout";
        public static string ProviderPath => _ProviderPath;

        // Unity3D project absolute path.
        private static readonly string _ProjectFolder = System.IO.Directory.GetCurrentDirectory();
        public static string ProjectFolder => _ProjectFolder;

        // Android default build path, sync this path with Jenkinsfile
        [SerializeField]
        private string _BuildFolder = "Builds/Android/";
        public string BuildFolder
        {
            get => _BuildFolder;
            set
            {
                _BuildFolder = value;
                SaveSettings();
            }
        }

        // Android default build path, sync this path with Jenkinsfile
        [SerializeField]
        private string _BuildFileName = "Build.aab";
        public string BuildFileName
        {
            get => _BuildFileName;
            set
            {
                _BuildFileName = value;
                SaveSettings();
            }
        }

        public void SaveSettings() => Save(true);
    }
}