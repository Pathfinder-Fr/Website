using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ScrewTurn.Wiki.PluginFramework;
using ScrewTurn.Wiki.Plugins.SqlCommon;
using ScrewTurn.Wiki.Plugins.SqlServer.Properties;

namespace ScrewTurn.Wiki.Plugins.SqlServer
{
    /// <summary>
    ///     Implements a SQL Server-based users storage provider.
    /// </summary>
    public class SqlServerUsersStorageProvider : SqlUsersStorageProviderBase, IUsersStorageProviderV30
    {
        private const int CurrentSchemaVersion = 3000;
        private readonly SqlServerCommandBuilder commandBuilder = new SqlServerCommandBuilder();

        /// <summary>
        ///     Gets the Information about the Provider.
        /// </summary>
        public override ComponentInformation Information { get; } =
            new ComponentInformation("SQL Server Users Storage Provider", "Threeplicate Srl", "3.0.1.471",
                "http://www.screwturn.eu", "http://www.screwturn.eu/Version/SQLServerProv/Users.txt");

        /// <summary>
        ///     Gets a brief summary of the configuration string format, in HTML. Returns <c>null</c> if no configuration is
        ///     needed.
        /// </summary>
        public override string ConfigHelpHtml
        {
            get
            {
                return
                    "Connection string format:<br /><code>Data Source=<i>Database Address and Instance</i>;Initial Catalog=<i>Database name</i>;User ID=<i>login</i>;Password=<i>password</i>;</code>";
            }
        }

        /// <summary>
        ///     Gets a new command with an open connection.
        /// </summary>
        /// <param name="connString">The connection string.</param>
        /// <returns>The command.</returns>
        private SqlCommand GetCommand(string connString)
        {
            return commandBuilder.GetCommand(connString, "select current_user", new List<Parameter>()) as SqlCommand;
        }

        /// <summary>
        /// Gets a command builder object.
        /// </summary>
        /// <returns>The command builder.</returns>
        protected override ICommandBuilder CommandBuilder => commandBuilder;

