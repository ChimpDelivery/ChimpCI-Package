using System.Collections.Generic;

using UnityEditor;

using TalusBackendData.Editor.Interfaces;

namespace TalusCI.Editor
{
    internal class BuildSettingsProvider : BaseSettingsProvider<BuildSettingsHolder>
    {
        public override BuildSettingsHolder Holder => BuildSettingsHolder.instance;
        public override string Title => "Talus Studio/2. Build Layout (Do not leave any input fields blank!)";
        public override string Description => "Platform specific build configurations.";

        public BuildSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        { }

        [SettingsProvider]
        public static SettingsProvider CreateProvider()
        {
            return new BuildSettingsProvider("Talus Studio/2. Build Layout", SettingsScope.Project);
        }
    }
}
