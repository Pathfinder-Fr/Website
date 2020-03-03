#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using Sueetie.Core;


namespace Sueetie.Search
{
    public class SueetieSearchResult
    {
        public int ContentID { get; set; }
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
        public string ContainerName { get; set; }
        public float Score { get; set; }
        public string HighlightedContent { get; set; }
        public int ContentTypeID { get; set; }
        public int ApplicationTypeID { get; set; }
        
        public string ApplicationKey { get; set; }
        public string PermaLink { get; set; }
        public string Author { get; set; }
        public string DisplayTags { get; set; }

    }
}
