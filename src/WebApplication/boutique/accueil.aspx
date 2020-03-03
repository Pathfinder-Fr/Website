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
        <h1>Lancement offre sp�ciale</h1>
        <p style="font-weight: bold; font-size: 18px; color: #4b3124; margin: 2em 0">
            Pathfinder-fr et BBE sont tr�s fiers de vous annoncer qu�ils ont sign� un accord permettant d�offrir toujours davantage de contenu aux joueurs de Pathfinder le Jeu de r�les.
        </p>
        <p>
            Pour moins d�un caf� par mois, chacun peut acc�der au contenu des r�gles de base et pour � peine plus cher,
            vous pourrez disposer d�un acc�s complet � l�ensemble des r�gles et du contenu des parutions fran�aises d�s
            leur sortie tout en contribuant utilement � la gamme.
        </p>
        <blockquote>
            <p>
                ��Nous allons pouvoir aider des fans de la premi�re heure � s�occuper de la promotion et de la diffusion de la gamme.
                Ce nouveau mode de diffusion par abonnement va pouvoir nous permettre de nous recentrer davantage sur le travail d��dition
                tout en continuant de proposer davantage encore de contenu � la communaut� des joueurs de l�une de nos premi�res licences.��
            </p>
            <footer>s�est r�joui Damien de BBE.</footer>
        </blockquote>
        <blockquote>
            <p>
                ��Nous sommes vraiment ravis de pouvoir finaliser ce partenariat.
                Cet accord permet de continuer � offrir aux joueurs la possibilit� de jouer gratuitement et au site de proposer toujours davantage pour ceux qui peuvent payer.��
            </p>
            <footer>a d�clar� Dalvyn de Pathfinder-Fr.</footer>
        </blockquote>
        <blockquote>
            <p>
                ��Le passage � ce syst�me d�abonnement va permettre de compl�ter en quelques semaines le retard sur le wiki et de pouvoir rivaliser avec les sites anglophones.��
            </p>
            <footer>a soulign� Rectulo.</footer>
        </blockquote>
        <blockquote>
            <p>
                ��We are very happy to see the French speaking Pathfinder community grow and thrive! These guys sure put a lot of work in it!��
            </p>
            <footer>a d�clar� de son c�t� Lisa Stevens de Paizo.</footer>
        </blockquote>
        <p style="text-align: center">
            <a href="/boutique/offres" id="offerlink">Offres et tarifs &raquo;</a>
        </p>
    </div>
</asp:content>
