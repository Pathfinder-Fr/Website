<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" Inherits="YAF.ForumPageBase" %>

<%@ Register TagPrefix="YAF" Assembly="YAF" Namespace="YAF" %>
<%@ Register TagPrefix="uc" TagName="UserMenu" Src="~/themes/v2/menus/UserMenu.ascx" %>
<%@ Import Namespace="Sueetie.Core" %>
<%@ Import Namespace="Sueetie.Controls" %>
<%@ Import Namespace="System.IO" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="YafHead" runat="server">
    <meta id="YafMetaScriptingLanguage" http-equiv="Content-Script-Type" runat="server" name="scriptlanguage" content="text/javascript" />
    <meta id="YafMetaStyles" http-equiv="Content-Style-Type" runat="server" name="styles" content="text/css" />
    <meta id="YafMetaDescription" runat="server" name="description" content="Forum de la communauté francophone Pathfinder JdR" />
    <meta id="YafMetaKeywords" runat="server" name="keywords" content="Pathfinder, Golarion, Jeu de rôle, Open Gaming License, Donjons et Dragons, Communauté, JdR, Rpg, Paizo" />
    <meta name="HandheldFriendly" content="true" />
    <meta name="viewport" content="width=device-width,user-scalable=yes" />
    <link rel="shortcut icon" href="favicon.ico" />
    <title></title>
    <script type="text/javascript">
        var appInsights = window.appInsights || function (a) {
            function b(a) { c[a] = function () { var b = arguments; c.queue.push(function () { c[a].apply(c, b) }) } } var c = { config: a }, d = document, e = window; setTimeout(function () { var b = d.createElement("script"); b.src = a.url || "https://az416426.vo.msecnd.net/scripts/a/ai.0.js", d.getElementsByTagName("script")[0].parentNode.appendChild(b) }); try { c.cookie = d.cookie } catch (a) { } c.queue = []; for (var f = ["Event", "Exception", "Metric", "PageView", "Trace", "Dependency"]; f.length;)b("track" + f.pop()); if (b("setAuthenticatedUserContext"), b("clearAuthenticatedUserContext"), b("startTrackEvent"), b("stopTrackEvent"), b("startTrackPage"), b("stopTrackPage"), b("flush"), !a.disableExceptionTracking) { f = "onerror", b("_" + f); var g = e[f]; e[f] = function (a, b, d, e, h) { var i = g && g(a, b, d, e, h); return !0 !== i && c["_" + f](a, b, d, e, h), i } } return c
        }({
            instrumentationKey: "0220534a-383b-4901-ac9d-42db01e59904"
        });

        window.appInsights = appInsights, appInsights.queue && 0 === appInsights.queue.length && appInsights.trackPageView();
    </script>
    <script type="text/javascript" src="/Forum/JS_Code/choixparties.js"></script>
    <script type="text/javascript" async src="https://www.googletagmanager.com/gtag/js?id=UA-12647865-1"></script>
    <script type="text/javascript">
        window.dataLayer = window.dataLayer || [];
        function gtag() { dataLayer.push(arguments); }
        gtag('js', new Date());

        gtag('config', 'UA-12647865-1');
    </script>
</head>
<body id="YafBody" runat="server" style="margin: 0; padding: 5px">
    <form id="form1" runat="server" enctype="multipart/form-data">
        <table id="wrapper">
            <tr>
                <td id="margin-l" rowspan="2"></td>
                <td id="page">
                    <div id="header">
                        <div id="userpanel">
                            <uc:UserMenu runat="server" />
                        </div>
                        <div id="banner">
                            <span>Pathfinder-FR</span>
                        </div>
                        <SUEETIE:SiteMenu runat="server" />
                    </div>
                    <div id="container">
                        <div id="container-tl">
                            <div id="sidebar"></div>
                            <div id="container-tr">
                                <div id="content">
                                    <div id="PageLinksArea">
                                        <SUEETIE:SueetieLink SueetieUrlLinkTo="ForumsHome" TextKey="ForumTitle" runat="server" />
                                    </div>
                                    <YAF:Forum runat="server" ID="forum" />
                                </div>
                            </div>
                        </div>
                    </div>
                </td>
                <td id="margin-r" rowspan="2"></td>
            </tr>
            <tr>
                <td id="footer">
                    <SUEETIE:FooterMenu ID="FooterMenu1" runat="server" />
                    <p>La gamme Pathfinder est une cr&eacute;ation de <a href="http://www.paizo.com/" target="_blank">Paizo Publishing</a> traduite en fran&ccedil;ais par <a href="http://www.black-book-editions.fr/">Black Book Editions</a>.</p>
                    <p>Ce site se base sur les licences <a href="/Wiki/OGL.ashx">Open Game License</a>, <a href="/Wiki/PCUP.ashx">Pathfinder Community Use Policy et les conditions d'utilisation BBE</a>.</p>
                    <p>
                        <a href="http://yetanotherforum.net">
                            <img src="/images/yafsmall.jpg" border="0" /></a>
                    </p>
                    <p>
                        <SUEETIE:SueetieLogo ID="SueetieLogo1" runat="server" ShowText="True" LogoFont="White" />
                    </p>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
