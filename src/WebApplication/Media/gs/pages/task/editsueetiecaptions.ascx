<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="editsueetiecaptions.ascx.cs"
    Inherits="GalleryServerPro.Web.gs.pages.task.editsueetiecaptions" %>
<%@ Import Namespace="GalleryServerPro.Business.Interfaces" %>
<%@ Import Namespace="Sueetie.Core" %>
<div class="gsp_content">
    <asp:PlaceHolder ID="phTaskHeader" runat="server" />
    <asp:Repeater ID="rptr" runat="server" OnItemDataBound="rptr_OnItemDataBound">
        <HeaderTemplate>
            <div class="gsp_floatcontainer">
        </HeaderTemplate>
        <ItemTemplate>
            <div class="ListItem">
                <div class="ListThumbnail">
                    <a href='<%# Eval("MediaObjectUrl") %>'>
                        <img src="<%# GetThumbnailUrl((SueetieMediaObject) Container.DataItem) %>" alt="<%# RemoveHtmlTags(Eval("MediaObjectTitle").ToString()) %>"
                            title="<%# RemoveHtmlTags(Eval("MediaObjectTitle").ToString()) %>" style="width: <%# DataBinder.Eval(Container.DataItem, "ThumbnailWidth").ToString() %>px;
                            height: <%# DataBinder.Eval(Container.DataItem, "ThumbnailHeight").ToString() %>px;" /></a>
                </div>
                <div class="ListDescription">
                    <div class="ListItemTitle">
                        <input id="ta" runat="server" onfocus="javascript:this.select();" name="ta" value='<%# Eval("MediaObjectTitle") %>' />
                    </div>
                    <div class="ListItemDescription">
                        <textarea id="tdesc" runat="server" onfocus="javascript:this.select();" name="tdesc"><%# Eval("MediaObjectDescription") %></textarea>
                        <input id="Hidden1" runat="server" type="hidden" value='<%# Eval("MediaObjectID") %>' />
                        <div class="tagsEditArea">
                            <asp:PlaceHolder ID="phTagsControl" runat="server" />
                        </div>
                        <div class="calendarArea">
                            <asp:PlaceHolder ID="phCalendarControl" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </ItemTemplate>
        <FooterTemplate>
            </div>
        </FooterTemplate>
    </asp:Repeater>
    <asp:PlaceHolder ID="phTaskFooter" runat="server" />
</div>
