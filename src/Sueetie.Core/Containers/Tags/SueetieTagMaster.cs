// -----------------------------------------------------------------------
// <copyright file="SueetieTagMaster.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a z_sueetie_taglookup.
    /// </summary>
    [Serializable]
    public class SueetieTagMaster
    {
        public int TagID { get; set; }
        public int TagMasterID { get; set; }
        public int ContentID { get; set; }
        public string Tag { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public bool IsActive { get; set; }
        public int ContentTypeID { get; set; }
        public int ApplicationID { get; set; }
    }
}