<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewUsers.aspx.cs" Inherits="Sueetie.Web.NewUsers" %>

<%@ Register Src="../controls/adminUserNavLinks.ascx" TagName="adminUserNavLinks" TagPrefix="uc1" %>
<asp:content id="Content1" contentplaceholderid="cphHeader" runat="Server">

    <script language="javascript" type="text/javascript">

        //  toggle checkboxes in gridview javascript function 
        function SelectAllCheckboxes(spanChk) {

            var oItem = spanChk.children;
            var theBox = (spanChk.type == "checkbox") ?
        spanChk : spanChk.children.item[0];
            xState = theBox.checked;
            elm = theBox.form.elements;

            for (i = 0; i < elm.length; i++)
                if (elm[i].type == "checkbox" &&
              elm[i].id != theBox.id) {
                //elm[i].click();

                if (elm[i].checked != xState)
                    elm[i].click();
                //elm[i].checked=xState;
            }
        }
 
    </script>

</asp:content>
<asp:content id="Content5" runat="server" contentplaceholderid="cphBodyTitle">
    New Users
</asp:content>
<asp:content id="Content6" runat="server" contentplaceholderid="cphUserNavigation">
    <uc1:adminUserNavLinks ID="adminUserNavLinks1" runat="server" />
</asp:content>
<asp:content id="Content7" runat="server" contentplaceholderid="cphContentBody">

    <div class="AdminFormArea">
        <h2>
            New Users</h2>
            
    <asp:Repeater ID="AtoZRepeater" runat="server" OnItemCommand="AtoZRepeater_ItemCommand">
        <ItemTemplate>
            <div class="aTozNavigation">
                <asp:LinkButton runat="server" ID="lnkFilter" Text='<%# Container.DataItem %>' CommandName='<%# Container.DataItem %>'></asp:LinkButton>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <br />
    <br />
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
            <asp:TemplateField HeaderText="Del">
                <HeaderTemplate>
                    <input id="chkAll" onclick="javascript:SelectAllCheckboxes(this);" runat="server"
                        type="checkbox" title="Check all checkboxes" />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="chkRows" runat="server" ToolTip="Select for deletion" />
                </ItemTemplate>
                <ItemStyle Width="25px" HorizontalAlign="Center" />
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
            <asp:CheckBoxField DataField="IsOnline" HeaderText="Online">
                <ItemStyle HorizontalAlign="Center" />
            </asp:CheckBoxField>
        </Columns>
    </asp:GridView>
    <%-- gridview navigation links --%>
    <div class="membersGridViewPager">
        <asp:LinkButton ID="lnkFirst" runat="server" OnClick="lnkFirst_Click">&lt;&lt; First</asp:LinkButton>
        <asp:LinkButton ID="lnkPrev" runat="server" OnClick="lnkPrev_Click">&lt; Prev</asp:LinkButton>
        <asp:LinkButton ID="lnkNext" runat="server" OnClick="lnkNext_Click">Next &gt;</asp:LinkButton>
        <asp:LinkButton ID="lnkLast" runat="server" OnClick="lnkLast_Click">Last &gt;&gt;</asp:LinkButton>
    </div>
    <%-- delete checked users --%>
    <div class="membersToggle">
        <asp:LinkButton ID="btnDeleteSelected" runat="server" ToolTip="Click to delete the selected users."
            OnClientClick="return confirm('Are you sure you want to deactivate the selected users?');"
            OnClick="btnDeleteSelected_Click">Deactivate Selected</asp:LinkButton>
    </div>
   
    <br />
    <br />
    <%-- delete success label  --%>
    <asp:Label ID="lblDeleteSuccess" runat="server" Text="User(s) were sucessfully deleted! - and will be removed from grid on page refresh."
        Visible="false"></asp:Label>
</div>

</asp:content>
