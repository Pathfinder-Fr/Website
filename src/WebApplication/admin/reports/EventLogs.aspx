<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EventLogs.aspx.cs" Inherits="Sueetie.Web.AdminEventLogs" MasterPageFile="/themes/lollipop/masters/admin.master" %>
<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<%@ Register Src="../controls/adminReportsNavLinks.ascx" TagName="adminReportsNavLinks"
    TagPrefix="uc1" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">

    <script type="text/javascript">

        function ShowEventDetails(msg) {
            var decodedMsg = DecodeMsg(msg);
            var cleanMsg = CleanMsg(decodedMsg);
            $.modal('<div class=\"InnerModal\">' + cleanMsg + '</div><div class=\"ModalCloseArea\"><input type=\"button\" value=\"Close\" class=\"simplemodal-close\" /></div>');
        }
        function DecodeMsg(s) {
            var o = s; var binVal, t; var r = /(%[^%]{2})/;
            while ((m = r.exec(o)) != null && m.length > 1 && m[1] != '') {
                b = parseInt(m[1].substr(1), 16);
                t = String.fromCharCode(b); o = o.replace(m[1], t);
            } return o;
        }
        function CleanMsg(s) {
            var o = s; var binVal, t; var r = /(\+)/;
            while ((m = r.exec(o)) != null && m.length > 1 && m[1] != '') {
                b = parseInt(m[1].substr(1), 16);
                t = String.fromCharCode(b); o = o.replace(m[1], '\n');
            } return o;
        }
    </script>

       <script type="text/javascript">

           function pageLoad() {
               (function ($) {
                   var qstring = $.parseQuery();
                   if (qstring.r > 0)
                       jAlert('Event Log Cleared!','Sueetie Alert',null);
               })(jQuery);
           }

    </script>

</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Event Logs
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="4" />
</asp:Content>
<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminReportsNavLinks ID="adminReportsNavLinks1" runat="server" />
</asp:Content>
<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <div class="Activities">
            <div class="AdminTextTalk">
                <h2>
                    <%= Sueetie.Core.SiteSettings.Instance.SiteName %> Event Log</h2>
                <div class="AdminFormDescription">
                    <p>
                       Below is the complete event log to date. Select EventType or click on Category or AppKey to filter. Click on Message to view full message.  You have <strong><%= EventLogItems %></strong> records in your EventLog Table. <asp:LinkButton ID="lbtnClearLog" runat="server" OnClick="lbtnClearLog_OnClick">Click here</asp:LinkButton> to clear.
                    </p>
                    <div class="EventSelect">
                    Event Type: <asp:DropDownList ID="ddlSiteLogTypeIDs" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSiteLogTypeIDs_OnSelectedIndexChanged" />
                </div>
                </div>

            </div>
            <div class="TopGridButtonRight">
            <asp:Button ID="btnRefresh" runat="server" Text="All Events" CssClass="TopGridButton" OnClick="btnRefresh_OnClick" />
            </div>
        </div>
    <asp:GridView ID="UsersGridView" CssClass="rptgridviewMain" runat="server" AutoGenerateColumns="False"
        DataKeyNames="SiteLogID" EmptyDataText="No records found." AllowSorting="True">
        <RowStyle CssClass="gridRowStyle" />
        <SelectedRowStyle CssClass="gridrowSelectedBG" />
        <HeaderStyle CssClass="gridheaderBG" />
        <AlternatingRowStyle CssClass="gridAlternateRowStyle" />
        <PagerStyle CssClass="membersGridViewPager2" BorderWidth="0px" />
        <Columns>
            <asp:TemplateField>
                <HeaderStyle CssClass="gridheaderBG" Width="1px" />
                <ItemStyle CssClass="gridheaderBG" Width="1px" />
            </asp:TemplateField>
  <asp:TemplateField HeaderText="ID" ItemStyle-CssClass="rptColID">
                <ItemTemplate>
                        <%# Eval("SiteLogID")%>
                </ItemTemplate>
            </asp:TemplateField>            
            <asp:TemplateField HeaderText="EventType" ItemStyle-CssClass="rptColCode">
                <ItemTemplate>
                        <%# Eval("SiteLogTypeCode") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Category" ItemStyle-CssClass="rptColCategory">
                <ItemTemplate>
                        <asp:LinkButton ID="lbSiteLogCategoryID" CommandArgument='<%#Eval("SiteLogCategoryID")%>' runat="server" OnCommand="lbSiteLogCategoryID_OnCommand"><%#Eval("SiteLogCategoryCode")%></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
               <asp:TemplateField HeaderText="AppKey" ItemStyle-CssClass="rptColAppKey">
                <ItemTemplate>
                        <asp:LinkButton ID="lbApplicationID" CommandArgument='<%#Eval("ApplicationID")%>' runat="server" OnCommand="lbApplicationID_OnCommand"><%#Eval("ApplicationKey")%></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
              <asp:TemplateField HeaderText="Message" ItemStyle-CssClass="rptColDescription">
                <ItemTemplate>
                <a href="javascript:void(0);" onclick="ShowEventDetails('<%# System.Web.HttpUtility.UrlEncode(Eval("Message") as string) %>');return false;">
                        <%# Sueetie.Core.DataHelper.TruncateText(Eval("Message") as string, 180) %></a>
                </ItemTemplate>
            </asp:TemplateField>
              <asp:TemplateField HeaderText="DateTime" ItemStyle-CssClass="rptColDate">
                <ItemTemplate>
                        <%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "LogDateTime")).ToString("MM/dd/yy HH:mm:ss") %>
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>
    </asp:GridView>

    <div class="membersGridViewPager">
        <asp:LinkButton ID="lnkFirst" runat="server" OnClick="lnkFirst_Click">&lt;&lt; First</asp:LinkButton>
        <asp:LinkButton ID="lnkPrev" runat="server" OnClick="lnkPrev_Click">&lt; Prev</asp:LinkButton>
        <asp:LinkButton ID="lnkNext" runat="server" OnClick="lnkNext_Click">Next &gt;</asp:LinkButton>
        <asp:LinkButton ID="lnkLast" runat="server" OnClick="lnkLast_Click">Last &gt;&gt;</asp:LinkButton>
    </div>
 
   </div>
</asp:Content>
