using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Sueetie.Core;


public partial class jsonEvents : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        List<FullCalendarEvent> fullCalendarEvents = GetFullCalendarEvents();
        Response.Write(JsonConvert.SerializeObject(fullCalendarEvents));
    }

    public List<FullCalendarEvent> GetFullCalendarEvents()
    {
        int _calendarID = DataHelper.GetIntFromQueryString("calendarID", 1);
        int _editMode = DataHelper.GetIntFromQueryString("editMode", 0);

        List<SueetieCalendarEvent> _sueetieCalendarEvents = SueetieCalendars.GetSueetieCalendarEventList(_calendarID);
        
        List<FullCalendarEvent> _fullCalendarEvents = new List<FullCalendarEvent>();
        foreach (SueetieCalendarEvent _sueetieCalendarEvent in _sueetieCalendarEvents)
        {

            FullCalendarEvent _fullCalendarEvent = new FullCalendarEvent
            {
                id = _sueetieCalendarEvent.EventGuid,
                start =  _sueetieCalendarEvent.StartDateTime.ToString("MM/dd/yyyy HH:mm:ss"),
                end =  _sueetieCalendarEvent.EndDateTime.ToString("MM/dd/yyyy HH:mm:ss"),
                description = _sueetieCalendarEvent.EventDescription,
                moreMessage = SueetieLocalizer.GetString("click_for_more"),
                allDay = _sueetieCalendarEvent.AllDayEvent,
                title = _sueetieCalendarEvent.EventTitle,
                url = string.IsNullOrEmpty(_sueetieCalendarEvent.Url) || _editMode > 0 ? "na" : _sueetieCalendarEvent.Url,
                endRepeatDate = _sueetieCalendarEvent.RepeatEndDate.ToShortDateString(),
                sourceContentID = _sueetieCalendarEvent.SourceContentID
            };
            _fullCalendarEvents.Add(_fullCalendarEvent);
        }
        return _fullCalendarEvents;
    }
}

