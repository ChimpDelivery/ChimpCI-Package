using System.Collections.Generic;

using UnityEditor;

using TalusBackendData.Editor.Interfaces;

namespace TalusCI.Editor
{
    internal class PlatformSettingsProvider : BaseSettingsProvider<PlatformSettingsHolder>
    {
        public override PlatformSettingsHolder Holder => PlatformSettingsHolder.instance;
        public override string Title => "Talus Studio/2. Build Settings";
        public override string Description => "Platform specific build settings.";

        public PlatformSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        { }

        [SettingsProvider]
        public static SettingsProvider CreateProvider()
        {
            return new PlatformSettingsProvider("Talus Studio/2. Build Settings", SettingsScope.Project);
        }
    }
}