using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlogEngine.Core;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Globalization;
using System.Collections.Specialized;
using System.IO;
using Sueetie.Core;
using System.Xml.Linq;

namespace Sueetie.Blog
{
    public static class SueetieBlogUtils
    {

        #region Theming

        public static HtmlLink AddBlogEngineThemeCSS()
        {
            HtmlLink stylesheet = new HtmlLink();
            stylesheet.Href = Utils.RelativeWebRoot + "themes/" + BlogSettings.Instance.Theme + "/css.axd?name=style" + BlogSettings.Instance.Version() + ".css";
            stylesheet.Attributes.Add("rel", "stylesheet");
            stylesheet.Attributes.Add("type", "text/css");
            return stylesheet;
        }

        public static void UpdateBlogTheme(string _blogRoot, string theme)
        {
            StringDictionary _settings = LoadSettings(_blogRoot);
            UpdateThemeSetting(_settings, _blogRoot, theme);
        }

        #endregion

        #region XML Blog Settings

        private static string StorageLocation()
        {
            if (String.IsNullOrEmpty(System.Web.Configuration.WebConfigurationManager.AppSettings["StorageLocation"]))
                return @"~/app_data/";
            return System.Web.Configuration.WebConfigurationManager.AppSettings["StorageLocation"];
        }

        private static StringDictionary LoadSettings(string _blogRoot)
        {
            string filename = System.Web.HttpContext.Current.Server.MapPath("/" + _blogRoot + "/app_data/settings.xml");
            StringDictionary dic = new StringDictionary();

            XmlDocument doc = new XmlDocument();
            doc.Load(filename);

            foreach (XmlNode settingsNode in doc.SelectSingleNode("settings").ChildNodes)
            {
                string name = settingsNode.Name;
                string value = settingsNode.InnerText;

                dic.Add(name, value);
            }

            return dic;
        }

        public static void UpdateThemeSetting(StringDictionary settings, string _blogRoot, string _newTheme)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");

            string filename = System.Web.HttpContext.Current.Server.MapPath("/" + _blogRoot + "/app_data/settings.xml");
            XmlWriterSettings writerSettings = new XmlWriterSettings(); ;
            writerSettings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(filename, writerSettings))
            {
                writer.WriteStartElement("settings");
                foreach (string key in settings.Keys)
                {
                    if (key != "theme")
                        writer.WriteElementString(key, settings[key]);
                }
                writer.WriteElementString("theme", _newTheme);
                writer.WriteEndElement();
            }
        }

        #endregion

        #region Create Users

        public static void CreateProfile(SueetieUser sueetieUser, string _blogRoot)
        {

            string filename = System.Web.HttpContext.Current.Server.MapPath("/" + _blogRoot + "/app_data/profiles/" + sueetieUser.UserName + ".xml");
            if (!File.Exists(filename))
            {

                XDocument doc = new XDocument(
                new XElement("profileData",
                    new XElement("DisplayName", sueetieUser.DisplayName),
                    new XElement("FirstName"),
                    new XElement("MiddleName"),
                     new XElement("LastName"),
                     new XElement("CityTown"),
                     new XElement("RegionState"),
                     new XElement("Country"),
                     new XElement("Birthday","0001-01-01"),
                     new XElement("AboutMe"),
                     new XElement("PhotoURL"),
                     new XElement("Company"),
                     new XElement("EmailAddress"),
                     new XElement("PhoneMain"),
                     new XElement("PhoneMobile"),
                     new XElement("PhoneFax"),
                     new XElement("LastName"),
                     new XElement("IsPrivate", "False")
                    )
               );
                doc.Save(filename);

            }
        }

        #endregion
    }
}
