// -----------------------------------------------------------------------
// <copyright file="SueetieFavorite.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a Sueetie_Favorite.
    /// </summary>
    [Serializable]
    public class SueetieFavorite
    {
        public int FavoriteID { get; set; }
        public int UserID { get; set; }
        public int ContentID { get; set; }
        public DateTime DateTimeCreated { get; set; }
    }
}