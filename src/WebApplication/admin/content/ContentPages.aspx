<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContentPages.aspx.cs" Inherits="Sueetie.Web.AdminContentPages" %>

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
    Manage Content Pages
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
                Manage Content Pages</h2>
            <div class="AdminFormDescription">
                <p>
                    Select a page to edit or enter new page description with a cleared Content Page dropdown list.
                </p>
            </div>
        </div>
            <div class="AdminFormLabel">
                Select Content Page</div>
            <asp:DropDownList ID="ddlContentPages" runat="server" AutoPostBack="true" CssClass="BigDropDown" Width="500px"  OnSelectedIndexChanged="ddlContentPages_OnSelectedIndexChanged" />
                <div class="ContentPageGroupDropDownNotes">To create a new content page, clear dropdown selection</div>
        <div class="AdminFormInner">
                <table width="100%">
                    
                    <tr>
                        <td class="formLabel">
                            Page Key
                        </td>
                        <td class="formField">
                        <asp:TextBox ID="txtPageKey" runat="server" CssClass="BigTextBox" Width="200px" /> <span class="BigErrorMessage">*</span>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage=" *" CssClass="BigErrorMessage" ControlToValidate="txtPageKey" />
						<div class="AdminUserFieldInfo">PageKey used to form unique CMS content names. Usage: [GroupKey].<b> [PageKey]</b>.ContentName.</div>
                        </td>
                    </tr>
                    <tr>
                        <td class="formLabel">
                            Page Title
                        </td>
                        <td class="formField">
                            <asp:TextBox ID="txtPageTitle" runat="server" CssClass="BigTextBox" Width="500px" /> <span class="BigErrorMessage">*</span>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage=" *" CssClass="BigErrorMessage" ControlToValidate="txtPageTitle" />
                        </td>
                    </tr>
                    <tr>
                        <td class="formLabel">
                            Page Slug
                        </td>
                        <td class="formField">
                            <asp:TextBox ID="txtPageSlug" runat="server" CssClass="BigTextBox" Width="500px" />
                            <div class="AdminUserFieldInfo"><b>(Optional.)</b> Page Slug used as page_name.aspx for better SEO results. Created from title if blank.</div>

                        </td>
                    </tr>
                    <tr>
                        <td class="formLabel">
                            Page Description<br /><div class="AdminUserFieldInfo">(Optional)</div>
                        </td>
                        <td class="formField">
                            <asp:TextBox ID="txtDescription" runat="server" CssClass="BigTextBox" TextMode="MultiLine"
                                Rows="5" Width="500px" /> 
                        </td>
                    </tr>
           <tr>
                        <td class="formLabel">
                            Reader Roles
                        </td>
                        <td class="formField">
                            <asp:TextBox ID="txtReaders" runat="server" CssClass="BigTextBox" Width="500px" />
                            <div class="AdminUserFieldInfo">By default all users have page access unless roles specified here. (Separate by commas.)</div>
                        </td>
                    </tr>
           
                    <tr>
                        <td class="formLabel">
                            Is Published
                        </td>
                        <td class="formField">
                            <asp:CheckBox ID="chkActive" runat="server" />
                        </td>
                    </tr>
                      <tr>
                        <td class="formLabel">
                            Display Order
                        </td>
                        <td class="formField">
                            <asp:TextBox ID="txtDisplayOrder" runat="server" width="30px"/>
                              <div class="AdminUserFieldInfo">(Optional) integer only</div>
                        </td>
                    </tr>
                                                
                </table>
        </div>
        <div class="TextButtonBigArea">
            <asp:Button ID="btnAddNew" runat="server" Text="Add New" CssClass="TextButtonBig"
                OnCommand="btnAddUpdate_OnCommand" CausesValidation="true" CommandName="Add" />
            <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="TextButtonBig"
                OnCommand="btnAddUpdate_OnCommand" CausesValidation="true" CommandName="Delete" />
            <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="TextButtonBig"
                OnCommand="btnAddUpdate_OnCommand"  CausesValidation="true" CommandName="Update" />
        </div>
        <br />
        <br />
        <div id="ResultMessage" class="ResultsMessage">
            <asp:Label ID="lblResults" runat="server" />
        </div>
    </div>
</asp:content>
