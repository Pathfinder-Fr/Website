<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserActivities.aspx.cs" Inherits="Sueetie.Web.AdminUserActivities" %>

<%@ Register src="../controls/adminReportsNavLinks.ascx" tagname="adminReportsNavLinks" tagprefix="uc1" %>
<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation" TagPrefix="uc3" %>

<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    User Activities
</asp:Content>


<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="4" />
</asp:Content>

<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminReportsNavLinks ID="adminrReportsNavLinks1" runat="server" />
</asp:Content>

<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
    <div class="Activities"
        <div class="AdminTextTalk">
            <h2>
                User Activities Report</h2>
            <div class="AdminFormDescription">
                <p>
                    Recent User Activities.  Delete any activities you wish to remove from display and the Site Activity RSS Feed.<br />
                </p>
            </div>
        </div>
        </div>
        <asp:GridView ID="ActivitiesGridView" CssClass="fatgridviewMain" runat="server" AutoGenerateColumns="False"
            DataKeyNames="UserLogID" EmptyDataText="No records found." AllowSorting="True"
            DataSourceID="ActivitiesDataSource">
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
                        <%#Eval("UserLogID")%>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="DateTime" ItemStyle-CssClass="rptColSmallDate">
                    <ItemTemplate>
                        <%#Eval("DateTimeActivity")%>
                    </ItemTemplate>
                    <ItemStyle Width="115px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Activity Code">
                    <ItemTemplate>
                        <%#Eval("UserLogCategoryCode")%>
                    </ItemTemplate>
                    <ItemStyle Width="115px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Activity Description">
                    <ItemTemplate>
                        <%#Eval("Activity")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Display">
                    <EditItemTemplate>
                        <asp:CheckBox ID="chkRows" runat="server" ToolTip="Clear to not display" Checked='<%#Eval("IsDisplayed")%>' />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <img src='/images/shared/sueetie/<%#Eval("IsDisplayed")%>.png' alt='<%#Eval("IsDisplayed")%>' />
                    </ItemTemplate>
                    <ItemStyle Width="25px" HorizontalAlign="Center" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="RSS">
                    <EditItemTemplate>
                        <asp:CheckBox ID="chkRSS" runat="server" ToolTip="Clear to not include in RSS Feed" Checked='<%#Eval("IsSyndicated")%>' />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <img src='/images/shared/sueetie/<%#Eval("IsSyndicated")%>.png' alt='<%#Eval("IsSyndicated")%>' />
                    </ItemTemplate>
                    <ItemStyle Width="25px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:CommandField ShowDeleteButton="True" ShowEditButton="false" 
                    ControlStyle-CssClass="ActivityGridButton" />
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="ActivitiesDataSource" runat="server" TypeName="Sueetie.Core.SueetieLogs"
            SelectMethod="GetUserLogActivityList" DeleteMethod="DeleteUserLogActivity" OnSelecting="ActivityGrid_OnSelecting" >
            <SelectParameters>
            <asp:Parameter Name="ShowAll" DbType="Boolean" />
            </SelectParameters>
        </asp:ObjectDataSource>
        </div>
</asp:Content>
