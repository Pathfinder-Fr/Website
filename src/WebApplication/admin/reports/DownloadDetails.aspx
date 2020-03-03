<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DownloadDetails.aspx.cs" Inherits="Sueetie.Web.AdminDownloadDetails" %>
<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<%@ Register Src="../controls/adminReportsNavLinks.ascx" TagName="adminReportsNavLinks"
    TagPrefix="uc1" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server" />

<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    File Download Details
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
                    File Download Details</h2>
                <div class="AdminFormDescription">
                    <p>
                       Below are the download details for the selected file or user.<br />
                    </p>
                </div>

            </div>
          <div class="TopGridButtonRight">
            <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="TopGridButton" OnClick="btnRefresh_OnClick" />
            <asp:Button ID="btnAllDownloads" runat="server" Text="All Products" CssClass="TopGridButton" OnClick="btnAllDownloads_OnClick" />
            </div>
        </div>
    <asp:GridView ID="UsersGridView" CssClass="rptgridviewMain" runat="server" AutoGenerateColumns="False"
        DataKeyNames="MediaObjectID" EmptyDataText="No records found." AllowSorting="True">
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

            <asp:TemplateField HeaderText="Group">
                <ItemTemplate>
                        <asp:LinkButton ID="lbGroupID" CommandArgument='<%#Eval("GroupID")%>' runat="server" OnCommand="lbGroupID_OnCommand"> <%#Eval("GroupKey") == null ? "&nbsp;--&nbsp;" : Eval("GroupKey") %></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
               <asp:TemplateField HeaderText="AppKey">
                <ItemTemplate>
                        <asp:LinkButton ID="lbApplicationID" CommandArgument='<%#Eval("ApplicationID")%>' runat="server" OnCommand="lbApplicationID_OnCommand"><%#Eval("ApplicationKey")%></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
   <asp:TemplateField HeaderText="Album">
                <ItemTemplate>
                        <%# Eval("AlbumTitle")%>
                </ItemTemplate>
            </asp:TemplateField>            
              <asp:TemplateField HeaderText="File">
                <ItemTemplate>
              <asp:LinkButton ID="lbMediaObjectID" CommandArgument='<%#Eval("MediaObjectID")%>' runat="server" OnCommand="lbMediaObjectID_OnCommand"><%#Eval("MediaObjectTitle")%></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="User">
                <ItemTemplate>
                        <asp:LinkButton ID="lbUserID" CommandArgument='<%#Eval("DownloadUserID")%>' runat="server" OnCommand="lbUserID_OnCommand"><%#Eval("DownloadUserName")%></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField> 
              <asp:TemplateField HeaderText="Download DateTime" ItemStyle-CssClass="DateTimeColumn">
                <ItemTemplate>
                        <%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "DownloadDateTime")).ToString("MM/dd/yy HH:mm:ss") %>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <div class="membersGridViewPager">
        <asp:LinkButton ID="lnkFirst" runat="server" OnClick="lnkFirst_Click">&lt;&lt; First</asp:LinkButton>
        <asp:LinkButton ID="lnkPrev" runat="server" OnClick="lnkPrev_Click">&lt; Prev</asp:LinkButton>
        <asp:LinkButton ID="lnkNext" runat="server" OnClick="lnkNext_Click">Next &gt;</asp:LinkButton>
        <asp:LinkButton ID="lnkLast" runat="server" OnClick="lnkLast_Click">Last &gt;&gt;</asp:LinkButton>
    </div>

   </div>
</asp:Content>
