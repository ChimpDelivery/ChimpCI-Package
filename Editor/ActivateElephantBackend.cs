using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

namespace TalusCI.Editor
{
    public class ActivateElephantBackend : MonoBehaviour
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

            // remove elephant scene if exists
            var editorBuildSettingsScenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
            editorBuildSettingsScenes.RemoveAt(0);
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

            // add elephant scene to the active scenes.
            var editorBuildSettingsScenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
            var elephantScene = new EditorBuildSettingsScene(ELEPHANT_SCENE_PATH, true);
            editorBuildSettingsScenes.Insert(0, elephantScene);
            EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();

            Debug.Log("elephant_scene added to build settings...");
        }
#endif
    }
}
