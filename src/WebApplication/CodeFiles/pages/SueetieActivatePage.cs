using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;
using System.Web.Security;
using Sueetie.Controls;

namespace Sueetie.Web
{
    public partial class SueetieActivatePage : SueetieBaseThemedPage
    {

        protected SueetieLocal SueetieLocal1;
        protected Label lblWelcome;
        protected PlaceHolder phActivated;
        protected PlaceHolder phNot;

        public SueetieActivatePage()
            : base("members_activate_account")
        {
            this.SueetieMasterPage = "alternate.master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            MembershipUser user;

            string username = Request.QueryString["uname"];
            string valBinary = Request.QueryString["key"];
            int userid = DataHelper.GetIntFromQueryString("uid", -1);

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(valBinary) && userid > 0)
            {
                user = Membership.GetUser(username);

                SueetieUser sueetieUser = SueetieUsers.GetUser(username);

                if ((user.IsApproved == false) && (valBinary == user.CreationDate.ToBinary().ToString()) && !sueetieUser.IsBanned)
                {
                    user.IsApproved = true;
                    Membership.UpdateUser(user);
                    SueetieUsers.CreateUpdateUserProfileCookie(sueetieUser);
                    SueetieLogs.LogUserEntry(UserLogCategoryType.JoinedCommunity, -1, userid);

                    phActivated.Visible = true;
                    phNot.Visible = false;
                }
                else
                {
                    phActivated.Visible = false;
                    phNot.Visible = true;
                }

            }
            else
            {
                phActivated.Visible = false;
                phNot.Visible = true;
            }

        }
    }
}
