<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BackgroundTasks.aspx.cs" Inherits="Sueetie.Web.AdminBackgroundTasks" %>
<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation" TagPrefix="uc3" %>
<%@ Register Src="../controls/adminReportsNavLinks.ascx" TagName="adminReportsNavLinks" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server" />

<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Background Tasks
</asp:Content>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="4" />
</asp:Content>

<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminReportsNavLinks ID="adminReportsNavLinks1" runat="server" />
</asp:Content>

<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <div class="Activities">
            <div class="AdminTextTalk">
                <h2>
                    Background Tasks</h2>
                <div class="AdminFormDescription">
                    <p>
                       Below are the background tasks currently running on <%= Sueetie.Core.SiteSettings.Instance.SiteName %><br />
                    </p>
                </div>
            </div>
        </div>
        
    <asp:GridView ID="UsersGridView" CssClass="gridviewMain" runat="server" AutoGenerateColumns="False"
        DataKeyNames="Name" EmptyDataText="No records found." AllowSorting="True">
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
            <asp:TemplateField HeaderText="Task">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "TaskType")%>
                </ItemTemplate>
            </asp:TemplateField>
                <asp:TemplateField HeaderText="Mins">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "Minutes")%>
                </ItemTemplate>
            </asp:TemplateField>
         
            <asp:TemplateField HeaderText="Enabled">
                    <ItemTemplate>
                        <img src='/images/shared/sueetie/<%#Eval("Enabled")%>.png' alt='<%#Eval("Enabled")%>' />
                    </ItemTemplate>
                    </asp:TemplateField>
       <asp:TemplateField HeaderText="Running">
                    <ItemTemplate>
                        <img src='/images/shared/sueetie/<%#Eval("IsRunning")%>.png' alt='<%#Eval("IsRunning")%>' />
                    </ItemTemplate>
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="Last Started">
                    <ItemTemplate>
                        <%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "LastRunStart")).Year < 1969 ? "&nbsp;--&nbsp;" : Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "LastRunStart")).ToString("ddd, dd MMM yyyy HH:mm:ss")%>
                    </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Last Ended">
                    <ItemTemplate>
                        <%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "LastRunStart")).Year < 1969 ? "&nbsp;--&nbsp;" : Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "LastRunEnd")).ToString("ddd, dd MMM yyyy HH:mm:ss")%>
                    </ItemTemplate>
                    </asp:TemplateField>
                            <asp:TemplateField HeaderText="Successful">
                    <ItemTemplate>
                      <img src='/images/shared/sueetie/<%#Eval("IsLastRunSuccessful")%>.png' alt='<%#Eval("IsLastRunSuccessful")%>' />  
                    </ItemTemplate>
                    </asp:TemplateField>
        </Columns>
    </asp:GridView>
   
</div>
</asp:Content>
