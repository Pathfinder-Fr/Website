<%@ Page Language="C#" Inherits="Sueetie.Commerce.Pages.EditProductPage" Title="Sueetie Marketplace - Update Product" ValidateRequest="false" MasterPageFile="/Themes/Lollipop/Masters/admin.master" %>

<%@ Register TagPrefix="uc1" TagName="CategoryDropDown" Src="/Controls/Marketplace/CategoryDropDown.ascx" %>
<%@ Register Src="/controls/editors/htmlEditor.ascx" TagPrefix="cc2" TagName="TextEditor" %>
<%@ Register Src="../controls/adminMarketplaceNavLinks.ascx" TagName="adminMarketplaceNavLinks" TagPrefix="uc1" %>
<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation" TagPrefix="uc3" %>
<script runat="server">

    protected void FileFormView_ItemUpdating(object sender, FormViewUpdateEventArgs e)
    {

    }

    protected void FileFormView_DataBound(object sender, EventArgs e)
    {

    }
</script>

<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Marketplace - Update Product
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="7" />
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminMarketplaceNavLinks ID="adminMarketplaceNavLinks1" runat="server" />
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <div class="Activities">
            <div class="AdminTextTalk">
                <h2>Update Marketplace Product</h2>
                <div class="AdminFormDescription">
                    <p>
                        Edit Marketplace products.
                    </p>
                </div>
            </div>
        </div>
        <div class="MarketPlaceAdministration">
            <div class="editProductLinks">
                <asp:HyperLink ID="ManagePhotosLink" runat="server">Manage this Product's Photos</asp:HyperLink>
                |
                <asp:HyperLink ID="ShowProductLink" runat="server">Show Item</asp:HyperLink>
            </div>
            <div class="editProductForm">
                <div class="AdminFormLabel">Category:</div>
                <asp:PlaceHolder ID="ChangeCategoryPanel" runat="server" Visible="False">
                    <div class="editProductCategoryPanel">
                        <uc1:CategoryDropDown ID="CategoryDropDown" runat="server" AllCategoriesOptionVisible="false"></uc1:CategoryDropDown>
                        <p>
                            <asp:Button ID="ChangeCategoryOkButton" runat="server" Text="  Ok  " OnClick="ChangeCategoryOkButton_Click" CssClass="TextButtonBig" />
                            <asp:Button ID="ChangeCategoryCancelButton" runat="server" Text="Cancel" OnClick="ChangeCategoryCancelButton_Click" CssClass="TextButtonBig" />
                        </p>
                    </div>
                </asp:PlaceHolder>
                <asp:FormView ID="AdFormView" runat="server" DataSourceID="AdDataSource" DefaultMode="Edit" DataKeyNames="ProductID" OnItemUpdating="AdFormView_ItemUpdating" OnDataBound="AdFormView_OnDataBound" CssClass="editProductFormTable">
                    <EditItemTemplate>
                        <asp:Label Text='<%# Eval("CategoryName") %>' runat="server" ID="CategoryNameLabel" />
                        |
                        <asp:LinkButton ID="ChangeCategoryButton" runat="Server" OnClick="ChangeCategoryButton_Click">Change Category</asp:LinkButton>
                        <p>
                            <asp:ValidationSummary runat="server" ID="ValidationSummary" HeaderText="Please correct the following:" />
                        </p>
                        <div class="editProductFormTopLine">
                            <div class="mpEditLabel">Title: <span class="small_text">(100 characters max)</span></div>
                            <asp:TextBox Text='<%# Bind("Title") %>' runat="server" ID="TitleTextBox" CssClass="fat_title" MaxLength="50"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="A Title for the ad is required." ID="RequiredTitle" ControlToValidate="TitleTextBox">*</asp:RequiredFieldValidator>
                        </div>
                        <div class="mpFormLine">
                            <div class="mpEditLabel">SubTitle: <span class="small_text">(500 characters max)</span></div>
                            <asp:TextBox Text='<%# Bind("SubTitle") %>' runat="server" ID="SubTitleTextBox" CssClass="fat_subtitle" TextMode="MultiLine" Columns="80"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="The Product SubTitle is required." ID="RequiredSubTitle" ControlToValidate="SubTitleTextBox">*</asp:RequiredFieldValidator>
                        </div>
                        <div class="mpFormLine">
                            <div class="mpEditLabel">Description: <span class="small_text">(unlimited characters)</span></div>
                            <cc2:TextEditor runat="server" ID="DescriptionTextBox" Text='<%# Bind("ProductDescription") %>' />
                        </div>
                        <div class="mpFormLine">
                            <div class="mpEditLabel">Filename: <span class="small_text">(for downloads)</span></div>
                            <asp:TextBox Text='<%# Bind("DownloadURL") %>' runat="server" ID="URLTextBox" CssClass="fat_title"></asp:TextBox>
                        </div>
                        <div class="mpFormLine">
                            <div class="mpEditLabel">
                                Price: $
                                <asp:TextBox Text='<%# Bind("Price", "{0:f2}") %>' runat="server" ID="PriceTextBox" CssClass="fat_price" />
                                <asp:CustomValidator ID="PriceValidator" runat="server" ControlToValidate="PriceTextBox" ErrorMessage="The Price is not valid." OnServerValidate="PriceValidator_ServerValidate">*</asp:CustomValidator>
                            </div>
                        </div>
                        <div class="mpFormLine">
                            <div class="mpEditLabel">Purchase Type:</div>
                            <asp:DropDownList ID="ddPurchaseTypes" runat="server" CssClass="productTypes" />
                        </div>
                        <div class="mpFormLabel">Product Type:</div>
                        <div class="mpFormField">
                            <asp:RadioButtonList runat="server" ID="rblProductTypes" CssClass="mpRadioButtons" />
                        </div>
                        <div class="mpFormLine">
                            <div class="mpEditLabel">Product Active Status:</div>
                            <asp:DropDownList ID="ddStatusTypes" runat="server" CssClass="productTypes" />
                        </div>
                        <div class="mpFormLine">
                            <asp:Button ID="UpdateButton" runat="server" Text="Save" CommandName="Update" CssClass="TextButtonBig" />
                            <asp:Button ID="CancelButton" runat="server" Text="Cancel" OnClick="CancelButton_Click" CssClass="TextButtonBig" />
                        </div>
                    </EditItemTemplate>
                </asp:FormView>

                <div class="mpFormLine">
                    <div class="mpEditLabel">Upload file:</div>
                    <div class="mpFormField">
                        <asp:FileUpload ID="UploadFileControl" runat="server" />
                    </div>
                </div>
                <div class="mpFormLine">
                    <asp:Button ID="UploadFileButton" runat="server" Text="Save" CssClass="TextButtonBig" OnClick="UploadFileButton_Click" />
                </div>
            </div>
            <asp:Label ID="lblError" runat="server" />
            <asp:ObjectDataSource ID="AdDataSource" runat="server" TypeName="Sueetie.Commerce.Products" SelectMethod="GetSueetieProduct" UpdateMethod="UpdateSueetieProduct" OnUpdated="AdDataSource_Updated" OnSelected="AdDataSource_Selected" OnUpdating="AdDataSource_OnUpdating">
                <SelectParameters>
                    <asp:QueryStringParameter Name="productID" DefaultValue="0" Type="Int32" QueryStringField="id" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </div>
</asp:Content>
