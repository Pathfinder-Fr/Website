using System;
using System.Collections;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.ErrorHandler.CustomExceptions;
using GalleryServerPro.Web.Controller;
using Sueetie.Core;
using System.Web;

namespace GalleryServerPro.Web.Pages.Admin
{
	public partial class galleries : Pages.AdminPage
	{
		#region Private Fields

		private string _messageText;
		private string _messageCssClass;
		private bool _messageIsError;

		#endregion

		#region Properties

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

		protected void Page_Init(object sender, EventArgs e)
		{
			this.AdminHeaderPlaceHolder = phAdminHeader;
			this.AdminFooterPlaceHolder = phAdminFooter;

            // Sueetie Modified - To ensure using /media/default.aspx, not a gallery page
            if (HttpContext.Current.Request.RawUrl.ToLower().IndexOf("/default.aspx") == -1)
                Response.Redirect("default.aspx?g=admin_galleries");
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			this.CheckUserSecurity(SecurityActions.AdministerSite | SecurityActions.AdministerGallery);

			ConfigureControlsEveryTime();

			if (!IsPostBack)
			{
				ConfigureControlsFirstTime();
			}

			JQueryRequired = true;
		}

		protected void gvGalleries_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				if ((e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
				{
					gvGalleriesRowDataBoundEditMode(e); // Current row is in edit mode
				}
				else if ((e.Row.RowState & DataControlRowState.Normal) == DataControlRowState.Normal)
				{
					gvGalleriesRowDataBoundViewMode(e); // Current row is in view mode
				}
			}
		}

		private void gvGalleriesRowDataBoundEditMode(GridViewRowEventArgs e)
		{
			DataBindMediaPathInGalleriesGridRow(e);
		}

		private void DataBindMediaPathInGalleriesGridRow(GridViewRowEventArgs e)
		{
			Label lblMediaPath = (Label)e.Row.FindControl("lblMediaPath");

			if (lblMediaPath == null)
			{
				throw new WebException("Cannot find a Label with ID='lblMediaPath' in the current row of the GridView 'gvGalleries'.");
			}

			IGallery gallery = (IGallery)e.Row.DataItem;
			IGallerySettings gallerySettings = Factory.LoadGallerySetting(gallery.GalleryId);

			lblMediaPath.Text = gallerySettings.MediaObjectPath;
		}

		private void gvGalleriesRowDataBoundViewMode(GridViewRowEventArgs e)
		{
			DataBindViewButtonInGalleriesGridRow(e);

			DataBindEditButtonInGalleriesGridRow(e);

			DataBindDeleteButtonInGalleriesGridRow(e);

			DataBindMediaPathInGalleriesGridRow(e);
		}

		private void DataBindViewButtonInGalleriesGridRow(GridViewRowEventArgs e)
		{
			// reference the View LinkButton
			HyperLink hlViewGallery = (HyperLink)e.Row.FindControl("hlViewGallery");

			if (hlViewGallery == null)
			{
				throw new WebException("Cannot find a LinkButton with ID='lbViewGallery' in the current row of the GridView 'gvGalleries'.");
			}

			// Get information about the product bound to the row
			IGallery gallery = (IGallery)e.Row.DataItem;

			if (gallery.GalleryId == GalleryId)
			{
				hlViewGallery.NavigateUrl = Util.GetCurrentPageUrl();
				hlViewGallery.Visible = true;
			}
		}

		private void DataBindEditButtonInGalleriesGridRow(GridViewRowEventArgs e)
		{
			// reference the View LinkButton
			LinkButton lbEditGallery = (LinkButton)e.Row.FindControl("lbEditGallery");

			if (lbEditGallery == null)
			{
				throw new WebException("Cannot find a LinkButton with ID='lbEditGallery' in the current row of the GridView 'gvGalleries'.");
			}

			if (AppSetting.Instance.License.IsInReducedFunctionalityMode)
			{
				lbEditGallery.Visible = false;
			}
		}

