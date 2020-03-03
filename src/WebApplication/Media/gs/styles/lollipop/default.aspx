<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.Core.SueetieBasePage" %>
<%@ Import Namespace="Sueetie.Core" %>

<%@ Register TagPrefix="gsp" Namespace="GalleryServerPro.Web" Assembly="GalleryServerPro.Web" %>


<script runat="server" language="C#">

     public string SueetieMasterPage { get; set; }

        protected override void OnPreInit(EventArgs e)
        {
            string _sueetieMasterPage = SueetieMasterPage ?? "media.master";
            this.MasterPageFile = "~/masters/" + SueetieContext.Current.Theme + "/" + _sueetieMasterPage;
            base.OnPreInit(e);
        }

</script>

<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" runat="Server">

		<asp:ScriptManager ID="sm" runat="server" EnableScriptGlobalization="true" />
		<gsp:Gallery ID="g" runat="server"  />
</asp:Content>
