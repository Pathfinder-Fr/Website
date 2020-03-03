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

<asp:content contentplaceholderid="cphHeader" runat="server">
    <style type="text/css">
        #offerlink {
            display: inline-block;
            padding: 10px 16px;
            border: solid 1px #853a1e;
            background-color: #e7dfc6;
            font-weight: bold;
            text-decoration: none;
        }

        .art-content {
            text-align: justify;
        }

        .art-content blockquote {
            margin: 0;
            border-left: 5px solid #eee;
            padding-left: 15px;
        }

        .art-content blockquote footer {
            font-style: italic;
            color: #888;
        }

    </style>
</asp:content>
<asp:content id="Content3" contentplaceholderid="cphBody" runat="Server">
    <div style="font-size: 16px;max-width: 640px; margin: 0 auto;" class="art-content">
        <h1>Lancement offre spéciale</h1>
        <p style="font-weight: bold; font-size: 18px; color: #4b3124; margin: 2em 0">
            Pathfinder-fr et BBE sont très fiers de vous annoncer qu’ils ont signé un accord permettant d’offrir toujours davantage de contenu aux joueurs de Pathfinder le Jeu de rôles.
        </p>
        <p>
            Pour moins d’un café par mois, chacun peut accéder au contenu des règles de base et pour à peine plus cher,
            vous pourrez disposer d’un accès complet à l’ensemble des règles et du contenu des parutions françaises dès
            leur sortie tout en contribuant utilement à la gamme.
        </p>
        <blockquote>
            <p>
                « Nous allons pouvoir aider des fans de la première heure à s’occuper de la promotion et de la diffusion de la gamme.
                Ce nouveau mode de diffusion par abonnement va pouvoir nous permettre de nous recentrer davantage sur le travail d’édition
                tout en continuant de proposer davantage encore de contenu à la communauté des joueurs de l’une de nos premières licences. »
            </p>
            <footer>s’est réjoui Damien de BBE.</footer>
        </blockquote>
        <blockquote>
            <p>
                « Nous sommes vraiment ravis de pouvoir finaliser ce partenariat.
                Cet accord permet de continuer à offrir aux joueurs la possibilité de jouer gratuitement et au site de proposer toujours davantage pour ceux qui peuvent payer. »
            </p>
            <footer>a déclaré Dalvyn de Pathfinder-Fr.</footer>
        </blockquote>
        <blockquote>
            <p>
                « Le passage à ce système d’abonnement va permettre de compléter en quelques semaines le retard sur le wiki et de pouvoir rivaliser avec les sites anglophones. »
            </p>
            <footer>a souligné Rectulo.</footer>
        </blockquote>
        <blockquote>
            <p>
                « We are very happy to see the French speaking Pathfinder community grow and thrive! These guys sure put a lot of work in it! »
            </p>
            <footer>a déclaré de son côté Lisa Stevens de Paizo.</footer>
        </blockquote>
        <p style="text-align: center">
            <a href="/boutique/offres" id="offerlink">Offres et tarifs &raquo;</a>
        </p>
    </div>
</asp:content>
