<%@ Page Language="C#" MasterPageFile="/Themes/Lollipop/Masters/admin.master" Inherits="Sueetie.Commerce.Pages.ManageProductPhotosPage" Title="Sueetie Marketplace - Manage Photos" %>

<%@ Register Src="../controls/adminMarketplaceNavLinks.ascx" TagName="adminMarketplaceNavLinks" TagPrefix="uc1" %>
<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation" TagPrefix="uc3" %>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">Sueetie Marketplace - Manage Photos</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="7" />
</asp:Content>

<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminMarketplaceNavLinks ID="adminMarketplaceNavLinks1" runat="server" />
</asp:Content>

<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <div class="Activities">
            <div class="AdminTextTalk">
                <h2>Manage Product Photos</h2>
                <div class="AdminFormDescription">
                    <p>
                        Manage Sueetie Marketplace product photos.<br />
                    </p>
                </div>
            </div>
        </div>
        <div class="MarketPlaceAdministration">
            <div class="editProductLinks">
                <asp:HyperLink ID="BackToEditAdLink" runat="server">Edit this Product</asp:HyperLink>
                |                
                <asp:HyperLink ID="ShowProductLink" runat="server">Show Item</asp:HyperLink>
            </div>
            <div class="editProductForm">
                <div class="managePhotosProduct">
                    Product:                    
                    <asp:Label ID="AdTitleLabel" runat="server" Font-Bold="True" />
                </div>
                <asp:Panel ID="NoUploadsPanel" runat="server" Visible="False">
                    <p>
                        The site does not offer uploading ad photos at this point.
                    </p>
                </asp:Panel>
                <asp:Panel ID="MainUploadsPanel" runat="server">
                    <div class="managePhotoUploadMessage">
                        Use the
                        <img src="/themes/lollipop/Images/marketplace/non-preview-photo.gif" alt="Non Preview Photo" />
                        icon to select a different thumbnail photo for your ad.
                    </div>
                    <asp:Label ID="UploadErrorMessage" runat="server" Visible="False" ForeColor="Red">The selected file was not a recognizable image type.</asp:Label>
                    <div class="managePhotoTableArea">
                        <asp:GridView ID="PhotoGridView" runat="server" DataSourceID="PhotoDataSource" AutoGenerateColumns="False" DataKeyNames="PhotoID" OnRowCommand="PhotoGridView_RowCommand" BorderWidth="1" ShowFooter="false" ShowHeader="false" CellPadding="10" CssClass="managePhotoTable">
                            <EmptyDataTemplate>No photos have been uploaded for this Ad.</EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Image runat="Server" ID="IsCurrentPreview" Visible='<%# ((bool)Eval("IsMainPreview")) %>' ImageUrl="/themes/lollipop/Images/marketplace/preview-photo.gif" AlternateText="Icon indicating that there are photos for this ad." />
                                        <asp:ImageButton ID="SelectAsPreviewLink" runat="Server" CommandName="SelectAsPreview" AlternateText="Select as Preview" CommandArgument='<%# Eval("PhotoId") %>' Visible='<%# !((bool)Eval("IsMainPreview")) %>' ImageUrl="/themes/lollipop/Images/marketplace/non-preview-photo.gif" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" CssClass="managePhotoPicCell" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle HorizontalAlign="Center" CssClass="managePhotoThumbnailCell" />
                                    <ItemTemplate>
                                        <img src='<%# Eval("PhotoID", "/util/handlers/PhotoDisplay.ashx?pid={0}&size=medium&t=2") %>' style="border: 1px;" alt="Photo of Ad" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField ShowDeleteButton="True" DeleteText="Remove" ItemStyle-CssClass="managePhotoCommandCell" />
                            </Columns>
                            <RowStyle CssClass="row1" />
                            <AlternatingRowStyle CssClass="row2" />
                            <FooterStyle CssClass="item_list_footer" />
                            <HeaderStyle CssClass="item_list_footer" />
                        </asp:GridView>
                    </div>
                    <asp:ObjectDataSource ID="PhotoDataSource" runat="server" TypeName="Sueetie.Commerce.ProductPhotos" SelectMethod="GetPhotosByProduct" DeleteMethod="DeletePhoto" InsertMethod="InsertPhoto" OldValuesParameterFormatString="{0}" OnSelected="PhotoDataSource_Selected">
                        <SelectParameters>
                            <asp:QueryStringParameter Name="productID" DefaultValue="0" QueryStringField="id" Type="Int32" />
                        </SelectParameters>
                        <InsertParameters>
                            <asp:QueryStringParameter Name="productID" DefaultValue="0" QueryStringField="id" Type="Int32" />
                            <asp:Parameter Name="bytesFull" />
                            <asp:Parameter Name="bytesMedium" />
                            <asp:Parameter Name="bytesSmall" />
                            <asp:Parameter Name="useAsPreview" Type="boolean" />
                        </InsertParameters>
                    </asp:ObjectDataSource>
                    <asp:DetailsView ID="UploadPhotoDetailsView" runat="server" DataSourceID="PhotoDataSource" DefaultMode="Insert" DataKeyNames="PhotoID" AutoGenerateRows="False" OnItemInserting="UploadPhotoDetailsView_ItemInserting" CellPadding="5" GridLines="None" CellSpacing="5">
                        <Fields>
                            <asp:TemplateField>
                                <InsertItemTemplate>
                                    <asp:FileUpload ID="PhotoFile" runat="server" FileBytes='<%# Bind("UploadBytes") %>' />
                                </InsertItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowInsertButton="True" InsertText="Upload" ButtonType="Button" ShowCancelButton="False" ControlStyle-CssClass="TextButtonBig" />
                        </Fields>
                        <InsertRowStyle Width="50%" />
                    </asp:DetailsView>
                </asp:Panel>
            </div>
        </div>
    </div>
</asp:Content>
