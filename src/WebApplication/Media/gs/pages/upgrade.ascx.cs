using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.ErrorHandler.CustomExceptions;
using GalleryServerPro.Web.Entity;
using GalleryServerPro.Web.Controller;
using GalleryServerPro.Web.Sql;
using DataException = System.Data.DataException;

namespace GalleryServerPro.Web.Pages
{

	public partial class upgrade : System.Web.UI.UserControl
	{
		#region Member classes

		private class DatabaseUpgrader
		{
			#region Private Fields

			private ProviderDataStore _dataProvider = ProviderDataStore.Unknown;
			private readonly string _gspConfigPath;
			private string _dbVersion;
			private int _galleryId = int.MinValue;
			private Type _sqliteControllerType;

			#endregion

			#region Constructors

			public DatabaseUpgrader(string gspConfigPath)
			{
				_gspConfigPath = gspConfigPath;
			}

			#endregion

			#region Public Properties

			private int GalleryId
			{
				get
				{
					if (_galleryId == int.MinValue)
					{
						// Get it from galleryserverpro.config
						GspConfigImporter gspCfg = new GspConfigImporter(_gspConfigPath, this);
						_galleryId = gspCfg.GalleryId;
					}

					return _galleryId;
				}
			}

			/// <summary>
			/// Gets the version of the database that is required by the current application.
			/// </summary>
			/// <value>The version of the database that is required by the current application.</value>
			public GalleryDataSchemaVersion DataSchemaVersionRequiredByApp
			{
				get
				{
					return GalleryDataSchemaVersion.V2_4_6;
				}
			}

			/// <summary>
			/// Gets the connection string to the SQLite database.
			/// </summary>
			/// <value>The SQLite database connection string.</value>
			public string SQLiteConnectionString
			{
				get
				{
					WebConfigEntity webConfig = WebConfigController.GetWebConfigEntity();
					return webConfig.SQLiteConnectionStringValue;
				}
			}

			/// <summary>
			/// Gets the connection string to the SQL Server database.
			/// </summary>
			/// <value>The SQL Server database connection string.</value>
			public string SqlServerConnectionString
			{
				get
				{
					WebConfigEntity webConfig = WebConfigController.GetWebConfigEntity();
					return webConfig.SqlServerConnectionStringValue;
				}
			}

			/// <summary>
			/// Gets the database technology used to store the gallery data. Examples: SQLite, SqlServer
			/// </summary>
			/// <value>The database technology used to store the gallery data.</value>
			public ProviderDataStore DataProvider
			{
				get
				{
					WebConfigEntity webConfig = null;
					if (_dataProvider == ProviderDataStore.Unknown)
					{
						webConfig = WebConfigController.GetWebConfigEntity();
						_dataProvider = webConfig.DataProvider;
					}

					if (_dataProvider == ProviderDataStore.Unknown)
					{
						// Pre-2.4 versions have the gallery data provider specified in galleryserverpro.config, not web.config.
						// In these cases, we need to get it from there instead.
						try
						{
							GspConfigImporter gspCfg = new GspConfigImporter(_gspConfigPath, this);
							switch (gspCfg.GalleryDataProvider)
							{
								case GalleryDataProvider.SQLiteGalleryServerProProvider: _dataProvider = ProviderDataStore.SQLite; break;
								case GalleryDataProvider.SqlServerGalleryServerProProvider: _dataProvider = ProviderDataStore.SqlServer; break;
							}
						}
						catch (FileNotFoundException) { }
					}

					if (_dataProvider == ProviderDataStore.Unknown)
					{
						// Can't find galleryserverpro.config. Let's just use the database associated with the membership provider.
						if (webConfig != null)
						{
							switch (webConfig.MembershipDefaultProvider)
							{
								case MembershipDataProvider.SQLiteMembershipProvider:
									_dataProvider = ProviderDataStore.SQLite;
									break;
								case MembershipDataProvider.SqlMembershipProvider:
									_dataProvider = ProviderDataStore.SqlServer;
									break;
							}
						}
					}

					return _dataProvider;
				}
			}

			/// <summary>
			/// Gets a value indicating whether the current application requires a data schema that is newer than which exists 
			/// in the database.
			/// </summary>
			/// <value>
			/// 	<c>true</c> if an upgrade is required; otherwise, <c>false</c>.
			/// </value>
			public bool IsUpgradeRequired
			{
				get
				{
					return DataSchemaVersionRequiredByApp > GetDatabaseVersion();
				}
			}

			/// <summary>
			/// Gets a value indicating whether the database can be automatically upgraded to the version required by the application.
			/// </summary>
			/// <value>
			/// 	<c>true</c> if the database can be automatically upgraded; otherwise, <c>false</c>.
			/// </value>
			public bool IsAutoUpgradeSupported
			{
				get
				{
					return GetDatabaseVersion() == GalleryDataSchemaVersion.V2_3_3421;
				}
			}

			/// <summary>
			/// Gets the reason the database cannot be automatically upgraded. Returns <see cref="String.Empty" /> if 
			/// <see cref="IsAutoUpgradeSupported" /> is <c>true</c>.
			/// </summary>
			/// <value>The reason the database cannot be automatically upgraded..</value>
			public string AutoUpgradeNotSupportedReason
			{
				get
				{
					if (!IsAutoUpgradeSupported)
					{
						return string.Format("The Upgrade Wizard can only upgrade a database from version 2.3, but the database is at version {0}. Upgrade your gallery to 2.3.3750 and try again. If your current version is 2.4.1 or higher, than the Upgrade Wizard is not needed.", GetDataSchemaVersionString() ?? "<unknown>");
					}
					else
					{
						return String.Empty;
					}
				}
			}

			private Type SQLiteControllerType
			{
				get
				{
					if (_sqliteControllerType == null)
					{
						System.Reflection.Assembly assembly = System.Reflection.Assembly.Load("GalleryServerPro.Data.SQLite");

						// Get reference to static SQLiteController class.
						_sqliteControllerType = assembly.GetType("GalleryServerPro.Data.SQLite.SQLiteController");

						if (_sqliteControllerType == null)
						{
							throw new DataException("GalleryServerPro.Data.SQLite.dll does not contain the class \"SQLiteController\". This class is present in 2.4 and later versions.");
						}
					}

					return _sqliteControllerType;
				}
			}
			#endregion

			#region Public Methods

			/// <summary>
			/// Gets the database version as reported by the database. Returns <see cref="GalleryDataSchemaVersion.Unknown" />
			/// if the value in the database cannot be found or parsed into one of the enum values.
			/// </summary>
			/// <returns>A <see cref="GalleryDataSchemaVersion" /> instance.</returns>
			public GalleryDataSchemaVersion GetDatabaseVersion()
			{
				if (String.IsNullOrEmpty(_dbVersion))
				{
					_dbVersion = GetDatabaseVersionString();
				}

				return GalleryDataSchemaVersionEnumHelper.ConvertGalleryDataSchemaVersionToEnum(_dbVersion);
			}

			/// <summary>
			/// Gets the database version as reported by the database. Examples: "2.3.3421", "2.4.1" Returns null
			/// when the version cannot be found in the database.
			/// </summary>
			/// <returns>A <see cref="string" /> instance.</returns>
			public string GetDatabaseVersionString()
			{
				return GetDataSchemaVersionString();
			}

			/// <summary>
			/// Upgrades the database to the current version. No action is taken if an upgrade is not required or not possible.
			/// </summary>
			public void Upgrade()
			{
				if (IsUpgradeRequired && IsAutoUpgradeSupported)
				{
					UpgradeDatabaseSchema();
				}
			}

			public void ConfigureGallery(int galleryId)
			{
				// Excecute gs_GalleryConfig for SQL Server and equivalent for SQLite.
				_galleryId = galleryId;

				switch (DataProvider)
				{
					case ProviderDataStore.SQLite:
						ExecuteSQLiteGalleryConfig();
						break;
					case ProviderDataStore.SqlServer:
						ExecuteSqlServerProcGalleryConfig();
						break;
					default:
						throw new System.ComponentModel.InvalidEnumArgumentException(string.Format("The function ConfigureGallery is not able to handle an enum value ProviderDataStore.{0}.", DataProvider));
				}
			}

			public void UpdateAppSetting(string settingName, string settingValue)
			{
				switch (DataProvider)
				{
					case ProviderDataStore.SQLite:
						UpdateAppSettingSQLite(settingName, settingValue);
						break;
					case ProviderDataStore.SqlServer:
						UpdateAppSettingSqlServer(settingName, settingValue);
						break;
				}
			}

			private void UpdateAppSettingSqlServer(string settingName, string settingValue)
			{
				if (_galleryId == int.MinValue)
				{
					throw new InvalidOperationException("The function ConfigureGallery() must be invoked before UpdateAppSetting().");
				}

				using (SqlConnection cn = new SqlConnection(SqlServerConnectionString))
				{
					using (SqlCommand cmd = cn.CreateCommand())
					{
						cmd.CommandText = SqlServerHelper.GetSqlName("gs_AppSettingUpdate");
						cmd.CommandType = CommandType.StoredProcedure;

						// Add parameters
						cmd.Parameters.Add(new SqlParameter("@SettingName", SqlDbType.NVarChar, DataConstants.SettingNameLength));
						cmd.Parameters.Add(new SqlParameter("@SettingValue", SqlDbType.NVarChar, DataConstants.SettingValueLength));

						cmd.Parameters["@SettingName"].Value = settingName;
						cmd.Parameters["@SettingValue"].Value = settingValue;

						cmd.Connection.Open();
						int numRecords = cmd.ExecuteNonQuery();

						if (numRecords != 1)
						{
							if (numRecords < 0)
								numRecords = 0;

							throw new ErrorHandler.CustomExceptions.DataException(string.Format("Expected to update one record in gs_AppSetting, but instead {0} records were updated. Setting name=\"{1}\"; Setting value=\"{2}\"", numRecords, settingName, settingValue));
						}
					}
				}
			}

			private void UpdateAppSettingSQLite(string settingName, string settingValue)
			{
				Type[] parmTypes = new Type[3];
				parmTypes[0] = typeof(string);
				parmTypes[1] = typeof(string);
				parmTypes[2] = typeof(string);

				System.Reflection.MethodInfo updateAppSettingMethod = SQLiteControllerType.GetMethod("UpdateAppSetting", parmTypes);

				object[] parameters = new object[3];
				parameters[0] = settingName;
				parameters[1] = settingValue;
				parameters[2] = SQLiteConnectionString;

				updateAppSettingMethod.Invoke(null, parameters);
			}

			public void UpdateGallerySetting(string settingName, string settingValue)
			{
				switch (DataProvider)
				{
					case ProviderDataStore.SQLite:
						UpdateGallerySettingSQLite(settingName, settingValue);
						break;
					case ProviderDataStore.SqlServer:
						UpdateGallerySettingSqlServer(settingName, settingValue);
						break;
				}
			}

