// -----------------------------------------------------------------------
// <copyright file="UserContent.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// This object represents the properties and methods of a z_sueetie_UserContent.
    /// </summary>
    [Serializable]
    [DataContract]
    public class UserContent
    {
        [DataMember]
        public int UserID { get; set; }

        [DataMember]
        public int ContentID { get; set; }

        [DataMember]
        public int ContentTypeID { get; set; }

        [DataMember]
        public int ApplicationID { get; set; }

        [DataMember]
        public int ApplicationTypeID { get; set; }

        [DataMember]
        public int GroupID { get; set; }

        [DataMember]
        public bool IsRestricted { get; set; }
    }
}