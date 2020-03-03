<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RestartApp.aspx.cs" Inherits="Sueetie.Web.RestartApp" %>

<%@ Register Src="../controls/adminSiteSettingsNavLinks.ascx" TagName="adminSiteSettingsNavLinks"
    TagPrefix="uc1" %>
    <%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Restart Sueetie
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="1" />
</asp:Content>
<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminSiteSettingsNavLinks ID="adminSiteSettingsNavLinks1" runat="server" />
</asp:Content>
<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <h2>
            Restart Sueetie</h2>
             <div class="AdminFormDescription">
                <p>
                    Click below to restart Sueetie and clear the cache.</p>
            </div>
        <div class="AdminFormInner">
            <table width="100%">
                <tr>
                    <td>
                    </td>
                    <td>
                        <div class="TextButtonBigArea">
                            <asp:Button ID="SubmitButton" runat="server" Text="Restart" CssClass="TextButtonBig"
                                OnClick="Submit_Click" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                    
                        <asp:Label ID="lblResults" runat="server" Visible="false" CssClass="ResultsMessage RestartMessage" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
