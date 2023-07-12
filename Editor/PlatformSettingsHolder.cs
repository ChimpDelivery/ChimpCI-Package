using UnityEditor;
using UnityEngine;

using ChimpBackendData.Editor.Interfaces;

namespace ChimpCI.Editor
{
    [FilePath("ProjectSettings/ChimpPlatformProvider.asset", FilePathAttribute.Location.ProjectFolder)]
    public class PlatformSettingsHolder : BaseSettingsHolder<PlatformSettingsHolder>
    {
        [Header("Platforms")]
        [SerializeField]
        private PlatformSetting _iOS;
        public PlatformSetting iOS
        {
            get => _iOS;
            set
            {
                _iOS = value;
                SaveSettings();
            }
        }

        [SerializeField]
        private PlatformSetting _Android;
        public PlatformSetting Android
        {
            get => _Android;
            set
            {
                _Android = value;
                SaveSettings();
            }
        }
    }
}
