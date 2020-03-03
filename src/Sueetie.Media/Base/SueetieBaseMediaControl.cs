using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Configuration;
using System.Web.Caching;
using System.Reflection;
using System.Collections.Specialized;
using System.Web;

using Sueetie.Core;


namespace Sueetie.Media
{
    public class SueetieBaseMediaControl : SueetieBaseControl
    {
        public int CurrentSueetieGalleryID
        {
            get
            {
                Uri _uri = HttpContext.Current.Request.Url;
                string _groupElement = _uri.Segments.ElementAt(2).Replace(".aspx", string.Empty);
                SueetieMediaGallery _sueetieMediaGallery = SueetieMedia.GetSueetieMediaGallery(_groupElement.ToLower());
                if (_sueetieMediaGallery != null)
                    return _sueetieMediaGallery.GalleryID;
                else
                    return 1;
            }
        }

        public SueetieMediaGallery CurrentSueetieGallery
        {
            get
            {
                Uri _uri = HttpContext.Current.Request.Url;
                string _groupElement = _uri.Segments.ElementAt(2).Replace(".aspx", string.Empty);
                SueetieMediaGallery _sueetieMediaGallery = SueetieMedia.GetSueetieMediaGallery(_groupElement.ToLower());
                if (_sueetieMediaGallery != null)
                    return _sueetieMediaGallery;
                else
                    return SueetieMedia.GetSueetieMediaGallery(1);
            }
        }
        public SueetieMediaObject CurrentSueetieMediaObject
        {
            get
            {
                int moid = DataHelper.GetIntFromQueryString("moid", -1);
                if (moid > 0)
                {
                    return SueetieMedia.GetSueetieMediaObject(this.CurrentSueetieGalleryID, moid);
                }
                else
                    return null;
            }

        }

        public SueetieMediaAlbum CurrentSueetieMediaAlbum
        {
            get
            {
                int aid = DataHelper.GetIntFromQueryString("aid",-1);

                if (aid > 0)
                {
                    return SueetieMedia.GetSueetieMediaAlbum(this.CurrentSueetieGalleryID, aid);
                }
                else if (CurrentSueetieMediaObject != null)
                {
                    aid = CurrentSueetieMediaObject.AlbumID;
                    return SueetieMedia.GetSueetieMediaAlbum(this.CurrentSueetieGalleryID, aid);
                }
                else
                    return null;
            }

        }
    }
}
