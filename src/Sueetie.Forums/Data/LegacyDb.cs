/* Yet Another Forum.NET
 * Copyright (C) 2003-2005 Bjørnar Henden
 * Copyright (C) 2006-2009 Jaben Cargman
 * http://www.yetanotherforum.net/
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using YAF.Classes.Data;
using YAF.Classes;
using YAF.Types.Interfaces;

namespace Sueetie.Forums.Data
{
    public static class LegacyDb
    {
        #region Message

        public static DataTable post_list(
     object topicId,
     object authorUserID,
     object updateViewCount,
     bool showDeleted,
     bool styledNicks,
     DateTime sincePostedDate,
     DateTime toPostedDate,
     DateTime sinceEditedDate,
     DateTime toEditedDate,
     int pageIndex,
     int pageSize,
     int sortPosted,
     int sortEdited,
     int sortPosition,
     bool showThanks,
     int messagePosition)
        {
            using (var cmd = MsSqlDbAccess.GetCommand("sueetie_post_list"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("TopicID", topicId);
                cmd.Parameters.AddWithValue("AuthorUserID", authorUserID);
                cmd.Parameters.AddWithValue("UpdateViewCount", updateViewCount);
                cmd.Parameters.AddWithValue("ShowDeleted", showDeleted);
                cmd.Parameters.AddWithValue("StyledNicks", styledNicks);
                cmd.Parameters.AddWithValue("SincePostedDate", sincePostedDate);
                cmd.Parameters.AddWithValue("ToPostedDate", toPostedDate);
                cmd.Parameters.AddWithValue("SinceEditedDate", sinceEditedDate);
                cmd.Parameters.AddWithValue("ToEditedDate", toEditedDate);
                cmd.Parameters.AddWithValue("PageIndex", pageIndex);
                cmd.Parameters.AddWithValue("PageSize", pageSize);
                cmd.Parameters.AddWithValue("SortPosted", sortPosted);
                cmd.Parameters.AddWithValue("SortEdited", sortEdited);
                cmd.Parameters.AddWithValue("SortPosition", sortPosition);
                cmd.Parameters.AddWithValue("ShowThanks", showThanks);
                cmd.Parameters.AddWithValue("MessagePosition", messagePosition);

                return MsSqlDbAccess.Current.GetData(cmd);
            }
        }

        // Sueetie Modified - Using Sueetie_Post_List Procedure
        //public static DataTable post_list(object topicID, object updateViewCount, bool showDeleted, bool styledNicks)
        //{
        //    using (SqlCommand cmd = YafDBAccess.GetCommand("sueetie_post_list"))
        //    {
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("TopicID", topicID);
        //        cmd.Parameters.AddWithValue("UpdateViewCount", updateViewCount);
        //        cmd.Parameters.AddWithValue("ShowDeleted", showDeleted);
        //        cmd.Parameters.AddWithValue("StyledNicks", styledNicks);

        //        return YafDBAccess.Current.GetData(cmd);
        //    }
        //}

        #endregion

        #region Member Lists

        //static public DataTable user_list(object boardID, object sueetieUserID, int followType)
        //{
        //    using (SqlCommand cmd = YafDBAccess.GetCommand("sueetie_user_list"))
        //    {
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("BoardID", boardID);
        //        cmd.Parameters.AddWithValue("SueetieUserID", sueetieUserID);
        //        cmd.Parameters.AddWithValue("FollowType", followType);
        //        return YafDBAccess.Current.GetData(cmd);
        //    }
        //}

        #endregion

        #region Topics

        //static public DataTable sueetie_topic_unanswered(object boardID, object userId, object announcement, object date, object offset, object count, object useStyledNicks)
        //{
        //    using (SqlCommand cmd = YafDBAccess.GetCommand("sueetie_topic_unanswered"))
        //    {
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("BoardID", boardID);
        //        cmd.Parameters.AddWithValue("UserID", userId);
        //        cmd.Parameters.AddWithValue("Announcement", announcement);
        //        cmd.Parameters.AddWithValue("Date", date);
        //        cmd.Parameters.AddWithValue("Offset", offset);
        //        cmd.Parameters.AddWithValue("Count", count);
        //        cmd.Parameters.AddWithValue("@StyledNicks", useStyledNicks);
        //        return YafDBAccess.Current.GetData(cmd, true);
        //    }
        //}

        //static public DataTable sueetie_topic_popular(object boardID, object userId, object announcement, object date, object offset, object count, object useStyledNicks)
        //{
        //    using (SqlCommand cmd = YafDBAccess.GetCommand("sueetie_topic_popular"))
        //    {
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("BoardID", boardID);
        //        cmd.Parameters.AddWithValue("UserID", userId);
        //        cmd.Parameters.AddWithValue("Announcement", announcement);
        //        cmd.Parameters.AddWithValue("Date", date);
        //        cmd.Parameters.AddWithValue("Offset", offset);
        //        cmd.Parameters.AddWithValue("Count", count);
        //        cmd.Parameters.AddWithValue("@StyledNicks", useStyledNicks);
        //        return YafDBAccess.Current.GetData(cmd, true);
        //    }

        //}

        //static public DataTable sueetie_topic_latest(object boardID, object userId, object announcement, object date, object offset, object count, object useStyledNicks)
        //{
        //    using (SqlCommand cmd = YafDBAccess.GetCommand("sueetie_topic_latest"))
        //    {
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("BoardID", boardID);
        //        cmd.Parameters.AddWithValue("UserID", userId);
        //        cmd.Parameters.AddWithValue("Announcement", announcement);
        //        cmd.Parameters.AddWithValue("Date", date);
        //        cmd.Parameters.AddWithValue("Offset", offset);
        //        cmd.Parameters.AddWithValue("Count", count);
        //        cmd.Parameters.AddWithValue("@StyledNicks", useStyledNicks);
        //        return YafDBAccess.Current.GetData(cmd, true);
        //    }

        //}

        //static public DataTable sueetie_topic_favorites(object boardID, object userID, object applicationID, object contentTypeID, object sueetieUserID)
        //{
        //    using (SqlCommand cmd = YafDBAccess.GetCommand("sueetie_topic_favorites"))
        //    {
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("BoardID", boardID);
        //        cmd.Parameters.AddWithValue("UserID", userID);
        //        cmd.Parameters.AddWithValue("ApplicationID", applicationID);
        //        cmd.Parameters.AddWithValue("ContentTypeID", contentTypeID);
        //        cmd.Parameters.AddWithValue("SueetieUserID", sueetieUserID);
        //        return YafDBAccess.Current.GetData(cmd);
        //    }
        //}
        //static public DataTable sueetie_message_favorites(object boardID, object userID, object applicationID, object contentTypeID, object sueetieUserID)
        //{
        //    using (SqlCommand cmd = YafDBAccess.GetCommand("sueetie_message_favorites"))
        //    {
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("BoardID", boardID);
        //        cmd.Parameters.AddWithValue("UserID", userID);
        //        cmd.Parameters.AddWithValue("ApplicationID", applicationID);
        //        cmd.Parameters.AddWithValue("ContentTypeID", contentTypeID);
        //        cmd.Parameters.AddWithValue("SueetieUserID", sueetieUserID);
        //        return YafDBAccess.Current.GetData(cmd);
        //    }
        //}
        #endregion

    }
}