using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Controls;
using Sueetie.Core;

namespace Sueetie.Controls
{
    public class RecentPhotoListWebControl : SueetieBaseControl
    {

        protected PlaceHolder phRecentPhotos;

        public RecentPhotoListWebControl()
        {
            NumRecords = 12;
            UserID = -2;
            GroupID = -1;
            ApplicationID = -1;
            IsRestricted = true;
            ViewName = "RecentPhotoView";
            CacheMinutes = 5;
        }

        public int NumRecords { get; set; }
        public int UserID { get; set; }
        public int GroupID { get; set; }
        public int ApplicationID { get; set; }
        public bool IsRestricted { get; set; }
        public string ViewName { get; set; }
        public int CacheMinutes { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

            string path = this.ViewName.ControlPath();

            ContentQuery contentQuery = new ContentQuery
            {
                NumRecords = this.NumRecords,
                UserID = this.UserID,
                ContentTypeID = (int)SueetieContentType.MediaImage,
                GroupID = this.GroupID,
                ApplicationID = this.ApplicationID,
                IsRestricted = this.IsRestricted,
                TruncateText = false,
                SueetieContentViewTypeID = (int)SueetieContentViewType.RecentPhotos,
                CacheMinutes = this.CacheMinutes
            };

            List<SueetieMediaObject> recentPhotos = SueetieMedia.GetSueetieMediaObjectList(contentQuery);

            foreach (SueetieMediaObject photo in recentPhotos)
            {
                Sueetie.Controls.RecentPhotoView control = (Sueetie.Controls.RecentPhotoView)LoadControl(path);
                control.RecentPhoto = photo;
                phRecentPhotos.Controls.Add(control);
            }

        }

    }
}