<%@ Page Language="C#" MasterPageFile="/Themes/Lollipop/Masters/admin.master"
    Inherits="Sueetie.Commerce.Pages.ManageCategoriesPage" Title="Sueetie Marketplace - Manage Categories" %>

<%@ Register Src="../controls/adminMarketplaceNavLinks.ascx" TagName="adminMarketplaceNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<%@ Register TagPrefix="uc2" TagName="CategoryDropDown" Src="/Controls/Marketplace/CategoryDropDown.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CategoryPath" Src="/Controls/Marketplace/CategoryPath.ascx" %>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Sueetie Marketplace - Manage Categories
</asp:Content>
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
                <h2>Manage Product Categories</h2>
                <div class="AdminFormDescription">
                    <p>
                        Manage Sueetie Marketplace product categories.<br />
                    </p>
                </div>
            </div>
        </div>
        <div class="MarketPlaceAdministration">
            <div class="addProductCategoryPath">
                <uc1:CategoryPath ID="CategoryPath" runat="server" OnCategorySelectionChanged="CategoryPath_CategorySelectionChanged" />
            </div>
            <div class="manageCategoryCurrent">
                Current Selection:
                <asp:Label ID="CurrentCategoryLabel" runat="server"></asp:Label>
            </div>
            <div class="formSubListArea">
                <div class="formSubHeading">
                    Subcategories
                </div>
                <asp:ListBox ID="SubcategoriesList" runat="server" DataSourceID="SubcategoryDataSource"
                    DataValueField="CategoryID" DataTextField="CategoryName" AutoPostBack="True"
                    OnSelectedIndexChanged="SubcategoriesList_SelectedIndexChanged" OnDataBound="SubcategoriesList_DataBound"
                    Width="350px" Rows="5" CssClass="manageCategoryListBox"></asp:ListBox>
            </div>
            <h3 class="section">Actions for
                <asp:Label ID="CurrentCategoryActionLabel" runat="server" Font-Underline="true"></asp:Label>:</h3>
            <div class="formSubForm">
                <div class="formSubHeading">
                    Add Subcategory
                </div>
                <asp:TextBox ID="txtCategoryName" runat="server" ValidationGroup="NewCategory" CssClass="fat_title"></asp:TextBox>
                <asp:TextBox ID="txtCategoryDescription" runat="server" CssClass="fat_description"
                    TextMode="MultiLine" />
                <br />
                <asp:Button ID="AddCategoryButton" runat="server" Text="Add Subcategory" ValidationGroup="NewCategory"
                    OnClick="AddCategoryButton_Click" CssClass="TextButtonBig" />
                <asp:RequiredFieldValidator ID="RequiredCategoryValidator" runat="server" ValidationGroup="NewCategory"
                    ControlToValidate="txtCategoryName" ErrorMessage="*">
                </asp:RequiredFieldValidator>
            </div>
            <div class="formSubForm">
                <div class="formSubHeading">
                    Edit Category
                </div>
                <asp:TextBox ID="txtEditCategoryName" runat="server" ValidationGroup="CategoryRename"
                    CssClass="fat_title"></asp:TextBox>
                <asp:TextBox ID="txtEditCategoryDescription" runat="server" CssClass="fat_description"
                    TextMode="MultiLine" />
                <br />
                <asp:Button ID="RenameCategoryButton" runat="server" Text="Edit Category" OnClick="RenameCategoryButton_Click"
                    ValidationGroup="CategoryRename" CssClass="TextButtonBig" />
                <asp:RequiredFieldValidator ID="RequiredCategoryNameValidator" runat="server" ValidationGroup="CategoryRename"
                    ErrorMessage="*" ControlToValidate="txtEditCategoryName">
                </asp:RequiredFieldValidator>
            </div>
            <div class="formSubForm">
                <div class="formSubHeading">
                    Move
                </div>
                <asp:RadioButtonList ID="MoveAction" runat="server" ValidationGroup="MoveCategory">
                    <asp:ListItem Value="Category" Selected="True">Move Category (incl. any sub-categories)</asp:ListItem>
                    <asp:ListItem Value="Ads">Move all Products in this Category</asp:ListItem>
                </asp:RadioButtonList>
                <br />
                move to: &nbsp;<uc2:CategoryDropDown ID="CategoryDropDown" runat="server" AllCategoriesOptionText="[Top-Level]"></uc2:CategoryDropDown>
                <br />
                <br />
                <asp:Button ID="MoveButton" runat="server" Text="Move" OnClick="MoveButton_Click"
                    ValidationGroup="MoveCategory" CssClass="TextButtonBig" />
            </div>
            <div class="formSubForm">
                <div class="formSubHeading">
                    Delete
                </div>
                To delete the Category, it be must be empty, containing no sub-categories or products
                <br />
                <br />
                <asp:Button ID="RemoveCategoryButton" runat="server" Text="Remove" OnClick="RemoveCategoryButton_Click"
                    CssClass="TextButtonBig" />
            </div>
            <asp:ObjectDataSource ID="SubcategoryDataSource" runat="server" TypeName="Sueetie.Commerce.CategoryCache"
                SelectMethod="GetCategoriesByParentId">
                <SelectParameters>
                    <asp:ControlParameter ControlID="CategoryPath" PropertyName="CurrentCategoryId" Type="Int32" DefaultValue="0" Name="parentCategoryId" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </div>
</asp:Content>
