<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.AddonPack.Pages.CheckIpPage"
    MasterPageFile="/themes/lollipop/masters/admin.master" %>

<%@ Register Src="/admin/controls/adminAddonPackNavLinks.ascx" TagName="adminAddonPackNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="/admin/controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Sueetie Addons - Client Access Control - Check If IP Blocked
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="5" />
</asp:Content>
<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminAddonPackNavLinks ID="adminAddonPackNavLinks1" runat="server" />
</asp:Content>
<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <div class="Countries">
            <div class="AdminTextTalk">
                <h2>
                    Check If IP Blocked</h2>
                <div class="AdminFormDescription">
                    <p>
                        Enter an IP to test if it is blocked.<br />
                    </p>
                    <div class="AdminFormInner">
                        <table width="100%">
                            <tr class="rwAddSpace">
                                <td class="formLabel">
                                    IP Address
                                </td>
                                <td class="formField">
                                    <asp:TextBox runat="server" ID="txtIP" class="adminHeavyText" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <div class="TextButtonBigArea">
                                        <asp:Button ID="SubmitButton" runat="server" Text="Submit" CssClass="TextButtonBig"
                                            OnClick="Submit_Click" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Label ID="lblResults" runat="server" CssClass="ResultsMessage CellIpMessage" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
