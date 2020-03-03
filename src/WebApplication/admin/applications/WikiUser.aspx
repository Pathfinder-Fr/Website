<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WikiUser.aspx.cs" Inherits="Sueetie.Web.AdminWikiUser" %>

<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation" TagPrefix="uc3" %>
<%@ Register Src="../controls/adminWikiNavLinks.ascx" TagName="adminWikiNavLinks" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server" />

<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Add Wiki User
</asp:Content>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="3" />
</asp:Content>

<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminWikiNavLinks ID="adminWikiNavLinks1" runat="server" />
</asp:Content>

<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <div class="AdminTextTalk">
            <h2>
                Add Wiki Users for Site Main Wiki</h2>
            <div class="AdminFormDescription">
                <p>
                    Use this function to add one or more wiki accounts.  Users with wiki accounts can set update notification options as well as participate in document discussions, otherwise no account is created in the ScrewTurn Wiki application to promote optimum performance and scalability.  If you want to create wiki accounts for all new community user accounts by default, select the "Create Wiki Account for New Users" checkbox on the <a href="general.aspx">General Site Settings page.</a> </p>
            </div>
        </div>
                <div class="AdminLineBlock">
        <div class="AdminFormLabel">
            Enter Username</div>
        <input type="text" id="txtUsernames" class="adminHeavyText" />
        </div>
        <div class="TextButtonBigAreaSpaced">
            <input type="submit" text="Submit" cssclass="TextButtonBig" onclick="Submit_Click();return false;" value=" Add " />
        </div>
        <br /><br />
        <div id="ResultMessage" class="ResultsMessage">
        </div>
    </div>

    <script type="text/javascript">

        function Submit_Click() {
            var names = $("#txtUsernames").val();
            if (names != '') {
                var ws = new Sueetie.Web.SueetieService();
                ws.CreateWikiUsers(names, displayResults);
            }
        }

        function displayResults(result) {
            $("#ResultMessage").text(result);
        }

        $(document).ready(function() {

            $("#txtUsernames").tokenInput("../json/tokenusers.aspx", {
                classes: {
                    tokenList: "token-input-list-facebook",
                    token: "token-input-token-facebook",
                    tokenDelete: "token-input-delete-token-facebook",
                    selectedToken: "token-input-selected-token-facebook",
                    highlightedToken: "token-input-highlighted-token-facebook",
                    dropdown: "token-input-dropdown-facebook",
                    dropdownItem: "token-input-dropdown-item-facebook",
                    dropdownItem2: "token-input-dropdown-item2-facebook",
                    selectedDropdownItem: "token-input-selected-dropdown-item-facebook",
                    inputToken: "token-input-input-token-facebook"
                }
            });

        }); 

    </script>

</asp:Content>