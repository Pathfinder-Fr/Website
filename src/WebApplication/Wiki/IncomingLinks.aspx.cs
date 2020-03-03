using System;
using ScrewTurn.Wiki.PluginFramework;

namespace ScrewTurn.Wiki
{
    public partial class IncomingLinks : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = Properties.Messages.PageIncomingLinks + " - " + Settings.WikiTitle;

            var page = Pages.FindPage(Request["Page"]);

            PageContent content;

            if (page != null)
            {
                content = Content.GetPageContent(page, true);

                lblTitle.Text = Properties.Messages.PageIncomingLinks + ": " + FormattingPipeline.PrepareTitle(content.Title, false, FormattingContext.PageContent, page);

                if (!AuthChecker.CheckActionForPage(page, Actions.ForPages.ReadPage, SessionFacade.GetCurrentUsername(), SessionFacade.GetCurrentGroupNames()))
                {
                    UrlTools.Redirect("AccessDenied.aspx");
                    return;
                }
            }

            var incomingLinks = Pages.GetPageIncomingLinks(page);

            foreach (var link in incomingLinks)
            {
                var linkPage = Pages.FindPage(link);
                var linkPageContent = linkPage.Provider.GetContent(linkPage);
                ulItems.InnerHtml += string.Format("<li><a href=\"{0}\">{1}</a></li>", UrlTools.BuildUrl(Tools.UrlEncode(link), Settings.PageExtension), linkPageContent.Title);
            }
        }
    }
}