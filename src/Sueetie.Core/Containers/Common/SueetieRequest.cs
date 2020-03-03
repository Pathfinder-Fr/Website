// -----------------------------------------------------------------------
// <copyright file="SueetieRequest.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// This object represents the properties and methods of a z_sueetie_vw_request.
    /// </summary>
    [Serializable]
    [DataContract]
    public class SueetieRequest
    {
        [DataMember]
        public Guid LogID { get; set; }

        [DataMember]
        public int ContentID { get; set; }

        [DataMember]
        public int UserID { get; set; }

        [DataMember]
        public int PageID { get; set; }

        [DataMember]
        public DateTime RequestDateTime { get; set; }

        [DataMember]
        public string Url { get; set; }

        [DataMember]
        public string PageTitle { get; set; }

        [DataMember]
        public int ApplicationID { get; set; }

        [DataMember]
        public string RemoteIP { get; set; }

        [DataMember]
        public string UserAgent { get; set; }

        [DataMember]
        public int Count { get; set; }

        [DataMember]
        public int RecipientID { get; set; }

        [DataMember]
        public int ContactTypeID { get; set; }
    }
}