			private void UpdateGallerySettingSqlServer(string settingName, string settingValue)
			{
				if (_galleryId == int.MinValue)
				{
					throw new InvalidOperationException("The function ConfigureGallery() must be invoked before UpdateGallerySetting().");
				}

				#region Check for setting emailToAddress. Look up the username and store it in the setting UsersToNotifyWhenErrorOccurs.
				if (settingName.Equals("emailToAddress", StringComparison.OrdinalIgnoreCase))
				{
					string userName = System.Web.Security.Membership.GetUserNameByEmail(settingValue);

					if (String.IsNullOrEmpty(userName))
					{
						return;
					}

					settingName = "UsersToNotifyWhenErrorOccurs";
					settingValue = userName;
				}
				#endregion

				using (SqlConnection cn = new SqlConnection(SqlServerConnectionString))
				{
					using (SqlCommand cmd = cn.CreateCommand())
					{
						cmd.CommandText = SqlServerHelper.GetSqlName("gs_GallerySettingUpdate");
						cmd.CommandType = CommandType.StoredProcedure;

						// Add parameters
						cmd.Parameters.Add(new SqlParameter("@GalleryId", SqlDbType.Int));
						cmd.Parameters.Add(new SqlParameter("@SettingName", SqlDbType.NVarChar, DataConstants.SettingNameLength));
						cmd.Parameters.Add(new SqlParameter("@SettingValue", SqlDbType.NVarChar, DataConstants.SettingValueLength));

						cmd.Parameters["@GalleryId"].Value = _galleryId;
						cmd.Parameters["@SettingName"].Value = settingName;
						cmd.Parameters["@SettingValue"].Value = settingValue;

						cmd.Connection.Open();
						int numRecords = cmd.ExecuteNonQuery();

						if (numRecords != 1)
						{
							if (numRecords < 0)
								numRecords = 0;

							throw new ErrorHandler.CustomExceptions.DataException(string.Format("Expected to update one record in gs_GallerySetting, but instead {0} records were updated. Setting name=\"{1}\"; Setting value=\"{2}\"", numRecords, settingName, settingValue));
						}
					}
				}
			}

			private void UpdateGallerySettingSQLite(string settingName, string settingValue)
			{
				#region Check for setting emailToAddress. Look up the username and store it in the setting UsersToNotifyWhenErrorOccurs.
				if (settingName.Equals("emailToAddress", StringComparison.OrdinalIgnoreCase))
				{
					string userName = System.Web.Security.Membership.GetUserNameByEmail(settingValue);

					if (String.IsNullOrEmpty(userName))
					{
						return;
					}

					settingName = "UsersToNotifyWhenErrorOccurs";
					settingValue = userName;
				}
				#endregion

				Type[] parmTypes = new Type[4];
				parmTypes[0] = typeof(int);
				parmTypes[1] = typeof(string);
				parmTypes[2] = typeof(string);
				parmTypes[3] = typeof(string);

				System.Reflection.MethodInfo updateGallerySettingMethod = SQLiteControllerType.GetMethod("UpdateGallerySetting", parmTypes);

				object[] parameters = new object[4];
				parameters[0] = _galleryId;
				parameters[1] = settingName;
				parameters[2] = settingValue;
				parameters[3] = SQLiteConnectionString;

				updateGallerySettingMethod.Invoke(null, parameters);
			}

			public void UpdateMimeTypeGallery(int mimeTypeGalleryId, bool isEnabled)
			{
				switch (DataProvider)
				{
					case ProviderDataStore.SQLite:
						UpdateMimeTypeGallerySQLite(mimeTypeGalleryId, isEnabled);
						break;
					case ProviderDataStore.SqlServer:
						UpdateMimeTypeGallerySqlServer(mimeTypeGalleryId, isEnabled);
						break;
				}
			}

			private void UpdateMimeTypeGallerySqlServer(int mimeTypeGalleryId, bool isEnabled)
			{
				using (SqlConnection cn = new SqlConnection(SqlServerConnectionString))
				{
					using (SqlCommand cmd = cn.CreateCommand())
					{
						cmd.CommandText = SqlServerHelper.GetSqlName("gs_MimeTypeGalleryUpdate");
						cmd.CommandType = CommandType.StoredProcedure;

						// Add parameters
						cmd.Parameters.Add(new SqlParameter("@MimeTypeGalleryId", SqlDbType.Int));
						cmd.Parameters.Add(new SqlParameter("@IsEnabled", SqlDbType.Bit));

						cmd.Parameters["@MimeTypeGalleryId"].Value = mimeTypeGalleryId;
						cmd.Parameters["@IsEnabled"].Value = isEnabled;

						cmd.Connection.Open();
						int numRecords = cmd.ExecuteNonQuery();

						if (numRecords != 1)
						{
							if (numRecords < 0)
								numRecords = 0;

							throw new ErrorHandler.CustomExceptions.DataException(string.Format("Expected to update one record in gs_MimeTypeGallery, but instead {0} records were updated. MimeTypeGalleryId={1}; IsEnabled={2}", numRecords, mimeTypeGalleryId, isEnabled));
						}
					}
				}
			}

			private void UpdateMimeTypeGallerySQLite(int mimeTypeGalleryId, bool isEnabled)
			{
				Type[] parmTypes = new Type[3];
				parmTypes[0] = typeof(int);
				parmTypes[1] = typeof(bool);
				parmTypes[2] = typeof(string);

				System.Reflection.MethodInfo updateMimeTypeGallerySettingMethod = SQLiteControllerType.GetMethod("UpdateMimeTypeGallerySetting", parmTypes);

				object[] parameters = new object[3];
				parameters[0] = mimeTypeGalleryId;
				parameters[1] = isEnabled;
				parameters[2] = SQLiteConnectionString;

				updateMimeTypeGallerySettingMethod.Invoke(null, parameters);
			}

			public Dictionary<string, int> GetMimeTypeLookupValues(int galleryId)
			{
				switch (DataProvider)
				{
					case ProviderDataStore.SQLite:
						return GetMimeTypeLookupValuesSQLite(galleryId);
					case ProviderDataStore.SqlServer:
						return GetMimeTypeLookupValuesSqlServer(galleryId);
					default:
						throw new System.ComponentModel.InvalidEnumArgumentException(string.Format("This function does not support the enum value {0}.", DataProvider));
				}
			}

			/// <summary>
			/// Gets a collection of file extensions (ex: ".avi") and their MimeTypGalleryeIds from the database for the specified gallery.
			/// </summary>
			private Dictionary<string, int> GetMimeTypeLookupValuesSqlServer(int galleryId)
			{
				Dictionary<string, int> mimeTypeLookup = new Dictionary<string, int>();

				using (SqlConnection cn = new SqlConnection(SqlServerConnectionString))
				{
					using (SqlCommand cmd = cn.CreateCommand())
					{
						cmd.CommandText = SqlServerHelper.GetSqlName("gs_MimeTypeGallerySelect");
						cmd.CommandType = CommandType.StoredProcedure;

						cmd.Connection.Open();
						using (IDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
						{
							while (dr.Read())
							{
								if (dr.GetInt32(1) == galleryId)
								{
									mimeTypeLookup.Add(dr.GetString(2), dr.GetInt32(0));
								}
							}
						}
					}
				}

				return mimeTypeLookup;
			}

			private Dictionary<string, int> GetMimeTypeLookupValuesSQLite(int galleryId)
			{
				Type[] parmTypes = new Type[2];
				parmTypes[0] = typeof(int);
				parmTypes[1] = typeof(string);

				System.Reflection.MethodInfo updateMimeTypeGallerySettingMethod = SQLiteControllerType.GetMethod("GetMimeTypeLookupValues", parmTypes);

				object[] parameters = new object[2];
				parameters[0] = galleryId;
				parameters[1] = SQLiteConnectionString;

				return (Dictionary<string, int>)updateMimeTypeGallerySettingMethod.Invoke(null, parameters);
			}

			#endregion

			#region Private Functions

			private void UpgradeDatabaseSchema()
			{
				switch (DataProvider)
				{
					case ProviderDataStore.SQLite:
						UpgradeSQLiteTo_2_4_6();
						break;
					case ProviderDataStore.SqlServer:
						SqlServerHelper sqlHelper = new SqlServerHelper(SqlServerConnectionString, GalleryId);
						sqlHelper.ExecuteSqlInFile("Upgrade_2_3_3421_to_2_4_6.sql");
						break;
				}
			}

			/// <summary>
			/// Upgrades the SQLite database to version 2.4.6.
			/// </summary>
			/// <returns>Returns a <see cref="string" />.</returns>
			private void UpgradeSQLiteTo_2_4_6()
			{
				if (DataProvider != ProviderDataStore.SQLite)
				{
					return;
				}

				Type[] parmTypes = new Type[1];
				parmTypes[0] = typeof(string);

				System.Reflection.MethodInfo getDatabaseVersionMethod = SQLiteControllerType.GetMethod("Upgrade", parmTypes);

				object[] parameters = new object[1];
				parameters[0] = SQLiteConnectionString;

				getDatabaseVersionMethod.Invoke(null, parameters);
			}

			/// <summary>
			/// Invoke the SQLite method SQLiteGalleryServerProProvider.ConfigureGallery().
			/// </summary>
			private void ExecuteSQLiteGalleryConfig()
			{
				if (DataProvider != ProviderDataStore.SQLite)
				{
					return;
				}

				Type[] parmTypes = new Type[2];
				parmTypes[0] = typeof(int);
				parmTypes[1] = typeof(string);

				System.Reflection.MethodInfo getConfigureGalleryMethod = SQLiteControllerType.GetMethod("ConfigureGallery", parmTypes);

				object[] parameters = new object[2];
				parameters[0] = _galleryId;
				parameters[1] = SQLiteConnectionString;

				getConfigureGalleryMethod.Invoke(null, parameters);
			}

			/// <summary>
			// Execute the SQL Server stored proc gs_GalleryConfig.
			/// </summary>
			private void ExecuteSqlServerProcGalleryConfig()
			{
				using (SqlConnection cn = new SqlConnection(SqlServerConnectionString))
				{
					using (SqlCommand cmd = cn.CreateCommand())
					{
						cmd.CommandText = SqlServerHelper.GetSqlName("gs_GalleryConfig");
						cmd.CommandType = CommandType.StoredProcedure;

						// Add parameters
						cmd.Parameters.Add(new SqlParameter("@GalleryId", SqlDbType.Int));
						cmd.Parameters.Add(new SqlParameter("@RootAlbumTitle", SqlDbType.NVarChar, DataConstants.AlbumTitleLength));
						cmd.Parameters.Add(new SqlParameter("@RootAlbumSummary", SqlDbType.NVarChar, DataConstants.AlbumSummaryLength));

						cmd.Parameters["@GalleryId"].Value = _galleryId;
						cmd.Parameters["@RootAlbumTitle"].Value = "All albums";
						cmd.Parameters["@RootAlbumSummary"].Value = "Welcome to Gallery Server Pro!";

						cmd.Connection.Open();
						cmd.ExecuteNonQuery();
					}
				}
			}

			/// <summary>
			/// Gets the version of the objects in the database as reported by the database. Example: "2.3.3421"
			/// Returns null if the version cannot be found.
			/// </summary>
			/// <returns>Returns the version of the objects in the database as reported by the database.</returns>
			private string GetDataSchemaVersionString()
			{
				switch (DataProvider)
				{
					case ProviderDataStore.SQLite:
						return GetSQLiteDatabaseVersion();
					case ProviderDataStore.SqlServer:
						return GetSqlServerDatabaseVersion();
					default:
						return String.Empty;
				}
			}

			/// <summary>
			/// Gets the version of the data schema in the SQL server database. Examples: "2.3.3421", "2.4.1" Returns null
			/// when the current data store is not SQL Server or the version cannot be found in the database.
			/// </summary>
			/// <returns>Returns the version of the database, or null if not found.</returns>
			private string GetSqlServerDatabaseVersion()
			{
				// 2.3 and earlier stored the version in a user-defined function, so look there first. If it doesn't exist, 
				// look in the app settings table (which is where it is stored in 2.4 and later).
				string version = null;

				if (DataProvider != ProviderDataStore.SqlServer)
				{
					return version;
				}

				using (SqlConnection cn = new SqlConnection(SqlServerConnectionString))
				{
					using (SqlCommand cmd = cn.CreateCommand())
					{
						string sql = String.Concat("SELECT SettingValue FROM ", SqlServerHelper.GetSqlName("gs_AppSetting"), " WHERE SettingName = 'DataSchemaVersion'");
						cmd.CommandText = sql;

						if (cn.State == ConnectionState.Closed)
							cn.Open();

						try
						{
							version = cmd.ExecuteScalar().ToString();
						}
						catch (SqlException) // Will get here if table doesn't exist
						{
							version = GetSqlServerDatabaseVersionDeprecated(cmd);
						}
						catch (NullReferenceException) // Will get here if no matching record is in gs_AppSetting
						{
							version = GetSqlServerDatabaseVersionDeprecated(cmd);
						}
					}
				}

				return version;
			}

			/// <summary>
			/// Gets the version of the data schema in the SQL server database as it was stored in 2.3 and earlier versions.
			/// Examples: "2.3.3421", "2.4.1"
			/// </summary>
			/// <param name="cmd">The SqlCommand to use to query the database. It must have an attached, open connection.</param>
			/// <returns>Returns the version of the database, or null if not found.</returns>
			private static string GetSqlServerDatabaseVersionDeprecated(SqlCommand cmd)
			{
				string version = null;
				string sql = String.Concat("SELECT ", SqlServerHelper.GetSqlName("gs_GetVersion"), "() AS SchemaVersion");
				cmd.CommandText = sql;
				try
				{
					version = cmd.ExecuteScalar().ToString();
				}
				catch (SqlException) { }
				catch (NullReferenceException) { }

				return version;
			}

			/// <summary>
			/// Gets the data schema version of the SQLite database. Examples: "2.3.3421", "2.4.1" Returns null
			/// when the current data store is not SQLite or the version cannot be found in the database.
			/// </summary>
			/// <returns>Returns a <see cref="string" />.</returns>
			private string GetSQLiteDatabaseVersion()
			{
				string version = null;

				if (DataProvider != ProviderDataStore.SQLite)
				{
					return version;
				}

				Type[] parmTypes = new Type[1];
				parmTypes[0] = typeof(string);

				System.Reflection.MethodInfo getDatabaseVersionMethod = SQLiteControllerType.GetMethod("GetDatabaseVersion", parmTypes);

				object[] parameters = new object[1];
				parameters[0] = SQLiteConnectionString;

				version = getDatabaseVersionMethod.Invoke(null, parameters).ToString();

				return version;
			}

			#endregion
		}

