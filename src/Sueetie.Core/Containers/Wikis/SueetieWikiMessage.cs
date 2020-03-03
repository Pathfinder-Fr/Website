// -----------------------------------------------------------------------
// <copyright file="SueetieWikiMessage.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a z_sueetie_vw_WikiMessage.
    /// </summary>
    [Serializable]
    public class SueetieWikiMessage
    {
        public int MessageID { get; set; }
        public int PageID { get; set; }
        public string MessageQueryID { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public int UserID { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime DateTimeModified { get; set; }
        public bool IsActive { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string PageFileName { get; set; }
        public string PageTitle { get; set; }
        public string Namespace { get; set; }
        public int ContentID { get; set; }
        public int ContentTypeID { get; set; }
        public string ContentTypeName { get; set; }
        public int SourceID { get; set; }
        public int ApplicationID { get; set; }
        public int ApplicationTypeID { get; set; }
        public string ApplicationKey { get; set; }
        public int GroupID { get; set; }
        public string Permalink { get; set; }
        public bool IsRestricted { get; set; }
        public int UserLogCategoryID { get; set; }
    }
}