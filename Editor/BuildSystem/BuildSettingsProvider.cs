using System.Collections.Generic;

using UnityEditor;

using ChimpBackendData.Editor.Interfaces;

namespace ChimpCI.Editor.BuildSystem
{
    internal class BuildSettingsProvider : BaseSettingsProvider<BuildSettingsHolder>
    {
        public override BuildSettingsHolder Holder => BuildSettingsHolder.instance;
        public override string Title => "Chimp Delivery/3. Build Steps (Do not leave any input fields blank!)";
        public override string Description => "Platform specific build steps.";

        public BuildSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        { }

        [SettingsProvider]
        public static SettingsProvider CreateProvider()
        {
            return new BuildSettingsProvider("Chimp Delivery/3. Build Steps", SettingsScope.Project);
        }
    }
}
