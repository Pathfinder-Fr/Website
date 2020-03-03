<%@ Page Language="C#" Inherits="Sueetie.Commerce.Pages.ShowProductPage" MasterPageFile="~/Themes/Lollipop/Masters/marketplace.master" %>

<%@ Import Namespace="Sueetie.Commerce" %>
<asp:Content ID="cphHeader1" ContentPlaceHolderID="cphHeader" runat="Server">
    <link href="/scripts/jquery.alerts.css" rel="stylesheet" type="text/css" />
    <script type='text/javascript' src='/scripts/jquery.js'></script>
    <script type='text/javascript' src='/scripts/jquery.alerts.js'></script>
    <script type='text/javascript' src='/scripts/jquery-utils.js'></script>
    <script type="text/javascript">

        $(document).ready(function () {
            var qstring = $.parseQuery();
            if (qstring.r > 0)
                jAlert('We\'re sorry, but this product is not currently available.', 'Sueetie Alert', null);
        });

    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="NavBar" ID="NavBarContent" runat="server">
    <div id="crumbs_text">
        <a href="Default.aspx">Marketplace Home</a>&nbsp;&raquo;&nbsp; <a href="Browse.aspx">
            All Products</a>
        <SUEETIE:CommerceBreadCrumbs ID="CommerceCrumbs1" runat="server" />
</asp:Content>
<asp:Content ID="contentLowerSideBar" ContentPlaceHolderID="cphLowerSideBar" runat="server">
    <asp:PlaceHolder ID="phSidebarActions" runat="server">
        <h4>
            Links for this Product</h4>
        <div class="links">
            <ul>
                <li runat="server" id="ProductKeyButtonLI1">
                    <asp:LinkButton ID="lbtnProductKeyButton1" runat="server" OnClick="GenerateProductKey_Click">Create Free Product Key</asp:LinkButton></li>
                <SUEETIE:PackageCartLinks ID="PackageCartLinks1" runat="server" IsSideBarLink="true" />
                <li runat="server" id="LoginButtonLI1">
                    <asp:HyperLink ID="LoginButton1" runat="server" NavigateUrl="/members/login.aspx" /></li>
                <li runat="server" id="DownloadButtonLI1">
                    <asp:LinkButton ID="DownloadButton1" runat="server" OnClick="Download_Click">Download
                        this Item</asp:LinkButton></li>
                <asp:LinkButton ID="RespondButton1" runat="server" OnClick="RespondButton_Click">Email Author about this Item</asp:LinkButton></li>
                <li>
                    <asp:LinkButton ID="EmailAdButton1" runat="server" OnClick="EmailAdButton_Click">Send this Item to a Friend</asp:LinkButton></li>
                <%--                <li runat="server" id="SaveAddButtonLI1">
                    <asp:LinkButton ID="SaveAdButton1" runat="server" OnClick="SaveAdButton_Click">Add Item to your Bookmarks</asp:LinkButton></li>--%>
            </ul>
        </div>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phSidebarNotActivePanel" runat="server" Visible="false">
        <div class="links">
            <ul>
                <li><em>This Item is currently<br />
                    not active.</em></li></ul>
        </div>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phSidebarNotAvailablePanel" runat="server" Visible="false">
        <div class="fileNotAvailable">
            We're sorry, but this item is currently unavailable.
        </div>
    </asp:PlaceHolder>
    <div class="cartImageArea">
        <a target="_self" href="<%= Payments.ShoppingCartLink() %>">
            <img src="/themes/lollipop/images/marketplace/cart.png" alt="View your Shopping Cart" /></a>
    </div>
