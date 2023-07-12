using UnityEditor;
using UnityEngine;

using ChimpBackendData.Editor.Interfaces;

namespace ChimpCI.Editor.BuildSystem
{
    /// <summary>
    ///     BuildSettingsHolder provides information about which build generator will be used.
    /// </summary>
    [FilePath("ProjectSettings/ChimpBuild.asset", FilePathAttribute.Location.ProjectFolder)]
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
    }
}
