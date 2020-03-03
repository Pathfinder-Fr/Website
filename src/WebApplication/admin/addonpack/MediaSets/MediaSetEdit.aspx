<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.AddonPack.Pages.MediaSetPage" %>

<%@ Register Src="/admin/controls/adminAddonPackNavLinks.ascx" TagName="adminAddonPackNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="/admin/controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<asp:content id="Content1" contentplaceholderid="cphHeader" runat="Server" />
<asp:content id="Content5" runat="server" contentplaceholderid="cphBodyTitle">
    Manage Media Sets
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
                Manage Media Sets</h2>
            <div class="AdminFormDescription">
                <p>
                    Media Sets enable you to display Media Objects by any arrangement you choose, with the jQuery plugin or ASPNET User Control of your choice.
                </p>
            </div>
        </div>
        <div class="AdminFormInner">
                <table width="100%" class="SlideShowForm">
                <tr class="MainSelectorRow">
                        <td class="formLabel">
                            Select Media Set
                        </td>
                        <td class="formField">
                              <asp:DropDownList ID="ddlMediaSets" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMediaSets_OnSelectedIndexChanged"
                CssClass="BigDropDown" Width="500px" />

                             <div class="AdminUserFieldInfo">To enter new media set, clear dropdown selection</div>
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
                            Media Set Group
                        </td>
                        <td class="formField">
                             <asp:DropDownList ID="ddlMediaSetGroups" runat="server" CssClass="BigDropDown" Width="500px" />
                             <div class="AdminUserFieldInfo">Can arrange multiple media sets by group. Optional.</div>
                        </td>
                    </tr>
                      <tr>
                        <td class="formLabel">
                            Media Type
                        </td>
                        <td class="formField">
                             <asp:DropDownList ID="ddlContentTypeIDs" runat="server" CssClass="BigDropDown" Width="500px" />
                             <div class="AdminUserFieldInfo">In initial v3.2 release, only image media types are supported. Video, audio and documents to come.</div>
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
                      <tr runat="server" id="rwMediaSetID" visible="false">
                        <td class="formLabel">
                            ID
                        </td>
                        <td class="formField">
                            <asp:Label id="lblMediaSetID" runat="server" />
                        </td>
                    </tr>
                    <tr><td></td><td>
                     <div class="TextButtonBigArea">
            <asp:Button ID="btnAddNew" runat="server" Text="Add New" CssClass="TextButtonBig"
                OnCommand="btnAddUpdate_OnCommand" CausesValidation="true" CommandName="Add" />
            <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="TextButtonBig"
                OnCommand="btnAddUpdate_OnCommand"  CausesValidation="true" CommandName="Update" />
            <asp:Button ID="btnManage" runat="server" Text="Manage" CssClass="TextButtonBig"
                OnClick="btnManage_OnClick"  CausesValidation="false" />
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
