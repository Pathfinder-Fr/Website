using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sueetie.Core;

namespace Sueetie.Controls
{
    public class ForumTopicView : SueetieBaseControl
    {

        public virtual SueetieForumTopic Topic
        {
            get { return (SueetieForumTopic)(ViewState["Topic"] ?? default(SueetieForumTopic)); }
            set { ViewState["Topic"] = value; }
        }

    }
}
