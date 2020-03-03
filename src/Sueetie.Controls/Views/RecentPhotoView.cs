using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sueetie.Core;

namespace Sueetie.Controls
{
    public class RecentPhotoView : SueetieBaseControl
    {

        public virtual SueetieMediaObject RecentPhoto
        {
            get { return (SueetieMediaObject)(ViewState["RecentPhoto"] ?? default(SueetieMediaObject)); }
            set { ViewState["RecentPhoto"] = value; }
        }

    }
}
