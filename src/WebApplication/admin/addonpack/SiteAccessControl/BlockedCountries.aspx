<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.AddonPack.Pages.BlockedCountriesPage" MasterPageFile="/themes/lollipop/masters/admin.master" %>

<%@ Register Src="/admin/controls/adminAddonPackNavLinks.ascx" TagName="adminAddonPackNavLinks" TagPrefix="uc1" %>
<%@ Register Src="/admin/controls/adminSideNavigation.ascx" TagName="adminSideNavigation" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">
</asp:Content>

<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Manage Blocked Country List
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="5" />
</asp:Content>
<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminAddonPackNavLinks ID="adminAddonPackNavLinks1" runat="server" />
</asp:Content>
<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <div class="Countries">
            <div class="AdminTextTalk">
                <h2>
                    Blocked Countries</h2>
                <div class="AdminFormDescription">
                    <p>
                        Edit, Add and Delete countries in Client Access Control. The countries in this list work in tandem with imported IP lookup files you maintain in /util/ips.<br />
                    </p>
                </div>
            </div>
        </div>
        <asp:GridView ID="CountriesGridView" CssClass="fatgridviewMain countryTable" runat="server" AutoGenerateColumns="False"
            DataKeyNames="CountryID" EmptyDataText="No records found." AllowSorting="True"
            DataSourceID="CountriesDataSource" OnRowUpdating="CountriesGridView_OnRowUpdating"
            OnRowDataBound="CountriesGridView_OnRowDataBound">
            <RowStyle CssClass="gridRowStyle" />
            <SelectedRowStyle CssClass="gridrowSelectedBG" />
            <HeaderStyle CssClass="gridheaderBG" />
            <AlternatingRowStyle CssClass="gridAlternateRowStyle" />
            <PagerStyle CssClass="membersGridViewPager2" BorderWidth="0px" />
            <Columns>
                <asp:TemplateField>
                    <HeaderStyle CssClass="gridheaderBG" Width="1px" />
                    <ItemStyle CssClass="gridheaderBG" Width="1px" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        ID
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%#Eval("CountryID")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Country Name">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtCountryName" runat="server" Text='<%# Bind("CountryName") %>'
                            CssClass="ApplicationLongInput"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <%#Eval("CountryName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Country Code" ItemStyle-CssClass="countryCodeColumn" HeaderStyle-HorizontalAlign="Center">
                   <EditItemTemplate>
                        <asp:TextBox ID="txtCountryCode" runat="server" Text='<%# Bind("CountryCode") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <%#Eval("CountryCode")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Last IP Update" ItemStyle-CssClass="lastUpdateColumn" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%# Eval("LastUpdate")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Blocked" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <img src='/images/shared/sueetie/<%#Eval("IsBlocked")%>.png' alt='<%#Eval("IsBlocked")%>' />
                    </ItemTemplate>
                    <ItemStyle Width="50px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:CommandField ShowDeleteButton="True" ShowEditButton="true" UpdateText="Update"
                    ControlStyle-CssClass="ActivityGridButton" ItemStyle-CssClass="gridCommandColumn" />
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="CountriesDataSource" runat="server" TypeName="Sueetie.AddonPack.IpBlocker.BlockedIPs"
            SelectMethod="GetBlockedIpCountryList" UpdateMethod="UpdateBlockedIpCountry"
            InsertMethod="CreateBlockedIpCountry" DeleteMethod="DeleteBlockedIpCountry">
            <UpdateParameters>
                <asp:Parameter Name="CountryName" Type="String" />
                <asp:Parameter Name="CountryCode" Type="String" />
            </UpdateParameters>
            <InsertParameters>
            </InsertParameters>
        </asp:ObjectDataSource>
        <div class="AddActivitiesForm">
            <div class="UploadTitle">
                <strong>Add Country</strong> (All fields are required)</div>
            <asp:DetailsView ID="UpdateCountriesDetailsView" runat="server" DataSourceID="CountriesDataSource"
                DefaultMode="Insert" AutoGenerateRows="False" 
                CellPadding="2" GridLines="None" CellSpacing="2" CssClass="AddActivitiesFormTable">
                <FieldHeaderStyle CssClass="AddActivitiesLabel" />
                <Fields>
                    <asp:BoundField DataField="CountryName" ShowHeader="true" HeaderText="Country Name"
                        ItemStyle-CssClass="aaLong"  />
                    <asp:BoundField DataField="CountryCode" ShowHeader="true" HeaderText="Country Code"
                        ItemStyle-CssClass="aaShort" />
                    <asp:CommandField ShowInsertButton="True" InsertText="Add" ButtonType="Button" ShowCancelButton="False"
                        ControlStyle-CssClass="ActivityGridButton"></asp:CommandField>
                </Fields>
                <InsertRowStyle Width="100%"></InsertRowStyle>
            </asp:DetailsView>
        </div>
     
    </div>
</asp:Content>
