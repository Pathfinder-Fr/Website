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
        .check {
            width: 32px;
        }

        .art-content, .art-content table { font-size: 16px; }

        .art-content table td { padding: 16px; }
        .art-content table th { padding: 16px; }

        .text-center {
            text-align: center;
        }

        .col {
            background-color: #e8e2c8;
        }

        .alt td { background-color: #bdb494;}

        .alt td.no {background-color: transparent;}

        .payment {
            text-align: center;
        }

        .payment a img { border: 1px solid #4b3124; }
    </style>
</asp:content>

<asp:content id="Content3" contentplaceholderid="cphBody" runat="Server">
    <div style="font-size: 16px;max-width: 640px; margin: 0 auto;" class="art-content">
    
    <h1>Lancement offre spéciale</h1>
        <p>Choisissez le contenu qui vous convient et cliquez sur le mode de règlement souhaité.</p>
        <table style="width: 100%">
            <tbody>
                <tr>
                    <th style="width:40%">                    </th>
                    <th style="width:20%" class="col">Basique</th>
                    <th style="width:20%">Standard</th>
                    <th style="width:20%" class="col">Pack Premium</th>
                </tr>
                <tr class="">
                    <td class="no">                    </td>
                    <td class="text-center col">Gratuit</td>
                    <td class="text-center">3 &euro; par mois</td>
                    <td class="text-center col">5 &euro; par mois</td>
                </tr>
                <tr>
                    <td>Manuel des joueurs</td>
                    <td class="text-center col"><img src="/themes/v2/images/temp/check.png" class="check" /></td>
                    <td class="text-center"><img src="/themes/v2/images/temp/check.png" class="check" /></td>
                    <td class="text-center col"><img src="/themes/v2/images/temp/check.png" class="check" /></td>
                </tr>
                <tr class="">
                    <td>Toutes les règles Pathfinder<br />(avec tous les suppléments)</td>
                    <td class="text-center col"></td>
                    <td class="text-center"><img src="/themes/v2/images/temp/check.png" class="check" /></td>
                    <td class="text-center col"><img src="/themes/v2/images/temp/check.png" class="check" /></td>
                </tr>
                <tr>
                    <td>Toute la gamme Pathfinder<br />(et l’accès à toutes les campagnes)</td>
                    <td class="col"></td>
                    <td></td>
                    <td class="text-center col"><img src="/themes/v2/images/temp/check.png" class="check" /></td>
                </tr>
            </tbody>
        </table>
        <p></p>
        <div class="payment">
            Sélectionnez un mode de paiement pour continuer.<br />
            Cet abonnement sera associé à votre compte.<br />
            <br />
            <a href="/boutique/paiement/paypal" title="paiement Paypal"><img src="/themes/v2/images/temp/paypal.jpg" alt="paypal" /></a>
            <a href="/boutique/paiement/cb" title="paiement par carte bleue"><img src="/themes/v2/images/temp/cb.jpg" alt="cb" /></a>
            <a href="/boutique/paiement/mastercard" title="paiement par carte Mastercard"><img src="/themes/v2/images/temp/mastercard.jpg" alt="mastercard" /></a>
            <a href="/boutique/paiement/visa" title="paiement par carte Visa"><img src="/themes/v2/images/temp/visa.jpg" alt="visa" /></a>
        </div>
    </div>
</asp:content>
