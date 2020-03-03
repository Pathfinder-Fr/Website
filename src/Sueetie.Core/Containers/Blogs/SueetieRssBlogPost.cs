// -----------------------------------------------------------------------
// <copyright file="SueetieRssBlogPost.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a z_Sueetie_RssItem.
    /// </summary>
    [Serializable]
    public class SueetieRssBlogPost
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Link { get; set; }
        public string Author { get; set; }
        public string CommentsUrl { get; set; }
        public string ItemGuid { get; set; }
        public DateTime PubDate { get; set; }
        public string Category { get; set; }
        public string Publisher { get; set; }
        public string CommentCount { get; set; }
        public string CommentRSS { get; set; }
        public string Excerpt { get; set; }
    }
}