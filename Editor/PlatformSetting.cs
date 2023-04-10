using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using TalusBackendData.Editor.Interfaces;

namespace TalusCI.Editor
{
    [CreateAssetMenu]
    public class PlatformSetting : ScriptableObject
    {
        public List<BaseProvider> Providers;

        public void ApplySettings()
        {
            Providers.ForEach(provider => provider.Provide());
        }

        public bool IsApplied => Providers.Count(provider => provider.IsCompleted) == Providers.Count;
    }
}