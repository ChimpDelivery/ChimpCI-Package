using UnityEditor;

using ChimpBackendData.Editor.Interfaces;

namespace ChimpCI.Editor.BuildSystem
{
    internal class BuildSettingsProvider : BaseSettingsProvider<BuildSettingsHolder>
    {
        public override string Description => "Platform specific build steps.";

        public BuildSettingsProvider(string path) : base(path)
        { }

        [SettingsProvider]
        public static SettingsProvider CreateProvider()
        {
            return new BuildSettingsProvider("Chimp Delivery/3. Build Steps");
        }
    }
}
