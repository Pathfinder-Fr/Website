<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.Web.SueetieSearchPage"
    EnableViewState="false" %>

<asp:content id="Content3" contentplaceholderid="cphBody" runat="Server">

    <script type='text/javascript' src='/scripts/jquery.js'></script>

    <script type='text/javascript' src='/scripts/tags.js'></script>

    <script type='text/javascript'>
        function CheckAllApps(e) {
            if (e.clientX > 0) {
                var name = $("#chkAll").attr("name");
                if (name == "unchecked") {
                    $(".SearchCheckboxes input[type='checkbox']").each(function () {

                        if (!this.checked) {
                            $(this).click();
                        }
                    });
                    $("#chkAll").attr("name", "checked");
                }
                else {
                    $(".SearchCheckboxes input[type='checkbox']").each(function () {

                        if (this.checked) {
                            $(this).attr('checked', false);
                        }
                    });
                    $("#chkAll").attr("name", "unchecked");

                }
            }
        }

    </script>

    <div id="BodyTitleArea">
        Welcome to Sueetie Search</div>
    <div id="menu">
        <ul>
            <li><a href="default.aspx" rel="home">Search Home</a></li>
        </ul>
    </div>
    <div class="SearchCheckboxes">
        <span class="SearchInLabel">Search Areas </span>
        <asp:CheckBoxList ID="cblSearchApps" runat="server" RepeatDirection="Horizontal"
            RepeatLayout="Flow" CssClass="SearchCheckbox">
            <asp:ListItem Value="Blogs" Text="Blogs" Selected="True" />
            <asp:ListItem Value="Forums" Text="Forums" Selected="True" />
            <asp:ListItem Value="Wikis" Text="Wikis" Selected="True" />
            <asp:ListItem Value="Albums" Text="Media Albums" Selected="True" />
            <asp:ListItem Value="Media" Text="Media Content" Selected="True" />
            <asp:ListItem Value="CMS" Text="Content Pages" Selected="True" />
        </asp:CheckBoxList>
        <span class="btnSearchAllAppsArea">
        <input type='submit' onclick='CheckAllApps(event);return false;' value=' Toggle '
            id="chkAll" name="checked" /></span>
    </div>
    <div class="SearchTextArea">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="TextBoxBig" Width="500px" />
        <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_OnClick"
            CssClass="TextButtonBig" /></p>
    </div>
    <div class="SearchResults">
        <asp:PlaceHolder ID="noResults" runat="server" Visible="false">No Results for the search
            term:
            <asp:Literal ID="terms" runat="server"></asp:Literal>
        </asp:PlaceHolder>
        <asp:Repeater ID="rptResults" runat="server" OnItemCreated="SearchResultsCreated"
            Visible="false">
            <HeaderTemplate>
                <ul class="SrchResults">
            </HeaderTemplate>
            <ItemTemplate>
                <li runat="server" id="searchResultItem">
                    <asp:HyperLink runat="server" ID="Link" />
                    <div class="SrchResultSource">
                        <span>
                            <asp:Literal ID="ltContainerLabel" runat="server" /></span>
                        <asp:Literal ID="ltContainerName" runat="server" />
                    </div>
                    <div class="SrchContent">
                        <asp:Literal ID="ltHighlightedContent" runat="server" />
                    </div>
                    <div class="SrchResultDetails">
                        <span>Author: </span>
                        <asp:Literal runat="server" ID="ltAuthor" />
                        <span>Date: </span>
                        <asp:Literal runat="server" ID="DatePublished" />
                        <span>Score: </span>
                        <asp:Literal ID="score" runat="server" />
                    </div>
                     <div class="SrchResultTags">
                     <span>Tags: </span>
                     <asp:Literal runat="server" ID="ltTags" />
                     </div>
                </li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
    </div>
</asp:content>
