using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Networking;

namespace TalusCI.Editor
{
    public class FetchAppInfo
    {
        private const string API_URL = "http://d2a8-46-196-76-251.ngrok.io/api/appstoreconnect/get-app-list";
        
        public IEnumerator GetAppInfo(Action<AppModel> onFetchComplete)
        {
            using UnityWebRequest www = UnityWebRequest.Get($"{API_URL}/{GetProjectName()}");
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                var appModel = JsonUtility.FromJson<AppModel>(www.downloadHandler.text);

                yield return null;
                    
                onFetchComplete(appModel);
            }
        }

        public string GetProjectName()
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
