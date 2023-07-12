using System.Collections.Generic;

using UnityEditor;

using ChimpBackendData.Editor.Interfaces;

namespace ChimpCI.Editor
{
    internal class PlatformSettingsProvider : BaseSettingsProvider<PlatformSettingsHolder>
    {
        public override PlatformSettingsHolder Holder => PlatformSettingsHolder.instance;
        public override string Title => "Chimp Delivery/2. Build Settings";
        public override string Description => "Platform specific build settings.";

        public PlatformSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        { }

        [SettingsProvider]
        public static SettingsProvider CreateProvider()
        {
            return new PlatformSettingsProvider("Chimp Delivery/2. Build Settings", SettingsScope.Project);
        }
    }
}