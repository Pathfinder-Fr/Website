<%@ Page Language="C#" AutoEventWireup="true" %>
<%@ Import Namespace="Sueetie.Core" %>

<%@ Register TagPrefix="gsp" Namespace="GalleryServerPro.Web" Assembly="GalleryServerPro.Web" %>
<%@ Register TagPrefix="uc" TagName="UserMenu" Src="~/gs/styles/parfait/menus/UserMenu.ascx" %>

<script runat="server" language="C#">

     public string SueetieMasterPage { get; set; }

        protected override void OnPreInit(EventArgs e)
        {
            string _sueetieMasterPage = SueetieMasterPage ?? "media.master";
            this.MasterPageFile = "~/masters/" + SueetieContext.Current.Theme + "/" + _sueetieMasterPage;
            base.OnPreInit(e);
        }

</script>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTopBanner" runat="Server">
    <div id="PageBanner">
        <div id="PageBannerInner">
            <div id="PageTitle" style="margin-left: 180px;">
                Screenshots
            </div>
            <div id="userMenu">
                <uc:UserMenu ID="UserMenu1" runat="server" />
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">

		<asp:ScriptManager ID="sm" runat="server" EnableScriptGlobalization="true" />
		<gsp:Gallery ID="g" runat="server" GalleryId="1"  />
</asp:Content>
