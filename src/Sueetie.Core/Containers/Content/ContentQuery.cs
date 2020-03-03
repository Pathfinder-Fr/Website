// -----------------------------------------------------------------------
// <copyright file="ContentQuery.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a z_sueetie_Query.
    /// </summary>
    [Serializable]
    public class ContentQuery
    {
        public int SueetieContentViewTypeID { get; set; }
        public int NumRecords { get; set; }
        public int ContentTypeID { get; set; }
        public int GroupID { get; set; }
        public int ApplicationID { get; set; }
        public int SourceID { get; set; }
        public int UserID { get; set; }
        public bool IsRestricted { get; set; }
        public bool TruncateText { get; set; }
        public int CacheMinutes { get; set; }
        public int SortBy { get; set; }
        public string FeedUrl { get; set; }
    }
}