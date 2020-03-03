<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.Web.SueetieDefaultPage" SueetieMasterPage="alternate.master" ValidateRequest="false" Title="Communaut� francophone Pathfinder" EnableViewState="false" %>

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
            Si votre coeur a manqu� un battement � la lecture des pages pr�c�dentes et que vous aimez le wiki, n'h�sitez pas � y contribuer.
        </p>
        <p style="font-size: 14px; font-style: italic">
            Note : Pour continuer � utiliser le wiki durant cette journ�e, vous pouvez modifier manuellement l'adresse de la page et remplacer le mot "Pathfinder" par "Avril".<br />
            Ainsi, la page <a href="http://www.pathfinder-fr.org/Wiki/Pathfinder-RPG.Caract�ristiques.ashx#FORCE">pathfinder-fr.org/Wiki/Pathfinder-RPG.Caract�ristiques.ashx#FORCE</a>
            doit �tre modifi�e en <a href="http://www.pathfinder-fr.org/Wiki/Avril-RPG.Caract�ristiques.ashx#FORCE">pathfinder-fr.org/Wiki/Avril-RPG.Caract�ristiques.ashx#FORCE</a>.
        </p>
    </div>
</asp:content>
