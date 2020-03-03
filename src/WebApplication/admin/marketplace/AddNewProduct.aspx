<%@ Page Language="C#" Inherits="Sueetie.Commerce.Pages.AddNewProductPage" Title="Marketplace - Add a new Product" ValidateRequest="false" MasterPageFile="/Themes/Lollipop/Masters/admin.master" %>

<%@ Register TagPrefix="uc1" TagName="CategoryPath" Src="/Controls/marketplace/CategoryPath.ascx" %>
<%@ Register Src="/controls/editors/htmlEditor.ascx" TagPrefix="cc2" TagName="TextEditor" %>
<%@ Register Src="../controls/adminMarketplaceNavLinks.ascx" TagName="adminMarketplaceNavLinks" TagPrefix="uc1" %>
<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation" TagPrefix="uc3" %>

<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">Sueetie Marketplace - Add New Product</asp:Content>
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
                <h2>Add New Marketplace Products</h2>
                <div class="AdminFormDescription">
                    <p>
                        Add new Sueetie Marketplace products.<br />
                    </p>
                </div>
            </div>
        </div>
        <div class="MarketPlaceAdministration">
            <asp:PlaceHolder ID="phHasCategories" runat="server">
                <asp:Wizard ID="PostAdWizard" runat="server" OnFinishButtonClick="PostAdWizard_FinishButtonClick" DisplaySideBar="False" CssClass="wizard" StepStyle-CssClass="wizard-step" ActiveStepIndex="0"
                    NavigationButtonStyle-CssClass="TextButtonBigLeft" CancelButtonStyle-CssClass="TextButtonBig" StepPreviousButtonStyle-CssClass="TextButtonBigLeft" OnPreviousButtonClick="PostAdWizard_PreviousButtonClick"
                    NavigationStyle-HorizontalAlign="Left" Width="680px">
                    <WizardSteps>
                        <asp:WizardStep ID="WizardStep1" runat="server" Title="Category Selection">
                            <h3 class="section">List a Product: Category</h3>
                            <div class="select_category">Select a Product Category</div>
                            <div class="addProductCategoryPath">
                                <uc1:CategoryPath ID="CategoryPath" runat="server" OnCategorySelectionChanged="CategoryPath_CategorySelectionChanged" />
                            </div>
                            <asp:DataList runat="server" ID="SubcategoriesList" DataSourceID="SubcategoriesDS" OnItemCommand="SubcategoriesList_ItemCommand" CellSpacing="2" RepeatColumns="2" CssClass="SubCategoryList">
                                <ItemTemplate>
                                    <asp:LinkButton runat="Server" ID="CategoryButton" CommandArgument='<%# Eval("CategoryID") %>' Text='<%# Eval("CategoryName") %>' />
                                </ItemTemplate>
                            </asp:DataList>
                            <asp:ObjectDataSource ID="SubcategoriesDS" runat="server" TypeName="Sueetie.Commerce.CategoryCache" SelectMethod="GetCategoriesByParentId" OnSelected="SubcategoriesDS_Selected">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="CategoryPath" PropertyName="CurrentCategoryId" Type="Int32" DefaultValue="0" Name="parentCategoryId" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </asp:WizardStep>
                        <asp:WizardStep ID="AdDetailsStep" runat="server" Title="Enter Product Details">
                            <h3 class="section">List a Product: Details</h3>
                            <div class="select_category">Product Category:</div>
                            <div class="newProductCategoryLine">
                                <asp:Label runat="server" ID="CategoryPathLabel" />
                                |
                                <asp:LinkButton runat="server" ID="ChangeCategoryButton" OnClick="ChangeCategoryButton_Click" ValidationGroup="ChangeCategory">Change</asp:LinkButton>
                            </div>
                            <p>
                                <asp:ValidationSummary runat="server" ID="ValidationSummary1" HeaderText="Please correct the following:" />
                            </p>
                            <p></p>
                            <div class="mpFormLabel">Purchase Type:</div>
                            <div class="mpFormField">
                                <asp:RadioButtonList runat="server" ID="rblPurchaseTypes" CssClass="mpRadioButtons" />
                            </div>
                            <div class="mpFormLabel">Product Type:</div>
                            <div class="mpFormField">
                                <asp:RadioButtonList runat="server" ID="rblProductTypes" CssClass="mpRadioButtons" />
                            </div>
                            <div class="mpFormLine">
                                <div class="mpFormLabel">Title:</div>
                                <div class="mpFormFieldLong">
                                    <asp:TextBox Text='<%# Bind("Title") %>' runat="server" ID="TitleTextBox" CssClass="post_title" />
                                    <asp:RequiredFieldValidator runat="server" ErrorMessage="A Title for the ad is required." ID="RequiredTitle" ControlToValidate="TitleTextBox">*</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="mpFormLine">
                                <div class="mpFormLabel">SubTitle:</div>
                                <div class="mpFormFieldLong">
                                    <asp:TextBox Text='<%# Bind("SubTitle") %>' runat="server" ID="SubTitleTextBox" TextMode="MultiLine" Rows="2" CssClass="post_subtitle" />
                                    <asp:RequiredFieldValidator runat="server" ErrorMessage="A SubTitle is required." ID="RequiredFieldValidator1" ControlToValidate="SubTitleTextBox">*</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="mpFormLine">
                                <div class="mpFormLabel">Description:</div>
                                <div class="mpFormEditorField">
                                    <cc2:TextEditor runat="server" ID="DescriptionTextBox" Text='<%# Bind("Description") %>' />
                                </div>
                            </div>
                            <div class="mpFormLine">
                                <div class="mpFormLabel">
                                    File name:<br />
                                    <span class="small_text">(for downloads)</span>
                                </div>
                                <div class="mpFormFieldLong">
                                    <asp:TextBox Text='<%# Bind("URL") %>' runat="server" ID="UrlTextBox" CssClass="post_url" />
                                </div>
                            </div>
                            <div class="mpFormLine">
                                <div class="mpFormLabel">File:</div>
                                <asp:FileUpload ID="FileUploadControl" runat="server" />
                            </div>
                            <div class="mpFormLine">
                                <div class="mpFormLabel">Price: <span class="small_text">("0" if free)</span></div>
                                <div class="mpFormField">
                                    <asp:TextBox runat="server" ID="PriceTextBox" CssClass="post_dollars" Text="0.00"></asp:TextBox>
                                    <asp:CustomValidator ID="PriceValidator" runat="server" ErrorMessage="The Price is not valid." OnServerValidate="PriceValidator_ServerValidate">*</asp:CustomValidator>
                                </div>
                            </div>
                        </asp:WizardStep>
                        <asp:WizardStep ID="WizardStep2" runat="server" StepType="Complete" Title="Done">
                            <h3 class="section">Done!</h3>
                            <div class="AdSubmitted">New product was successfully entered!</div>
                            <p style="text-align: center">
                                <asp:HyperLink runat="server" ID="UploadImagesLink" Font-Bold="True">Upload Product Photos</asp:HyperLink>
                                |
                                <asp:HyperLink runat="server" ID="MyAdsLink" Font-Bold="True" NavigateUrl="AddNewProduct.aspx">Add Another Product</asp:HyperLink>
                            </p>
                        </asp:WizardStep>
                    </WizardSteps>
                    <StepStyle CssClass="wizard-step" />
                    <NavigationStyle HorizontalAlign="Left" />
                    <StartNextButtonStyle CssClass="NoDisplay" />
                </asp:Wizard>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="phNoCategories" runat="server">
                <div class="MPNoCategoriesArea">
                    <asp:Label ID="lblNeedCategories" runat="server" CssClass="MPNoCategories" />
                </div>
            </asp:PlaceHolder>
        </div>
    </div>
    <script type="text/javascript">
        function textCounter(elem, maxLimit) {
            if (elem.value.length > maxLimit) {
                elem.value = elem.value.substring(0, maxLimit);
            }
        }
    </script>
</asp:Content>
