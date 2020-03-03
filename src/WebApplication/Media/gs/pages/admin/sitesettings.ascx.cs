using System;
using System.Globalization;
using System.Web.UI.WebControls;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.WebControls;

namespace GalleryServerPro.Web.Pages.Admin
{
	public partial class sitesettings : Pages.AdminPage
	{
		#region Private Fields

		private AppSettingsEntity _appSettingUpdateable;
		private ILicense _license;
		private string _messageText;
		private string _messageCssClass;
		private bool _messageIsError;

		#endregion

		#region Properties

		public ILicense License
		{
			get
			{
				if (_license == null)
				{
					_license = AppSetting.Instance.License;
				}

				return _license;
			}
			set { _license = value; }
		}

		public AppSettingsEntity AppSettingsUpdateable
		{
			get
			{
				if (_appSettingUpdateable == null)
				{
					_appSettingUpdateable = new AppSettingsEntity();
					_appSettingUpdateable.EnableCache = AppSetting.Instance.EnableCache;
					_appSettingUpdateable.AllowGalleryAdminToManageUsersAndRoles = AppSetting.Instance.AllowGalleryAdminToManageUsersAndRoles;
					_appSettingUpdateable.AllowGalleryAdminToViewAllUsersAndRoles = AppSetting.Instance.AllowGalleryAdminToViewAllUsersAndRoles;
					_appSettingUpdateable.MaxNumberErrorItems = AppSetting.Instance.MaxNumberErrorItems;
					_appSettingUpdateable.JQueryScriptPath = AppSetting.Instance.JQueryScriptPath;
					_appSettingUpdateable.JQueryUiScriptPath = AppSetting.Instance.JQueryUiScriptPath;
				}

				return _appSettingUpdateable;
			}
		}

		public IAppSetting AppSettings
		{
			get { return AppSetting.Instance; }
		}

		public string MessageText
		{
			get { return _messageText; }
			set { _messageText = value; }
		}

		public string MessageCssClass
		{
			get { return _messageCssClass; }
			set { _messageCssClass = value; }
		}

		public bool MessageIsError
		{
			get { return _messageIsError; }
			set { _messageIsError = value; }
		}

		public bool SavingIsEnabled
		{
			get
			{
				return (License.IsInTrialPeriod || License.IsValid);
			}
		}

		public System.Configuration.Provider.ProviderBase MembershipGsp
		{
			get
			{
				return Controller.UserController.MembershipGsp;
			}
		}

		public System.Configuration.Provider.ProviderBase RoleGsp
		{
			get
			{
				return Controller.RoleController.RoleGsp;
			}
		}

		public System.Configuration.Provider.ProviderBase ProfileGsp
		{
			get
			{
				return System.Web.Profile.ProfileManager.Provider;
			}
		}

		public System.Configuration.Provider.ProviderBase GalleryDataProvider
		{
			get
			{
				return (System.Configuration.Provider.ProviderBase)Factory.GetDataProvider();
			}
		}

		#endregion

		#region Protected Events

		protected void Page_Init(object sender, EventArgs e)
		{
			this.AdminHeaderPlaceHolder = phAdminHeader;
			this.AdminFooterPlaceHolder = phAdminFooter;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!UserCanAdministerSite && UserCanAdministerGallery)
			{
				Util.Redirect(PageId.admin_galleries, "aid={0}", this.GetAlbumId());
			}

			this.CheckUserSecurity(SecurityActions.AdministerSite);

			ConfigureControlsEveryTime();

			if (!IsPostBack)
			{
				ConfigureControlsFirstTime();
			}
		}

		protected void btnEnterProductKey_Click(object sender, EventArgs args)
		{
			string productKey = txtProductKey.Text.Trim();

			ValidateProductKey(productKey);

			if (String.IsNullOrEmpty(productKey) || License.IsValid)
			{
				AppSetting.Instance.Save(productKey, null, null, null, null, null, null, null, null, null, null, null);
			}
		}

		protected override bool OnBubbleEvent(object source, EventArgs args)
		{
			//An event from the control has bubbled up.  If it's the Ok button, then run the
			//code to save the data to the database; otherwise ignore.
			Button btn = source as Button;
			if ((btn != null) && (((btn.ID == "btnOkTop") || (btn.ID == "btnOkBottom"))))
			{
				SaveSettings();

				// When auto trim is disabled, we store "0" in the config file, but we want to display an empty string
				// in the max # of items textbox. The event wwDataBinder_BeforeUnbindControl may have put a "0" in the 
				// textbox, so undo that now.
				if (txtMaxErrorItems.Text == "0")
					txtMaxErrorItems.Text = String.Empty;
			}

			return true;
		}

