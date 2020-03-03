// -----------------------------------------------------------------------
// <copyright file="SueetieContentPageGroup.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a z_sueetie_vw_contentpagegroup.
    /// </summary>
    [Serializable]
    public class SueetieContentPageGroup
    {
        public int ContentPageGroupID { get; set; }
        public int ApplicationID { get; set; }
        public string ApplicationKey { get; set; }
        public string ContentPageGroupTitle { get; set; }
        public string EditorRoles { get; set; }
        public bool IsActive { get; set; }
    }
}