		/// <summary>
		/// Contains functionality for updating web.config from 2.3 to 2.4. The following changes are made:
		/// (1) The galleryServerPro section is updated.
		/// </summary>
		private class WebConfigUpdater
		{
			#region Private Fields

			private readonly WebConfigEntity _webConfig;
			private const string GspConfigDefault = @"
		<galleryServerPro>
			<core galleryResourcesPath=""{GalleryResourcePath}"" />
			<dataProvider defaultProvider=""{Unknown}"">
				<providers>
					<clear />
					{SQLiteGspProvider}
					{SqlServerGspProvider}
				</providers>
			</dataProvider>
		</galleryServerPro>
";
			private const string SQLiteGspProviderDefault = @"<add applicationName=""Gallery Server Pro"" connectionStringName=""SQLiteDbConnection"" name=""SQLiteGalleryServerProProvider"" type=""GalleryServerPro.Data.SQLite.SQLiteGalleryServerProProvider"" />";
			private const string SqlServerGspProviderDefault = @"<add applicationName=""Gallery Server Pro"" connectionStringName=""SqlServerDbConnection"" name=""SqlServerGalleryServerProProvider"" type=""GalleryServerPro.Data.SqlServer.SqlDataProvider"" />";

			private readonly bool _upgradeRequired;

			#endregion

			#region Constructors

			/// <summary>
			/// Initializes a new instance of the <see cref="WebConfigUpdater"/> class.
			/// </summary>
			public WebConfigUpdater()
			{
				_webConfig = WebConfigController.GetWebConfigEntity();

				this._upgradeRequired = IsUpgradeRequired();
			}

			#endregion

			#region Public Properties

			public bool UpgradeRequired
			{
				get { return this._upgradeRequired; }
			}

			public bool IsWriteable
			{
				get { return this._webConfig.IsWriteable; }
			}

			#endregion

			#region Public Methods

			/// <summary>
			/// Update web.config with the connection strings and provider names for membership, roles, and profile.
			/// </summary>
			public void Upgrade()
			{
				if (UpgradeRequired)
				{
					UpgradeGalleryServerProConfigSection();
				}
			}

			/// <summary>
			/// Determines whether any settings in the current web.config must be modified to satisfy 2.4 requirements. Specifically, it checks to 
			/// see if the galleryserverpro section has been updated to the new syntax.
			/// </summary>
			/// <returns>Returns <c>true</c> if the file must be updated; otherwise returns <c>false</c>.</returns>
			private bool IsUpgradeRequired()
			{
				// 2.3 will have: <galleryServerPro configSource="gs\config\galleryserverpro.config" />
				return _webConfig.GalleryServerProConfigSection.StartsWith("<galleryServerPro configSource=");
			}

			#endregion

			#region Private Methods

			/// <summary>
			/// Update web.config with the galleryServerPro section.
			/// </summary>
			private void UpgradeGalleryServerProConfigSection()
			{
				string gspConfigSection = GspConfigDefault.Replace("{GalleryResourcePath}", Util.GalleryResourcesPath);

				// Include the SQLite and/or SQL Server provider definitions.
				gspConfigSection = gspConfigSection.Replace("{SQLiteGspProvider}", String.IsNullOrEmpty(_webConfig.SQLiteConnectionStringValue) ? String.Empty : SQLiteGspProviderDefault);
				gspConfigSection = gspConfigSection.Replace("{SqlServerGspProvider}", String.IsNullOrEmpty(_webConfig.SqlServerConnectionStringValue) ? String.Empty : SqlServerGspProviderDefault);

				//_webConfig.GalleryDataDefaultProvider = // Can't update now since we don't know it yet. We'll set it later after importing galleryserverpro.config
				_webConfig.GalleryServerProConfigSection = gspConfigSection;

				WebConfigController.Save(_webConfig);
			}

			#endregion
		}

		/// <summary>
		/// Contains functionality for importing settings from a previous version of galleryserverpro.config to the 
		/// current one. Only the gallery data provider name and the values in the &lt;core ...&gt; section are imported.
		/// </summary>
		private class GspConfigImporter
		{
			#region Member Classes

			private class MimeTypeEntity
			{
				#region Private Fields

				private readonly string _extension;
				private readonly string _browserId;
				private readonly string _fullMimeType;
				private readonly string _browserMimeType;
				private readonly bool _allowAddToGallery;

				#endregion

				#region Constructors

				/// <summary>
				/// Initializes a new instance of the <see cref="MimeType"/> class.
				/// </summary>
				/// <param name="extension">A string representing the file's extension, including the period (e.g. ".jpg", ".avi").
				/// It is not case sensitive.</param>
				/// <param name="fullMimeType">The full mime type (e.g. image/jpeg, video/quicktime).</param>
				/// <param name="browserId">The id of the browser for the default browser as specified in the .Net Framework's browser definition file. 
				/// This should always be the string "default", which means it will match all browsers. Once this instance is created, additional
				/// values that specify more specific browsers or browser families can be added to the private _browserMimeTypes member variable.</param>
				/// <param name="browserMimeType">The MIME type that can be understood by the browser for displaying this media object. The value will be applied
				/// to the browser specified in <paramref name="browserId"/>. Specify null or <see cref="String.Empty" /> if the MIME type appropriate for the 
				/// browser is the same as <paramref name="fullMimeType"/>.</param>
				/// <param name="allowAddToGallery">Indicates whether a file having this MIME type can be added to Gallery Server Pro.</param>
				public MimeTypeEntity(string extension, string fullMimeType, string browserId, string browserMimeType, bool allowAddToGallery)
				{
					this._extension = extension;
					this._browserId = browserId;
					this._fullMimeType = fullMimeType;
					this._browserMimeType = browserMimeType;
					this._allowAddToGallery = allowAddToGallery;
				}

				#endregion

				#region Properties

				/// <summary>
				/// Gets the file extension this mime type is associated with.
				/// </summary>
				/// <value>The file extension this mime type is associated with.</value>
				public string Extension
				{
					get
					{
						return this._extension;
					}
				}

				/// <summary>
				/// Gets the id of the browser for which the <see cref="BrowserMimeType" /> property applies.
				/// </summary>
				/// <value>
				/// The id of the browser for which the <see cref="BrowserMimeType" /> property applies.
				/// </value>
				public string BrowserId
				{
					get
					{
						return this._browserId;
					}
				}

				/// <summary>
				/// Gets the full mime type (e.g. image/jpeg, video/quicktime).
				/// </summary>
				/// <value>The full mime type.</value>
				public string FullMimeType
				{
					get
					{
						return this._fullMimeType;
					}
				}

				/// <summary>
				/// Gets the MIME type that can be understood by the browser for displaying this media object.
				/// </summary>
				/// <value>
				/// The MIME type that can be understood by the browser for displaying this media object.
				/// </value>
				public string BrowserMimeType
				{
					get
					{
						return this._browserMimeType;
					}
				}

				/// <summary>
				/// Gets a value indicating whether objects of this MIME type can be added to Gallery Server Pro.
				/// </summary>
				/// <value>
				/// 	<c>true</c> if objects of this MIME type can be added to Gallery Server Pro; otherwise, <c>false</c>.
				/// </value>
				public bool AllowAddToGallery
				{
					get
					{
						return this._allowAddToGallery;
					}
				}

				#endregion

			}

			#endregion

			#region Private Fields

			private readonly string _sourceConfigPath;
			private GalleryDataProvider _galleryDataProvider;
			private readonly Dictionary<string, string> _coreValues = new Dictionary<string, string>();
			private readonly IList<MimeTypeEntity> _mimeTypes = new List<MimeTypeEntity>();
			private static readonly List<String> _deletedCoreAttributes = new List<String>();
			private readonly DatabaseUpgrader _dbUpgrader;
			private static readonly Dictionary<string, string> _renamedCoreConfigItems = GetRenamedCoreConfigItems();

			#endregion

			public GalleryDataProvider GalleryDataProvider
			{
				get { return _galleryDataProvider; }
			}

			public int GalleryId
			{
				get { return Convert.ToInt32(this._coreValues["galleryId"]); }
			}

			#region Constructors

