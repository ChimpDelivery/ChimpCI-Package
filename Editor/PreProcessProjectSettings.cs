using UnityEngine;

using UnityEditor;

using TalusBackendData.Editor;
using TalusBackendData.Editor.Models;
using TalusBackendData.Editor.Utility;

namespace TalusCI.Editor
{
    public class PreProcessProjectSettings
    {
        public static System.Action<AppModel> OnSyncComplete = delegate { };

        private string _ApiUrl => (Application.isBatchMode) ? CommandLineParser.GetArgument("-apiUrl") : BackendSettingsHolder.instance.ApiUrl;
        private string _ApiToken = (Application.isBatchMode) ? CommandLineParser.GetArgument("-apiKey") : BackendSettingsHolder.instance.ApiToken;
        private string _AppId = (Application.isBatchMode) ? CommandLineParser.GetArgument("-appId") : BackendSettingsHolder.instance.AppId;

        [MenuItem("TalusBackend/Sync Project Settings", priority = 12000)]
        public static void Sync()
        {
            new PreProcessProjectSettings().SyncSettings();
        }

        private void SyncSettings()
        {
            Debug.Log($"[TalusCI-Package] PreProcessProjectSettings::Sync()");

            BackendApi api = new(_ApiUrl, _ApiToken);
            api.GetAppInfo(_AppId, (app) =>
            {
                UpdateProductSettings(app);

                OnSyncComplete(app);
            });
        }

        private void UpdateProductSettings(AppModel app)
        {
            Debug.Log($"[TalusCI-Package] update product settings...");

            PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);

            if (app != null)
            {
                PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, app.app_bundle);
                PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, app.app_bundle);

                PlayerSettings.productName = app.app_name;

                Debug.Log($"[TalusCI-Package] App Model used by Pre Process: {app}");
            }
            else
            {
                Debug.LogError($"[TalusCI-Package] AppModel data is null! Product Settings couldn't updated...");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
