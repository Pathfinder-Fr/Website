// -----------------------------------------------------------------------
// <copyright file="SueetieContent.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// This object represents the properties and methods of a Sueetie_Content.
    /// </summary>
    [Serializable]
    [DataContract]
    public class SueetieContent
    {
        [DataMember]
        public int ContentID { get; set; }

        [DataMember]
        public int SourceID { get; set; }

        [DataMember]
        public int ContentTypeID { get; set; }

        [DataMember]
        public int ApplicationID { get; set; }

        [DataMember]
        public int UserID { get; set; }

        [DataMember]
        public string Permalink { get; set; }

        [DataMember]
        public DateTime DateTimeCreated { get; set; }

        [DataMember]
        public bool IsRestricted { get; set; }
    }
}