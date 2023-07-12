using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using ChimpBackendData.Editor.Interfaces;

namespace ChimpCI.Editor
{
    [CreateAssetMenu(menuName = "ChimpDelivery/Platform Setting")]
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