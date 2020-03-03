using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.ErrorHandler.CustomExceptions;
using GalleryServerPro.Web.Controller;
using System.Data;
using Sueetie.Core;

namespace GalleryServerPro.Web.gs.pages.task
{
    public partial class includeindownload : Pages.TaskPage
    {
        #region Private Fields

        private IGalleryObjectCollection _galleryObjects;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the gallery objects that are candidates for deleting.
        /// </summary>
        /// <value>The gallery objects that are candidates for deleting.</value>
        public IGalleryObjectCollection GalleryObjects
        {
            get
            {
                if (this._galleryObjects == null)
                {
                    this._galleryObjects = this.GetAlbum().GetChildGalleryObjects(GalleryObjectType.MediaObject, true);
                }

                return this._galleryObjects;
            }
        }

        #endregion

        #region Event Handlers

        protected void Page_Init(object sender, EventArgs e)
        {
            this.TaskHeaderPlaceHolder = phTaskHeader;
            this.TaskFooterPlaceHolder = phTaskFooter;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.GallerySettings.MediaObjectPathIsReadOnly)
                RedirectToAlbumViewPage("msg={0}", ((int)Message.CannotEditGalleryIsReadOnly).ToString(CultureInfo.InvariantCulture));

            this.CheckUserSecurity(SecurityActions.DeleteMediaObject);

            if (!IsPostBack)
            {
                ConfigureControls();
            }

            ConfigureControlsEveryPageLoad();
        }

        protected override bool OnBubbleEvent(object source, EventArgs args)
        {
            //An event from a control has bubbled up.  If it's the Ok button, then run the
            //code to synchronize; otherwise ignore.
            Button btn = source as Button;
            if ((btn != null) && (((btn.ID == "btnOkTop") || (btn.ID == "btnOkBottom"))))
            {
                if (btnOkClicked())
                {
                    this.RedirectToAlbumViewPage("msg={0}", ((int)Message.ObjectsSuccessfullyIncluded).ToString(CultureInfo.InvariantCulture));
                }
            }

            return true;
        }

        #endregion

        #region Protected Methods

        // Sueetie Modified - Bind Checkbox Checked Status

