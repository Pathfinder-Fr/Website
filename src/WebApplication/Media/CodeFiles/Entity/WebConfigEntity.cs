using GalleryServerPro.Business;

namespace GalleryServerPro.Web.Entity
{
	/// <summary>
	/// A simple object that contains several configuration settings in web.config.
	/// This entity is designed to be an updateable object whose properties can be changed and passed to the 
	/// <see cref="GalleryServerPro.Web.Controller.WebConfigController"/> for persisting back to the configuration file on disk.
	/// Therefore, this entity is typically used only in scenarios where we must persist changes to the config file, such as 
	/// in the Install Wizard.
	/// </summary>
	public class WebConfigEntity
	{
		private string _galleryServerProConfigSection = string.Empty;
		private bool _galleryServerProConfigSectionHasChanges;

		public const string SqlServerConnectionStringName = Constants.SQL_SERVER_CN_STRING_NAME;
		public const string SQLiteConnectionStringName = Constants.SQLITE_CN_STRING_NAME;

		/// <summary>
		/// Gets the connection string named "SqlServerDbConnection" in the connectionStrings section of web.config.
		/// </summary>
		/// <value>The SQL Server database connection string.</value>
		public string SqlServerConnectionStringValue;

		/// <summary>
		/// Gets the connection string named "SQLiteDbConnection" in the connectionStrings section of web.config.
		/// </summary>
		/// <value>The SQLite database connection string.</value>
		public string SQLiteConnectionStringValue;

		public ProviderDataStore DataProvider = ProviderDataStore.Unknown;
		public MembershipDataProvider MembershipDefaultProvider = MembershipDataProvider.Unknown;
		public RoleDataProvider RoleDefaultProvider = RoleDataProvider.Unknown;
		public GalleryDataProvider GalleryDataDefaultProvider = GalleryDataProvider.Unknown;

		public bool IsWriteable;

		/// <summary>
		/// Gets or sets the galleryserverpro section in web.config.
		/// </summary>
		public string GalleryServerProConfigSection
		{
			get { return _galleryServerProConfigSection; }
			set
			{
				if (!_galleryServerProConfigSection.Equals(value))
				{
					_galleryServerProConfigSection = value;
					_galleryServerProConfigSectionHasChanges = true;
				}
			}
		}

		/// <summary>
		/// Gets a value indicating whether <see cref="GalleryServerProConfigSection" /> has a different value than what is
		/// in the web.config file.
		/// </summary>
		public bool GalleryServerProConfigSectionHasChanges
		{
			get { return _galleryServerProConfigSectionHasChanges; }
		}
	}
}
