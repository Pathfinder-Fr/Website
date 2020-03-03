using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Sueetie.Core;

namespace Sueetie.Controls
{
    /// <summary>
    /// Shows content based on UserRole
    /// </summary>
    public class SueetieLogo : SueetieBaseControl
    {

        protected override void OnLoad(EventArgs e)
        {
            ImageButton _logoButton = new ImageButton();
            _logoButton.PostBackUrl = "http://sueetie.com";
            _logoButton.ImageUrl = LogoFont == LogoFontColor.Black ? "/images/shared/sueetie/footertagblack.png" : "/images/shared/sueetie/footertagwhite.png";
            _logoButton.AlternateText = "This Site Built with the Sueetie Open Source Community Framework.";
            _logoButton.CssClass = "SueetieLogoImage";
            Controls.Add(_logoButton);

            Label _tagText = new Label();
            _tagText.CssClass = "SueetieLogoText";
            _tagText.Text = "<p>This site was built with the Sueetie Open Source Community Framework. Learn " +
                        "more at <a href='http://sueetie.com'>Sueetie.com.</a> Sueetie is distributed under the <a href='http://creativecommons.org/licenses/GPL/2.0/'>GNU GPL v2 license</a></p>";

            if (ShowText == DisplayTagText.True)
                Controls.Add(_tagText);
        }


        public DisplayTagText ShowText
        {
            get { return (DisplayTagText)(ViewState["ShowText"] ?? DisplayTagText.True); }
            set { ViewState["ShowText"] = value; }
        }

        public LogoFontColor LogoFont
        {
            get { return (LogoFontColor)(ViewState["LogoColor"] ?? LogoFontColor.Black); }
            set { ViewState["LogoColor"] = value; }
        }

        public enum DisplayTagText
        {
            True,
            False
        }
        public enum LogoFontColor
        {
            White,
            Black
        }

    }


}

