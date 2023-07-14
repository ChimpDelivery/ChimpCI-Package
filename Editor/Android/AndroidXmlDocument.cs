using System.Text;
using System.Xml;

namespace ChimpCI.Editor.Android
{
    // Reference Used : https://github.com/Over17/UnityAndroidManifestCallback
    internal class AndroidXmlDocument : XmlDocument
    {
        public readonly string AndroidXmlNamespace = "http://schemas.android.com/apk/res/android";

        private readonly string _Path;

        protected XmlNamespaceManager NsManager;

        public AndroidXmlDocument(string path)
        {
            _Path = path;
            using (var reader = new XmlTextReader(_Path))
            {
                reader.Read();
                Load(reader);
            }
            NsManager = new XmlNamespaceManager(NameTable);
            NsManager.AddNamespace("android", AndroidXmlNamespace);
        }

        public string Save()
        {
            return SaveAs(_Path);
        }

        public string SaveAs(string path)
        {
            using (var writer = new XmlTextWriter(path, new UTF8Encoding(false)))
            {
                writer.Formatting = Formatting.Indented;
                Save(writer);
            }
            return path;
        }
    }
}