// -----------------------------------------------------------------------
// <copyright file="SueetieAspnetUser.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a z_sueetie_vw_aspnetuserlist.
    /// </summary>
    [Serializable]
    public class SueetieAspnetUser
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime LastActivityDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
    }
}