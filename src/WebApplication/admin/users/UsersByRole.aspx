<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UsersByRole.aspx.cs" Inherits="Sueetie.Web.UsersByRole" %>
<%@ Register src="../controls/adminUserNavLinks.ascx" tagname="adminUserNavLinks" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" Runat="Server" />

<asp:Content ID="Content5" runat="server" contentplaceholderid="cphBodyTitle">
    User By Role
</asp:Content>

<asp:Content ID="Content6" runat="server" contentplaceholderid="cphUserNavigation">
    <uc1:adminUserNavLinks ID="adminUserNavLinks1" runat="server" />
</asp:Content>

<asp:Content ID="Content7" runat="server" contentplaceholderid="cphContentBody">

    <div class="AdminFormArea">
        <h2>
            Users by Role</h2>
            
            <div class="RoleSelect">
        <asp:DropDownList ID="UserRoles" runat="server" AppendDataBoundItems="true" AutoPostBack="true" ToolTip="Select a role to display users in that role." OnSelectedIndexChanged="UserRoles_OnSelectedIndexChanged">
            <asp:ListItem>Select a Role</asp:ListItem>
        </asp:DropDownList>
</div>

        <asp:GridView ID="UsersGridView" CssClass="gridviewMain" runat="server" AutoGenerateColumns="False" DataKeyNames="userName" EmptyDataText="No records found." AllowSorting="True" >
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
                        <a href='UserEdit.aspx?username=<%# Eval("UserName") %>' title="click for details"><%# Eval("UserName") %></a>
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
        
        <div class="membersGridViewPager">
        
        <asp:LinkButton ID="lnkFirst" runat="server" onclick="lnkFirst_Click">&lt;&lt; First</asp:LinkButton>
        <asp:LinkButton ID="lnkPrev" runat="server" onclick="lnkPrev_Click">&lt; Prev</asp:LinkButton>
        <asp:LinkButton ID="lnkNext" runat="server" onclick="lnkNext_Click">Next &gt;</asp:LinkButton>
        <asp:LinkButton ID="lnkLast" runat="server" onclick="lnkLast_Click">Last &gt;&gt;</asp:LinkButton>
        
        </div>
</div>

</asp:Content>