			static GspConfigImporter()
			{
				_deletedCoreAttributes.Add("websiteTitle");
				_deletedCoreAttributes.Add("defaultAlbumDirectoryName");
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="GspConfigImporter"/> class.
			/// </summary>
			/// <param name="sourceConfigPath">The full path to the galleryserverpro.config file containing the source data.
			/// 	Ex: C:\inetpub\wwwroot\gallery\gs\config\galleryserverpro_old.config</param>
			/// <param name="databaseUpgrader"></param>
			public GspConfigImporter(string sourceConfigPath, DatabaseUpgrader databaseUpgrader)
			{
				this._sourceConfigPath = sourceConfigPath;
				this._dbUpgrader = databaseUpgrader;

				ExtractConfigData();
			}

			#endregion

			#region Public Methods

			/// <summary>
			/// Import data from galleryserverpro.config to the database tables. Returns the gallery ID.
			/// </summary>
			public int Import()
			{
				ImportConfigData();

				return Convert.ToInt32(_coreValues["galleryId"]);
			}

			#endregion

			#region Private Methods


			/// <summary>
			/// Extracts configuration settings from the source galleryserverpro.config file and stores them in member variables.
			/// </summary>
			private void ExtractConfigData()
			{
				using (FileStream fs = new FileStream(_sourceConfigPath, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					using (StreamReader sr = new StreamReader(fs))
					{
						XmlReader r = XmlReader.Create(sr);
						while (r.Read())
						{
							if (r.Name == "core")
							{
								// Get core attributes.
								while (r.MoveToNextAttribute())
								{
									if (!_deletedCoreAttributes.Contains(r.Name))
									{
										this._coreValues.Add(r.Name, r.Value);
									}
								}
							}

							else if (r.Name == "mimeTypes")
							{
								// Get mime types.
								XmlReader mimeTypes = r.ReadSubtree();

								while (mimeTypes.Read())
								{
									if (mimeTypes.Name == "mimeType")
									{
										// Get fileExtension
										if (!mimeTypes.MoveToAttribute("fileExtension"))
											throw new WebException(String.Format("Could not find fileExtension attribute in mimeType element of {0}.", _sourceConfigPath));

										string fileExtension = mimeTypes.Value;

										// Get browserId
										if (!mimeTypes.MoveToAttribute("browserId"))
											throw new WebException(String.Format("Could not find browserId attribute in mimeType element of {0}. fileExtension={1}", _sourceConfigPath, fileExtension));

										string browserId = mimeTypes.Value;

										// Get type
										if (!mimeTypes.MoveToAttribute("type"))
											throw new WebException(String.Format("Could not find type attribute in mimeType element of {0}. fileExtension={1}", _sourceConfigPath, fileExtension));

										string type = mimeTypes.Value;

										// Get browserMimeType. It is optional.
										string browserMimeType = String.Empty;
										if (mimeTypes.MoveToAttribute("browserMimeType"))
											browserMimeType = mimeTypes.Value;

										// Get allowAddToGallery
										if (!mimeTypes.MoveToAttribute("allowAddToGallery"))
											throw new WebException(String.Format("Could not find allowAddToGallery attribute in mimeType element of {0}. fileExtension={1}", _sourceConfigPath, fileExtension));

										bool allowAddToGallery = Convert.ToBoolean(mimeTypes.Value);

										_mimeTypes.Add(new MimeTypeEntity(fileExtension, type, browserId, browserMimeType, allowAddToGallery));
									}
								}
							}

							else if ((r.Name == "dataProvider") && r.MoveToAttribute("defaultProvider"))
							{
								// Get gallery data provider
								try
								{
									this._galleryDataProvider = (GalleryDataProvider)Enum.Parse(typeof(GalleryDataProvider), r.Value, false);
								}
								catch (ArgumentException) { }
							}
						}
					}
				}
			}

			/// <summary>
			/// Import data from galleryserverpro.config to the database tables.
			/// </summary>
			private void ImportConfigData()
			{
				ImportCoreAttributes();

				ImportMimeTypes();
			}

			private void ImportCoreAttributes()
			{
				// Import the attributes of the <core ...> element to the relevant database tables.
				List<string> appSettingNames = GetAppSettingNames();
				List<string> coreConfigItemsToSkip = GetCoreConfigItemsToSkip();

				foreach (KeyValuePair<string, string> kvp in _coreValues)
				{
					if (kvp.Key.Equals("galleryId", StringComparison.OrdinalIgnoreCase))
					{
						_dbUpgrader.ConfigureGallery(Convert.ToInt32(kvp.Value));
					}

					if (coreConfigItemsToSkip.Contains(kvp.Key))
					{
						continue; // Skip this one
					}

					if (appSettingNames.Contains(kvp.Key))
					{
						_dbUpgrader.UpdateAppSetting(GetDbSettingName(kvp.Key), GetDbSettingValue(kvp.Key, kvp.Value));
					}
					else
					{
						_dbUpgrader.UpdateGallerySetting(GetDbSettingName(kvp.Key), GetDbSettingValue(kvp.Key, kvp.Value));
					}
				}
			}

			private static string GetDbSettingValue(string settingName, string settingValue)
			{
				string value = settingValue;

				if (settingName.Equals("silverlightFileTypes", StringComparison.OrdinalIgnoreCase))
				{
					// Add .mov.
					if (!settingValue.ToLowerInvariant().Contains(".mov"))
					{
						value += ",.mov";
					}
				}

				return value;
			}

			/// <summary>
			/// Update the database to make sure that each MIME type that was enabled in the config file is also enabled in the 
			/// matching database table.
			/// </summary>
			private void ImportMimeTypes()
			{
				int galleryId = Convert.ToInt32(_coreValues["galleryId"]);

				Dictionary<string, int> mimeTypeLookup = _dbUpgrader.GetMimeTypeLookupValues(galleryId);

				foreach (MimeTypeEntity mimeType in this._mimeTypes)
				{
					// Try to find the matching MIME type in the config file. Each mimeType element is uniquely identified by the 
					// fileExtension and browserIf attributes.
					if (mimeType.BrowserId.Equals("default", StringComparison.OrdinalIgnoreCase) && mimeType.AllowAddToGallery)
					{
						try
						{
							EnableMimeType(mimeType, mimeTypeLookup[mimeType.Extension]);
						}
						catch (KeyNotFoundException) { }
					}
				}
			}

			private void EnableMimeType(MimeTypeEntity mimeType, int mimeTypeGalleryId)
			{
				_dbUpgrader.UpdateMimeTypeGallery(mimeTypeGalleryId, mimeType.AllowAddToGallery);
			}

			private static string GetDbSettingName(string configSettingName)
			{
				string renamedCoreConfigItem;

				if (_renamedCoreConfigItems.TryGetValue(configSettingName, out renamedCoreConfigItem))
				{
					return renamedCoreConfigItem;
				}
				else
				{
					throw new KeyNotFoundException(string.Format("Cannot find config setting name \"{0}\" in the collection of configuration names.", configSettingName));
				}
			}

			private static Dictionary<string, string> GetRenamedCoreConfigItems()
			{
				Dictionary<string, string> renamedCoreConfigItems = new Dictionary<string, string>();

				renamedCoreConfigItems.Add("enableImageMetadata", "EnableMetadata");
				renamedCoreConfigItems.Add("enableWpfMetadataExtraction", "ExtractMetadataUsingWpf");
				renamedCoreConfigItems.Add("pageHeaderText", "GalleryTitle");
				renamedCoreConfigItems.Add("pageHeaderTextUrl", "GalleryTitleUrl");
				renamedCoreConfigItems.Add("allowHtmlInTitlesAndCaptions", "AllowUserEnteredHtml");
				renamedCoreConfigItems.Add("enableMediaObjectZipDownload", "EnableGalleryObjectZipDownload");

				renamedCoreConfigItems.Add("galleryId", "GalleryId");
				renamedCoreConfigItems.Add("mediaObjectPath", "MediaObjectPath");
				renamedCoreConfigItems.Add("mediaObjectPathIsReadOnly", "MediaObjectPathIsReadOnly");
				renamedCoreConfigItems.Add("showLogin", "ShowLogin");
				renamedCoreConfigItems.Add("showSearch", "ShowSearch");
				renamedCoreConfigItems.Add("showErrorDetails", "ShowErrorDetails");
				renamedCoreConfigItems.Add("enableExceptionHandler", "EnableExceptionHandler");
				renamedCoreConfigItems.Add("defaultAlbumDirectoryNameLength", "DefaultAlbumDirectoryNameLength");
				renamedCoreConfigItems.Add("synchAlbumTitleAndDirectoryName", "SynchAlbumTitleAndDirectoryName");
				renamedCoreConfigItems.Add("emptyAlbumThumbnailBackgroundColor", "EmptyAlbumThumbnailBackgroundColor");
				renamedCoreConfigItems.Add("emptyAlbumThumbnailText", "EmptyAlbumThumbnailText");
				renamedCoreConfigItems.Add("emptyAlbumThumbnailFontName", "EmptyAlbumThumbnailFontName");
				renamedCoreConfigItems.Add("emptyAlbumThumbnailFontSize", "EmptyAlbumThumbnailFontSize");
				renamedCoreConfigItems.Add("emptyAlbumThumbnailFontColor", "EmptyAlbumThumbnailFontColor");
				renamedCoreConfigItems.Add("emptyAlbumThumbnailWidthToHeightRatio", "EmptyAlbumThumbnailWidthToHeightRatio");
				renamedCoreConfigItems.Add("maxAlbumThumbnailTitleDisplayLength", "MaxAlbumThumbnailTitleDisplayLength");
				renamedCoreConfigItems.Add("maxMediaObjectThumbnailTitleDisplayLength", "MaxMediaObjectThumbnailTitleDisplayLength");
				renamedCoreConfigItems.Add("allowUserEnteredJavascript", "AllowUserEnteredJavascript");
				renamedCoreConfigItems.Add("allowedHtmlTags", "AllowedHtmlTags");
				renamedCoreConfigItems.Add("allowedHtmlAttributes", "AllowedHtmlAttributes");
				renamedCoreConfigItems.Add("allowCopyingReadOnlyObjects", "AllowCopyingReadOnlyObjects");
				renamedCoreConfigItems.Add("allowManageOwnAccount", "AllowManageOwnAccount");
				renamedCoreConfigItems.Add("allowDeleteOwnAccount", "AllowDeleteOwnAccount");
				renamedCoreConfigItems.Add("mediaObjectTransitionType", "MediaObjectTransitionType");
				renamedCoreConfigItems.Add("mediaObjectTransitionDuration", "MediaObjectTransitionDuration");
				renamedCoreConfigItems.Add("slideshowInterval", "SlideshowInterval");
				renamedCoreConfigItems.Add("mediaObjectDownloadBufferSize", "MediaObjectDownloadBufferSize");
				renamedCoreConfigItems.Add("encryptMediaObjectUrlOnClient", "EncryptMediaObjectUrlOnClient");
				renamedCoreConfigItems.Add("encryptionKey", "EncryptionKey");
				renamedCoreConfigItems.Add("allowUnspecifiedMimeTypes", "AllowUnspecifiedMimeTypes");
				renamedCoreConfigItems.Add("imageTypesStandardBrowsersCanDisplay", "ImageTypesStandardBrowsersCanDisplay");
				renamedCoreConfigItems.Add("silverlightFileTypes", "SilverlightFileTypes");
				renamedCoreConfigItems.Add("allowAnonymousHiResViewing", "AllowAnonymousHiResViewing");
				renamedCoreConfigItems.Add("enableMediaObjectDownload", "EnableMediaObjectDownload");
				renamedCoreConfigItems.Add("enablePermalink", "EnablePermalink");
				renamedCoreConfigItems.Add("enableSlideShow", "EnableSlideShow");
				renamedCoreConfigItems.Add("maxThumbnailLength", "MaxThumbnailLength");
				renamedCoreConfigItems.Add("thumbnailImageJpegQuality", "ThumbnailImageJpegQuality");
				renamedCoreConfigItems.Add("thumbnailClickShowsOriginal", "ThumbnailClickShowsOriginal");
				renamedCoreConfigItems.Add("thumbnailWidthBuffer", "ThumbnailWidthBuffer");
				renamedCoreConfigItems.Add("thumbnailHeightBuffer", "ThumbnailHeightBuffer");
				renamedCoreConfigItems.Add("thumbnailFileNamePrefix", "ThumbnailFileNamePrefix");
				renamedCoreConfigItems.Add("thumbnailPath", "ThumbnailPath");
				renamedCoreConfigItems.Add("maxOptimizedLength", "MaxOptimizedLength");
				renamedCoreConfigItems.Add("optimizedImageJpegQuality", "OptimizedImageJpegQuality");
				renamedCoreConfigItems.Add("optimizedImageTriggerSizeKB", "OptimizedImageTriggerSizeKB");
				renamedCoreConfigItems.Add("optimizedFileNamePrefix", "OptimizedFileNamePrefix");
				renamedCoreConfigItems.Add("optimizedPath", "OptimizedPath");
				renamedCoreConfigItems.Add("originalImageJpegQuality", "OriginalImageJpegQuality");
				renamedCoreConfigItems.Add("discardOriginalImageDuringImport", "DiscardOriginalImageDuringImport");
				renamedCoreConfigItems.Add("applyWatermark", "ApplyWatermark");
				renamedCoreConfigItems.Add("applyWatermarkToThumbnails", "ApplyWatermarkToThumbnails");
				renamedCoreConfigItems.Add("watermarkText", "WatermarkText");
				renamedCoreConfigItems.Add("watermarkTextFontName", "WatermarkTextFontName");
				renamedCoreConfigItems.Add("watermarkTextFontSize", "watermarkTextFontSize");
				renamedCoreConfigItems.Add("watermarkTextWidthPercent", "WatermarkTextWidthPercent");
				renamedCoreConfigItems.Add("watermarkTextColor", "WatermarkTextColor");
				renamedCoreConfigItems.Add("watermarkTextOpacityPercent", "WatermarkTextOpacityPercent");
				renamedCoreConfigItems.Add("watermarkTextLocation", "WatermarkTextLocation");
				renamedCoreConfigItems.Add("watermarkImagePath", "WatermarkImagePath");
				renamedCoreConfigItems.Add("watermarkImageWidthPercent", "WatermarkImageWidthPercent");
				renamedCoreConfigItems.Add("watermarkImageOpacityPercent", "WatermarkImageOpacityPercent");
				renamedCoreConfigItems.Add("watermarkImageLocation", "WatermarkImageLocation");
				renamedCoreConfigItems.Add("sendEmailOnError", "SendEmailOnError");
				renamedCoreConfigItems.Add("emailFromName", "EmailFromName");
				renamedCoreConfigItems.Add("emailFromAddress", "EmailFromAddress");
				renamedCoreConfigItems.Add("emailToName", "EmailToName");
				renamedCoreConfigItems.Add("emailToAddress", "EmailToAddress");
				renamedCoreConfigItems.Add("smtpServer", "SmtpServer");
				renamedCoreConfigItems.Add("smtpServerPort", "SmtpServerPort");
				renamedCoreConfigItems.Add("sendEmailUsingSsl", "SendEmailUsingSsl");
				renamedCoreConfigItems.Add("autoStartMediaObject", "AutoStartMediaObject");
				renamedCoreConfigItems.Add("defaultVideoPlayerWidth", "DefaultVideoPlayerWidth");
				renamedCoreConfigItems.Add("defaultVideoPlayerHeight", "DefaultVideoPlayerHeight");
				renamedCoreConfigItems.Add("defaultAudioPlayerWidth", "DefaultAudioPlayerWidth");
				renamedCoreConfigItems.Add("defaultAudioPlayerHeight", "DefaultAudioPlayerHeight");
				renamedCoreConfigItems.Add("defaultGenericObjectWidth", "DefaultGenericObjectWidth");
				renamedCoreConfigItems.Add("defaultGenericObjectHeight", "DefaultGenericObjectHeight");
				renamedCoreConfigItems.Add("maxUploadSize", "MaxUploadSize");
				renamedCoreConfigItems.Add("allowAddLocalContent", "AllowAddLocalContent");
				renamedCoreConfigItems.Add("allowAddExternalContent", "AllowAddExternalContent");
				renamedCoreConfigItems.Add("allowAnonymousBrowsing", "AllowAnonymousBrowsing");
				renamedCoreConfigItems.Add("pageSize", "PageSize");
				renamedCoreConfigItems.Add("pagerLocation", "PagerLocation");
				renamedCoreConfigItems.Add("maxNumberErrorItems", "MaxNumberErrorItems");
				renamedCoreConfigItems.Add("enableSelfRegistration", "EnableSelfRegistration");
				renamedCoreConfigItems.Add("requireEmailValidationForSelfRegisteredUser", "RequireEmailValidationForSelfRegisteredUser");
				renamedCoreConfigItems.Add("requireApprovalForSelfRegisteredUser", "RequireApprovalForSelfRegisteredUser");
				renamedCoreConfigItems.Add("useEmailForAccountName", "UseEmailForAccountName");
				renamedCoreConfigItems.Add("defaultRolesForSelfRegisteredUser", "DefaultRolesForSelfRegisteredUser");
				renamedCoreConfigItems.Add("usersToNotifyWhenAccountIsCreated", "UsersToNotifyWhenAccountIsCreated");
				renamedCoreConfigItems.Add("enableUserAlbum", "EnableUserAlbum");
				renamedCoreConfigItems.Add("userAlbumParentAlbumId", "UserAlbumParentAlbumId");
				renamedCoreConfigItems.Add("userAlbumNameTemplate", "UserAlbumNameTemplate");
				renamedCoreConfigItems.Add("userAlbumSummaryTemplate", "UserAlbumSummaryTemplate");
				renamedCoreConfigItems.Add("redirectToUserAlbumAfterLogin", "RedirectToUserAlbumAfterLogin");
				renamedCoreConfigItems.Add("jQueryScriptPath", "JQueryScriptPath");
				renamedCoreConfigItems.Add("membershipProviderName", "MembershipProviderName");
				renamedCoreConfigItems.Add("roleProviderName", "RoleProviderName");
				renamedCoreConfigItems.Add("productKey", "ProductKey");

				return renamedCoreConfigItems;
			}

			private static List<string> GetCoreConfigItemsToSkip()
			{
				List<string> coreConfigItemsToSkip = new List<string>();
				coreConfigItemsToSkip.Add("galleryId");
				coreConfigItemsToSkip.Add("productKey");
				coreConfigItemsToSkip.Add("emailToName");
				coreConfigItemsToSkip.Add("encryptMediaObjectUrlOnClient");
				coreConfigItemsToSkip.Add("jQueryScriptPath");
				coreConfigItemsToSkip.Add("silverlightFileTypes");
				return coreConfigItemsToSkip;
			}

			private static List<string> GetAppSettingNames()
			{
				List<string> appSettingNames = new List<string>();
				appSettingNames.Add("mediaObjectDownloadBufferSize");
				appSettingNames.Add("encryptMediaObjectUrlOnClient");
				appSettingNames.Add("encryptionKey");
				appSettingNames.Add("jQueryScriptPath");
				appSettingNames.Add("membershipProviderName");
				appSettingNames.Add("roleProviderName");
				appSettingNames.Add("productKey");
				appSettingNames.Add("maxNumberErrorItems");
				return appSettingNames;
			}

			private static XmlElement GetGspCore(XmlNode gspConfig)
			{
				XmlElement core = (XmlElement)gspConfig.SelectSingleNode(@"galleryServerPro/core");

				if (core == null)
				{
					throw new WebException("Could not find the galleryServerPro/core section in galleryserverpro.config.");
				}
				return core;
			}

			private void UpdateDataProviderInConfigFile(XmlNode gspConfig)
			{
				// Update the gallery data provider.
				XmlElement core = (XmlElement)gspConfig.SelectSingleNode(@"galleryServerPro/dataProvider");

				core.SetAttribute("defaultProvider", this._galleryDataProvider.ToString());
			}

			#endregion
		}

		#endregion

		#region Enum declarations

		public enum UpgradeWizardPanel
		{
			Welcome,
			ReadyToUpgrade,
			ImportProfiles,
			Finished,
		}

		#endregion

		#region Private Fields

		private DatabaseUpgrader _dbUpgrader;

		#endregion

		#region Public Properties

		public bool UpgradeSuccessful
		{
			get
			{
				return DatabaseSuccessfullyUpgraded && WebConfigSuccessfullyUpdated && GspConfigSuccessfullyImported && ProfilesSuccessfullyImported;
			}
		}

		public bool WebConfigSuccessfullyUpdated
		{
			get
			{
				if (ViewState["WebConfigSuccessfullyUpdated"] != null)
					return (bool)ViewState["WebConfigSuccessfullyUpdated"];

				return false;
			}
			set
			{
				ViewState["WebConfigSuccessfullyUpdated"] = value;
			}
		}

		public bool WebConfigUpdateRequired
		{
			get
			{
				if (ViewState["WebConfigUpdateRequired"] != null)
					return (bool)ViewState["WebConfigUpdateRequired"];

				return false;
			}
			set
			{
				ViewState["WebConfigUpdateRequired"] = value;
			}
		}

		public string WebConfigUpdateErrorMsg
		{
			get
			{
				if (ViewState["WebConfigUpdateErrorMsg"] != null)
					return ViewState["WebConfigUpdateErrorMsg"].ToString();

				return String.Empty;
			}
			set
			{
				ViewState["WebConfigUpdateErrorMsg"] = value;
			}
		}

		public bool GspConfigSuccessfullyImported
		{
			get
			{
				if (ViewState["GspConfigSuccessfullyImported"] != null)
					return (bool)ViewState["GspConfigSuccessfullyImported"];

				return false;
			}
			set
			{
				ViewState["GspConfigSuccessfullyImported"] = value;
			}
		}

		public string GspConfigUpdateErrorMsg
		{
			get
			{
				if (ViewState["GspConfigUpdateErrorMsg"] != null)
					return ViewState["GspConfigUpdateErrorMsg"].ToString();

				return String.Empty;
			}
			set
			{
				ViewState["GspConfigUpdateErrorMsg"] = value;
			}
		}

		public bool DatabaseUpgradeRequired
		{
			get
			{
				if (ViewState["DatabaseUpgradeRequired"] != null)
					return (bool)ViewState["DatabaseUpgradeRequired"];

				return false;
			}
			set
			{
				ViewState["DatabaseUpgradeRequired"] = value;
			}
		}

		public bool DatabaseSuccessfullyUpgraded
		{
			get
			{
				if (ViewState["DatabaseSuccessfullyUpgraded"] != null)
					return (bool)ViewState["DatabaseSuccessfullyUpgraded"];

				return false;
			}
			set
			{
				ViewState["DatabaseSuccessfullyUpgraded"] = value;
			}
		}

		public string DbUpgradeErrorMsg
		{
			get
			{
				if (ViewState["DbUpgradeErrorMsg"] != null)
					return ViewState["DbUpgradeErrorMsg"].ToString();

				return String.Empty;
			}
			set
			{
				ViewState["DbUpgradeErrorMsg"] = value;
			}
		}

		public string DbUpgradeErrorSql
		{
			get
			{
				if (ViewState["DbUpgradeErrorSql"] != null)
					return ViewState["DbUpgradeErrorSql"].ToString();

				return String.Empty;
			}
			set
			{
				ViewState["DbUpgradeErrorSql"] = value;
			}
		}

		public bool ProfilesSuccessfullyImported
		{
			get
			{
				if (ViewState["ProfilesSuccessfullyImported"] != null)
					return (bool)ViewState["ProfilesSuccessfullyImported"];

				return false;
			}
			set
			{
				ViewState["ProfilesSuccessfullyImported"] = value;
			}
		}

		public int ProfilesImportedNumber
		{
			get
			{
				if (ViewState["ProfilesImportedNumber"] != null)
					return (int)ViewState["ProfilesImportedNumber"];

				return int.MinValue;
			}
			set
			{
				ViewState["ProfilesImportedNumber"] = value;
			}
		}

		public string ProfilesImportErrorMsg
		{
			get
			{
				if (ViewState["ProfilesImportErrorMsg"] != null)
					return ViewState["ProfilesImportErrorMsg"].ToString();

				return String.Empty;
			}
			set
			{
				ViewState["ProfilesImportErrorMsg"] = value;
			}
		}

		public bool WizardsSuccessfullyDisabled
		{
			get
			{
				if (ViewState["UpgradeWizardSuccessfullyDisabled"] != null)
					return (bool)ViewState["UpgradeWizardSuccessfullyDisabled"];

				return false;
			}
			set
			{
				ViewState["UpgradeWizardSuccessfullyDisabled"] = value;
			}
		}

		/// <summary>
		/// Gets a reference to an object that can assist with SQL Server execution and other management.
		/// </summary>
		/// <value>The SQL helper.</value>
		private DatabaseUpgrader DbUpgrader
		{
			get
			{
				if (_dbUpgrader == null)
				{
					_dbUpgrader = new DatabaseUpgrader(GspConfigSourcePath);
				}

				return _dbUpgrader;
			}
		}

		public UpgradeWizardPanel CurrentWizardPanel
		{
			get
			{
				if (ViewState["WizardPanel"] != null)
					return (UpgradeWizardPanel)ViewState["WizardPanel"];

				return UpgradeWizardPanel.Welcome;
			}
			set
			{
				ViewState["WizardPanel"] = value;
			}
		}

		public int GalleryId
		{
			get
			{
				if (ViewState["GalleryId"] != null)
					return (int)ViewState["GalleryId"];

				return int.MinValue;
			}
			set
			{
				ViewState["GalleryId"] = value;
			}
		}

		public string WebConfigPath
		{
			get
			{
				return Server.MapPath("~/web.config");
			}
		}

		public string GspConfigSourcePath
		{
			get
			{
				return Server.MapPath(Util.GetUrl("/config/galleryserverpro_old.config"));
			}
		}

		#endregion

		#region Event Handlers

		protected void Page_Load(object sender, EventArgs e)
		{
			bool setupEnabled;
			if (Boolean.TryParse(ENABLE_SETUP.Value, out setupEnabled) && setupEnabled)
			{
				if (!Page.IsPostBack)
				{
					SetCurrentPanel(UpgradeWizardPanel.Welcome, Welcome);
				}

				ConfigureControls();
			}
			else
			{
				Response.Write(String.Format(CultureInfo.CurrentCulture, "<h1>{0}</h1>", Resources.GalleryServerPro.Installer_Disabled_Msg));
				Response.Flush();
				Response.End();
			}
		}

		protected void btnNext_Click(object sender, EventArgs e)
		{
			if (Page.IsValid)
				ShowNextPanel();
		}

		protected void btnPrevious_Click(object sender, EventArgs e)
		{
			ShowPreviousPanel();
		}

		protected void lbTryAgain_Click(object sender, EventArgs e)
		{
			ConfigureReadyToUpgradeControls();
		}

		#endregion

		#region Private Methods

		private void ConfigureControls()
		{
			if (!IsPostBack)
				ConfigureControlsFirstTime();

			Page.Form.DefaultFocus = btnNext.ClientID;
		}

		private void ConfigureControlsFirstTime()
		{
			string version = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Footer_Gsp_Version_Text, Util.GetGalleryServerVersion());
			litVersion.Text = version;
		}

