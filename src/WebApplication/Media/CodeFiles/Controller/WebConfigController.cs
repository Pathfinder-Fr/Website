using System;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using GalleryServerPro.Business;
using GalleryServerPro.ErrorHandler.CustomExceptions;
using GalleryServerPro.Web.Entity;

namespace GalleryServerPro.Web.Controller
{
	/// <summary>
	/// Contains functionality for interacting with the web.config configuration file, including write operations.
	/// All write operations work in Medium Trust as long as the IIS process identity has write NTFS permissions on the file.
	/// </summary>
	public static class WebConfigController
	{
		#region Private Fields

		private static readonly string _webConfigPath = HttpContext.Current.Server.MapPath("~/web.config");

		#endregion

		#region Public Static Methods

		/// <summary>
		/// Gets an instance of <see cref="WebConfigEntity"/> that contains commonly referenced settings from web.config. The 
		/// entity can be updated with new values and then passed to the <see cref="Save"/> method for persisting back to the file system.
		/// </summary>
		/// <returns>Returns an instance of <see cref="WebConfigEntity"/> that contains commonly referenced settings from web.config.</returns>
		public static WebConfigEntity GetWebConfigEntity()
		{
			WebConfigEntity wce = new WebConfigEntity();

			XmlDocument webConfig = GetWebConfigXmlDoc();
			XPathNavigator xpathNav = webConfig.CreateNavigator();

			wce.SQLiteConnectionStringValue = GetSQLiteConnectionString(xpathNav);

			wce.SqlServerConnectionStringValue = GetSqlServerConnectionString(xpathNav);

			wce.GalleryServerProConfigSection = GetGalleryServerProConfigSection(xpathNav);

			wce.MembershipDefaultProvider = GetMembershipProvider(xpathNav);

			wce.RoleDefaultProvider = GetRoleProvider(xpathNav);

			wce.GalleryDataDefaultProvider = GetGalleryDataProvider(xpathNav);

			wce.DataProvider = GetDataProvider(wce);

			wce.IsWriteable = IsWebConfigWriteable();

			return wce;
		}

		/// <summary>
		/// Persist the configuration data to web.config.
		/// </summary>
		/// <param name="webConfigEntity">An instance of <see cref="WebConfigEntity"/> that contains data to save to web.config.</param>
		/// <exception cref="UnauthorizedAccessException">Thrown when the IIS application pool identity does not have
		/// write access to web.config.</exception>
		public static void Save(WebConfigEntity webConfigEntity)
		{
			string ns; // Holds the xmlns attribute (will be empty in most cases)
			XmlDocument xmlDoc = GetWebConfigXmlDoc(out ns);

			#region Update connection strings

			// Update SQLite and SQL Server connection strings.
			WriteConnectionStringToWebConfig(xmlDoc, WebConfigEntity.SQLiteConnectionStringName, webConfigEntity.SQLiteConnectionStringValue);
			WriteConnectionStringToWebConfig(xmlDoc, WebConfigEntity.SqlServerConnectionStringName, webConfigEntity.SqlServerConnectionStringValue);

			#endregion

			#region Gallery Server Pro config section

			if (webConfigEntity.GalleryServerProConfigSectionHasChanges)
			{
				XmlNode galleryServerProNode = xmlDoc.SelectSingleNode(@"/configuration/system.web/galleryServerPro");

				if (galleryServerProNode == null)
					throw new WebException("Could not find the galleryServerPro section in web.config.");

				if (galleryServerProNode.ParentNode == null)
					throw new WebException("The galleryServerPro section in web.config does not have a parent.");

				// Get a fragment and slide the changed data into it.
				XmlDocumentFragment frag = xmlDoc.CreateDocumentFragment();
				frag.InnerXml = webConfigEntity.GalleryServerProConfigSection;

				galleryServerProNode.ParentNode.ReplaceChild(frag, galleryServerProNode);
			}

			#endregion

			#region Update membership, role and gallery data

			if (webConfigEntity.MembershipDefaultProvider != MembershipDataProvider.Unknown)
			{
				// Update membership
				XmlNode membershipNode = xmlDoc.SelectSingleNode(@"/configuration/system.web/membership");

				if (membershipNode == null)
					throw new WebException("Could not find the membership section in web.config.");

				membershipNode.Attributes["defaultProvider"].Value = webConfigEntity.MembershipDefaultProvider.ToString();
			}

			if (webConfigEntity.RoleDefaultProvider != RoleDataProvider.Unknown)
			{
				// Update roles provider
				XmlNode roleNode = xmlDoc.SelectSingleNode(@"/configuration/system.web/roleManager");

				if (roleNode == null)
					throw new WebException("Could not find the roleManager section in web.config.");
			
				roleNode.Attributes["defaultProvider"].Value = webConfigEntity.RoleDefaultProvider.ToString();
			}

			// Update gallery data provider
			if (webConfigEntity.GalleryDataDefaultProvider != GalleryDataProvider.Unknown)
			{
				XmlNode galleryDataNode = xmlDoc.SelectSingleNode(@"/configuration/system.web/galleryServerPro/dataProvider");

				if (galleryDataNode == null)
					throw new WebException("Could not find the galleryServerPro/dataProvider section in web.config.");

				galleryDataNode.Attributes["defaultProvider"].Value = webConfigEntity.GalleryDataDefaultProvider.ToString();
			}

			#endregion

			#region Save to disk

			// If the config file had a root namespace, restore it now.
			if (!String.IsNullOrEmpty(ns))
				xmlDoc.DocumentElement.Attributes["xmlns"].Value = ns;

			// Persist changes to disk.
			XmlWriterSettings xws = new XmlWriterSettings();
			xws.Indent = true;
			xws.Encoding = new UTF8Encoding(false);

			XmlWriter writer = XmlWriter.Create(_webConfigPath, xws);

			xmlDoc.Save(writer);

			writer.Close();

			#endregion
		}

