// -----------------------------------------------------------------------
// <copyright file="SueetieMediaDirectory.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a z_Sueetie_vw_MediaDirectorie.
    /// </summary>
    [Serializable]
    public class SueetieMediaDirectory
    {
        public int MediaObjectId { get; set; }
        public int ContentID { get; set; }
        public int GalleryId { get; set; }
        public int AlbumId { get; set; }
        public int AlbumParentId { get; set; }
        public string DirectoryName { get; set; }
        public string SueetieAlbumPath { get; set; }
        public string ThumbnailFilename { get; set; }
        public string OptimizedFilename { get; set; }
        public string OriginalFilename { get; set; }
        public int ContentTypeID { get; set; }
        public int ApplicationID { get; set; }
        public string ApplicationKey { get; set; }
    }
}