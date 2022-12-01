using System;

using UnityEditor;
using UnityEngine;

using TalusBackendData.Editor.Interfaces;

using TalusCI.Editor.BuildSystem;

namespace TalusCI.Editor
{
    /// <summary>
    ///     BuildSettingsHolder provides information about which build generator will be used.
    /// </summary>
    [FilePath("ProjectSettings/TalusBuild.asset", FilePathAttribute.Location.ProjectFolder)]
    public class BuildSettingsHolder : BaseSettingsHolder<BuildSettingsHolder>
    {
#if TALUS_ADDRESSABLES
        [Header("Build Generators - Release (addressables support)")]
        [SerializeField] private BuildGenerator _IOSReleaseAddressable;
        public BuildGenerator IOSReleaseAddressable
        {
            get => _IOSReleaseAddressable;
            set
            {
                _IOSReleaseAddressable = value;
                SaveSettings();
            }
        }

        [SerializeField] private BuildGenerator _AndroidReleaseAABAddressable;
        public BuildGenerator AndroidReleaseAABAddressable
        {
            get => _AndroidReleaseAABAddressable;
            set
            {
                _AndroidReleaseAABAddressable = value;
                SaveSettings();
            }
        }

        [SerializeField] private BuildGenerator _AndroidReleaseAPKAddressable;
        public BuildGenerator AndroidReleaseAPKAddressable
        {
            get => _AndroidReleaseAPKAddressable;
            set
            {
                _AndroidReleaseAPKAddressable = value;
                SaveSettings();
            }
        }
#endif

        [Header("Build Generators - Release (no addressables)")]
        [SerializeField] private BuildGenerator _IOSRelease;
        public BuildGenerator IOSRelease
        {
            get => _IOSRelease;
            set
            {
                _IOSRelease = value;
                SaveSettings();
            }
        }

        [SerializeField] private BuildGenerator _AndroidReleaseAAB;
        public BuildGenerator AndroidReleaseAAB
        {
            get => _AndroidReleaseAAB;
            set
            {
                _AndroidReleaseAAB = value;
                SaveSettings();
            }
        }

        [SerializeField] private BuildGenerator _AndroidReleaseAPK;
        public BuildGenerator AndroidReleaseAPK
        {
            get => _AndroidReleaseAPK;
            set
            {
                _AndroidReleaseAPK = value;
                SaveSettings();
            }
        }

        [Header("iOS")]
        [SerializeField] private string _UserTrackingText = "Your data will be used for analytical purposes.";
        public string UserTrackingText
        {
            get => _UserTrackingText;
            set
            {
                _UserTrackingText = value;
                SaveSettings();
            }
        }
        
        [Header("Android")]
        [SerializeField] private string _ManifestFilePath = "Assets/Plugins/Android/AndroidManifest.xml";
        public string ManifestFilePath
        {
            get => _ManifestFilePath;
            set
            {
                _ManifestFilePath = value;
                SaveSettings();
            }
        }
    }
}
