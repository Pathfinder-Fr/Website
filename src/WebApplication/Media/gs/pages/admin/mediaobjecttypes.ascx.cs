using System;
using System.Globalization;
using System.Web.UI.WebControls;
using ComponentArt.Web.UI;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;

namespace GalleryServerPro.Web.Pages.Admin
{
	public partial class mediaobjecttypes : Pages.AdminPage
	{
		#region Properties

		#endregion

		#region Protected Events

		protected void Page_Init(object sender, EventArgs e)
		{
			this.AdminHeaderPlaceHolder = phAdminHeader;
			this.AdminFooterPlaceHolder = phAdminFooter;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			this.CheckUserSecurity(SecurityActions.AdministerSite | SecurityActions.AdministerGallery);

			ConfigureControlsEveryTime();

			if (!IsPostBack)
			{
				ConfigureControlsFirstTime();
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
			}

			return true;
		}

		#endregion

		#region Private Methods

		private void ConfigureControlsFirstTime()
		{
			ConfigureGrid();

			AdminPageTitle = Resources.GalleryServerPro.Admin_Media_Objects_Mime_Types_Page_Header;

			//if (!HasEditConfigPermission)
			//{
			//  wwMessage.ShowMessage(String.Format(Resources.GalleryServerPro.Admin_Config_Security_Ex_Msg, Util.GalleryServerProConfigFilePath));
			//  wwMessage.CssClass = "wwErrorSuccess gsp_msgwarning";
			//  OkButtonBottom.Enabled = false;
			//  OkButtonTop.Enabled = false;
			//}

			if (AppSetting.Instance.License.IsInReducedFunctionalityMode)
			{
				wwMessage.ShowMessage(Resources.GalleryServerPro.Admin_Need_Product_Key_Msg2);
				wwMessage.CssClass = "wwErrorSuccess gsp_msgwarning";
				OkButtonBottom.Enabled = false;
				OkButtonTop.Enabled = false;
			}

			this.wwDataBinder.DataBind();
		}

		private void ConfigureGrid()
		{
			gdMimeTypes.ImagesBaseUrl = String.Concat(Util.GalleryRoot, "/images/componentart/grid/");
			gdMimeTypes.DataSource = MimeType.LoadMimeTypes(GalleryId);
			gdMimeTypes.DataBind();
		}

		private void ConfigureControlsEveryTime()
		{
			this.PageTitle = Resources.GalleryServerPro.Admin_Media_Objects_Mime_Types_Page_Header;
			lblGalleryDescription.Text = String.Format(Resources.GalleryServerPro.Admin_Gallery_Description_Label, Util.GetCurrentPageUrl(), Util.HtmlEncode(Factory.LoadGallery(GalleryId).Description));
		
			AddClientTemplatesToGrid();
		}

		private void AddClientTemplatesToGrid()
		{
			// Add table header client template that contains the Check/uncheck all checkbox.
			ClientTemplate tableHdrClientTemplate = new ClientTemplate();
			tableHdrClientTemplate.ID = "enabledHeader";

			tableHdrClientTemplate.Text = String.Format(CultureInfo.InvariantCulture, @"<input id=""chkAll"" type=""checkbox"" onclick=""javascript:setGridCheck(0, this.checked);"" ## chkAllCheckboxIsChecked ? ""checked='checked'"" : """" ## /> <label for=""chkAll"">{0}</label>",
				Resources.GalleryServerPro.Admin_MimeTypes_ToggleCheckAll_Label);

			gdMimeTypes.ClientTemplates.Add(tableHdrClientTemplate);
		}

		private void SaveSettings()
		{
			this.wwDataBinder.Unbind(this);

			bool previousAllowAllValue = GallerySettings.AllowUnspecifiedMimeTypes;
			if (GallerySettingsUpdateable.AllowUnspecifiedMimeTypes != previousAllowAllValue)
			{
				GallerySettingsUpdateable.Save();
			}

			if (wwDataBinder.BindingErrors.Count > 0)
			{
				this.wwMessage.CssClass = "wwErrorFailure gsp_msgwarning";
				this.wwMessage.Text = wwDataBinder.BindingErrors.ToHtml();
				return;
			}

			// Loop through each record in the grid. For each file extension, get the matching MIME type. If the value has changed, update it.
			foreach (GridItem row in gdMimeTypes.Items)
			{
				object[] rowValues = (object[])row.DataItem;
				bool enabled = Convert.ToBoolean(rowValues[0], CultureInfo.InvariantCulture);
				string fileExt = rowValues[1].ToString();

				IMimeType mimeType = MimeType.LoadMimeType(GalleryId, fileExt);
				if (mimeType.AllowAddToGallery != enabled)
				{
					mimeType.AllowAddToGallery = enabled;
					mimeType.Save();
				}
			}

			this.wwMessage.CssClass = "wwErrorSuccess gsp_msgfriendly gsp_bold";
			this.wwMessage.ShowMessage(Resources.GalleryServerPro.Admin_Save_Success_Text);
		}

		#endregion

	}
}