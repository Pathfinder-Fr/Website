<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.Licensing.Pages.LicenseEntryPage"
    MasterPageFile="/themes/lollipop/masters/admin.master" %>

<%@ Register Src="../controls/adminSiteSettingsNavLinks.ascx" TagName="adminSiteSettingsNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Sueetie Licenses
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="1" />
</asp:Content>
<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminSiteSettingsNavLinks ID="adminSiteSettingsNavLinks1" runat="server" />
</asp:Content>
<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <div class="Licenses">
            <div class="AdminTextTalk">
                <h2>
                    Sueetie Product Keys</h2>
                <div class="AdminFormDescription">
                    <p>
                        Below are the Sueetie product keys currently installed on this community. You
                        can obtain a list of all of your Sueetie Product Keys on your member dashboard at Sueetie.com.
                        You can also upgrade any product keys there as well.<br />
                    </p>
                </div>
            </div>
        </div>
        <asp:GridView ID="LicensesGridView" CssClass="fatgridviewMain licenseTable" runat="server"
            AutoGenerateColumns="False" DataKeyNames="PackageID" EmptyDataText="No licenses found."
            AllowSorting="True" DataSourceID="LicensesDataSource" OnRowDataBound="LicensesGridView_OnRowDataBound"
            OnRowUpdating="LicensesGridView_OnRowUpdating" OnRowCancelingEdit="LicensesGridView_OnRowCancelingEdit">
            <RowStyle CssClass="gridRowStyle" VerticalAlign="Middle" />
            <SelectedRowStyle CssClass="gridrowSelectedBG" VerticalAlign="Middle" />
            <HeaderStyle CssClass="gridheaderBG" />
            <AlternatingRowStyle CssClass="gridAlternateRowStyle" />
            <PagerStyle CssClass="membersGridViewPager2" BorderWidth="0px" />
            <Columns>
                <asp:TemplateField>
                    <HeaderStyle CssClass="gridheaderBG" Width="1px" />
                    <ItemStyle CssClass="gridheaderBG" Width="1px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Product" HeaderStyle-HorizontalAlign="Left">
                    <ItemStyle Width="160px" />
                    <ItemTemplate>
                        <%#Eval("PackageDescription")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Version" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lblVersion" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Product Key" HeaderStyle-HorizontalAlign="Center">
                    <ItemStyle CssClass="aaLong" HorizontalAlign="Center" />
                    <EditItemTemplate>
                        <asp:TextBox ID="txtLicense" runat="server" Text='<%# Bind("License") %>' CssClass="inputLicense"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <%# Eval("License")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Date Created">
                    <ItemStyle Width="100px" HorizontalAlign="Center" />
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "DateLicensed","{0:d}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Product Key Type" HeaderStyle-HorizontalAlign="Center">
                    <ItemStyle Width="100px" HorizontalAlign="Center" />
                    <ItemTemplate>
                        <%#Eval("LicenseTypeDescription")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowDeleteButton="True" ShowEditButton="true" EditText="Update"
                    UpdateText="Apply" ControlStyle-CssClass="ActivityGridButton" ItemStyle-CssClass="gridCommandColumn"
                    DeleteText="Remove" />
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="LicensesDataSource" runat="server" TypeName="Sueetie.Licensing.LicensingCommon"
            SelectMethod="GetLicensedSueetiePackageList" UpdateMethod="UpdateSueetiePackageFromLicense"
            InsertMethod="UpdateSueetiePackageFromLicense" DeleteMethod="ResetSueetiePackage">
        </asp:ObjectDataSource>
        <div class="AddActivitiesForm">
            <div class="UploadTitle">
                <strong>Add New Product Key</strong></div>
            <asp:DetailsView ID="UpdateLicensesDetailsView" runat="server" DataSourceID="LicensesDataSource"
                DefaultMode="Insert" AutoGenerateRows="False" OnItemInserting="LicensesDetailsView_ItemInserting"
                CellPadding="2" GridLines="None" CellSpacing="2" CssClass="AddActivitiesFormTable"
                OnDataBound="UpdateLicensesDetailsView_OnDataBound" >
                <FieldHeaderStyle CssClass="AddActivitiesLabel" />
                <Fields>
                    <asp:TemplateField HeaderText="Enter Product Key">
                    <ItemStyle CssClass="aaLong" />
                        <ItemTemplate>
                            <asp:TextBox ID="txtLicense" runat="server"  />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ShowInsertButton="True" InsertText="Add" ButtonType="Button" ShowCancelButton="False"
                        ControlStyle-CssClass="ActivityGridButton"></asp:CommandField>
                </Fields>
                <InsertRowStyle Width="100%"></InsertRowStyle>
            </asp:DetailsView>
            <asp:Label ID="lblResults" runat="server" CssClass="ResultsMessage licenseMessage" />
        </div>
    </div>
</asp:Content>
