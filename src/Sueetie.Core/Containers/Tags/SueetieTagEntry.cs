// -----------------------------------------------------------------------
// <copyright file="SueetieTagEntry.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a z_Sueetie_TagEntry.
    /// </summary>
    [Serializable]
    public class SueetieTagEntry
    {
        public int ContentID { get; set; }
        public int ItemID { get; set; }
        public int ContentTypeID { get; set; }
        public int UserID { get; set; }
        public string Tags { get; set; }
    }
}