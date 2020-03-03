using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Web;
using ScrewTurn.Wiki.PluginFramework;

namespace ScrewTurn.Wiki.Plugins.SqlCommon
{
    /// <summary>
    ///     Implements a base class for a SQL users storage provider.
    /// </summary>
    public abstract class SqlUsersStorageProviderBase : SqlStorageProviderBase, IUsersStorageProviderV30
    {
        /// <summary>
        ///     Gets a value indicating whether user accounts are read-only.
        /// </summary>
        public bool UserAccountsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        ///     Gets a value indicating whether user groups are read-only. If so, the provider
        ///     should support default user groups as defined in the wiki configuration.
        /// </summary>
        public bool UserGroupsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        ///     Gets a value indicating whether group membership is read-only (if <see cref="UserAccountsReadOnly" />
        ///     is <c>false</c>, then this property must be <c>false</c>). If this property is <c>true</c>, the provider
        ///     should return membership data compatible with default user groups.
        /// </summary>
        public bool GroupMembershipReadOnly
        {
            get { return false; }
        }

        /// <summary>
        ///     Gets a value indicating whether users' data is read-only.
        /// </summary>
        public bool UsersDataReadOnly
        {
            get { return false; }
        }

        #region IUsersStorageProvider Members

        /// <summary>
        ///     Tests a Password for a User account.
        /// </summary>
        /// <param name="user">The User account.</param>
        /// <param name="password">The Password to test.</param>
        /// <returns>True if the Password is correct.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="user" /> or <paramref name="password" /> are <c>null</c>.</exception>
        public bool TestAccount(UserInfo user, string password)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (password == null) throw new ArgumentNullException("password");

            return TryManualLogin(user.Username, password) != null;
        }

        /// <summary>
        ///     Gets the complete list of Users.
        /// </summary>
        /// <returns>All the Users, sorted by username.</returns>
        public IEnumerable<UserInfo> GetUsers()
        {
            var builder = CommandBuilder;

            var queryBuilder = QueryBuilder.NewQuery(builder);
            var query = queryBuilder.SelectFrom(
                "User", "UserGroupMembership", "Username", "User", Join.LeftJoin,
                new[] {"Username", "DisplayName", "Email", "Active", "DateTime"},
                new[] {"UserGroup"});
            query = queryBuilder.OrderBy(query, new[] {"User_Username"}, new[] {Ordering.Asc});

            var command = builder.GetCommand(connString, query, new List<Parameter>());

            var reader = ExecuteReader(command);

            if (reader != null)
            {
                var result = new List<UserInfo>(100);

                var prevUsername = "|||";
                string username = null;
                string displayName;
                string email;
                bool active;
                DateTime dateTime;
                var groups = new List<string>(5);

                while (reader.Read())
                {
                    username = reader["User_Username"] as string;

                    if (username != prevUsername)
                    {
                        // Set previous user's groups
                        if (prevUsername != "|||")
                        {
                            result[result.Count - 1].Groups = groups.ToArray();
                            groups.Clear();
                        }

                        // Read new data
                        displayName = GetNullableColumn<string>(reader, "User_DisplayName", null);
                        email = reader["User_Email"] as string;
                        active = (bool) reader["User_Active"];
                        dateTime = (DateTime) reader["User_DateTime"];

                        // Append new user
                        result.Add(new UserInfo(username, displayName, email, active, dateTime, this));
                    }

                    // Keep reading groups
                    prevUsername = username;
                    if (!IsDBNull(reader, "UserGroupMembership_UserGroup"))
                    {
                        groups.Add(reader["UserGroupMembership_UserGroup"] as string);
                    }
                }

                if (result.Count > 0)
                {
                    result[result.Count - 1].Groups = groups.ToArray();
                }

                CloseReader(command, reader);

                return result;
            }
            return null;
        }

