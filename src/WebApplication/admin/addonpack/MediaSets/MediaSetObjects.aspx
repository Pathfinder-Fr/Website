<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.AddonPack.Pages.MediaSetObjectsPage"
    MasterPageFile="/themes/lollipop/masters/admin.master" %>

<%@ Register Src="/admin/controls/adminAddonPackNavLinks.ascx" TagName="adminAddonPackNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="/admin/controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">
    <script src="/scripts/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <script src="/scripts/jquery-utils.js" type="text/javascript"></script>
    <link href="/themes/Lollipop/style/jquery-ui-1.8.13.custom.css" rel="stylesheet"
        type="text/css" />
    <script type="text/javascript">

        $(function () {
            // there's the gallery and the trash
            var $gallery = $("#gallery"),
			$trash = $("#trash");

            var $mediaSetID = 1;
            var qstring = $.parseQuery();
            if (qstring.id > 0)
                $mediaSetID = qstring.id;

            // let the gallery items be draggable
            $("li", $gallery).draggable({
                cancel: "a.ui-icon", // clicking an icon won't initiate dragging
                revert: "invalid", // when not dropped, the item will revert back to its initial position
                containment: $("#demo-frame").length ? "#demo-frame" : "document", // stick to demo-frame if present
                helper: "clone",
                cursor: "move"
            });

            // let the gallery items be draggable
            $("li", $trash).draggable({
                cancel: "a.ui-icon", // clicking an icon won't initiate dragging
                revert: "invalid", // when not dropped, the item will revert back to its initial position
                containment: $("#demo-frame").length ? "#demo-frame" : "document", // stick to demo-frame if present
                helper: "clone",
                cursor: "move"
            });

            // let the trash be droppable, accepting the gallery items
            $trash.droppable({
                accept: "#gallery > li",
                activeClass: "ui-state-highlight",
                drop: function (event, ui) {
                    addImage(ui.draggable);
                }
            });

            // let the gallery be droppable as well, accepting items from the trash
            $gallery.droppable({
                accept: "#trash li",
                activeClass: "custom-state-active",
                drop: function (event, ui) {
                    recycleImage(ui.draggable);
                }
            });

            // image deletion function
            var recycle_icon = "<a href='javascript:recycleImage()' title='Remove this image' class='ui-icon ui-icon-refresh'>Remove image</a>";
            function addImage($item) {
                $item.fadeOut(function () {
                    var $list = $("ul", $trash).length ?
					$("ul", $trash) :
					$("<ul class='gallery ui-helper-reset'/>").appendTo($trash);

                    $item.find("a.ui-icon-plus").remove();
                    $item.append(recycle_icon).appendTo($list).fadeIn(function () {
                        $item
						.animate({ width: "48px" })
						.find("img")
							.animate({ height: "36px" });
                    });
                    var contentID = parseInt(this.firstElementChild.id.replace('img',''));
                    var mediasetID = $mediaSetID;
                    var ws = new Sueetie.Web.SueetieService();
                    ws.AddMediaSetItem(contentID, mediasetID, <%= CurrentSueetieUserID %>);

                });

            }

            // image recycle function
            var trash_icon = "<a href='javascript:addImage()' title='Add this image to Media Set' class='ui-icon ui-icon-plus'></a>";
            function recycleImage($item) {
                $item.fadeOut(function () {
                    $item
					.find("a.ui-icon-refresh")
						.remove()
					.end()
					.css("width", "96px")
					.append(trash_icon)
					.find("img")
						.css("height", "72px")
					.end()
					.appendTo($gallery)
					.fadeIn();
                });
                    var contentID = parseInt($item[0].firstElementChild.id.replace('img',''));
                    var mediasetID = $mediaSetID;
                    var ws = new Sueetie.Web.SueetieService();
                    ws.DeleteMediaSetItem(contentID, mediasetID, <%= CurrentSueetieUserID %>);
            }

            function viewLargerImage($link) {
                var src = $link.attr("href"),
				title = $link.siblings("img").attr("alt"),
				$modal = $("img[src$='" + src + "']");

                if ($modal.length) {
                    $modal.dialog("open");
                } else {
                    var img = $("<img alt='" + title + "' width='584' height='288' style='display: none; padding: 8px;' />")
					.attr("src", src).appendTo("body");
                    setTimeout(function () {
                        img.dialog({
                            title: title,
                            width: 680,
                            modal: true
                        });
                    }, 1);
                }
            }

            // resolve the icons behavior with event delegation
            $("ul.gallery > li").click(function (event) {
                var $item = $(this),
				$target = $(event.target);

                if ($target.is("a.ui-icon-plus")) {
                    addImage($item);
                } else if ($target.is("a.ui-icon-zoomin")) {
                    viewLargerImage($target);
                } else if ($target.is("a.ui-icon-refresh")) {
                    recycleImage($item);
                }

                return false;
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Manage Media Set Objects
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="5" />
</asp:Content>
<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminAddonPackNavLinks ID="adminAddonPackNavLinks1" runat="server" />
</asp:Content>
<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <div class="AdminTextTalk">
            <div class="ReportTitleSelectArea">
                <div class="Title">
                    Manage Media Set Objects</div>
                <div class="SelectArea">
                    Select Gallery:
                    <asp:DropDownList ID="ddMediaGalleries" runat="server" CssClass="BigDropDown" Width="300px"
                        AutoPostBack="true" OnSelectedIndexChanged="ddMediaGalleries_OnSelectedIndexChanged"
                        ClientIDMode="Static" />
                </div>
            </div>
            <div class="HeaderBox">
                Media objects for Media Set <strong>
                    <%= CurrentMediaSet.MediaSetTitle %></strong>
            </div>
            <div class="TableTop">
                <div class="TableTopLeft">
                    <asp:DropDownList ID="ddMediaAlbums" runat="server" CssClass="BigDropDown" Width="500px"
                        AutoPostBack="true" OnSelectedIndexChanged="ddMediaAlbums_OnSelectedIndexChanged" />
                </div>
                <div class="TableTopRight BigLink">
                </div>
            </div>
        </div>
        <div class="ui-widget ui-helper-clearfix mediasetarea">
            <ul id="gallery" class="gallery ui-helper-reset ui-helper-clearfix">
                <asp:Repeater ID="rptSelectMedia" runat="server" OnItemDataBound="rptSelectMedia_OnItemDataBound">
                    <ItemTemplate>
                        <li class='ui-widget-content ui-corner-tr'>
                            <asp:Literal runat="server" ID="ltMediaObject" />
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
            <div id="trash" class="ui-widget-content ui-state-default">
                <h4 class="ui-widget-header">
                    <span class="ui-icon ui-icon-plus">Trash</span>
                    <%= CurrentMediaSet.MediaSetTitle %></h4>
                <ul id="initTrash" class="gallery ui-helper-reset ui-helper-clearfix">
                    <asp:Repeater ID="rptInitMedia" runat="server" OnItemDataBound="rptInitMedia_OnItemDataBound">
                        <ItemTemplate>
                            <li class='ui-widget-content ui-corner-tr initMedia'>
                                <asp:Literal runat="server" ID="ltMediaObject" />
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
<%--                    <li class='ui-widget-content ui-corner-tr initMedia'>
                        <img src='http://sueetie/media/gs/mediaobjects/1/Sueetie Home Page History/zThumb_suehome0530b.jpeg'
                            id='img3288' class='initImg' /><a href='http://sueetie/media/gs/mediaobjects/1/Sueetie Home Page History/zOpt_suehome0530b.jpeg'
                                class='ui-icon ui-icon-zoomin'>View Larger</a><a href='javascript:recycleImage()'
                                    class='ui-icon ui-icon-refresh'>Delete Image</a> </li>--%>
                </ul>
            </div>
        </div>
</asp:Content>
