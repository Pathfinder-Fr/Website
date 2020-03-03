// -----------------------------------------------------------------------
// <copyright file="SueetieDownload.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a z_sueetie_vw_download.
    /// </summary>
    [Serializable]
    public class SueetieDownload
    {
        public int DownloadID { get; set; }
        public int ContentID { get; set; }
        public int ContentTypeID { get; set; }
        public string ContentTypeDescription { get; set; }
        public int ApplicationID { get; set; }
        public string ApplicationKey { get; set; }
        public int GroupID { get; set; }
        public string GroupKey { get; set; }
        public string GroupName { get; set; }
        public int MediaObjectID { get; set; }
        public string MediaObjectTitle { get; set; }
        public int AlbumID { get; set; }
        public string AlbumTitle { get; set; }
        public int GalleryID { get; set; }
        public string GalleryName { get; set; }
        public int DownloadUserID { get; set; }
        public string DownloadUserName { get; set; }
        public string DownloadDisplayName { get; set; }
        public string DownloadEmail { get; set; }
        public DateTime DownloadDateTime { get; set; }
        public bool InDownloadReport { get; set; }
        public int TotalDownloads { get; set; }
        public DateTime DateTimeLastDownload { get; set; }
    }
}