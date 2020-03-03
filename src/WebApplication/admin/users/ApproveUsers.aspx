<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApproveUsers.aspx.cs" Inherits="Sueetie.Web.ApproveUsers" %>

<%@ Register Src="../controls/adminUserNavLinks.ascx" TagName="adminUserNavLinks" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server" />

<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Users To Be Approved
</asp:Content>

<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminUserNavLinks ID="adminUserNavLinks1" runat="server" />
</asp:Content>

<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    
    <div class="AdminFormArea">
        <h2>
            Approve Users</h2>
            
    <asp:GridView ID="UsersGridView" CssClass="gridviewMain" runat="server" AutoGenerateColumns="False"
        DataKeyNames="userName" EmptyDataText="No records found." AllowSorting="True">
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
                    <a href='UserEdit.aspx?username=<%# Eval("UserName") %>' 
                        title="Edit User Details">
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
            <asp:BoundField DataField="createdate" HeaderText="Creation Date" />
            <asp:BoundField DataField="lastlogindate" HeaderText="Last Login" />
            <asp:BoundField DataField="lastactivitydate" HeaderText="Last Activity" />
        </Columns>
    </asp:GridView>

</div>
</asp:Content>
