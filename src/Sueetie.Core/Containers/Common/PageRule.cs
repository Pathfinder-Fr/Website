// -----------------------------------------------------------------------
// <copyright file="PageRule.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a SuAnalytics_PageRule.
    /// </summary>
    [Serializable]
    public class PageRule
    {
        public int PageRuleID { get; set; }
        public string UrlExcerpt { get; set; }
        public string UrlFinal { get; set; }
        public string PageTitle { get; set; }
        public bool IsEqual { get; set; }
    }
}