		private void SetCurrentPanel(UpgradeWizardPanel panel, Control controlToShow)
		{
			Panel currentPanel = wizCtnt.FindControl(CurrentWizardPanel.ToString()) as Panel;
			if (currentPanel != null)
				currentPanel.Visible = false;

			switch (panel)
			{
				case UpgradeWizardPanel.Welcome:
					btnPrevious.Enabled = false;
					break;
				case UpgradeWizardPanel.Finished:
					btnNext.Enabled = false;
					break;
				default:
					btnPrevious.Enabled = true;
					btnNext.Enabled = true;
					break;
			}

			controlToShow.Visible = true;
			CurrentWizardPanel = panel;
		}

		private void ShowNextPanel()
		{
			switch (CurrentWizardPanel)
			{
				case UpgradeWizardPanel.Welcome:
					{
						SetCurrentPanel(UpgradeWizardPanel.ReadyToUpgrade, ReadyToUpgrade);
						ConfigureReadyToUpgradeControls();
						break;
					}
				case UpgradeWizardPanel.ReadyToUpgrade:
					{
						if (UpgradeDatabase())
						{
							GalleryId = ImportConfigSettings();

							SetCurrentPanel(UpgradeWizardPanel.ImportProfiles, ImportProfiles);
						}
						else
						{
							WebConfigUpdateErrorMsg = "Not attempted because of database upgrade failure.";
							GspConfigUpdateErrorMsg = "Not attempted because of database upgrade failure.";
							ProfilesImportErrorMsg = "Not attempted because of database upgrade failure.";

							ConfigureFinishedControls();

							SetCurrentPanel(UpgradeWizardPanel.Finished, Finished);
						}
					}
					break;
				case UpgradeWizardPanel.ImportProfiles:
					{
						ImportProfileSettings(GalleryId);

						AssignGalleryControlSetting();

						SetCurrentPanel(UpgradeWizardPanel.Finished, Finished);

						DeleteInstallFileTrigger();

						DisableInstallAndUpgradeWizards();

						ConfigureFinishedControls();

						btnNext.Text = Resources.GalleryServerPro.Installer_Finish_Button_Text;

						break;
					}
			}
		}

