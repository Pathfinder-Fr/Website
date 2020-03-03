<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="sueetiecontent.ascx.cs"
    Inherits="GalleryServerPro.Web.gs.pages.admin.sueetiecontent" %>
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
            Media Gallery Objects are added to the Sueetie Core Content datastore automatically.  This function serves to inspect the media library for any content that for any reason may not already reside in the Sueetie Core Content Datastore. The Sueetie Datastore enables features like tagging, global search, and displaying media content outside of the Gallery Server Pro application.
            </div>
        </div>
        <asp:PlaceHolder ID="phAdminFooter" runat="server" />
    </div>
</div>
