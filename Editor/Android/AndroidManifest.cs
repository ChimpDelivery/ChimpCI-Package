using System.Xml;

namespace TalusCI.Android
{
    internal class AndroidManifest : AndroidXmlDocument
    {
        private readonly XmlElement _ApplicationElement;

        public AndroidManifest(string path) : base(path)
        {
            _ApplicationElement = SelectSingleNode("/manifest/application") as XmlElement;
        }

        private XmlAttribute CreateAndroidAttribute(string key, string value)
        {
            XmlAttribute attr = CreateAttribute("android", key, AndroidXmlNamespace);
            attr.Value = value;
            return attr;
        }

        internal XmlNode GetActivityWithLaunchIntent()
        {
            return SelectSingleNode("/manifest/application/activity[intent-filter/action/@android:name='android.intent.action.MAIN' and " +
                    "intent-filter/category/@android:name='android.intent.category.LAUNCHER']", NsManager);
        }

        internal void SetApplicationTheme(string appTheme)
        {
            _ApplicationElement.Attributes.Append(CreateAndroidAttribute("theme", appTheme));
        }

        internal void SetStartingActivityName(string activityName)
        {
            GetActivityWithLaunchIntent().Attributes.Append(CreateAndroidAttribute("name", activityName));
        }

        internal void SetApplicationAttribute(string key, string value)
        {
            _ApplicationElement.Attributes.Append(CreateAndroidAttribute(key, value));
        }

        internal void SetStartingActivityAttribute(string key, string value)
        {
            XmlNode node = GetActivityWithLaunchIntent();
            if (node != null)
            {
                XmlAttributeCollection attributes = node.Attributes;
                attributes.Append(CreateAndroidAttribute(key, value));
            }
        }
    }
}