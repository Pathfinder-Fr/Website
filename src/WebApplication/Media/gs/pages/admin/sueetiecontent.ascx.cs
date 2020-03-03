using System;
using System.Globalization;
using System.Web.UI.WebControls;
using GalleryServerPro.Business;
using GalleryServerPro.Web.Controller;
using Sueetie.Media;
using Sueetie.Core;
using GalleryServerPro.Business.Interfaces;
using System.Collections.Generic;

namespace GalleryServerPro.Web.gs.pages.admin
{
    public partial class sueetiecontent : Pages.AdminPage
    {
        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            this.CheckUserSecurity(SecurityActions.AdministerSite);

            if (!IsPostBack)
            {
                ConfigureControls();
                this.OkButtonText = Resources.Sueetie.Admin_Task_SueetieContent_Ok_Button_Text;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            this.AdminHeaderPlaceHolder = phAdminHeader;
            this.AdminFooterPlaceHolder = phAdminFooter;
        }

        protected override bool OnBubbleEvent(object source, EventArgs args)
        {
            //An event from the control has bubbled up.  If it's the Ok button, then run the
            //code to save the data to the database; otherwise ignore.
            Button btn = source as Button;
            if ((btn != null) && (((btn.ID == "btnOkTop") || (btn.ID == "btnOkBottom"))))
            {
                SaveSettings();

                //if (IsDoublePostBack)
                //{
                //    AppRecycleFlag = true;
                //}
            }

            return true;
        }

        #endregion

        #region Private Methods

        private void ConfigureControls()
        {
            AdminPageTitle = Resources.Sueetie.Admin_Add_Media_General_Hdr;
            this.PageTitle = Resources.Sueetie.Admin_Sueetie_Hdr_Text;


        }

        private void SaveSettings()
        {
            List<SueetieMediaAlbum> sueetieMediaAlbums = SueetieMedia.GetSueetieAlbumUpdateList(this.GalleryId);
            int j = 0;
            
            foreach (SueetieMediaAlbum _sueetieMediaAlbum in sueetieMediaAlbums)
            {
                IAlbum album = Factory.LoadAlbumInstance(_sueetieMediaAlbum.AlbumID, false, true);
                SueetieContent sueetieAlbumContent = new SueetieContent
                {
                    SourceID = _sueetieMediaAlbum.AlbumID,
                    ContentTypeID = (int)SueetieContentType.OtherAlbum,
                    ApplicationID = (int)SueetieApplications.Current.ApplicationID,
                    UserID = _sueetieMediaAlbum.SueetieUserID,
                    IsRestricted = _sueetieMediaAlbum.IsRestricted,
                    Permalink = "/" + SueetieApplications.Current.ApplicationKey + "/" + CurrentSueetieGallery.GalleryKey + ".aspx?aid=" + _sueetieMediaAlbum.AlbumID,
                };
                SueetieCommon.AddSueetieContent(sueetieAlbumContent);
                string sueetieAlbumPath = SueetieMedia.CreateSueetieAlbumPath(album.FullPhysicalPath);
                SueetieMedia.CreateSueetieAlbum(_sueetieMediaAlbum.AlbumID, sueetieAlbumPath, (int)SueetieContentType.OtherAlbum);
                j++;
            }

            List<SueetieMediaObject> sueetieMediaObjects = SueetieMedia.GetSueetieMediaUpdateList();
            int i = 0;
            foreach (SueetieMediaObject _sueetieMediaObject in sueetieMediaObjects)
            {

                int _moid = _sueetieMediaObject.MediaObjectID;
                IGalleryObject mo = Factory.LoadMediaObjectInstance(_moid);
                SueetieContent sueetieMediaContent = new SueetieContent
                {
                    SourceID = _sueetieMediaObject.MediaObjectID,
                    ContentTypeID = MediaHelper.ConvertContentType(mo.MimeType.TypeCategory),
                    ApplicationID = (int)SueetieApplications.Current.ApplicationID,
                    UserID = _sueetieMediaObject.SueetieUserID,
                    DateTimeCreated = mo.DateAdded,
                    IsRestricted = mo.IsPrivate,
                    Permalink = MediaHelper.SueetieMediaObjectUrl(_moid, this.GalleryId)
                };
                SueetieCommon.AddSueetieContent(sueetieMediaContent);
                SueetieMedia.CreateSueetieMediaObject(_sueetieMediaObject);
                i++;

            }
            SueetieMedia.ClearMediaPhotoListCache(0); // Clear Recent Photos for top level Gallery
            SueetieMedia.ClearSueetieMediaAlbumListCache(this.GalleryId);
            SueetieMedia.ClearSueetieMediaObjectListCache(this.GalleryId);

            this.wwMessage.CssClass = "wwErrorSuccess gsp_msgfriendly gsp_bold";
            this.wwMessage.ShowMessage(i.ToString() + " media objects, " + j.ToString() + " albums added to Sueetie Content Core.");
        }

        #endregion
    }
}