<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CalendarEdit.aspx.cs" Inherits="Sueetie.Web.AdminCalendarEdit"
    MasterPageFile="/themes/lollipop/masters/admin.master" %>

<%@ Register Src="../controls/adminContentNavLinks.ascx" TagName="adminAddonPackNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Sueetie Event Calendar Administration
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="2" />
</asp:Content>
<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminAddonPackNavLinks ID="adminAddonPackNavLinks1" runat="server" />
</asp:Content>
<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <h2>
            Manage Calendars and Events</h2>
        <div class="AdminTextTalk">
            <div class="AdminFormLabel">
                Select Calendar</div>
            <asp:DropDownList ID="ddlCalendars" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCalendars_OnSelectedIndexChanged"
                CssClass="BigDropDown" Width="500px" />
            <div class="SlideshowDropDownNotes">
                To enter new calendar, clear dropdown selection</div>
            <div class="AdminFormInner">
                <table width="100%">
                    <tr>
                        <td class="formLabel">
                            Title
                        </td>
                        <td class="formField">
                            <asp:TextBox ID="txtTitle" runat="server" CssClass="BigTextBox" Width="500px" />
                            <asp:RequiredFieldValidator runat="server" ErrorMessage=" *" CssClass="BigErrorMessage"
                                ControlToValidate="txtTitle" />
                        </td>
                    </tr>
                    <tr>
                        <td class="formLabel">
                            Description
                        </td>
                        <td class="formField">
                            <asp:TextBox ID="txtDescription" runat="server" CssClass="BigTextBox" TextMode="MultiLine"
                                Rows="5" Width="500px" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage=" *"
                                CssClass="BigErrorMessage" ControlToValidate="txtDescription" />
                        </td>
                    </tr>
                     <tr>
                        <td class="formLabel">
                            Calendar Url
                        </td>
                        <td class="formField">
                            <asp:TextBox ID="txtCalendarUrl" runat="server" CssClass="BigTextBox" Width="500px" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage=" *" CssClass="BigErrorMessage"
                                ControlToValidate="txtCalendarUrl" />
                                            <div class="AdminUserFieldInfo">
                Note: Full path from site root.  Ex: <b>/calendars/meetings.aspx</b></div>
                        </td>
                    </tr>
                    <tr>
                        <td class="formLabel">
                            Is Active
                        </td>
                        <td class="formField">
                            <asp:CheckBox ID="chkActive" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="TextButtonBigArea">
                <asp:Button ID="btnAddNew" runat="server" Text="Add New" CssClass="TextButtonBig"
                    OnCommand="btnAddUpdate_OnCommand" CausesValidation="true" CommandName="Add" />
                <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="TextButtonBig"
                    OnCommand="btnAddUpdate_OnCommand" CausesValidation="true" CommandName="Update" />
                <asp:Button ID="btnManage" runat="server" Text="Manage" CssClass="TextButtonBig"
                    OnClick="btnManage_OnClick" CausesValidation="false" />
            </div>
            <br />
            <br />
            <div id="ResultMessage" class="ResultsMessage">
                <asp:Label ID="lblResults" runat="server" />
            </div>
        </div>
    </div>
</asp:Content>
