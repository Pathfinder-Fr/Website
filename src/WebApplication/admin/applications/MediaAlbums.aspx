<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MediaAlbums.aspx.cs" Inherits="Sueetie.Web.AdminMediaAlbums" %>

<%@ Register Src="../controls/adminMediaNavLinks.ascx" TagName="adminMediaNavLinks" TagPrefix="uc1" %>
<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server" />

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="3" />
</asp:Content>

<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Manage Media Albums
</asp:Content>

<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminMediaNavLinks ID="adminMediaNavLinks1" runat="server" />
</asp:Content>

<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <div class="Activities">
            <div class="AdminTextTalk">
                <h2>
                    Manage Media Albums</h2>
                <div class="AdminFormDescription">
                    <p>
                        Update the Sueetie-specific Content Types of media albums which determines the event descriptions in the User Activity List and elsewhere.<br />
                    </p>
                </div>
            </div>
        </div>
        <asp:GridView ID="ActivitiesGridView" CssClass="fatgridviewMain" runat="server" AutoGenerateColumns="False"
            DataKeyNames="AlbumID" EmptyDataText="No records found." AllowSorting="True"
            DataSourceID="ActivitiesDataSource" OnRowUpdating="ActivitiesGridView_OnRowUpdating"
            OnRowDataBound="ActivitiesGridView_OnRowDataBound">
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
                        <%#Eval("AlbumID")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="GalleryKey">
                    <ItemTemplate>
                        <%#Eval("GalleryKey")%>
                    </ItemTemplate>
                </asp:TemplateField>
<asp:TemplateField HeaderText="Album Title">
                    <ItemTemplate>
                        <%#Eval("AlbumTitle")%>
                    </ItemTemplate>
                </asp:TemplateField>                
                <asp:TemplateField HeaderText="Content Type">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddContentTypes" runat="server" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <%#Eval("ContentTypeDescription")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowDeleteButton="false" ShowEditButton="true" UpdateText="Update"
                    ControlStyle-CssClass="ActivityGridButton" />
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="ActivitiesDataSource" runat="server" TypeName="Sueetie.Core.SueetieMedia"
            SelectMethod="GetSueetieMediaAlbumList" UpdateMethod="AdminUpdateSueetieMediaAlbum" >
            <UpdateParameters>
                <asp:Parameter Name="contentTypeId" Type="Int32" />
                 <asp:Parameter Name="contentId" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
            </InsertParameters>
        </asp:ObjectDataSource>

    </div>
</asp:Content>
