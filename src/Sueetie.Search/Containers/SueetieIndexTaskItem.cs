
using System;
using System.Xml;
using System.Xml.Serialization;

namespace Sueetie.Core
{

    /// <summary>
    /// This object represents the properties and methods of a z_Sueetie_IndexCount.
    /// </summary>
    [Serializable]
    public class SueetieIndexTaskItem
    {

        public SueetieIndexTaskItem() { }

        public int TaskQueueID { get; set; }
        public int TaskTypeID { get; set; }
        public DateTime TaskStartDateTime { get; set; }
        public int BlogPosts { get; set; }
        public int ForumMessages { get; set; }
        public int WikiPages { get; set; }
        public int MediaAlbums { get; set; }
        public int MediaObjects { get; set; }
        public int ContentPages { get; set; }
        public int DocumentsIndexed { get; set; }


    }

}

