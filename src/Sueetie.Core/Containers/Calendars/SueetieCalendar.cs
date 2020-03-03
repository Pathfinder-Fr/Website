// -----------------------------------------------------------------------
// <copyright file="SueetieCalendar.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a Sueetie_Calendar.
    /// </summary>
    [Serializable]
    public class SueetieCalendar
    {
        public int CalendarID { get; set; }
        public string CalendarTitle { get; set; }
        public string CalendarDescription { get; set; }
        public string CalendarUrl { get; set; }
        public bool IsActive { get; set; }
    }
}