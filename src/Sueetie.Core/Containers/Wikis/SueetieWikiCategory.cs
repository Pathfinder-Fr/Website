// -----------------------------------------------------------------------
// <copyright file="SueetieWikiCategory.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a z_Sueetie_WikiCategorie.
    /// </summary>
    [Serializable]
    public class SueetieWikiCategory
    {
        public int CategoryID { get; set; }
        public int PageID { get; set; }
        public string Category { get; set; }
    }
}