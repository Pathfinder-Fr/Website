<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.AddonPack.Pages.BlogPostImagePage"
    MasterPageFile="/themes/lollipop/masters/admin.master" %>

<%@ Register Src="/admin/controls/adminAddonPackNavLinks.ascx" TagName="adminAddonPackNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="/admin/controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Sueetie Addons - Blog Post Thumbnail Settings
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="5" />
</asp:Content>
<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminAddonPackNavLinks ID="adminAddonPackNavLinks1" runat="server" />
</asp:Content>
<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <div class="AdminTextTalk">
            <div class="MainMenuContents">
                <h2>
                    Sueetie Addon Pack - Blog Post Thumbnail Settings</h2>
                <div class="AdminFormDescription">
                    <p>
                        Use this function to update post thumbnail settings for each site blog.
                    </p>
                </div>
            </div>
            </div>
            <div class="AdminFormInner">
                <table width="100%" class="SlideShowForm">
                    <tr class="MainSelectorRow">
                        <td class="formLabel">
                            Select Blog
                        </td>
                        <td class="formField">
                            <asp:DropDownList ID="ddBlogs" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddBlogs_OnSelectedIndexChanged"
                                CssClass="BigDropDown" Width="500px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="formLabel">
                            Post Image Type
                        </td>
                        <td class="formField">
                            <asp:DropDownList ID="ddPostImageTypes" runat="server" CssClass="BigDropDown" Width="300px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="formLabel">
                            Default Post Thumbnail
                        </td>
                        <td class="formField">
                            <asp:TextBox ID="txtDefaultPostImage" runat="server" CssClass="BigTextBox" Width="500px" />
                            <div class="AdminUserFieldInfo">
                                Thumbnail to display when no other image present. Example: <strong>/images/shared/sueetie/lollipop.png</strong></div>
                        </td>
                    </tr>
                    <tr>
                        <td class="formLabel">
                            Thumbnail Height
                        </td>
                        <td class="formField">
                            <asp:TextBox ID="txtPostImageHeight" runat="server" CssClass="BigTextBox" Width="60px" /><asp:RequiredFieldValidator
                                ID="RequiredFieldValidator2" runat="server" ErrorMessage=" *" CssClass="BigErrorMessage"
                                ControlToValidate="txtPostImageHeight" />
                        </td>
                    </tr>
                    <tr>
                        <td class="formLabel">
                            Thumbnail Width
                        </td>
                        <td class="formField">
                            <asp:TextBox ID="txtPostImageWidth" runat="server" CssClass="BigTextBox" Width="60px" /><asp:RequiredFieldValidator
                                ID="RequiredFieldValidator1" runat="server" ErrorMessage=" *" CssClass="BigErrorMessage"
                                ControlToValidate="txtPostImageWidth" />
                        </td>
                    </tr>
                    <tr>
                        <td class="formLabel">
                            Anchor Position
                        </td>
                        <td class="formField">
                            <asp:DropDownList ID="ddAnchorPositions" runat="server" CssClass="BigDropDown" Width="100px" />
                            <div class="AdminUserFieldInfo">
                                Cropping logic when using first post image option</div>
                        </td>
                    </tr>
                    <tr>
                        <td class="formLabel">
                            Media Album
                        </td>
                        <td class="formField">
                            <asp:DropDownList ID="ddMediaAlbums" runat="server" CssClass="BigDropDown" Width="500px" />
                            <div class="AdminUserFieldInfo">
                                With Random Media Gallery Image option. Selects all images from album and child albums.</div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <div class="TextButtonBigArea">
                                <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="TextButtonBig"
                                    OnCommand="btnUpdate_OnCommand" CausesValidation="true" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <br />
                            <br />
                            <div id="ResultMessage" class="ResultsMessage">
                                <asp:Label ID="lblResults" runat="server" />
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
