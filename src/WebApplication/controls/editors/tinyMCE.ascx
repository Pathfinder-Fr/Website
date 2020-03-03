<%@ Control Language="C#" AutoEventWireup="true" Inherits="Sueetie.Controls.tinyMCE" %>
<%@ Import Namespace="Sueetie.Core" %>

<script type="text/javascript" src="/util/editors/tiny_mce3/tiny_mce.js"></script>
<script type="text/javascript">
	tinyMCE.init({
		// General options
		mode: "exact",
		elements : "<%=txtContent.ClientID %>",
		theme: "advanced",
		plugins : "inlinepopups,fullscreen,contextmenu,emotions,table,iespell,advlink",
		convert_urls: false,
		
	  // Theme options
		theme_advanced_buttons1: "cut,copy,paste,|,undo,redo,|,bold,italic,underline,strikethrough,|,bullist,numlist,outdent,indent,|,iespell,link,unlink,removeformat,emotions,format,code",
		theme_advanced_buttons2: "",
		theme_advanced_toolbar_location: "top",
		theme_advanced_toolbar_align: "left",
		theme_advanced_statusbar_location: "bottom",
		theme_advanced_resizing: false,
		
		tab_focus : ":prev,:next"
	});
</script>

<asp:TextBox runat="Server" ID="txtContent" CssClass="EditorFont" TextMode="MultiLine"  Width="100%" />