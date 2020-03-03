// -----------------------------------------------------------------------
// <copyright file="SueetieContentPagePart.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a z_sueetie_vw_contentpagepart.
    /// </summary>
    [Serializable]
    public class SueetieContentPagePart
    {
        public int ContentPartID { get; set; }
        public string ContentName { get; set; }
        public string PageKey { get; set; }
        public int ContentPageID { get; set; }
        public int ContentPageGroupID { get; set; }
        public string PageTitle { get; set; }
        public DateTime LastUpdateDateTime { get; set; }
        public int LastUpdateUserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string ContentText { get; set; }
        public int ContentID { get; set; }
        public int ContentTypeID { get; set; }
        public string Permalink { get; set; }
        public string ApplicationKey { get; set; }
        public string PageSlug { get; set; }
        public string ContentPageGroupTitle { get; set; }
    }
}