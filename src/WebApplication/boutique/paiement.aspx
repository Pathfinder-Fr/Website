<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.Web.SueetieDefaultPage" SueetieMasterPage="alternate.master" ValidateRequest="false" Title="Communauté francophone Pathfinder" EnableViewState="false" %>

<%@ Import Namespace="Sueetie.Core" %>
<%@ Register TagPrefix="uc" TagName="UserMenu" Src="~/themes/Lollipop/menus/UserMenu.ascx" %>
<%@ Register TagPrefix="uc" TagName="HomeBodyMenu" Src="~/themes/Lollipop/menus/HomeBodyMenu.ascx" %>

<script runat="server">

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
    }

</script>

<asp:content id="Content3" contentplaceholderid="cphBody" runat="Server">
    <div style="font-size: 16px;max-width: 640px; margin: 0 auto;">
        <h1>Aboleth d'avril !</h1>
        <img src="/themes/v2/images/temp/aboleth.jpg" title="Aboleth d'avril!" style="width:340px; display: block; margin: 0 auto;" />
        <p>
            Le wiki reste naturellement gratuit... mais s'il n'est pas complet, c'est parce qu'il manque de bras.
            Si votre coeur a manqué un battement à la lecture des pages précédentes et que vous aimez le wiki, n'hésitez pas à y contribuer.
        </p>
        <p style="font-size: 14px; font-style: italic">
            Note : Pour continuer à utiliser le wiki durant cette journée, vous pouvez modifier manuellement l'adresse de la page et remplacer le mot "Pathfinder" par "Avril".<br />
            Ainsi, la page <a href="http://www.pathfinder-fr.org/Wiki/Pathfinder-RPG.Caractéristiques.ashx#FORCE">pathfinder-fr.org/Wiki/Pathfinder-RPG.Caractéristiques.ashx#FORCE</a>
            doit être modifiée en <a href="http://www.pathfinder-fr.org/Wiki/Avril-RPG.Caractéristiques.ashx#FORCE">pathfinder-fr.org/Wiki/Avril-RPG.Caractéristiques.ashx#FORCE</a>.
        </p>
    </div>
</asp:content>