</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="cphBody" runat="server">
    <div id="body">
        <div id="resultsContent">
            <div class="display_left">
                <asp:DataList OnItemCommand="PhotoList_ItemCommand" ID="PhotoList" runat="Server"
                    DataSourceID="PhotosDataSource" DataKeyField="PhotoID" RepeatLayout="Flow">
                    <ItemTemplate>
                        <asp:LinkButton ID="AdPhotoImageButton" runat="Server" CommandName="ShowFullSize"
                            CommandArgument='<%# Eval("photoId") %>'><img alt="Enlarge Photo" src='<%# Eval("PhotoID", "/util/handlers/PhotoDisplay.ashx?pid={0}&size=medium&t=2") %>'  /></asp:LinkButton>
                    </ItemTemplate>
                </asp:DataList>
            </div>
            <div class="display_right" id="ad_details">
                <asp:Panel ID="AdDetailsPanel" runat="server">
                    <h3 class="ProductTitle">
                        <asp:HyperLink ID="hyperlinkTitle" runat="server" /></h3>
                    <h5>
                        <asp:Label ID="SubTitleLabel" runat="server" /></h5>
                    <asp:Panel ID="PhotoPanel" runat="server" Visible="False">
                        <p class="notice">
                            <asp:LinkButton ID="HidePhotoPanel" runat="server" OnClick="HidePhotoPanel_Click">[close]</asp:LinkButton></p>
                        <p>
                            <asp:ImageButton ID="FullSizePhoto" runat="server" OnClick="FullSizePhoto_Click"
                                AlternateText="Show other product photos if they exist." /></p>
                    </asp:Panel>
                    <asp:Panel ID="ResponsePanel" runat="server" Visible="False">
                        <h5 class="action">
                            Contact Us About This Product</h5>
                        Your Name:<br />
                        <asp:TextBox ID="ResponseContactNameTextBox" runat="server" CssClass="user_info"></asp:TextBox>
                        <p>
                            Your Contact Email<br />
                            <asp:TextBox ID="ResponseContactEmailTextBox" runat="server" CssClass="user_info"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="ResponseContactEmailTextBox"
                                ErrorMessage="A contact email is required." ValidationGroup="RespondPanelGroup"
                                ToolTip="A contact email is required." ID="ResponseEmailRequired1" Display="dynamic">
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="ResponseContactEmailTextBox"
                                ValidationExpression=".*@.*\..*" ErrorMessage="A valid email is required." ToolTip="A valid email is required."
                                ID="ResponseEmailRequired2" Display="dynamic">
                            </asp:RegularExpressionValidator>
                        </p>
                        <p>
                            Comments or Questions:<br />
                            <asp:TextBox ID="ResponseCommentsTextBox" runat="server" TextMode="MultiLine" CssClass="ResponseCommentsTextBox"></asp:TextBox></p>
                        <p>
                            <asp:Button ID="ResponseSubmitButton" runat="server" Text="Submit" OnClick="ResponseSubmitButton_Click"
                                CssClass="TextButtonBig" />
                            <asp:Button ID="CancelResponseButton" runat="server" Text="Cancel" OnClick="CancelResponseButton_Click"
                                CssClass="TextButtonBig" />
                        </p>
                    </asp:Panel>
                    <asp:Panel ID="EmailPanel" runat="server" Visible="false">
                        <h5 class="action">
                            Tell a Friend About This Product</h5>
                        <p>
                            Your Name:<br />
                            <asp:TextBox ID="EmailSenderNameTextBox" runat="server" CssClass="user_info"></asp:TextBox>
                        </p>
                        <p>
                            Your Email:<br />
                            <asp:TextBox ID="EmailSenderAddressTextBox" runat="server" CssClass="user_info"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="EmailSenderAddressTextBox"
                                ErrorMessage="A sender email is required." ValidationGroup="EmailPanelGroup"
                                ToolTip="A sender email is required." ID="EmailSenderAddressTextBoxValidator1"
                                Display="Dynamic">
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="EmailSenderAddressTextBox"
                                ValidationExpression=".*@.*\..*" ErrorMessage="A valid email is required." ToolTip="A valid email is required."
                                ID="EmailSenderAddressTextBoxValidator2" Display="Dynamic">
                            </asp:RegularExpressionValidator>
                        </p>
                        <p>
                            Send to (Recipient Email):<br />
                            <asp:TextBox ID="EmailRecipientAddressTextBox" runat="server" CssClass="user_info"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="EmailRecipientAddressTextBox"
                                ErrorMessage="A recipient email is required." ValidationGroup="EmailPanelGroup"
                                ToolTip="A recipient email is required." ID="EmailRecipientAddressTextBoxValidator1"
                                Display="Dynamic">
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="EmailRecipientAddressTextBox"
                                ValidationExpression=".*@.*\..*" ErrorMessage="A valid email is required." ToolTip="A valid email is required."
                                ID="EmailRecipientAddressTextBoxValidator2" Display="Dynamic">
                            </asp:RegularExpressionValidator>
                        </p>
                        <p>
                            Subject:<br />
                            <asp:TextBox ID="EmailSubjectTextBox" runat="server" Width="452px" CssClass="user_info"></asp:TextBox>
                        </p>
                        <p>
                            Message:<br />
                            <asp:TextBox ID="EmailMessageTextBox" runat="server" TextMode="MultiLine" Width="452px"
                                Height="123px"></asp:TextBox></p>
                        <p>
                            &nbsp;<asp:Button ID="EmailSubmitButton" runat="server" Text="Submit" OnClick="EmailSubmitButton_Click"
                                CssClass="TextButtonBig" />
                            <asp:Button ID="CancelEmailButton" runat="server" Text="Cancel" OnClick="CancelEmailButton_Click"
                                CssClass="TextButtonBig" />
                        </p>
                    </asp:Panel>
                    <asp:Panel ID="AdSavedPanel" runat="server" Visible="False" EnableViewState="False">
                        <div style="text-align: center;" class="notice">
                            <span>The item has been <a href="MyAds.aspx?saved=1">saved to your list of Bookmarks.</a></span></div>
                    </asp:Panel>
                    <asp:Panel ID="DownloadErrorPanel" runat="server" Visible="False" EnableViewState="False">
                        <div style="text-align: center;" class="notice">
                            <span>An error occurred when attempting to download this file and has been recorded.
                                Please use the <a href="/blog/contact.aspx">contact form</a> to inform us so we
                                can immediately resolve this for you.</span></div>
                    </asp:Panel>
                    <asp:Panel ID="EmailSentPanel" runat="server" Visible="False" EnableViewState="False">
                        <div class="notice" style="text-align: center;">
                            <span>The email was sent successfully.</span></div>
                    </asp:Panel>
                    <asp:Panel ID="EmailNotSentPanel" runat="server" Visible="False" EnableViewState="False">
                        <div class="notice" style="text-align: center; font-weight: bold;">
                            <span>The email was not sent. Contact the system administrator for further details.</span></div>
                    </asp:Panel>
                    <asp:Panel ID="pnlLicenseGeneration" runat="server" Visible="False" EnableViewState="False">
                        <div class="notice">
                            <asp:Literal ID="ltLicenseGeneration" runat="server" />
                            <div class="newLicense">
                                <asp:Literal ID="ltNewLicense" runat="server" />
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Repeater ID="AdDetails" runat="server" DataSourceID="AdsDataSource" OnItemDataBound="AdDetails_ItemDataBound">
                        <ItemTemplate>
                            <table>
                                <colgroup span="2">
                                    <col class="col_heading" />
                                    <col class="col_detail" />
                                </colgroup>
                                <tbody>
                                    <tr>
                                        <td class="col_heading">
                                            Released:
                                        </td>
                                        <td class="col_detail">
                                            <asp:Label ID="DateAddedLabel" runat="server" Text='<%# Eval("DateCreated", "{0:D}") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="col_heading">
                                            Category:
                                        </td>
                                        <td class="col_detail">
                                            <asp:HyperLink ID="PostByLink2" runat="server" NavigateUrl='<%# Eval("CategoryID", "/marketplace/browse.aspx?c={0}") %>'
                                                Text='<%# Eval("CategoryName") %>' />
                                        </td>
                                    </tr>
                                    <tr runat="server" id="rwSize">
                                        <td class="col_heading">
                                            Size:
                                        </td>
                                        <td class="col_detail">
                                            <asp:Literal ID="ltSize" runat="server" />
                                        </td>
                                    </tr>
                                     <tr runat="server" id="rwPrice">
                                        <td class="col_heading">
                                            Price: 
                                        </td>
                                        <td class="col_detail">
                                            <asp:Literal ID="ltPrice" runat="server" />
                                        </td>
                                    </tr>
                                    <tr class="spacer">
                                        <td colspan="2">
                                        </td>
                                    </tr>
                                    <tr class="new_section">
                                        <td class="col_detail_description" valign="top" colspan="2">
                                            <asp:Label ID="DescriptionLabel" runat="server" Text='<%# Eval("ProductDescription") %>'></asp:Label>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </ItemTemplate>
                    </asp:Repeater>
                </asp:Panel>
                <div id="adBottom">
                    <a id="content_start"></a>
                    <h3>
                        <asp:Label ID="lblOptions" runat="server" /></h3>
                    <ul>
                        <asp:PlaceHolder ID="AdActions" runat="server">
                            <li runat="server" id="ProductKeyButtonLI2">
                                <asp:LinkButton ID="lbtnProductKeyButton2" runat="server" OnClick="GenerateProductKey_Click" />
                            </li>
                            <SUEETIE:PackageCartLinks ID="PackageCartLinks2" runat="server" IsSideBarLink="false" />
                            <li runat="server" id="LoginButtonLI2">
                                <asp:HyperLink ID="LoginButton2" runat="server" NavigateUrl="/members/login.aspx" /></asp:HyperLink></li>
                            <li runat="server" id="DownloadButtonLI2">
                                <asp:LinkButton ID="DownloadButton2" runat="server" OnClick="Download_Click" />
                            </li>
                            <li>
                                <asp:LinkButton ID="RespondButton2" runat="server" OnClick="RespondButton_Click">Email Author about this Item</asp:LinkButton></li>
                            <li>
                                <asp:LinkButton ID="EmailAdButton2" runat="server" OnClick="EmailAdButton_Click">Send this Item to a Friend</asp:LinkButton></li>
                            <%--                            <li runat="server" id="SaveAddButtonLI2">
                                <asp:LinkButton ID="SaveAdButton2" runat="server" OnClick="SaveAdButton_Click">Add Item to your Bookmarks</asp:LinkButton></li>--%>
                        </asp:PlaceHolder>
                    </ul>
                    <div class="DownloadNote" id="DownloadMessage" style="display: none;">
                        <div class="DownloadNoteTitle">
                            File is Downloading...</div>
                        <strong>Please note: </strong>Larger downloads may require processing time to prepare
                        the file for downloading. You will be prompted to save the file when processing
                        completes, so please resist the urge to click again on the download link.
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:ObjectDataSource ID="AdsDataSource" runat="server" TypeName="Sueetie.Commerce.Products"
        SelectMethod="GetSueetieProduct" OnSelected="AdsDataSource_Selected">
        <SelectParameters>
            <asp:QueryStringParameter Name="ProductID" DefaultValue="0" QueryStringField="id"
                Type="Int32"></asp:QueryStringParameter>
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="PhotosDataSource" runat="server" SelectMethod="GetPhotosByProduct"
        TypeName="Sueetie.Commerce.ProductPhotos">
        <SelectParameters>
            <asp:QueryStringParameter Name="productID" DefaultValue="0" QueryStringField="id"
                Type="Int32"></asp:QueryStringParameter>
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