		/// <summary>
		/// Determines whether the installer has permission to update the web.config file.
		/// </summary>
		/// <returns>Returns <c>true</c> if the installer has permission to update the web.config file; otherwise returns <c>false</c>.</returns>
		/// <remarks>This method also exists in upgrade.ascx.</remarks>
		public static bool IsWebConfigUpdateable()
		{
			try
			{
				// Can we open the web.config file for writing? This will fail when the web.config file does not have 'write' NTFS permission.
				using (File.OpenWrite(_webConfigPath)) { }

				return true;
			}
			catch (UnauthorizedAccessException)
			{
				return false;
			}
		}

		/// <summary>
		/// Determines whether the web.config file contains entries for the selected data provider for the membership, role, 
		/// and gallery data providers. Note that this does not verify the provider is selected (that is, is specified in the 
		/// defaultProvider attribute); it only verifies that the entry exists within those sections. Use the method
		/// AreProvidersSpecifiedInWebConfig to determine whether the selected provider is selected as the currently active
		/// provider.
		/// </summary>
		/// <param name="providerName">The provider to check for in web.config (e.b. SQLite or SqlServer).</param>
		/// <returns>Returns <c>true</c> if the membership, role, and gallery data providers in web.config contain entries
		/// corresponding to the data provider selected by the user; otherwise returns <c>false</c>.</returns>
		/// <remarks>Unfortunately, we cannot use WebConfigurationManager.GetSection, as that works only in Full Trust.</remarks>
		public static bool AreProvidersAvailableInWebConfig(string providerName)
		{
			string membershipProviderName = (providerName == "SQLite" ? "SQLiteMembershipProvider" : "SqlMembershipProvider");
			string roleProviderName = (providerName == "SQLite" ? "SQLiteRoleProvider" : "SqlRoleProvider");
			string galleryDataProviderName = (providerName == "SQLite" ? "SQLiteGalleryServerProProvider" : "SqlServerGalleryServerProProvider");

			XmlDocument webConfig = GetWebConfigXmlDoc();
			XPathNavigator xpathNav = webConfig.CreateNavigator();

			string xpathQuery = String.Format("/configuration/system.web/membership/providers/add[@name=\"{0}\"]", membershipProviderName);
			bool membershipProviderIsAvailable = (xpathNav.SelectSingleNode(xpathQuery) != null);

			xpathQuery = String.Format("/configuration/system.web/roleManager/providers/add[@name=\"{0}\"]", roleProviderName);
			bool roleProviderIsAvailable = (xpathNav.SelectSingleNode(xpathQuery) != null);

			xpathQuery = String.Format("/configuration/system.web/galleryServerPro/dataProvider/providers/add[@name=\"{0}\"]", galleryDataProviderName);
			bool galleryDataProviderIsAvailable = (xpathNav.SelectSingleNode(xpathQuery) != null);

			return (membershipProviderIsAvailable && roleProviderIsAvailable && galleryDataProviderIsAvailable);
		}