		private void ShowPreviousPanel()
		{
			switch (this.CurrentWizardPanel)
			{
				case UpgradeWizardPanel.Welcome: break;
				case UpgradeWizardPanel.ReadyToUpgrade:
					{
						SetCurrentPanel(UpgradeWizardPanel.Welcome, Welcome);
						break;
					}
				case UpgradeWizardPanel.ImportProfiles:
					{
						SetCurrentPanel(UpgradeWizardPanel.ReadyToUpgrade, ReadyToUpgrade);
						ConfigureReadyToUpgradeControls();
						break;
					}
				case UpgradeWizardPanel.Finished:
					{
						SetCurrentPanel(UpgradeWizardPanel.ImportProfiles, ImportProfiles);
						ConfigureReadyToUpgradeControls();
						break;
					}
			}
		}

		/// <summary>
		/// Configures the controls on the Finished step of the wizard. This appears after the upgrade is complete. If an
		/// error occurred, show the error message and call stack.
		/// </summary>
		private void ConfigureFinishedControls()
		{
			if (UpgradeSuccessful)
			{
				// No errors! Yippee!
				lblFinishedHdrMsg.Text = Resources.GalleryServerPro.Installer_Upgrade_Finished_Hdr;
				imgFinishedIcon.ImageUrl = Util.GetUrl("/images/ok_26x26.png");
				imgFinishedIcon.Width = Unit.Pixel(26);
				imgFinishedIcon.Height = Unit.Pixel(26);
				l61.Text = String.Format(Resources.GalleryServerPro.Installer_Upgrade_Finished_No_Addl_Steps_Reqd, Util.GetCurrentPageUrl());

				if (WizardsSuccessfullyDisabled)
				{
					lblWizardDisableMsg.Text = Resources.GalleryServerPro.Installer_Finished_WizardsDisabled_Msg;
				}
				else
				{
					lblWizardDisableMsg.Text = Resources.GalleryServerPro.Installer_Finished_NeedToDisableWizard_Msg;
				}

				lblWizardDisableMsg.Visible = true;
			}
			else
			{
				// Something went wrong.
				lblFinishedHdrMsg.Text = Resources.GalleryServerPro.Installer_Upgrade_Finished_Error_Hdr;
				imgFinishedIcon.ImageUrl = Util.GetUrl("/images/warning_32x32.png");
				imgFinishedIcon.Width = Unit.Pixel(32);
				imgFinishedIcon.Height = Unit.Pixel(32);
				l61.Text = Resources.GalleryServerPro.Installer_Upgrade_Finished_Error_Dtl;
			}

			// Database related controls
			if (DatabaseUpgradeRequired)
			{
				if (DatabaseSuccessfullyUpgraded)
				{
					imgFinishedDbStatus.ImageUrl = Util.GetUrl("/images/green_check_13x12.png");
					imgFinishedDbStatus.Width = Unit.Pixel(13);
					imgFinishedDbStatus.Height = Unit.Pixel(12);

					lblFinishedDbStatus.Text = Resources.GalleryServerPro.Installer_Upgrade_Finished_Db_Upgraded_Msg;
					lblFinishedDbStatus.CssClass = "gsp_msgfriendly";
					lblFinishedDbSql.Visible = false;
				}
				else
				{
					imgFinishedDbStatus.ImageUrl = Util.GetUrl("/images/error_16x16.png");
					imgFinishedDbStatus.Width = Unit.Pixel(16);
					imgFinishedDbStatus.Height = Unit.Pixel(16);
					lblFinishedDbStatus.Text = Util.HtmlEncode(DbUpgradeErrorMsg);
					lblFinishedDbStatus.CssClass = "gsp_msgattention";

					lblFinishedDbSql.Text = DbUpgradeErrorSql;
					lblFinishedDbSql.Visible = true;
				}
			}
			else
			{
				imgFinishedDbStatus.ImageUrl = Util.GetUrl("/images/green_check_13x12.png");
				imgFinishedDbStatus.Width = Unit.Pixel(13);
				imgFinishedDbStatus.Height = Unit.Pixel(12);

				lblFinishedDbStatus.Text = Resources.GalleryServerPro.Installer_Upgrade_Db_Status_No_Upgrade_Msg;
				lblFinishedDbStatus.CssClass = "gsp_msgfriendly";
				lblFinishedDbSql.Visible = false;
			}

			// web.config related controls
			if (WebConfigSuccessfullyUpdated)
			{
				imgFinishedWebConfigStatus.ImageUrl = Util.GetUrl("/images/green_check_13x12.png");
				imgFinishedWebConfigStatus.Width = Unit.Pixel(13);
				imgFinishedWebConfigStatus.Height = Unit.Pixel(12);

				if (WebConfigUpdateRequired)
				{
					lblFinishedWebConfigStatus.Text = Resources.GalleryServerPro.Installer_Upgrade_Finished_WebConfig_OK_Msg;
					lblFinishedWebConfigStatus.CssClass = "gsp_msgfriendly";
				}
				else
				{
					lblFinishedWebConfigStatus.Text = Resources.GalleryServerPro.Installer_Upgrade_Config_Status_No_Import_Msg;
					lblFinishedWebConfigStatus.CssClass = "gsp_msgfriendly";
				}
			}
			else
			{
				imgFinishedWebConfigStatus.ImageUrl = Util.GetUrl("/images/error_16x16.png");
				imgFinishedWebConfigStatus.Width = Unit.Pixel(16);
				imgFinishedWebConfigStatus.Height = Unit.Pixel(16);
				lblFinishedWebConfigStatus.Text = Util.HtmlEncode(WebConfigUpdateErrorMsg);
				lblFinishedWebConfigStatus.CssClass = "gsp_msgattention";
			}

			// galleryserverpro.config related controls
			if (GspConfigSuccessfullyImported)
			{
				imgFinishedGspConfigStatus.ImageUrl = Util.GetUrl("/images/green_check_13x12.png");
				imgFinishedGspConfigStatus.Width = Unit.Pixel(13);
				imgFinishedGspConfigStatus.Height = Unit.Pixel(12);
				lblFinishedGspConfigStatus.Text = String.Format(Resources.GalleryServerPro.Installer_Upgrade_Finished_Config_OK_Msg, GspConfigSourcePath);
				lblFinishedGspConfigStatus.CssClass = "gsp_msgfriendly";
			}
			else
			{
				imgFinishedGspConfigStatus.ImageUrl = Util.GetUrl("/images/error_16x16.png");
				imgFinishedGspConfigStatus.Width = Unit.Pixel(16);
				imgFinishedGspConfigStatus.Height = Unit.Pixel(16);
				lblFinishedGspConfigStatus.Text = Util.HtmlEncode(GspConfigUpdateErrorMsg);
				lblFinishedGspConfigStatus.CssClass = "gsp_msgattention";
			}

			// Profiles related controls
			if (ProfilesSuccessfullyImported)
			{
				imgFinishedProfilesStatus.ImageUrl = Util.GetUrl("/images/green_check_13x12.png");
				imgFinishedProfilesStatus.Width = Unit.Pixel(13);
				imgFinishedProfilesStatus.Height = Unit.Pixel(12);

				if (ProfilesImportedNumber > 0)
				{
					lblFinishedProfilesStatus.Text = String.Format(Resources.GalleryServerPro.Installer_Upgrade_Finished_Profiles_OK_Msg, ProfilesImportedNumber);
				}
				else
				{
					lblFinishedProfilesStatus.Text = Resources.GalleryServerPro.Installer_Upgrade_Finished_NoProfilesImported_Msg;
				}

				lblFinishedProfilesStatus.CssClass = "gsp_msgfriendly";
			}
			else
			{
				imgFinishedProfilesStatus.ImageUrl = Util.GetUrl("/images/error_16x16.png");
				imgFinishedProfilesStatus.Width = Unit.Pixel(16);
				imgFinishedProfilesStatus.Height = Unit.Pixel(16);
				lblFinishedProfilesStatus.Text = Util.HtmlEncode(ProfilesImportErrorMsg);
				lblFinishedProfilesStatus.CssClass = "gsp_msgattention";
			}
		}

