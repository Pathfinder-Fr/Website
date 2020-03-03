<%@ Page Language="C#" AutoEventWireup="true" 
    Inherits="Sueetie.Commerce.Pages.SueetieProductKeyPage" MasterPageFile="/themes/lollipop/masters/admin.master" %>

<%@ Register Src="../controls/adminMarketplaceNavLinks.ascx" TagName="adminMarketplaceNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">
    <style type="text/css"> 
        .RadioButtonList
        {
            margin-bottom: 20px;
            line-height: 1.2;
            font-size: 1em;
        }
        .LicenseButton
        {
            background-color: #eee;
            border: 1px solid #ccc;
            padding: 6px 10px;
            font-weight: bold;
        }
        .LicenseButton:hover
        {
            background-color: #ddd;
        }
        .Results
        {
            font-weight: bold;
            font-size: .9em;
        }
        .KeyTextBoxArea
        {
            overflow: hidden;
            margin: 20px auto;
        }
        input.KeyTextBox
        {
            padding: 4px;
            border: 1px solid #ccc;
            width: 400px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Sueetie Marketplace - Create Member Product Key
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
            <h2>
                Create Member Product Key</h2>
            <div class="AdminTextTalk">
                <div class="AdminFormDescription">
                    <p>
                        Sueetie Package Distribution Feature. Use to create member product keys for Triple
                        Scoop Applications.</p>
                </div>
            </div>
        </div>
        <div class="MarketPlaceAdministration">
            <asp:Panel ID="pnlSearch" runat="server">
                <div class="adminSearchFields">
                    Sueetie Member:
                    <asp:TextBox ID="txtSearchText" runat="server" MaxLength="100" ToolTip="Enter Username" />
                    <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search"
                        ToolTip="Click to start search." CssClass="TextButtonBig" />
                </div>
                <asp:GridView ID="UsersGridView" CssClass="gridviewMain" runat="server" AutoGenerateColumns="False"
                    DataKeyNames="userName" EmptyDataText="No records found." Width="500">
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
                                User Name
                            </HeaderTemplate>
                            <ItemTemplate>
                                <a href='SueetieProductKey.aspx?username=<%# Eval("UserName") %>' title="Edit User Details">
                                    <%# Eval("UserName") %></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Email">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Email") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <a href='Mailto:<%# Eval("Email") %>' title="click to email from your computer">
                                    <%#Eval("Email")%></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
            <asp:Panel ID="pnlProductKey" runat="server">
                <div class="ProductKeyForm">
                    <div class="ProductKeyMember">
                        <asp:Label ID="lblProductKeyMember" runat="server" />
                    </div>
                    <div class="ProductKeyButtons">
                        <div class="RadioButtonList">
                            <asp:RadioButtonList ID="rblPackageTypes" runat="server" Height="63px" Width="142px">
                                <asp:ListItem Value="1" Text="Addon Pack" Selected="True" />
                                <asp:ListItem Value="2" Text="Analytics" />
                                <asp:ListItem Value="3" Text="Marketplace" />
                            </asp:RadioButtonList>
                        </div>
                        <div class="RadioButtonList">
                            <asp:RadioButtonList ID="rblLicenseTypes" runat="server">
                                <asp:ListItem Value="1" Text="Free" Selected="True" />
                                <asp:ListItem Value="11" Text="Sueetie Insider" />
                                <asp:ListItem Value="12" Text="Entrepreneur" />
                                <asp:ListItem Value="13" Text="Small Business" />
                                <asp:ListItem Value="14" Text="Corporate" />
                                <asp:ListItem Value="15" Text="Enterprise" />
                                <asp:ListItem Value="16" Text="Evaluation" />
                            </asp:RadioButtonList>
                        </div>
                        <asp:Button OnClick="btnProductKey_OnClick" runat="server" ID="btnCreate" Text="Create License"
                            CssClass="TextButtonBig" />
                        <div class="KeyTextBoxArea">
                            <asp:TextBox ID="txtKey" runat="server" CssClass="KeyTextBox" />
                        </div>

                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
