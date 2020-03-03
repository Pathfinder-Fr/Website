// -----------------------------------------------------------------------
// <copyright file="SiteLogEntry.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a z_sueetie_sitelogentry.
    /// </summary>
    [Serializable]
    public class SiteLogEntry
    {
        public int SiteLogID { get; set; }
        public int SiteLogTypeID { get; set; }
        public string SiteLogTypeCode { get; set; }
        public int SiteLogCategoryID { get; set; }
        public int ApplicationID { get; set; }
        public string Message { get; set; }
        public DateTime LogDateTime { get; set; }
        public string ApplicationKey { get; set; }
        public string SiteLogCategoryCode { get; set; }
    }
}