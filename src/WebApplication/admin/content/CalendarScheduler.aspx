<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CalendarScheduler.aspx.cs"
    Inherits="Sueetie.Web.AdminCalendarScheduler" %>

<%@ Register Src="../controls/adminContentNavLinks.ascx" TagName="adminAddonPackNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<asp:content id="Content1" runat="server" contentplaceholderid="cphHeader">


</asp:content>
<asp:content id="Content5" runat="server" contentplaceholderid="cphBodyTitle">
    Sueetie Event Calendar Administration
</asp:content>
<asp:content id="Content2" runat="server" contentplaceholderid="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="2" />
</asp:content>
<asp:content id="Content6" runat="server" contentplaceholderid="cphUserNavigation">
    <uc1:adminAddonPackNavLinks ID="adminAddonPackNavLinks1" runat="server" />
</asp:content>
<asp:content id="Content7" runat="server" contentplaceholderid="cphContentBody">

    <script type='text/javascript'>

        $(document).ready(function() {

            var calendarID = 2;
            var qstring = $.parseQuery();
            if (qstring.c > 0)
                calendarID = qstring.c;
            var jsonUrl = "/util/json/jsonEvents.aspx?calendarID=" + calendarID + "&editMode=1";


            var txtNew = 'Event Title:<br /><input type="text" id="eventTitle" name="eventTitle" value="" class="modalEventTitle" /><br /><br />' +
                            'Description:<br /><textarea id="eventDescription" name="eventDescription" value="" class="modalEventDescription" /><br /><br />' +
                            'Repeat Date End (optional MM/DD/YYYY):<br /><input type="text" id="eventRepeatEndDate" class="modalEventRepeatDateTime" name="eventRepeatEndDate" value="" /><br /><br />';
            var txtEdit = 'Event Title:<br /><input type="text" id="eventTitle" name="eventTitle" value="%TITLE%" class="modalEventTitle" /><br /><br />' +
                            'Description:<br /><textarea id="eventDescription" name="eventDescription" class="modalEventDescription">%DESCRIPTION%</textarea><br /><br />' +
                            'Repeat Date End (optional MM/DD/YYYY):<br /><input type="text" id="eventRepeatEndDate" class="modalEventRepeatDateTime" name="eventRepeatEndDate" value="%ENDREPEATDATE%" /><br /><br />';

            var date = new Date();
            var d = date.getDate();
            var m = date.getMonth();
            var y = date.getFullYear();

            var calendar = $('#calendar').fullCalendar({
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'month,agendaWeek,agendaDay'
                },
                selectable: true,
                selectHelper: true,
                select: function(start, end, allDay) {
                    $.prompt(txtNew, {
                        buttons: { Submit: 'submit', Cancel: 'cancel' },
                        callback: function mycallbackform(v, m, f) {
                            if (v != undefined)
                                var _title;

                            if (v == "submit") {
                                _title = f.eventTitle;
                                var _id = CreateGuid();
                                var _description = f.eventDescription;
                                var _eventEndRepeatDate = f.eventRepeatEndDate != undefined ? f.eventRepeatEndDate : '6/9/1969';

                                if (_title) {
                                    calendar.fullCalendar('renderEvent',
						            {
						                id: _id,
						                title: _title,
						                description: _description,
						                start: start,
						                end: end,
						                allDay: true,
						                url: 'na'
						            },
						            true // make the event "stick"
					            );
                                    //t(string id, string title, string description, string start, string end, string allDay, string endRepeat, int calendarID, string url, int sourceContentID)
                                    Sueetie.Web.SueetieService.AddCalendarEvent(_id, _title, _description, start, end, allDay, _eventEndRepeatDate, calendarID, null, 0, CreateCalendarEventComplete, CreateCalendarEventFailed);
                                }
                                calendar.fullCalendar('unselect');
                            }
                        }
                    });

                },
                eventClick: function(calEvent, jsEvent, view) {
                    var _title;
                    var _description;
                    var _eventEndRepeatDate;
                    var _populatedTxtEdit = txtEdit.replace('%TITLE%', calEvent.title).replace('%DESCRIPTION%', calEvent.description);

                    var _endDate = new Date(calEvent.endRepeatDate);

                    if (_endDate.getFullYear() != 1969)
                        _populatedTxtEdit = _populatedTxtEdit.replace('%ENDREPEATDATE%', calEvent.endRepeatDate);
                    else
                        _populatedTxtEdit = _populatedTxtEdit.replace('%ENDREPEATDATE%', '');


                    $.prompt(_populatedTxtEdit, {
                        buttons: { Update: 'update', Delete: 'delete', Cancel: 'cancel' },
                        callback: function mycallbackform(v, m, f) {
                            if (v != undefined) {
                                if (v == "update") {
                                    _title = f.eventTitle;
                                    _description = f.eventDescription;
                                    _eventEndRepeatDate = f.eventRepeatEndDate != undefined ? f.eventRepeatEndDate : '6/9/1969';

                                    Sueetie.Web.SueetieService.EditCalendarEvent(calEvent.id, _title, _description, calEvent.start, calEvent.end, calEvent.allDay, _eventEndRepeatDate, calendarID, 0, UpdateCalendarEventComplete, UpdateCalendarEventFailed);
                                }
                                if (v == "delete") {
                                    Sueetie.Web.SueetieService.DeleteCalendarEvent(calEvent.id, calendarID, UpdateCalendarEventComplete, UpdateCalendarEventFailed);
                                }
                                //location.reload();
                            }
                        }
                    });
                },
                eventResize: function(calEvent, jsEvent, ui, view) {
                    Sueetie.Web.SueetieService.EditCalendarEvent(calEvent.id, calEvent.title, calEvent.description, calEvent.start, calEvent.end, calEvent.allDay, calEvent.endRepeatDate, calendarID, 0, CreateCalendarEventComplete, CreateCalendarEventFailed);
                },
                eventDrop: function(calEvent, delta) {
                    Sueetie.Web.SueetieService.EditCalendarEvent(calEvent.id, calEvent.title, calEvent.description, calEvent.start, calEvent.end, calEvent.allDay, calEvent.endRepeatDate, calendarID, delta, CreateCalendarEventComplete, CreateCalendarEventFailed);
                },
                editable: true,
                events: jsonUrl
            });


        });

        function CreateGuid() {
            return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
                var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
                return v.toString(16);
            }).toUpperCase();
        }


    </script>

    <style type='text/css'>
        #loading
        {
            position: absolute;
            top: 5px;
            right: 5px;
        }
        #calendar
        {
            width: 92%;
            margin: 0 auto;
        }
    </style>
        <div class="AdminFormArea">
       <h2>
           Event Calendar Scheduler</h2>
        <div class="AdminTextTalk">
            <div class="AdminSubHeader">
            <asp:Label id="lblCalendar" runat="server" />
            </div>
                
                 <div id='calendar'>
                    </div>
        </div>
        </div>
</asp:content>