        /// <summary>
        ///     Adds a new User.
        /// </summary>
        /// <param name="username">The Username.</param>
        /// <param name="displayName">The display name (can be <c>null</c>).</param>
        /// <param name="password">The Password.</param>
        /// <param name="email">The Email address.</param>
        /// <param name="active">A value indicating whether the account is active.</param>
        /// <param name="dateTime">The Account creation Date/Time.</param>
        /// <returns>The correct <see cref="T:UserInfo" /> object or <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException">
        ///     If <paramref name="username" />, <paramref name="password" /> or
        ///     <paramref name="email" /> are <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     If <paramref name="username" />, <paramref name="password" /> or
        ///     <paramref name="email" /> are empty.
        /// </exception>
        public UserInfo AddUser(string username, string displayName, string password, string email, bool active,
            DateTime dateTime)
        {
            if (username == null) throw new ArgumentNullException("username");
            if (username.Length == 0) throw new ArgumentException("Username cannot be empty", "username");
            if (password == null) throw new ArgumentNullException("password");
            if (password.Length == 0) throw new ArgumentException("Password cannot be empty", "password");
            if (email == null) throw new ArgumentNullException("email");
            if (email.Length == 0) throw new ArgumentException("Email cannot be empty", "email");

            var builder = CommandBuilder;

            var query = QueryBuilder.NewQuery(builder).InsertInto("User",
                new[] {"Username", "PasswordHash", "DisplayName", "Email", "Active", "DateTime"},
                new[] {"Username", "PasswordHash", "DisplayName", "Email", "Active", "DateTime"});

            var parameters = new List<Parameter>(6);
            parameters.Add(new Parameter(ParameterType.String, "Username", username));
            parameters.Add(new Parameter(ParameterType.String, "PasswordHash", Hash.Compute(password)));
            if (string.IsNullOrEmpty(displayName))
                parameters.Add(new Parameter(ParameterType.String, "DisplayName", DBNull.Value));
            else parameters.Add(new Parameter(ParameterType.String, "DisplayName", displayName));
            parameters.Add(new Parameter(ParameterType.String, "Email", email));
            parameters.Add(new Parameter(ParameterType.Boolean, "Active", active));
            parameters.Add(new Parameter(ParameterType.DateTime, "DateTime", dateTime));

            var command = builder.GetCommand(connString, query, parameters);

            var rows = ExecuteNonQuery(command);

            if (rows == 1)
            {
                return new UserInfo(username, displayName, email, active, dateTime, this);
            }
            return null;
        }

        /// <summary>
        ///     Gets the user groups of a user.
        /// </summary>
        /// <param name="transaction">A database transaction.</param>
        /// <param name="username">The username.</param>
        /// <returns>The groups.</returns>
        private string[] GetUserGroups(IDbTransaction transaction, string username)
        {
            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.SelectFrom("UserGroupMembership", new[] {"UserGroup"});
            query = queryBuilder.Where(query, "User", WhereOperator.Equals, "Username");
            queryBuilder.OrderBy(query, new[] {"UserGroup_Name"}, new[] {Ordering.Asc});

            var parameters = new List<Parameter>(1);
            parameters.Add(new Parameter(ParameterType.String, "Username", username));

            var command = builder.GetCommand(transaction, query, parameters);

            var reader = ExecuteReader(command);

            if (reader != null)
            {
                var result = new List<string>(5);

                while (reader.Read())
                {
                    result.Add(reader["UserGroup"] as string);
                }

                CloseReader(reader);

                return result.ToArray();
            }
            return null;
        }

        /// <summary>
        ///     Gets the user groups of a user.
        /// </summary>
        /// <param name="connection">A database connection.</param>
        /// <param name="username">The username.</param>
        /// <returns>The groups.</returns>
        private string[] GetUserGroups(DbConnection connection, string username)
        {
            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.SelectFrom("UserGroupMembership", new[] {"UserGroup"});
            query = queryBuilder.Where(query, "User", WhereOperator.Equals, "Username");
            queryBuilder.OrderBy(query, new[] {"UserGroup_Name"}, new[] {Ordering.Asc});

            var parameters = new List<Parameter>(1);
            parameters.Add(new Parameter(ParameterType.String, "Username", username));

            var command = builder.GetCommand(connection, query, parameters);

            var reader = ExecuteReader(command);

            if (reader != null)
            {
                var result = new List<string>(5);

                while (reader.Read())
                {
                    result.Add(reader["UserGroup"] as string);
                }

                CloseReader(reader);

                return result.ToArray();
            }
            return null;
        }

