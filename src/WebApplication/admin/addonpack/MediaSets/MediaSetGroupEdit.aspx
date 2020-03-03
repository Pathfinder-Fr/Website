<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.AddonPack.Pages.MediaSetGroupPage" %>

<%@ Register Src="/admin/controls/adminAddonPackNavLinks.ascx" TagName="adminAddonPackNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="/admin/controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<asp:content id="Content1" contentplaceholderid="cphHeader" runat="Server" />
<asp:content id="Content5" runat="server" contentplaceholderid="cphBodyTitle">
    Manage Media Set Groups
</asp:content>
<asp:content id="Content2" runat="server" contentplaceholderid="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="5" />
</asp:content>
<asp:content id="Content6" runat="server" contentplaceholderid="cphUserNavigation">
    <uc1:adminAddonPackNavLinks ID="adminAddonPackNavLinks1" runat="server" />
</asp:content>
<asp:content id="Content7" runat="server" contentplaceholderid="cphContentBody">
    <div class="AdminFormArea">
        <div class="AdminTextTalk">
            <h2>
                Manage Media Set Groups</h2>
            <div class="AdminFormDescription">
                <p>
                    Media Set Groups are an optional manner by which you can arrange and display Media Sets.  Example: "Home Page Group."  Only this group's Media Sets would be retrieved by your jQuery plugin or ASPNET User Control.
                </p>
            </div>
        </div>
        <div class="AdminFormInner">
                <table width="100%" class="SlideShowForm">
                <tr class="MainSelectorRow">
                        <td class="formLabel">
                            Select Media Set Group
                        </td>
                        <td class="formField">
                              <asp:DropDownList ID="ddlMediaSetGroups" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMediaSetGroups_OnSelectedIndexChanged"
                CssClass="BigDropDown" Width="500px" />

                             <div class="AdminUserFieldInfo">To enter new media set group, clear dropdown selection</div>
                        </td>
                    </tr>
                    <tr>
                        <td class="formLabel">
                            Title
                        </td>
                        <td class="formField">
                            <asp:TextBox ID="txtTitle" runat="server" CssClass="BigTextBox" Width="500px" />
                            <asp:RequiredFieldValidator runat="server" ErrorMessage=" *" CssClass="BigErrorMessage" ControlToValidate="txtTitle" />
                        </td>
                    </tr>
                    <tr>
                        <td class="formLabel textboxLabel">
                            Description
                        </td>
                        <td class="formField">
                            <asp:TextBox ID="txtDescription" runat="server" CssClass="BigTextBox" TextMode="MultiLine"
                                Rows="5" Width="500px" /> <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage=" *" CssClass="BigErrorMessage" ControlToValidate="txtDescription" />
                        </td>
                    </tr>
                    <tr>
                        <td class="formLabel">
                            Active
                        </td>
                        <td class="formField">
                            <asp:CheckBox ID="chkActive" runat="server" />
                        </td>
                    </tr>
                      <tr runat="server" id="rwMediaSetGroupID" visible="false">
                        <td class="formLabel">
                            ID
                        </td>
                        <td class="formField">
                            <asp:Label id="lblMediaSetGroupID" runat="server" />
                        </td>
                    </tr>
                    <tr><td></td><td>
                     <div class="TextButtonBigArea">
            <asp:Button ID="btnAddNew" runat="server" Text="Add New" CssClass="TextButtonBig"
                OnCommand="btnAddUpdate_OnCommand" CausesValidation="true" CommandName="Add" />
            <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="TextButtonBig"
                OnCommand="btnAddUpdate_OnCommand"  CausesValidation="true" CommandName="Update" />
        </div>
                    </td></tr> 
                    <tr><td></td><td>       
        <br />
        <br />
        <div id="ResultMessage" class="ResultsMessage">
            <asp:Label ID="lblResults" runat="server" />
        </div></td></tr>                                     
                </table>
        </div>

    </div>
</asp:content>