		/// <summary>
		/// Update the timestamp for the web.config file. This has the effect of restarting the application.
		/// </summary>
		public static void Touch()
		{
			File.SetLastWriteTime(_webConfigPath, DateTime.Now);
		}

		#endregion

		#region Private Static Methods

		private static XmlDocument GetWebConfigXmlDoc()
		{
			string ns;
			return GetWebConfigXmlDoc(out ns);
		}

		private static XmlDocument GetWebConfigXmlDoc(out string ns)
		{
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(_webConfigPath);

			if (xmlDoc == null)
				throw new WebException(String.Format("Could not load {0}.", _webConfigPath));

			if (xmlDoc.DocumentElement == null)
				throw new WebException(String.Format("Could not find the root element of {0}.", _webConfigPath));

			// If the root element has a namespace, save it to a temporary variable and then set it to an empty string. 
			// This will allow us to locate nodes without having to specify a namespace in the xpath. Normally there shouldn't 
			// be a namespace on the <configuration> element of web.config, but versions of the ASP.NET Configuration Tool 
			// before VS 2008 incorrectly added the following: xmlns="http://schemas.microsoft.com/.NetConfiguration/v2.0"
			// We'll add it back before saving it so there isn't any change to the file as stored on disk.
			ns = String.Empty;
			if (xmlDoc.DocumentElement.HasAttribute("xmlns"))
			{
				ns = xmlDoc.DocumentElement.Attributes["xmlns"].Value;
				xmlDoc.DocumentElement.Attributes["xmlns"].Value = "";
				xmlDoc.LoadXml(xmlDoc.DocumentElement.OuterXml);
			}
			return xmlDoc;
		}

		private static string GetSQLiteConnectionString(XPathNavigator xpathNav)
		{
			string xpathQuery = String.Format("/configuration/connectionStrings/add[@name=\"{0}\"]", Constants.SQLITE_CN_STRING_NAME);
			XPathNavigator node = xpathNav.SelectSingleNode(xpathQuery);
			return (node != null ? node.GetAttribute("connectionString", String.Empty) : String.Empty);
		}

		private static string GetSqlServerConnectionString(XPathNavigator xpathNav)
		{
			string xpathQuery = String.Format("/configuration/connectionStrings/add[@name=\"{0}\"]", Constants.SQL_SERVER_CN_STRING_NAME);
			XPathNavigator node = xpathNav.SelectSingleNode(xpathQuery);
			return (node != null ? node.GetAttribute("connectionString", String.Empty) : String.Empty);
		}

		private static string GetGalleryServerProConfigSection(XPathNavigator xpathNav)
		{
			const string xpathQuery = "/configuration/system.web/galleryServerPro";
			XPathNavigator node = xpathNav.SelectSingleNode(xpathQuery);
			return (node != null ? node.OuterXml : String.Empty);
		}

		private static MembershipDataProvider GetMembershipProvider(XPathNavigator xpathNav)
		{
			const string xpathQuery = "/configuration/system.web/membership";
			XPathNavigator node = xpathNav.SelectSingleNode(xpathQuery);
			MembershipDataProvider membershipDataProvider = MembershipDataProvider.Unknown;
			if (node != null)
			{
				try
				{
					membershipDataProvider = (MembershipDataProvider)Enum.Parse(typeof(MembershipDataProvider), node.GetAttribute("defaultProvider", String.Empty), false);
				}
				catch (ArgumentException) { }
			}
			return membershipDataProvider;
		}

