<%@ WebHandler Language="C#" Class="Sueetie.Web.PhotoDisplay" %>

using System;
using System.Web;
using Sueetie.Core;
using Sueetie.AddonPack;

namespace Sueetie.Web
{
    public class PhotoDisplay : IHttpHandler
    {
        public const string QueryStringFullSize = "full";
        public const string QueryStringMediumSize = "medium";

        public void ProcessRequest(HttpContext context)
        {

            HttpResponse Response = context.Response;
            HttpRequest Request = context.Request;

            int photoId = 0;
            string photoIdQs = Request.QueryString["pid"];
            SueetiePhotoType _photoType = (SueetiePhotoType)DataHelper.GetIntFromQueryString("t", -1);

            if (photoIdQs != null && Int32.TryParse(photoIdQs, out photoId))
            {
                string sizeQs = Request.QueryString["size"];
                SueetiePhotoSize size = SueetiePhotoSize.Small;
                if (sizeQs != null)
                {
                    if (sizeQs.Equals(QueryStringFullSize))
                        size = SueetiePhotoSize.Full;
                    else if (sizeQs.Equals(QueryStringMediumSize))
                        size = SueetiePhotoSize.Medium;
                }


                string url = ImageHelper.GetFilePath(photoId, true, size, _photoType);
                Response.Redirect(url, true);

            }
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

    }
}