		private void ConfigureReadyToUpgradeControls()
		{
			#region Database

			bool dbUpgradeOk = false;
			DatabaseUpgrader db = new DatabaseUpgrader(GspConfigSourcePath);

			lblReadyToUpgradeDbHeader.Text = String.Concat(db.DataProvider, " Database");

			if (db.IsUpgradeRequired)
			{
				if (db.IsAutoUpgradeSupported)
				{
					lblReadyToUpgradeDbStatus.Text = Util.HtmlEncode(string.Format(Resources.GalleryServerPro.Installer_Upgrade_Db_Status_Upgrade_Reqd_Msg, db.GetDatabaseVersionString() ?? "<unknown>"));
					lblReadyToUpgradeDbStatus.CssClass = "gsp_msgfriendly";
					imgReadyToUpgradeDbStatus.ImageUrl = Util.GetUrl("/images/go_14x14.png");
					dbUpgradeOk = true;
				}
				else
				{
					lblReadyToUpgradeDbStatus.Text = Util.HtmlEncode(db.AutoUpgradeNotSupportedReason);
					lblReadyToUpgradeDbStatus.CssClass = "gsp_msgwarning";
					imgReadyToUpgradeDbStatus.ImageUrl = Util.GetUrl("/images/error_16x16.png");
					imgReadyToUpgradeDbStatus.Width = Unit.Pixel(16);
					imgReadyToUpgradeDbStatus.Height = Unit.Pixel(16);
				}
			}
			else
			{
				// web.config has same settings as the source web.config. No update needed.
				lblReadyToUpgradeDbStatus.Text = Resources.GalleryServerPro.Installer_Upgrade_Db_Status_No_Upgrade_Reqd_Msg;
				lblReadyToUpgradeDbStatus.CssClass = String.Empty;
				imgReadyToUpgradeDbStatus.ImageUrl = Util.GetUrl("/images/ok_16x16.png");
				imgReadyToUpgradeDbStatus.Width = Unit.Pixel(16);
				imgReadyToUpgradeDbStatus.Height = Unit.Pixel(16);
				dbUpgradeOk = true;
			}

			#endregion

			#region web.config

			bool webConfigUpdateOk = false;
			// Check permissions on web.config
			WebConfigUpdater webCfg = null;
			try
			{
				webCfg = new WebConfigUpdater();
			}
			catch (FileNotFoundException ex)
			{
				lblReadyToUpgradeWebConfigStatus.Text = ex.Message;
				lblReadyToUpgradeWebConfigStatus.CssClass = "gsp_msgwarning";
				imgReadyToUpgradeWebConfigStatus.ImageUrl = Util.GetUrl("/images/error_16x16.png");
			}

			if (webCfg != null)
			{
				if (webCfg.UpgradeRequired)
				{
					if (webCfg.IsWriteable)
					{
						// Web.config has different settings than the source file, so an update is needed. And we have the necessary write
						// permission to update the file, so we are good to go!
						lblReadyToUpgradeWebConfigStatus.Text = Util.HtmlEncode(Resources.GalleryServerPro.Installer_Upgrade_Config_Status_Upgrade_Msg);
						lblReadyToUpgradeWebConfigStatus.CssClass = "gsp_msgfriendly";
						imgReadyToUpgradeWebConfigStatus.ImageUrl = Util.GetUrl("/images/go_14x14.png");
						webConfigUpdateOk = true;
					}
					else
					{
						// Web.config file needs updating, but installer doesn't have the required write permission.
						lblReadyToUpgradeWebConfigStatus.Text = String.Format(Resources.GalleryServerPro.Installer_Upgrade_ReadyToUpgrade_Config_Status_No_Perm_Msg, WebConfigPath);
						lblReadyToUpgradeWebConfigStatus.CssClass = "gsp_msgwarning";
						imgReadyToUpgradeWebConfigStatus.ImageUrl = Util.GetUrl("/images/error_16x16.png");
						imgReadyToUpgradeWebConfigStatus.Width = Unit.Pixel(16);
						imgReadyToUpgradeWebConfigStatus.Height = Unit.Pixel(16);
					}
				}
				else
				{
					// web.config has same settings as the source web.config. No update needed.
					lblReadyToUpgradeWebConfigStatus.Text = Resources.GalleryServerPro.Installer_Upgrade_Config_Status_No_Import_Msg;
					lblReadyToUpgradeWebConfigStatus.CssClass = String.Empty;
					imgReadyToUpgradeWebConfigStatus.ImageUrl = Util.GetUrl("/images/ok_16x16.png");
					imgReadyToUpgradeWebConfigStatus.Width = Unit.Pixel(16);
					imgReadyToUpgradeWebConfigStatus.Height = Unit.Pixel(16);
					webConfigUpdateOk = true;
				}
			}

			#endregion

			#region galleryserverpro.config

			bool gspConfigOk = false;
			// Check permissions on galleryserverpro.config
			GspConfigImporter gspCfg = null;
			try
			{
				gspCfg = new GspConfigImporter(GspConfigSourcePath, DbUpgrader);
			}
			catch (FileNotFoundException)
			{
				lblReadyToUpgradeGspConfigStatus.Text = String.Format(Resources.GalleryServerPro.Installer_Upgrade_ReadyToUpgrade_GspConfigNotFound, GspConfigSourcePath);
				lblReadyToUpgradeGspConfigStatus.CssClass = "gsp_msgwarning";
				imgReadyToUpgradeGspConfigStatus.ImageUrl = Util.GetUrl("/images/error_16x16.png");
				imgReadyToUpgradeGspConfigStatus.Width = Unit.Pixel(16);
				imgReadyToUpgradeGspConfigStatus.Height = Unit.Pixel(16);
			}

			if (gspCfg != null)
			{
				lblReadyToUpgradeGspConfigStatus.Text = String.Format(Resources.GalleryServerPro.Installer_Upgrade_ReadyToUpgrade_Config_Status_OK_Msg, GspConfigSourcePath);
				lblReadyToUpgradeGspConfigStatus.CssClass = "gsp_msgfriendly";
				imgReadyToUpgradeGspConfigStatus.ImageUrl = Util.GetUrl("/images/go_14x14.png");
				gspConfigOk = true;
			}

			#endregion

			if (dbUpgradeOk && webConfigUpdateOk && gspConfigOk)
			{
				// Show the summary text that we are ready for the upgrade.
				lblReadyToUpgradeHdrMsg.Text = Resources.GalleryServerPro.Installer_Upgrade_ReadyToUpgrade_Hdr;
				lblReadyToUpgradeDetail1Msg.Text = Resources.GalleryServerPro.Installer_Upgrade_ReadyToUpgrade_OK_Dtl1;
				imgReadyToUpgradeStatus.ImageUrl = Util.GetUrl("/images/ok_26x26.png");
				imgReadyToUpgradeStatus.Width = Unit.Pixel(26);
				imgReadyToUpgradeStatus.Height = Unit.Pixel(26);
			}
			else
			{
				// Show the summary text that something is wrong and we can't proceed.
				lblReadyToUpgradeHdrMsg.Text = Resources.GalleryServerPro.Installer_Upgrade_ReadyToUpgrade_Cannot_Upgrade_Hdr;
				lblReadyToUpgradeDetail1Msg.Text = Resources.GalleryServerPro.Installer_Upgrade_ReadyToUpgrade_CannotUpgrade_Dtl1;

				if ((webCfg != null) && (!webCfg.IsWriteable))
				{
					lblReadyToUpgradeDetail2Msg.Text = Resources.GalleryServerPro.Installer_Upgrade_ReadyToUpgrade_No_Perm_Dtl1;
				}

				lbTryAgain.Visible = true;
				btnNext.Enabled = false;
				imgReadyToUpgradeStatus.ImageUrl = Util.GetUrl("/images/warning_32x32.png");
				imgReadyToUpgradeStatus.Width = Unit.Pixel(32);
				imgReadyToUpgradeStatus.Height = Unit.Pixel(32);
			}
		}

