<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../masters/Calendar.Master"
    ValidateRequest="false" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphHeader" runat="server">

    <script runat="server" language="C#">

        protected void Page_Init(object sender, EventArgs e)
        {
            Page.Title = "Sueetie Community Calendar";
        }
    
    </script>


            <script type='text/javascript'>

                $(document).ready(function() {

                    var jsonUrl = "/util/json/jsonEvents.aspx?calendarID=1";

                    var calendar = $('#calendar').fullCalendar({
                        header: {
                            left: 'prev,next today',
                            center: 'title',
                            right: 'month,agendaWeek,agendaDay'
                        },
                        selectable: false,
                        selectHelper: false,
                        editable: false,
                        events: jsonUrl
                    });

                });

    </script>
    
    
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" runat="server">
    <a id="backtop">&nbsp;</a>
    <div id="CalendarMenuTopOuter">
        <div id="CalendarMenuTop">
            <a href="/calendar/default.aspx" class="current">Community Calendar</a>
            | <a href="/calendar/google.aspx">Shared Google Calendar Demo</a> | <a href="/calendar/editdemo.aspx">
                Editable Calendar</a>
        </div>
    </div>
    <div id="calendar">
    </div>
    <div class="backtop">
        <a href="#backtop">Back to top...</a>
    </div>
</asp:Content>
