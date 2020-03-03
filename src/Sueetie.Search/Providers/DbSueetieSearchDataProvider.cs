using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Data.SqlClient;
using Sueetie.Core;

namespace Sueetie.Search
{

    public partial class DbSueetieSearchDataProvider : SueetieSearchDataProvider
    {

        #region ConnectionString

        private string connectionString;
        protected SqlConnection GetSqlConnection()
        {
            return new SqlConnection(ConnectionString);
        }
        public string ConnectionString
        {
            get
            {
                return connectionString;
            }
            set
            {
                connectionString = value;
            }
        }
        public DbSueetieSearchDataProvider(string _connectionString)
        {
            this.connectionString = _connectionString;
        }

        #endregion

        #region Blogs

        public override List<SueetieBlogPost> GetSueetieBlogPostsToIndex(int contenttypeID, DateTime pubDate)
        {
            List<SueetieBlogPost> sueetieBlogPosts = new List<SueetieBlogPost>();
            using (SqlConnection cn = GetSqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand("Sueetie_Search_BlogPosts_Get", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@contenttypeID", SqlDbType.Int, 4).Value = contenttypeID;
                    cmd.Parameters.Add("@pubDate", SqlDbType.DateTime, 8).Value = pubDate == DateTime.MinValue ? new DateTime(1900, 1, 1) : pubDate;
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        SueetieBlogPost _SueetieBlogPost = null;
                        while (dr.Read())
                        {
                            _SueetieBlogPost = new SueetieBlogPost();
                            PopulateSueetieBlogPostList(dr, _SueetieBlogPost);
                            sueetieBlogPosts.Add(_SueetieBlogPost);
                        }

                        dr.Close();
                        cn.Close();
                    }
                }
            }
            return sueetieBlogPosts;
        }

        #endregion

        #region Wiki Pages


