using System;
using System.Web.Security;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.Web.Controller;
using System.Globalization;
using GalleryServerPro.ErrorHandler.CustomExceptions;

namespace GalleryServerPro.Web.Pages
{
	public partial class myaccount : Pages.GalleryPage
	{
		#region Private Fields

		private IUserAccount _user;
		private IUserProfile _currentProfile;
		private string _messageText;
		private string _messageCssClass;
		private bool _messageIsError;

		#endregion

		#region Public Properties

		public IUserAccount CurrentUser
		{
			get
			{
				if (this._user == null)
					_user = UserController.GetUser();

				return this._user;
			}
		}

		protected IUserProfile CurrentProfile
		{
			get
			{
				if (this._currentProfile == null)
				{
					this._currentProfile = ProfileController.GetProfile().Copy();
				}

				return this._currentProfile;
			}
		}

		public IUserGalleryProfile CurrentGalleryProfile
		{
			get
			{
				return CurrentProfile.GetGalleryProfile(GalleryId);
			}
		}

		/// <summary>
		/// Gets or sets the text to display to the user.
		/// </summary>
		/// <value>The text to display to the user.</value>
		private string MessageText
		{
			get { return _messageText; }
			set { _messageText = value; }
		}

		/// <summary>
		/// Gets or sets the CSS class to use to format the text to display to the user.
		/// </summary>
		/// <value>The CSS class to use to format the text to display to the user.</value>
		private string MessageCssClass
		{
			get { return _messageCssClass; }
			set { _messageCssClass = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the message is an error.
		/// </summary>
		/// <value><c>true</c> if the message is an error; otherwise, <c>false</c>.</value>
		private bool MessageIsError
		{
			get { return _messageIsError; }
			set { _messageIsError = value; }
		}

		#endregion

		#region Protected Events

		protected void Page_Load(object sender, EventArgs e)
		{
			if (this.IsAnonymousUser)
				Util.Redirect(Web.PageId.album);

			if (!GallerySettings.AllowManageOwnAccount)
				Util.Redirect(Web.PageId.album);

			if (!IsPostBack)
				ConfigureControlsFirstTime();

			RegisterJavascript();
		}

		private void RegisterJavascript()
		{
			// Register some startup script that will make the user album warning invisible.
			string script = String.Format(System.Globalization.CultureInfo.InvariantCulture, @"
var msgbox = $get('{0}');
if (msgbox != null)
	msgbox.style.display = 'none';",
	wwUserAlbumWarning.ClientID);

			System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "myAccountStartupScript", script, true);

			// Register some script to handle the click event of the user album checkbox.
			script = String.Format(System.Globalization.CultureInfo.InvariantCulture, @"
function toggleWarning(chk)
	{{
		if (chk.checked)
			$get('{0}').style.display = 'none';
		else
			$get('{0}').style.display = '';
	}}
",
 wwUserAlbumWarning.ClientID);

			System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myAccountScript", script, true);

		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			SaveSettings();
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			RedirectToPreviousPage();
		}

		protected void lbDeleteAccount_Click(object sender, EventArgs e)
		{
			ProcessAccountDeletion();
		}

		#endregion

		#region Private Methods

		private void ConfigureControlsFirstTime()
		{
			hlChangePwd.NavigateUrl = Web.Util.GetUrl(Web.PageId.changepassword);

			lbDeleteAccount.OnClientClick = String.Format(CultureInfo.CurrentCulture, "return confirm('{0}')", Resources.GalleryServerPro.MyAccount_Delete_Account_Confirmation);

			if (GallerySettings.EnableUserAlbum)
			{
				litDeleteAccountWarning.Text = Resources.GalleryServerPro.MyAccount_Delete_Account_With_User_Albums_Warning;
			}
			else
			{
				litDeleteAccountWarning.Text = Resources.GalleryServerPro.MyAccount_Delete_Account_Warning;
				pnlUserAlbum.Visible = false;
			}

			if (GallerySettings.AllowDeleteOwnAccount)
			{
				pnlDeleteAccount.Visible = true;
			}

			CheckForMessages();

			this.wwDataBinder.DataBind();

			UpdateUI();
		}

		private void SaveSettings()
		{
			this.wwDataBinder.Unbind(this);

			if (wwDataBinder.BindingErrors.Count > 0)
			{
				MessageText = wwDataBinder.BindingErrors.ToHtml();
				MessageIsError = true;
				return;
			}

			UserController.SaveUser(this.CurrentUser);

			bool originalEnableUserAlbumSetting = ProfileController.GetProfileForGallery(GalleryId).EnableUserAlbum;

			SaveProfile(this.CurrentProfile);

			SaveSettingsCompleted(originalEnableUserAlbumSetting);
		}

		private void SaveSettingsCompleted(bool originalEnableUserAlbumSetting)
		{
			MessageText = Resources.GalleryServerPro.MyAccount_Save_Success_Text;

			bool newEnableUserAlbumSetting = ProfileController.GetProfileForGallery(GalleryId).EnableUserAlbum;
			
			if (originalEnableUserAlbumSetting != newEnableUserAlbumSetting)
			{
				// Since we changed a setting that affect how and which controls are rendered to the page, let us redirect to the current page and
				// show the save success message. If we simply show a message without redirecting, two things happen: (1) the user doesn't see the effect
				// of their change until the next page load, (2) there is the potential for a viewstate validation error.
				const Message msg = Message.SettingsSuccessfullyChanged;

				Util.Redirect(PageId.myaccount, "msg={0}", ((int)msg).ToString(CultureInfo.InvariantCulture));
			}
			else
			{
				UpdateUI();
			}
		}

		private void SaveProfile(IUserProfile userProfile)
		{
			// Get reference to user's album. We need to do this *before* saving the profile, because if the user disabled their user album,
			// this method will return null after saving the profile.
			IAlbum album = UserController.GetUserAlbum(GalleryId);

			IUserGalleryProfile profile = userProfile.GetGalleryProfile(GalleryId);

			if (!profile.EnableUserAlbum)
			{
				profile.UserAlbumId = 0;
			}

			ProfileController.SaveProfile(userProfile);

			if (!profile.EnableUserAlbum)
			{
				AlbumController.DeleteAlbum(album);
			}
		}

		private void ProcessAccountDeletion()
		{
			try
			{
				UserController.DeleteGalleryServerProUser(this.CurrentUser.UserName, false);
			}
			catch (WebException ex)
			{
				int errorId = LogError(ex);
				MessageIsError = true;
				MessageText = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.MyAccount_Delete_Account_Err_Msg, errorId, ex.GetType());

				UpdateUI();

				return;
			}
			catch (GallerySecurityException ex)
			{
				int errorId = LogError(ex);
				MessageIsError = true;
				MessageText = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.MyAccount_Delete_Account_Err_Msg, errorId, ex.GetType());

				UpdateUI();

				return;
			}

			FormsAuthentication.SignOut();

			UserController.UserLoggedOff();

			RedirectToAlbumViewPage();
		}

		private void CheckForMessages()
		{
			if (Message == Message.SettingsSuccessfullyChanged)
			{
				MessageText = Resources.GalleryServerPro.Admin_Save_Success_Text;
			}
		}

		private void UpdateUI()
		{
			if (!String.IsNullOrEmpty(MessageText))
			{
				if (MessageIsError)
				{
					wwMessage.CssClass = "wwErrorFailure gsp_msgwarning";
					wwMessage.Text = this.MessageText;
				}
				else
				{
					wwMessage.CssClass = "wwErrorSuccess gsp_msgfriendly gsp_bold";
					wwMessage.ShowMessage(this.MessageText);
				}

				// If a CSS class has been specified, use that instead of the default ones above.
				if (!String.IsNullOrEmpty(MessageCssClass))
				{
					wwMessage.CssClass = MessageCssClass;
				}
			}
		}

		#endregion
	}
}