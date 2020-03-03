// -----------------------------------------------------------------------
// <copyright file="SueetieUserProfile.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;
    using System.Web.Profile;

    /// <summary>
    /// This object represents the properties and methods of a z_sueetie_vw_UserProfileData.
    /// </summary>
    [Serializable]
    public class SueetieUserProfile : ProfileBase
    {
        public string DisplayName { get; set; }
        public string Gender { get; set; }
        public string Country { get; set; }
        public string Occupation { get; set; }
        public string Website { get; set; }
        public string TwitterName { get; set; }
        public bool Newsletter { get; set; }
    }
}