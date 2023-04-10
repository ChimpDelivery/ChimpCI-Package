using System.Collections.Generic;

using UnityEditor;

using TalusBackendData.Editor.Interfaces;

namespace TalusCI.Editor.BuildSystem
{
    internal class BuildSettingsProvider : BaseSettingsProvider<BuildSettingsHolder>
    {
        public override BuildSettingsHolder Holder => BuildSettingsHolder.instance;
        public override string Title => "Talus Studio/3. Build Steps (Do not leave any input fields blank!)";
        public override string Description => "Platform specific build steps.";

        public BuildSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        { }

        [SettingsProvider]
        public static SettingsProvider CreateProvider()
        {
            return new BuildSettingsProvider("Talus Studio/3. Build Steps", SettingsScope.Project);
        }
    }
}
