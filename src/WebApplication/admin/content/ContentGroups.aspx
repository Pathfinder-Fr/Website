<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContentGroups.aspx.cs"
    Inherits="Sueetie.Web.AdminContentGroups" %>

<%@ Register Src="../controls/adminContentNavLinks.ascx" TagName="adminContentNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<asp:content id="Content1" contentplaceholderid="cphHeader" runat="Server">
<style type="text/css">
    td.formLabel, td.formLabelChk
    {
        width: 124px;
    }
</style>
</asp:content>
<asp:content id="Content5" runat="server" contentplaceholderid="cphBodyTitle">
    Manage Content Pages and Groups
</asp:content>
<asp:content id="Content2" runat="server" contentplaceholderid="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="2" />
</asp:content>
<asp:content id="Content6" runat="server" contentplaceholderid="cphUserNavigation">
    <uc1:adminContentNavLinks ID="adminContentNavLinks1" runat="server" />
</asp:content>
<asp:content id="Content7" runat="server" contentplaceholderid="cphContentBody">

    <div class="AdminFormArea">
        <div class="AdminTextTalk">
            <h2>
                Manage Content Pages and Groups</h2>
            <div class="AdminFormDescription">
                <p>
                    All content pages are associated with a <strong>Content Page Group,</strong> which is associated with a Sueetie CMS Application.  The application must be created BEFORE creating the Content Page Group.  The Application Key matches the content group's url subdirectory name.  The installed default "CMS Pages" Application Key is "cms."  Ex: /<b>cms</b>/welcome-page.aspx. Modify the Application Key to change the CMS url subdirectory. See the <a href="http://sueetie.com/wiki/patternsCMS.ashx">CMS Guide</a> in the Sueetie Wiki for more information.</p>
            </div>
        </div>
            <div class="AdminFormLabel">
                Select Content Page Group</div>
            <asp:DropDownList ID="ddlContentGroups" runat="server" AutoPostBack="true" CssClass="BigDropDown" Width="500px"  OnSelectedIndexChanged="ddlContentGroups_OnSelectedIndexChanged" />

                <div class="ContentPageGroupDropDownNotes">To create a new content group, clear dropdown selection</div>

        <div class="AdminFormInner">
                <table width="100%">
                    
                    <tr>
                        <td class="formLabel">
                            Application
                        </td>
                        <td class="formField" runat="server" id="tdApplicationKeyDropDown">
                        <asp:DropDownList id="ddlApplicationKeys" runat="server" CssClass="BigDropDown" width="500px" />
                        <div class="AdminUserFieldInfo"><b>Note:</b> Because of page and content part dependencies, application cannot be changed once selected.<br />Must be created prior to creating content page group.  See above.</div>
                        </td>
                        <td class="formField" runat="server" id="tdApplicationKeyLabel">
                        <asp:Label id="lblApplicationKey" runat="server" CssClass="BigLabel" />
                        </td>
                    </tr>
                    <tr>
                        <td class="formLabel">
                            Group Title
                        </td>
                        <td class="formField">
                            <asp:TextBox ID="txtTitle" runat="server" CssClass="BigTextBox" Width="510px" />
                            <asp:RequiredFieldValidator runat="server" ErrorMessage=" *" CssClass="BigErrorMessage" ControlToValidate="txtTitle" />
                        </td>
                    </tr>
           <tr>
                        <td class="formLabel">
                            Editor Roles
                        </td>
                        <td class="formField">
                            <asp:TextBox ID="txtEditors" runat="server" CssClass="BigTextBox" Width="500px" />
                            <div class="AdminUserFieldInfo"><b>ContentAdministrator</b> default editor group. Enter additional roles separated by commas.</div>
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
           
                </table>
        </div>
        <div class="TextButtonBigArea">
            <asp:Button ID="btnAddNew" runat="server" Text="Add New" CssClass="TextButtonBig"
                OnCommand="btnAddUpdate_OnCommand" CausesValidation="true" CommandName="Add" />
            <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="TextButtonBig"
                OnCommand="btnAddUpdate_OnCommand"  CausesValidation="true" CommandName="Update" />
            <asp:Button ID="btnManage" runat="server" Text="Manage" CssClass="TextButtonBig"
                OnClick="btnManage_OnClick"  CausesValidation="false" />
        </div>
        <br />
        <br />
        <div id="ResultMessage" class="ResultsMessage">
            <asp:Label ID="lblResults" runat="server" />
        </div>
    </div>
</asp:content>
