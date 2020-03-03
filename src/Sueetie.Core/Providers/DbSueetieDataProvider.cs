// -----------------------------------------------------------------------
// <copyright file="DbSueetieDataProvider.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.Data.SqlClient;
    using System.Web;
    using System.Web.UI;

    public class DbSueetieDataProvider : SueetieDataProvider
    {
        protected SqlConnection GetSqlConnection()
        {
            return new SqlConnection(this.ConnectionString);
        }

        public string ConnectionString { get; set; }

        public DbSueetieDataProvider(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public override StringDictionary GetSiteSettingsDictionary()
        {
            var dic = new StringDictionary();
            using (var cn = this.GetSqlConnection())
            {
                var sql = "SELECT SettingName, SettingValue FROM Sueetie_Settings";
                var cmd = new SqlCommand(sql, cn) { CommandType = CommandType.Text };

                try
                {
                    cn.Open();
                    var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        {
                            var name = dr["SettingName"] as string;
                            var value = dr["SettingValue"] as string;
                            dic.Add(name, value);
                        }
                    }
                    dr.Close();
                    cn.Close();
                }
                catch (SqlException ex)
                {
                    if (ex.Message.IndexOf("not open database") > 0)
                        HttpContext.Current.Response.Redirect("/install/default.aspx?DBERROR=1");
                    else
                        HttpContext.Current.Response.Redirect("/install/default.aspx");
                }
            }
            return dic;
        }

        public override void UpdateSiteSetting(SiteSetting siteSetting)
        {
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_SiteSetting_Update", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@SettingName", SqlDbType.NVarChar, 50).Value = siteSetting.SettingName;
                    cmd.Parameters.Add("@SettingValue", SqlDbType.NVarChar, -1).Value = DataHelper.StringOrNull(siteSetting.SettingValue);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }
            }
        }

        public override SueetieUser GetUser(int userID)
        {
            var sueetieUser = new SueetieUser();
            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("sueetie_user_getbyid", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = userID;
                cn.Open();

                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        PopulateSueetieUser(dr, sueetieUser);
                    }
                }
            }

            return sueetieUser;
        }

        public override SueetieUser GetUser(string username)
        {
            var sueetieUser = new SueetieUser();

            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("sueetie_user_getbyusername", cn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Add("@username", SqlDbType.NVarChar, 50).Value = username;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        PopulateSueetieUser(dr, sueetieUser);
                    }
                }
            }
            return sueetieUser;
        }

        public override SueetieUser GetSueetieUserByEmail(string email)
        {
            var sueetieUser = new SueetieUser();

            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("sueetie_user_getbyemail", cn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Add("@email", SqlDbType.NVarChar, 150).Value = email;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        PopulateSueetieUser(dr, sueetieUser);
                    }
                }
            }

            return sueetieUser;
        }

        public override SueetieUser GetSueetieUserFromForumID(int userforumid)
        {
            var _sueetieUser = new SueetieUser();

            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("Sueetie_User_getByForumID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UserForumID", SqlDbType.Int, 4).Value = userforumid;
                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        _sueetieUser.UserID = (int)dr["userid"];
                        _sueetieUser.MembershipID = DataHelper.GetGuid(dr, "membershipid");
                        _sueetieUser.UserName = dr["username"] as string;
                        _sueetieUser.Email = dr["email"] as string;
                        _sueetieUser.DisplayName = dr["displayname"] as string;
                        _sueetieUser.Bio = dr["bio"] as string;
                    }
                }
            }
            return _sueetieUser;
        }

        public override SueetieUserProfile GetSueetieUserProfile(int userId)
        {
            var sueetieUserProfile = new SueetieUserProfile();

            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("sueetie_User_GetProfileDetails", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@userId", SqlDbType.Int, 4).Value = userId;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        PopulateSueetieUserProfile(dr, sueetieUserProfile);
                    }
                }
            }
            return sueetieUserProfile;
        }

        public override SueetieUserProfile GetSueetieUserProfile(string username)
        {
            var sueetieUserProfile = new SueetieUserProfile();

            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("Sueetie_User_GetProfileByUsername", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@username", SqlDbType.NVarChar, 50).Value = username;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        PopulateSueetieUserProfile(dr, sueetieUserProfile);
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return sueetieUserProfile;
        }

        public override int GetUserID(string username)
        {
            int userId;
            using (var cn = this.GetSqlConnection())
            {
                var sql = "select userid from sueetie_users where username = @username";

                var cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = username;
                cmd.CommandType = CommandType.Text;
                cn.Open();
                userId = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                cn.Close();
            }

            return userId;
        }

        public override int CreateSueetieUser(SueetieUser sueetieUser)
        {
            var userID = 0;
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("sueetie_User_Create", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 50).Value = sueetieUser.UserName;
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 255).Value = sueetieUser.Email;
                    cmd.Parameters.Add("@MembershipID", SqlDbType.UniqueIdentifier, 16).Value = sueetieUser.MembershipID;
                    cmd.Parameters.Add("@DisplayName", SqlDbType.NVarChar, 150).Value = sueetieUser.DisplayName;
                    cmd.Parameters.Add("@IsActive", SqlDbType.Bit, 1).Value = sueetieUser.IsActive;
                    cmd.Parameters.Add("@IP", SqlDbType.NVarChar, 15).Value = sueetieUser.IP;
                    cn.Open();
                    userID = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                    cn.Close();
                }
            }
            return userID;
        }

        public override void UpdateSueetieUser(SueetieUser sueetieUser)
        {
            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("Sueetie_User_Update", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = sueetieUser.UserID;
                cmd.Parameters.Add("@MembershipID", SqlDbType.UniqueIdentifier, 16).Value = sueetieUser.MembershipID;
                cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 50).Value = sueetieUser.UserName;
                cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 255).Value = sueetieUser.Email;
                cmd.Parameters.Add("@DisplayName", SqlDbType.NVarChar, 150).Value = sueetieUser.DisplayName;
                cmd.Parameters.Add("@IsActive", SqlDbType.Bit, 1).Value = sueetieUser.IsActive;
                cmd.Parameters.Add("@TimeZone", SqlDbType.Int, 4).Value = sueetieUser.TimeZone;
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override void UpdateSueetieUserBio(SueetieUser sueetieUser)
        {
            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("Sueetie_User_UpdateBio", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = sueetieUser.UserID;
                cmd.Parameters.Add("@Bio", SqlDbType.NText).Value = sueetieUser.Bio;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override void UpdateDisplayName(SueetieUser sueetieUser)
        {
            using (var cn = this.GetSqlConnection())
            {
                //string sql = "update sueetie_users set displayname = '" +
                //   sueetieUser.DisplayName + "' where userid  = " + sueetieUser.UserID;
                var sql = "update sueetie_users set displayname = @displayName where userid = @userId";
                var cmd = new SqlCommand(sql, cn);

                cmd.Parameters.Add("@displayName", SqlDbType.VarChar).Value = sueetieUser.DisplayName;
                cmd.Parameters.Add("@userId", SqlDbType.Int).Value = sueetieUser.UserID;

                cmd.CommandType = CommandType.Text;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override SueetiePM[] GetUnreadPMs(string username)
        {
            var PMs = new List<SueetiePM>();
            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("Sueetie_UnreadPMs_Get", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UserName", SqlDbType.VarChar, 150).Value = username;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    while (dr.Read())
                    {
                        var curPM = new SueetiePM();
                        curPM.PMessageID = int.Parse(dr["PMessageID"].ToString());
                        curPM.FromUserID = int.Parse(dr["FromUserID"].ToString());
                        curPM.FromUser = dr["FromUser"].ToString();
                        curPM.Created = DateTime.Parse(dr["Created"].ToString());
                        curPM.Subject = dr["Subject"].ToString();
                        curPM.MessageUrl = string.Format("/{0}/default.aspx?g=cp_message&pm={1}&v=in", SueetieApplications.Get().Forum.ApplicationKey, curPM.PMessageID);

                        //curPM.MessageUrl = string.Format("http://{0}/forum/default.aspx?g=cp_message&pm={1}&v=in", System.Web.HttpContext.Current.Request.Url.Host, curPM.PMessageID.ToString());

                        PMs.Add(curPM);
                    }
                cn.Close();
            }
            return PMs.ToArray();
        }

        public override List<SueetieUser> GetSueetieUserList(SueetieUserType sueetieUserType)
        {
            var SueetieUserList = new List<SueetieUser>();
            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("Sueetie_Users_Get", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UserTypeID", SqlDbType.Int, 4).Value = (int)sueetieUserType;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    SueetieUser _SueetieUser = null;
                    while (dr.Read())
                    {
                        _SueetieUser = new SueetieUser();
                        PopulateSueetieUserList(dr, _SueetieUser);
                        SueetieUserList.Add(_SueetieUser);
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return SueetieUserList;
        }


        public override List<SueetieAspnetUser> GetUnapprovedUserList()
        {
            var SueetieAspnetUserList = new List<SueetieAspnetUser>();
            using (var cn = this.GetSqlConnection())
            {
                var sql = "select * from Sueetie_vw_AspnetUserlist where IsActive = 1 and IsApproved = 0 order by username";
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    SueetieAspnetUser _SueetieAspnetUser = null;
                    while (dr.Read())
                    {
                        _SueetieAspnetUser = new SueetieAspnetUser();
                        PopulateSueetieAspnetUserList(dr, _SueetieAspnetUser);
                        SueetieAspnetUserList.Add(_SueetieAspnetUser);
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return SueetieAspnetUserList;
        }

        public override List<SueetieAspnetUser> GetInactiveUserList()
        {
            var SueetieAspnetUserList = new List<SueetieAspnetUser>();
            using (var cn = this.GetSqlConnection())
            {
                var sql = "select * from Sueetie_vw_AspnetUserlist where IsActive = 0 and IsApproved = 0 order by username";
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    SueetieAspnetUser _SueetieAspnetUser = null;
                    while (dr.Read())
                    {
                        _SueetieAspnetUser = new SueetieAspnetUser();
                        PopulateSueetieAspnetUserList(dr, _SueetieAspnetUser);
                        SueetieAspnetUserList.Add(_SueetieAspnetUser);
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return SueetieAspnetUserList;
        }

        public override void DeactivateUser(string _username)
        {
            using (var cn = this.GetSqlConnection())
            {
                //string sql = "update sueetie_users set IsActive = 0 where username = '" + _username + "'";
                var sql = "update sueetie_users set IsActive = 0 where username = @username";
                var cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = _username;
                cmd.CommandType = CommandType.Text;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override List<SueetieBannedIP> GetSueetieBannedIPList()
        {
            var SueetieBannedIPList = new List<SueetieBannedIP>();
            using (var cn = this.GetSqlConnection())
            {
                var sql = "select * from Sueetie_BannedIPs";
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    SueetieBannedIP _SueetieBannedIP = null;
                    while (dr.Read())
                    {
                        _SueetieBannedIP = new SueetieBannedIP();
                        PopulateSueetieBannedIPList(dr, _SueetieBannedIP);
                        SueetieBannedIPList.Add(_SueetieBannedIP);
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return SueetieBannedIPList;
        }

        public override void BanIP(string ip)
        {
            using (var cn = this.GetSqlConnection())
            {
                var sql = "insert into Sueetie_BannedIPs (Mask, BannedDateTime) values ('" + ip + "','" + DateTime.Now + "')";
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override void UpdateBannedIP(int bannedID, string ip)
        {
            using (var cn = this.GetSqlConnection())
            {
                var sql = "update Sueetie_BannedIPs set Mask = '" + ip + "' where bannedID = " + bannedID;
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override void RemoveBannedIP(int bannedID)
        {
            using (var cn = this.GetSqlConnection())
            {
                var sql = "delete from Sueetie_BannedIPs where bannedID = " + bannedID;
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override void RemoveBannedIP(string ip)
        {
            using (var cn = this.GetSqlConnection())
            {
                var sql = "delete from Sueetie_BannedIPs where mask = '" + ip + "'";
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override void UpdateSueetieUserIP(SueetieUser sueetieUser)
        {
            using (var cn = this.GetSqlConnection())
            {
                //string sql = "update sueetie_users set ip = '" + sueetieUser.IP + "' where userid = " + sueetieUser.UserID;
                var sql = "update sueetie_users set ip = @ip where userid = @userid";

                var cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add("@ip", SqlDbType.VarChar).Value = sueetieUser.IP;
                cmd.Parameters.Add("@userid", SqlDbType.Int).Value = sueetieUser.UserID;
                cmd.CommandType = CommandType.Text;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override void UpdateSueetieUserProfile(Pair pair, int userID)
        {
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_User_UpdateProfile", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = userID;
                    cmd.Parameters.Add("@PropertyNames", SqlDbType.NText).Value = pair.First as string;
                    cmd.Parameters.Add("@PropertyValues", SqlDbType.NText).Value = pair.Second as string;

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }
            }
        }


        public override List<SueetieSubscriber> GetSueetieSubscriberList()
        {
            var SueetieSubscriberList = new List<SueetieSubscriber>();
            using (var cn = this.GetSqlConnection())
            {
                var sql = "select * from sueetie_vw_UserProfileData where Newsletter = 'true'";
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    SueetieSubscriber _SueetieSubscriber = null;
                    while (dr.Read())
                    {
                        _SueetieSubscriber = new SueetieSubscriber();
                        PopulateSueetieSubscriberList(dr, _SueetieSubscriber);
                        SueetieSubscriberList.Add(_SueetieSubscriber);
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return SueetieSubscriberList;
        }

        public override bool IsNewUsername(string username)
        {
            var isNewUsername = false;
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_user_IsNewUsername", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@username", SqlDbType.NVarChar, 50).Value = username;

                    cn.Open();
                    isNewUsername = bool.Parse(cmd.ExecuteScalar().ToString());
                    cn.Close();
                }
            }

            return isNewUsername;
        }


        public override bool IsNewEmailAddress(string email)
        {
            var isNewEmailAddress = false;
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_user_IsNewEmailAddress", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@email", SqlDbType.NVarChar, 255).Value = email;

                    cn.Open();
                    isNewEmailAddress = bool.Parse(cmd.ExecuteScalar().ToString());
                    cn.Close();
                }
            }

            return isNewEmailAddress;
        }

        public override List<string> GetRoles(string applicationName, string username)
        {
            var _roles = new List<string>();
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("aspnet_UsersInRoles_GetRolesForUser", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ApplicationName", SqlDbType.NVarChar, 256).Value = applicationName;
                    cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 256).Value = username;

                    cn.Open();
                    using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            _roles.Add(dr["rolename"] as string);
                        }
                        dr.Close();
                    }
                    cn.Close();
                }
                return _roles;
            }
        }

        public override List<SueetieRole> GetSueetieRoleList()
        {
            var SueetieRoleList = new List<SueetieRole>();
            using (var cn = this.GetSqlConnection())
            {
                var sql = "select * from sueetie_roles order by rolename";
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    SueetieRole _SueetieRole = null;
                    while (dr.Read())
                    {
                        _SueetieRole = new SueetieRole();
                        PopulateSueetieRoleList(dr, _SueetieRole);
                        SueetieRoleList.Add(_SueetieRole);
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return SueetieRoleList;
        }

        public override void CreateSueetieRole(SueetieRole sueetieRole)
        {
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_Role_Add", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@RoleID", SqlDbType.UniqueIdentifier, 16).Value = sueetieRole.RoleID;
                    cmd.Parameters.Add("@RoleName", SqlDbType.NVarChar, 256).Value = sueetieRole.RoleName;
                    cmd.Parameters.Add("@IsGroupAdminRole", SqlDbType.Bit, 1).Value = sueetieRole.IsGroupAdminRole;
                    cmd.Parameters.Add("@IsGroupUserRole", SqlDbType.Bit, 1).Value = sueetieRole.IsGroupUserRole;
                    cmd.Parameters.Add("@IsBlogOwnerRole", SqlDbType.Bit, 1).Value = sueetieRole.IsBlogOwnerRole;

                    cn.Open();
                    cmd.ExecuteNonQuery();

                    cn.Close();
                }
            }
        }

        public override Guid GetAspnetRoleID(string rolename)
        {
            var roleid = new Guid();
            using (var cn = this.GetSqlConnection())
            {
                //string sql = "select roleid from aspnet_roles where RoleName = '" + rolename + "'";
                var sql = "select roleid from aspnet_roles where RoleName = @rolename";
                var cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add("@rolename", SqlDbType.VarChar).Value = rolename;
                cmd.CommandType = CommandType.Text;
                cn.Open();
                roleid = new Guid(cmd.ExecuteScalar().ToString());
                cn.Close();
            }
            return roleid;
        }

        public override bool DeleteSueetieRole(string rolename)
        {
            var wasDeleted = false;
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_Role_Delete", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@RoleName", SqlDbType.NVarChar, 256).Value = rolename;

                    cn.Open();
                    wasDeleted = bool.Parse(cmd.ExecuteScalar().ToString());
                    cn.Close();
                }
            }

            return wasDeleted;
        }


        public override void UpdateSueetieRole(SueetieRole sueetieRole)
        {
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_Role_Update", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@RoleName", SqlDbType.NVarChar, 256).Value = sueetieRole.RoleName;
                    cmd.Parameters.Add("@IsGroupAdminRole", SqlDbType.Bit, 1).Value = sueetieRole.IsGroupAdminRole;
                    cmd.Parameters.Add("@IsGroupUserRole", SqlDbType.Bit, 1).Value = sueetieRole.IsGroupUserRole;
                    cmd.Parameters.Add("@IsBlogOwnerRole", SqlDbType.Bit, 1).Value = sueetieRole.IsBlogOwnerRole;

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }
            }
        }

        public override void UpdateSueetieUserAvatar(SueetieUserAvatar sueetieUserAvatar)
        {
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("sueetie_User_SaveAvatar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = sueetieUserAvatar.UserID;
                    cmd.Parameters.Add("@AvatarImage", SqlDbType.Image).Value = sueetieUserAvatar.AvatarImage;
                    cmd.Parameters.Add("@AvatarImageType", SqlDbType.NVarChar, 50).Value = sueetieUserAvatar.AvatarImageType;

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }
            }
        }

        public override void DeleteAvatar(int _userID)
        {
            using (var cn = this.GetSqlConnection())
            {
                var sql = "update sueetie_useravatar set AvatarImage = null, AvatarImageType = null WHERE UserID = " + _userID;
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override int FollowUser(SueetieFollow sueetieFollow)
        {
            var followID = 0;

            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_Follower_Add", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@FollowerUserID", SqlDbType.Int, 4).Value = sueetieFollow.FollowerUserID;
                    cmd.Parameters.Add("@FollowingUserID", SqlDbType.Int, 4).Value = sueetieFollow.FollowingUserID;
                    cmd.Parameters.Add("@ContentIDFollowed", SqlDbType.Int, 4).Value = sueetieFollow.ContentIDFollowed;
                    cmd.Parameters.Add("@FollowID", SqlDbType.Int).Direction = ParameterDirection.Output;

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    followID = (int)cmd.Parameters["@FollowID"].Value;

                    cn.Close();
                }
            }
            return followID;
        }

        public override void UnFollowUser(SueetieFollow sueetieFollow)
        {
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_Follower_Remove", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@FollowerUserID", SqlDbType.Int, 4).Value = sueetieFollow.FollowerUserID;
                    cmd.Parameters.Add("@FollowingUserID", SqlDbType.Int, 4).Value = sueetieFollow.FollowingUserID;

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }
            }
        }

        public override int CreateFavorite(UserContent userContent)
        {
            var favoriteID = 0;
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_Favorite_Add", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@ContentID", SqlDbType.Int, 4).Value = userContent.ContentID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = userContent.UserID;
                    cmd.Parameters.Add("@FavoriteID", SqlDbType.Int).Direction = ParameterDirection.Output;

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    favoriteID = (int)cmd.Parameters["@FavoriteID"].Value;

                    cn.Close();
                }
            }
            return favoriteID;
        }

        public override int GetFavoriteID(UserContent userContent)
        {
            var favoriteID = -1;

            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_Favorite_GetID", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ContentID", SqlDbType.Int, 4).Value = userContent.ContentID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = userContent.UserID;
                    cmd.Parameters.Add("@FavoriteID", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cn.Open();
                    favoriteID = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                    cn.Close();
                }
            }
            return favoriteID;
        }

        public override UserContent DeleteFavorite(int favoriteID)
        {
            var _userContent = new UserContent();

            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("Sueetie_Favorite_Delete", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@favoriteID", SqlDbType.Int, 4).Value = favoriteID;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        _userContent.UserID = (int)dr["userid"];
                        _userContent.ContentTypeID = (int)dr["contenttypeid"];
                        _userContent.GroupID = (int)dr["groupid"];
                        _userContent.IsRestricted = (bool)dr["isrestricted"];
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return _userContent;
        }

        public override UserContent DeleteFavorite(UserContent userContent)
        {
            var _userContent = new UserContent();

            //using (SqlConnection cn = GetSqlConnection())
            //{

            //    SqlCommand cmd = new SqlCommand("Sueetie_Favorite_DeleteByContent", cn);
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.Add("@favoriteID", SqlDbType.Int, 4).Value = favoriteID;

            //    cn.Open();
            //    using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            //    {
            //        while (dr.Read())
            //        {
            //            _userContent.UserID = (int)dr["userid"];
            //            _userContent.ContentTypeID = (int)dr["contenttypeid"];
            //            _userContent.GroupID = (int)dr["groupid"];
            //            _userContent.IsRestricted = (bool)dr["isrestricted"];
            //        }

            //        dr.Close();
            //        cn.Close();

            //    }
            //}
            return _userContent;
        }

        public override int GetFollowID(SueetieFollow sueetieFollow)
        {
            var followID = -1;
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_Follower_GetID", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@FollowerUserID", SqlDbType.Int, 4).Value = sueetieFollow.FollowerUserID;
                    cmd.Parameters.Add("@FollowingUserID", SqlDbType.Int, 4).Value = sueetieFollow.FollowingUserID;
                    cmd.Parameters.Add("@FollowID", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cn.Open();
                    followID = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                    cn.Close();
                }
            }
            return followID;
        }

        public override List<SueetieFollow> GetSueetieFollowList(int userID, int followTypeID)
        {
            var SueetieFollowList = new List<SueetieFollow>();

            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("Sueetie_Followers_Get", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = userID;
                cmd.Parameters.Add("@FollowType", SqlDbType.Int, 4).Value = followTypeID;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    SueetieFollow _SueetieFollow = null;
                    while (dr.Read())
                    {
                        _SueetieFollow = new SueetieFollow();
                        PopulateSueetieFollowList(dr, _SueetieFollow);
                        SueetieFollowList.Add(_SueetieFollow);
                    }
                    dr.Close();
                    cn.Close();
                }
            }
            return SueetieFollowList;
        }

        public override List<FavoriteContent> GetFavoriteContentList(int userID)
        {
            var FavoriteContents = new List<FavoriteContent>();
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand(".Sueetie_Favorites_Get", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@userId", SqlDbType.Int, 4).Value = userID;
                    cn.Open();
                    using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        FavoriteContent _FavoriteContent = null;
                        while (dr.Read())
                        {
                            _FavoriteContent = new FavoriteContent();
                            PopulateFavoriteContentList(dr, _FavoriteContent);
                            FavoriteContents.Add(_FavoriteContent);
                        }

                        dr.Close();
                        cn.Close();
                    }
                }
            }
            return FavoriteContents;
        }

        public override SueetieBlog GetSueetieBlog(int blogID, string applicationKey)
        {
            var _sueetieBlog = new SueetieBlog();

            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("Sueetie_Blog_Get", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@blogID", SqlDbType.Int, 4).Value = blogID;
                cmd.Parameters.Add("@applicationKey", SqlDbType.NVarChar, 25).Value = applicationKey;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        PopulateSueetieBlog(dr, _sueetieBlog);
                    }
                    dr.Close();
                    cn.Close();
                }
            }
            return _sueetieBlog;
        }

        public override List<SueetieBlog> GetSueetieBlogTitles()
        {
            var SueetieBlogTitles = new List<SueetieBlog>();
            using (var cn = this.GetSqlConnection())
            {
                var sql = "select blogID, blogTitle from sueetie_blogs order by blogTitle";
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    SueetieBlog _SueetieBlog = null;
                    while (dr.Read())
                    {
                        _SueetieBlog = new SueetieBlog();
                        PopulateSueetieBlogTitles(dr, _SueetieBlog);
                        SueetieBlogTitles.Add(_SueetieBlog);
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return SueetieBlogTitles;
        }

        public override void UpdateSueetieBlog(SueetieBlog sueetieBlog)
        {
            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("Sueetie_Blog_Update", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@BlogID", SqlDbType.Int, 4).Value = sueetieBlog.BlogID;
                cmd.Parameters.Add("@CategoryID", SqlDbType.Int, 4).Value = sueetieBlog.CategoryID;
                cmd.Parameters.Add("@BlogOwnerRole", SqlDbType.NVarChar, 50).Value = DataHelper.StringOrNull(sueetieBlog.BlogOwnerRole);
                cmd.Parameters.Add("@BlogAccessRole", SqlDbType.NVarChar, 50).Value = DataHelper.StringOrNull(sueetieBlog.BlogAccessRole);
                cmd.Parameters.Add("@BlogTitle", SqlDbType.NVarChar, 255).Value = sueetieBlog.BlogTitle;
                cmd.Parameters.Add("@BlogDescription", SqlDbType.NVarChar, -1).Value = DataHelper.StringOrNull(sueetieBlog.BlogDescription);
                cmd.Parameters.Add("@IncludeInAggregateList", SqlDbType.Bit, 1).Value = sueetieBlog.IncludeInAggregateList;
                cmd.Parameters.Add("@RegisteredComments", SqlDbType.Bit, 1).Value = sueetieBlog.RegisteredComments;
                cmd.Parameters.Add("@IsActive", SqlDbType.Bit, 1).Value = sueetieBlog.IsActive;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }


        public override void CreateSueetieBlog(SueetieBlog sueetieBlog)
        {
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_Blog_Add", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ApplicationID", SqlDbType.Int, 4).Value = sueetieBlog.ApplicationID;
                    cmd.Parameters.Add("@BlogTitle", SqlDbType.NVarChar, 255).Value = sueetieBlog.BlogTitle;
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }
            }
        }


        //public override void CreateSueetieBlog(SueetieBlog sueetieBlog)
        //{
        //    using (SqlConnection cn = GetSqlConnection())
        //    {
        //        string sql = "insert into Sueetie_Blogs (ApplicationID, BlogTitle, DateBlogCreated) values (" + sueetieBlog.ApplicationID + ",'" + sueetieBlog.BlogTitle + "', getdate())";
        //        SqlCommand cmd = new SqlCommand(sql, cn);
        //        cmd.CommandType = CommandType.Text;

        //        cn.Open();
        //        cmd.ExecuteNonQuery();
        //        cn.Close();

        //    }
        //}

        public override void SetMostRecentContentID(SueetieBlog sueetieBlog)
        {
            using (var cn = this.GetSqlConnection())
            {
                var sql = "update Sueetie_Blogs set MostRecentContentID = " + sueetieBlog.ContentID + " where applicationID = " + sueetieBlog.ApplicationID;
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override List<SueetieBlog> GetSueetieBlogList()
        {
            var SueetieBlogList = new List<SueetieBlog>();
            using (var cn = this.GetSqlConnection())
            {
                var sql = "select * from sueetie_vw_blogs";
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    SueetieBlog _SueetieBlog = null;
                    while (dr.Read())
                    {
                        _SueetieBlog = new SueetieBlog();
                        PopulateSueetieBlogList(dr, _SueetieBlog);
                        SueetieBlogList.Add(_SueetieBlog);
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return SueetieBlogList;
        }

        public override int CreateUpdateSueetieBlogPost(SueetieBlogPost sueetieBlogPost)
        {
            var contentID = 0;
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_bePost_CreateUpdate", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = sueetieBlogPost.UserID;
                    cmd.Parameters.Add("@ApplicationID", SqlDbType.Int, 4).Value = sueetieBlogPost.ApplicationID;
                    cmd.Parameters.Add("@ContentTypeID", SqlDbType.Int, 4).Value = sueetieBlogPost.ContentTypeID;
                    cmd.Parameters.Add("@PostID", SqlDbType.UniqueIdentifier, 16).Value = sueetieBlogPost.PostID;
                    cmd.Parameters.Add("@Title", SqlDbType.NVarChar, 255).Value = DataHelper.StringOrNull(sueetieBlogPost.Title);
                    cmd.Parameters.Add("@Description", SqlDbType.NVarChar, -1).Value = DataHelper.StringOrNull(sueetieBlogPost.Description);
                    cmd.Parameters.Add("@PostContent", SqlDbType.NText).Value = sueetieBlogPost.PostContent;
                    cmd.Parameters.Add("@DateCreated", SqlDbType.DateTime, 8).Value = sueetieBlogPost.DateCreated;
                    cmd.Parameters.Add("@DateModified", SqlDbType.DateTime, 8).Value = sueetieBlogPost.DateModified;
                    cmd.Parameters.Add("@Author", SqlDbType.NVarChar, 50).Value = DataHelper.StringOrNull(sueetieBlogPost.Author);
                    cmd.Parameters.Add("@IsPublished", SqlDbType.Bit, 1).Value = sueetieBlogPost.IsPublished;
                    cmd.Parameters.Add("@IsCommentEnabled", SqlDbType.Bit, 1).Value = sueetieBlogPost.IsCommentEnabled;
                    cmd.Parameters.Add("@Raters", SqlDbType.Int, 4).Value = sueetieBlogPost.Raters;
                    cmd.Parameters.Add("@Rating", SqlDbType.Real, 4).Value = sueetieBlogPost.Rating;
                    cmd.Parameters.Add("@Slug", SqlDbType.NVarChar, 255).Value = DataHelper.StringOrNull(sueetieBlogPost.Slug);
                    cmd.Parameters.Add("@Permalink", SqlDbType.NVarChar, 1000).Value = sueetieBlogPost.Permalink;
                    cmd.Parameters.Add("@Categories", SqlDbType.NVarChar, 2000).Value = sueetieBlogPost.Categories;
                    cmd.Parameters.Add("@Tags", SqlDbType.NVarChar, 2000).Value = sueetieBlogPost.Tags;

                    cn.Open();
                    contentID = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                    cn.Close();
                }
            }
            return contentID;
        }

        public override int CreateUpdateSueetieBlogComment(SueetieBlogComment sueetieBlogComment)
        {
            var contentID = 0;
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_beComment_CreateUpdate", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = sueetieBlogComment.UserID;
                    cmd.Parameters.Add("@ApplicationID", SqlDbType.Int, 4).Value = sueetieBlogComment.ApplicationID;
                    cmd.Parameters.Add("@ContentTypeID", SqlDbType.Int, 4).Value = sueetieBlogComment.ContentTypeID;
                    cmd.Parameters.Add("@PostCommentID", SqlDbType.UniqueIdentifier, 16).Value = sueetieBlogComment.PostCommentID;
                    cmd.Parameters.Add("@PostID", SqlDbType.UniqueIdentifier, 16).Value = sueetieBlogComment.PostID;
                    cmd.Parameters.Add("@CommentDate", SqlDbType.DateTime, 8).Value = sueetieBlogComment.CommentDate;
                    cmd.Parameters.Add("@Author", SqlDbType.NVarChar, 255).Value = DataHelper.StringOrNull(sueetieBlogComment.Author);
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 255).Value = DataHelper.StringOrNull(sueetieBlogComment.Email);
                    cmd.Parameters.Add("@Website", SqlDbType.NVarChar, 255).Value = DataHelper.StringOrNull(sueetieBlogComment.Website);
                    cmd.Parameters.Add("@Comment", SqlDbType.NVarChar, -1).Value = DataHelper.StringOrNull(sueetieBlogComment.Comment);
                    cmd.Parameters.Add("@Country", SqlDbType.NVarChar, 255).Value = DataHelper.StringOrNull(sueetieBlogComment.Country);
                    cmd.Parameters.Add("@Ip", SqlDbType.NVarChar, 50).Value = DataHelper.StringOrNull(sueetieBlogComment.Ip);
                    cmd.Parameters.Add("@IsApproved", SqlDbType.Bit, 1).Value = sueetieBlogComment.IsApproved;
                    cmd.Parameters.Add("@ParentCommentID", SqlDbType.UniqueIdentifier, 16).Value = sueetieBlogComment.ParentCommentID;
                    cmd.Parameters.Add("@Permalink", SqlDbType.NVarChar, 1000).Value = sueetieBlogComment.Permalink;

                    cn.Open();
                    contentID = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                    cn.Close();
                }
            }
            return contentID;
        }

        public override SueetieBlogPost GetSueetieBlogPost(string postGuid)
        {
            var _sueetieBlogPost = new SueetieBlogPost();

            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_BlogPost_Get", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@postGuid", SqlDbType.NVarChar, 50).Value = postGuid;
                    cmd.Parameters.Add("@contentTypeID", SqlDbType.Int, 4).Value = SueetieContentType.BlogPost;
                    cn.Open();
                    using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            PopulateSueetieBlogPost(dr, _sueetieBlogPost);
                        }
                        dr.Close();
                        cn.Close();
                    }
                }
            }
            return _sueetieBlogPost;
        }

        public override SueetieBlogComment GetSueetieBlogComment(string postGuid)
        {
            var _sueetieBlogComment = new SueetieBlogComment();

            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_BlogComment_Get", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@postGuid", SqlDbType.NVarChar, 50).Value = postGuid;
                    cmd.Parameters.Add("@contentTypeID", SqlDbType.Int, 4).Value = SueetieContentType.BlogComment;
                    cn.Open();
                    using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            PopulateSueetieBlogComment(dr, _sueetieBlogComment);
                        }
                        dr.Close();
                        cn.Close();
                    }
                }
            }
            return _sueetieBlogComment;
        }

        public override List<SueetieBlogPost> GetSueetieBlogPostList()
        {
            var SueetieBlogPostList = new List<SueetieBlogPost>();
            using (var cn = this.GetSqlConnection())
            {
                var sql = "select * from sueetie_vw_blogposts where ContentTypeID = 1";
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    SueetieBlogPost _SueetieBlogPost = null;
                    while (dr.Read())
                    {
                        _SueetieBlogPost = new SueetieBlogPost();
                        PopulateSueetieBlogPost(dr, _SueetieBlogPost);
                        SueetieBlogPostList.Add(_SueetieBlogPost);
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return SueetieBlogPostList;
        }


        public override List<SueetieBlog> GetSueetieBlogList(ApplicationQuery applicationQuery)
        {
            var SueetieBlogs = new List<SueetieBlog>();

            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("Sueetie_Blogs_Get", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@NumRecords", SqlDbType.Int, 4).Value = applicationQuery.NumRecords;
                cmd.Parameters.Add("@CategoryID", SqlDbType.Int, 4).Value = applicationQuery.CategoryID;
                cmd.Parameters.Add("@GroupID", SqlDbType.Int, 4).Value = applicationQuery.GroupID;
                cmd.Parameters.Add("@IsRestricted", SqlDbType.Bit, 1).Value = applicationQuery.IsRestricted;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    SueetieBlog _SueetieBlog = null;
                    while (dr.Read())
                    {
                        _SueetieBlog = new SueetieBlog();
                        PopulateSueetieBlog(dr, _SueetieBlog);
                        SueetieBlogs.Add(_SueetieBlog);
                    }
                    dr.Close();
                    cn.Close();
                }
            }
            return SueetieBlogs;
        }

        public override List<SueetieBlogPost> GetSueetieBlogPostList(ContentQuery contentQuery)
        {
            var SueetieBlogPosts = new List<SueetieBlogPost>();

            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("Sueetie_BlogPosts_Get", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@contentTypeID", SqlDbType.Int, 4).Value = SueetieContentType.BlogPost;
                cmd.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = contentQuery.UserID;
                cmd.Parameters.Add("@GroupID", SqlDbType.Int, 4).Value = contentQuery.GroupID;
                cmd.Parameters.Add("@ApplicationID", SqlDbType.Int, 4).Value = contentQuery.ApplicationID;
                cmd.Parameters.Add("@NumRecords", SqlDbType.Int, 4).Value = SueetieConfiguration.Get().Core.MaxListViewRecords;
                cmd.Parameters.Add("@IsRestricted", SqlDbType.Bit, 1).Value = contentQuery.IsRestricted;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    SueetieBlogPost _SueetieBlogPost = null;
                    while (dr.Read())
                    {
                        _SueetieBlogPost = new SueetieBlogPost();
                        PopulateSueetieBlogPost(dr, _SueetieBlogPost);
                        SueetieBlogPosts.Add(_SueetieBlogPost);
                    }
                    dr.Close();
                    cn.Close();
                }
            }
            return SueetieBlogPosts;
        }

        public override List<SueetieBlogComment> GetSueetieBlogCommentList(ContentQuery contentQuery)
        {
            var SueetieBlogComments = new List<SueetieBlogComment>();
            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("Sueetie_BlogComments_Get", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ContentTypeID", SqlDbType.Int, 4).Value = SueetieContentType.BlogComment;
                cmd.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = contentQuery.UserID;
                cmd.Parameters.Add("@GroupID", SqlDbType.Int, 4).Value = contentQuery.GroupID;
                cmd.Parameters.Add("@ApplicationID", SqlDbType.Int, 4).Value = contentQuery.ApplicationID;
                cmd.Parameters.Add("@NumRecords", SqlDbType.Int, 4).Value = SueetieConfiguration.Get().Core.MaxListViewRecords;
                cmd.Parameters.Add("@IsRestricted", SqlDbType.Bit, 1).Value = contentQuery.IsRestricted;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    SueetieBlogComment _SueetieBlogComment = null;
                    while (dr.Read())
                    {
                        _SueetieBlogComment = new SueetieBlogComment();
                        PopulateSueetieBlogComment(dr, _SueetieBlogComment);
                        SueetieBlogComments.Add(_SueetieBlogComment);
                    }
                    dr.Close();
                    cn.Close();
                }
            }
            return SueetieBlogComments;
        }

        public override void CreateBlogAdmin(SueetieUser sueetieUser)
        {
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_BlogAdmin_Add", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 50).Value = sueetieUser.UserName;
                    cn.Open();
                    cmd.ExecuteNonQuery();

                    cn.Close();
                }
            }
        }

        public override void SaveSueetieBlogSpam(SueetieBlogSpam sueetieBlogSpam)
        {
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_BlogSpam_Add", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@SueetiePostID", SqlDbType.Int, 4).Value = sueetieBlogSpam.SueetiePostID;
                    cmd.Parameters.Add("@IP", SqlDbType.NVarChar, 25).Value = sueetieBlogSpam.IP;
                    cmd.Parameters.Add("@SpamInfo", SqlDbType.NVarChar, -1).Value = sueetieBlogSpam.SpamInfo;

                    cn.Open();
                    cmd.ExecuteNonQuery();

                    cn.Close();
                }
            }
        }

        public override int AddForumTopic(SueetieForumContent sueetieForumContent)
        {
            var contentID = 0;
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_ForumTopic_Add", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@ContentTypeID", SqlDbType.Int, 4).Value = sueetieForumContent.ContentTypeID;
                    cmd.Parameters.Add("@ApplicationID", SqlDbType.Int, 4).Value = sueetieForumContent.ApplicationID;
                    cmd.Parameters.Add("@BoardID", SqlDbType.Int, 4).Value = sueetieForumContent.BoardID;
                    cmd.Parameters.Add("@ForumID", SqlDbType.Int, 4).Value = sueetieForumContent.ForumID;
                    cmd.Parameters.Add("@TopicID", SqlDbType.Int, 4).Value = sueetieForumContent.TopicID;
                    cmd.Parameters.Add("@YAFUserID", SqlDbType.Int, 4).Value = sueetieForumContent.YAFUserID;

                    cn.Open();
                    contentID = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                    cn.Close();
                }
            }
            return contentID;
        }

        public override int AddForumMessage(SueetieForumContent sueetieForumContent)
        {
            var contentID = 0;
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_ForumMessage_Add", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@ContentTypeID", SqlDbType.Int, 4).Value = sueetieForumContent.ContentTypeID;
                    cmd.Parameters.Add("@ApplicationID", SqlDbType.Int, 4).Value = sueetieForumContent.ApplicationID;
                    cmd.Parameters.Add("@BoardID", SqlDbType.Int, 4).Value = sueetieForumContent.BoardID;
                    cmd.Parameters.Add("@ForumID", SqlDbType.Int, 4).Value = sueetieForumContent.ForumID;
                    cmd.Parameters.Add("@MessageID", SqlDbType.Int, 4).Value = sueetieForumContent.MessageID;
                    cmd.Parameters.Add("@YAFUserID", SqlDbType.Int, 4).Value = sueetieForumContent.YAFUserID;

                    cn.Open();
                    contentID = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                    cn.Close();
                }
            }
            return contentID;
        }

        public override SueetieForumTopic GetSueetieForumTopic(SueetieForumContent sueetieForumContent)
        {
            var _sueetieForumTopic = new SueetieForumTopic();

            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("Sueetie_ForumTopic_Get", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TopicID", SqlDbType.Int, 4).Value = sueetieForumContent.TopicID;
                cmd.Parameters.Add("@ContentTypeID", SqlDbType.Int, 4).Value = sueetieForumContent.ContentTypeID;
                cmd.Parameters.Add("@ApplicationID", SqlDbType.Int, 4).Value = sueetieForumContent.ApplicationID;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        PopulateSueetieForumTopic(dr, _sueetieForumTopic);
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return _sueetieForumTopic;
        }

        public override SueetieForumMessage GetSueetieForumMessage(SueetieForumContent sueetieForumContent)
        {
            var _sueetieForumMessage = new SueetieForumMessage();

            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("Sueetie_ForumMessage_Get", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@MessageID", SqlDbType.Int, 4).Value = sueetieForumContent.MessageID;
                cmd.Parameters.Add("@ContentTypeID", SqlDbType.Int, 4).Value = sueetieForumContent.ContentTypeID;
                cmd.Parameters.Add("@ApplicationID", SqlDbType.Int, 4).Value = sueetieForumContent.ApplicationID;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        PopulateSueetieForumMessage(dr, _sueetieForumMessage);
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return _sueetieForumMessage;
        }

        public override void UpdateForumTheme(string themename)
        {
            using (var cn = this.GetSqlConnection())
            {
                var sql = "update yaf_Registry set Value = '" + themename + ".xml' where Name = 'theme'";

                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override List<SueetieForumTopic> GetSueetieForumTopicList(ContentQuery contentQuery)
        {
            var SueetieForumTopics = new List<SueetieForumTopic>();
            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("Sueetie_ForumTopics_Get", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ContentTypeID", SqlDbType.Int, 4).Value = (int)SueetieContentType.ForumTopic;
                cmd.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = contentQuery.UserID;
                cmd.Parameters.Add("@GroupID", SqlDbType.Int, 4).Value = contentQuery.GroupID;
                cmd.Parameters.Add("@NumRecords", SqlDbType.Int, 4).Value = SueetieConfiguration.Get().Core.MaxListViewRecords;
                cmd.Parameters.Add("@IsRestricted", SqlDbType.Bit, 1).Value = contentQuery.IsRestricted;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    SueetieForumTopic _SueetieForumTopic = null;
                    while (dr.Read())
                    {
                        _SueetieForumTopic = new SueetieForumTopic();
                        PopulateSueetieForumTopic(dr, _SueetieForumTopic);
                        SueetieForumTopics.Add(_SueetieForumTopic);
                    }
                    dr.Close();
                    cn.Close();
                }
            }
            return SueetieForumTopics;
        }

        public override List<SueetieForumMessage> GetSueetieForumMessageList(ContentQuery contentQuery)
        {
            var SueetieForumMessages = new List<SueetieForumMessage>();
            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("Sueetie_ForumMessages_Get", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ContentTypeID", SqlDbType.Int, 4).Value = (int)SueetieContentType.ForumMessage;
                cmd.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = contentQuery.UserID;
                cmd.Parameters.Add("@NumRecords", SqlDbType.Int, 4).Value = SueetieConfiguration.Get().Core.MaxListViewRecords;
                cmd.Parameters.Add("@GroupID", SqlDbType.Int, 4).Value = contentQuery.GroupID;
                cmd.Parameters.Add("@IsRestricted", SqlDbType.Bit, 1).Value = contentQuery.IsRestricted;
                cmd.Parameters.Add("@ApplicationID", SqlDbType.Int, 4).Value = contentQuery.ApplicationID;


                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    SueetieForumMessage _SueetieForumMessage = null;
                    while (dr.Read())
                    {
                        _SueetieForumMessage = new SueetieForumMessage();
                        PopulateSueetieForumMessage(dr, _SueetieForumMessage);
                        SueetieForumMessages.Add(_SueetieForumMessage);
                    }
                    dr.Close();
                    cn.Close();
                }
            }
            return SueetieForumMessages;
        }

        public override void CompleteForumSetup()
        {
            using (var cn = this.GetSqlConnection())
            {
                var sql = "insert into yaf_registry ([name], value, boardid) values ('theme','lollipop.xml',1)";
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override int GetForumUserID(int boardID, string username)
        {
            var forumUserID = -1;

            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_User_GetForumID", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@boardID", SqlDbType.Int, 4).Value = boardID;
                    cmd.Parameters.Add("@username", SqlDbType.NVarChar, 25).Value = username;
                    cmd.Parameters.Add("@UserForumID", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cn.Open();
                    forumUserID = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                    cn.Close();
                }
            }
            return forumUserID;
        }

        public override int CreateForumUser(SueetieUser sueetieUser)
        {
            var userForumID = -1;

            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("yaf_sueetie_user_aspnet", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@BoardID", SqlDbType.Int, 4).Value = 1;
                    cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 50).Value = sueetieUser.UserName;
                    cmd.Parameters.Add("@DisplayName", SqlDbType.NVarChar, 50).Value = sueetieUser.DisplayName;
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 255).Value = sueetieUser.Email;
                    cmd.Parameters.Add("@ProviderUserKey", SqlDbType.NVarChar, 64).Value = sueetieUser.MembershipID.ToString();
                    cmd.Parameters.Add("@IsApproved", SqlDbType.Bit).Value = 1;
                    cmd.Parameters.Add("@TimeZone", SqlDbType.Int, 4).Value = sueetieUser.TimeZone;

                    cn.Open();
                    userForumID = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                    cn.Close();
                }
            }
            return userForumID;
        }

        public override void EnterForumTopicTags(SueetieTagEntry sueetieTagEntry)
        {
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_ForumTopicTags_CreateUpdate", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@ContentID", SqlDbType.Int, 4).Value = sueetieTagEntry.ContentID;
                    cmd.Parameters.Add("@ItemID", SqlDbType.Int, 4).Value = sueetieTagEntry.ItemID;
                    cmd.Parameters.Add("@ContentTypeID", SqlDbType.Int, 4).Value = sueetieTagEntry.ContentTypeID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = sueetieTagEntry.UserID;
                    cmd.Parameters.Add("@Tags", SqlDbType.NVarChar, 500).Value = DataHelper.StringOrNull(sueetieTagEntry.Tags);

                    cn.Open();
                    cmd.ExecuteNonQuery();

                    cn.Close();
                }
            }
        }

        public override List<SueetieMediaGallery> GetSueetieMediaGalleryList()
        {
            var SueetieMediaGalleries = new List<SueetieMediaGallery>();
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_MediaGalleries_Get", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cn.Open();
                    using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        SueetieMediaGallery _SueetieMediaGallery = null;
                        while (dr.Read())
                        {
                            _SueetieMediaGallery = new SueetieMediaGallery();
                            PopulateSueetieMediaGalleryList(dr, _SueetieMediaGallery);
                            SueetieMediaGalleries.Add(_SueetieMediaGallery);
                        }

                        dr.Close();
                        cn.Close();
                    }
                }
            }
            return SueetieMediaGalleries;
        }

        public override void AdminUpdateSueetieMediaGallery(SueetieMediaGallery sueetieMediaGallery)
        {
            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("Sueetie_MediaGallery_AdminUpdate", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@GalleryID", SqlDbType.Int, 4).Value = sueetieMediaGallery.GalleryID;
                cmd.Parameters.Add("@GalleryKey", SqlDbType.NVarChar, 60).Value = sueetieMediaGallery.GalleryKey.ToLower();
                cmd.Parameters.Add("@DisplayTypeID", SqlDbType.Int, 4).Value = sueetieMediaGallery.DisplayTypeID;
                cmd.Parameters.Add("@IsPublic", SqlDbType.Bit, 1).Value = sueetieMediaGallery.IsPublic;
                cmd.Parameters.Add("@IsLogged", SqlDbType.Bit, 1).Value = sueetieMediaGallery.IsLogged;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override void CreateMediaGallery(int _galleryID)
        {
            using (var cn = this.GetSqlConnection())
            {
                var _applicationID = SueetieApplications.Current.ApplicationID;
                var sql = "insert into Sueetie_gs_Gallery (galleryid, applicationid) values (" + _galleryID + "," + _applicationID + ")";
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override SueetieMediaObject GetSueetieMediaPhoto(int mediaObjectID)
        {
            var _sueetieMediaObject = new SueetieMediaObject();

            using (var cn = this.GetSqlConnection())
            {
                //string sql = "select * from sueetie_vw_mediaobjects where ContentTypeID = 6 and SourceID = " + mediaObjectID;
                var sql = "select * from sueetie_vw_mediaobjects where ContentTypeID = 6 and SourceID = @mediaObjectID";
                var cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add("@mediaObjectID", SqlDbType.Int).Value = mediaObjectID;
                cmd.CommandType = CommandType.Text;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        PopulateSueetieMediaObjectList(dr, _sueetieMediaObject);
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return _sueetieMediaObject;
        }


        public override List<SueetieMediaAlbum> GetSueetieAlbumUpdateList(int galleryID)
        {
            var sueetieMediaAlbumList = new List<SueetieMediaAlbum>();
            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("Sueetie_AlbumUpdates_Get", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@GalleryID", SqlDbType.Int, 4).Value = galleryID;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    SueetieMediaAlbum _SueetieMediaAlbum = null;
                    while (dr.Read())
                    {
                        _SueetieMediaAlbum = new SueetieMediaAlbum();
                        _SueetieMediaAlbum.AlbumID = (int)dr["albumid"];
                        _SueetieMediaAlbum.SueetieUserID = (int)dr["sueetieuserid"];
                        _SueetieMediaAlbum.IsRestricted = (bool)dr["isprivate"];
                        sueetieMediaAlbumList.Add(_SueetieMediaAlbum);
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return sueetieMediaAlbumList;
        }

        public override List<SueetieMediaObject> GetSueetieMediaUpdateList()
        {
            return this.GetSueetieMediaUpdateList(-1);
        }

        public override List<SueetieMediaObject> GetSueetieMediaUpdateList(int albumID)
        {
            var SueetieMediaObjectList = new List<SueetieMediaObject>();

            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("Sueetie_MediaUpdates_Get", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@AlbumID", SqlDbType.Int, 4).Value = albumID;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    SueetieMediaObject _SueetieMediaObject = null;
                    while (dr.Read())
                    {
                        _SueetieMediaObject = new SueetieMediaObject();
                        _SueetieMediaObject.MediaObjectID = (int)dr["mediaobjectid"];
                        _SueetieMediaObject.AlbumID = (int)dr["albumid"];
                        _SueetieMediaObject.SueetieUserID = (int)dr["sueetieuserid"];
                        SueetieMediaObjectList.Add(_SueetieMediaObject);
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return SueetieMediaObjectList;
        }

        public override List<SueetieMediaObject> GetSueetieMediaObjectList(ContentQuery contentQuery)
        {
            var SueetieMediaObjects = new List<SueetieMediaObject>();
            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("Sueetie_MediaObjects_Get", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ContentViewTypeID", SqlDbType.Int, 4).Value = contentQuery.SueetieContentViewTypeID;
                cmd.Parameters.Add("@ContentTypeID", SqlDbType.Int, 4).Value = contentQuery.ContentTypeID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = contentQuery.UserID;
                cmd.Parameters.Add("@GroupID", SqlDbType.Int, 4).Value = contentQuery.GroupID;
                cmd.Parameters.Add("@ApplicationID", SqlDbType.Int, 4).Value = contentQuery.ApplicationID;
                cmd.Parameters.Add("@NumRecords", SqlDbType.Int, 4).Value = SueetieConfiguration.Get().Core.MaxListViewRecords;
                cmd.Parameters.Add("@IsRestricted", SqlDbType.Bit, 1).Value = contentQuery.IsRestricted;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    SueetieMediaObject _SueetieMediaObject = null;
                    while (dr.Read())
                    {
                        _SueetieMediaObject = new SueetieMediaObject();
                        PopulateSueetieMediaObjectList(dr, _SueetieMediaObject);
                        SueetieMediaObjects.Add(_SueetieMediaObject);
                    }
                    dr.Close();
                    cn.Close();
                }
            }
            return SueetieMediaObjects;
        }

        public override List<SueetieMediaObject> GetSueetieMediaObjectList(int galleryID)
        {
            var SueetieMediaObjectList = new List<SueetieMediaObject>();
            using (var cn = this.GetSqlConnection())
            {
                //string sql = "SELECT  * from Sueetie_vw_MediaObjects where " +
                //    "ContentTypeID between 6 and 10 and galleryID = " + galleryID;
                var sql = "SELECT  * from Sueetie_vw_MediaObjects where ContentTypeID between 6 and 10 and galleryID = @galleryID";
                var cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add("@galleryID", SqlDbType.Int).Value = galleryID;
                cmd.CommandType = CommandType.Text;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    SueetieMediaObject _SueetieMediaObject = null;
                    while (dr.Read())
                    {
                        _SueetieMediaObject = new SueetieMediaObject();
                        PopulateSueetieMediaObjectList(dr, _SueetieMediaObject);
                        SueetieMediaObjectList.Add(_SueetieMediaObject);
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return SueetieMediaObjectList;
        }

        public override void RecordDownload(SueetieDownload sueetieDownload)
        {
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_Downloads_Record", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@MediaObjectID", SqlDbType.Int, 4).Value = sueetieDownload.MediaObjectID;
                    cmd.Parameters.Add("@ApplicationID", SqlDbType.Int, 4).Value = sueetieDownload.ApplicationID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = sueetieDownload.DownloadUserID;

                    cn.Open();
                    cmd.ExecuteNonQuery();

                    cn.Close();
                }
            }
        }

        public override List<ContentTypeDescription> GetAlbumContentTypeDescriptionList()
        {
            var ContentTypeDescriptionList = new List<ContentTypeDescription>();
            using (var cn = this.GetSqlConnection())
            {
                var sql = "select * from sueetie_contenttypes where isalbum = 1";
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    ContentTypeDescription _ContentTypeDescription = null;
                    while (dr.Read())
                    {
                        _ContentTypeDescription = new ContentTypeDescription();
                        PopulateContentTypeDescriptionList(dr, _ContentTypeDescription);
                        ContentTypeDescriptionList.Add(_ContentTypeDescription);
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return ContentTypeDescriptionList;
        }

        public override SueetieMediaAlbum GetSueetieMediaAlbum(int albumID)
        {
            var _sueetieMediaAlbum = new SueetieMediaAlbum();

            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("Sueetie_MediaAlbum_Get", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@AlbumID", SqlDbType.Int, 4).Value = albumID;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        PopulateSueetieMediaAlbumList(dr, _sueetieMediaAlbum);
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return _sueetieMediaAlbum;
        }

        public override void CreateSueetieAlbum(int albumID, string albumPath, int contentTypeID)
        {
            using (var cn = this.GetSqlConnection())
            {
                var sql = "insert into sueetie_gs_album (albumid, sueetiealbumpath, contenttypeid) values (" + albumID + ",'" + albumPath + "'," + contentTypeID + ")";

                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override void CreateSueetieMediaObject(SueetieMediaObject sueetieMediaObject)
        {
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_MediaObject_Add", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@MediaObjectID", SqlDbType.Int, 4).Value = sueetieMediaObject.MediaObjectID;
                    cmd.Parameters.Add("@MediaObjectDescription", SqlDbType.NVarChar, -1).Value = DataHelper.StringOrNull(sueetieMediaObject.MediaObjectDescription);
                    cmd.Parameters.Add("@InDownloadReport", SqlDbType.Bit, 1).Value = sueetieMediaObject.InDownloadReport;

                    cn.Open();
                    cmd.ExecuteNonQuery();

                    cn.Close();
                }
            }
        }

        public override int GetAlbumContentTypeID(int albumid)
        {
            var contentTypeID = -1;
            using (var cn = this.GetSqlConnection())
            {
                //string sql = "select ContentTypeID from Sueetie_gs_album where albumID = " + albumid;
                var sql = "select ContentTypeID from Sueetie_gs_album where albumID = @albumid";
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@albumid", SqlDbType.Int).Value = albumid;
                cn.Open();
                contentTypeID = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                cn.Close();
            }
            return contentTypeID;
        }

        public override void UpdateAlbumContentTypeID(SueetieMediaObject sueetieMediaObject)
        {
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_MediaAlbumType_Update", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@AlbumID", SqlDbType.Int, 4).Value = sueetieMediaObject.AlbumID;
                    cmd.Parameters.Add("@ContentTypeID", SqlDbType.Int, 4).Value = sueetieMediaObject.ContentTypeID;
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }
            }
        }

        public override List<SueetieMediaAlbum> GetSueetieMediaAlbumList(ContentQuery contentQuery)
        {
            var SueetieMediaAlbums = new List<SueetieMediaAlbum>();
            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("Sueetie_MediaAlbums_Get", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ContentViewTypeID", SqlDbType.Int, 4).Value = contentQuery.SueetieContentViewTypeID;
                cmd.Parameters.Add("@ContentTypeID", SqlDbType.Int, 4).Value = contentQuery.ContentTypeID;
                cmd.Parameters.Add("@GroupID", SqlDbType.Int, 4).Value = contentQuery.GroupID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = contentQuery.UserID;
                cmd.Parameters.Add("@ApplicationID", SqlDbType.Int, 4).Value = contentQuery.ApplicationID;
                cmd.Parameters.Add("@NumRecords", SqlDbType.Int, 4).Value = contentQuery.NumRecords;
                cmd.Parameters.Add("@IsRestricted", SqlDbType.Bit, 1).Value = contentQuery.IsRestricted;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    SueetieMediaAlbum _SueetieMediaAlbum = null;
                    while (dr.Read())
                    {
                        _SueetieMediaAlbum = new SueetieMediaAlbum();
                        PopulateSueetieMediaAlbumList(dr, _SueetieMediaAlbum);
                        SueetieMediaAlbums.Add(_SueetieMediaAlbum);
                    }
                    dr.Close();
                    cn.Close();
                }
            }
            return SueetieMediaAlbums;
        }

        public override List<SueetieMediaAlbum> GetSueetieMediaAlbumList(int galleryID)
        {
            var SueetieMediaAlbumList = new List<SueetieMediaAlbum>();
            using (var cn = this.GetSqlConnection())
            {
                //string sql = "SELECT  * from Sueetie_vw_MediaAlbums where ContentTypeID between 11 and 17 " +
                //        "and albumID > 1 and galleryID = " + galleryID;
                var sql = "SELECT  * from Sueetie_vw_MediaAlbums where ContentTypeID between 11 and 17 and albumID > 1 and galleryID = @galleryID";
                var cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add("@galleryID", SqlDbType.Int).Value = galleryID;
                cmd.CommandType = CommandType.Text;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    SueetieMediaAlbum _SueetieMediaAlbum = null;
                    while (dr.Read())
                    {
                        _SueetieMediaAlbum = new SueetieMediaAlbum();
                        PopulateSueetieMediaAlbumList(dr, _SueetieMediaAlbum);
                        SueetieMediaAlbumList.Add(_SueetieMediaAlbum);
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return SueetieMediaAlbumList;
        }


        public override void AdminUpdateSueetieMediaAlbum(SueetieMediaAlbum sueetieMediaAlbum)
        {
            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("Sueetie_MediaAlbum_AdminUpdate", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@AlbumID", SqlDbType.Int, 4).Value = sueetieMediaAlbum.AlbumID;
                cmd.Parameters.Add("@ContentID", SqlDbType.Int, 4).Value = sueetieMediaAlbum.ContentID;
                cmd.Parameters.Add("@ContentTypeID", SqlDbType.Int, 4).Value = sueetieMediaAlbum.ContentTypeID;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override void UpdateSueetieMediaObject(SueetieMediaObject sueetieMediaObject)
        {
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_MediaObject_Update", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@MediaObjectID", SqlDbType.Int, 4).Value = sueetieMediaObject.MediaObjectID;
                    cmd.Parameters.Add("@MediaObjectDescription", SqlDbType.NVarChar, -1).Value = DataHelper.StringOrNull(sueetieMediaObject.MediaObjectDescription);
                    cmd.Parameters.Add("@Abstract", SqlDbType.NVarChar, -1).Value = DataHelper.StringOrNull(sueetieMediaObject.Abstract);
                    cmd.Parameters.Add("@Authors", SqlDbType.NVarChar, 255).Value = DataHelper.StringOrNull(sueetieMediaObject.Authors);
                    cmd.Parameters.Add("@Location", SqlDbType.NVarChar, 255).Value = DataHelper.StringOrNull(sueetieMediaObject.Location);
                    cmd.Parameters.Add("@Series", SqlDbType.NVarChar, 50).Value = DataHelper.StringOrNull(sueetieMediaObject.Series);
                    cmd.Parameters.Add("@DocumentType", SqlDbType.NVarChar, 50).Value = DataHelper.StringOrNull(sueetieMediaObject.DocumentType);
                    cmd.Parameters.Add("@Keywords", SqlDbType.NVarChar, 500).Value = DataHelper.StringOrNull(sueetieMediaObject.Keywords);
                    cmd.Parameters.Add("@Misc", SqlDbType.NVarChar, 255).Value = DataHelper.StringOrNull(sueetieMediaObject.Misc);
                    cmd.Parameters.Add("@Number", SqlDbType.NVarChar, 50).Value = DataHelper.StringOrNull(sueetieMediaObject.Number);
                    cmd.Parameters.Add("@Version", SqlDbType.NVarChar, 25).Value = DataHelper.StringOrNull(sueetieMediaObject.Version);
                    cmd.Parameters.Add("@Organization", SqlDbType.NVarChar, 255).Value = DataHelper.StringOrNull(sueetieMediaObject.Organization);
                    cmd.Parameters.Add("@Conference", SqlDbType.NVarChar, 100).Value = DataHelper.StringOrNull(sueetieMediaObject.Conference);
                    cmd.Parameters.Add("@ISxN", SqlDbType.NVarChar, 25).Value = DataHelper.StringOrNull(sueetieMediaObject.ISxN);
                    cmd.Parameters.Add("@PublicationDate", SqlDbType.NVarChar, 50).Value = DataHelper.StringOrNull(sueetieMediaObject.PublicationDate);
                    cmd.Parameters.Add("@Publisher", SqlDbType.NVarChar, 100).Value = DataHelper.StringOrNull(sueetieMediaObject.Publisher);


                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }
            }
        }

        public override void UpdateSueetieMediaAlbum(SueetieMediaAlbum sueetieMediaAlbum)
        {
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_MediaAlbum_Update", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@AlbumID", SqlDbType.Int, 4).Value = sueetieMediaAlbum.AlbumID;
                    cmd.Parameters.Add("@AlbumTitle", SqlDbType.NVarChar, 200).Value = sueetieMediaAlbum.AlbumTitle;
                    cmd.Parameters.Add("@AlbumDescription", SqlDbType.NVarChar, -1).Value = DataHelper.StringOrNull(sueetieMediaAlbum.AlbumDescription);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }
            }
        }

        public override List<SueetieDownload> GetSueetieDownloadList(ContentQuery contentQuery)
        {
            var SueetieDownloads = new List<SueetieDownload>();
            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("Sueetie_Downloads_Get", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ContentViewTypeID", SqlDbType.Int, 4).Value = contentQuery.SueetieContentViewTypeID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = contentQuery.UserID;
                cmd.Parameters.Add("@ApplicationID", SqlDbType.Int, 4).Value = contentQuery.ApplicationID;
                cmd.Parameters.Add("@SourceID", SqlDbType.Int, 4).Value = contentQuery.SourceID;
                cmd.Parameters.Add("@GroupID", SqlDbType.Int, 4).Value = contentQuery.GroupID;
                cmd.Parameters.Add("@NumRecords", SqlDbType.Int, 4).Value = contentQuery.NumRecords;
                cmd.Parameters.Add("@SortBy", SqlDbType.Int, 4).Value = contentQuery.SortBy;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    SueetieDownload _SueetieDownload = null;
                    while (dr.Read())
                    {
                        _SueetieDownload = new SueetieDownload();
                        PopulateSueetieDownloadList(dr, _SueetieDownload);
                        SueetieDownloads.Add(_SueetieDownload);
                    }
                    dr.Close();
                    cn.Close();
                }
            }
            return SueetieDownloads;
        }

        public override bool IsIncludedInDownload(int mediaobjectid)
        {
            var isIncludedInDownload = false;
            using (var cn = this.GetSqlConnection())
            {
                var sql = "select InDownloadReport from Sueetie_gs_MediaObject where MediaObjectID = " + mediaobjectid;
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;
                cn.Open();
                isIncludedInDownload = bool.Parse(cmd.ExecuteScalar().ToString());
                cn.Close();
            }
            return isIncludedInDownload;
        }

        public override void SetIncludedInDownload(int mediaobjectid, bool includedInDownload)
        {
            using (var cn = this.GetSqlConnection())
            {
                var sql = "update Sueetie_gs_mediaObject set InDownloadReport = " + DataHelper.BoolToBit(includedInDownload) +
                          " where MediaObjectID = " + mediaobjectid;

                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override void EnterMediaObjectTags(SueetieTagEntry sueetieTagEntry)
        {
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_MediaObjectTags_CreateUpdate", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@ContentID", SqlDbType.Int, 4).Value = sueetieTagEntry.ContentID;
                    cmd.Parameters.Add("@ItemID", SqlDbType.Int, 4).Value = sueetieTagEntry.ItemID;
                    cmd.Parameters.Add("@ContentTypeID", SqlDbType.Int, 4).Value = sueetieTagEntry.ContentTypeID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = sueetieTagEntry.UserID;
                    cmd.Parameters.Add("@Tags", SqlDbType.NVarChar, 500).Value = DataHelper.StringOrNull(sueetieTagEntry.Tags);


                    cn.Open();
                    cmd.ExecuteNonQuery();

                    cn.Close();
                }
            }
        }

        public override void EnterMediaAlbumTags(SueetieTagEntry sueetieTagEntry)
        {
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_MediaAlbumTags_CreateUpdate", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@ContentID", SqlDbType.Int, 4).Value = sueetieTagEntry.ContentID;
                    cmd.Parameters.Add("@ItemID", SqlDbType.Int, 4).Value = sueetieTagEntry.ItemID;
                    cmd.Parameters.Add("@ContentTypeID", SqlDbType.Int, 4).Value = sueetieTagEntry.ContentTypeID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = sueetieTagEntry.UserID;
                    cmd.Parameters.Add("@Tags", SqlDbType.NVarChar, 500).Value = DataHelper.StringOrNull(sueetieTagEntry.Tags);


                    cn.Open();
                    cmd.ExecuteNonQuery();

                    cn.Close();
                }
            }
        }

        public override void PopulateMediaObjectTitles()
        {
            using (var cn = this.GetSqlConnection())
            {
                var sql = "update gs_mediaobject set title = originalFilename where len(rtrim(Title)) = 0";
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override List<SueetieMediaDirectory> GetSueetieMediaDirectoryList()
        {
            var SueetieMediaDirectoryList = new List<SueetieMediaDirectory>();
            using (var cn = this.GetSqlConnection())
            {
                var sql = "select * from sueetie_vw_MediaDirectories where contentTypeID = 6";
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    SueetieMediaDirectory _SueetieMediaDirectory = null;
                    while (dr.Read())
                    {
                        _SueetieMediaDirectory = new SueetieMediaDirectory();
                        PopulateSueetieMediaDirectoryList(dr, _SueetieMediaDirectory);
                        SueetieMediaDirectoryList.Add(_SueetieMediaDirectory);
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return SueetieMediaDirectoryList;
        }

        public override void UpdateSueetieAlbumPath(SueetieMediaAlbum sueetieMediaAlbum)
        {
            using (var cn = this.GetSqlConnection())
            {
                var sql = "update Sueetie_gs_Album set SueetieAlbumPath = '" +
                          sueetieMediaAlbum.SueetieAlbumPath + "' where albumID = " + sueetieMediaAlbum.AlbumID;
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override SueetieWikiPage GetSueetieWikiPage(SueetieWikiPage sueetieWikiPage)
        {
            var _sueetieWikiPage = new SueetieWikiPage();
            using (var cn = this.GetSqlConnection())
            {
                string sql;
                if (sueetieWikiPage.Namespace == null)
                {
                    sql = "select * from Sueetie_vw_WikiPages where pageFileName = @pageFileName and namespace is null and ApplicationId = @applicationId and ContentTypeID = @ContentTypeId";
                }
                else
                {
                    sql = "select * from Sueetie_vw_WikiPages where pageFileName = @pageFileName and Namespace = @namespace and ApplicationId = @applicationId and ContentTypeID = @ContentTypeId";
                }

                //string sql = "select * from Sueetie_vw_WikiPages where pageFileName = '" +
                //    sueetieWikiPage.PageFileName + "' and " + ns + " and ApplicationID = " + sueetieWikiPage.ApplicationID +
                //    " and ContentTypeID = " + (int)SueetieContentType.WikiPage;
                var cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add("@pageFileName", SqlDbType.VarChar).Value = sueetieWikiPage.PageFileName;
                cmd.Parameters.Add("@namespace", SqlDbType.VarChar).Value = (object)sueetieWikiPage.Namespace ?? DBNull.Value;
                cmd.Parameters.Add("@applicationId", SqlDbType.Int).Value = sueetieWikiPage.ApplicationID;
                cmd.Parameters.Add("@ContentTypeId", SqlDbType.Int).Value = SueetieContentType.WikiPage;
                cmd.CommandType = CommandType.Text;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        PopulateSueetieWikiPageList(dr, _sueetieWikiPage);
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return _sueetieWikiPage;
        }

        public override int CreateSueetieWikiPage(SueetieWikiPage sueetieWikiPage)
        {
            var pageID = 0;

            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_WikiPage_Add", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@PageFileName", SqlDbType.NVarChar, 200).Value = sueetieWikiPage.PageFileName;
                    cmd.Parameters.Add("@PageTitle", SqlDbType.NVarChar, 500).Value = sueetieWikiPage.PageTitle;
                    cmd.Parameters.Add("@Keywords", SqlDbType.NVarChar, 1000).Value = sueetieWikiPage.Keywords;
                    cmd.Parameters.Add("@Abstract", SqlDbType.NVarChar, -1).Value = DataHelper.StringOrNull(sueetieWikiPage.Abstract);
                    cmd.Parameters.Add("@Namespace", SqlDbType.NVarChar, 200).Value = DataHelper.StringOrNull(sueetieWikiPage.Namespace);
                    cmd.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = sueetieWikiPage.UserID;
                    cmd.Parameters.Add("@ApplicationID", SqlDbType.Int, 4).Value = sueetieWikiPage.ApplicationID;
                    cmd.Parameters.Add("@Categories", SqlDbType.NVarChar, 2000).Value = sueetieWikiPage.Categories;
                    cmd.Parameters.Add("@PageContent", SqlDbType.NText).Value = sueetieWikiPage.PageContent;
                    cmd.Parameters.Add("@PageID", SqlDbType.Int).Direction = ParameterDirection.Output;

                    cn.Open();
                    cmd.ExecuteNonQuery();

                    pageID = (int)cmd.Parameters["@PageID"].Value;
                    cn.Close();
                }
            }
            return pageID;
        }

        public override void UpdateSueetieWikiPage(SueetieWikiPage sueetieWikiPage)
        {
            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("Sueetie_WikiPage_update", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@PageID", SqlDbType.Int, 4).Value = sueetieWikiPage.PageID;
                cmd.Parameters.Add("@PageTitle", SqlDbType.NVarChar, 500).Value = sueetieWikiPage.PageTitle;
                cmd.Parameters.Add("@Keywords", SqlDbType.NVarChar, 1000).Value = sueetieWikiPage.Keywords;
                cmd.Parameters.Add("@Abstract", SqlDbType.NVarChar, -1).Value = DataHelper.StringOrNull(sueetieWikiPage.Abstract);
                cmd.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = sueetieWikiPage.UserID;
                cmd.Parameters.Add("@Categories", SqlDbType.NVarChar, 2000).Value = sueetieWikiPage.Categories;
                cmd.Parameters.Add("@PageContent", SqlDbType.NText).Value = sueetieWikiPage.PageContent;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override void EnterWikiPageTags(SueetieTagEntry sueetieTagEntry)
        {
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_WikiPageTags_CreateUpdate", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@ContentID", SqlDbType.Int, 4).Value = sueetieTagEntry.ContentID;
                    cmd.Parameters.Add("@ItemID", SqlDbType.Int, 4).Value = sueetieTagEntry.ItemID;
                    cmd.Parameters.Add("@ContentTypeID", SqlDbType.Int, 4).Value = sueetieTagEntry.ContentTypeID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = sueetieTagEntry.UserID;
                    cmd.Parameters.Add("@Tags", SqlDbType.NVarChar, 500).Value = DataHelper.StringOrNull(sueetieTagEntry.Tags);

                    cn.Open();
                    cmd.ExecuteNonQuery();

                    cn.Close();
                }
            }
        }

        public override int CreateWikiMessage(SueetieWikiMessage sueetieWikiMessage)
        {
            var messageID = 0;

            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_WikiMessage_Add", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PageID", SqlDbType.Int, 4).Value = sueetieWikiMessage.PageID;
                    cmd.Parameters.Add("@MessageQueryID", SqlDbType.NVarChar, 50).Value = sueetieWikiMessage.MessageQueryID;
                    cmd.Parameters.Add("@Subject", SqlDbType.NVarChar, 500).Value = sueetieWikiMessage.Subject;
                    cmd.Parameters.Add("@Message", SqlDbType.NText).Value = sueetieWikiMessage.Message;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = sueetieWikiMessage.UserID;

                    cmd.Parameters.Add("@MessageID", SqlDbType.Int).Direction = ParameterDirection.Output;

                    cn.Open();
                    cmd.ExecuteNonQuery();

                    messageID = (int)cmd.Parameters["@MessageID"].Value;
                    cn.Close();
                }
            }
            return messageID;
        }

        public override List<SueetieWikiMessage> GetSueetieWikiMessageList()
        {
            var SueetieWikiMessages = new List<SueetieWikiMessage>();
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_WikiMessages_Get", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@contentTypeID", SqlDbType.Int, 4).Value = (int)SueetieContentType.WikiMessage;

                    cn.Open();
                    using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        SueetieWikiMessage _SueetieWikiMessage = null;
                        while (dr.Read())
                        {
                            _SueetieWikiMessage = new SueetieWikiMessage();
                            PopulateSueetieWikiMessageList(dr, _SueetieWikiMessage);
                            SueetieWikiMessages.Add(_SueetieWikiMessage);
                        }

                        dr.Close();
                        cn.Close();
                    }
                }
            }
            return SueetieWikiMessages;
        }

        public override SueetieWikiMessage GetSueetieWikiMessage(string messageQueryID)
        {
            var _sueetieWikiMessage = new SueetieWikiMessage();

            using (var cn = this.GetSqlConnection())
            {
                //string sql = "select * from sueetie_vw_WikiMessages where contentTypeID = " +
                //    (int)SueetieContentType.WikiMessage + " and messageQueryID = '" + messageQueryID + "'";
                var sql = "select * from sueetie_vw_WikiMessages where contentTypeID = @contentTypeID and messageQueryID = @messageQueryID";
                var cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add("@contentTypeID", SqlDbType.Int).Value = SueetieContentType.WikiMessage;
                cmd.Parameters.Add("@messageQueryID", SqlDbType.Int).Value = messageQueryID;
                cmd.CommandType = CommandType.Text;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        PopulateSueetieWikiMessageList(dr, _sueetieWikiMessage);
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return _sueetieWikiMessage;
        }

        public override void UpdateSueetieWikiMessage(SueetieWikiMessage sueetieWikiMessage)
        {
            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("Sueetie_WikiMessage_Update", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@MessageID", SqlDbType.Int, 4).Value = sueetieWikiMessage.MessageID;
                cmd.Parameters.Add("@MessageQueryID", SqlDbType.NVarChar, 50).Value = DataHelper.StringOrNull(sueetieWikiMessage.MessageQueryID);
                cmd.Parameters.Add("@Subject", SqlDbType.NVarChar, 500).Value = DataHelper.StringOrNull(sueetieWikiMessage.Subject);
                cmd.Parameters.Add("@Message", SqlDbType.NText).Value = sueetieWikiMessage.Message;
                cmd.Parameters.Add("@IsActive", SqlDbType.Bit, 1).Value = sueetieWikiMessage.IsActive;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override List<SueetieWikiPage> GetSueetieWikiPageList(ContentQuery contentQuery)
        {
            var SueetieWikiPages = new List<SueetieWikiPage>();
            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("Sueetie_WikiPages_Get", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@NumRecords", SqlDbType.Int, 4).Value = contentQuery.NumRecords;
                cmd.Parameters.Add("@ContentTypeID", SqlDbType.Int, 4).Value = contentQuery.ContentTypeID;
                cmd.Parameters.Add("@GroupID", SqlDbType.Int, 4).Value = contentQuery.GroupID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = contentQuery.UserID;
                cmd.Parameters.Add("@IsRestricted", SqlDbType.Bit, 1).Value = contentQuery.IsRestricted;
                cmd.Parameters.Add("@ApplicationID", SqlDbType.Int, 4).Value = contentQuery.ApplicationID;


                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    SueetieWikiPage _SueetieWikiPage = null;
                    while (dr.Read())
                    {
                        _SueetieWikiPage = new SueetieWikiPage();
                        PopulateSueetieWikiPageList(dr, _SueetieWikiPage);
                        SueetieWikiPages.Add(_SueetieWikiPage);
                    }
                    dr.Close();
                    cn.Close();
                }
            }
            return SueetieWikiPages;
        }

        public override int AddPhoto(ClassifiedsPhoto classifiedsPhoto)
        {
            var photoId = 0;

            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("cfds_InsertPhoto", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@AdId", SqlDbType.Int, 4).Value = classifiedsPhoto.AdId;
                    cmd.Parameters.Add("@IsMainPreview", SqlDbType.Bit, 1).Value = classifiedsPhoto.IsMainPreview;
                    cmd.Parameters.Add("@PhotoId", SqlDbType.Int).Direction = ParameterDirection.Output;

                    cn.Open();
                    cmd.ExecuteNonQuery();

                    photoId = (int)cmd.Parameters["@PhotoId"].Value;
                    cn.Close();
                }
            }
            return photoId;
        }

        public override bool HasClassifiedsCategories()
        {
            var hasCategories = false;
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_cfds_HasCategories", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cn.Open();
                    hasCategories = bool.Parse(cmd.ExecuteScalar().ToString());
                    cn.Close();
                }
            }
            return hasCategories;
        }

        public override int AddSueetieContent(SueetieContent sueetieContent)
        {
            var contentID = 0;

            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_Content_Add", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@SourceID", SqlDbType.Int, 4).Value = sueetieContent.SourceID;
                    cmd.Parameters.Add("@ContentTypeID", SqlDbType.Int, 4).Value = sueetieContent.ContentTypeID;
                    cmd.Parameters.Add("@ApplicationID", SqlDbType.Int, 4).Value = sueetieContent.ApplicationID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = sueetieContent.UserID;
                    cmd.Parameters.Add("@Permalink", SqlDbType.NVarChar, 500).Value = DataHelper.StringOrNull(sueetieContent.Permalink);
                    cmd.Parameters.Add("@IsRestricted", SqlDbType.Bit, 1).Value = sueetieContent.IsRestricted;

                    cmd.Parameters.Add("@ContentID", SqlDbType.Int).Direction = ParameterDirection.Output;


                    cn.Open();
                    cmd.ExecuteNonQuery();

                    contentID = (int)cmd.Parameters["@ContentID"].Value;
                    cn.Close();
                }
            }
            return contentID;
        }

        public override void UpdateSueetieContentPermalink(SueetieContent sueetieContent)
        {
            using (var cn = this.GetSqlConnection())
            {
                //string sql = "update sueetie_content set permalink = '" + sueetieContent.Permalink + "' where contentID = " + sueetieContent.ContentID;
                var sql = "update sueetie_content set permalink = @permalink where contentID = @contentID";
                var cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add("@permalink", SqlDbType.VarChar).Value = sueetieContent.Permalink;
                cmd.Parameters.Add("@contentID", SqlDbType.Int).Value = sueetieContent.ContentID;
                cmd.CommandType = CommandType.Text;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override void UpdateSueetieContentIsRestricted(int contentID, bool isRestricted)
        {
            using (var cn = this.GetSqlConnection())
            {
                //string sql = "update sueetie_content set isrestricted = " + DataHelper.BoolToBit(isRestricted) +
                //    " where contentID = " + contentID;
                var sql = "update sueetie_content set isrestricted = @isRestricted where contentID = @contactID";

                var cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add("@isRestricted", SqlDbType.Bit).Value = isRestricted;
                cmd.Parameters.Add("@contactID", SqlDbType.Int).Value = contentID;
                cmd.CommandType = CommandType.Text;
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override List<SueetieApplication> GetSueetieApplicationList()
        {
            var SueetieApplications = new List<SueetieApplication>();
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_Applications_Get", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cn.Open();
                    using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        SueetieApplication _SueetieApplication = null;
                        while (dr.Read())
                        {
                            _SueetieApplication = new SueetieApplication();
                            PopulateSueetieApplicationList(dr, _SueetieApplication);
                            SueetieApplications.Add(_SueetieApplication);
                        }

                        dr.Close();
                        cn.Close();
                    }
                }
            }
            return SueetieApplications;
        }

        public override List<SueetieGroup> GetSueetieGroupList()
        {
            var SueetieGroupList = new List<SueetieGroup>();
            using (var cn = this.GetSqlConnection())
            {
                var sql = "select * from sueetie_groups order by groupid";
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    SueetieGroup _SueetieGroup = null;
                    while (dr.Read())
                    {
                        _SueetieGroup = new SueetieGroup();
                        PopulateSueetieGroupList(dr, _SueetieGroup);
                        SueetieGroupList.Add(_SueetieGroup);
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return SueetieGroupList;
        }

        public override SueetieGroup GetSueetieGroup(int groupID)
        {
            var _sueetieGroup = new SueetieGroup();

            using (var cn = this.GetSqlConnection())
            {
                //string sql = "select * from sueetie_groups where groupid = " + groupID;
                var sql = "select * from sueetie_groups where groupid = @groupID";
                var cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add("@groupID", SqlDbType.Int).Value = groupID;
                cmd.CommandType = CommandType.Text;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        _sueetieGroup.GroupID = (int)dr["groupid"];
                        _sueetieGroup.GroupKey = dr["groupkey"] as string;
                        _sueetieGroup.GroupName = dr["groupname"] as string;
                        _sueetieGroup.GroupAdminRole = dr["groupadminrole"] as string;
                        _sueetieGroup.GroupUserRole = dr["groupuserrole"] as string;
                        _sueetieGroup.GroupDescription = dr["groupdescription"] as string;
                        _sueetieGroup.GroupTypeID = (int)dr["grouptypeid"];
                        _sueetieGroup.IsActive = (bool)dr["isactive"];
                        _sueetieGroup.HasAvatar = (bool)dr["hasavatar"];
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return _sueetieGroup;
        }

        public override int CreateGroup(SueetieGroup sueetieGroup)
        {
            var groupID = 0;

            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_Group_Add", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@GroupKey", SqlDbType.NVarChar, 25).Value = DataHelper.StringOrNull(sueetieGroup.GroupKey);
                    cmd.Parameters.Add("@GroupName", SqlDbType.NVarChar, 255).Value = sueetieGroup.GroupName;
                    cmd.Parameters.Add("@GroupAdminRole", SqlDbType.NVarChar, 50).Value = DataHelper.StringOrNull(sueetieGroup.GroupAdminRole);
                    cmd.Parameters.Add("@GroupUserRole", SqlDbType.NVarChar, 50).Value = DataHelper.StringOrNull(sueetieGroup.GroupUserRole);
                    cmd.Parameters.Add("@GroupDescription", SqlDbType.NVarChar, 1000).Value = DataHelper.StringOrNull(sueetieGroup.GroupDescription);
                    cmd.Parameters.Add("@GroupTypeID", SqlDbType.Int, 4).Value = sueetieGroup.GroupTypeID;
                    cmd.Parameters.Add("@IsActive", SqlDbType.Bit, 1).Value = sueetieGroup.IsActive;
                    cmd.Parameters.Add("@HasAvatar", SqlDbType.Bit, 1).Value = sueetieGroup.HasAvatar;

                    cmd.Parameters.Add("@GroupID", SqlDbType.Int).Direction = ParameterDirection.Output;


                    cn.Open();
                    cmd.ExecuteNonQuery();

                    groupID = (int)cmd.Parameters["@GroupID"].Value;
                    cn.Close();
                }
            }
            return groupID;
        }

        public override void UpdateSueetieGroup(SueetieGroup sueetieGroup)
        {
            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("Sueetie_Group_Update", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@GroupID", SqlDbType.Int, 4).Value = sueetieGroup.GroupID;
                cmd.Parameters.Add("@GroupKey", SqlDbType.NVarChar, 25).Value = DataHelper.StringOrNull(sueetieGroup.GroupKey);
                cmd.Parameters.Add("@GroupName", SqlDbType.NVarChar, 255).Value = sueetieGroup.GroupName;
                cmd.Parameters.Add("@GroupAdminRole", SqlDbType.NVarChar, 50).Value = DataHelper.StringOrNull(sueetieGroup.GroupAdminRole);
                cmd.Parameters.Add("@GroupUserRole", SqlDbType.NVarChar, 50).Value = DataHelper.StringOrNull(sueetieGroup.GroupUserRole);
                cmd.Parameters.Add("@GroupDescription", SqlDbType.NVarChar, 1000).Value = DataHelper.StringOrNull(sueetieGroup.GroupDescription);
                cmd.Parameters.Add("@GroupTypeID", SqlDbType.Int, 4).Value = sueetieGroup.GroupTypeID;
                cmd.Parameters.Add("@IsActive", SqlDbType.Bit, 1).Value = sueetieGroup.IsActive;
                cmd.Parameters.Add("@HasAvatar", SqlDbType.Bit, 1).Value = sueetieGroup.HasAvatar;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override void UpdateSueetieApplication(SueetieApplication sueetieApplication)
        {
            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("Sueetie_Application_Update", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ApplicationID", SqlDbType.Int, 4).Value = sueetieApplication.ApplicationID;
                cmd.Parameters.Add("@ApplicationTypeID", SqlDbType.Int, 4).Value = sueetieApplication.ApplicationTypeID;
                cmd.Parameters.Add("@ApplicationKey", SqlDbType.NVarChar, 25).Value = sueetieApplication.ApplicationKey;
                cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 1000).Value = sueetieApplication.Description;
                cmd.Parameters.Add("@GroupID", SqlDbType.Int, 4).Value = sueetieApplication.GroupID;
                cmd.Parameters.Add("@IsActive", SqlDbType.Bit, 1).Value = sueetieApplication.IsActive;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override int CreateSueetieApplication(SueetieApplication sueetieApplication)
        {
            var applicationID = -1;
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_Application_Add", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ApplicationID", SqlDbType.Int, 4).Value = sueetieApplication.ApplicationID;
                    cmd.Parameters.Add("@ApplicationTypeID", SqlDbType.Int, 4).Value = sueetieApplication.ApplicationTypeID;
                    cmd.Parameters.Add("@ApplicationKey", SqlDbType.NVarChar, 25).Value = sueetieApplication.ApplicationKey;
                    cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 1000).Value = sueetieApplication.Description;
                    cmd.Parameters.Add("@GroupID", SqlDbType.Int, 4).Value = sueetieApplication.GroupID;

                    cn.Open();
                    applicationID = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                    cn.Close();
                }
            }
            return applicationID;
        }

        public override void DeleteSueetieApplication(int ApplicationID)
        {
            using (var cn = this.GetSqlConnection())
            {
                var sql = "delete from sueetie_applications where applicationID = " + ApplicationID + " and islocked = 0";

                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override SueetieContentPart GetSueetieContentPart(string contentName)
        {
            var content = new SueetieContentPart();

            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_ContentPart_Get", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ContentName", SqlDbType.Char).Value = contentName;
                    cn.Open();
                    using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            PopulateSueetieContentPartList(dr, content);
                        }
                    }
                }
            }
            return content;
        }

        public override int UpdateSueetieContentPart(SueetieContentPart sueetieContentPart)
        {
            var contentID = -1;
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_ContentPart_Update", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ContentName", SqlDbType.NVarChar, 256).Value = sueetieContentPart.ContentName;
                    cmd.Parameters.Add("@ContentPageID", SqlDbType.Int, 4).Value = sueetieContentPart.ContentPageID;
                    cmd.Parameters.Add("@ContentPageGroupID", SqlDbType.Int, 4).Value = sueetieContentPart.ContentPageGroupID;
                    cmd.Parameters.Add("@Permalink", SqlDbType.NVarChar, 255).Value = sueetieContentPart.Permalink ?? "na";
                    cmd.Parameters.Add("@LastUpdateUserID", SqlDbType.Int, 4).Value = sueetieContentPart.LastUpdateUserID;
                    cmd.Parameters.Add("@ContentText", SqlDbType.NText).Value = sueetieContentPart.ContentText;
                    cmd.Parameters.Add("@ApplicationID", SqlDbType.Int, 4).Value = sueetieContentPart.ApplicationID;
                    cmd.Parameters.Add("@ContentID", SqlDbType.Int).Direction = ParameterDirection.Output;

                    cn.Open();
                    cmd.ExecuteNonQuery();

                    contentID = (int)cmd.Parameters["@ContentID"].Value;
                    cn.Close();
                }
            }
            return contentID;
        }

        public override List<SueetieContentPart> GetSueetieContentPartList(int pageID)
        {
            var SueetieContentParts = new List<SueetieContentPart>();
            using (var cn = this.GetSqlConnection())
            {
                //string sql = "select * from sueetie_vw_contentparts where contentPageID = " + pageID;
                var sql = "select * from sueetie_vw_contentparts where contentPageID = @pageID";
                var cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add("@pageID", SqlDbType.Int).Value = pageID;
                cmd.CommandType = CommandType.Text;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    SueetieContentPart _SueetieContentPart = null;
                    while (dr.Read())
                    {
                        _SueetieContentPart = new SueetieContentPart();
                        PopulateSueetieContentPartList(dr, _SueetieContentPart);
                        SueetieContentParts.Add(_SueetieContentPart);
                    }
                    dr.Close();
                    cn.Close();
                }
            }
            return SueetieContentParts;
        }

        public override SueetieContentPage GetSueetieContentPage(int pageID)
        {
            var _sueetieContentPage = new SueetieContentPage();

            using (var cn = this.GetSqlConnection())
            {
                //string sql = "select * from sueetie_vw_contentpages where contentPageID = " + pageID +
                //    " and contentTypeID = " + (int)SueetieContentType.CMSPage;
                var sql = "select * from sueetie_vw_contentpages where contentPageID = @pageID  and contentTypeID = @contentTypeID";

                var cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add("@pageId", SqlDbType.Int).Value = pageID;
                cmd.Parameters.Add("@contentTypeID", SqlDbType.Int).Value = SueetieContentType.CMSPage;
                cmd.CommandType = CommandType.Text;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        PopulateSueetieContentPageList(dr, _sueetieContentPage);
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return _sueetieContentPage;
        }

        public override List<SueetieContentPage> GetSueetieContentPageList(int contentGroupID)
        {
            //string sql = "select * from sueetie_vw_contentpages where contentTypeID = " + _contentTypeID + " and contentPageID > 0";
            //if (contentGroupID > 0)
            //    sql = "select * from sueetie_vw_contentpages where contentTypeID = " + _contentTypeID + " and contentPageGroupID = " + contentGroupID +
            //        " order by PageTitle";
            var sql = "select * from sueetie_vw_contentpages where contentTypeID = @contentTypeID and ((@contentGroupID = 0 and contentPageID > 0) or contentPageGroupID = @contentGroupID) order by PageTitle";
            var SueetieContentPageList = new List<SueetieContentPage>();
            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add("@contentTypeID", SqlDbType.Int).Value = SueetieContentType.CMSPage;
                cmd.Parameters.Add("@contentGroupID", SqlDbType.Int).Value = contentGroupID;
                cmd.CommandType = CommandType.Text;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    SueetieContentPage _SueetieContentPage = null;
                    while (dr.Read())
                    {
                        _SueetieContentPage = new SueetieContentPage();
                        PopulateSueetieContentPageList(dr, _SueetieContentPage);
                        SueetieContentPageList.Add(_SueetieContentPage);
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return SueetieContentPageList;
        }

        public override List<SueetieContentPageGroup> GetSueetieContentPageGroupList()
        {
            var SueetieContentPageGroupList = new List<SueetieContentPageGroup>();
            using (var cn = this.GetSqlConnection())
            {
                var sql = "select * from Sueetie_vw_ContentPageGroups where contentpagegroupID > 0 order by ContentPageGroupTitle";
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    SueetieContentPageGroup _SueetieContentPageGroup = null;
                    while (dr.Read())
                    {
                        _SueetieContentPageGroup = new SueetieContentPageGroup();
                        PopulateSueetieContentPageGroupList(dr, _SueetieContentPageGroup);
                        SueetieContentPageGroupList.Add(_SueetieContentPageGroup);
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return SueetieContentPageGroupList;
        }

        public override SueetieContentPageGroup GetSueetieContentPageGroup(int contentPageGroupID)
        {
            var _sueetieContentPageGroup = new SueetieContentPageGroup();

            using (var cn = this.GetSqlConnection())
            {
                //string sql = "select * from sueetie_vw_contentpagegroups where contentpagegroupID = " + contentPageGroupID;
                var sql = "select * from sueetie_vw_contentpagegroups where contentpagegroupID = @contentPageGroupID";
                var cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add("@contentPageGroupID", SqlDbType.Int).Value = contentPageGroupID;
                cmd.CommandType = CommandType.Text;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        PopulateSueetieContentPageGroupList(dr, _sueetieContentPageGroup);
                    }
                    dr.Close();
                    cn.Close();
                }
            }
            return _sueetieContentPageGroup;
        }

        public override void CreateContentPageGroup(SueetieContentPageGroup sueetieContentPageGroup)
        {
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_ContentPageGroup_Add", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@ApplicationID", SqlDbType.Int, 4).Value = sueetieContentPageGroup.ApplicationID;
                    cmd.Parameters.Add("@ContentPageGroupTitle", SqlDbType.NVarChar, 255).Value = sueetieContentPageGroup.ContentPageGroupTitle;
                    cmd.Parameters.Add("@EditorRoles", SqlDbType.NVarChar, 255).Value = DataHelper.StringOrNull(sueetieContentPageGroup.EditorRoles);
                    cmd.Parameters.Add("@IsActive", SqlDbType.Bit, 1).Value = sueetieContentPageGroup.IsActive;

                    cn.Open();
                    cmd.ExecuteNonQuery();

                    cn.Close();
                }
            }
        }

        public override void UpdateSueetieContentPageGroup(SueetieContentPageGroup sueetieContentPageGroup)
        {
            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("Sueetie_ContentPageGroup_Update", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ContentPageGroupID", SqlDbType.Int, 4).Value = sueetieContentPageGroup.ContentPageGroupID;
                cmd.Parameters.Add("@ContentPageGroupTitle", SqlDbType.NVarChar, 255).Value = sueetieContentPageGroup.ContentPageGroupTitle;
                cmd.Parameters.Add("@EditorRoles", SqlDbType.NVarChar, 255).Value = DataHelper.StringOrNull(sueetieContentPageGroup.EditorRoles);
                cmd.Parameters.Add("@IsActive", SqlDbType.Bit, 1).Value = sueetieContentPageGroup.IsActive;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override int CreateContentPage(SueetieContentPage sueetieContentPage)
        {
            var contentPageID = -1;
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_ContentPage_Add", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@ContentPageGroupID", SqlDbType.Int, 4).Value = sueetieContentPage.ContentPageGroupID;
                    cmd.Parameters.Add("@PageKey", SqlDbType.NVarChar, 50).Value = DataHelper.StringOrNull(sueetieContentPage.PageKey);
                    cmd.Parameters.Add("@PageSlug", SqlDbType.NVarChar, 255).Value = DataHelper.StringOrNull(sueetieContentPage.PageSlug);
                    cmd.Parameters.Add("@PageTitle", SqlDbType.NVarChar, 255).Value = DataHelper.StringOrNull(sueetieContentPage.PageTitle);
                    cmd.Parameters.Add("@PageDescription", SqlDbType.NVarChar, 255).Value = DataHelper.StringOrNull(sueetieContentPage.PageDescription);
                    cmd.Parameters.Add("@ReaderRoles", SqlDbType.NVarChar, 255).Value = DataHelper.StringOrNull(sueetieContentPage.ReaderRoles);
                    cmd.Parameters.Add("@LastUpdateUserID", SqlDbType.Int, 4).Value = sueetieContentPage.LastUpdateUserID;
                    cmd.Parameters.Add("@IsPublished", SqlDbType.Bit, 1).Value = sueetieContentPage.IsPublished;
                    cmd.Parameters.Add("@DisplayOrder", SqlDbType.Int, 4).Value = sueetieContentPage.DisplayOrder;
                    cmd.Parameters.Add("@ContentPageID", SqlDbType.Int).Direction = ParameterDirection.Output;

                    cn.Open();
                    cmd.ExecuteNonQuery();

                    contentPageID = (int)cmd.Parameters["@ContentPageID"].Value;
                    cn.Close();
                }
            }
            return contentPageID;
        }

        public override void UpdateSueetieContentPage(SueetieContentPage sueetieContentPage)
        {
            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("Sueetie_ContentPage_Update", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ContentPageID", SqlDbType.Int, 4).Value = sueetieContentPage.ContentPageID;
                cmd.Parameters.Add("@PageKey", SqlDbType.NVarChar, 50).Value = DataHelper.StringOrNull(sueetieContentPage.PageKey);
                cmd.Parameters.Add("@PageSlug", SqlDbType.NVarChar, 255).Value = DataHelper.StringOrNull(sueetieContentPage.PageSlug);
                cmd.Parameters.Add("@PageTitle", SqlDbType.NVarChar, 255).Value = DataHelper.StringOrNull(sueetieContentPage.PageTitle);
                cmd.Parameters.Add("@PageDescription", SqlDbType.NVarChar, 255).Value = DataHelper.StringOrNull(sueetieContentPage.PageDescription);
                cmd.Parameters.Add("@ReaderRoles", SqlDbType.NVarChar, 255).Value = DataHelper.StringOrNull(sueetieContentPage.ReaderRoles);
                cmd.Parameters.Add("@LastUpdateUserID", SqlDbType.Int, 4).Value = sueetieContentPage.LastUpdateUserID;
                cmd.Parameters.Add("@IsPublished", SqlDbType.Bit, 1).Value = sueetieContentPage.IsPublished;
                cmd.Parameters.Add("@DisplayOrder", SqlDbType.Int, 4).Value = sueetieContentPage.DisplayOrder;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override List<SueetieApplication> GetCMSApplicationList()
        {
            var SueetieApplicationList = new List<SueetieApplication>();
            using (var cn = this.GetSqlConnection())
            {
                var sql = "select *, isgroup = convert(bit,0) from sueetie_vw_applications where applicationtypeID = 7 " +
                          "and applicationid not in (select applicationID from sueetie_contentpagegroups)";
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    SueetieApplication _SueetieApplication = null;
                    while (dr.Read())
                    {
                        _SueetieApplication = new SueetieApplication();
                        PopulateSueetieApplicationList(dr, _SueetieApplication);
                        SueetieApplicationList.Add(_SueetieApplication);
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return SueetieApplicationList;
        }

        public override void UpdateCMSPermalink(SueetieContentPage sueetieContentPage)
        {
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_ContentPageUrls_Update", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@ContentPageID", SqlDbType.Int, 4).Value = sueetieContentPage.ContentPageID;
                    cmd.Parameters.Add("@Permalink", SqlDbType.NVarChar, 500).Value = sueetieContentPage.Permalink;

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }
            }
        }

        public override void DeleteContentPage(int _contentpageID)
        {
            using (var cn = this.GetSqlConnection())
            {
                var sql = "delete from sueetie_contentpages where contentpageID = " + _contentpageID;
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override void EnterContentPageTags(SueetieTagEntry sueetieTagEntry)
        {
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_ContentPageTags_CreateUpdate", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@ContentID", SqlDbType.Int, 4).Value = sueetieTagEntry.ContentID;
                    cmd.Parameters.Add("@ItemID", SqlDbType.Int, 4).Value = sueetieTagEntry.ItemID;
                    cmd.Parameters.Add("@ContentTypeID", SqlDbType.Int, 4).Value = sueetieTagEntry.ContentTypeID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = sueetieTagEntry.UserID;
                    cmd.Parameters.Add("@Tags", SqlDbType.NVarChar, 500).Value = DataHelper.StringOrNull(sueetieTagEntry.Tags);

                    cn.Open();
                    cmd.ExecuteNonQuery();

                    cn.Close();
                }
            }
        }

        public override bool IsSiteInstalled()
        {
            var isSiteInstalled = false;
            var cnt = 0;
            using (var cn = this.GetSqlConnection())
            {
                var sql = "select count(*) as 'cnt' from sueetie_users";

                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;
                try
                {
                    cn.Open();
                    cnt = int.Parse(cmd.ExecuteScalar().ToString());
                    if (cnt > 0)
                        isSiteInstalled = true;
                }
                catch (Exception)
                {
                    isSiteInstalled = false;
                }

                cn.Close();
            }

            return isSiteInstalled;
        }

        public override void CreateSiteLogEntry(SiteLogEntry siteLogEntry)
        {
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_SiteLog_Add", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@SiteLogTypeID", SqlDbType.Int, 4).Value = siteLogEntry.SiteLogTypeID;
                    cmd.Parameters.Add("@SiteLogCategoryID", SqlDbType.Int, 4).Value = siteLogEntry.SiteLogCategoryID;
                    cmd.Parameters.Add("@ApplicationID", SqlDbType.Int, 4).Value = siteLogEntry.ApplicationID;
                    cmd.Parameters.Add("@Message", SqlDbType.NText).Value = siteLogEntry.Message;

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }
            }
        }

        public override List<UserLogCategory> GetUserLogCategoryList()
        {
            var UserLogCategoryList = new List<UserLogCategory>();
            using (var cn = this.GetSqlConnection())
            {
                var sql = "select * from sueetie_userlogcategories order by userlogcategoryID";
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    UserLogCategory _UserLogCategory = null;
                    while (dr.Read())
                    {
                        _UserLogCategory = new UserLogCategory();
                        PopulateUserLogCategoryList(dr, _UserLogCategory);
                        UserLogCategoryList.Add(_UserLogCategory);
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return UserLogCategoryList;
        }

        public override void UpdateUserLogCategory(UserLogCategory userLogCategory)
        {
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_UserLogCategory_Update", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@UserLogCategoryID", SqlDbType.Int, 4).Value = userLogCategory.UserLogCategoryID;
                    cmd.Parameters.Add("@IsDisplayed", SqlDbType.Bit, 1).Value = userLogCategory.IsDisplayed;
                    cmd.Parameters.Add("@IsSyndicated", SqlDbType.Bit, 1).Value = userLogCategory.IsSyndicated;
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }
            }
        }

        public override void CreateUserLogCategory(UserLogCategory userLogCategory)
        {
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_UserLogCategory_Add", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@UserLogCategoryID", SqlDbType.Int, 4).Value = userLogCategory.UserLogCategoryID;
                    cmd.Parameters.Add("@UserLogCategoryCode", SqlDbType.NVarChar, 25).Value = DataHelper.StringOrNull(userLogCategory.UserLogCategoryCode);
                    cmd.Parameters.Add("@UserLogCategoryDescription", SqlDbType.NVarChar, 255).Value = DataHelper.StringOrNull(userLogCategory.UserLogCategoryDescription);
                    cmd.Parameters.Add("@IsDisplayed", SqlDbType.Bit, 1).Value = userLogCategory.IsDisplayed;
                    cmd.Parameters.Add("@IsSyndicated", SqlDbType.Bit, 1).Value = userLogCategory.IsSyndicated;

                    cn.Open();
                    cmd.ExecuteNonQuery();

                    cn.Close();
                }
            }
        }

        public override void DeleteUserLogCategory(int _userlogcategoryID)
        {
            using (var cn = this.GetSqlConnection())
            {
                var sql = "delete from Sueetie_UserLogCategories where userLogCategoryID = " + _userlogcategoryID + " and isLocked = 0";
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override void CreateUserLogEntry(UserLogEntry userLogEntry)
        {
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_UserLog_Add", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@UserLogCategoryID", SqlDbType.Int, 4).Value = userLogEntry.UserLogCategoryID;
                    cmd.Parameters.Add("@ItemID", SqlDbType.Int, 4).Value = userLogEntry.ItemID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = userLogEntry.UserID;

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }
            }
        }

        public override List<UserLogActivity> GetUserLogActivityList(ContentQuery contentQuery)
        {
            var _userLogActivity = new List<UserLogActivity>();
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_UserLogActivities_Get", cn))
                {
                    var IsSyndicatedList = false;
                    if (contentQuery.SueetieContentViewTypeID == (int)SueetieContentViewType.SyndicatedUserLogActivityList)
                        IsSyndicatedList = true;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@groupID", SqlDbType.Int, 4).Value = contentQuery.GroupID;
                    cmd.Parameters.Add("@syndicatedList", SqlDbType.Bit, 1).Value = IsSyndicatedList;
                    cmd.Parameters.Add("@showAll", SqlDbType.Bit, 1).Value = false;
                    cn.Open();
                    using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        UserLogActivity _UserLogActivity = null;
                        while (dr.Read())
                        {
                            _UserLogActivity = new UserLogActivity();
                            PopulateUserLogActivityList(dr, _UserLogActivity);
                            _userLogActivity.Add(_UserLogActivity);
                        }

                        dr.Close();
                        cn.Close();
                    }
                }
            }
            return _userLogActivity;
        }

        public override List<UserLogActivity> GetUserLogActivityList(bool showAll)
        {
            var _userLogActivity = new List<UserLogActivity>();
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_UserLogActivities_Get", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@groupID", SqlDbType.Int, 4).Value = 0;
                    cmd.Parameters.Add("@syndicatedList", SqlDbType.Bit, 1).Value = 0;
                    cmd.Parameters.Add("@showAll", SqlDbType.Bit, 1).Value = showAll;
                    cn.Open();
                    using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        UserLogActivity _UserLogActivity = null;
                        while (dr.Read())
                        {
                            _UserLogActivity = new UserLogActivity();
                            PopulateUserLogActivityList(dr, _UserLogActivity);
                            _userLogActivity.Add(_UserLogActivity);
                        }

                        dr.Close();
                        cn.Close();
                    }
                }
            }
            return _userLogActivity;
        }

        public override void DeleteUserLogActivity(int _userlogID)
        {
            using (var cn = this.GetSqlConnection())
            {
                //string sql = "delete from Sueetie_UserLog where userLogID = " + _userlogID;
                var sql = "delete from Sueetie_UserLog where userLogID = @userLogID";
                var cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add("@userLogID", SqlDbType.Int).Value = _userlogID;
                cmd.CommandType = CommandType.Text;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override List<SiteLogEntry> GetSiteLogEntryList(SiteLogEntry siteLogEntry)
        {
            var SiteLogEntrys = new List<SiteLogEntry>();
            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("Sueetie_SiteLog_Get", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@SiteLogTypeID", SqlDbType.Int, 4).Value = siteLogEntry.SiteLogTypeID;
                cmd.Parameters.Add("@SiteLogCategoryID", SqlDbType.Int, 4).Value = siteLogEntry.SiteLogCategoryID;
                cmd.Parameters.Add("@ApplicationID", SqlDbType.Int, 4).Value = siteLogEntry.ApplicationID;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    SiteLogEntry _SiteLogEntry = null;
                    while (dr.Read())
                    {
                        _SiteLogEntry = new SiteLogEntry();
                        PopulateSiteLogEntryList(dr, _SiteLogEntry);
                        SiteLogEntrys.Add(_SiteLogEntry);
                    }
                    dr.Close();
                    cn.Close();
                }
            }
            return SiteLogEntrys;
        }

        public override List<SiteLogEntry> GetSiteLogTypeList()
        {
            var SiteLogEntryList = new List<SiteLogEntry>();
            using (var cn = this.GetSqlConnection())
            {
                var sql = "select * from Sueetie_SiteLogTypes order by sitelogtypecode";
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    SiteLogEntry _SiteLogEntry = null;
                    while (dr.Read())
                    {
                        _SiteLogEntry = new SiteLogEntry();
                        PopulateSiteLogTypeList(dr, _SiteLogEntry);
                        SiteLogEntryList.Add(_SiteLogEntry);
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return SiteLogEntryList;
        }

        public override int GetEventLogCount()
        {
            var eventLogCount = -1;
            using (var cn = this.GetSqlConnection())
            {
                var sql = "select count(*) from sueetie_sitelog";
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;
                cn.Open();
                eventLogCount = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                cn.Close();
            }

            return eventLogCount;
        }

        public override void ClearEventLog()
        {
            using (var cn = this.GetSqlConnection())
            {
                var sql = "truncate table sueetie_sitelog";
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override void TestTaskEntry()
        {
            using (var cn = this.GetSqlConnection())
            {
                var sql = "insert into z_Sueetie_SqlRecordTask (DateTimeRun) values (GETDATE())";
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override List<SueetieTag> GetSueetieTagList()
        {
            var SueetieTagList = new List<SueetieTag>();
            using (var cn = this.GetSqlConnection())
            {
                var sql = "select TagMasterID, Tag, 0 as 'TagCount' from Sueetie_TagMaster where isActive = 1 order by tag";
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    SueetieTag _SueetieTag = null;
                    while (dr.Read())
                    {
                        _SueetieTag = new SueetieTag();
                        PopulateSueetieTagList(dr, _SueetieTag);
                        SueetieTagList.Add(_SueetieTag);
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return SueetieTagList;
        }

        public override List<SueetieTag> GetSueetieTagCloudList(SueetieTagQuery sueetieTagQuery)
        {
            var SueetieTags = new List<SueetieTag>();
            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("Sueetie_CloudTags_Get", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ApplicationTypeID", SqlDbType.Int, 4).Value = sueetieTagQuery.ApplicationTypeID;
                cmd.Parameters.Add("@ApplicationID", SqlDbType.Int, 4).Value = sueetieTagQuery.ApplicationID;
                cmd.Parameters.Add("@IsRestricted", SqlDbType.Bit, 1).Value = sueetieTagQuery.IsRestricted;
                cmd.Parameters.Add("@CloudTagNum", SqlDbType.Int, 4).Value = sueetieTagQuery.CloudTagNum;


                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    SueetieTag _SueetieTag = null;
                    while (dr.Read())
                    {
                        _SueetieTag = new SueetieTag();
                        PopulateSueetieTagList(dr, _SueetieTag);
                        SueetieTags.Add(_SueetieTag);
                    }
                    dr.Close();
                    cn.Close();
                }
            }
            return SueetieTags;
        }

        public override List<SueetieTagMaster> GetSueetieTagMasterList()
        {
            var SueetieTagMasterList = new List<SueetieTagMaster>();
            using (var cn = this.GetSqlConnection())
            {
                var sql = "SELECT t.TagID, t.TagMasterID, t.ContentID, t.Tag, t.CreatedBy, t.CreatedDateTime, t.IsActive, " +
                          "c.ContentTypeID, c.ApplicationID FROM Sueetie_Content c INNER JOIN Sueetie_Tags t ON c.ContentID = t.ContentID " +
                          "where t.IsActive = 1";
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    SueetieTagMaster _SueetieTagMaster = null;
                    while (dr.Read())
                    {
                        _SueetieTagMaster = new SueetieTagMaster();
                        PopulateSueetieTagMasterList(dr, _SueetieTagMaster);
                        SueetieTagMasterList.Add(_SueetieTagMaster);
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return SueetieTagMasterList;
        }

        public override List<SueetieCalendarEvent> GetSueetieCalendarEventList(int calendarID)
        {
            var SueetieCalendarEventList = new List<SueetieCalendarEvent>();
            using (var cn = this.GetSqlConnection())
            {
                //string sql = "select * from sueetie_vw_calendarevents where contentTypeID = 22 and calendarID = " + calendarID;
                var sql = "select * from sueetie_vw_calendarevents where contentTypeID = 22 and calendarID = @calendarID";
                var cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add("@calendarID", SqlDbType.Int).Value = calendarID;
                cmd.CommandType = CommandType.Text;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    SueetieCalendarEvent _SueetieCalendarEvent = null;
                    while (dr.Read())
                    {
                        _SueetieCalendarEvent = new SueetieCalendarEvent();
                        PopulateSueetieCalendarEventList(dr, _SueetieCalendarEvent);
                        SueetieCalendarEventList.Add(_SueetieCalendarEvent);
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return SueetieCalendarEventList;
        }

        public override List<SueetieCalendar> GetSueetieCalendarList()
        {
            var SueetieCalendarList = new List<SueetieCalendar>();
            using (var cn = this.GetSqlConnection())
            {
                var sql = "select * from sueetie_calendars order by calendarTitle";
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    SueetieCalendar _SueetieCalendar = null;
                    while (dr.Read())
                    {
                        _SueetieCalendar = new SueetieCalendar();
                        PopulateSueetieCalendarList(dr, _SueetieCalendar);
                        SueetieCalendarList.Add(_SueetieCalendar);
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return SueetieCalendarList;
        }

        public override void UpdateSueetieCalendar(SueetieCalendar sueetieCalendar)
        {
            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("Sueetie_Calendar_Update", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CalendarID", SqlDbType.Int, 4).Value = sueetieCalendar.CalendarID;
                cmd.Parameters.Add("@CalendarTitle", SqlDbType.NVarChar, 255).Value = sueetieCalendar.CalendarTitle;
                cmd.Parameters.Add("@CalendarDescription", SqlDbType.NVarChar, -1).Value = DataHelper.StringOrNull(sueetieCalendar.CalendarDescription);
                cmd.Parameters.Add("@CalendarUrl", SqlDbType.NVarChar, 255).Value = sueetieCalendar.CalendarUrl;
                cmd.Parameters.Add("@IsActive", SqlDbType.Bit, 1).Value = sueetieCalendar.IsActive;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override void CreateSueetieCalendar(SueetieCalendar sueetieCalendar)
        {
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_Calendar_Add", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CalendarTitle", SqlDbType.NVarChar, 255).Value = sueetieCalendar.CalendarTitle;
                    cmd.Parameters.Add("@CalendarDescription", SqlDbType.NVarChar, -1).Value = DataHelper.StringOrNull(sueetieCalendar.CalendarDescription);
                    cmd.Parameters.Add("@CalendarUrl", SqlDbType.NVarChar, 255).Value = sueetieCalendar.CalendarUrl;

                    cn.Open();
                    cmd.ExecuteNonQuery();

                    cn.Close();
                }
            }
        }

        public override SueetieCalendar GetSueetieCalendar(int calendarID)
        {
            var _sueetieCalendar = new SueetieCalendar();

            using (var cn = this.GetSqlConnection())
            {
                //string sql = "select * from sueetie_calendars where calendarID = " + calendarID;
                var sql = "select * from sueetie_calendars where calendarID = @calendarID";
                var cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add("@calendarID", SqlDbType.Int).Value = calendarID;
                cmd.CommandType = CommandType.Text;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        PopulateSueetieCalendarList(dr, _sueetieCalendar);
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return _sueetieCalendar;
        }

        public override int CreateSueetieCalendarEvent(SueetieCalendarEvent sueetieCalendarEvent)
        {
            var eventID = 0;

            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_CalendarEvent_Add", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@EventGuid", SqlDbType.UniqueIdentifier, 16).Value = sueetieCalendarEvent.EventGuid;
                    cmd.Parameters.Add("@CalendarID", SqlDbType.Int, 4).Value = sueetieCalendarEvent.CalendarID;
                    cmd.Parameters.Add("@EventTitle", SqlDbType.NVarChar, 255).Value = sueetieCalendarEvent.EventTitle;
                    cmd.Parameters.Add("@EventDescription", SqlDbType.NVarChar, 1000).Value = sueetieCalendarEvent.EventDescription;
                    cmd.Parameters.Add("@StartDateTime", SqlDbType.SmallDateTime, 4).Value = sueetieCalendarEvent.StartDateTime;
                    cmd.Parameters.Add("@EndDateTime", SqlDbType.SmallDateTime, 4).Value = sueetieCalendarEvent.EndDateTime;
                    cmd.Parameters.Add("@AllDayEvent", SqlDbType.Bit, 1).Value = sueetieCalendarEvent.AllDayEvent;
                    cmd.Parameters.Add("@Url", SqlDbType.NVarChar, 500).Value = DataHelper.StringOrNull(sueetieCalendarEvent.Url);
                    cmd.Parameters.Add("@RepeatEndDate", SqlDbType.SmallDateTime, 4).Value = sueetieCalendarEvent.RepeatEndDate;
                    cmd.Parameters.Add("@SourceContentID", SqlDbType.Int, 4).Value = sueetieCalendarEvent.SourceContentID;
                    cmd.Parameters.Add("@CreatedBy", SqlDbType.Int, 4).Value = sueetieCalendarEvent.CreatedBy;

                    cmd.Parameters.Add("@EventID", SqlDbType.Int).Direction = ParameterDirection.Output;


                    cn.Open();
                    cmd.ExecuteNonQuery();

                    eventID = (int)cmd.Parameters["@EventID"].Value;
                    cn.Close();
                }
            }
            return eventID;
        }

        public override void UpdateSueetieCalendarEvent(SueetieCalendarEvent sueetieCalendarEvent)
        {
            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("Sueetie_CalendarEvent_Update", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@EventGuid", SqlDbType.UniqueIdentifier, 16).Value = sueetieCalendarEvent.EventGuid;
                cmd.Parameters.Add("@CalendarID", SqlDbType.Int, 4).Value = sueetieCalendarEvent.CalendarID;
                cmd.Parameters.Add("@EventTitle", SqlDbType.NVarChar, 255).Value = DataHelper.StringOrNull(sueetieCalendarEvent.EventTitle);
                cmd.Parameters.Add("@EventDescription", SqlDbType.NVarChar, 1000).Value = DataHelper.StringOrNull(sueetieCalendarEvent.EventDescription);
                cmd.Parameters.Add("@StartDateTime", SqlDbType.SmallDateTime, 4).Value = sueetieCalendarEvent.StartDateTime;
                cmd.Parameters.Add("@EndDateTime", SqlDbType.SmallDateTime, 4).Value = sueetieCalendarEvent.EndDateTime;
                cmd.Parameters.Add("@AllDayEvent", SqlDbType.Bit, 1).Value = sueetieCalendarEvent.AllDayEvent;
                cmd.Parameters.Add("@RepeatEndDate", SqlDbType.SmallDateTime, 4).Value = sueetieCalendarEvent.RepeatEndDate;
                cmd.Parameters.Add("@IsActive", SqlDbType.Bit, 1).Value = sueetieCalendarEvent.IsActive;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override void DeleteCalendarEvent(string _calendareventGuid)
        {
            using (var cn = this.GetSqlConnection())
            {
                var sql = "delete from sueetie_calendarevents where eventGuid = '" + _calendareventGuid + "'";
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override void AddSueetieRequest(SueetieRequest sueetieRequest)
        {
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("Sueetie_Request_Add", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@ContentID", SqlDbType.Int, 4).Value = sueetieRequest.ContentID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = sueetieRequest.UserID;
                    cmd.Parameters.Add("@Url", SqlDbType.NVarChar, 500).Value = sueetieRequest.Url.ToLower();
                    cmd.Parameters.Add("@PageTitle", SqlDbType.NVarChar, 500).Value = sueetieRequest.PageTitle;
                    cmd.Parameters.Add("@ApplicationID", SqlDbType.Int, 4).Value = sueetieRequest.ApplicationID;
                    cmd.Parameters.Add("@RemoteIP", SqlDbType.NVarChar, 25).Value = sueetieRequest.RemoteIP;
                    cmd.Parameters.Add("@UserAgent", SqlDbType.NVarChar, 1000).Value = sueetieRequest.UserAgent;
                    cmd.Parameters.Add("@RecipientID", SqlDbType.Int, 4).Value = sueetieRequest.RecipientID;
                    cmd.Parameters.Add("@ContactTypeID", SqlDbType.Int, 4).Value = sueetieRequest.ContactTypeID;

                    cn.Open();
                    cmd.ExecuteNonQuery();

                    cn.Close();
                }
            }
        }

        public override List<string> GetFilteredUrlList()
        {
            var stringList = new List<string>();
            using (var cn = this.GetSqlConnection())
            {
                var sql = "select urlExcerpt from SuAnalytics_FilteredUrls";
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        stringList.Add(dr["urlExcerpt"] as string);
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return stringList;
        }

        public override List<PageRule> GetPageRuleList()
        {
            var PageRuleList = new List<PageRule>();
            using (var cn = this.GetSqlConnection())
            {
                var sql = "select * from SuAnalytics_PageRules order by UrlExcerpt";
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    PageRule _PageRule = null;
                    while (dr.Read())
                    {
                        _PageRule = new PageRule();
                        PopulatePageRuleList(dr, _PageRule);
                        PageRuleList.Add(_PageRule);
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return PageRuleList;
        }

        public override void UpdatePageRule(PageRule pageRule)
        {
            using (var cn = this.GetSqlConnection())
            {
                var cmd = new SqlCommand("SuAnalytics_PageRule_Update", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@PageRuleID", SqlDbType.Int, 4).Value = pageRule.PageRuleID;
                cmd.Parameters.Add("@UrlExcerpt", SqlDbType.NVarChar, 255).Value = pageRule.UrlExcerpt;
                cmd.Parameters.Add("@UrlFinal", SqlDbType.NVarChar, 255).Value = pageRule.UrlFinal;
                cmd.Parameters.Add("@PageTitle", SqlDbType.NVarChar, 255).Value = pageRule.PageTitle;
                cmd.Parameters.Add("@IsEqual", SqlDbType.Bit, 1).Value = pageRule.IsEqual;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override void DeletePageRule(int pageruleId)
        {
            using (var cn = this.GetSqlConnection())
            {
                var sql = "delete from SuAnalytics_PageRules where pageRuleID = " + pageruleId;

                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public override void CreatePageRule(PageRule pageRule)
        {
            using (var cn = this.GetSqlConnection())
            {
                using (var cmd = new SqlCommand("SuAnalytics_PageRule_Create", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UrlExcerpt", SqlDbType.NVarChar, 255).Value = DataHelper.StringOrNull(pageRule.UrlExcerpt);
                    cmd.Parameters.Add("@UrlFinal", SqlDbType.NVarChar, 255).Value = DataHelper.StringOrNull(pageRule.UrlFinal);
                    cmd.Parameters.Add("@PageTitle", SqlDbType.NVarChar, 255).Value = pageRule.PageTitle;
                    cmd.Parameters.Add("@IsEqual", SqlDbType.Bit, 1).Value = pageRule.IsEqual;

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }
            }
        }

        public override List<CrawlerAgent> GetCrawlerAgentList()
        {
            var crawlerAgentList = new List<CrawlerAgent>();
            using (var cn = this.GetSqlConnection())
            {
                var sql = "select * from SuAddons_CrawlerAgents order by agentExcerpt";
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    CrawlerAgent _CrawlerAgent = null;
                    while (dr.Read())
                    {
                        _CrawlerAgent = new CrawlerAgent();
                        PopulateCrawlerAgentList(dr, _CrawlerAgent);
                        crawlerAgentList.Add(_CrawlerAgent);
                    }

                    dr.Close();
                    cn.Close();
                }
            }
            return crawlerAgentList;
        }
    }
}