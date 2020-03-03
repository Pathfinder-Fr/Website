<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MediaGalleries.aspx.cs" Inherits="Sueetie.Web.AdminMediaGalleries" %>

<%@ Register Src="../controls/adminMediaNavLinks.ascx" TagName="adminMediaNavLinks" TagPrefix="uc1" %>
<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server" />

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="3" />
</asp:Content>

<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Manage Media Galleries
</asp:Content>

<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminMediaNavLinks ID="adminMediaNavLinks1" runat="server" />
</asp:Content>

<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <div class="Activities">
            <div class="AdminTextTalk">
                <h2>
                    Manage Media Galleries</h2>
                <div class="AdminFormDescription">
                    <p>
                        Update Sueetie-specific properties of media galleries.  Note: The GalleryKey field must match the media gallery's base page. For example, the GalleryKey of a "Document Library" media gallery with base page of "/media/library.aspx" is "library."<br />
                    </p>
                </div>
            </div>
        </div>
        <asp:GridView ID="ActivitiesGridView" CssClass="fatgridviewMain" runat="server" AutoGenerateColumns="False"
            DataKeyNames="GalleryID" EmptyDataText="No records found." AllowSorting="True"
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
                        <%#Eval("GalleryID")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="GalleryKey">
                    <ItemTemplate>
                        <%#Eval("GalleryKey") == null ? "&nbsp;--&nbsp;" : Eval("GalleryKey")%>
                    </ItemTemplate>
                    <EditItemTemplate>
                   <asp:TextBox id="txtGalleryKey" runat="server" Text='<%# Bind("GalleryKey") %>'
                            CssClass="ApplicationMediumInput" />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Gallery Title">
                    <ItemTemplate>
                        <%#Eval("GalleryTitle")%>
                    </ItemTemplate>
                </asp:TemplateField>                
                <asp:TemplateField HeaderText="Display Type">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddDisplayTypes" runat="server" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <%#Eval("DisplayTypeDescription")%>
                    </ItemTemplate>
                </asp:TemplateField>
                    <asp:TemplateField HeaderText="IsPublic">
                    <EditItemTemplate>
                        <asp:CheckBox ID="chkPublic" runat="server" ToolTip="Clear if a private or utility gallery" Checked='<%#Eval("IsPublic")%>' />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <img src='/images/shared/sueetie/<%#Eval("IsPublic")%>.png' alt='<%#Eval("IsPublic")%>' />
                    </ItemTemplate>
                    <ItemStyle Width="25px" HorizontalAlign="Center" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="IsLogged">
                    <EditItemTemplate>
                        <asp:CheckBox ID="chkLogged" runat="server" ToolTip="Clear if you do not want to log activity in this gallery" Checked='<%#Eval("IsLogged")%>' />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <img src='/images/shared/sueetie/<%#Eval("IsLogged")%>.png' alt='<%#Eval("IsLogged")%>' />
                    </ItemTemplate>
                    <ItemStyle Width="25px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:CommandField ShowDeleteButton="false" ShowEditButton="true" UpdateText="Update"
                    ControlStyle-CssClass="ActivityGridButton" />
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="ActivitiesDataSource" runat="server" TypeName="Sueetie.Core.SueetieMedia"
            SelectMethod="GetSueetieMediaGalleryList" UpdateMethod="AdminUpdateSueetieMediaGallery" >
            <UpdateParameters />
            <InsertParameters>
            </InsertParameters>
        </asp:ObjectDataSource>

    </div>
</asp:Content>
