using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Sueetie.Web
{
    public partial class SiteMap : System.Web.UI.Page
    {

        private void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                // set the content type

                Page.Response.ContentType = "text/xml";

                // create a SiteMap

                SiteMapFeedGenerator gen = new SiteMapFeedGenerator(Page.Response.Output);

                gen.WriteStartDocument();


                gen.WriteItem("http://sueetie.com/", DateTime.Now, "daily", "1.0");
                gen.WriteItem("http://sueetie.com/wiki/default.aspx", DateTime.Now, "daily", "1.0");
                gen.WriteItem("http://sueetie.com/blog/default.aspx", DateTime.Now, "daily", "1.0");
                gen.WriteItem("http://sueetie.com/forum/default.aspx", DateTime.Now, "daily", "1.0");
                gen.WriteItem("http://sueetie.com/media/default.aspx", DateTime.Now, "daily", "0.6");

                gen.WriteEndDocument();
                gen.Close();
            }
            catch (Exception ex)
            {

            }
        }


        ///
        /// Some parts of this code are form unknown sources,
        /// This code is not 100% from me. I modified it from older
        /// projects of mine. Please keep this in mind when using this
        /// code. 
        ///
        /// Alexandre Brisebois
        ///

        public class SiteMapFeedGenerator
        {
            XmlTextWriter writer;
            public SiteMapFeedGenerator(System.IO.Stream stream, System.Text.Encoding encoding)
            {
                writer = new XmlTextWriter(stream, encoding);
                writer.Formatting = Formatting.Indented;
            }
            public SiteMapFeedGenerator(System.IO.TextWriter w)
            {
                writer = new XmlTextWriter(w);
                writer.Formatting = Formatting.Indented;
            }
            /// <summary>
            /// Writes the beginning of the SiteMap document
            /// </summary>
            public void WriteStartDocument()
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("urlset");
                writer.WriteAttributeString("xmlns", "http://www.google.com/schemas/sitemap/0.84");
            }
            /// <summary>
            /// Writes the end of the SiteMap document
            /// </summary>
            public void WriteEndDocument()
            {
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            /// <summary>
            /// Closes this stream and the underlying stream
            /// </summary>
            public void Close()
            {
                writer.Flush();
                writer.Close();
            }

            public void WriteItem(string link, DateTime publishedDate, string changeFreq, string priority)
            {
                writer.WriteStartElement("url");
                writer.WriteElementString("loc", link);
                writer.WriteElementString("lastmod", formatDate(publishedDate));
                writer.WriteElementString("changefreq", changeFreq);
                if (priority == null)
                {
                    writer.WriteElementString("priority", "0.8");
                }
                else
                {
                    writer.WriteElementString("priority", priority);
                }
                writer.WriteEndElement();
            }

            public string formatDate(DateTime d)
            {
                return d.ToString("s") + "+00:00";
            }
        }

        //Calculate Percentage rating Method
        private string percent(int current, int total)
        {
            if (current > total - 5)
            {
                return "1.0";
            }
            else if (current > total - 20)
            {
                return "0.9";
            }
            else if (current > total - 50)
            {
                return "0.8";
            }
            else if (current > total - 100)
            {
                return "0.7";
            }
            else if (current > total - 200)
            {
                return "0.6";
            }
            else if (current > total - 300)
            {
                return "0.5";
            }
            else if (current > total - 400)
            {
                return "0.4";
            }
            else if (current > total - 500)
            {
                return "0.3";
            }
            else if (current > total - 1500)
            {
                return "0.2";
            }
            else
            {
                return "0.1";
            }
        }


    }
}
