<%@ Page Language="C#" Inherits="Sueetie.Commerce.Pages.ShowProductPage" MasterPageFile="~/Themes/v2/Masters/marketplace.master" %>

<%@ Import Namespace="Sueetie.Commerce" %>
<asp:Content ID="cphHeader1" ContentPlaceHolderID="cphHeader" runat="Server">
    <link href="/scripts/jquery.alerts.css" rel="stylesheet" type="text/css" />
    <script type='text/javascript' src='/scripts/jquery.js'></script>
    <script type='text/javascript' src='/scripts/jquery.alerts.js'></script>
    <script type='text/javascript' src='/scripts/jquery-utils.js'></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var qstring = $.parseQuery();
            if (qstring.r > 0) {
                jAlert('Nous sommes désolés, mais ce document n\'est pas disponible actuellement.', 'Document indisponible', null);
            }
        });
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="NavBar" ID="NavBarContent" runat="server">
    <div id="crumbs_text">
    </div>
</asp:Content>
<asp:Content ID="contentLowerSideBar" ContentPlaceHolderID="cphLowerSideBar" runat="server">
    <asp:PlaceHolder ID="phSidebarActions" runat="server"></asp:PlaceHolder>
    <asp:PlaceHolder ID="phSidebarNotActivePanel" runat="server" Visible="false"></asp:PlaceHolder>
    <asp:PlaceHolder ID="phSidebarNotAvailablePanel" runat="server" Visible="false"></asp:PlaceHolder>
