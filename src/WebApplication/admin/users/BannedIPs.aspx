<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BannedIPs.aspx.cs" Inherits="Sueetie.Web.BannedIPs" %>
<%@ Register Src="../controls/adminUserNavLinks.ascx" TagName="adminUserNavLinks" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">
</asp:Content>

<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Banned IPs
</asp:Content>
<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminUserNavLinks ID="adminUserNavLinks1" runat="server" />
</asp:Content>
<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <div class="Activities">
            <div class="AdminTextTalk">
                <h2>
                    Banned IPs</h2>
                <div class="AdminFormDescription">
                    <p>
                       Banned IPs are #.#.#.* masks of banned user IP remote host addresses.  They are displayed on the user's Administrative Profile page and updated on each user login.<br />
                    </p>
                </div>
            </div>
        </div>
        <asp:GridView ID="ActivitiesGridView" CssClass="fatgridviewMain" runat="server" AutoGenerateColumns="False"
            DataKeyNames="BannedID" EmptyDataText="No banned IPs found." AllowSorting="True"
            DataSourceID="ActivitiesDataSource" OnRowUpdating="ActivitiesGridView_OnRowUpdating">
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

                <asp:TemplateField HeaderText="IP Mask">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtMask" runat="server" Text='<%# Bind("Mask") %>'
                            CssClass="ApplicationMediumInput"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <%#Eval("Mask")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Banned Date">
                    <ItemTemplate>
                        <%#Eval("BannedDateTime")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" UpdateText="Update"
                    ControlStyle-CssClass="ActivityGridButton" />
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="ActivitiesDataSource" runat="server" TypeName="Sueetie.Core.SueetieUsers"
            SelectMethod="GetSueetieBannedIPList"  UpdateMethod="UpdateBannedIP"
            InsertMethod="BanIP" DeleteMethod="RemoveBannedIP" >
            <InsertParameters>
            <asp:Parameter Name="ip" Type="String" />
            </InsertParameters>
        </asp:ObjectDataSource>
        <div class="AddActivitiesForm">
            <div class="UploadTitle">
                <strong>Add IP Mask</strong></div>
            <asp:DetailsView ID="UpdateActivitiesDetailsView" runat="server" DataSourceID="ActivitiesDataSource"
                DefaultMode="Insert" AutoGenerateRows="False" 
                CellPadding="2" GridLines="None" CellSpacing="2" CssClass="AddActivitiesFormTable">
                <FieldHeaderStyle CssClass="AddActivitiesLabel" />
                <Fields>
                    <asp:BoundField DataField="ip" ShowHeader="true" HeaderText="IP Mask (Ex: 127.0.0.*)" 
                        ItemStyle-CssClass="aaMedium"  />
                    <asp:CommandField ShowInsertButton="True" InsertText="Add" ButtonType="Button" ShowCancelButton="False"
                        ControlStyle-CssClass="TextButtonBig" ></asp:CommandField>
                </Fields>
                <InsertRowStyle Width="100%"></InsertRowStyle>
            </asp:DetailsView>
        </div>
        
    </div>
</asp:Content>
