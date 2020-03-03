
using Sueetie.Core;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace Sueetie.Controls
{
    public class InboxLink : SueetieBaseControl
    {
        protected override void OnLoad(EventArgs e)
        {
            int unreadCount = SueetieDataProvider.Provider.GetUnreadPMs(SueetieContext.Current.User.UserName).Count();

            HyperLink _inboxLink = new HyperLink();
            _inboxLink.NavigateUrl = "/forum/default.aspx?g=cp_pm";
            _inboxLink.Text = SueetieLocalizer.GetString("link_conversations");
            if (unreadCount > 0)
                _inboxLink.Text = SueetieLocalizer.GetString("link_conversations") + string.Format(" ({0})", unreadCount);
            Controls.Add(_inboxLink);

        }


    }
}
