// -----------------------------------------------------------------------
// <copyright file="ContentTypeDescription.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a Sueetie_ContentType.
    /// </summary>
    [Serializable]
    public class ContentTypeDescription
    {
        public int ContentTypeID { get; set; }
        public string ContentTypeName { get; set; }
        public string Description { get; set; }
        public bool IsAlbum { get; set; }
        public int UserLogCategoryID { get; set; }
        public int AlbumMediaCategoryID { get; set; }
    }
}