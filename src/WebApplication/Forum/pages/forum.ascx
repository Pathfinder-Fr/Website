<%@ Control Language="c#" AutoEventWireup="True" Inherits="YAF.Pages.forum" Codebehind="forum.ascx.cs" %>
<%@ Register TagPrefix="YAF" TagName="ForumWelcome" Src="../controls/ForumWelcome.ascx" %>
<%@ Register TagPrefix="YAF" TagName="ForumIconLegend" Src="../controls/ForumIconLegend.ascx" %>
<%@ Register TagPrefix="YAF" TagName="ForumStatistics" Src="../controls/ForumStatistics.ascx" %>
<%@ Register TagPrefix="YAF" TagName="ForumActiveDiscussion" Src="../controls/ForumActiveDiscussion.ascx" %>
<%@ Register TagPrefix="YAF" TagName="ForumCategoryList" Src="../controls/ForumCategoryList.ascx" %>
<%@ Register TagPrefix="YAF" TagName="ShoutBox" Src="../controls/ShoutBox.ascx" %>
<%@ Register TagPrefix="YAF" TagName="PollList" Src="../controls/PollList.ascx" %>
<YAF:PageLinks runat="server" ID="PageLinks" />
<YAF:ForumWelcome runat="server" ID="Welcome" />
<div class="DivTopSeparator">
</div>
<YAF:ShoutBox ID="ShoutBox1" runat="server" />
<YAF:PollList ID="PollList" runat="server"/>
<YAF:ForumCategoryList ID="ForumCategoryList" runat="server"></YAF:ForumCategoryList>
<div id="choixParties">
  <p><a href="javascript:$('#cpdiv').toggle('fast');void(0);" title="Cliquez ici pour afficher/masquer l'interface permettant de choisir les parties affichées">Choisir les parties en ligne à afficher</a></p>
  <div id="cpdiv">
    <h3>Parties actuellement affichées (cliquez pour supprimer)</h3>
    <div id="cpVisibles"></div>
    <button type="button" id="cpToutvoir" onclick="cp_toutvoir();" title="Rendre toutes les parties visibles">Tout voir</button>
    <button type="button" id="cpToutcacher" onclick="cp_toutcacher();" title="Masquer toutes les parties">Tout cacher</button>
    —
    <button type="button" id="cpAjouter" onclick="cp_ajouter();" title="Rendre visible la partie sélectionnée dans le menu déroulant">Ajouter la partie</button>
    <select id="cpChoix"></select>
  </div>
</div>
<br />
<YAF:ForumActiveDiscussion ID="ActiveDiscussions" runat="server" />
<br />
<YAF:ForumStatistics ID="ForumStats" runat="Server" />
<YAF:ForumIconLegend ID="IconLegend" runat="server" />
<div id="DivSmartScroller">
	<YAF:SmartScroller ID="SmartScroller1" runat="server" />
</div>

