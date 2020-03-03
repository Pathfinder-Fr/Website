// -----------------------------------------------------------------------
// <copyright file="SueetieNoLog.cs" company="Pathfinder-fr.org">
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
    public class SueetieNoLog
    {
        public string Name { get; set; }
        public string UniquePathExcerpt { get; set; }
    }
}