		private void DataBindDeleteButtonInGalleriesGridRow(GridViewRowEventArgs e)
		{
			// reference the Delete LinkButton
			LinkButton lbDeleteGallery = (LinkButton)e.Row.FindControl("lbDeleteGallery");

			if (lbDeleteGallery == null)
			{
				throw new WebException("Cannot find a LinkButton with ID='lbDeleteGallery' in the current row of the GridView 'gvGalleries'.");
			}

			// Get information about the product bound to the row
			IGallery gallery = (IGallery)e.Row.DataItem;

			if (gallery.GalleryId == GalleryId)
			{
				string msg = String.Format(Resources.GalleryServerPro.Admin_Gallery_Settings_Cannot_Delete_Current_Gallery_Text, gallery.Description.Replace("'", @"\'"));
				lbDeleteGallery.OnClientClick = string.Format("alert('{0}'); return false;", msg);
			}
			else
			{
				string msg = String.Format(Resources.GalleryServerPro.Admin_Gallery_Settings_Delete_Gallery_Confirm_Text, gallery.Description.Replace("'", @"\'"));
				lbDeleteGallery.OnClientClick = string.Format("return confirm('{0}');", msg);
			}

			if (AppSetting.Instance.License.IsInReducedFunctionalityMode)
			{
				lbDeleteGallery.Visible = false;
			}
		}

		protected void gvGalleries_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			switch (e.CommandName)
			{
				case "Insert":
					{
						TextBox txtInsert = gvGalleries.FooterRow.FindControl("txtDescriptionInsert") as TextBox;

						if (txtInsert == null)
						{
							throw new WebException("Could not find a TextBox named 'txtDescriptionInsert' in the GridView's row.");
						}

						if (String.IsNullOrEmpty(txtInsert.Text))
						{
							HandleGalleryEditFailure(Resources.GalleryServerPro.Admin_Gallery_Settings_Gallery_Description_Required);
						}
						else
						{
							IGallery newGallery = CreateGallery(txtInsert.Text);

							SetMediaObjectPathForNewGallery(newGallery);

							VerifyUserHasGalleryAdminPermission(newGallery);

							HandleGalleryEditSuccess(Resources.GalleryServerPro.Admin_Gallery_Settings_Gallery_Created_Success_Text);
						}

						break;
					}
			}

			gvGalleries.DataBind();
		}

		protected void odsGalleries_Updating(object sender, ObjectDataSourceMethodEventArgs e)
		{
			foreach (DictionaryEntry entry in e.InputParameters)
			{
				IGallery gallery = entry.Value as IGallery;
				if ((gallery != null) && String.IsNullOrEmpty(gallery.Description))
				{
					e.Cancel = true;

					this.MessageText = Resources.GalleryServerPro.Admin_Gallery_Settings_Gallery_Description_Required;
					this.MessageIsError = true;

					UpdateUI();
				}
			}
		}

		protected void odsGalleries_Updated(object sender, ObjectDataSourceStatusEventArgs e)
		{
			HandleGalleryEditSuccess(Resources.GalleryServerPro.Admin_Save_Success_Text);
		}

		protected void odsGalleries_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
		{
			// Make sure user isn't trying to delete the current gallery. Javascript on the client should have prevented us from getting to this point, but 
			// we check again as an extra safety measure.
			foreach (DictionaryEntry entry in e.InputParameters)
			{
				IGallery gallery = entry.Value as IGallery;
				if ((gallery != null) && (gallery.GalleryId == GalleryId))
				{
					e.Cancel = true;

					string msg = String.Format(Resources.GalleryServerPro.Admin_Gallery_Settings_Cannot_Delete_Current_Gallery_Text, gallery.Description.Replace("'", @"\'"));

					this.MessageText = msg;
					this.MessageIsError = true;

					UpdateUI();
				}
			}
		}

		protected void odsGalleries_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
		{
			HandleGalleryEditSuccess(Resources.GalleryServerPro.Admin_Gallery_Settings_Gallery_Deleted_Success_Text);
		}

		protected void lbChangeGallery_Click(object sender, EventArgs e)
		{
			DetectAndSaveChangedGallery();
		}

		#endregion

		#region Private Methods

		private void ConfigureControlsFirstTime()
		{
			AdminPageTitle = Resources.GalleryServerPro.Admin_Gallery_Manager_Page_Header;

			OkButtonIsVisible = false;
			CancelButtonIsVisible = false;

			if (AppSetting.Instance.License.IsInReducedFunctionalityMode)
			{
				wwMessage.ShowMessage(Resources.GalleryServerPro.Admin_Need_Product_Key_Msg2);
				wwMessage.CssClass = "wwErrorSuccess gsp_msgwarning";
				lbChangeGallery.Visible = false;

				gvGalleries.Columns[0].Visible = false;
				gvGalleries.ShowFooter = false;
			}

			DataBindGalleriesComboBox();

			CheckForMessages();

			UpdateUI();
		}

		private void DataBindGalleriesComboBox()
		{
			ddlCurrentGallery.Items.Clear();

			foreach (IGallery gallery in UserController.GetGalleriesCurrentUserCanAdminister())
			{
				ListItem li = new ListItem(Util.RemoveHtmlTags(gallery.Description), gallery.GalleryId.ToString());

				if (gallery.GalleryId == GalleryId)
				{
					li.Selected = true;
				}

				ddlCurrentGallery.Items.Add(li);
			}
		}

