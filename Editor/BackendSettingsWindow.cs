using System;
using System.Collections.Generic;

using TalusBackendData;

using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

using UnityEngine;
using UnityEngine.Networking.Types;

namespace TalusCI.Editor
{
    [System.Serializable]
    public class BackendPackage
    {
        public string PackageUrl;
        public bool Status;

        public BackendPackage(string url, bool status)
        {
            PackageUrl = url;
            Status = status;
        }
    }

    public class BackendSettingsWindow : EditorWindow
    {
        private const string TALUS_BACKEND_KEYWORD = "ENABLE_BACKEND";
        private const string ELEPHANT_SCENE_PATH = "Assets/Scenes/Template_Persistent/elephant_scene.unity";

        private static Dictionary<string, BackendPackage> BACKEND_PACKAGES = new Dictionary<string, BackendPackage>
        {
            { "com.talus.talusplayservicesresolver", new BackendPackage("https://github.com/TalusStudio/TalusPlayServicesResolver-Package.git", false) },
            { "com.talus.talusfacebook", new BackendPackage("https://github.com/TalusStudio/TalusFacebook-Package.git", false) },
            { "com.talus.taluselephant", new BackendPackage("https://github.com/TalusStudio/TalusElephant-Package.git", false) }
        };

        private static AddRequest _AddRequest;
        private static RemoveRequest _RemoveRequest;
        private static ListRequest _ListRequest;

        private static void ListProgress()
        {
            if (_ListRequest.IsCompleted)
            {
                if (_ListRequest.Status == StatusCode.Success)
                {
                    foreach (var package in _ListRequest.Result)
                    {
                        // Only retrieve packages that are currently installed in the
                        // project (and are neither Built-In nor already Embedded)
                        if (package.isDirectDependency && package.source
                            != PackageSource.BuiltIn && package.source
                            != PackageSource.Embedded)
                        {
                            if (BACKEND_PACKAGES.ContainsKey(package.name))
                            {
                                BACKEND_PACKAGES[package.name] = new BackendPackage(BACKEND_PACKAGES[package.name].PackageUrl, true);
                            }
                        }
                    }
                }
                else
                {
                    Debug.Log(_ListRequest.Error.message);
                }

                EditorApplication.update -= ListProgress;
            }
        }

        [MenuItem("TalusKit/Backend/Backend Settings")]
        private static void Init()
        {
            _ListRequest = Client.List();
            EditorApplication.update += ListProgress;

            var window = (BackendSettingsWindow) GetWindow(typeof(BackendSettingsWindow));
            window.titleContent = new GUIContent("Backend Settings");
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.Label("Backend Settings", EditorStyles.boldLabel);

#if ENABLE_BACKEND
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Disable Backend"))
            {
                DisableBackend();
            }
#else
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Enable Backend"))
            {
                EnableBackend();
            }
#endif

            if (_ListRequest == null)
            {
                _ListRequest = Client.List();
                EditorApplication.update += ListProgress;
            }

            if (_ListRequest != null)
            {
                if (_ListRequest.IsCompleted)
                {
                    GUILayout.Space(4);
                    GUILayout.Label("Installed Backend Packages", EditorStyles.boldLabel);

                    foreach (var package in BACKEND_PACKAGES)
                    {
                        GUI.backgroundColor = (package.Value.Status) ? Color.green : Color.red;

                        if (GUILayout.Button(package.Key))
                        {
                            if (package.Value.Status)
                            {
                                _RemoveRequest = Client.Remove(package.Key);
                                Debug.Log(package.Key + " removing...");
                            }
                            else
                            {
                                _AddRequest = Client.Add(package.Value.PackageUrl);
                                Debug.Log(package.Value.PackageUrl + " adding...");
                            }
                        }
                    }
                }
                else
                {
                    GUILayout.Space(4);
                    GUILayout.Label("Fetching...", EditorStyles.boldLabel);
                }
            }

            GUILayout.EndVertical();
        }

        private static void CheckAddProgress()
        {
            if (_AddRequest.IsCompleted)
            {
                if (_AddRequest.Status == StatusCode.Success)
                {
                    Debug.Log("Installed: " + _AddRequest.Result.packageId);
                }
                else if (_AddRequest.Status >= StatusCode.Failure)
                {
                    Debug.Log(_AddRequest.Error.message);
                }

                EditorApplication.update -= CheckAddProgress;
            }
        }

        private static void DisableBackend()
        {
            if (DefineSymbols.Contains(TALUS_BACKEND_KEYWORD))
            {
                DefineSymbols.Remove(TALUS_BACKEND_KEYWORD);
                Debug.Log(TALUS_BACKEND_KEYWORD + " define symbol removing...");
            }

            //
            var editorBuildSettingsScenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
            editorBuildSettingsScenes.Remove(editorBuildSettingsScenes.Find(val => val.path.Contains("elephant")));

            Debug.Log("elephant_scene removed from build settings...");
        }

        private static void EnableBackend()
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
        }
    }
}
