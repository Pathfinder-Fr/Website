using System;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.Web.Controller;

namespace GalleryServerPro.Web.Controls
{
    public partial class galleryheader : GalleryUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ConfigureControls();
        }

        private void ConfigureControls()
        {
            AddUserNavigationControls();

            AddGalleryTitle();
        }

        private void AddGalleryTitle()
        {
            string galleryTitle = this.GalleryPage.GalleryTitle;
            string galleryTitleUrl = this.GalleryPage.GalleryTitleUrl;

            if (String.IsNullOrEmpty(galleryTitle))
                return;

            HtmlGenericControl pTag = new HtmlGenericControl("p");
            pTag.Attributes["class"] = "gsp_bannertext";

            if (!String.IsNullOrEmpty(galleryTitle) && (galleryTitleUrl.Trim().Length > 0))
            {
                HyperLink hlHeader = new HyperLink();
                hlHeader.Text = galleryTitle;

                string headerTextUrl = galleryTitleUrl.Trim();

                switch (headerTextUrl)
                {
                    case "/":
                        {
                            // Create a link to the root of the web site.
                            hlHeader.NavigateUrl = headerTextUrl;
                            hlHeader.ToolTip = Resources.GalleryServerPro.Header_PageHeaderTextUrlToolTipWebRoot;
                            break;
                        }
                    case "~/":
                        {
                            // Create a link to the top level album.
                            hlHeader.NavigateUrl = Util.GetCurrentPageUrl();
                            hlHeader.ToolTip = Resources.GalleryServerPro.Header_PageHeaderTextUrlToolTipAppRoot;
                            break;
                        }
                    default:
                        {
                            // Create a link to the specified URL.
                            hlHeader.NavigateUrl = headerTextUrl;
                            hlHeader.ToolTip = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Header_PageHeaderTextUrlToolTip, headerTextUrl);
                            break;
                        }
                }

                pTag.Controls.Add(hlHeader);
            }
            else
            {
                pTag.InnerText = galleryTitle;
            }

            pnlHeader.Controls.Add(pTag);
        }

        private void AddUserNavigationControls()
        {
            if (this.GalleryPage.ShowSearch)
                pnlUserNav.Controls.Add(Page.LoadControl(Util.GetUrl("/controls/search.ascx")));

            if (!this.GalleryPage.IsAnonymousUser && this.GalleryPage.GallerySettings.EnableUserAlbum)
                this.AddHomePageLinkControl();

            if (this.GalleryPage.IsAnonymousUser && this.GalleryPage.GallerySettings.EnableSelfRegistration)
                this.AddCreateUserControl();

            //if (!this.GalleryPage.IsAnonymousUser && GalleryPage.GallerySettings.AllowManageOwnAccount)
            //    pnlUserNav.Controls.Add(Page.LoadControl(Util.GetUrl("/controls/myaccount.ascx")));

            //if (this.GalleryPage.ShowLogin)
            //    pnlUserNav.Controls.Add(Page.LoadControl(Util.GetUrl("/controls/login.ascx")));
        }

        private void AddHomePageLinkControl()
        {
            IAlbum userAlbum = UserController.GetUserAlbum(this.GalleryPage.GalleryId);

            if (userAlbum != null)
            {
                HyperLink hlGoToHomeAlbum = new HyperLink();
                hlGoToHomeAlbum.NavigateUrl = Util.GetUrl(PageId.album, "aid={0}", userAlbum.Id);
                hlGoToHomeAlbum.ImageUrl = Util.GetUrl("/images/home_32x32.png");
                hlGoToHomeAlbum.ToolTip = String.Format(CultureInfo.CurrentCulture, Util.HtmlEncode(Util.RemoveHtmlTags(Resources.GalleryServerPro.Header_Go_To_Home_Album_Link_Tooltip)), userAlbum.Title);

                HtmlGenericControl pTag = new HtmlGenericControl("p");
                pTag.Attributes["class"] = "gsp_homealbumlink gsp_useroption";
                pTag.Controls.Add(hlGoToHomeAlbum);

                this.pnlUserNav.Controls.Add(pTag);
            }
        }

        private void AddCreateUserControl()
        {
            HyperLink hlCreateAccount = new HyperLink();
            hlCreateAccount.NavigateUrl = Util.GetUrl(PageId.createaccount);
            hlCreateAccount.Text = Resources.GalleryServerPro.Header_Create_Account_Link_Text;
            hlCreateAccount.ToolTip = Resources.GalleryServerPro.Header_Create_Account_Link_Tooltip;

            HtmlGenericControl pTag = new HtmlGenericControl("p");
            pTag.Attributes["class"] = "gsp_createaccount";
            pTag.Controls.Add(hlCreateAccount);

            this.pnlUserNav.Controls.Add(pTag);
        }
    }
}