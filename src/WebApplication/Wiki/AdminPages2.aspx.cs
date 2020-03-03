namespace ScrewTurn.Wiki
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.UI.WebControls;
    using ScrewTurn.Wiki.PluginFramework;
    using CacheWiki = ScrewTurn.Wiki.Cache;

    public partial class AdminPages2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var context = this.Context;
            var request = context.Request;

            var currentNamespace = request.QueryString["namespace"] ?? string.Empty;
            var orphanOnly = (request.QueryString["orphanOnly"] ?? string.Empty) == "true";
            var name = request.QueryString["name"];
            var title = request.QueryString["title"];
            var sortKey = request.QueryString["sortKey"];
            var sortDesc = (request.QueryString["sortKey"] ?? string.Empty) == "true";
            var pageIndex = int.Parse(request.QueryString["pageIndex"] ?? "0");
            var pageSize = int.Parse(request.QueryString["pageSize"] ?? "50");
            var incomingLink = request.QueryString["incomingLink"];
            var outgoingLink = request.QueryString["outgoingLink"];

            var action = request.QueryString["action"];
            var id = request.QueryString["id"];

            var url = SetParameter(SetParameter(this.Request.Url.PathAndQuery, "action", null), "id", null);

            this.@namespace.Items.Add(new ListItem("<root>", "") { Selected = string.IsNullOrEmpty(currentNamespace) });
            this.@namespace.Items.AddRange(Pages.GetNamespaces().Select(n => new ListItem(n.Name, n.Name) { Selected = currentNamespace.Equals(n.Name) }).ToArray());

            this.sortKey.Items.AddRange(new[] {
                new ListItem("Nom", "name"),
                new ListItem("Titre", "title"),
                new ListItem("Dernière modification", "lastModified"),
                new ListItem("Création", "created"),
                new ListItem("Nombre de versions", "backupCount")
            });
            this.sortKey.Items.Cast<ListItem>().ForEach(x => x.Selected = (sortKey == x.Value));

            this.pageSize.Items.AddRange(new[] {
                new ListItem("30"),
                new ListItem("50"),
                new ListItem("100"),
                new ListItem("200"),
            });
            this.pageSize.Items.Cast<ListItem>().ForEach(x => x.Selected = (pageSize.ToString() == x.Value));

            this.orphanOnly.Checked = orphanOnly;
            this.title.Value = title;
            this.name.Value = name;
            /*
            this.incomingLink.Value = incomingLink;
            this.outgoingLink.Value = outgoingLink;
             
            this.firstLink.Visible = pageIndex > 0;
            this.firstLink.HRef = this.SetParameter(url, "pageIndex", "0");
            this.previousLink.Visible = pageIndex > 0;
            this.previousLink.HRef = this.SetParameter(url, "pageIndex", (pageIndex - 1).ToString());
            this.nextLink.HRef = this.SetParameter(url, "pageIndex", (pageIndex + 1).ToString());
             */

            switch (action)
            {
                case "del":
                    this.DoDelete(id);
                    break;
            }

            var nspace = Pages.GetNamespaces().FirstOrDefault(x => x.Name == currentNamespace);

            var filter = new FindPagesFilter()
            {
                Name = name,
                Namespace = nspace,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            IEnumerable<PageInfo> pages = null;
            if (orphanOnly)
            {
                pages = filter.ApplyOn(Pages.GetOrphanedPages(nspace));
            }
            else
            {
                pages = Pages.FindPages(filter).SelectMany(p => p);
            }

            var pagesData = pages.Select(p => new PageData
            {
                Info = p,
                Content = Content.GetPageContent(p, true),
                Creation = p.Provider.GetBackupContent(p, 0),
                Backups = p.Provider.GetBackups(p),
                IncomingLinks = Pages.GetPageIncomingLinks(p),
                OutgoingLinks = Pages.GetPageOutgoingLinks(p)
            }).ToList();

            var table = new StringBuilder();

            foreach (var page in pagesData)
            {
                table.Append("<tr>")
                    .AppendFormat("<th><a href=\"{1}.ashx\">{0}</a></th>", NameTools.GetLocalName(page.Info.FullName), page.Info.FullName)
                    .AppendFormat("<td>{0}</td>", page.Content.Title)
                    .AppendFormat("<td class=\"center\">{0}</td>", page.Info.CreationDateTime.ToString("dd/MM/yyyy HH:mm"))
                    .AppendFormat("<td class=\"center\">{0}</td>", page.Creation != null ? page.Creation.User : page.Content.User)
                    .AppendFormat("<td class=\"center\">{0}</td>", page.HasBackups ? page.Content.LastModified.ToString("dd/MM/yyyy HH:mm") : "-")
                    .AppendFormat("<td class=\"center\">{0}</td>", page.HasBackups ? page.Content.User : "-")
                    .AppendFormat("<td class=\"center\">{0}</td>", page.Info.Provider.GetBackups(page.Info).Length + 1)
                    .AppendFormat("<td class=\"center\">{0} / {1}</td>", page.IncomingLinks.Length, page.OutgoingLinks.Length)
                    .Append("<td class=\"center\">")
                    .AppendFormat("<a href=\"{0}\" class=\"delLink\">Suppr.</a>", this.SetParameter(this.SetParameter(url, "action", "del"), "id", page.Info.FullName))
                    .Append("</td>")
                    .Append("</tr>");
            }

            this.tableFooterRemark.Text = string.Format("{0} pages renvoyée(s)", pagesData.Count);

            this.tableBody.InnerHtml = table.ToString();
        }

        private void DoDelete(string id)
        {
            var page = Pages.FindPage(id);
            if (page != null)
            {
                Pages.DeletePage(page);
            }
        }

        private string SetParameter(string url, string name, string value)
        {
            return ParameterSetter.SetParameter(url, name, value);
        }

        class ParameterSetter
        {
            private bool replaced;

            private string newValue;

            private ParameterSetter()
            {

            }

            private string Run(string url, string name, string value)
            {
                this.newValue = (string.IsNullOrEmpty(value)) ? string.Empty : name + "=" + Uri.EscapeDataString(value);

                url = Regex.Replace(url, name + @"=[^&\r\n]+", Evaluator);

                if (replaced)
                    return url;

                return url + AppendSeparator(url) + this.newValue;
            }

            private string AppendSeparator(string url)
            {
                if (url.IndexOf('?') == -1)
                    return "?";
                else if (url[url.Length - 1] == '&')
                    return string.Empty;
                else
                    return "&";
            }

            private string Evaluator(Match m)
            {
                this.replaced = true;
                return this.newValue;
            }

            public static string SetParameter(string url, string name, string value)
            {
                var setter = new ParameterSetter();
                return setter.Run(url, name, value);
            }

        }

        class PageData
        {
            public PageData()
            {
            }

            public PageInfo Info { get; set; }

            public PageContent Content { get; set; }

            public PageContent Creation { get; set; }

            public int[] Backups { get; set; }

            public bool HasBackups { get { return this.Backups.Length != 0; } }

            public string[] IncomingLinks { get; set; }

            public string[] OutgoingLinks { get; set; }
        }
    }
}