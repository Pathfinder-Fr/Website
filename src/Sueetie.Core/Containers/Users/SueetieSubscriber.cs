// -----------------------------------------------------------------------
// <copyright file="SueetieSubscriber.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a z_Sueetie_Subscriber.
    /// </summary>
    [Serializable]
    public class SueetieSubscriber
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
    }
}