// -----------------------------------------------------------------------
// <copyright file="UrlRewrite.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core.HttpModules
{
    using System;
    using System.Collections.Generic;
    using System.Web;

    public class UrlRewrite : IHttpModule
    {
        public void Dispose()
        {
            // Nothing to dispose
        }

        public void Init(HttpApplication application)
        {
            application.BeginRequest += this.context_BeginRequest;
        }

        private void context_BeginRequest(object sender, EventArgs e)
        {
            var context = ((HttpApplication)sender).Context;
            var _url = context.Request.Url.LocalPath.ToLower();
            if ((SueetieUrlHelper.IsPageRequest(context) || _url == "/") && (_url != "/install/default.aspx"))
            {
                var _uri = context.Request.Url;
                foreach (var _sueetieUrl in SueetieUrls.Instance.All)
                {
                    if (!string.IsNullOrEmpty(_sueetieUrl.ContentUrl) && _uri.Segments.Length == 3 && _url.IndexOf(".aspx") > 0)
                    {
                        var _groupKey = _uri.Segments[1].Replace("/", string.Empty);
                        var _slug = _uri.Segments[2].ToLower().Replace(".aspx", string.Empty);

                        var _allContentPages = SueetieContentParts.GetSueetieContentPageList();
                        var _contentPageUrls = this.GetContentUrls(SueetieUrls.Instance.All);
                        var _page =
                            _allContentPages.Find(p => _slug.Equals(p.PageSlug, StringComparison.OrdinalIgnoreCase)
                                                       && _groupKey.Equals(p.ApplicationKey, StringComparison.OrdinalIgnoreCase));


                        if (_page != null)
                        {
                            var _contentUrl = "/themes/{0}/pages/content.aspx?pg={1}";
                            try
                            {
                                _contentUrl = _contentPageUrls.Find(c => c.Url.ToLower().Substring(1, c.Url.LastIndexOf("/") - 1) == _groupKey).ContentUrl;
                            }
                            catch
                            {
                            }
                            context.RewritePath(string.Format(_contentUrl, SueetieContext.Current.Theme, _page.ContentPageID));
                            return;
                        }
                    }
                    else if (!string.IsNullOrEmpty(_sueetieUrl.RewrittenUrl) && _url == _sueetieUrl.Url)
                    {
                        context.RewritePath(string.Format(_sueetieUrl.RewrittenUrl, SueetieContext.Current.Theme));
                        return;
                    }
                }
            }
        }

        private List<SueetieUrl> GetContentUrls(IList<SueetieUrl> urls)
        {
            var _sueetieUrls = new List<SueetieUrl>();
            foreach (var _url in urls)
            {
                if (!string.IsNullOrEmpty(_url.ContentUrl))
                    _sueetieUrls.Add(_url);
            }
            return _sueetieUrls;
        }
    }
}