<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="thumbnailview.ascx.cs"
	Inherits="GalleryServerPro.Web.Controls.thumbnailview" EnableViewState="false" %>
<%@ Register Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<%@ Import Namespace="GalleryServerPro.Business.Interfaces" %>
<script type="text/javascript">
function cbThumbnailView_onCallBackError(sender, e)
{
	alert('An error occurred while communicating with the server. Try again. Error: ' + e.get_message());
}
</script>
<asp:PlaceHolder ID="phMsg" runat="server" />
<ComponentArt:CallBack ID="cbThumbnailView" runat="server" OnCallback="cbThumbnailView_Callback"
	CacheContent="true" EnableViewState="false" PostState="true"><ClientEvents><CallbackError EventHandler="cbThumbnailView_onCallBackError" /></ClientEvents>
	<Content>
		<asp:PlaceHolder ID="phPagerTop" runat="server" EnableViewState="false" />
		<asp:Repeater ID="rptr" runat="server" EnableViewState="false">
			<HeaderTemplate>
				<div id="thmbCtnr" class="<%# GetThumbnailContainerCssClass() %>">
			</HeaderTemplate>
			<ItemTemplate>
				<div class="<%# GetThumbnailCssClass(Container.DataItem.GetType()) %>">
					<%# GalleryPage.GetThumbnailHtml((IGalleryObject) Container.DataItem, GalleryPage.GalleryControl.AllowUrlOverride, true) %>
					<%# GetGalleryObjectText(Eval("Title").ToString(), Container.DataItem.GetType())%>
				</div>
			</ItemTemplate>
			<FooterTemplate>
				</div>
			</FooterTemplate>
		</asp:Repeater>
		<asp:PlaceHolder ID="phPagerBtm" runat="server" EnableViewState="false" />
		<asp:HiddenField ID="hfCallbackData" runat="server" />
	</Content>
</ComponentArt:CallBack>
