using System;
using System.Collections.Generic;

using Facebook.Unity.Settings;

using TalusBackendData.Editor;
using TalusBackendData.Editor.Models;

using Unity.EditorCoroutines.Editor;

using UnityEditor;

using UnityEngine;

namespace TalusCI.Editor
{
    public class ActivateElephantBackend : EditorWindow
    {
        private const string TALUS_BACKEND_KEYWORD = "ENABLE_BACKEND";
        private const string ELEPHANT_SCENE_PATH = "Assets/Scenes/Template_Persistent/elephant_scene.unity";

#if ENABLE_BACKEND
        [MenuItem("TalusKit/Backend/Disable Backend")]
        static void DisableBackend()
        {
            if (DefineSymbols.Contains(TALUS_BACKEND_KEYWORD))
            {
                DefineSymbols.Remove(TALUS_BACKEND_KEYWORD);
                Debug.Log(TALUS_BACKEND_KEYWORD + " define symbol removing...");
            }

            //
            var editorBuildSettingsScenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
            editorBuildSettingsScenes.Remove(editorBuildSettingsScenes.Find(val => val.path.Contains("elephant")));
            EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();

            Debug.Log("elephant_scene removed from build settings...");
        }
#else
        [MenuItem("TalusKit/Backend/Enable Backend")]
        static void EnableBackend()
        {
            if (!DefineSymbols.Contains(TALUS_BACKEND_KEYWORD))
            {
                DefineSymbols.Add(TALUS_BACKEND_KEYWORD);
                Debug.Log(TALUS_BACKEND_KEYWORD + " define symbol adding...");
            }

            //
            var editorBuildSettingsScenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
            if (editorBuildSettingsScenes.Count > 0 && !editorBuildSettingsScenes[0].path.Contains("elephant"))
            {
                var elephantScene = new EditorBuildSettingsScene(ELEPHANT_SCENE_PATH, true);
                editorBuildSettingsScenes.Insert(0, elephantScene);
                EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();

                Debug.Log("elephant_scene added to build settings...");
            }

            //
            EditorCoroutineUtility.StartCoroutineOwnerless(FetchAppInfo.GetAppInfo(UpdateKeys));
        }

        private static void UpdateKeys(AppModel app)
        {
            // update facebook settings
            FacebookSettings.SelectedAppIndex = 0;
            if (app.fb_id != null)
            {
                FacebookSettings.AppIds = new List<string> { app.fb_id };
            }
            FacebookSettings.AppLabels = new List<string> { app.app_name };

            // update elephant settings
            // ElephantSettings elephantSettings = Resources.Load<ElephantSettings>("ElephantSettings");
        }
#endif
    }
}
