// -----------------------------------------------------------------------
// <copyright file="SueetieMediaGallery.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a z_Sueetie_MediaGallery.
    /// </summary>
    [Serializable]
    public class SueetieMediaGallery
    {
        public int GalleryID { get; set; }
        public string GalleryKey { get; set; }
        public string GalleryTitle { get; set; }
        public string GalleryDescription { get; set; }
        public DateTime DateAdded { get; set; }
        public int DisplayTypeID { get; set; }
        public string DisplayTypeDescription { get; set; }
        public int ApplicationID { get; set; }
        public string ApplicationKey { get; set; }
        public string ApplicationDescription { get; set; }
        public bool IsPublic { get; set; }
        public bool IsLogged { get; set; }
        public string MediaObjectPath { get; set; }
    }
}