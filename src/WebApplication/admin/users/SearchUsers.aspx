<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchUsers.aspx.cs" Inherits="Sueetie.Web.SearchUsers" %>

<%@ Register Src="../controls/adminUserNavLinks.ascx" TagName="adminUserNavLinks"
    TagPrefix="uc1" %>
<asp:content id="Content1" contentplaceholderid="cphHeader" runat="Server" />
<asp:content id="Content5" runat="server" contentplaceholderid="cphBodyTitle">
    Member Search
</asp:content>
<asp:content id="Content6" runat="server" contentplaceholderid="cphUserNavigation">
    <uc1:adminUserNavLinks ID="adminUserNavLinks1" runat="server" />
</asp:content>
<asp:content id="Content7" runat="server" contentplaceholderid="cphContentBody">
    <div class="AdminFormArea">
        <h2>
            Search Users</h2>
            <div class="adminSearchFields">
    <asp:DropDownList ID="ddlUserSearchTypes" runat="server" ToolTip="Click to select which database column you want to search">
        <asp:ListItem Selected="true" Text="UserName" />
        <asp:ListItem Text="E-mail" />
    </asp:DropDownList>

    &nbsp;contains&nbsp;

    <asp:TextBox ID="txtSearchText" runat="server" MaxLength="100" ToolTip="Type your search term" />
    <div class="TextButtonBigArea">
       
    <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search"
        ToolTip="Click to start search." CssClass="TextButtonBig" />
        </div>
    </div>
    
    
    <asp:GridView ID="UsersGridView" CssClass="gridviewMain" runat="server" AutoGenerateColumns="False"
        DataKeyNames="userName" EmptyDataText="No records found.">
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
            <asp:BoundField DataField="creationdate" HeaderText="Creation Date" />
            <asp:BoundField DataField="lastlogindate" HeaderText="Last Login" />
            <asp:BoundField DataField="lastactivitydate" HeaderText="Last Activity" />
            <asp:CheckBoxField DataField="IsApproved" HeaderText="Approved">
                <ItemStyle HorizontalAlign="Center" />
            </asp:CheckBoxField>
            <asp:CheckBoxField DataField="IsOnline" HeaderText="Online">
                <ItemStyle HorizontalAlign="Center" />
            </asp:CheckBoxField>
            <asp:CheckBoxField DataField="IsLockedOut" HeaderText="Locked Out">
                <ItemStyle HorizontalAlign="Center" />
            </asp:CheckBoxField>
        </Columns>
    </asp:GridView>
    
   </div>
</asp:content>
