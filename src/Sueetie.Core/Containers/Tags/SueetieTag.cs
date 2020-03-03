// -----------------------------------------------------------------------
// <copyright file="SueetieTag.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// This object represents the properties and methods of a z_sueetie_CloudTag.
    /// </summary>
    [Serializable]
    [DataContract]
    public class SueetieTag
    {
        [DataMember]
        public int TagMasterID { get; set; }

        [DataMember]
        public string Tag { get; set; }

        [DataMember]
        public int TagCount { get; set; }

        [DataMember]
        public string TagPlus { get; set; }

        [DataMember]
        public string WeightedClass { get; set; }
    }
}