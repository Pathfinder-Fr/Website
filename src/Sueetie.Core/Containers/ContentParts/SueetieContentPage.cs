// -----------------------------------------------------------------------
// <copyright file="SueetieContentPage.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a z_sueetie_vw_ContentPage.
    /// </summary>
    [Serializable]
    public class SueetieContentPage
    {
        public int ContentPageID { get; set; }
        public int ContentPageGroupID { get; set; }
        public string PageSlug { get; set; }
        public string PageTitle { get; set; }
        public string PageDescription { get; set; }
        public string ReaderRoles { get; set; }
        public DateTime LastUpdateDateTime { get; set; }
        public int LastUpdateUserID { get; set; }
        public string PageKey { get; set; }
        public string EditorRoles { get; set; }
        public bool IsPublished { get; set; }
        public int DisplayOrder { get; set; }
        public int ContentTypeID { get; set; }
        public string Permalink { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public bool IsRestricted { get; set; }
        public int ContentID { get; set; }
        public int ApplicationID { get; set; }
        public string ApplicationKey { get; set; }
        public string ContentPageGroupTitle { get; set; }
        public string Tags { get; set; }
        public string SearchBody { get; set; }
        public string ApplicationName { get; set; }
        public int ApplicationTypeID { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
    }
}