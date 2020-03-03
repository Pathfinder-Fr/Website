<%@ Page Language="C#" %>
<%@ Register TagPrefix="gsp" Namespace="GalleryServerPro.Web" Assembly="GalleryServerPro.Web" %>
<!DOCTYPE html>
<html>
<head runat="server">
	<style type="text/css">
		html { height: 100%; }
		body { margin: 0;height: 100%;  /* overflow: hidden; */ }
		.gsp_footer { display: none; }
	</style>
</head>
<body>
	<form id="form1" runat="server">
	<asp:ScriptManager ID="sm" runat="server" EnableScriptGlobalization="true" />
	<gsp:Gallery ID="g" runat="server" ShowHeader="false" ShowActionMenu="false" ShowAlbumBreadCrumb="false"
		ShowMediaObjectToolbar="false" ShowMediaObjectNavigation="false" ShowMediaObjectIndexPosition="false"
		ShowMediaObjectTitle="true" />
	</form>
</body>
</html>
