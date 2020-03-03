namespace ScrewTurn.Wiki
{
    using System;
    using System.Linq;

    public partial class AdminCleanUp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var context = this.Context;

            context.Response.ContentType = "text/plain";

            AdminMaster.RedirectToLoginIfNeeded();

            if (!AdminMaster.CanManageConfiguration(SessionFacade.GetCurrentUsername(), SessionFacade.GetCurrentGroupNames()))
                UrlTools.Redirect("~/AccessDenied.aspx");

            var qs = context.Request.QueryString;

            // Espace de nom de filtrage
            var ns = qs["NS"];
            var nspace = Pages.GetNamespaces().FirstOrDefault(x => x.Name == ns);
            if (nspace == null)
            {
                context.Response.Write(string.Format("Namespace \"{0}\" not found", ns));
                return;
            }

            // On ne supprime que les révisions datant de plus de xx jours par rapport à la dernière modification
            var ageDays = 30;

            // On ne supprime que les révisions au délà de cette limite
            var revisions = 10;

            if (!string.IsNullOrEmpty(qs["age"]))
                int.TryParse(qs["age"], out ageDays);

            if (!string.IsNullOrEmpty(qs["revisions"]))
                int.TryParse(qs["revisions"], out revisions);

            foreach (var page in Pages.GetPages(nspace))
            {
                // Liste des revisions de la page, du plus grand (plus récent) au plus petit (plus vieux)
                var backups = Pages.GetBackups(page);

                if (backups == null)
                {
                    context.Response.Write(string.Format("ERROR : Page {0} has no backups\r\n", page.FullName));
                    continue;
                }

                backups = backups.OrderByDescending(x => x).ToList();

                if (page.Provider == null)
                {
                    context.Response.Write(string.Format("ERROR : Page {0} has no provider\r\n", page.FullName));
                    continue;
                }

                var pageContent = page.Provider.GetContent(page);

                if (pageContent == null)
                {
                    context.Response.Write(string.Format("ERROR : Page {0} has no content\r\n", page.FullName));
                    continue;
                }

                var pageLastModified = pageContent.LastModified;

                var count = backups.Count;

                int? mostRecentRevisionToDelete = null;

                for (int i = 0; i < count; i++)
                {
                    var revision = backups[i];

                    if (i >= revisions)
                    {
                        // On a dépassé le nombre de révisions max, on va supprimer à partir de cette révision
                        var backupContent = Pages.GetBackupContent(page, revision);

                        // Nombre de jours écoulés depuis la dernière modification
                        var age = (pageLastModified - backupContent.LastModified).TotalDays;

                        if (age > ageDays)
                        {
                            mostRecentRevisionToDelete = revision;
                            break;
                        }
                    }
                }

                if (mostRecentRevisionToDelete.HasValue)
                {
                    if (mostRecentRevisionToDelete.Value == backups[0])
                    {
                        context.Response.Write(string.Format("Suppression de toutes les révisions pour la page {1}\r\n", mostRecentRevisionToDelete.Value, page.FullName));
                        Pages.DeleteBackups(page);

                    }
                    else
                    {
                        context.Response.Write(string.Format("Suppression des révisions <= {0} pour la page {1}\r\n", mostRecentRevisionToDelete.Value, page.FullName));
                        Pages.DeleteBackups(page, mostRecentRevisionToDelete.Value);
                    }
                }
            }
        }
    }
}