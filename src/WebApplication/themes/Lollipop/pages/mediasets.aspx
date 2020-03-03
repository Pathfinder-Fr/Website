<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.AddonPack.Pages.AddonPackMediaSetPage"
    EnableViewState="false" MasterPageFile="/themes/lollipop/masters/alternate.master" %>

<%@ Register TagPrefix="uc" TagName="UserMenu" Src="~/themes/lollipop/menus/UserMenu.ascx" %>
<asp:content id="Content1" contentplaceholderid="cphHeader" runat="server">
    <link href="/themes/lollipop/style/ceebox.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        div.mediaSet
        {
            overflow: hidden;
            margin-top: 40px;
        }
        .mediaSet li
        {
            list-style: none;
            float: left;
            margin: 10px;
        }
        .mediaSet li img
        {
            border: 1px solid #ddd;
        }
        div#mediaSetArea
        {
            width: 500px;
            float: left;
            padding: 12px;
            overflow: hidden;
            min-height: 300px;
        }
        div.static
        {
            float: left;
            margin-left: 30px;
            width: 300px;
        }
        .mediaSet h2
        {
            font-size: 1.6em;
            margin: 8px auto 18px auto;
        }
    </style>
    <script type='text/javascript'>
        function pageLoad() {
            (function ($) {
                $.fn.emptyList = function () {
                    return this.empty();
                }
                var mediasetid = 1
                $(".ceeboxStatic").ceebox({ borderColor: '#dcdcdc', boxColor: "#fff" });
                $.fn.loadMediaSet = function (inputDataArray) {
                    this.emptyList();
                    var input = new Sys.StringBuilder("");
                    $.each(inputDataArray, function (index, inputData) {
                        input.append("<li><a href='" + inputData.OptimizedUrl + "'><img src='" + inputData.ThumbnailUrl + "' width='96' height='72' /></a></li>");
                    });
                    this.html(input.toString());
                    return this;
                }
                if ($("#mediaSetArea").not(':empty')) {
                    populateMediaSet(mediasetid, '<%= CurrentSueetieUserID %>');
                }
            })(jQuery);
        }

        function populateMediaSet(mediasetid, userid) {
            var divName = '#mediaSetImages' + mediasetid;
            var ws = new Sueetie.Web.SueetieService();
            ws.GetMediaSet(mediasetid, userid, getMediaSetComplete, null, divName);
        }

        function getMediaSetComplete(result, divName) {
            $('ul[id^="mediaSetImages"]').hide();
            if ($(divName).is(':empty')) {
                $(divName).loadMediaSet(result).show();
                $(divName).ceebox({ borderColor: '#dcdcdc', boxColor: "#fff" });
            }
            else {
                $(divName).show();
            }
        }

    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphBody" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Scripts>
            <asp:ScriptReference Path="~/scripts/jquery.js" />
            <asp:ScriptReference Path="~/scripts/jquery.alerts.js" />
            <asp:ScriptReference Path="~/scripts/jquery-utils.js" />
            <asp:ScriptReference Path="~/scripts/jquery.ceebox-min.js" />
        </Scripts>
        <Services>
            <asp:ServiceReference Path="~/util/services/SueetieService.svc" />
        </Services>
    </asp:ScriptManager>
    <div class="mediaSet">
        <div id="mediaSetArea">
            <h2>
                Dynamic Media Set</h2>
            <a href="javascript:void(0);" onclick="populateMediaSet(1,'<%= CurrentUserID.ToString() %>');return false;"
                class="mediaSetLink1">First Media Set</a> | <a href="javascript:void(0);" onclick="populateMediaSet(2,'<%= CurrentUserID.ToString() %>');return false;"
                    class="mediaSetLink2">Second Media Set</a> | <a href="javascript:void(0);" onclick="populateMediaSet(3,'<%= CurrentUserID.ToString() %>');return false;"
                        class="mediaSetLink3">Third Media Set</a>
            <ul id="mediaSetImages1"></ul>
            <ul id="mediaSetImages2"></ul>
            <ul id="mediaSetImages3"></ul>
        </div>
        <div class="static">
            <h2>
                Static Media Set</h2>
            <ul class="ceeboxStatic images">
                <li><a href='http://yoursite/media/gs/mediaobjects/1/some.jpeg'>
                    <img src='http://yoursite/media/gs/mediaobjects/1/some.jpeg'
                        width='96' height='72' id='img1' /></a></li>
                <li><a href='http://yoursite/media/gs/mediaobjects/1/some.jpeg'>
                    <img src='http://yoursite/media/gs/mediaobjects/1/some.jpeg'
                        width='96' height='72' id='img3485' /></a> </li>
                <li><a href='http://yoursite/media/gs/mediaobjects/1/some.jpeg'>
                    <img src='http://yoursite/media/gs/mediaobjects/1/some.jpeg'
                        width='96' height='72' id='img3298' /></a> </li>
                <li><a href='http://yoursite/media/gs/mediaobjects/1/some.jpeg'>
                    <img src='http://yoursite/media/gs/mediaobjects/1/some.jpeg'
                        width='96' height='72' id='img3290' /></a> </li>
            </ul>
        </div>
    </div>
</asp:Content>