		private static RoleDataProvider GetRoleProvider(XPathNavigator xpathNav)
		{
			const string xpathQuery = "/configuration/system.web/roleManager";
			XPathNavigator node = xpathNav.SelectSingleNode(xpathQuery);
			RoleDataProvider roleDataProvider = RoleDataProvider.Unknown;
			if (node != null)
			{
				try
				{
					roleDataProvider = (RoleDataProvider)Enum.Parse(typeof(RoleDataProvider), node.GetAttribute("defaultProvider", String.Empty), false);
				}
				catch (ArgumentException) { }
			}
			return roleDataProvider;
		}

		private static GalleryDataProvider GetGalleryDataProvider(XPathNavigator xpathNav)
		{
			const string xpathQuery = "/configuration/system.web/galleryServerPro/dataProvider";
			XPathNavigator node = xpathNav.SelectSingleNode(xpathQuery);
			GalleryDataProvider galleryDataProvider = GalleryDataProvider.Unknown;
			if (node != null)
			{
				try
				{
					galleryDataProvider = (GalleryDataProvider)Enum.Parse(typeof(GalleryDataProvider), node.GetAttribute("defaultProvider", String.Empty), false);
				}
				catch (ArgumentException) { }
			}
			return galleryDataProvider;
		}

		private static ProviderDataStore GetDataProvider(WebConfigEntity wce)
		{
			// Update the data provider. Each provider (membership, roles, and gallery data) could theoretically use a different 
			// database technology, but we are most interested in where the gallery data is stored, so use that one.
			ProviderDataStore dataProvider = ProviderDataStore.Unknown;
			switch (wce.GalleryDataDefaultProvider)
			{
				case GalleryDataProvider.SQLiteGalleryServerProProvider:
					dataProvider = ProviderDataStore.SQLite;
					break;
				case GalleryDataProvider.SqlServerGalleryServerProProvider:
					dataProvider = ProviderDataStore.SqlServer;
					break;
			}
			return dataProvider;
		}

		/// <summary>
		/// Determines whether the application has permission to update the web.config file.
		/// </summary>
		/// <returns>Returns <c>true</c> if the application has permission to update the web.config file; otherwise returns <c>false</c>.</returns>
		private static bool IsWebConfigWriteable()
		{
			bool isWriteable;
			try
			{
				// Can we open and save the web.config file? This will fail when the app is running under Medium Trust and when
				// the web.config file does not have 'write' NTFS permission.
				using (File.OpenWrite(_webConfigPath)) { }
				isWriteable = true;
			}
			catch
			{
				isWriteable = false;
			}
			return isWriteable;
		}

		private static void WriteConnectionStringToWebConfig(XmlDocument xmlDoc, string cnName, string cnValue)
		{
			if (String.IsNullOrEmpty(cnValue))
				return;

			XmlNode cnStringNode = xmlDoc.SelectSingleNode(String.Format(@"/configuration/connectionStrings/add[@name=""{0}""]", cnName));

			if (cnStringNode != null)
			{
				// Update connection string.
				cnStringNode.Attributes["connectionString"].Value = cnValue;
			}
			else
			{
				// Add connection string.
				cnStringNode = xmlDoc.CreateNode(XmlNodeType.Element, "add", null);

				XmlAttribute nameAttribute = xmlDoc.CreateAttribute("name");
				nameAttribute.Value = cnName;
				cnStringNode.Attributes.SetNamedItem(nameAttribute);

				XmlAttribute cnStringAttribute = xmlDoc.CreateAttribute("connectionString");
				cnStringAttribute.Value = cnValue;
				cnStringNode.Attributes.SetNamedItem(cnStringAttribute);

				XmlElement cnStringsElement = (XmlElement)xmlDoc.SelectSingleNode(@"/configuration/connectionStrings");

				if (cnStringsElement == null)
					throw new WebException("Could not find the connectionStrings section in web.config.");

				cnStringsElement.AppendChild(cnStringNode);
			}
		}

		#endregion
	}
}
