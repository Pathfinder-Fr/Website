<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="sueetiealbumpaths.ascx.cs"
    Inherits="GalleryServerPro.Web.gs.pages.admin.sueetiealbumpaths" %>
<%@ Register Src="../../Controls/popupinfo.ascx" TagName="popup" TagPrefix="uc1" %>
<%@ Register Assembly="GalleryServerPro.WebControls" Namespace="GalleryServerPro.WebControls"
    TagPrefix="tis" %>
<div class="gsp_indentedContent">
    <asp:PlaceHolder ID="phAdminHeader" runat="server" />
    <div style="margin-left: 25px">
        <div class="gsp_addpadding1">
            <tis:wwErrorDisplay ID="wwMessage" runat="server" UserMessage="<%$ Resources:GalleryServerPro, Validation_Summary_Text %>"
                CellPadding="2" UseFixedHeightWhenHiding="False" Center="False" Width="453px">
            </tis:wwErrorDisplay>
            <div class="GalleryAdminText">
            This Sueetie Process adds/updates Gallery Server Pro album paths for use outside of GSP with jQuery plugins and other client-side functions.  Current Gallery albums are processed only.  Switch to other galleries if necessary.
            </div>
        </div>
        <asp:PlaceHolder ID="phAdminFooter" runat="server" />
    </div>
</div>
