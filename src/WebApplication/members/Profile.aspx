<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="Sueetie.Web.UserProfile" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphHeader" runat="server">


    <script type="text/javascript">
        var noFollowMsg;
        var displayName;
        var noFavoriteMsg;
        var noActivityMsg;

        function pageLoad() {

            (function($) {
                $.fn.emptyList = function() {
                    return this.empty();
                }

                $.fn.loadFollowList = function(inputDataArray) {
                    this.emptyList();
                    var input = new Sys.StringBuilder("");
                    $.each(inputDataArray, function(index, inputData) {
                        input.append("<div class='followThumb'><a href='profile.aspx?u=" + inputData.First + "'><img src='/images/avatars/" +
                        inputData.Third + "'></a><br /><a href='profile.aspx?u=" + inputData.First + "'>" + inputData.Second + "</a></div>");
                    });
                    this.html(input.toString());
                    return this;
                }
                $.fn.loadFavoriteList = function(inputDataArray) {
                    this.emptyList();
                    var input = new Sys.StringBuilder("");
                    $.each(inputDataArray, function(index, inputData) {
                        input.append("<div class='favoriteTitle'><a href='" + inputData.Permalink + "'>" + inputData.Title + "</a></div>");
                        input.append("<div class='favoriteDescription'>" + inputData.Description + "</div>");
                        input.append("<div class='favoriteAuthoredBy'>" + inputData.ContentTypeAuthoredBy + "</div>");

                    });
                    this.html(input.toString());
                    return this;
                }
                $.fn.loadRecentCommentsList = function(inputDataArray) {
                    this.emptyList();
                    var input = new Sys.StringBuilder("");
                    $.each(inputDataArray, function(index, inputData) {
                        input.append("<div class='favoriteDescription'>" + inputData.Comment + "</div>");
                        input.append("<div class='favoriteAuthoredBy'>" + formatDate(inputData.CommentDate) + "  In post <a href='" + inputData.Permalink + "'>" + inputData.Title + "</a> by <a href='profile.aspx?u=" + inputData.UserID + "'>" + inputData.PostDisplayName + "</a></div>");

                    });
                    this.html(input.toString());
                    return this;
                }
                $.fn.loadRecentForumMessagesList = function(inputDataArray) {
                    this.emptyList();
                    var input = new Sys.StringBuilder("");
                    $.each(inputDataArray, function(index, inputData) {
                        input.append("<div class='favoriteDescription'>" + inputData.Message + "</div>");
                        input.append("<div class='favoriteAuthoredBy'>" + formatDate(inputData.DateTimeCreated) + "  In discussion <a href='" + inputData.Permalink + "'>" + inputData.Topic + "</a> started by <a href='profile.aspx?u=" + inputData.TopicSueetieUserID + "'>" + inputData.TopicDisplayName + "</a></div>");
                    });
                    this.html(input.toString());
                    return this;
                }
                populateFollows('-2', '0');
                populateFavorites('-2', '1', '0', '1');
                populateRecentForumMessages('6', '-2', '2', '1');
            })(jQuery);
        }

        function formatDate(_date) {
            return dateFormat(Date(_date), "mmm, dd, yyyy") + ".";
        }
        
        function refreshFavorites(result) {
            populateFavorites(result.UserID, result.ContentTypeID, result.GroupID, '1');
        }

        function populateFollows(userid, followType) {
            var followMsg;
            var qstring = $.parseQuery();

            if (qstring.u > 0)
                userid = qstring.u;

            $(".FollowFollowing, .FollowFollowers, .FollowFriends").removeClass("hilite");

            var ws = new Sueetie.Web.SueetieService();
            ws.GetDisplayName(userid, getDisplayNameResult);

            switch (followType) {
                case '1':
                    followMsg = 'Members following ' + displayName;
                    noFollowMsg = 'No one is following ' + displayName + ' at the moment.';
                    $(".FollowFollowers").addClass("hilite");
                    break;
                case '2':
                    followMsg = 'Friends of ' + displayName + '.';
                    noFollowMsg = displayName + ' has no friends at the moment.';
                    $(".FollowFriends").addClass("hilite");
                    break;
                default:
                    if (displayName == undefined) {
                        followMsg = 'Members this user is following';
                        noFollowMsg = 'This community member is not curently following anyone.';
                    }
                    else {
                        followMsg = 'Members ' + displayName + ' is following';
                        noFollowMsg = displayName + ' is not curently following anyone.';
                    }
                    $(".FollowFollowing").addClass("hilite");
                    break;
            }
            $("#FollowingNote").text(followMsg);

            var ws = new Sueetie.Web.SueetieService();
            ws.GetFollowList(userid, followType, getFollowsComplete);
        }

        function populateFavorites(userid, contenttypeid, groupid, isrestricted) {
            var favoriteMsg;
            var qstring = $.parseQuery();
            if (qstring.u > 0)
                userid = qstring.u;

            $(".FavoriteComments, .FavoritePosts, .FavoriteTopics, .FavoriteMessages").removeClass("hilite");

            switch (contenttypeid) {
                case '1':
                    favoriteMsg = 'This member\'s favorite Blog Posts';
                    if (displayName == undefined) {
                        noFavoriteMsg = 'This community member has not tagged any blog posts as favorites.';
                    }
                    else {
                        noFavoriteMsg = displayName + ' has not tagged any blog posts as favorites.';
                    }
                    $(".FavoritePosts").addClass("hilite");
                    break;
                case '2':
                    favoriteMsg = 'This member\'s favorite Blog Comments';
                    noFavoriteMsg = displayName + ' has not tagged any blog comments as favorites.';
                    $(".FavoriteComments").addClass("hilite");
                    break;
                case '3':
                    favoriteMsg = 'This member\'s favorite Forum Topics';
                    noFavoriteMsg = displayName + ' has not tagged any forum topics as favorites.';
                    $(".FavoriteTopics").addClass("hilite");
                    break;
                case '4':
                    favoriteMsg = 'This member\'s favorite Forum Messages';
                    noFavoriteMsg = displayName + ' has not tagged any forum messages as favorites.';
                    $(".FavoriteMessages").addClass("hilite");
                    break;
                default:
                    favoriteMsg = 'This members favorite Blog Posts';
                    noFavoriteMsg = displayName + 'has not tagged any blog posts as favorites.';
                    $(".FavoritePosts").addClass("hilite");
                    break;
            }
            $("#FavoriteNote").text(favoriteMsg);

            var ws = new Sueetie.Web.SueetieService();
            ws.GetFavoriteContent(userid, contenttypeid, groupid, isrestricted, getFavoritesComplete, null);
        }

        function populateRecentComments(numrecords, userid, applicationid, isrestricted) {
            var activityMsg;
            var qstring = $.parseQuery();
            if (qstring.u > 0)
                userid = qstring.u;

            activityMsg = 'This member\'s recent Blog Comments';
            if (displayName == undefined) {
                noActivityMsg = 'This community member has not yet entered any blog comments.';
            }
            else {
                noActivityMsg = displayName + ' has not yet entered any blog comments.';
            }
            $("#ActivityNote").text(activityMsg);
            $(".ActivityBlogCommentsLink").addClass("hilite");
            $(".ActivityForumMessagesLink").removeClass("hilite");

            var ws = new Sueetie.Web.SueetieService();
            ws.GetRecentComments(numrecords, userid, applicationid, isrestricted, getRecentCommentsComplete, null);
        }

        function populateRecentForumMessages(numrecords, userid, applicationid, isrestricted) {
            var activityMsg;
            var qstring = $.parseQuery();
            if (qstring.u > 0)
                userid = qstring.u;

            activityMsg = 'This member\'s recent Discussions';
            if (displayName == undefined) {
                noActivityMsg = 'This community member has not yet engaged in any discussions.';
            }
            else {
                noActivityMsg = displayName + ' has not yet engaged in any discussions.';
            }
            $("#ActivityNote").text(activityMsg);
            $(".ActivityBlogCommentsLink").removeClass("hilite");
            $(".ActivityForumMessagesLink").addClass("hilite");

            var ws = new Sueetie.Web.SueetieService();
            ws.GetRecentForumMessages(numrecords, userid, applicationid, isrestricted, getRecentForumMessagesComplete, null);
        }

        function CheckForFollowEmpty() {
            if ($("#FollowArea").is(':empty')) {
                $("#FollowArea").text(noFollowMsg).addClass('NoFollowMsg');
            }
            else {
                $("#FollowArea").removeClass('NoFollowMsg');
            }
        }

        function CheckForFavoriteEmpty() {
            if ($("#FavoriteArea").is(':empty')) {
                $("#FavoriteArea").text(noFavoriteMsg).addClass('NoFavoriteMsg');
            }
            else {
                $("#FavoriteArea").removeClass('NoFavoriteMsg');
            }
        }

        function CheckForActivityEmpty() {
            if ($("#ActivityArea").is(':empty')) {
                $("#ActivityArea").text(noActivityMsg).addClass('NoActivityMsg');
            }
            else {
                $("#ActivityArea").removeClass('NoActivityMsg');
            }
        }

        function HideMessage() {
            $("#ProfileUserFollowMsg").hide();
        }

        function getDisplayNameResult(result) {
            displayName = result;
        }

        function getFollowsComplete(result) {
            $("#FollowArea").loadFollowList(result);
            CheckForFollowEmpty();
        }

        function getFavoritesComplete(result) {
            $("#FavoriteArea").loadFavoriteList(result);
            CheckForFavoriteEmpty();
        }

        function getRecentCommentsComplete(result) {
            $("#ActivityArea").loadRecentCommentsList(result);
            CheckForActivityEmpty();
        }

        function getRecentForumMessagesComplete(result) {
            $("#ActivityArea").loadRecentForumMessagesList(result);
            CheckForActivityEmpty();
        }
    </script>

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" runat="server">
    <div align="center">
        <div class="CreateAccountArea">
            <h2 class="MessageTitle">
                <%= UserProfiled.DisplayName %>
            </h2>
            <div class="MessageContent">
                <div class="FormArea">
                    <div class="FormFieldDescription">
                        <asp:Label ID="lblMemberSince" runat="server" />
                    </div>
                    <div class="JoinArea">
                        <%-- ajax script manager --%>
                        <asp:ScriptManager ID="ScriptManager1" runat="server">
                            <Scripts>
                                <asp:ScriptReference Path="~/scripts/jquery.js" />
                                <asp:ScriptReference Path="~/scripts/jquery-utils.js" />
                                <asp:ScriptReference Path="~/scripts/date.format.js" />
                                <asp:ScriptReference Path="~/scripts/DateNet.js" />                                
                            </Scripts>
                            <Services>
                                <asp:ServiceReference Path="~/util/services/SueetieService.svc" />
                            </Services>
                        </asp:ScriptManager>
                        <%-- ajax update panel start --%>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <%-- ajax tab container start --%>
                                <AjaxControlToolkit:TabContainer ID="TabContainer1" runat="server" Width="100%">
                                    <AjaxControlToolkit:TabPanel ID="TabPanelAbout" runat="server" HeaderText="Profile">
                                        <ContentTemplate>
                                            <div class="ProfileArea">
                                                <table cellpadding="10" style="width: 100%">
                                                    <tr>
                                                        <td width="260" valign="top" class="ProfileLeftTD" text-align="right">
                                                            <div class="AvatarBig">
                                                                <asp:Image ID="AvatarImg" runat="server" Visible="true" />
                                                            </div>
                                                        </td>
                                                        <td valign="top" width="315">
                                                            <div class="bioSectionTitle">
                                                                Bio</div>
                                                            <div class="ProfileBio">
                                                                <asp:Label ID="lblBio" runat="server" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </ContentTemplate>
                                    </AjaxControlToolkit:TabPanel>
                                </AjaxControlToolkit:TabContainer>
                                <p>
                                </p>
                                <%-- ajax update panel end --%>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
