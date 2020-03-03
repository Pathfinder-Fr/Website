// -----------------------------------------------------------------------
// <copyright file="FullCalendarEvent.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a Sueetie_CalendarEvent.
    /// </summary>
    [Serializable]
    public class FullCalendarEvent
    {
        public Guid id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string moreMessage { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public bool allDay { get; set; }
        public string url { get; set; }
        public int sourceContentID { get; set; }
        public string endRepeatDate { get; set; }
    }
}