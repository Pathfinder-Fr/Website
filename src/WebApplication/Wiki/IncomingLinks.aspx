<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="IncomingLinks.aspx.cs" Inherits="ScrewTurn.Wiki.IncomingLinks" %>

<asp:Content ID="CtnHistory" ContentPlaceHolderID="CphMaster" runat="Server">
    <h1 class="pagetitlesystem"><asp:Literal ID="lblTitle" runat="server" meta:resourcekey="lblTitleResource1" Text="-- Title --" /></h1>
    <p>List of incoming links for this page :</p>
    <ul runat="server" id="ulItems">
    </ul>
</asp:Content>
