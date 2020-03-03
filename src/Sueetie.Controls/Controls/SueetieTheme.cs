
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;
using System.Web.UI.HtmlControls;

namespace Sueetie.Controls
{

    public class SueetieTheme : SueetieBaseControl
    {
        public string StyleSheet { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            string href = "/themes/" + this.CurrentTheme + "/style/" + StyleSheet;
            HtmlLink stylesheet = new HtmlLink();
            stylesheet.Href = href;
            stylesheet.Attributes.Add("rel", "stylesheet");
            stylesheet.Attributes.Add("type", "text/css");
            Controls.Add(stylesheet);
        }


    }
}
