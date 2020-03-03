using System;
using GalleryServerPro.Web.Controls;
using Sueetie.Core;

namespace GalleryServerPro.Web.Pages
{
    public partial class album : Pages.GalleryPage
    {
        #region Private Fields

        // Sueetie Modified - Loading Thumbnail or Sueetie ListView Controls

        private thumbnailview _thumbnailview;
        private sueetielistview _sueetielistview;


        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="album"/> class.
        /// </summary>
        protected album()
        {
            this.BeforeHeaderControlsAdded += AlbumBeforeHeaderControlsAdded;
        }

        #region Public Properties

        #endregion

        #region Protected Events


        protected void Page_Load(object sender, EventArgs e)
        {
            // Sueetie Modified - Displaying either Thumbnail or Sueetie ListView Control
            if (this.CurrentSueetieGallery.DisplayTypeID == (int)SueetieDisplayType.Folders)
            {
                _sueetielistview = (sueetielistview)LoadControl("../controls/sueetielistview.ascx");
                phThumbnailLoader.Controls.Add(_sueetielistview);
            }
            else
            {
                _thumbnailview = (thumbnailview)LoadControl("../controls/thumbnailview.ascx");
                phThumbnailLoader.Controls.Add(_thumbnailview);
            }
            ShowMessage();
        }

        #endregion

        #region Private Static Methods

        #endregion

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

        private void AlbumBeforeHeaderControlsAdded(object sender, EventArgs e)
        {
            ShowAlbumTreeViewForAlbum = (this.GalleryControl.ShowAlbumTreeViewForAlbum.HasValue ? this.GalleryControl.ShowAlbumTreeViewForAlbum.Value : false);
        }
    }
}