        public override List<SueetieWikiPage> GetSueetieWikiPagesToIndex(int contenttypeID, DateTime pubDate)
        {
            List<SueetieWikiPage> sueetieWikiPages = new List<SueetieWikiPage>();
            using (SqlConnection cn = GetSqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand("Sueetie_Search_WikiPages_Get", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@contenttypeID", SqlDbType.Int, 4).Value = contenttypeID;
                    cmd.Parameters.Add("@pubDate", SqlDbType.DateTime, 8).Value = pubDate == DateTime.MinValue
                        ? new DateTime(1900, 1, 1)
                        : pubDate;
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        SueetieWikiPage _SueetieWikiPage = null;
                        while (dr.Read())
                        {
                            _SueetieWikiPage = new SueetieWikiPage();
                            PopulateSueetieWikiPageList(dr, _SueetieWikiPage);
                            sueetieWikiPages.Add(_SueetieWikiPage);
                        }

                        dr.Close();
                        cn.Close();
                    }
                }
            }
            return sueetieWikiPages;
        }


        #endregion

        #region Forum Messages

        public override List<SueetieForumMessage> GetSueetieForumMessagesToIndex(int contenttypeID, DateTime pubDate)
        {
            List<SueetieForumMessage> sueetieForumMessages = new List<SueetieForumMessage>();
            using (SqlConnection cn = GetSqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand("Sueetie_Search_ForumMessages_Get", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@contenttypeID", SqlDbType.Int, 4).Value = contenttypeID;
                    cmd.Parameters.Add("@pubDate", SqlDbType.DateTime, 8).Value = pubDate == DateTime.MinValue
                        ? new DateTime(1900, 1, 1)
                        : pubDate;
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        SueetieForumMessage _SueetieForumMessage = null;
                        while (dr.Read())
                        {
                            _SueetieForumMessage = new SueetieForumMessage();
                            PopulateSueetieForumMessageList(dr, _SueetieForumMessage);
                            sueetieForumMessages.Add(_SueetieForumMessage);
                        }

                        dr.Close();
                        cn.Close();
                    }
                }
            }
            return sueetieForumMessages;
        }

        #endregion

        #region Media Albums

        public override List<SueetieMediaAlbum> GetSueetieMediaAlbumsToIndex(DateTime pubDate)
        {
            List<SueetieMediaAlbum> sueetieMediaAlbums = new List<SueetieMediaAlbum>();
            using (SqlConnection cn = GetSqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand("Sueetie_Search_MediaAlbums_Get", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pubDate", SqlDbType.DateTime, 8).Value = pubDate == DateTime.MinValue
                        ? new DateTime(1900, 1, 1)
                        : pubDate;
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        SueetieMediaAlbum _SueetieMediaAlbum = null;
                        while (dr.Read())
                        {
                            _SueetieMediaAlbum = new SueetieMediaAlbum();
                            PopulateSueetieMediaAlbumList(dr, _SueetieMediaAlbum);
                            sueetieMediaAlbums.Add(_SueetieMediaAlbum);
                        }

                        dr.Close();
                        cn.Close();
                    }
                }
            }
            return sueetieMediaAlbums;
        }

        #endregion

        #region Media Objects

        public override List<SueetieMediaObject> GetSueetieMediaObjectsToIndex(DateTime pubDate)
        {
            List<SueetieMediaObject> sueetieMediaObjects = new List<SueetieMediaObject>();
            using (SqlConnection cn = GetSqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand("Sueetie_Search_MediaObjects_Get", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pubDate", SqlDbType.DateTime, 8).Value = pubDate == DateTime.MinValue
                        ? new DateTime(1900, 1, 1)
                        : pubDate;
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        SueetieMediaObject _SueetieMediaObject = null;
                        while (dr.Read())
                        {
                            _SueetieMediaObject = new SueetieMediaObject();
                            PopulateSueetieMediaObjectList(dr, _SueetieMediaObject);
                            sueetieMediaObjects.Add(_SueetieMediaObject);
                        }

                        dr.Close();
                        cn.Close();
                    }
                }
            }
            return sueetieMediaObjects;
        }

        #endregion

        #region Content Pages


        public override List<SueetieContentPage> GetSueetieContentPagesToIndex(int contenttypeID, DateTime pubDate)
        {
            List<SueetieContentPage> sueetieContentPages = new List<SueetieContentPage>();
            using (SqlConnection cn = GetSqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand("Sueetie_Search_ContentPages_Get", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@contenttypeID", SqlDbType.Int, 4).Value = contenttypeID;
                    cmd.Parameters.Add("@pubDate", SqlDbType.DateTime, 8).Value = pubDate == DateTime.MinValue
                        ? new DateTime(1900, 1, 1)
                        : pubDate;
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        SueetieContentPage _SueetieContentPage = null;
                        while (dr.Read())
                        {
                            _SueetieContentPage = new SueetieContentPage();
                            PopulateSueetieContentPageList(dr, _SueetieContentPage);
                            sueetieContentPages.Add(_SueetieContentPage);
                        }

                        dr.Close();
                        cn.Close();
                    }
                }
            }
            return sueetieContentPages;
        }


        #endregion

        #region Background Task Functions

        public override SueetieIndexTaskItem GetSueetieIndexTaskItem(int taskID)
        {
            SueetieIndexTaskItem _sueetieIndexItem = new SueetieIndexTaskItem();

            using (SqlConnection cn = GetSqlConnection())
            {

                SqlCommand cmd = new SqlCommand("Sueetie_Tasks_QueueInfo_Get", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TaskTypeID", SqlDbType.Int, 4).Value = taskID;				

                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        _sueetieIndexItem.TaskQueueID = (int)dr["taskqueueid"];
                        _sueetieIndexItem.TaskStartDateTime = dr["taskstartdatetime"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["taskstartdatetime"];
                    }

                    dr.Close();
                    cn.Close();

                }
            }
            return _sueetieIndexItem;
        }

        public override void UpdateAndEndTask(SueetieIndexTaskItem sueetieIndexTaskItem)
        {
            using (SqlConnection cn = GetSqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand("Sueetie_Search_IndexTask_End", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@TaskQueueID", SqlDbType.Int, 4).Value = sueetieIndexTaskItem.TaskQueueID;
                    cmd.Parameters.Add("@BlogPosts", SqlDbType.Int, 4).Value = sueetieIndexTaskItem.BlogPosts;
                    cmd.Parameters.Add("@ForumMessages", SqlDbType.Int, 4).Value = sueetieIndexTaskItem.ForumMessages;
                    cmd.Parameters.Add("@WikiPages", SqlDbType.Int, 4).Value = sueetieIndexTaskItem.WikiPages;
                    cmd.Parameters.Add("@MediaAlbums", SqlDbType.Int, 4).Value = sueetieIndexTaskItem.MediaAlbums;
                    cmd.Parameters.Add("@MediaObjects", SqlDbType.Int, 4).Value = sueetieIndexTaskItem.MediaObjects;
                    cmd.Parameters.Add("@ContentPages", SqlDbType.Int, 4).Value = sueetieIndexTaskItem.ContentPages;
                    cmd.Parameters.Add("@DocumentsIndexed", SqlDbType.Int, 4).Value = sueetieIndexTaskItem.DocumentsIndexed;

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }
            }

        }
		
	
        #endregion
    }
}
