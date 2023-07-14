using UnityEditor;

using ChimpBackendData.Editor.Interfaces;

namespace ChimpCI.Editor
{
    internal class PlatformSettingsProvider : BaseSettingsProvider<PlatformSettingsHolder>
    {
        public override string Description => "Platform specific build settings.";

        public PlatformSettingsProvider(string path) : base(path)
        { }

        [SettingsProvider]
        public static SettingsProvider CreateProvider()
        {
            return new PlatformSettingsProvider("Chimp Delivery/2. Build Settings");
        }
    }
}