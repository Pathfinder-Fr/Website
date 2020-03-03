<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.AddonPack.Pages.SlideshowImagePage" MasterPageFile="/Themes/Lollipop/Masters/admin.master" %>

<%@ Register Src="/admin/controls/adminAddonPackNavLinks.ascx" TagName="adminAddonPackNavLinks" TagPrefix="uc1" %>
<%@ Register Src="/admin/controls/adminSideNavigation.ascx" TagName="adminSideNavigation" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server" />

<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Manage Slideshow Images
</asp:Content>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="5" />
</asp:Content>

<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminAddonPackNavLinks ID="adminAddonPackNavLinks1" runat="server" />
</asp:Content>

<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <div class="AdminTextTalk">
            <h2>
                Manage Slideshow Images</h2>
            <div class="AdminFormDescription">
                <p>
                    Use this function to add, update or deactivate site slideshow images. First column indicates display status ("x" if not displayed), second column display order. Images are displayed by display order, first displayed at grid top.</p>
            </div>
        </div>
        <h3 class="section slideshow">
            Slideshow:
            <asp:Label ID="lblSlideshow" runat="server" Font-Bold="true" /></h3>
            <asp:Label ID="UploadErrorMessage" runat="server" Visible="False" ForeColor="Red">
                    The selected file was not a recognizable image type.
            </asp:Label>
            <asp:GridView ID="PhotoGridView" runat="server" DataSourceID="PhotoDataSource" AutoGenerateColumns="False"
                DataKeyNames="slideshowimageID"  OnRowUpdating="PhotoGridView_OnRowUpdating"
                ShowFooter="false" ShowHeader="false" CellPadding="8" CssClass="SlideshowGrid" >
                <EmptyDataTemplate>
                    No images have yet been uploaded for this Slideshow.
                </EmptyDataTemplate>
                <Columns>
                  <asp:TemplateField HeaderText="IsActive">
                    <EditItemTemplate>
                        <asp:CheckBox ID="chkIsActive" runat="server" ToolTip="Clear if you do not want to display this slideshow" Checked='<%#Eval("IsActive")%>' />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <img src='/images/shared/sueetie/<%#Eval("IsActive")%>.png' alt='<%#Eval("IsActive")%>' />
                    </ItemTemplate>
                    <ItemStyle Width="25px" HorizontalAlign="Center" />
                </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Display Order">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtDisplayOrder" runat="server" Text='<%#Eval("DisplayOrder")%>' CssClass="DisplayOrder" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <%#Eval("DisplayOrder") %>
                    </ItemTemplate>
                    <ItemStyle Width="25px" HorizontalAlign="Center" />
                </asp:TemplateField>
                    <asp:TemplateField ItemStyle-CssClass="SlideshowImage">
                        <ItemTemplate>
                            <img src='<%# Eval("slideshowimageID", "/images/slideshows/{0}.LG.JPG") %>'
                                alt="Slideshow Image" width="200" height="125"  /></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-CssClass="ImageDescription">
                   <ItemTemplate>
                   <%# Eval("ImageDescription")%>
                   </ItemTemplate>
                    <EditItemTemplate>
                    <asp:TextBox ID="ImageDescription" runat="server" Text='<%# Bind("ImageDescription") %>' CssClass="ImageDescription"  TextMode="MultiLine"/>
                    </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ShowDeleteButton="True" DeleteText="Remove" ShowEditButton="true" UpdateText="Update" ControlStyle-CssClass="SlideshowButton" ItemStyle-CssClass="ButtonColumn" />
                </Columns>
                <RowStyle CssClass="slideshowrow1"></RowStyle>
                <AlternatingRowStyle CssClass="slideshowrow2"></AlternatingRowStyle>
                <FooterStyle CssClass="item_list_footer"></FooterStyle>
                <HeaderStyle CssClass="item_list_footer"></HeaderStyle>
            </asp:GridView>
            <asp:ObjectDataSource ID="PhotoDataSource" runat="server" TypeName="Sueetie.AddonPack.SueetieSlideshows"
                SelectMethod="GetSueetieSlideshowImageList" DeleteMethod="DeleteSlideshowImage" InsertMethod="CreateSlideshowImage"
                UpdateMethod="UpdateSlideshowImage" OldValuesParameterFormatString="{0}">
                <UpdateParameters>
                    <asp:Parameter Name="ImageDescription" Type="String" />
                    <asp:Parameter Name="displayOrder" Type="Int32" />
                    <asp:Parameter Name="IsActive" Type="Boolean" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:QueryStringParameter Name="slideshowID" DefaultValue="0" QueryStringField="id"
                        Type="Int32"></asp:QueryStringParameter>
                </SelectParameters>
                <InsertParameters>
                    <asp:QueryStringParameter Name="slideshowID" DefaultValue="0" QueryStringField="id"
                        Type="Int32"></asp:QueryStringParameter>
                    <asp:Parameter Name="bytesFull"></asp:Parameter>
                    <asp:Parameter Name="bytesMedium"></asp:Parameter>
                    <asp:Parameter Name="bytesSmall"></asp:Parameter>
                    <asp:Parameter Name="ImageDescription" Type="String" />
                </InsertParameters>
            </asp:ObjectDataSource>
            <div class="UploadForm">
            <asp:DetailsView ID="UploadPhotoDetailsView" runat="server" DataSourceID="PhotoDataSource"
                DefaultMode="Insert" DataKeyNames="slideshowimageID" AutoGenerateRows="False"
                OnItemInserting="UploadPhotoDetailsView_ItemInserting" CellPadding="5" GridLines="None"
                CellSpacing="5" CssClass="SlideShowUpload">
                <Fields>
                    <asp:TemplateField>
                        <InsertItemTemplate>
                        <div class="UploadTitle"><strong>Upload slideshow image and add description</strong> (Description optional)</div>
                            <asp:FileUpload ID="PhotoFile" runat="server" FileBytes='<%# Bind("UploadBytes") %>'  CssClass="SlideUpload" />
                            
                            <asp:TextBox ID="ImageDescription" runat="server" Text='<%# Bind("ImageDescription") %>' CssClass="ImageDescription"  TextMode="MultiLine"/>
                        </InsertItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ShowInsertButton="True" InsertText="Upload" ButtonType="Button"
                        ShowCancelButton="False" ControlStyle-CssClass="TextButtonBig"></asp:CommandField>
                </Fields>
                <InsertRowStyle Width="50%"></InsertRowStyle>
            </asp:DetailsView>
            </div>

    </div>
</asp:Content>
