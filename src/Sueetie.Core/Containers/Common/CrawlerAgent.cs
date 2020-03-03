// -----------------------------------------------------------------------
// <copyright file="CrawlerAgent.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a SuAddons_CrawlerAgent.
    /// </summary>
    [Serializable]
    public class CrawlerAgent
    {
        public int AgentID { get; set; }
        public string AgentExcerpt { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime DateEntered { get; set; }
    }
}