		private void ConfigureControlsEveryTime()
		{
			this.PageTitle = Resources.GalleryServerPro.Admin_Gallery_Manager_Page_Header;
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

		private void DetectAndSaveChangedGallery()
		{
			int galleryId = Convert.ToInt32(ddlCurrentGallery.SelectedValue);

			if (GalleryId != galleryId)
			{
				// User wants to change the current gallery. First verify gallery exists and the user has permission, then update.
				IGallery gallery;
				try
				{
					gallery = Factory.LoadGallery(galleryId);
				}
				catch (InvalidGalleryException)
				{
					// Not a valid gallery. Set message and return.
					this.MessageText = "Invalid gallery.";
					this.MessageIsError = true;
					return;
				}

				if (UserController.GetGalleriesCurrentUserCanAdminister().Contains(gallery))
				{
					this.GalleryControlSettingsUpdateable.GalleryId = galleryId;
					this.GalleryControlSettingsUpdateable.AlbumId = null;
					this.GalleryControlSettingsUpdateable.MediaObjectId = null;
					this.GalleryControlSettingsUpdateable.Save();

					Factory.ClearGalleryControlSettingsCache();

					// Since we are changing galleries, we need to perform a redirect to get rid of the album ID from the old gallery that
					// is sitting in the query string.
					const Message msg = Message.GallerySuccessfullyChanged;

					Util.Redirect(PageId.admin_galleries, "msg={0}", ((int)msg).ToString(CultureInfo.InvariantCulture));
				}
				else
				{
					// User does not have permission to change to this gallery. Set message and return.
					this.MessageText = "Invalid gallery.";
					this.MessageIsError = true;
				}
			}
			else
			{
				this.MessageText = Resources.GalleryServerPro.Admin_Gallery_Settings_Different_Gallery_Not_Selected_Text;
			}

			UpdateUI();
		}

		private static IGallery CreateGallery(string description)
		{
			IGallery gallery = Factory.CreateGalleryInstance();
			gallery.Description = description;
			gallery.Save();


            // Sueetie Modified - Create Sueetie Media Gallery record stub
            SueetieMedia.CreateMediaGallery(gallery.GalleryId);

			return gallery;
		}

		private void VerifyUserHasGalleryAdminPermission(IGallery newGallery)
		{
			// If the current user is only a gallery admin, she won't have access to the new gallery, so we need to add the
			// new gallery to the gallery admin role she is in.
			if (!UserCanAdministerSite && UserCanAdministerGallery)
			{
				foreach (IGalleryServerRole role in RoleController.GetGalleryServerRolesForUser())
				{
					if (role.AllowAdministerGallery)
					{
						IAlbum rootAlbum = Factory.LoadRootAlbumInstance(newGallery.GalleryId);

						if (!role.RootAlbumIds.Contains(rootAlbum.Id))
						{
							role.RootAlbumIds.Add(rootAlbum.Id);
							role.Save();
						}
						break;
					}
				}
			}
		}

		private void HandleGalleryEditSuccess(string msg)
		{
			HelperFunctions.PurgeCache();

			DataBindGalleriesComboBox();

			this.MessageText = msg;

			UpdateUI();
		}

		private void HandleGalleryEditFailure(string msg)
		{
			HelperFunctions.PurgeCache();

			DataBindGalleriesComboBox();

			this.MessageText = msg;
			this.MessageIsError = true;

			UpdateUI();
		}

		/// <summary>
		/// Determine if there are any messages we need to display to the user.
		/// </summary>
		private void CheckForMessages()
		{
			if (Message == Message.GallerySuccessfullyChanged)
			{
				MessageText = Resources.GalleryServerPro.Admin_Gallery_Settings_Gallery_Changed_Text;
			}
		}

		/// <summary>
		/// Sets the media object path for the new gallery to the path of the current gallery. The change is persisted to the data store.
		/// </summary>
		/// <param name="gallery">The gallery.</param>
		private void SetMediaObjectPathForNewGallery(IGallery gallery)
		{
			IGallerySettings gallerySettings = Factory.LoadGallerySetting(gallery.GalleryId, true);

			gallerySettings.MediaObjectPath = GallerySettings.MediaObjectPath;
			gallerySettings.ThumbnailPath = GallerySettings.ThumbnailPath;
			gallerySettings.OptimizedPath = GallerySettings.OptimizedPath;

			gallerySettings.Save();
		}

		#endregion
	}
}