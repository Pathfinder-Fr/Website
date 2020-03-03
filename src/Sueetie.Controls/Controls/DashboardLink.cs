
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;

namespace Sueetie.Controls
{
    public class DashboardLink : SueetieBaseControl
    {
        protected override void OnLoad(EventArgs e)
        {
            HyperLink _dashboardLink = new HyperLink();
            _dashboardLink.NavigateUrl = string.Format("http://{0}/forum/default.aspx?g=cp_profile&u={1}", string.Format("{0}{1}", System.Web.HttpContext.Current.Request.Url.Host,
               System.Web.HttpContext.Current.Request.Url.Port == 80 ? "" :
                ":" + System.Web.HttpContext.Current.Request.Url.Port.ToString()), CurrentSueetieUser.ForumUserID);
            _dashboardLink.Text = SueetieLocalizer.GetString("menu_dashboard");
            Controls.Add(_dashboardLink);
        }


    }
}
