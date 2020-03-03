// -----------------------------------------------------------------------
// <copyright file="SueetieCalendarEvent.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a z_sueetie_vw_calendarevent.
    /// </summary>
    [Serializable]
    public class SueetieCalendarEvent
    {
        public int EventID { get; set; }
        public Guid EventGuid { get; set; }
        public int CalendarID { get; set; }
        public string EventTitle { get; set; }
        public string EventDescription { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public bool AllDayEvent { get; set; }
        public DateTime RepeatEndDate { get; set; }
        public bool IsActive { get; set; }
        public int SourceContentID { get; set; }
        public int ContentID { get; set; }
        public int ContentTypeID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public DateTime CreatedDateTIme { get; set; }
        public int CreatedBy { get; set; }
        public string Url { get; set; }
        public string CalendarTitle { get; set; }
        public string CalendarDescription { get; set; }
        public string CalendarUrl { get; set; }
    }
}