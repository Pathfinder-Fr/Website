// -----------------------------------------------------------------------
// <copyright file="SueetieBlogSpam.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a Sueetie_beSpam.
    /// </summary>
    [Serializable]
    public class SueetieBlogSpam
    {
        public int SpamID { get; set; }
        public int SueetiePostID { get; set; }
        public DateTime SpamDateTime { get; set; }
        public string IP { get; set; }
        public string SpamInfo { get; set; }
    }
}