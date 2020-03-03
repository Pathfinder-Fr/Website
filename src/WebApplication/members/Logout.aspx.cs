using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;
using System.Web.Security;

namespace Sueetie.Web
{
    public partial class UserLogout : SueetieBaseThemedPage
    {
        public UserLogout() : base("members_logout")
        {
            this.SueetieMasterPage = "alternate.master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {  
                if (Context.Request.IsAuthenticated)
                {
                    FormsAuthentication.SignOut();
                    Response.Redirect("message.aspx?msgid=3");
                }
            }
        }
    }

}
