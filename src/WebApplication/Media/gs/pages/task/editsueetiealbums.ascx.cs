using System;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.Web.Controller;
using System.Collections.Generic;
using Sueetie.Core;
using Sueetie.Media;
using Sueetie.Controls;

namespace GalleryServerPro.Web.gs.pages.task
{
    public partial class editsueetiealbums : Pages.TaskPage
    {
        #region Properties

        TagControl tagControl;
        CalendarControl calendarControl;

        #endregion

        #region Event Handlers

        protected void Page_Init(object sender, EventArgs e)
        {
            this.TaskHeaderPlaceHolder = phTaskHeader;
            this.TaskFooterPlaceHolder = phTaskFooter;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.CheckUserSecurity(SecurityActions.EditMediaObject);

            if (!IsPostBack)
            {
                ConfigureControls();
            }
        }

        protected override bool OnBubbleEvent(object source, EventArgs args)
        {
            //An event from a control has bubbled up.  If it's the Ok button, then run the
            //code to synchronize; otherwise ignore.
            Button btn = source as Button;
            if ((btn != null) && (((btn.ID == "btnOkTop") || (btn.ID == "btnOkBottom"))))
            {
                Message msg = btnOkClicked();

                if (msg == Message.None)
                    this.RedirectToAlbumViewPage();
                else
                    this.RedirectToAlbumViewPage("msg={0}", ((int)msg).ToString(CultureInfo.InvariantCulture));
            }

            return true;
        }


        protected void rptr_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                SueetieMediaObject sueetieMediaObject = ((SueetieMediaObject)e.Item.DataItem);
                SueetieMediaAlbum _album = SueetieMedia.GetSueetieMediaAlbum(CurrentSueetieGalleryID, sueetieMediaObject.MediaObjectID);
                PlaceHolder phTagsControl = (PlaceHolder)e.Item.FindControl("phTagsControl");
                tagControl = new TagControl();
                tagControl.TagSueetieMediaAlbum = _album;
                phTagsControl.Controls.Add(tagControl);

                PlaceHolder phCalendarControl = (PlaceHolder)e.Item.FindControl("phCalendarControl");
                calendarControl = new CalendarControl();
                calendarControl.CalendarSueetieMediaAlbum = _album;
                phCalendarControl.Controls.Add(calendarControl);

            }
        }

        #endregion

        #region Private Methods

        private void ConfigureControls()
        {
            this.TaskHeaderText = Resources.GalleryServerPro.Task_Edit_Captions_Header_Text;
            this.TaskBodyText = Resources.GalleryServerPro.Task_Edit_Captions_Body_Text;
            this.OkButtonText = SueetieLocalizer.GetString("process_updates", "MediaGallery.xml");
            this.OkButtonToolTip = Resources.GalleryServerPro.Task_Edit_Captions_Ok_Button_Tooltip;

            this.PageTitle = Resources.GalleryServerPro.Task_Edit_Captions_Page_Title;

            // Sueetie Modified - Converting GSP galleryObjects into SueetieGalleryObjects
            SueetieConfiguration config = SueetieConfiguration.Get();
            List<SueetieMediaObject> sueetieMediaObjects = new List<SueetieMediaObject>();

            IGalleryObjectCollection albumChildren = this.GetAlbum().GetChildGalleryObjects(GalleryObjectType.Album, true);

            foreach (IGalleryObject _galleryObject in albumChildren)
            {

                SueetieMediaAlbum _album = SueetieMedia.GetSueetieMediaAlbum(CurrentSueetieGalleryID, _galleryObject.Id);
                sueetieMediaObjects.Insert(0,new SueetieMediaObject
                {
                    MediaObjectID = _galleryObject.Id,
                    MediaObjectTitle = _galleryObject.Title,
                    AlbumID = _galleryObject.Id,
                    IsAlbum = true,
                    MediaObjectUrl = String.Concat(Util.GetUrl(PageId.album, "aid={0}", _galleryObject.Id)),
                    MediaObjectDescription = _album.AlbumDescription,
                    DisplayName = _album.DisplayName,
                    ThumbnailHeight = config.Media.ThumbnailHeight,
                    ThumbnailWidth = config.Media.ThumbnailWidth
                });

            }

            if (albumChildren.Count > 0)
            {
                const int textareaWidthBuffer = 30; // Extra width padding to allow room for the caption.
                const int textareaHeightBuffer = 72; // Extra height padding to allow room for the caption.
                SetThumbnailCssStyle(albumChildren, textareaWidthBuffer, textareaHeightBuffer);

                rptr.DataSource = sueetieMediaObjects;
                rptr.DataBind();
            }
            else
            {
                this.RedirectToAlbumViewPage("msg={0}", ((int)Message.CannotEditCaptionsNoEditableObjectsExistInAlbum).ToString(CultureInfo.InvariantCulture));
            }
        }

        private Message btnOkClicked()
        {
            return saveCaptions();
        }

        private Message saveCaptions()
        {
            // Iterate through all the textboxes, saving any captions that have changed.
            // The media object IDs are stored in a hidden input tag.
            HtmlInputText ta;
            HtmlTextArea tdesc;
            HtmlInputHidden gc;
            string previousTitle, previousDescription;
            Message msg = Message.None;

            if (!IsUserAuthorized(SecurityActions.EditMediaObject))
                return msg;

            try
            {
                HelperFunctions.BeginTransaction();

                foreach (RepeaterItem rptrItem in rptr.Items)
                {
                    ta = (HtmlInputText)rptrItem.Controls[1]; // The <input TEXT> tag
                    tdesc = (HtmlTextArea)rptrItem.Controls[3]; // The <TEXTAREA> tag
                    gc = (HtmlInputHidden)rptrItem.Controls[5]; // The hidden <input> tag


                    SueetieMediaAlbum _sueetieMediaAlbum = SueetieMedia.GetSueetieMediaAlbum(CurrentSueetieGalleryID, Convert.ToInt32(gc.Value));
                    previousDescription = _sueetieMediaAlbum.AlbumDescription;
                    previousTitle = _sueetieMediaAlbum.AlbumTitle;
                    _sueetieMediaAlbum.AlbumTitle = Util.HtmlDecode(ta.Value);
                    _sueetieMediaAlbum.AlbumDescription = tdesc.Value;

                    if (_sueetieMediaAlbum.AlbumTitle != previousTitle || _sueetieMediaAlbum.AlbumDescription != previousDescription)
                        SueetieMedia.UpdateSueetieMediaAlbum(_sueetieMediaAlbum);

                }
                HelperFunctions.CommitTransaction();
            }
            catch
            {
                HelperFunctions.RollbackTransaction();
                throw;
            }

            HelperFunctions.PurgeCache();
            SueetieMedia.ClearSueetieMediaAlbumListCache(CurrentSueetieGalleryID);
            return msg;
        }

        #endregion
    }
}