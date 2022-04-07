using System;
using System.Collections;

using TalusCI.Editor.Models;

using UnityEngine;
using UnityEngine.Networking;

namespace TalusCI.Editor
{
    public class FetchAppInfo
    {
        public IEnumerator GetAppInfo(Action<AppModel> onFetchComplete)
        {
            string apiKey = CommandLineParser.GetArgument("-apiKey");
            string url = CommandLineParser.GetArgument("-apiUrl");

            string apiUrl = $"{url}/api/appstoreconnect/get-app-list/{GetProjectName()}";
            Debug.Log("apiUrl: " + apiUrl);

            using UnityWebRequest www = UnityWebRequest.Get(apiUrl);
            www.SetRequestHeader("api-key", apiKey);

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                var appModel = JsonUtility.FromJson<AppModel>(www.downloadHandler.text);

                yield return null;

                Debug.Log("App bundle: " + appModel.app_bundle);
                onFetchComplete(appModel);
            }
        }

        private string GetProjectName()
        {
            // jenkins ws => s-WorkSpace_ProjectName_master
            string[] s = Application.dataPath.Split('/');
            string fullWorkspaceName = s[s.Length - 2];
            return Between(fullWorkspaceName, '_');
        }

        private string Between(string str, char delimiter)
        {
            string[] nextStr = str.Split(delimiter);
            return nextStr[1];
        }
    }
}
