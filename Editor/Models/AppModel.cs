namespace TalusCI.Editor.Models
{
    [System.Serializable]
    public class AppModel
    {
        public string app_bundle;
        public string app_name;

        public override string ToString()
        {
            return UnityEngine.JsonUtility.ToJson(this, true);
        }
    }
}

