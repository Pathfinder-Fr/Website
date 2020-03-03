<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="sueetielistview.ascx.cs"
    Inherits="GalleryServerPro.Web.Controls.sueetielistview" EnableViewState="false" %>
<%@ Register Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<%@ Import Namespace="GalleryServerPro.Business.Interfaces" %>
<%@ Import Namespace="Sueetie.Core" %>

<script type="text/javascript">
    function cbThumbnailView_onCallBackError(sender, e) {
        alert('An error occurred while communicating with the server. Try again. Error: ' + e.get_message());
    }
</script>

<asp:PlaceHolder ID="phMsg" runat="server" />
<ComponentArt:CallBack ID="cbThumbnailView" runat="server" OnCallback="cbThumbnailView_Callback"
    CacheContent="true" EnableViewState="false" PostState="true">
    <ClientEvents>
        <CallbackError EventHandler="cbThumbnailView_onCallBackError" />
    </ClientEvents>
    <Content>
        <asp:PlaceHolder ID="phPagerTop" runat="server" EnableViewState="false" />
        <asp:Repeater ID="rptr" runat="server" EnableViewState="false" OnItemDataBound="rptr_OnItemDataBound">
            <HeaderTemplate>
                <div id="thmbCtnr" class="gsp_listcontainer">
            </HeaderTemplate>
            <ItemTemplate>
                <div class="ListItem">
                    <div class="<%# GetThumbnailCssClass((SueetieMediaObject) Container.DataItem) %>">
                        <a href="<%# GenerateUrl((SueetieMediaObject) Container.DataItem) %>" title="<%# GetHovertip((SueetieMediaObject) Container.DataItem) %>">
                            <img src="<%# GetThumbnailUrl((SueetieMediaObject)Container.DataItem) %>" alt="<%# GetHovertip((SueetieMediaObject) Container.DataItem) %>"
                                style="width: <%# DataBinder.Eval(Container.DataItem, "ThumbnailWidth").ToString() %>px;
                                height: <%# DataBinder.Eval(Container.DataItem, "ThumbnailHeight").ToString() %>px;" /></a>
                    </div>
                    <div class="DocListDescription">
                        <div class="ListItemTitle">
                            <a href='<%# Eval("MediaObjectUrl") %>'>
                                <%# Eval("MediaObjectTitle") %></a>
                        </div>
                        <div class="ListItemDescription">
                            <%# Eval("MediaObjectDescription") %>
                        </div>
                        <div class="ListItemAuthorDate">
                            <SUEETIE:SueetieLocal runat="server" LanguageFile="MediaGallery.xml" Key="list_createdon" />
                            <%#  string.Format("{0:ddd MMM d, yyyy}", Eval("DateTimeCreated")) %>
                            <SUEETIE:SueetieLocal ID="SueetieLocal1" runat="server" LanguageFile="MediaGallery.xml"
                                Key="list_createdby" />
                            <a href="/members/profile.aspx?u=<%# Eval("SueetieUserID") %>">
                                <%# Eval("DisplayName") %></a>
                        </div>
                        <div class="mediaTagsArea">
                            <SUEETIE:SueetieLocal ID="SueetieLocal2" Key="tags_label" runat="server" CssClass="tagsLabel"
                                Anchor="span" />
                            <asp:PlaceHolder ID="phTagsControl" runat="server" />
                        </div>
                    </div>
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
