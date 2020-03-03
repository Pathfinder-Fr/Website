// -----------------------------------------------------------------------
// <copyright file="SueetieCalendars.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;
    using System.Collections.Generic;

    public static class SueetieCalendars
    {
        public static List<SueetieCalendarEvent> GetSueetieCalendarEventList(int calendarId)
        {
            var key = SueetieCalendarEventListCacheKey(calendarId);

            var sueetieCalendarEvents = SueetieCache.Current[key] as List<SueetieCalendarEvent>;

            if (sueetieCalendarEvents == null)
            {
                var provider = SueetieDataProvider.LoadProvider();
                sueetieCalendarEvents = provider.GetSueetieCalendarEventList(calendarId);

                // Process Repeating Events
                var sueetieRepeatingCalendarEvents = new List<SueetieCalendarEvent>();

                foreach (var sueetieCalendarEvent in sueetieCalendarEvents)
                {
                    sueetieRepeatingCalendarEvents.Add(sueetieCalendarEvent);
                    if (!DataHelper.IsMinDate(sueetieCalendarEvent.RepeatEndDate))
                    {
                        var dateEventStart = sueetieCalendarEvent.StartDateTime;
                        var dateEventEnd = sueetieCalendarEvent.EndDateTime;
                        var repeatEndDate = sueetieCalendarEvent.RepeatEndDate;
                        while (dateEventStart.Date.AddDays(7) <= repeatEndDate.Date)
                        {
                            dateEventStart = dateEventStart.AddDays(7);
                            dateEventEnd = dateEventEnd.AddDays(7);
                            var repeatingEvent = new SueetieCalendarEvent
                            {
                                EventID = sueetieCalendarEvent.EventID,
                                EventGuid = sueetieCalendarEvent.EventGuid,
                                CalendarID = sueetieCalendarEvent.CalendarID,
                                EventTitle = sueetieCalendarEvent.EventTitle,
                                EventDescription = sueetieCalendarEvent.EventDescription,
                                StartDateTime = dateEventStart,
                                EndDateTime = dateEventEnd,
                                AllDayEvent = sueetieCalendarEvent.AllDayEvent,
                                RepeatEndDate = sueetieCalendarEvent.RepeatEndDate,
                                IsActive = sueetieCalendarEvent.IsActive,
                                SourceContentID = sueetieCalendarEvent.SourceContentID,
                                ContentID = sueetieCalendarEvent.ContentID,
                                Url = sueetieCalendarEvent.Url,
                                ContentTypeID = sueetieCalendarEvent.ContentTypeID,
                                UserID = sueetieCalendarEvent.UserID,
                                UserName = sueetieCalendarEvent.UserName,
                                DisplayName = sueetieCalendarEvent.DisplayName,
                                CreatedDateTIme = sueetieCalendarEvent.CreatedDateTIme,
                                CreatedBy = sueetieCalendarEvent.CreatedBy
                            };
                            sueetieRepeatingCalendarEvents.Add(repeatingEvent);
                        }
                    }
                }

                SueetieCache.Current.Insert(key, sueetieRepeatingCalendarEvents);
                return sueetieRepeatingCalendarEvents;
            }

            return sueetieCalendarEvents;
        }

        public static string SueetieCalendarEventListCacheKey(int calendarId)
        {
            return string.Format("SueetieCalendarEventList-{0}-{1}", calendarId, SueetieConfiguration.Get().Core.SiteUniqueName);
        }

        public static void ClearSueetieCalendarEventListCache(int calendarId)
        {
            SueetieCache.Current.Remove(SueetieCalendarEventListCacheKey(calendarId));
        }


        public static List<SueetieCalendar> GetSueetieCalendarList()
        {
            var key = SueetieCalendarListCacheKey();

            var sueetieCalendars = SueetieCache.Current[key] as List<SueetieCalendar>;
            if (sueetieCalendars == null)
            {
                var provider = SueetieDataProvider.LoadProvider();
                sueetieCalendars = provider.GetSueetieCalendarList();
                SueetieCache.Current.Insert(key, sueetieCalendars);
            }

            return sueetieCalendars;
        }

        public static string SueetieCalendarListCacheKey()
        {
            return string.Format("SueetieCalendarList-{0}", SueetieConfiguration.Get().Core.SiteUniqueName);
        }

        public static void ClearSueetieCalendarListCache()
        {
            SueetieCache.Current.Remove(SueetieCalendarListCacheKey());
        }

        public static void UpdateSueetieCalendar(SueetieCalendar sueetieCalendar)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.UpdateSueetieCalendar(sueetieCalendar);
        }

        public static void CreateSueetieCalendar(SueetieCalendar sueetieCalendar)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.CreateSueetieCalendar(sueetieCalendar);
        }

        public static SueetieCalendar GetSueetieCalendar(int calendarId)
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.GetSueetieCalendar(calendarId);
        }

        public static SueetieCalendar GetCurrentCalendar()
        {
            var rawurl = SueetieContext.Current.RawUrl;
            var calendar = GetSueetieCalendarList().Find(p => p.CalendarUrl.ToLower() == rawurl.ToLower());
            return calendar;
        }

        public static SueetieCalendarEvent GetSueetieCalendarEvent(int sourceContentId, int calendarId)
        {
            return GetSueetieCalendarEvent(sourceContentId, calendarId, true);
        }

        public static SueetieCalendarEvent GetSueetieCalendarEvent(int sourceContentId, int calendarId, bool cachedCalendarList)
        {
            if (!cachedCalendarList)
                ClearSueetieCalendarEventListCache(calendarId);

            return GetSueetieCalendarEventList(calendarId).Find(p => p.SourceContentID == sourceContentId);
        }

        public static int CreateSueetieCalendarEvent(SueetieCalendarEvent sueetieCalendarEvent)
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.CreateSueetieCalendarEvent(sueetieCalendarEvent);
        }

        public static void UpdateSueetieCalendarEvent(SueetieCalendarEvent sueetieCalendarEvent)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.UpdateSueetieCalendarEvent(sueetieCalendarEvent);
        }

        public static void DeleteCalendarEvent(string calendareventGuid)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.DeleteCalendarEvent(calendareventGuid);
        }

        public static long ToUnixTimespan(DateTime date)
        {
            var tspan = date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)Math.Truncate(tspan.TotalSeconds);
        }

        public static DateTime ConvertJsonDate(string jsonDate)
        {
            var shift = int.Parse(SiteSettings.Instance.DefaultTimeZone);
            if (string.IsNullOrEmpty(jsonDate))
                return Convert.ToDateTime("6/9/1969");

            var seconds = int.Parse(jsonDate.Substring(6, 10));
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(seconds + 3600).AddMinutes(shift - (!DateTime.Now.IsDaylightSavingTime() ? 60 : 0));
        }

        public static string NAit(string url)
        {
            if (string.IsNullOrEmpty(url))
                return "na";
            return url;
        }
    }
}