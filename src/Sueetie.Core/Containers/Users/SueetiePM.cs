// -----------------------------------------------------------------------
// <copyright file="SueetiePM.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    [Serializable]
    public class SueetiePM
    {
        public int PMessageID { get; set; }
        public int FromUserID { get; set; }
        public string FromUser { get; set; }
        public DateTime Created { get; set; }
        public string Subject { get; set; }
        public string MessageUrl { get; set; }
    }
}