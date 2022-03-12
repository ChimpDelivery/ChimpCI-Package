using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Networking;

namespace TalusCI.Editor
{
    public class FetchAppInfo
    {
        public IEnumerator GetAppInfo(Action<AppModel> onFetchComplete)
        {
            using (UnityWebRequest www = UnityWebRequest.Get($"http://de6c-46-196-76-251.ngrok.io/api/appstoreconnect/get-app-list/{GetProjectName()}"))
            {
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
        }

        private string GetProjectName()
        {
            string[] s = Application.dataPath.Split('/');
            return s[s.Length - 2];
        }
    }
}
