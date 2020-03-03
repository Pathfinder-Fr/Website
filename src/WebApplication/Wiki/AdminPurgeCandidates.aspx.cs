using System;
using System.Linq;
using ScrewTurn.Wiki.PluginFramework;

namespace ScrewTurn.Wiki
{
    public partial class AdminPurgeCandidates : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var context = this.Context;

            context.Response.ContentType = "text/plain";

            AdminMaster.RedirectToLoginIfNeeded();

            if (!AdminMaster.CanManageConfiguration(SessionFacade.GetCurrentUsername(), SessionFacade.GetCurrentGroupNames()))
                UrlTools.Redirect("~/AccessDenied.aspx");

            var qs = context.Request.QueryString;

            var ns = qs["NS"];
            var nspace = Pages.GetNamespaces().FirstOrDefault(x => x.Name == ns);
            if (nspace == null)
            {
                context.Response.Write(string.Format("Namespace \"{0}\" not found", ns));
                return;
            }
            
            // Liste des pages orphelines
            var candidates = Pages.GetOrphanedPages(nspace)
                // récupération contenu
                .Select(p => new { PageInfo = p, PageContent = p.Provider.GetContent(p) })
                // qui n'ont pas été modifées depuis plus d'un mois
                .Where(p => p.PageContent != null && p.PageContent.LastModified < DateTime.Today.AddMonths(-1))
                .OrderBy(p => p.PageInfo.FullName);

            foreach(var candidate in candidates)
            {
                context.Response.Write(string.Format("[url=/Wiki/{0}.ashx]{0}[/url] \"{1}\"\r\n",
                    candidate.PageInfo.FullName,
                    candidate.PageContent.Title));
            }
        }
    }
}