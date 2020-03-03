using System;

namespace GalleryServerPro.Web.Pages
{
	public partial class login : Pages.GalleryPage
	{
		#region Private Fields


		#endregion

		#region Public Properties

		#endregion

		#region Protected Events

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				if (!this.IsAnonymousUser)
					Util.Redirect(Web.PageId.album);

				ConfigureControlsFirstTime();
			}

			ConfigureControlsEveryTime();
		}

		protected void Login1_LoggedIn(object sender, EventArgs e)
		{
			Controller.UserController.UserLoggedOn(Login1.UserName, GalleryId);

			if (GallerySettings.EnableUserAlbum && GallerySettings.RedirectToUserAlbumAfterLogin)
			{
				Util.Redirect(Util.GetUrl(PageId.album, "aid={0}", Controller.UserController.GetUserAlbumId(Login1.UserName, GalleryId)));
			}

			// Reload page.
			if (String.IsNullOrEmpty(Util.GetQueryStringParameterString("ReturnUrl")))
				Util.Redirect(Web.PageId.album);
			else
				Response.Redirect(Util.GetQueryStringParameterString("ReturnUrl"));
			//Response.Redirect(GetRedirectUrlAfterLoginOrLogout());

			// Note: For reasons I don't quite understand we cannot use the following pattern that is used elsewhere. It has 
			// something to do with the fact that we are in a postback. When we try to use it, the page finishes the postback
			// without doing the redirect. That would be OK except we need to clear out any msg parameters in
			// the query string. The easiest way to do that is with a redirect.
			//Response.Redirect(GetRedirectUrlAfterLoginOrLogout(), false);
			//System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
		}

		#endregion


		#region Private Methods

		private void ConfigureControlsEveryTime()
		{
			// Don't need the login link if we are already on the login page.
			this.ShowLogin = false;

			// Hide the search button if anonymous browsing is disabled, since the user can't search if they are not logged in.
			if (!this.AllowAnonymousBrowsing)
				this.ShowSearch = false;
			
			Login1.Focus();
		}

		private void ConfigureControlsFirstTime()
		{
			if (this.Message == Message.UserNameOrPasswordIncorrect)
			{
				string msg = Resources.GalleryServerPro.Login_Failure_Text;
				pnlInvalidLogin.Controls.Add(new System.Web.UI.LiteralControl(msg));
				pnlInvalidLogin.Visible = true;
			}

			Login1.MembershipProvider = Controller.UserController.MembershipGsp.Name;
			Login1.PasswordRecoveryUrl = Util.GetUrl(Web.PageId.recoverpassword);

			// Don't show login at top right. We don't need it, since we are already showing login controls, and it doesn' work
			// right anyway, because the 
			//this.GalleryControl.ShowLogin = false;

			if (GallerySettings.EnableSelfRegistration)
			{
				Login1.CreateUserText = Resources.GalleryServerPro.Login_Create_Account_Text;
				Login1.CreateUserUrl = Util.GetUrl(Web.PageId.createaccount);
			}
		}

		#endregion
	}
}