using System;
using GalleryServerPro.Business;

namespace GalleryServerPro.Web.Controls.Admin
{
	public partial class adminmenu : GalleryUserControl
	{
		#region Event Handlers

		protected void Page_Load(object sender, EventArgs e)
		{
			ConfigureControlsEveryTime();
		}

		#endregion

		#region Private Methods

		private void ConfigureControlsEveryTime()
		{
			nbAdminMenu.ImagesBaseUrl = String.Concat(Util.GalleryRoot, "/images/componentart/navbar/");

			nbiSiteGeneral.NavigateUrl = Util.GetUrl(PageId.admin_sitesettings, "aid={0}", GalleryPage.GetAlbumId());
			nbiBackupRestore.NavigateUrl = Util.GetUrl(PageId.admin_backuprestore, "aid={0}", GalleryPage.GetAlbumId());
			nbiGalleries.NavigateUrl = Util.GetUrl(PageId.admin_galleries, "aid={0}", GalleryPage.GetAlbumId());
			nbiGallerySetting.NavigateUrl = Util.GetUrl(PageId.admin_gallerysettings, "aid={0}", GalleryPage.GetAlbumId());
			nbiGalleryControl.NavigateUrl = Util.GetUrl(PageId.admin_gallerycontrolsettings, "aid={0}", GalleryPage.GetAlbumId());
			nbiErrorLog.NavigateUrl = Util.GetUrl(PageId.admin_errorlog, "aid={0}", GalleryPage.GetAlbumId());
			nbiUserSettings.NavigateUrl = Util.GetUrl(PageId.admin_usersettings, "aid={0}", GalleryPage.GetAlbumId());
			nbiManageUsers.NavigateUrl = Util.GetUrl(PageId.admin_manageusers, "aid={0}", GalleryPage.GetAlbumId());
			nbiManageRoles.NavigateUrl = Util.GetUrl(PageId.admin_manageroles, "aid={0}", GalleryPage.GetAlbumId());
			nbiAlbumsGeneral.NavigateUrl = Util.GetUrl(PageId.admin_albums, "aid={0}", GalleryPage.GetAlbumId());
			nbiMediaObjectsGeneral.NavigateUrl = Util.GetUrl(PageId.admin_mediaobjects, "aid={0}", GalleryPage.GetAlbumId());
			nbiMetadata.NavigateUrl = Util.GetUrl(PageId.admin_metadata, "aid={0}", GalleryPage.GetAlbumId());
			nbiMediaObjectTypes.NavigateUrl = Util.GetUrl(PageId.admin_mediaobjecttypes, "aid={0}", GalleryPage.GetAlbumId());
			nbiImages.NavigateUrl = Util.GetUrl(PageId.admin_images, "aid={0}", GalleryPage.GetAlbumId());
			nbiVideoAudioOther.NavigateUrl = Util.GetUrl(PageId.admin_videoaudioother, "aid={0}", GalleryPage.GetAlbumId());

            // Sueetie Modified - add menu link
            nbiSueetieObjects.NavigateUrl = Util.GetUrl(PageId.admin_sueetiecontent, "aid={0}", GalleryPage.GetAlbumId());
            nbiSueetieAlbumPaths.NavigateUrl = Util.GetUrl(PageId.admin_sueetiealbumpaths, "aid={0}", GalleryPage.GetAlbumId());

			if (this.GalleryPage.UserCanAdministerSite || this.GalleryPage.UserCanAdministerGallery)
			{
				// Proactive security: Even though the pages that use this control have their own security that make this redundant,
				// we do it anyway for extra protection. Only show menu when user is a site or gallery admin.
				nbAdminMenu.Visible = true;

				if (!this.GalleryPage.UserCanAdministerSite)
				{
					// Hide the site-level settings from gallery administators
					nbiSiteSettings.Visible = false;

					// Hide the user/role management pages from gallery admins when the app setting says that can't manage them.
					nbiManageUsers.Visible = AppSetting.Instance.AllowGalleryAdminToManageUsersAndRoles;
					nbiManageRoles.Visible = AppSetting.Instance.AllowGalleryAdminToManageUsersAndRoles;
				}
			}
		}

		#endregion
	}
}