// -----------------------------------------------------------------------
// <copyright file="UserLogCategory.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a Sueetie_UserLogCategorie.
    /// </summary>
    [Serializable]
    public class UserLogCategory
    {
        public int UserLogCategoryID { get; set; }
        public string UserLogCategoryCode { get; set; }
        public string UserLogCategoryDescription { get; set; }
        public bool IsDisplayed { get; set; }
        public bool IsLocked { get; set; }
        public bool IsSyndicated { get; set; }
    }
}