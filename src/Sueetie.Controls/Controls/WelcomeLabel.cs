using Sueetie.Core;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sueetie.Controls
{
    /// <summary>
    /// Displays Welcome, [Display Name]!
    /// </summary>
    public class WelcomeLabel : Label
    {

        public WelcomeLabel()
            : base()
        {
        }

        protected override void Render(HtmlTextWriter writer)
        {
            SueetieUser user = SueetieContext.Current.User;

            string _href = SueetieUrls.Instance.MasterAccountInfo().Url;
            if (SueetieConfiguration.Get().Core.UseForumProfile)
                _href = SueetieUrls.Instance.MyAccountInfo().Url;

            string _imgUrl = "/images/avatars/noavatarthumbnail.jpg";
            if (user.HasAvatarImage)
                _imgUrl = "/images/avatars/" + user.UserID.ToString() + "t.jpg?d=" + DateTime.Now.ToLongTimeString();

            writer.BeginRender();
            writer.Write("<span class='WelcomeText'>" + string.Format(SueetieLocalizer.GetString("welcome_back"), user.DisplayName, SueetieLocalizer.GetString("welcome_exclamation")) + "</span>");
            writer.Write(string.Format("<span class='AvatarImage'><a href='{0}'><img src='{1}' alt='' /></a></span>", _href, _imgUrl));


            var aspUser = System.Web.Security.Membership.GetUser(false);

            //System.Web.Profile.ProfileManager.

            writer.Write($"<!-- ProviderUserKey = {aspUser?.ProviderUserKey} ; UserName = {aspUser?.UserName} ; DisplayName = {HttpContext.Current.Profile["DisplayName"]} -->");

            writer.EndRender();

        }


    }


}