		protected bool wwDataBinder_BeforeUnbindControl(GalleryServerPro.WebControls.wwDataBindingItem item)
		{
			// When auto trim is disabled, we store "0" in the config file.
			if (item.ControlId == chkAutoTrimLog.ID)
			{
				if (!chkAutoTrimLog.Checked)
				{
					txtMaxErrorItems.Text = "0";
				}
			}

			if (item.ControlId == txtJQueryScriptPath.ID)
			{
				string url = txtJQueryScriptPath.Text.Trim();

				if (!String.IsNullOrEmpty(url) && !Util.IsAbsoluteUrl(url) && (!url.StartsWith("~")))
				{
					url = String.Concat("~", url); // Ensure relative URLs start with "~"
				}

				txtJQueryScriptPath.Text = url;
			}

			if (item.ControlId == txtJQueryUiScriptPath.ID)
			{
				string url = txtJQueryUiScriptPath.Text.Trim();

				if (!String.IsNullOrEmpty(url) && !Util.IsAbsoluteUrl(url) && (!url.StartsWith("~")))
				{
					url = String.Concat("~", url); // Ensure relative URLs start with "~"
				}

				txtJQueryUiScriptPath.Text = url;
			}

			return true;
		}

		protected void wwDataBinder_AfterBindControl(GalleryServerPro.WebControls.wwDataBindingItem item)
		{
			// 
			if (item.ControlInstance == txtMaxErrorItems)
			{
				int maxErrorItems = Convert.ToInt32(this.txtMaxErrorItems.Text);
				if (maxErrorItems == 0)
				{
					// Disable the checkbox because feature is turned off (a "0" indicates it is off). Set textbox to
					// an empty string because we don't want to display 0.
					chkAutoTrimLog.Checked = false;
					txtMaxErrorItems.Text = String.Empty;
				}
				else if (maxErrorItems > 0)
					chkAutoTrimLog.Checked = true; // Select the checkbox when max # of items is > 0
				else
				{
					// We'll never get here because the config definition uses an IntegerValidator to force the number
					// to be greater than 0.
				}
			}
		}

		protected bool wwDataBinder_ValidateControl(GalleryServerPro.WebControls.wwDataBindingItem item)
		{
			if (item.ControlInstance == txtMaxErrorItems)
			{
				if ((chkAutoTrimLog.Checked) && (Convert.ToInt32(txtMaxErrorItems.Text) <= 0))
				{
					item.BindingErrorMessage = Resources.GalleryServerPro.Admin_Error_Invalid_MaxNumberErrorItems_Msg;
					return false;
				}
			}

			if (item.ControlInstance == txtJQueryScriptPath)
			{
				if (!ValidateUrl(txtJQueryScriptPath.Text.Trim()))
				{
					item.BindingErrorMessage = Resources.GalleryServerPro.Admin_Site_Settings_InvalidJQueryPath;
					return false;
				}
			}

			if (item.ControlInstance == txtJQueryUiScriptPath)
			{
				if (!ValidateUrl(txtJQueryUiScriptPath.Text.Trim()))
				{
					item.BindingErrorMessage = Resources.GalleryServerPro.Admin_Site_Settings_InvalidJQueryPath;
					return false;
				}
			}

			return true;
		}

		#endregion

		#region Private Methods

		private bool ValidateUrl(string url)
		{
			// Verify the jQuery path exists, but only for local URLs. We don't bother with absolute URLs.
			if (String.IsNullOrEmpty(url) || Util.IsAbsoluteUrl(url))
				return true;
			else
				return System.IO.File.Exists(Server.MapPath(url));
		}

		private void ConfigureControlsFirstTime()
		{
			AdminPageTitle = Resources.GalleryServerPro.Admin_Site_Settings_General_Page_Header;
			txtProductKey.Text = License.ProductKey;

			OkButtonBottom.Enabled = this.SavingIsEnabled;
			OkButtonTop.Enabled = this.SavingIsEnabled;

			this.wwDataBinder.DataBind();

			UpdateUI();

			UpdateProductKeyValidationMessage();
		}

		private void ConfigureControlsEveryTime()
		{
			this.PageTitle = Resources.GalleryServerPro.Admin_Site_Settings_General_Page_Header;

			lblVersion.Text = String.Concat("v", Util.GetGalleryServerVersion());
		}

