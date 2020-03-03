// -----------------------------------------------------------------------
// <copyright file="SueetieWikiPage.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a z_sueetie_vw_WikiPage.
    /// </summary>
    [Serializable]
    public class SueetieWikiPage
    {
        public int PageID { get; set; }
        public string PageFileName { get; set; }
        public string PageTitle { get; set; }
        public string Keywords { get; set; }
        public string Abstract { get; set; }
        public string Namespace { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime DateTimeModified { get; set; }
        public int UserID { get; set; }
        public int ApplicationID { get; set; }
        public int ApplicationTypeID { get; set; }
        public int ContentID { get; set; }
        public int ContentTypeID { get; set; }
        public string Permalink { get; set; }
        public bool IsRestricted { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string ApplicationKey { get; set; }
        public string ApplicationName { get; set; }
        public int GroupID { get; set; }
        public string GroupKey { get; set; }
        public string GroupName { get; set; }
        public string Categories { get; set; }
        public bool Active { get; set; }
        public string PageContent { get; set; }
        public string UserName { get; set; }
        public string Tags { get; set; }
    }
}