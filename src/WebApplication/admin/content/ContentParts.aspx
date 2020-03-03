<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContentParts.aspx.cs" Inherits="Sueetie.Web.AdminContentParts" ValidateRequest="false" %>
<%@ Register src="../controls/adminContentNavLinks.ascx" tagname="adminContentNavLinks" tagprefix="uc1" %>
<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation" TagPrefix="uc3" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" Runat="Server">
    <script type="text/javascript">
        function ToggleEditor(_this) {
            var id = $(_this.parentNode.parentNode).children()[0].id;
            if (_this.checked && _this.editor != null) {
                _this.editor.removeInstance(_this.origId);
                _this.editor = null;
            }
            else if (!_this.checked && _this.editor == null) {
                _this.origId = id;
                _this.editor = new nicEditor({ iconsPath: '/images/shared/nicedit/nicEditorIcons.gif' }).panelInstance(id);
            }
        }
    </script>
        <script type='text/javascript' src='/scripts/nicedit.js'></script>
    <script type='text/javascript' src='/scripts/jquery-impromptu.3.1.min.js'></script>
    <title>Content Parts</title>
</asp:Content>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="2" />
</asp:Content>


<asp:Content ID="Content5" runat="server" contentplaceholderid="cphBodyTitle">
    Content Parts
</asp:Content>



<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminContentNavLinks ID="adminContentNavLinks1" runat="server" />
</asp:Content>

<asp:Content ID="Content7" runat="server" contentplaceholderid="cphContentBody">
    <div style="height:25px;">
        <asp:UpdateProgress runat="server">
            <ProgressTemplate>Loading...</ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    
    <cc1:Accordion ID="ContentParts1" runat="server" SuppressHeaderPostbacks="false" CssClass="contentparts">
    </cc1:Accordion>
    
</asp:Content>