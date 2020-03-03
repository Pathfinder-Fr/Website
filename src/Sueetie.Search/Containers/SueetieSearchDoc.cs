using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sueetie.Core;

namespace Sueetie.Search
{
    public class SueetieSearchDoc
    {
        public int ContentID { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Tags { get; set; }
        public string DisplayTags { get; set; }

        public string App { get; set; }
        public int AppID { get; set; }
        public bool IsRestricted { get; set; }
        public DateTime PublishDate { get; set; }
        public string ContainerName { get; set; }
        public int GroupID { get; set; }
        public int ContentTypeID { get; set; }
        public string Categories { get; set; }

        public int ApplicationTypeID { get; set; }
        public string ApplicationKey { get; set; }
        public string PermaLink { get; set; }
        public string Author { get; set; }
        public string Username { get; set; }
        public string GroupKey { get; set; }
    }
}
