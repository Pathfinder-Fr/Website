<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Footer.ascx.cs" Inherits="BlogEngine.Themes.Chiclet.Footer" %>

<%@ Register Src="~/themes/Chiclet/UserMenu.ascx" TagName="UserMenu" TagPrefix="uc" %>

  <div id="footer">
    <uc:UserMenu ID="UserMenu1" runat="server" />
    <SUEETIE:FooterMenu ID="FooterMenu1" runat="server" />
        <div id="sueetieFooterLogo">
            <SUEETIE:SueetieLogo ID="SueetieLogo1" runat="server" LogoFont="White" ShowText="True" />
        </div>
    </div>