		private void DetermineMessage()
		{
			if (String.IsNullOrEmpty(this.MessageText))
			{
				bool isInTrialMode = (License.IsInTrialPeriod && !License.IsValid);
				if (isInTrialMode)
				{
					int daysLeftInTrial = (License.InstallDate.AddDays(GlobalConstants.TrialNumberOfDays) - DateTime.Today).Days;
					this.MessageText = string.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Admin_In_Trial_Period_Msg, daysLeftInTrial);
					this.MessageCssClass = "wwErrorSuccess gsp_msgfriendly";
				}

				bool trialPeriodExpired = (!License.IsInTrialPeriod && !License.IsValid);
				if (trialPeriodExpired)
				{
					this.MessageText = Resources.GalleryServerPro.Admin_Need_Product_Key_Msg;
					this.MessageCssClass = "wwErrorSuccess gsp_msgwarning";
				}
			}
		}

		private void UpdateUI()
		{
			DetermineMessage();

			if (!String.IsNullOrEmpty(this.MessageText))
			{
				wwMessage.CssClass = this.MessageCssClass;

				if (this.MessageIsError)
					wwMessage.Text = this.MessageText;
				else
					wwMessage.ShowMessage(this.MessageText);
			}
		}

		private void UpdateProductKeyValidationMessage()
		{
			if (String.IsNullOrEmpty(License.ProductKey))
			{
				lblProductKeyValidationMsg.Text = Resources.GalleryServerPro.Admin_Site_Settings_ProductKey_NotEntered_Label;
				lblProductKeyValidationMsg.CssClass = "gsp_msgfriendly";
				imgProductKeyValidation.ImageUrl = Util.GetUrl("/images/info.gif");
				imgProductKeyValidation.Visible = true;
			}
			else if (License.IsValid)
			{
				lblProductKeyValidationMsg.Text = Resources.GalleryServerPro.Admin_Site_Settings_ProductKey_Correct_Label;
				lblProductKeyValidationMsg.CssClass = "gsp_msgfriendly";
				imgProductKeyValidation.ImageUrl = Util.GetUrl("/images/ok_16x16.png");
				imgProductKeyValidation.Visible = true;
			}
			else
			{
				lblProductKeyValidationMsg.Text = Resources.GalleryServerPro.Admin_Site_Settings_ProductKey_Incorrect_Label;
				lblProductKeyValidationMsg.CssClass = "gsp_msgwarning";
				imgProductKeyValidation.ImageUrl = Util.GetUrl("/images/error_16x16.png");
				imgProductKeyValidation.Visible = true;
			}
		}

		/// <summary>
		/// Verify the product key is valid and displays to the user the results of the validation. The <see cref="License" />
		/// property is updated with the results of the validation.
		/// </summary>
		/// <param name="productKey">The product key to validate.</param>
		private void ValidateProductKey(string productKey)
		{
			ILicense license = new License();
			license.ProductKey = productKey;

			License = SecurityManager.ValidateLicense(license, HelperFunctions.GetGalleryServerVersion(), true);

			if (!String.IsNullOrEmpty(productKey))
			{
				if (License.IsValid)
				{
					this.MessageText = Resources.GalleryServerPro.Admin_Save_ProductKey_Success_Text;
					this.MessageCssClass = "wwErrorSuccess gsp_msgfriendly gsp_bold";
				}
				else
				{
					wwDataBinder.AddBindingError(Resources.GalleryServerPro.Admin_Site_Settings_ProductKey_Incorrect_Msg, txtProductKey);

					if (wwDataBinder.BindingErrors.Count > 0)
					{
						this.MessageText = wwDataBinder.BindingErrors.ToHtml();
						this.MessageCssClass = "wwErrorFailure gsp_msgwarning";
						this.MessageIsError = true;
					}
				}
			}

			OkButtonBottom.Enabled = this.SavingIsEnabled;
			OkButtonTop.Enabled = this.SavingIsEnabled;

			UpdateUI();
			UpdateProductKeyValidationMessage();
		}

		private void SaveSettings()
		{
			this.wwDataBinder.Unbind(this);

			if (wwDataBinder.BindingErrors.Count > 0)
			{
				this.MessageText = wwDataBinder.BindingErrors.ToHtml();
				this.MessageCssClass = "wwErrorFailure gsp_msgwarning";
				this.MessageIsError = true;
				UpdateUI();
				return;
			}

			AppSetting.Instance.Save(null, null, null, null, AppSettingsUpdateable.JQueryScriptPath, AppSettingsUpdateable.JQueryUiScriptPath,
				null, null, AppSettingsUpdateable.EnableCache, AppSettingsUpdateable.AllowGalleryAdminToManageUsersAndRoles,
				AppSettingsUpdateable.AllowGalleryAdminToViewAllUsersAndRoles, AppSettingsUpdateable.MaxNumberErrorItems);

			this.MessageText = Resources.GalleryServerPro.Admin_Save_Success_Text;
			this.MessageCssClass = "wwErrorSuccess gsp_msgfriendly gsp_bold";

			HelperFunctions.PurgeCache();

			UpdateUI();
		}

		#endregion
	}

	/// <summary>
	/// A simple object to store application settings from the web page in preparation for being saved to the data store.
	/// </summary>
	public class AppSettingsEntity
	{
		public bool EnableCache;
		public bool AllowGalleryAdminToManageUsersAndRoles;
		public bool AllowGalleryAdminToViewAllUsersAndRoles;
		public int MaxNumberErrorItems;
		public string JQueryScriptPath;
		public string JQueryUiScriptPath;
	}
}