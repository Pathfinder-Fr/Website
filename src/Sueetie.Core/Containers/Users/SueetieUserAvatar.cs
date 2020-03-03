// -----------------------------------------------------------------------
// <copyright file="SueetieUserAvatar.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a Sueetie_UserAvatar.
    /// </summary>
    [Serializable]
    public class SueetieUserAvatar
    {
        public int UserID { get; set; }
        public byte[] AvatarImage { get; set; }
        public string AvatarImageType { get; set; }
    }
}