        /// <summary>
        ///     Validates a connection string.
        /// </summary>
        /// <param name="connString">The connection string to validate.</param>
        /// <remarks>If the connection string is invalid, the method throws <see cref="T:InvalidConfigurationException" />.</remarks>
        protected override void ValidateConnectionString(string connString)
        {
            SqlCommand cmd = null;
            try
            {
                cmd = GetCommand(connString);
            }
            catch (SqlException ex)
            {
                throw new InvalidConfigurationException("Provided connection string is not valid", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidConfigurationException("Provided connection string is not valid", ex);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidConfigurationException("Provided connection string is not valid", ex);
            }
            finally
            {
                try
                {
                    cmd.Connection.Close();
                }
                catch
                {
                }
            }
        }

        /// <summary>
        ///     Detects whether the database schema exists.
        /// </summary>
        /// <returns><c>true</c> if the schema exists, <c>false</c> otherwise.</returns>
        private bool SchemaExists()
        {
            var cmd = GetCommand(connString);
            cmd.CommandText = "select [Version] from [Version] where [Component] = 'Users'";

            var exists = false;

            try
            {
                var version = ExecuteScalar(cmd, -1);
                if (version > CurrentSchemaVersion)
                    throw new InvalidConfigurationException(
                        "The version of the database schema is greater than the supported version");
                exists = version != -1;
            }
            catch (SqlException)
            {
                exists = false;
            }
            finally
            {
                try
                {
                    cmd.Connection.Close();
                }
                catch
                {
                }
            }

            return exists;
        }

        /// <summary>
        ///     Detects whether the database schema needs to be updated.
        /// </summary>
        /// <returns><c>true</c> if an update is needed, <c>false</c> otherwise.</returns>
        private bool SchemaNeedsUpdate()
        {
            var cmd = GetCommand(connString);
            cmd.CommandText = "select [Version] from [Version] where [Component] = 'Users'";

            var exists = false;

            try
            {
                var version = ExecuteScalar(cmd, -1);
                exists = version < CurrentSchemaVersion;
            }
            catch (SqlException)
            {
                exists = false;
            }
            finally
            {
                try
                {
                    cmd.Connection.Close();
                }
                catch
                {
                }
            }

            return exists;
        }

        /// <summary>
        ///     Creates the standard database schema.
        /// </summary>
        private void CreateStandardSchema()
        {
            var cmd = GetCommand(connString);
            cmd.CommandText = Resources.UsersDatabase;

            cmd.ExecuteNonQuery();

            cmd.Connection.Close();
        }

        /// <summary>
        ///     Creates or updates the database schema if necessary.
        /// </summary>
        protected override void CreateOrUpdateDatabaseIfNecessary()
        {
            if (!SchemaExists())
            {
                // Verify if an upgrade from version 2.0 is possible
                if (SchemaAllowsUpgradeFrom20())
                {
                    UpgradeFrom20();
                }
                else
                {
                    // If not, create the standard schema
                    CreateStandardSchema();
                }
            }
            if (SchemaNeedsUpdate())
            {
                // Run minor update batches...
            }
        }

        /// <summary>
        ///     Detects whether an upgrade is possible from version 2.0.
        /// </summary>
        /// <returns><c>true</c> if the upgrade is possible, <c>false</c> otherwise.</returns>
        private bool SchemaAllowsUpgradeFrom20()
        {
            // Look for 'UsersProviderVersion' tables
            var cmd = GetCommand(connString);
            cmd.CommandText = "select count(*) from sys.tables where [name] = 'UsersProviderVersion'";

            var count = ExecuteScalar(cmd, -1);

            return count == 1;
        }

        /// <summary>
        ///     Upgrades the database schema and data from version 2.0.
        /// </summary>
        private void UpgradeFrom20()
        {
            // 1. Load all user data in memory
            // 2. Rename old tables so they won't get in the way but the can still be recovered (_v2)
            // 3. Create new schema
            // 4. Add new users and default groups (admins, users)

            var cmd = GetCommand(connString);
            cmd.CommandText = "select * from [User]";

            var reader = cmd.ExecuteReader();

            var administratorsGroup = host.GetSettingValue(SettingName.AdministratorsGroup);
            var usersGroup = host.GetSettingValue(SettingName.UsersGroup);

            var newUsers = new List<UserInfo>(100);
            var passwordHashes = new List<string>(100);

            while (reader.Read())
            {
                var username = reader["Username"] as string;
                var passwordHash = reader["PasswordHash"] as string;
                var email = reader["Email"] as string;
                var dateTime = (DateTime)reader["DateTime"];
                var active = (bool)reader["Active"];
                var admin = (bool)reader["Admin"];

                var temp = new UserInfo(username, null, email, active, dateTime, this);
                temp.Groups = admin ? new[] { administratorsGroup } : new[] { usersGroup };
                newUsers.Add(temp);
                passwordHashes.Add(passwordHash);
            }

            reader.Close();
            cmd.Connection.Close();

            cmd = GetCommand(connString);
            cmd.CommandText =
                "exec sp_rename 'UsersProviderVersion', 'UsersProviderVersion_v2'; exec sp_rename 'User', 'User_v2';";
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

            CreateStandardSchema();

            var admins = AddUserGroup(administratorsGroup, "Built-in Administrators");
            var users = AddUserGroup(usersGroup, "Built-in Users");

            for (var i = 0; i < newUsers.Count; i++)
            {
                cmd = GetCommand(connString);
                cmd.CommandText =
                    "insert into [User] ([Username], [PasswordHash], [Email], [Active], [DateTime]) values (@Username, @PasswordHash, @Email, @Active, @DateTime)";
                cmd.Parameters.Add(new SqlParameter("@Username", newUsers[i].Username));
                cmd.Parameters.Add(new SqlParameter("@PasswordHash", passwordHashes[i]));
                cmd.Parameters.Add(new SqlParameter("@Email", newUsers[i].Email));
                cmd.Parameters.Add(new SqlParameter("@Active", newUsers[i].Active));
                cmd.Parameters.Add(new SqlParameter("@DateTime", newUsers[i].DateTime));

                cmd.ExecuteNonQuery();
                cmd.Connection.Close();

                SetUserMembership(newUsers[i], newUsers[i].Groups);
            }

            host.UpgradeSecurityFlagsToGroupsAcl(admins, users);
        }

        /// <summary>
        ///     Tries to load the configuration from a corresponding v2 provider.
        /// </summary>
        /// <returns>The configuration, or an empty string.</returns>
        protected override string TryLoadV2Configuration()
        {
            return host.GetProviderConfiguration("ScrewTurn.Wiki.PluginPack.SqlServerUsersStorageProvider");
        }

        /// <summary>
        ///     Tries to load the configuration of the corresponding settings storage provider.
        /// </summary>
        /// <returns>The configuration, or an empty string.</returns>
        protected override string TryLoadSettingsStorageProviderConfiguration()
        {
            return host.GetProviderConfiguration(typeof(SqlServerSettingsStorageProvider).FullName);
        }
    }
}