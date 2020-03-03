<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../masters/Calendar.Master"
    ValidateRequest="false" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphHeader" runat="server">

    <script runat="server" language="C#">

        protected void Page_Init(object sender, EventArgs e)
        {
            Page.Title = "Shared Google Community Calendar Demo";
        }
    
    </script>
    <script type='text/javascript'>

        $(document).ready(function() {

            $('#calendar').fullCalendar({

                events: $.fullCalendar.gcalFeed('http://www.google.com/calendar/feeds/adams.edu_5tqulmrvta2716n7ukmtdal3oc%40group.calendar.google.com/public/basic'),

                eventClick: function(event) {
                    // opens events in a popup window
                    window.open(event.url, 'gcalevent', 'width=700,height=600');
                    return false;
                },

                loading: function(bool) {
                    if (bool) {
                        $('#loading').show();
                    } else {
                        $('#loading').hide();
                    }
                }

            });

            $("td.fc-header-center").html('<h3>Shared Google Calendar Demo</h3>');
        });
    </script>
<style type='text/css'>

.fc-event-moreMessage
{
    display: none;
}
</style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" runat="server">
<a id="backtop">&nbsp;</a>
    <div id="CalendarMenuTopOuter">
        <div id="CalendarMenuMessage">
            See <a href="#calinfo">notes at bottom of page</a> for more information about the Shared Google Calendar
            </div>
        <div id="CalendarMenuTop">
            <a href="/calendar/default.aspx">Sueetie Community Calendar</a> | <a href="/calendar/google.aspx"
                class="current">Shared Google Calendar Demo</a> | <a href="/calendar/editdemo.aspx">
                    Editable Calendar</a>
        </div>
    </div>
    <div id="calendar">
    </div>
    <div id="CalendarDescription">
    <a id="calinfo">&nbsp;</a>
        <SUEETIE:ContentPart ID="ContentPart1" ContentName="CalendarGoogle" runat="server" />
    </div>
    <div class="backtop">
    <a href="#backtop">Back to top...</a>
    </div>
</asp:Content>
