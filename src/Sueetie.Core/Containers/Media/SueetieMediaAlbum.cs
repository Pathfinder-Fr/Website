// -----------------------------------------------------------------------
// <copyright file="SueetieMediaAlbum.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a z_sueetie_vw_mediaAlbum.
    /// </summary>
    [Serializable]
    public class SueetieMediaAlbum
    {
        public int AlbumID { get; set; }
        public int AlbumParentID { get; set; }
        public int ContentID { get; set; }
        public int SourceID { get; set; }
        public int ContentTypeID { get; set; }
        public int ApplicationID { get; set; }
        public string ApplicationKey { get; set; }
        public int SueetieUserID { get; set; }
        public string Permalink { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public bool IsRestricted { get; set; }
        public string AlbumTitle { get; set; }
        public string DirectoryName { get; set; }
        public string SueetieAlbumPath { get; set; }
        public string ApplicationDescription { get; set; }
        public int GroupID { get; set; }
        public string GroupKey { get; set; }
        public string GroupName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string MediaObjectUrl { get; set; }
        public int GalleryId { get; set; }
        public string GalleryName { get; set; }
        public string GalleryKey { get; set; }
        public string ContentTypeName { get; set; }
        public bool IsAlbum { get; set; }
        public string ContentTypeDescription { get; set; }
        public int SueetieAlbumID { get; set; }
        public int UserLogCategoryID { get; set; }
        public int AlbumMediaCategoryID { get; set; }
        public int ApplicationTypeID { get; set; }
        public string AlbumDescription { get; set; }
        public DateTime DateLastModified { get; set; }
        public string Tags { get; set; }
    }
}