        /// <summary>
        ///     Modifies a User.
        /// </summary>
        /// <param name="user">The Username of the user to modify.</param>
        /// <param name="newDisplayName">The new display name (can be <c>null</c>).</param>
        /// <param name="newPassword">The new Password (<c>null</c> or blank to keep the current password).</param>
        /// <param name="newEmail">The new Email address.</param>
        /// <param name="newActive">A value indicating whether the account is active.</param>
        /// <returns>The correct <see cref="T:UserInfo" /> object or <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException">If <b>user</b> or <b>newEmail</b> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <b>newEmail</b> is empty.</exception>
        public UserInfo ModifyUser(UserInfo user, string newDisplayName, string newPassword, string newEmail,
            bool newActive)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (newEmail == null) throw new ArgumentNullException("newEmail");
            if (newEmail.Length == 0) throw new ArgumentException("New Email cannot be empty", "newEmail");

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);
            var transaction = BeginTransaction(connection);

            var queryBuilder = new QueryBuilder(builder);

            var query = "";
            if (string.IsNullOrEmpty(newPassword))
            {
                query = queryBuilder.Update("User",
                    new[] {"DisplayName", "Email", "Active"},
                    new[] {"DisplayName", "Email", "Active"});
            }
            else
            {
                query = queryBuilder.Update("User",
                    new[] {"PasswordHash", "DisplayName", "Email", "Active"},
                    new[] {"PasswordHash", "DisplayName", "Email", "Active"});
            }
            query = queryBuilder.Where(query, "Username", WhereOperator.Equals, "Username");

            var parameters = new List<Parameter>(5);
            if (!string.IsNullOrEmpty(newPassword))
            {
                parameters.Add(new Parameter(ParameterType.String, "PasswordHash", Hash.Compute(newPassword)));
            }
            if (string.IsNullOrEmpty(newDisplayName))
                parameters.Add(new Parameter(ParameterType.String, "DisplayName", DBNull.Value));
            else parameters.Add(new Parameter(ParameterType.String, "DisplayName", newDisplayName));
            parameters.Add(new Parameter(ParameterType.String, "Email", newEmail));
            parameters.Add(new Parameter(ParameterType.Boolean, "Active", newActive));
            parameters.Add(new Parameter(ParameterType.String, "Username", user.Username));

            var command = builder.GetCommand(transaction, query, parameters);

            var rows = ExecuteNonQuery(command, false);

            if (rows == 1)
            {
                var result = new UserInfo(user.Username, newDisplayName, newEmail, newActive, user.DateTime, this);
                result.Groups = GetUserGroups(transaction, user.Username);

                CommitTransaction(transaction);
                return result;
            }
            RollbackTransaction(transaction);
            return null;
        }

        /// <summary>
        ///     Removes a User.
        /// </summary>
        /// <param name="user">The User to remove.</param>
        /// <returns>True if the User has been removed successfully.</returns>
        /// <exception cref="ArgumentNullException">If <b>user</b> is <c>null</c>.</exception>
        public bool RemoveUser(UserInfo user)
        {
            if (user == null) throw new ArgumentNullException("user");

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.DeleteFrom("User");
            query = queryBuilder.Where(query, "Username", WhereOperator.Equals, "Username");

            var parameters = new List<Parameter>(1);
            parameters.Add(new Parameter(ParameterType.String, "Username", user.Username));

            var command = builder.GetCommand(connString, query, parameters);

            var rows = ExecuteNonQuery(command);

            return rows == 1;
        }

        /// <summary>
        ///     Gets all the user groups.
        /// </summary>
        /// <returns>All the groups, sorted by name.</returns>
        public IEnumerable<UserGroup> GetUserGroups()
        {
            var builder = CommandBuilder;

            var queryBuilder = QueryBuilder.NewQuery(builder);
            var query = queryBuilder.SelectFrom(
                "UserGroup", "UserGroupMembership", "Name", "UserGroup", Join.LeftJoin,
                new[] {"Name", "Description"}, new[] {"User"});
            query = queryBuilder.OrderBy(query, new[] {"UserGroup_Name", "UserGroupMembership_User"},
                new[] {Ordering.Asc, Ordering.Asc});

            var command = builder.GetCommand(connString, query, new List<Parameter>());

            var reader = ExecuteReader(command);

            var previousName = "|||";
            string name = null;
            string description;
            var users = new List<string>(50);

            if (reader != null)
            {
                var result = new List<UserGroup>(5);

                while (reader.Read())
                {
                    name = reader["UserGroup_Name"] as string;

                    if (name != previousName)
                    {
                        // Set previous group's users
                        if (previousName != "|||")
                        {
                            result[result.Count - 1].Users = users.ToArray();
                            users.Clear();
                        }

                        // Read new data
                        description = reader["UserGroup_Description"] as string;

                        // Append new group
                        result.Add(new UserGroup(name, description, this));
                    }

                    // Keep reading users
                    previousName = name;
                    if (!IsDBNull(reader, "UserGroupMembership_User"))
                    {
                        users.Add(reader["UserGroupMembership_User"] as string);
                    }
                }

                if (result.Count > 0)
                {
                    result[result.Count - 1].Users = users.ToArray();
                }

                CloseReader(command, reader);

                return result;
            }
            return null;
        }

        /// <summary>
        ///     Adds a new user group.
        /// </summary>
        /// <param name="name">The name of the group.</param>
        /// <param name="description">The description of the group.</param>
        /// <returns>The correct <see cref="T:UserGroup" /> object or <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException">If <b>name</b> or <b>description</b> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <b>name</b> is empty.</exception>
        public UserGroup AddUserGroup(string name, string description)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (name.Length == 0) throw new ArgumentException("Name cannot be empty", "name");
            if (description == null) throw new ArgumentNullException("description");

            var builder = CommandBuilder;

            var query = QueryBuilder.NewQuery(builder).InsertInto("UserGroup",
                new[] {"Name", "Description"}, new[] {"Name", "Description"});

            var parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.String, "Name", name));
            parameters.Add(new Parameter(ParameterType.String, "Description", description));

            var command = builder.GetCommand(connString, query, parameters);

            var rows = ExecuteNonQuery(command);

            if (rows == 1)
            {
                return new UserGroup(name, description, this);
            }
            return null;
        }

        /// <summary>
        ///     Gets the users of a group.
        /// </summary>
        /// <param name="transaction">A database transaction.</param>
        /// <param name="group">The group.</param>
        /// <returns>The users.</returns>
        private string[] GetUserGroupUsers(IDbTransaction transaction, string group)
        {
            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.SelectFrom("UserGroupMembership", new[] {"User"});
            query = queryBuilder.Where(query, "UserGroup", WhereOperator.Equals, "UserGroup");
            query = queryBuilder.OrderBy(query, new[] {"User"}, new[] {Ordering.Asc});

            var parameters = new List<Parameter>(1);
            parameters.Add(new Parameter(ParameterType.String, "UserGroup", group));

            var command = builder.GetCommand(transaction, query, parameters);

            var reader = ExecuteReader(command);

            if (reader != null)
            {
                var result = new List<string>(100);

                while (reader.Read())
                {
                    result.Add(reader["User"] as string);
                }

                CloseReader(reader);

                return result.ToArray();
            }
            return null;
        }

        /// <summary>
        ///     Gets the users of a group.
        /// </summary>
        /// <param name="connection">A database connection.</param>
        /// <param name="group">The group.</param>
        /// <returns>The users.</returns>
        private string[] GetUserGroupUsers(IDbConnection connection, string group)
        {
            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.SelectFrom("UserGroupMembership", new[] {"User"});
            query = queryBuilder.Where(query, "UserGroup", WhereOperator.Equals, "UserGroup");
            query = queryBuilder.OrderBy(query, new[] {"User"}, new[] {Ordering.Asc});

            var parameters = new List<Parameter>(1);
            parameters.Add(new Parameter(ParameterType.String, "UserGroup", group));

            var command = builder.GetCommand(connection, query, parameters);

            var reader = ExecuteReader(command);

            if (reader != null)
            {
                var result = new List<string>(100);

                while (reader.Read())
                {
                    result.Add(reader["User"] as string);
                }

                CloseReader(reader);

                return result.ToArray();
            }
            return null;
        }

        /// <summary>
        ///     Modifies a user group.
        /// </summary>
        /// <param name="group">The group to modify.</param>
        /// <param name="description">The new description of the group.</param>
        /// <returns>The correct <see cref="T:UserGroup" /> object or <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException">If <b>group</b> or <b>description</b> are <c>null</c>.</exception>
        public UserGroup ModifyUserGroup(UserGroup group, string description)
        {
            if (group == null) throw new ArgumentNullException("group");
            if (description == null) throw new ArgumentNullException("description");

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);
            var transaction = BeginTransaction(connection);

            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.Update("UserGroup",
                new[] {"Description"}, new[] {"Description"});
            query = queryBuilder.Where(query, "Name", WhereOperator.Equals, "Name");

            var parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.String, "Description", description));
            parameters.Add(new Parameter(ParameterType.String, "Name", group.Name));

            var command = builder.GetCommand(transaction, query, parameters);

            var rows = ExecuteNonQuery(command, false);

            if (rows == 1)
            {
                var result = new UserGroup(group.Name, description, this);
                result.Users = GetUserGroupUsers(transaction, group.Name);

                CommitTransaction(transaction);
                return result;
            }
            RollbackTransaction(transaction);
            return null;
        }

        /// <summary>
        ///     Removes a user group.
        /// </summary>
        /// <param name="group">The group to remove.</param>
        /// <returns><c>true</c> if the group is removed, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">If <b>group</b> is <c>null</c>.</exception>
        public bool RemoveUserGroup(UserGroup group)
        {
            if (group == null) throw new ArgumentNullException("group");

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.DeleteFrom("UserGroup");
            query = queryBuilder.Where(query, "Name", WhereOperator.Equals, "Name");

            var parameters = new List<Parameter>(1);
            parameters.Add(new Parameter(ParameterType.String, "Name", group.Name));

            var command = builder.GetCommand(connString, query, parameters);

            var rows = ExecuteNonQuery(command);

            return rows == 1;
        }

        /// <summary>
        ///     Verifies that a user exists.
        /// </summary>
        /// <param name="transaction">A database transaction.</param>
        /// <param name="username">The username.</param>
        /// <returns><c>true</c> if the user exists, <c>false</c> otherwise.</returns>
        private bool UserExists(IDbTransaction transaction, string username)
        {
            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.SelectCountFrom("User");
            query = queryBuilder.Where(query, "Username", WhereOperator.Equals, "Username");

            var parameters = new List<Parameter>(1);
            parameters.Add(new Parameter(ParameterType.String, "Username", username));

            var command = builder.GetCommand(transaction, query, parameters);

            var count = ExecuteScalar(command, -1, false);

            return count == 1;
        }

        /// <summary>
        ///     Verifies that a user exists.
        /// </summary>
        /// <param name="connection">A database connection.</param>
        /// <param name="username">The username.</param>
        /// <returns><c>true</c> if the user exists, <c>false</c> otherwise.</returns>
        private bool UserExists(IDbConnection connection, string username)
        {
            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.SelectCountFrom("User");
            query = queryBuilder.Where(query, "Username", WhereOperator.Equals, "Username");

            var parameters = new List<Parameter>(1);
            parameters.Add(new Parameter(ParameterType.String, "Username", username));

            var command = builder.GetCommand(connection, query, parameters);

            var count = ExecuteScalar(command, -1, false);

            return count == 1;
        }

        /// <summary>
        ///     Removes the user group membership for a user.
        /// </summary>
        /// <param name="transaction">A database transaction.</param>
        /// <param name="username">The username.</param>
        private void RemoveUserGroupMembership(IDbTransaction transaction, string username)
        {
            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.DeleteFrom("UserGroupMembership");
            query = queryBuilder.Where(query, "User", WhereOperator.Equals, "Username");

            var parameters = new List<Parameter>(1);
            parameters.Add(new Parameter(ParameterType.String, "Username", username));

            var command = builder.GetCommand(transaction, query, parameters);

            ExecuteNonQuery(command, false);
        }

        /// <summary>
        ///     Sets the group memberships of a user account.
        /// </summary>
        /// <param name="user">The user account.</param>
        /// <param name="groups">The groups the user account is member of.</param>
        /// <returns>The correct <see cref="T:UserGroup" /> object or <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException">If <b>user</b> or <b>groups</b> are <c>null</c>.</exception>
        public UserInfo SetUserMembership(UserInfo user, IEnumerable<string> groups)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (groups == null) throw new ArgumentNullException("groups");

            // 1. Remove old user group membership
            // 2. Add new memberships, one by one

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);
            var transaction = BeginTransaction(connection);

            if (!UserExists(transaction, user.Username))
            {
                RollbackTransaction(transaction);
                return null;
            }

            RemoveUserGroupMembership(transaction, user.Username);

            var query = QueryBuilder.NewQuery(builder).InsertInto("UserGroupMembership",
                new[] {"User", "UserGroup"}, new[] {"User", "UserGroup"});

            foreach (var group in groups)
            {
                var parameters = new List<Parameter>(2);
                parameters.Add(new Parameter(ParameterType.String, "User", user.Username));
                parameters.Add(new Parameter(ParameterType.String, "UserGroup", group));

                var command = builder.GetCommand(transaction, query, parameters);

                var rows = ExecuteNonQuery(command, false);

                if (rows != 1)
                {
                    RollbackTransaction(transaction);
                    return null;
                }
            }

            var result = new UserInfo(user.Username, user.DisplayName, user.Email, user.Active, user.DateTime, this);
            result.Groups = groups;

            CommitTransaction(transaction);

            return result;
        }

        /// <summary>
        ///     Tries to login a user directly through the provider.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>The correct UserInfo object, or <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException">If <b>username</b> or <b>password</b> are <c>null</c>.</exception>
        public UserInfo TryManualLogin(string username, string password)
        {
            if (username == null) throw new ArgumentNullException("username");
            if (password == null) throw new ArgumentNullException("password");

            // Shortcut
            if (username.Length == 0) return null;
            if (password.Length == 0) return null;

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.SelectFrom(
                "User", "UserGroupMembership", "Username", "User", Join.LeftJoin,
                new[] {"Username", "PasswordHash", "DisplayName", "Email", "Active", "DateTime"},
                new[] {"UserGroup"});
            query = queryBuilder.Where(query, "User", "Username", WhereOperator.Equals, "Username");
            query = queryBuilder.AndWhere(query, "User", "PasswordHash", WhereOperator.Equals, "PasswordHash");

            var providedPasswordHash = Hash.Compute(password);

            var parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.String, "Username", username));
            parameters.Add(new Parameter(ParameterType.String, "PasswordHash", providedPasswordHash));

            var command = builder.GetCommand(connString, query, parameters);

            var reader = ExecuteReader(command);

            if (reader != null)
            {
                UserInfo result = null;

                string realUsername = null;
                string passwordHash = null;
                string displayName;
                string email;
                bool active;
                DateTime dateTime;
                var groups = new List<string>(5);

                while (reader.Read())
                {
                    if (result == null)
                    {
                        // Read data
                        realUsername = reader["User_Username"] as string;
                        passwordHash = reader["User_PasswordHash"] as string;
                        displayName = GetNullableColumn<string>(reader, "User_DisplayName", null);
                        email = reader["User_Email"] as string;
                        active = (bool) reader["User_Active"];
                        dateTime = (DateTime) reader["User_DateTime"];

                        // Create user
                        result = new UserInfo(realUsername, displayName, email, active, dateTime, this);
                    }

                    // Keep reading groups
                    if (!IsDBNull(reader, "UserGroupMembership_UserGroup"))
                    {
                        groups.Add(reader["UserGroupMembership_UserGroup"] as string);
                    }
                }

                if (result != null)
                {
                    result.Groups = groups.ToArray();
                }

                CloseReader(command, reader);

                if (result != null)
                {
                    if (result.Active &&
                        string.CompareOrdinal(result.Username, username) == 0 &&
                        string.CompareOrdinal(passwordHash, providedPasswordHash) == 0)
                    {
                        return result;
                    }
                    return null;
                }
                return null;
            }
            return null;
        }

        /// <summary>
        ///     Tries to login a user directly through the provider using
        ///     the current HttpContext and without username/password.
        /// </summary>
        /// <param name="context">The current HttpContext.</param>
        /// <returns>The correct UserInfo object, or <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException">If <b>context</b> is <c>null</c>.</exception>
        public UserInfo TryAutoLogin(HttpContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            return null;
        }

        /// <summary>
        ///     Gets a user account.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>The <see cref="T:UserInfo" />, or <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException">If <b>username</b> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <b>username</b> is empty.</exception>
        public UserInfo GetUser(string username)
        {
            if (username == null) throw new ArgumentNullException("username");
            if (username.Length == 0) throw new ArgumentException("Username cannot be empty", "username");

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.SelectFrom(
                "User", "UserGroupMembership", "Username", "User", Join.LeftJoin,
                new[] {"Username", "DisplayName", "Email", "Active", "DateTime"},
                new[] {"UserGroup"});
            query = queryBuilder.Where(query, "User", "Username", WhereOperator.Equals, "Username");

            var parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.String, "Username", username));

            var command = builder.GetCommand(connString, query, parameters);

            var reader = ExecuteReader(command);

            if (reader != null)
            {
                UserInfo result = null;

                string realUsername = null;
                string displayName;
                string email;
                bool active;
                DateTime dateTime;
                var groups = new List<string>(5);

                while (reader.Read())
                {
                    if (result == null)
                    {
                        // Read data
                        realUsername = reader["User_Username"] as string;
                        displayName = GetNullableColumn<string>(reader, "User_DisplayName", null);
                        email = reader["User_Email"] as string;
                        active = (bool) reader["User_Active"];
                        dateTime = (DateTime) reader["User_DateTime"];

                        // Create user
                        result = new UserInfo(realUsername, displayName, email, active, dateTime, this);
                    }

                    // Keep reading groups
                    if (!IsDBNull(reader, "UserGroupMembership_UserGroup"))
                    {
                        groups.Add(reader["UserGroupMembership_UserGroup"] as string);
                    }
                }

                if (result != null)
                {
                    result.Groups = groups.ToArray();
                }

                CloseReader(command, reader);

                return result;
            }
            return null;
        }

        /// <summary>
        ///     Gets a user account.
        /// </summary>
        /// <param name="email">The email address.</param>
        /// <returns>The first user found with the specified email address, or <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException">If <b>email</b> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <b>email</b> is empty.</exception>
        public UserInfo GetUserByEmail(string email)
        {
            if (email == null) throw new ArgumentNullException("email");
            if (email.Length == 0) throw new ArgumentException("Email cannot be empty", "email");

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.SelectFrom(
                "User", "UserGroupMembership", "Username", "User", Join.LeftJoin,
                new[] {"Username", "DisplayName", "Email", "Active", "DateTime"},
                new[] {"UserGroup"});
            query = queryBuilder.Where(query, "User", "Email", WhereOperator.Equals, "Email");


            var parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.String, "Email", email));

            var command = builder.GetCommand(connString, query, parameters);

            var reader = ExecuteReader(command);

            if (reader != null)
            {
                UserInfo result = null;

                string username = null;
                string displayName;
                string realEmail = null;
                bool active;
                DateTime dateTime;
                var groups = new List<string>(5);

                while (reader.Read())
                {
                    if (result == null)
                    {
                        // Read data
                        username = reader["User_Username"] as string;
                        displayName = GetNullableColumn<string>(reader, "User_DisplayName", null);
                        realEmail = reader["User_Email"] as string;
                        active = (bool) reader["User_Active"];
                        dateTime = (DateTime) reader["User_DateTime"];

                        // Create user
                        result = new UserInfo(username, displayName, realEmail, active, dateTime, this);
                    }

                    // Keep reading groups
                    if (!IsDBNull(reader, "UserGroupMembership_UserGroup"))
                    {
                        groups.Add(reader["UserGroupMembership_UserGroup"] as string);
                    }
                }

                if (result != null)
                {
                    result.Groups = groups.ToArray();
                }

                CloseReader(command, reader);

                return result;
            }
            return null;
        }

        /// <summary>
        ///     Notifies the provider that a user has logged in through the authentication cookie.
        /// </summary>
        /// <param name="user">The user who has logged in.</param>
        /// <exception cref="ArgumentNullException">If <b>user</b> is <c>null</c>.</exception>
        public void NotifyCookieLogin(UserInfo user)
        {
            if (user == null) throw new ArgumentNullException("user");
            // Nothing to do
        }

        /// <summary>
        ///     Notifies the provider that a user has logged out.
        /// </summary>
        /// <param name="user">The user who has logged out.</param>
        /// <exception cref="ArgumentNullException">If <b>user</b> is <c>null</c>.</exception>
        public void NotifyLogout(UserInfo user)
        {
            if (user == null) throw new ArgumentNullException("user");
            // Nothing to do
        }

        /// <summary>
        ///     Removes a user data element.
        /// </summary>
        /// <param name="transaction">A database transaction.</param>
        /// <param name="username">The username.</param>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if the data element is removed, <c>false</c> otherwise.</returns>
        private bool RemoveUserData(IDbTransaction transaction, string username, string key)
        {
            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.DeleteFrom("UserData");
            query = queryBuilder.Where(query, "User", WhereOperator.Equals, "Username");
            query = queryBuilder.AndWhere(query, "Key", WhereOperator.Equals, "Key");

            var parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.String, "Username", username));
            parameters.Add(new Parameter(ParameterType.String, "Key", key));

            var command = builder.GetCommand(transaction, query, parameters);

            var rows = ExecuteNonQuery(command, false);

            return rows != -1; // Success also if no elements are removed
        }

        /// <summary>
        ///     Stores a user data element, overwriting the previous one if present.
        /// </summary>
        /// <param name="user">The user the data belongs to.</param>
        /// <param name="key">The key of the data element (case insensitive).</param>
        /// <param name="value">The value of the data element, <c>null</c> for deleting the data.</param>
        /// <returns><c>true</c> if the data element is stored, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">If <b>user</b> or <b>key</b> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <b>key</b> is empty.</exception>
        public bool StoreUserData(UserInfo user, string key, string value)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (key == null) throw new ArgumentNullException("key");
            if (key.Length == 0) throw new ArgumentException("Key cannot be empty", "key");

            // 1. Remove previous key, if present
            // 2. Add new key, if value != null

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);
            var transaction = BeginTransaction(connection);

            var done = RemoveUserData(transaction, user.Username, key);

            if (done)
            {
                if (value != null)
                {
                    var query = QueryBuilder.NewQuery(builder).InsertInto("UserData",
                        new[] {"User", "Key", "Data"}, new[] {"Username", "Key", "Data"});

                    var parameters = new List<Parameter>(3);
                    parameters.Add(new Parameter(ParameterType.String, "Username", user.Username));
                    parameters.Add(new Parameter(ParameterType.String, "Key", key));
                    parameters.Add(new Parameter(ParameterType.String, "Data", value));

                    var command = builder.GetCommand(transaction, query, parameters);

                    var rows = ExecuteNonQuery(command, false);
                    if (rows == 1) CommitTransaction(transaction);
                    else RollbackTransaction(transaction);

                    return rows == 1;
                }
                CommitTransaction(transaction);
                return true;
            }
            RollbackTransaction(transaction);
            return false;
        }

        /// <summary>
        ///     Gets a user data element, if any.
        /// </summary>
        /// <param name="user">The user the data belongs to.</param>
        /// <param name="key">The key of the data element.</param>
        /// <returns>The value of the data element, or <c>null</c> if the element is not found.</returns>
        /// <exception cref="ArgumentNullException">If <b>user</b> or <b>key</b> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <b>key</b> is empty.</exception>
        public string RetrieveUserData(UserInfo user, string key)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (key == null) throw new ArgumentNullException("key");
            if (key.Length == 0) throw new ArgumentException("Key cannot be empty", "key");

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.SelectFrom("UserData", new[] {"Data"});
            query = queryBuilder.Where(query, "User", WhereOperator.Equals, "Username");
            query = queryBuilder.AndWhere(query, "Key", WhereOperator.Equals, "Key");

            var parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.String, "Username", user.Username));
            parameters.Add(new Parameter(ParameterType.String, "Key", key));

            var command = builder.GetCommand(connString, query, parameters);

            var reader = ExecuteReader(command);

            if (reader != null)
            {
                string data = null;

                if (reader.Read())
                {
                    data = reader["Data"] as string;
                }

                CloseReader(command, reader);

                return data;
            }
            return null;
        }

        /// <summary>
        ///     Retrieves all the user data elements for a user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The user data elements (key-&gt;value).</returns>
        /// <exception cref="ArgumentNullException">If <b>user</b> is <c>null</c>.</exception>
        public IDictionary<string, string> RetrieveAllUserData(UserInfo user)
        {
            if (user == null) throw new ArgumentNullException("user");

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            // Sorting order is not relevant
            var query = queryBuilder.SelectFrom("UserData", new[] {"Key", "Data"});
            query = queryBuilder.Where(query, "User", WhereOperator.Equals, "Username");

            var parameters = new List<Parameter>(1);
            parameters.Add(new Parameter(ParameterType.String, "Username", user.Username));

            var command = builder.GetCommand(connString, query, parameters);

            var reader = ExecuteReader(command);

            if (reader != null)
            {
                var result = new Dictionary<string, string>(10);

                while (reader.Read())
                {
                    result.Add(reader["Key"] as string, reader["Data"] as string);
                }

                CloseReader(command, reader);

                return result;
            }
            return null;
        }

        /// <summary>
        ///     Gets all the users that have the specified element in their data.
        /// </summary>
        /// <param name="key">The key of the data.</param>
        /// <returns>The users and the data.</returns>
        /// <exception cref="ArgumentNullException">If <b>key</b> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <b>key</b> is empty.</exception>
        public IDictionary<UserInfo, string> GetUsersWithData(string key)
        {
            if (key == null) throw new ArgumentNullException("key");
            if (key.Length == 0) throw new ArgumentException("Key cannot be empty", "key");

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.SelectFrom("User", "UserData", "Username", "User", Join.RightJoin,
                new[] {"Username", "DisplayName", "Email", "Active", "DateTime"}, new[] {"Data"},
                "UserGroupMembership", "User", Join.LeftJoin, new[] {"UserGroup"});
            query = queryBuilder.Where(query, "UserData", "Key", WhereOperator.Equals, "Key");
            query = queryBuilder.OrderBy(query, new[] {"User_Username"}, new[] {Ordering.Asc});

            var parameters = new List<Parameter>(1);
            parameters.Add(new Parameter(ParameterType.String, "Key", key));

            var command = builder.GetCommand(connString, query, parameters);

            var reader = ExecuteReader(command);

            if (reader != null)
            {
                var result = new Dictionary<UserInfo, string>(100);

                var prevUsername = "|||";
                UserInfo prevUser = null;
                string username = null;
                string displayName;
                string email;
                bool active;
                DateTime dateTime;
                string data;
                var groups = new List<string>(5);

                while (reader.Read())
                {
                    username = reader["User_Username"] as string;

                    if (username != prevUsername)
                    {
                        // Set previous user's groups
                        if (prevUsername != "|||")
                        {
                            prevUser.Groups = groups.ToArray();
                            groups.Clear();
                        }

                        // Read new data
                        displayName = GetNullableColumn<string>(reader, "User_DisplayName", null);
                        email = reader["User_Email"] as string;
                        active = (bool) reader["User_Active"];
                        dateTime = (DateTime) reader["User_DateTime"];
                        data = reader["UserData_Data"] as string;

                        // Append new user
                        prevUser = new UserInfo(username, displayName, email, active, dateTime, this);
                        result.Add(prevUser, data);
                    }

                    // Keep reading groups
                    prevUsername = username;
                    if (!IsDBNull(reader, "UserGroupMembership_UserGroup"))
                    {
                        groups.Add(reader["UserGroupMembership_UserGroup"] as string);
                    }
                }

                if (prevUser != null)
                {
                    prevUser.Groups = groups.ToArray();
                }

                CloseReader(command, reader);

                return result;
            }
            return null;
        }

        #endregion
    }
}