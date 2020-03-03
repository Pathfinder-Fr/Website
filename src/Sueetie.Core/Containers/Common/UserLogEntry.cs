// -----------------------------------------------------------------------
// <copyright file="UserLogEntry.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a Sueetie_UserLog.
    /// </summary>
    [Serializable]
    public class UserLogEntry
    {
        public int UserLogID { get; set; }
        public int UserLogCategoryID { get; set; }
        public int ItemID { get; set; }
        public int UserID { get; set; }
        public DateTime LogDateTime { get; set; }
    }
}