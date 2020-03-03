// -----------------------------------------------------------------------
// <copyright file="ApplicationQuery.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a z_sueetie_AppQuery.
    /// </summary>
    [Serializable]
    public class ApplicationQuery
    {
        public int QueryID { get; set; }
        public int SueetieApplicationViewTypeID { get; set; }
        public int NumRecords { get; set; }
        public int CategoryID { get; set; }
        public int GroupID { get; set; }
        public bool IsRestricted { get; set; }
        public int SortBy { get; set; }
        public bool TruncateText { get; set; }
        public int CacheMinutes { get; set; }
    }
}