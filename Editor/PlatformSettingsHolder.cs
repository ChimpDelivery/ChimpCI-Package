using UnityEditor;
using UnityEngine;

using TalusBackendData.Editor.Interfaces;

namespace TalusCI.Editor
{
    /// <summary>
    ///     Platform Data Providers Holder
    /// </summary>
    [FilePath("ProjectSettings/TalusPlatformProvider.asset", FilePathAttribute.Location.ProjectFolder)]
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
