// -----------------------------------------------------------------------
// <copyright file="SueetieBannedIP.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a Sueetie_UserBanned.
    /// </summary>
    [Serializable]
    public class SueetieBannedIP
    {
        public int BannedID { get; set; }
        public string Mask { get; set; }
        public DateTime BannedDateTime { get; set; }
    }
}