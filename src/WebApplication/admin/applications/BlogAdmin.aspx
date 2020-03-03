<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BlogAdmin.aspx.cs" Inherits="Sueetie.Web.BlogAdmin" %>

<%@ Register Src="../controls/adminBlogNavLinks.ascx" TagName="adminBlogNavLinks" TagPrefix="uc1" %>
<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server" />

<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Add Blog Administrator
</asp:Content>

<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminBlogNavLinks ID="adminBlogNavLinks1" runat="server" />
</asp:Content>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="3" />
</asp:Content>
<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <div class="AdminTextTalk">
            <h2>
                Add Site Blog Administrators</h2>
            <div class="AdminFormDescription">
                <p>
                    Use this function to add one or more blog authors/administrators to your site blogs. This
                    is typically performed on site setup, since by default the only user with blog administrative
                    permissions is "admin."</p>
                <p>
                    This archived <a href="http://sueetie.com/wiki/BlogLivewriter.ashx">
                        wiki document at Sueetie.com</a> describes some of the back-end processes performed by this form.</p>
            </div>
        </div>
        <div class="AdminLineBlock">
        <div class="AdminFormLabel">
            Enter Username</div>
        <input type="text" id="txtUsernames" class="adminHeavyText" />
        </div>
        <div class="TextButtonBigAreaSpaced">
            <input type="submit" class="TextButtonBig" onclick="Submit_Click();return false;" value="Add"/>
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
                ws.CreateBlogAdmin(names, displayResults);
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
