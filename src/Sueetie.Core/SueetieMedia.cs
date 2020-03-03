// -----------------------------------------------------------------------
// <copyright file="SueetieMedia.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System.Collections.Generic;
    using System.Linq;

    public static class SueetieMedia
    {
        public static List<SueetieMediaAlbum> GetSueetieAlbumUpdateList(int galleryId)
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.GetSueetieAlbumUpdateList(galleryId);
        }

        public static List<SueetieMediaObject> GetSueetieMediaUpdateList()
        {
            return GetSueetieMediaUpdateList(-1);
        }

        public static List<SueetieMediaObject> GetSueetieMediaUpdateList(int albumId)
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.GetSueetieMediaUpdateList(albumId);
        }

        public static List<ContentTypeDescription> GetAlbumContentTypeDescriptionList()
        {
            var key = AlbumContentTypeDescriptionCacheKey();
            var albumContentTypeDescriptionList = SueetieCache.Current[key] as List<ContentTypeDescription>;
            if (albumContentTypeDescriptionList == null)
            {
                var provider = SueetieDataProvider.LoadProvider();
                albumContentTypeDescriptionList = provider.GetAlbumContentTypeDescriptionList();
                SueetieCache.Current.Insert(key, albumContentTypeDescriptionList);
            }
            return albumContentTypeDescriptionList;
        }

        public static string AlbumContentTypeDescriptionCacheKey()
        {
            return string.Format("SueetieApplicationList-{0}", SueetieConfiguration.Get().Core.SiteUniqueName);
        }

        public static SueetieMediaAlbum GetSueetieMediaAlbum(int albumId)
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.GetSueetieMediaAlbum(albumId);
        }

        public static void CreateSueetieAlbum(int albumId, string albumDirectory, int contentTypeId)
        {
            var dp = SueetieDataProvider.LoadProvider();
            dp.CreateSueetieAlbum(albumId, albumDirectory, contentTypeId);
        }

        public static List<SueetieMediaGallery> GetSueetieMediaGalleryList()
        {
            var key = SueetieMediaGalleryListCacheKey();

            var sueetieMediaGalleries = SueetieCache.Current[key] as List<SueetieMediaGallery>;
            if (sueetieMediaGalleries == null)
            {
                var provider = SueetieDataProvider.LoadProvider();
                sueetieMediaGalleries = provider.GetSueetieMediaGalleryList();
                SueetieCache.Current.Insert(key, sueetieMediaGalleries);
            }

            return sueetieMediaGalleries;
        }

        public static string SueetieMediaGalleryListCacheKey()
        {
            return string.Format("SueetieMediaGalleryList-{0}", SueetieConfiguration.Get().Core.SiteUniqueName);
        }

        public static void ClearSueetieMediaGalleryListCache()
        {
            SueetieCache.Current.Remove(SueetieMediaGalleryListCacheKey());
        }

        public static void AdminUpdateSueetieMediaGallery(string galleryKey, int displayTypeId, bool isPublic, bool isLogged, int galleryId)
        {
            var sueetieMediaGallery = new SueetieMediaGallery
            {
                GalleryID = galleryId,
                DisplayTypeID = displayTypeId,
                GalleryKey = galleryKey,
                IsLogged = isLogged,
                IsPublic = isPublic
            };
            var provider = SueetieDataProvider.LoadProvider();
            provider.AdminUpdateSueetieMediaGallery(sueetieMediaGallery);
            ClearSueetieMediaGalleryListCache();
        }


        public static void CreateMediaGallery(int galleryId)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.CreateMediaGallery(galleryId);
        }

        public static SueetieMediaGallery GetSueetieMediaGallery(int galleryId)
        {
            return GetSueetieMediaGalleryList().Find(p => p.GalleryID == galleryId);
        }

        public static SueetieMediaGallery GetSueetieMediaGallery(string galleryKey)
        {
            return GetSueetieMediaGalleryList().Find(p => p.GalleryKey == galleryKey);
        }

        public static SueetieMediaObject GetSueetieMediaPhoto(int mediaObjectId)
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.GetSueetieMediaPhoto(mediaObjectId);
        }

        public static SueetieMediaObject GetSueetieMediaObject(int galleryId, int mediaObjectId)
        {
            return GetSueetieMediaObjectList(galleryId).Find(p => p.MediaObjectID == mediaObjectId);
        }

        public static void CreateSueetieMediaObject(SueetieMediaObject sueetieMediaObject)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.CreateSueetieMediaObject(sueetieMediaObject);
        }

        public static int GetAlbumContentTypeId(int albumid)
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.GetAlbumContentTypeID(albumid);
        }

        public static void UpdateAlbumContentTypeId(SueetieMediaObject sueetieMediaObject)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.UpdateAlbumContentTypeID(sueetieMediaObject);
        }

        public static void AdminUpdateSueetieMediaAlbum(int albumId, int contentTypeId, int contentId)
        {
            var sueetieMediaAlbum = new SueetieMediaAlbum
            {
                AlbumID = albumId,
                ContentTypeID = contentTypeId,
                ContentID = contentId
            };
            var provider = SueetieDataProvider.LoadProvider();
            provider.AdminUpdateSueetieMediaAlbum(sueetieMediaAlbum);
        }

        public static void UpdateSueetieMediaObject(SueetieMediaObject sueetieMediaObject)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.UpdateSueetieMediaObject(sueetieMediaObject);
        }

        public static void UpdateSueetieMediaAlbum(SueetieMediaAlbum sueetieMediaAlbum)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.UpdateSueetieMediaAlbum(sueetieMediaAlbum);
        }

        public static List<SueetieMediaAlbum> GetSueetieMediaAlbumList(int galleryId)
        {
            var key = SueetieMediaAlbumListCacheKey(galleryId);

            var sueetieMediaAlbums = SueetieCache.Current[key] as List<SueetieMediaAlbum>;
            if (sueetieMediaAlbums == null)
            {
                var provider = SueetieDataProvider.LoadProvider();
                sueetieMediaAlbums = provider.GetSueetieMediaAlbumList(galleryId);
                SueetieCache.Current.Insert(key, sueetieMediaAlbums);
            }

            return sueetieMediaAlbums;
        }

        public static string SueetieMediaAlbumListCacheKey(int galleryId)
        {
            return string.Format("SueetieMediaAlbumList-{0}-{1}", SueetieConfiguration.Get().Core.SiteUniqueName, galleryId);
        }

        public static void ClearSueetieMediaAlbumListCache(int galleryId)
        {
            SueetieCache.Current.Remove(SueetieMediaAlbumListCacheKey(galleryId));
        }

        public static SueetieMediaAlbum GetSueetieMediaAlbum(int galleryId, int albumId)
        {
            return GetSueetieMediaAlbumList(galleryId).Find(p => p.AlbumID == albumId);
        }

        public static List<SueetieMediaObject> GetSueetieMediaObjectList(int galleryId)
        {
            return GetSueetieMediaObjectList(galleryId, false);
        }

        public static List<SueetieMediaObject> GetSueetieMediaObjectList(int galleryId, bool photosOnly)
        {
            var key = SueetieMediaObjectListCacheKey(galleryId);

            var sueetieMediaObjects = SueetieCache.Current[key] as List<SueetieMediaObject>;
            if (sueetieMediaObjects == null)
            {
                var provider = SueetieDataProvider.LoadProvider();
                sueetieMediaObjects = provider.GetSueetieMediaObjectList(galleryId);
                SueetieCache.Current.Insert(key, sueetieMediaObjects);
            }
            if (photosOnly)
            {
                return sueetieMediaObjects.Where(p => p.IsImage).ToList();
            }
            return sueetieMediaObjects;
        }

        public static string SueetieMediaObjectListCacheKey(int galleryId)
        {
            return string.Format("SueetieMediaObjectList-{0}", galleryId);
        }

        public static void ClearSueetieMediaObjectListCache(int galleryId)
        {
            SueetieCache.Current.Remove(SueetieMediaObjectListCacheKey(galleryId));
        }

        public static void RecordDownload(SueetieDownload sueetieDownload)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.RecordDownload(sueetieDownload);
        }

        public static List<SueetieDownload> GetSueetieDownloadList(ContentQuery contentQuery)
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.GetSueetieDownloadList(contentQuery);
        }

        public static bool IsIncludedInDownload(int mediaobjectid)
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.IsIncludedInDownload(mediaobjectid);
        }

        public static void SetIncludedInDownload(int mediaobjectid, bool includedInDownload)
        {
            var dp = SueetieDataProvider.LoadProvider();
            dp.SetIncludedInDownload(mediaobjectid, includedInDownload);
        }

        public static List<SueetieMediaObject> GetSueetieMediaObjectList(ContentQuery contentQuery)
        {
            var provider = SueetieDataProvider.LoadProvider();

            var key = MediaObjectListCacheKey(contentQuery.GroupID);

            var cachedSueetieMediaObjects = SueetieCache.Current[key] as List<SueetieMediaObject>;
            if (cachedSueetieMediaObjects != null)
            {
                if (contentQuery.NumRecords > 0)
                    return cachedSueetieMediaObjects.Take(contentQuery.NumRecords).ToList();
                return cachedSueetieMediaObjects.ToList();
            }

            var sueetieMediaObjects = from p in provider.GetSueetieMediaObjectList(contentQuery)
                select p;

            if (contentQuery.GroupID > -1)
                sueetieMediaObjects = from g in sueetieMediaObjects where g.GroupID == contentQuery.GroupID select g;

            if (contentQuery.ApplicationID > 0)
                sueetieMediaObjects = from a in sueetieMediaObjects where a.ApplicationID == contentQuery.ApplicationID select a;

            if (contentQuery.UserID > 0)
                sueetieMediaObjects = from u in sueetieMediaObjects where u.SueetieUserID == contentQuery.UserID select u;

            if (contentQuery.IsRestricted)
                sueetieMediaObjects = from r in sueetieMediaObjects where r.IsRestricted == false select r;

            if (contentQuery.CacheMinutes > 0)
                SueetieCache.Current.InsertMinutes(key, sueetieMediaObjects.ToList(), contentQuery.CacheMinutes);
            else
                SueetieCache.Current.InsertMax(key, sueetieMediaObjects.ToList());

            if (contentQuery.NumRecords > 0)
                return sueetieMediaObjects.Take(contentQuery.NumRecords).ToList();
            return sueetieMediaObjects.ToList();
        }

        public static List<SueetieMediaAlbum> GetSueetieMediaAlbumList()
        {
            var contentQuery = new ContentQuery
            {
                SueetieContentViewTypeID = -1,
                NumRecords = 1000,
                ContentTypeID = -1,
                ApplicationID = -1,
                UserID = -1,
                IsRestricted = false,
                GroupID = -1
            };
            var provider = SueetieDataProvider.LoadProvider();
            return provider.GetSueetieMediaAlbumList(contentQuery);
        }

        public static List<SueetieMediaAlbum> GetSueetieMediaAlbumList(ContentQuery contentQuery)
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.GetSueetieMediaAlbumList(contentQuery);
        }

        public static string MediaObjectListCacheKey(int groupId)
        {
            return string.Format("MediaPhotoList-{0}", groupId);
        }

        public static void ClearMediaPhotoListCache(int groupId)
        {
            SueetieCache.Current.Remove(MediaObjectListCacheKey(groupId));
        }

        public static string ThumbnailUrl(SueetieMediaObject sueetieMediaObject)
        {
            var extension = "folder";
            if (!sueetieMediaObject.IsAlbum)
            {
                var extensionStart = sueetieMediaObject.OriginalFilename.LastIndexOf(".") + 1;
                var extentionLength = sueetieMediaObject.OriginalFilename.Length - extensionStart;
                extension = sueetieMediaObject.OriginalFilename.Substring(extensionStart, extentionLength);
            }
            return string.Format("/themes/{0}/images/media/{1}.png", SueetieContext.Current.Theme, extension);
        }

        public static void PopulateMediaObjectTitles()
        {
            var dp = SueetieDataProvider.LoadProvider();
            dp.PopulateMediaObjectTitles();
        }

        public static string SueetieMediaObjectUrl(int mediaObjectId, int galleryId)
        {
            return "/" + SueetieApplications.Current.ApplicationKey + "/gs/handler/getmediaobject.ashx?moid=" + mediaObjectId + "&dt={0}&g=" + galleryId;
        }

        public static string PopulateMediaObjectUrl(string permalink, SueetieImageDisplayType displayType)
        {
            return string.Format(permalink, displayType);
        }

        public static int ConvertContentType(int mimeTypeCategory)
        {
            var sueetieContentType = new SueetieContentType();
            switch ((GSPMimeType)mimeTypeCategory)
            {
                case GSPMimeType.NotSet:
                case GSPMimeType.Other:
                    sueetieContentType = SueetieContentType.MediaOther;
                    break;
                case GSPMimeType.Document:
                    sueetieContentType = SueetieContentType.MediaDocument;
                    break;
                case GSPMimeType.Image:
                    sueetieContentType = SueetieContentType.MediaImage;
                    break;
                case GSPMimeType.Video:
                    sueetieContentType = SueetieContentType.MediaVideo;
                    break;
                case GSPMimeType.Audio:
                    sueetieContentType = SueetieContentType.MediaAudioFile;
                    break;
                default:
                    sueetieContentType = SueetieContentType.MediaOther;
                    break;
            }
            return (int)sueetieContentType;
        }

        public static void EnterMediaObjectTags(SueetieTagEntry sueetieTagEntry)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.EnterMediaObjectTags(sueetieTagEntry);
        }

        public static void EnterMediaAlbumTags(SueetieTagEntry sueetieTagEntry)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.EnterMediaAlbumTags(sueetieTagEntry);
        }

        public static string CreateSueetieAlbumPath(string albumFullPhysicalPath)
        {
            return albumFullPhysicalPath.Substring(albumFullPhysicalPath.IndexOf("mediaobjects") + 12).Replace("\\", "/") + "/";
        }

        public static void UpdateSueetieAlbumPath(SueetieMediaAlbum sueetieMediaAlbum)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.UpdateSueetieAlbumPath(sueetieMediaAlbum);
        }

        public static List<SueetieMediaDirectory> GetSueetieMediaDirectoryList()
        {
            var key = SueetieMediaDirectoryListCacheKey();

            var sueetieMediaDirectories = SueetieCache.Current[key] as List<SueetieMediaDirectory>;
            if (sueetieMediaDirectories == null)
            {
                var provider = SueetieDataProvider.LoadProvider();
                sueetieMediaDirectories = provider.GetSueetieMediaDirectoryList();
                SueetieCache.Current.Insert(key, sueetieMediaDirectories);
            }

            return sueetieMediaDirectories;
        }

        public static string SueetieMediaDirectoryListCacheKey()
        {
            return string.Format("SueetieMediaDirectoryList-{0}", SueetieConfiguration.Get().Core.SiteUniqueName);
        }

        public static void ClearSueetieMediaDirectoryListCache()
        {
            SueetieCache.Current.Remove(SueetieMediaDirectoryListCacheKey());
        }

        public static SueetieMediaDirectory GetSueetieMediaDirectory(int mediaObjectId)
        {
            return GetSueetieMediaDirectoryList().Find(d => d.MediaObjectId == mediaObjectId);
        }

        public static string GetSueetieMediaDirectoryFileName(SueetieImageDisplayType sueetieImageDisplayType, SueetieMediaDirectory sueetieMediaDirectory)
        {
            var filename = string.Empty;
            switch (sueetieImageDisplayType)
            {
                case SueetieImageDisplayType.Unknown:
                case SueetieImageDisplayType.External:
                case SueetieImageDisplayType.Thumbnail:
                    filename = sueetieMediaDirectory.ThumbnailFilename;
                    break;
                case SueetieImageDisplayType.Optimized:
                    filename = sueetieMediaDirectory.OptimizedFilename;
                    break;
                case SueetieImageDisplayType.Original:
                    filename = sueetieMediaDirectory.OriginalFilename;
                    break;
                default:
                    break;
            }
            return filename;
        }
    }
}