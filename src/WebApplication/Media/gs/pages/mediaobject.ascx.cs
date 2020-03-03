using System;
using GalleryServerPro.Web.Controls;

namespace GalleryServerPro.Web.Pages
{
	public partial class mediaobject : Pages.GalleryPage
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="mediaobject"/> class.
		/// </summary>
		protected mediaobject()
		{
			this.BeforeHeaderControlsAdded += MediaObjectBeforeHeaderControlsAdded;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			ShowMessage();
		}

		/// <summary>
		/// Renders the <see cref="Message"/>. No action is taken if <see cref="Message"/> is Message.None.
		/// </summary>
		private void ShowMessage()
		{
			if (this.Message == Message.None)
				return;

			usermessage msgBox = this.GetMessageControl();

			phMessage.Controls.Add(msgBox);
		}

		private void MediaObjectBeforeHeaderControlsAdded(object sender, EventArgs e)
		{
			bool showAlbumTreeViewSetting = (this.GalleryControl.ShowAlbumTreeViewForAlbum.HasValue ? this.GalleryControl.ShowAlbumTreeViewForAlbum.Value : false);
			
			ShowAlbumTreeViewForAlbum = (ShowAlbumTreeViewForMediaObject & showAlbumTreeViewSetting);
		}
	}
}