using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sueetie.Core;
using Saltie.Core;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace Saltie.Controls
{
    public class UserContentCountLabel : Label
    {

        public string CSSClass
        {
            get { return ((string)ViewState["CSSClass"]) ?? string.Empty; }
            set { ViewState["CSSClass"] = value; }
        }

        public UserContentCountLabel()
            : base()
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            SueetieUser user = SueetieContext.Current.User;

            Label _userContentCountLabel = new Label();
            _userContentCountLabel.Text = user.DisplayName + " has created " + SaltieUsers.GetContentCount(user.UserID) + " content items!";
            _userContentCountLabel.CssClass = this.CSSClass;
            Controls.Add(_userContentCountLabel);
        }

    }
}