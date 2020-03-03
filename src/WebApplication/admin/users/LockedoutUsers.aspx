<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LockedoutUsers.aspx.cs" Inherits="Sueetie.Web.LockedoutUsers" %>
<%@ Register src="../controls/adminUserNavLinks.ascx" tagname="adminUserNavLinks" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" Runat="Server" />

<asp:Content ID="Content5" runat="server" contentplaceholderid="cphBodyTitle">
    Locked Out Users
</asp:Content>


<asp:Content ID="Content6" runat="server" contentplaceholderid="cphUserNavigation">
    <uc1:adminUserNavLinks ID="adminUserNavLinks1" runat="server" />
</asp:Content>

<asp:Content ID="Content7" runat="server" contentplaceholderid="cphContentBody">

    <div class="AdminFormArea">
        <h2>
            Locked Out Users</h2>
            
    <asp:GridView ID="UsersGridView" runat="server" CssClass="gridviewMain"  AutoGenerateColumns="False"  DataKeyNames="userName" EmptyDataText="No records found.">
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
                    <div class="usernameGridRoll">
                    <a href='UserEdit.aspx?username=<%# Eval("UserName") %>' title="click for details"><%# Eval("UserName") %></a>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>          
            <asp:TemplateField HeaderText="Email">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Email") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <a href='Mailto:<%# Eval("Email") %>' title="click to email from your computer"><%#Eval("Email")%></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="creationdate" HeaderText="Creation Date" />
            <asp:BoundField DataField="lastlogindate" HeaderText="Last Login" />
            <asp:BoundField DataField="lastactivitydate" HeaderText="Last Activity" />
        </Columns>
    </asp:GridView>
            
</div>
</asp:Content>
