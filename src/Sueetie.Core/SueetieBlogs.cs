// -----------------------------------------------------------------------
// <copyright file="SueetieBlogs.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class SueetieBlogs
    {
        public static SueetieBlog GetSueetieBlog(int blogId)
        {
            return GetSueetieBlogList().Find(b => b.BlogID == blogId);
        }

        public static SueetieBlog GetSueetieBlog(string applicationKey)
        {
            return GetSueetieBlogList().Find(b => b.ApplicationKey == applicationKey);
        }


        public static List<SueetieBlog> GetSueetieBlogTitles()
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.GetSueetieBlogTitles();
        }

        public static void UpdateSueetieBlog(SueetieBlog sueetieBlog)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.UpdateSueetieBlog(sueetieBlog);
        }

        public static void CreateSueetieBlog(SueetieBlog sueetieBlog)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.CreateSueetieBlog(sueetieBlog);
        }

        public static void SetMostRecentContentId(SueetieBlog sueetieBlog)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.SetMostRecentContentID(sueetieBlog);
        }

        public static int CreateUpdateSueetieBlogPost(SueetieBlogPost sueetieBlogPost)
        {
            var provider = SueetieDataProvider.LoadProvider();
            var contentId = provider.CreateUpdateSueetieBlogPost(sueetieBlogPost);
            ClearBlogPostListCache(sueetieBlogPost);
            return contentId;
        }

        public static int CreateUpdateSueetieBlogComment(SueetieBlogComment sueetieBlogComment)
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.CreateUpdateSueetieBlogComment(sueetieBlogComment);
        }

        public static SueetieBlogPost GetSueetieBlogPost(string postGuid)
        {
            return GetSueetieBlogPostList().Find(p => p.PostID.ToString().ToLower() == postGuid.ToLower());
        }

        public static SueetieBlogPost GetSueetieBlogPost(int sueetiePostId)
        {
            var sueetieBlogPost = GetSueetieBlogPostList().Find(p => p.SueetiePostID == sueetiePostId);
            if (sueetieBlogPost == null)
                sueetieBlogPost = GetSueetieBlogPostList(false).Find(p => p.SueetiePostID == sueetiePostId);
            return sueetieBlogPost;
        }

        public static SueetieBlogComment GetSueetieBlogComment(string postcommentid)
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.GetSueetieBlogComment(postcommentid);
        }

        public static List<SueetieBlogPost> GetSueetieBlogPostList()
        {
            return GetSueetieBlogPostList(true);
        }

        public static List<SueetieBlogPost> GetSueetieBlogPostList(bool useCache)
        {
            var sueetieBlogPosts = new List<SueetieBlogPost>();
            var provider = SueetieDataProvider.LoadProvider();
            var key = SueetieBlogPostListCacheKey();

            if (useCache)
            {
                sueetieBlogPosts = SueetieCache.Current[key] as List<SueetieBlogPost>;
                if (sueetieBlogPosts == null)
                {
                    sueetieBlogPosts = provider.GetSueetieBlogPostList();
                    SueetieCache.Current.Insert(key, sueetieBlogPosts);
                }
            }
            else
            {
                sueetieBlogPosts = provider.GetSueetieBlogPostList();
            }
            return sueetieBlogPosts;
        }

        public static string SueetieBlogPostListCacheKey()
        {
            return string.Format("SueetieBlogPostList-{0}", SueetieConfiguration.Get().Core.SiteUniqueName);
        }

        public static void ClearSueetieBlogPostListCache()
        {
            SueetieCache.Current.Remove(SueetieBlogPostListCacheKey());
        }

        public static List<SueetieBlog> GetSueetieBlogList()
        {
            var key = SueetieBlogListCacheKey();

            var sueetieBlogs = SueetieCache.Current[key] as List<SueetieBlog>;
            if (sueetieBlogs == null)
            {
                var provider = SueetieDataProvider.LoadProvider();
                sueetieBlogs = provider.GetSueetieBlogList();
                SueetieCache.Current.Insert(key, sueetieBlogs);
            }

            return sueetieBlogs;
        }

        public static string SueetieBlogListCacheKey()
        {
            return string.Format("SueetieAllBlogList-{0}", SueetieConfiguration.Get().Core.SiteUniqueName);
        }


        public static List<SueetieBlog> GetSueetieBlogList(ApplicationQuery applicationQuery)
        {
            var provider = SueetieDataProvider.LoadProvider();

            var key = BlogListCacheKey(applicationQuery);

            var cachedSueetieBlogs = SueetieCache.Current[key] as List<SueetieBlog>;
            if (cachedSueetieBlogs != null)
            {
                if (applicationQuery.NumRecords > 0)
                    return cachedSueetieBlogs.Take(applicationQuery.NumRecords).ToList();
                return cachedSueetieBlogs.ToList();
            }

            var sueetieBlogs = from p in provider.GetSueetieBlogList(applicationQuery)
                select p;

            if (applicationQuery.GroupID > -1)
                sueetieBlogs = from g in sueetieBlogs where g.GroupID == applicationQuery.GroupID select g;

            if (applicationQuery.CategoryID > 0)
                sueetieBlogs = from a in sueetieBlogs where a.CategoryID == applicationQuery.CategoryID select a;

            if (applicationQuery.IsRestricted)
                sueetieBlogs = from r in sueetieBlogs where r.BlogAccessRole == null select r;

            if (applicationQuery.CacheMinutes > 0)
                SueetieCache.Current.InsertMinutes(key, sueetieBlogs.ToList(), applicationQuery.CacheMinutes);
            else
                SueetieCache.Current.InsertMax(key, sueetieBlogs.ToList());


            if (applicationQuery.NumRecords > 0)
                return sueetieBlogs.Take(applicationQuery.NumRecords).ToList();
            return sueetieBlogs.ToList();
        }

        public static List<SueetieBlogPost> GetSueetieBlogPostList(ContentQuery contentQuery)
        {
            var provider = SueetieDataProvider.LoadProvider();

            var key = BlogPostListCacheKey(contentQuery);

            var cachedSueetieBlogPosts = SueetieCache.Current[key] as List<SueetieBlogPost>;
            if (cachedSueetieBlogPosts != null)
            {
                if (contentQuery.NumRecords > 0)
                    return cachedSueetieBlogPosts.Take(contentQuery.NumRecords).ToList();
                return cachedSueetieBlogPosts.ToList();
            }

            var sueetieBlogPosts = from p in provider.GetSueetieBlogPostList(contentQuery)
                select p;

            if (contentQuery.GroupID > -1)
                sueetieBlogPosts = from g in sueetieBlogPosts where g.GroupID == contentQuery.GroupID select g;

            if (contentQuery.ApplicationID > 0)
                sueetieBlogPosts = from a in sueetieBlogPosts where a.ApplicationID == contentQuery.ApplicationID select a;

            if (contentQuery.UserID > 0)
                sueetieBlogPosts = from u in sueetieBlogPosts where u.UserID == contentQuery.UserID select u;

            if (contentQuery.IsRestricted)
                sueetieBlogPosts = from r in sueetieBlogPosts where r.IsRestricted == false select r;

            if (contentQuery.CacheMinutes > 0)
                SueetieCache.Current.InsertMinutes(key, sueetieBlogPosts.ToList(), contentQuery.CacheMinutes);
            else
                SueetieCache.Current.InsertMax(key, sueetieBlogPosts.ToList());


            if (contentQuery.NumRecords > 0)
                return sueetieBlogPosts.Take(contentQuery.NumRecords).ToList();
            return sueetieBlogPosts.ToList();
        }

        public static List<SueetieBlogComment> GetSueetieBlogCommentList(ContentQuery contentQuery)
        {
            var provider = SueetieDataProvider.LoadProvider();

            var key = BlogCommentListCacheKey(contentQuery);

            var cachedSueetieBlogComments = SueetieCache.Current[key] as List<SueetieBlogComment>;
            if (cachedSueetieBlogComments != null)
            {
                if (contentQuery.NumRecords > 0)
                    return cachedSueetieBlogComments.Take(contentQuery.NumRecords).ToList();
                return cachedSueetieBlogComments.ToList();
            }

            var sueetieBlogComments = from p in provider.GetSueetieBlogCommentList(contentQuery)
                select p;

            if (contentQuery.GroupID > -1)
                sueetieBlogComments = from g in sueetieBlogComments where g.GroupID == contentQuery.GroupID select g;

            if (contentQuery.ApplicationID > 0)
                sueetieBlogComments = from a in sueetieBlogComments where a.ApplicationID == contentQuery.ApplicationID select a;

            if (contentQuery.UserID > 0)
                sueetieBlogComments = from u in sueetieBlogComments where u.UserID == contentQuery.UserID select u;

            if (contentQuery.IsRestricted)
                sueetieBlogComments = from r in sueetieBlogComments where r.IsRestricted == false select r;

            if (contentQuery.NumRecords > 0)
                sueetieBlogComments = from n in sueetieBlogComments.Take(contentQuery.NumRecords) select n;

            if (contentQuery.CacheMinutes > 0)
                SueetieCache.Current.InsertMinutes(key, sueetieBlogComments.ToList(), contentQuery.CacheMinutes);
            else
                SueetieCache.Current.InsertMax(key, sueetieBlogComments.ToList());

            return sueetieBlogComments.ToList();
        }

        private static string BlogPostListCacheKey(ContentQuery contentQuery)
        {
            return string.Format("BlogPostList-{0}-{1}-{2}-{3}-{4}", contentQuery.GroupID, contentQuery.ApplicationID,
                contentQuery.UserID, contentQuery.IsRestricted, contentQuery.SueetieContentViewTypeID);
        }

        private static string BlogListCacheKey(ApplicationQuery applicationQuery)
        {
            return string.Format("BlogList-{0}-{1}", SueetieConfiguration.Get().Core.SiteUniqueName, applicationQuery.CategoryID);
        }

        public static void ClearBlogPostListCache(ContentQuery contentQuery)
        {
            // Clearing cached BlogPost list for the post author and for the all users BlogPost listview
            SueetieCache.Current.Remove(BlogPostListCacheKey(contentQuery));
            contentQuery.UserID = (int)SueetieUserType.AllUsers;
            SueetieCache.Current.Remove(BlogPostListCacheKey(contentQuery));
            SueetieCache.Current.Remove(SueetieBlogPostListCacheKey());
        }

        private static void ClearBlogPostListCache(SueetieBlogPost sueetieBlogPost)
        {
            var contentQuery = new ContentQuery
            {
                GroupID = sueetieBlogPost.GroupID,
                UserID = sueetieBlogPost.UserID,
                ApplicationID = sueetieBlogPost.ApplicationID,
                IsRestricted = sueetieBlogPost.IsRestricted
            };
            ClearBlogPostListCache(contentQuery);
        }

        private static string BlogCommentListCacheKey(ContentQuery contentQuery)
        {
            return string.Format("BlogCommentList-{0}-{1}-{2}-{3}", contentQuery.GroupID, contentQuery.ApplicationID,
                contentQuery.UserID, contentQuery.IsRestricted);
        }

        public static void ClearBlogCommentListCache(ContentQuery contentQuery)
        {
            // Clearing cached BlogComment list for the post author and for the all users BlogComment listview
            SueetieCache.Current.Remove(BlogCommentListCacheKey(contentQuery));
            contentQuery.UserID = (int)SueetieUserType.AllUsers;
            SueetieCache.Current.Remove(BlogCommentListCacheKey(contentQuery));
            SueetieCache.Current.Remove(SueetieBlogListCacheKey());
        }

        private static void ClearBlogCommentListCache(SueetieBlogComment sueetieBlogComment)
        {
            var contentQuery = new ContentQuery
            {
                GroupID = sueetieBlogComment.GroupID,
                UserID = sueetieBlogComment.UserID,
                ApplicationID = sueetieBlogComment.ApplicationID,
                IsRestricted = sueetieBlogComment.IsRestricted
            };
            ClearBlogCommentListCache(contentQuery);
        }

        public static void CreateBlogAdmin(SueetieUser sueetieUser)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.CreateBlogAdmin(sueetieUser);
        }

        public static void SaveSueetieBlogSpam(SueetieBlogSpam sueetieBlogSpam)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.SaveSueetieBlogSpam(sueetieBlogSpam);
        }

        public static string CategoryUrls(SueetieBlogPost post)
        {
            if (string.IsNullOrEmpty(post.Categories))
                return SueetieLocalizer.GetString("no_categories");

            var sb = new StringBuilder();
            var firstItem = true;
            foreach (var category in post.Categories.Split('|'))
            {
                if (!firstItem)
                    sb.Append(", ");
                sb.Append(string.Format("<a href=\"/{0}/category/{1}.aspx\">{2}</a>", post.ApplicationKey, DataHelper.PrepCategoryLink(category), category));
                firstItem = false;
            }
            return sb.ToString();
        }
    }
}