        protected void rptr_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                GalleryObject galleryObject = ((GalleryObject)e.Item.DataItem);
                if (galleryObject.GetType().Name.ToLower() != "album")
                {
                    int mediaObjectID = galleryObject.Id;
                    CheckBox chkMO = (CheckBox)e.Item.FindControl("chkMO");
                    chkMO.Checked = SueetieMedia.IsIncludedInDownload(mediaObjectID);
                }
            }
        }

        /// <summary>
        /// Return an HTML formatted string representing the title of the gallery object. It is truncated and purged of HTML tags
        /// if necessary.
        /// </summary>
        /// <param name="title">The title of the gallery object as stored in the data store.</param>
        /// <param name="galleryObjectType">The type of the object to which the title belongs.</param>
        /// <returns>Returns a string representing the title of the media object. It is truncated and purged of HTML tags
        /// if necessary.</returns>
        protected string GetGalleryObjectText(string title, Type galleryObjectType)
        {
            // If this is an album, return an empty string. Otherwise, return the title, truncated and purged of HTML
            // tags if necessary. If the title is truncated, add an ellipses to the text.
            //<asp:Label ID="lblAlbumPrefix" runat="server" CssClass="gsp_bold" Text="<%$ Resources:GalleryServerPro, UC_ThumbnailView_Album_Title_Prefix_Text %>" />&nbsp;<%# GetGalleryObjectText(Eval("Title").ToString(), Container.DataItem.GetType())%>

            int maxLength = GallerySettings.MaxMediaObjectThumbnailTitleDisplayLength;

            string titlePrefix = String.Empty;

            if (galleryObjectType == typeof(Album))
            {
                // Album titles need a prefix, so assign that now.
                titlePrefix = String.Format(CultureInfo.CurrentCulture, "<span class='gsp_bold'>{0} </span>", Resources.GalleryServerPro.UC_ThumbnailView_Album_Title_Prefix_Text);

                // Override the previous max length with the value that is appropriate for albums.
                maxLength = GallerySettings.MaxAlbumThumbnailTitleDisplayLength;
            }

            string truncatedText = Util.TruncateTextForWeb(title, maxLength);

            if (truncatedText.Length != title.Length)
                return String.Concat(titlePrefix, truncatedText, "...");
            else
                return String.Concat(titlePrefix, truncatedText);
        }

        protected static string GetThumbnailCssClass(Type galleryObjectType)
        {
            // If it's an album then specify the appropriate CSS class so that the "Album"
            // header appears over the thumbnail. This is to indicate to the user that the
            // thumbnail represents an album.
            if (galleryObjectType == typeof(Album))
                return "thmb album";
            else
                return "thmb";
        }

        protected static string GetId(IGalleryObject galleryObject)
        {
            // Prepend an 'a' (for album) or 'm' (for media object) to the ID to indicate whether it is
            // an album ID or media object ID.
            if (galleryObject is Album)
                return "a" + galleryObject.Id.ToString(CultureInfo.InvariantCulture);
            else
                return "m" + galleryObject.Id.ToString(CultureInfo.InvariantCulture);
        }

        #endregion

        #region Private Methods

        private void ConfigureControls()
        {
            this.TaskHeaderText = Resources.Sueetie.Include_Download_Objects_Header_Text;
            this.TaskBodyText = Resources.Sueetie.Include_Download_Objects_Body_Text;
            this.OkButtonText = Resources.Sueetie.Include_Download_Objects_Ok_Button_Text;
            this.OkButtonToolTip = Resources.GalleryServerPro.Task_Delete_Objects_Ok_Button_Tooltip;

            this.PageTitle = Resources.Sueetie.Include_Download_Objects_Page_Title;

            if (GalleryObjects.Count > 0)
            {
                rptr.DataSource = GalleryObjects;
                rptr.DataBind();
            }
            else
            {
                this.RedirectToAlbumViewPage("msg={0}", ((int)Message.CannotIncludeObjectsNoObjectsExistInAlbum).ToString(CultureInfo.InvariantCulture));
            }
        }

        private void ConfigureControlsEveryPageLoad()
        {
            SetThumbnailCssStyle(GalleryObjects);
        }

        private bool btnOkClicked()
        {

            CheckBox chkbx;
            HiddenField gc;

            foreach (RepeaterItem rptrItem in rptr.Items)
            {
                chkbx = rptrItem.FindControl("chkMO") as CheckBox;
                gc = (HiddenField)rptrItem.FindControl("hdn");
                int id = Convert.ToInt32(gc.Value.Substring(1), CultureInfo.InvariantCulture);
                SueetieMedia.SetIncludedInDownload(id, chkbx.Checked);
            }

            return true;
        }


        private string[] RetrieveUserSelections()
        {
            // Iterate through all the checkboxes, saving checked ones to an array. The gallery object IDs are stored 
            // in a hidden input tag. Albums have an 'a' prefix; images have a 'm' prefix (e.g. "a322", "m999")
            CheckBox chkbx;
            HiddenField gc;
            List<string> ids = new List<string>();

            // Loop through each item in the repeater control. If an item is checked, extract the ID.
            foreach (RepeaterItem rptrItem in rptr.Items)
            {
                // Each item will have one checkbox named either chkMO (for a media object) or chkAlbum (for an album).
                chkbx = rptrItem.FindControl("chkMO") as CheckBox;

                gc = (HiddenField)rptrItem.FindControl("hdn");
                ids.Add(gc.Value);


                if (chkbx.Checked)
                {
                    // Checkbox is checked. Save media object ID to array.
                    //gc = (HiddenField)rptrItem.FindControl("hdn");

                    //ids.Add(gc.Value);
                    string s = gc.Value;
                }
            }

            // Convert the int array to an array of strings of exactly the right length.
            string[] idArray = new string[ids.Count];
            ids.CopyTo(idArray);

            return idArray;
        }

        #endregion
    }
}