		/// <summary>
		/// Upgrade the database, returning <c>true</c> if the upgrade is successful and <c>false</c> if not. If any exceptions 
		/// occur, swallow them and grab the error message and callstack in member variables.
		/// </summary>
		/// <returns>Returns <c>true</c> if the upgrade is successful and <c>false</c> if not.</returns>
		private bool UpgradeDatabase()
		{
			try
			{
				DatabaseUpgrader dbUpgrader = new DatabaseUpgrader(GspConfigSourcePath);

				DatabaseUpgradeRequired = dbUpgrader.IsUpgradeRequired;

				dbUpgrader.Upgrade();

				DatabaseSuccessfullyUpgraded = true;
			}
			catch (Exception ex)
			{

				DbUpgradeErrorMsg = GetExceptionDetails(ex);

				if (ex.Data.Contains(SqlServerHelper.ExceptionDataId))
				{
					DbUpgradeErrorSql = ex.Data[SqlServerHelper.ExceptionDataId].ToString();
				}
				if ((ex.InnerException != null) && (ex.InnerException.Data.Contains(SqlServerHelper.ExceptionDataId)))
				{
					DbUpgradeErrorSql = ex.InnerException.Data[SqlServerHelper.ExceptionDataId].ToString();
				}
			}

			return DatabaseSuccessfullyUpgraded;
		}

		private int ImportConfigSettings()
		{
			int galleryId = int.MinValue;

			try
			{
				WebConfigUpdater configImporter = new WebConfigUpdater();
				this.WebConfigUpdateRequired = configImporter.UpgradeRequired;
				configImporter.Upgrade();
				WebConfigSuccessfullyUpdated = true;
			}
			catch (Exception ex)
			{
				WebConfigUpdateErrorMsg = GetExceptionDetails(ex);
			}

			GalleryDataProvider galleryDataProvider = GalleryDataProvider.Unknown;
			try
			{
				GspConfigImporter gspConfigImporter = new GspConfigImporter(GspConfigSourcePath, DbUpgrader);
				galleryId = gspConfigImporter.Import();
				galleryDataProvider = gspConfigImporter.GalleryDataProvider;

				string gspConfigPath = Server.MapPath(Util.GetUrl("/config/galleryserverpro.config"));

				try
				{
					// Delete galleryserverpro.config file, but don't worry if it fails - after an upgrade the app doesn't use it anyway.
					File.Delete(gspConfigPath);
				}
				catch (IOException) { }
				catch (UnauthorizedAccessException) { }
				catch (System.Security.SecurityException) { }

				string gspConfigFilePathAfterImport = GspConfigSourcePath.Replace("galleryserverpro_old.config", "galleryserverpro_IMPORTED.config");
				try
				{
					// Rename galleryserverpro_old.config so that its presence doesn't trigger the upgrade wizard.
					File.Move(GspConfigSourcePath, gspConfigFilePathAfterImport);
					GspConfigSuccessfullyImported = true;
				}
				catch (IOException)
				{
					GspConfigUpdateErrorMsg = String.Format(Resources.GalleryServerPro.Installer_Upgrade_Finished_ConfigImportedButCannotRenameFile_Msg, GspConfigSourcePath, gspConfigFilePathAfterImport);
				}
				catch (UnauthorizedAccessException)
				{
					GspConfigUpdateErrorMsg = String.Format(Resources.GalleryServerPro.Installer_Upgrade_Finished_ConfigImportedButCannotRenameFile_Msg, GspConfigSourcePath, gspConfigFilePathAfterImport);
				}
				catch (System.Security.SecurityException)
				{
					GspConfigUpdateErrorMsg = String.Format(Resources.GalleryServerPro.Installer_Upgrade_Finished_ConfigImportedButCannotRenameFile_Msg, GspConfigSourcePath, gspConfigFilePathAfterImport);
				}
			}
			catch (Exception ex)
			{
				GspConfigUpdateErrorMsg = GetExceptionDetails(ex);
			}

			// Now update the gallery data provider in web.config.
			WebConfigEntity webConfig = WebConfigController.GetWebConfigEntity();
			webConfig.GalleryDataDefaultProvider = galleryDataProvider;
			WebConfigController.Save(webConfig);

			return galleryId;
		}

		/// <summary>
		/// Assigns the current Gallery control to the gallery we just imported.
		/// </summary>
		private void AssignGalleryControlSetting()
		{
			if (GalleryId > int.MinValue)
			{
				string controlId = String.Concat(System.Web.HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath, "|", this.Parent.ClientID);
				IGalleryControlSettings controlSettings = Factory.LoadGalleryControlSetting(controlId);
				controlSettings.GalleryId = GalleryId;
				controlSettings.Save();
			}
		}

		private void ImportProfileSettings(int galleryId)
		{
			try
			{
				if (GalleryId > int.MinValue)
				{
					GalleryController.InitializeGspApplication();

					ProfilesImportedNumber = ImportProfileSettingsFromGallery(galleryId);

					ProfilesSuccessfullyImported = true;
				}
				else
				{
					ProfilesImportErrorMsg = "Could not import profiles: No valid gallery ID was available."; // Will happen when a database upgrade failure occurs
				}
			}
			catch (Exception ex)
			{
				ProfilesImportErrorMsg = GetExceptionDetails(ex);
			}
		}

		private static int ImportProfileSettingsFromGallery(int galleryId)
		{
			// Import profile settings
			System.Web.Profile.ProfileBase oldProfile;
			int profilesImportedCounter = 0;

			foreach (System.Web.Profile.ProfileInfo profileInfo in System.Web.Profile.ProfileManager.GetAllProfiles(System.Web.Profile.ProfileAuthenticationOption.Authenticated))
			{
				oldProfile = System.Web.Profile.ProfileBase.Create(profileInfo.UserName, false);

				IUserProfile newProfile = ProfileController.GetProfile(oldProfile.UserName);
				IUserGalleryProfile userGalleryProfile = newProfile.GetGalleryProfile(galleryId);

				bool showMediaObjectMetadata;
				if (Boolean.TryParse(oldProfile.GetPropertyValue("ShowMediaObjectMetadata").ToString(), out showMediaObjectMetadata))
				{
					userGalleryProfile.ShowMediaObjectMetadata = showMediaObjectMetadata;
				}

				int userAlbumId;
				if (Int32.TryParse(oldProfile.GetPropertyValue("UserAlbumId").ToString(), out userAlbumId))
				{
					userGalleryProfile.UserAlbumId = userAlbumId;
				}

				bool enableUserAlbum;
				if (Boolean.TryParse(oldProfile.GetPropertyValue("EnableUserAlbum").ToString(), out enableUserAlbum))
				{
					userGalleryProfile.EnableUserAlbum = enableUserAlbum;
				}

				ProfileController.SaveProfile(newProfile);

				profilesImportedCounter++;
			}

			return profilesImportedCounter;
		}

		private void DeleteInstallFileTrigger()
		{
			// Note: This function is also in the install wizard page (but slightly modified).
			string installFilePath = Path.Combine(Request.PhysicalApplicationPath, Path.Combine(GlobalConstants.AppDataDirectory, GlobalConstants.InstallTriggerFileName));

			if (File.Exists(installFilePath))
			{
				try
				{
					File.Delete(installFilePath);
				}
				catch (IOException)
				{
					GspConfigUpdateErrorMsg = String.Format(Resources.GalleryServerPro.Installer_Upgrade_Finished_CouldNotDeleteInstallFile_Msg, installFilePath);
					GspConfigSuccessfullyImported = false;
				}
				catch (UnauthorizedAccessException)
				{
					GspConfigUpdateErrorMsg = String.Format(Resources.GalleryServerPro.Installer_Upgrade_Finished_CouldNotDeleteInstallFile_Msg, installFilePath);
					GspConfigSuccessfullyImported = false;
				}
				catch (System.Security.SecurityException)
				{
					GspConfigUpdateErrorMsg = String.Format(Resources.GalleryServerPro.Installer_Upgrade_Finished_CouldNotDeleteInstallFile_Msg, installFilePath);
					GspConfigSuccessfullyImported = false;
				}
			}
		}

		private void DisableInstallAndUpgradeWizards()
		{
			// When the upgrade is successful, we want to automatically disable the install and upgrade wizards so they can't be run again.
			// Note: This function is also in the install wizard page.
			if (UpgradeSuccessful)
			{
				string upgradeWizardFilePath = Server.MapPath(Util.GetUrl("/pages/upgrade.ascx"));
				string installWizardFilePath = Server.MapPath(Util.GetUrl("/pages/install.ascx"));
				string[] wizardFilePaths = new string[] { upgradeWizardFilePath, installWizardFilePath };

				bool wizardDisableFailed = false;

				foreach (string wizardFilePath in wizardFilePaths)
				{
					if (File.Exists(wizardFilePath))
					{
						try
						{
							const string hiddenFieldUpgradeEnabled = "<asp:HiddenField ID=\"ENABLE_SETUP\" runat=\"server\" Value=\"true\" />";
							const string hiddenFieldUpgradeDisabled = "<asp:HiddenField ID=\"ENABLE_SETUP\" runat=\"server\" Value=\"false\" />";

							string[] upgradeWizardFileLines = File.ReadAllLines(wizardFilePath);
							StringBuilder newFile = new StringBuilder();
							bool foundHiddenField = false;

							foreach (string line in upgradeWizardFileLines)
							{
								if (!foundHiddenField && line.Contains(hiddenFieldUpgradeEnabled))
								{
									newFile.AppendLine(line.Replace(hiddenFieldUpgradeEnabled, hiddenFieldUpgradeDisabled));
									foundHiddenField = true;
								}
								else
								{
									newFile.AppendLine(line);
								}
							}

							File.WriteAllText(wizardFilePath, newFile.ToString());
						}
						catch (IOException) { wizardDisableFailed = true; }
						catch (UnauthorizedAccessException) { wizardDisableFailed = true; }
						catch (System.Security.SecurityException) { wizardDisableFailed = true; }
					}
				}

				WizardsSuccessfullyDisabled = !wizardDisableFailed;
			}
		}

		private static string GetExceptionDetails(Exception ex)
		{
			string msg = String.Format(@"{0} {1}
Stack Trace: {2}
", ex.GetType(), ex.Message, ex.StackTrace);

			if (ex.InnerException != null)
			{
				msg += String.Format(@"Inner Exception:
{0} {1}
Inner Exception Stack Trace:
{2}
", ex.InnerException.GetType(), ex.InnerException.Message, ex.InnerException.StackTrace);
			}

			return msg;
		}

		#endregion
	}
}