</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="cphBody" runat="server">
    <h3 class="ProductTitle">
        <asp:HyperLink ID="hyperlinkTitle" runat="server" /></h3>
    <div class="display_left">
        <asp:DataList OnItemCommand="PhotoList_ItemCommand" ID="PhotoList" runat="Server" DataSourceID="PhotosDataSource" DataKeyField="PhotoID" RepeatLayout="Flow">
            <ItemTemplate>
                <asp:LinkButton ID="AdPhotoImageButton" runat="Server" CommandName="ShowFullSize" CommandArgument='<%# Eval("photoId") %>'><img alt="Enlarge Photo" src='<%# Eval("PhotoID", "/util/handlers/PhotoDisplay.ashx?pid={0}&size=medium&t=2") %>'  /></asp:LinkButton>
            </ItemTemplate>
        </asp:DataList>
    </div>
    <div class="display_right" id="ad_details">
        <asp:Panel ID="AdDetailsPanel" runat="server">
            <h4>
                <asp:Label ID="SubTitleLabel" runat="server" /></h4>
            <asp:Panel ID="PhotoPanel" runat="server" Visible="False">
                <p class="notice">
                    <asp:LinkButton ID="HidePhotoPanel" runat="server" OnClick="HidePhotoPanel_Click">[fermer]</asp:LinkButton>
                </p>
                <p>
                    <asp:ImageButton ID="FullSizePhoto" runat="server" OnClick="FullSizePhoto_Click" AlternateText="Show other product photos if they exist." />
                </p>
            </asp:Panel>
            <asp:Panel ID="ResponsePanel" runat="server" Visible="False">
                <h5 class="action">Contactez-nous à propos de ce document</h5>
                Votre nom :<br />
                <asp:TextBox ID="ResponseContactNameTextBox" runat="server" CssClass="user_info"></asp:TextBox>
                <p>
                    Adresse email de contact :<br />
                    <asp:TextBox ID="ResponseContactEmailTextBox" runat="server" CssClass="user_info"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ResponseContactEmailTextBox" ErrorMessage="A contact email is required." ValidationGroup="RespondPanelGroup" ToolTip="A contact email is required." ID="ResponseEmailRequired1" Display="dynamic" />
                    <asp:RegularExpressionValidator runat="server" ControlToValidate="ResponseContactEmailTextBox" ValidationExpression=".*@.*\..*" ErrorMessage="A valid email is required." ToolTip="A valid email is required." ID="ResponseEmailRequired2" Display="dynamic" />
                </p>
                <p>
                    Commentaires ou questions :<br />
                    <asp:TextBox ID="ResponseCommentsTextBox" runat="server" TextMode="MultiLine" CssClass="ResponseCommentsTextBox"></asp:TextBox>
                </p>
                <p>
                    <asp:Button ID="ResponseSubmitButton" runat="server" Text="Envoyer" OnClick="ResponseSubmitButton_Click" CssClass="TextButtonBig" />
                    <asp:Button ID="CancelResponseButton" runat="server" Text="Annuler" OnClick="CancelResponseButton_Click" CssClass="TextButtonBig" />
                </p>
            </asp:Panel>
            <asp:Panel ID="EmailPanel" runat="server" Visible="false">
                <h5 class="action">Tell a Friend About This Product</h5>
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
                        Height="123px"></asp:TextBox>
                </p>
                <p>
                    &nbsp;<asp:Button ID="EmailSubmitButton" runat="server" Text="Submit" OnClick="EmailSubmitButton_Click"
                        CssClass="TextButtonBig" />
                    <asp:Button ID="CancelEmailButton" runat="server" Text="Cancel" OnClick="CancelEmailButton_Click"
                        CssClass="TextButtonBig" />
                </p>
            </asp:Panel>
            <asp:Panel ID="AdSavedPanel" runat="server" Visible="False" EnableViewState="False">
                <div style="text-align: center;" class="notice">
                    <span>The item has been <a href="MyAds.aspx?saved=1">saved to your list of Bookmarks.</a></span>
                </div>
            </asp:Panel>
            <asp:Panel ID="DownloadErrorPanel" runat="server" Visible="False" EnableViewState="False">
                <div style="text-align: center;" class="notice">
                    <span>An error occurred when attempting to download this file and has been recorded.
                                Please use the <a href="/blog/contact.aspx">contact form</a> to inform us so we
                                can immediately resolve this for you.</span>
                </div>
            </asp:Panel>
            <asp:Panel ID="EmailSentPanel" runat="server" Visible="False" EnableViewState="False">
                <div class="notice" style="text-align: center;">
                    <span>The email was sent successfully.</span>
                </div>
            </asp:Panel>
            <asp:Panel ID="EmailNotSentPanel" runat="server" Visible="False" EnableViewState="False">
                <div class="notice" style="text-align: center; font-weight: bold;">
                    <span>The email was not sent. Contact the system administrator for further details.</span>
                </div>
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
                    <table class="ItemDetail">
                        <colgroup span="2">
                            <col class="col_heading" />
                            <col class="col_detail" />
                        </colgroup>
                        <tbody>
                            <tr>
                                <td class="col_heading">
                                    <SUEETIE:SueetieLocal LanguageFile="marketplace.xml" Key="product_released" runat="server" />
                                    :</td>
                                <td class="col_detail">
                                    <asp:Label ID="DateAddedLabel" runat="server" Text='<%# Eval("DateCreated", "{0:D}") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="col_heading">
                                    <SUEETIE:SueetieLocal LanguageFile="marketplace.xml" Key="product_category" runat="server" />
                                    :</td>
                                <td class="col_detail">
                                    <SUEETIE:CommerceBreadCrumbs ID="CommerceCrumbs1" runat="server" Prefix="false" />
                                    <%--<asp:HyperLink ID="PostByLink2" runat="server" NavigateUrl='<%# Eval("CategoryID", "/marketplace/browse.aspx?c={0}") %>' Text='<%# Eval("CategoryName") %>' />--%>
                                </td>
                            </tr>
                            <tr runat="server" id="rwSize">
                                <td class="col_heading">
                                    <SUEETIE:SueetieLocal LanguageFile="marketplace.xml" Key="product_size" runat="server" />
                                    :
                                </td>
                                <td class="col_detail">
                                    <asp:Literal ID="ltSize" runat="server" />
                                </td>
                            </tr>
                            <tr runat="server" id="rwPrice">
                                <td class="col_heading">
                                    <SUEETIE:SueetieLocal LanguageFile="marketplace.xml" Key="product_price" runat="server" />
                                    : 
                                </td>
                                <td class="col_detail">
                                    <asp:Literal ID="ltPrice" runat="server" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <div class="ItemDescription">
                        <asp:Literal runat="server" Text='<%# Eval("ProductDescription") %>' />
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </asp:Panel>
        <div id="adBottom">
            <a id="content_start"></a>
            <h3>
                <asp:Label ID="lblOptions" runat="server" /></h3>
            <ul>
                <asp:PlaceHolder ID="AdActions" runat="server">
                    <li runat="server" id="ProductKeyButtonLI2"><asp:LinkButton ID="lbtnProductKeyButton2" runat="server" OnClick="GenerateProductKey_Click" /></li>
                    <li runat="server" id="LoginButtonLI2"><asp:HyperLink ID="LoginButton2" runat="server" NavigateUrl="/members/login.aspx" /></li>
                    <li runat="server" id="DownloadButtonLI2"><asp:LinkButton ID="DownloadButton2" runat="server" OnClick="Download_Click" /></li>
                    <li><asp:LinkButton ID="RespondButton2" runat="server" OnClick="RespondButton_Click">Envoyer un email à l'auteur à propos de ce document</asp:LinkButton></li>
                    <li><asp:LinkButton ID="EmailAdButton2" runat="server" OnClick="EmailAdButton_Click">Envoyer un lien vers ce document à un ami</asp:LinkButton></li>
                    <%--                            <li runat="server" id="SaveAddButtonLI2">
                                <asp:LinkButton ID="SaveAdButton2" runat="server" OnClick="SaveAdButton_Click">Add Item to your Bookmarks</asp:LinkButton></li>--%>
                </asp:PlaceHolder>
            </ul>
        </div>
    </div>
    <asp:ObjectDataSource ID="AdsDataSource" runat="server" TypeName="Sueetie.Commerce.Products" SelectMethod="GetSueetieProduct" OnSelected="AdsDataSource_Selected">
        <SelectParameters>
            <asp:QueryStringParameter Name="ProductID" DefaultValue="0" QueryStringField="id" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="PhotosDataSource" runat="server" SelectMethod="GetPhotosByProduct" TypeName="Sueetie.Commerce.ProductPhotos">
        <SelectParameters>
            <asp:QueryStringParameter Name="productID" DefaultValue="0" QueryStringField="id" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
