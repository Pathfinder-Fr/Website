using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;
using Sueetie.Core;

namespace Sueetie.Media
{
    public static class MediaHelper
    {
        public static string SueetieMediaObjectUrl(int mediaObjectID, int galleryID)
        {
            return "/" + SueetieApplications.Current.ApplicationKey + "/gs/handler/getmediaobject.ashx?moid=" + mediaObjectID + "&dt={0}&g=" + galleryID;
        }

        public static string PopulateMediaObjectUrl(string permalink, SueetieImageDisplayType displayType)
        {
            return string.Format(permalink, displayType);
        }

        public static int ConvertContentType(MimeTypeCategory _mimeTypeCategory)
        {
            SueetieContentType sueetieContentType = new SueetieContentType();
            switch (_mimeTypeCategory)
            {
                case MimeTypeCategory.NotSet:
                    sueetieContentType = SueetieContentType.MediaOther;
                    break;
                case MimeTypeCategory.Other:
                    sueetieContentType = SueetieContentType.MediaDocument;
                    break;
                case MimeTypeCategory.Image:
                    sueetieContentType = SueetieContentType.MediaImage;
                    break;
                case MimeTypeCategory.Video:
                    sueetieContentType = SueetieContentType.MediaVideo;
                    break;
                case MimeTypeCategory.Audio:
                    sueetieContentType = SueetieContentType.MediaAudioFile;
                    break;
                default:
                    sueetieContentType = SueetieContentType.MediaOther;
                    break;
            }
            return (int)sueetieContentType;
        }

        public static SueetieMediaObject PopulateMediaObject(SueetieMediaObject _sueetieMediaObject, IGalleryObject galleryObject)
        {
            _sueetieMediaObject.IsImage = galleryObject is GalleryServerPro.Business.Image;
            if (_sueetieMediaObject.IsImage)
            {
                _sueetieMediaObject.ThumbnailHeight = galleryObject.Thumbnail.Height;
                _sueetieMediaObject.ThumbnailWidth = galleryObject.Thumbnail.Width;
            }
            else
            {
                _sueetieMediaObject.ThumbnailWidth = SueetieConfiguration.Get().Media.ThumbnailWidth;
                _sueetieMediaObject.ThumbnailHeight = SueetieConfiguration.Get().Media.ThumbnailHeight;
            }
            return _sueetieMediaObject;
        }

    }
}
