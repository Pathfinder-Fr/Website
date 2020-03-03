<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.AddonPack.Pages.SlideshowDefaultPage" %>

<%@ Register Src="/admin/controls/adminAddonPackNavLinks.ascx" TagName="adminAddonPackNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="/admin/controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<asp:content id="Content1" contentplaceholderid="cphHeader" runat="Server" />
<asp:content id="Content5" runat="server" contentplaceholderid="cphBodyTitle">
    Manage Slideshows
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
                Manage Slideshows</h2>
            <div class="AdminFormDescription">
                <p>
                    Use this function to add, update or deactivate site slideshows and slideshow images. 
                </p>
            </div>
        </div>
        <div class="AdminFormInner">
                <table width="100%" class="SlideShowForm">
                <tr class="MainSelectorRow">
                        <td class="formLabel">
                            Select Slideshow
                        </td>
                        <td class="formField">
                              <asp:DropDownList ID="ddlSlideshows" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSlideshows_OnSelectedIndexChanged"
                CssClass="BigDropDown" Width="500px" />

                             <div class="AdminUserFieldInfo">To enter new slideshow, clear dropdown selection</div>
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
                      <tr>
                        <td class="formLabel">
                            Full Image Height
                        </td>
                        <td class="formField">
                            <asp:TextBox ID="txtFullImageHeight" runat="server" CssClass="BigTextBox" Width="60px" /><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage=" *" CssClass="BigErrorMessage" ControlToValidate="txtFullImageHeight" />
                        </td>
                    </tr>
                      <tr>
                        <td class="formLabel">
                            Full Image Width
                        </td>
                        <td class="formField">
                            <asp:TextBox ID="txtFullImageWidth" runat="server" CssClass="BigTextBox" Width="60px" /><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage=" *" CssClass="BigErrorMessage" ControlToValidate="txtFullImageWidth" />
                        </td>
                    </tr>
   <tr>
                        <td class="formLabel">
                            Medium Image Height
                        </td>
                        <td class="formField">
                            <asp:TextBox ID="txtMediumImageHeight" runat="server" CssClass="BigTextBox" Width="60px" /><asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage=" *" CssClass="BigErrorMessage" ControlToValidate="txtMediumImageHeight" />
                        </td>
                    </tr>
                      <tr>
                        <td class="formLabel">
                            Medium Image Width
                        </td>
                        <td class="formField">
                            <asp:TextBox ID="txtMediumImageWidth" runat="server" CssClass="BigTextBox" Width="60px" /><asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage=" *" CssClass="BigErrorMessage" ControlToValidate="txtMediumImageWidth" />
                        </td>
                    </tr>
   <tr>
                        <td class="formLabel">
                            Small Image Height
                        </td>
                        <td class="formField">
                            <asp:TextBox ID="txtSmallImageHeight" runat="server" CssClass="BigTextBox" Width="60px" /><asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage=" *" CssClass="BigErrorMessage" ControlToValidate="txtSmallImageHeight" />
                        </td>
                    </tr>
                      <tr>
                        <td class="formLabel">
                            Small Image Width
                        </td>
                        <td class="formField">
                            <asp:TextBox ID="txtSmallImageWidth" runat="server" CssClass="BigTextBox" Width="60px" /><asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage=" *" CssClass="BigErrorMessage" ControlToValidate="txtSmallImageWidth" />
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
                    <tr><td></td><td>        <br />
        <br />
        <div id="ResultMessage" class="ResultsMessage">
            <asp:Label ID="lblResults" runat="server" />
        </div></td></tr>                                 
                </table>
        </div>
       

    </div>
</asp:content>
