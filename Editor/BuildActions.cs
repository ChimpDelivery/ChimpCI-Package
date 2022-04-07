using System.Linq;

using UnityEditor;
using Unity.EditorCoroutines.Editor;

using TalusCI.Editor.Models;

#if ENABLE_BACKEND
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity.Settings;
#endif

namespace TalusCI.Editor
{
    public static class BuildActions
    {
        private static readonly FetchAppInfo _FetchedAppInfo = new FetchAppInfo();

        public static void IOSDevelopment()
        {
            EditorUserBuildSettings.development = true;
            EditorCoroutineUtility.StartCoroutineOwnerless(_FetchedAppInfo.GetAppInfo(CreateBuild));
        }

        public static void IOSRelease()
        {
            EditorCoroutineUtility.StartCoroutineOwnerless(_FetchedAppInfo.GetAppInfo(CreateBuild));
        }

        private static void CreateBuild(AppModel app)
        {
            // splash screen
            if (PlayerSettings.SplashScreen.showUnityLogo)
            {
                PlayerSettings.SplashScreen.showUnityLogo = false;
            }

            // app name & bundle id
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, app.app_bundle);
            PlayerSettings.productName = app.app_name;
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);

            // facebook settings
            #if ENABLE_BACKEND
            FacebookSettings.SelectedAppIndex = 0;
            FacebookSettings.AppIds = new List<string> { app.fb_id };
            FacebookSettings.AppLabels = new List<string> { app.app_name };
            #endif

            // elephant settings
            // #if ENABLE_BACKEND
            // ElephantSettings elephantSettings = Resources.Load<ElephantSettings>("ElephantSettings");
            // #endif

            AssetDatabase.SaveAssets();

            BuildPipeline.BuildPlayer(GetScenes(), iOSAppBuildInfo.IOSFolder, BuildTarget.iOS, BuildOptions.CompressWithLz4HC);
            EditorApplication.Exit(0);
        }

        private static string[] GetScenes()
        {
            return (from t in EditorBuildSettings.scenes where t.enabled select t.path).ToArray();
        }
    }
}
