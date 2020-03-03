using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Web;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.Business.Metadata;
using GalleryServerPro.Web.Entity;

namespace GalleryServerPro.Web.Controller
{
	/// <summary>
	/// Contains functionality for interacting with galleries and gallery settings.
	/// </summary>
	public static class GalleryController
	{
		#region Fields

		private static readonly object _sharedLock = new object();
		private static bool _isInitialized;

		#endregion

		#region Properties

		/// <summary>
		/// Gets a value indicating whether the Gallery Server Pro code has been initializaed.
		/// The code is initialized by calling <see cref="InitializeGspApplication" />.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the code is initialized; otherwise, <c>false</c>.
		/// </value>
		public static bool IsInitialized
		{
			get { return _isInitialized; }
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Initialize the Gallery Server Pro application. This method is designed to be run at application startup. The business layer
		/// is initialized with the current trust level and a few configuration settings. The business layer also initializes
		/// the data store, including verifying a minimal level of data integrity, such as at least one record for the root album.
		/// Initialization that requires an HttpContext is also performed. When this method completes, <see cref="IAppSetting.IsInitialized" />
		/// will be <c>true</c>, but <see cref="GalleryController.IsInitialized" /> will be <c>true</c> only when an HttpContext instance
		/// exists. If this function is initially called from a place where an HttpContext doesn't exist, it will automatically be called 
		/// again later, eventually being called from a place where an HttpContext does exist, thus completing app initialization.
		/// </summary>
		public static void InitializeGspApplication()
		{
			try
			{
				InitializeApplication();

				lock (_sharedLock)
				{
					if (IsInitialized)
						return;

					if (HttpContext.Current != null)
					{
						// Set application key so ComponentArt knows it is properly licensed.
						HttpContext.Current.Application["ComponentArtWebUI_AppKey"] = "This edition of ComponentArt Web.UI is licensed for Gallery Server Pro application only.";

						// Add a dummy value to session so that the session ID remains constant. (This is required by Util.GetRolesForUser())
						// Check for null session first. It will be null when this is triggered by a web method that does not have
						// session enabled (that is, the [WebMethod(EnableSession = true)] attribute). That's OK because the roles functionality
						// will still work (we might have to an extra data call, though), and we don't want the overhead of session for some web methods.
						if (HttpContext.Current.Session != null)
							HttpContext.Current.Session.Add("1", "1");

						// Update the user accounts in a few gallery settings. The DotNetNuke version requires this call to happen when there
						// is an HttpContext, so to reduce differences between the two branches we put it here.
						AddMembershipDataToGallerySettings();

						_isInitialized = true;
					}
				}
			}
			catch (ThreadAbortException) { }
			catch (Exception ex)
			{
				// Let the error handler deal with it. It will decide whether to transfer the user to a friendly error page.
				// If the function returns, that means it didn't redirect, so we should re-throw the exception.
				AppErrorController.HandleGalleryException(ex);
				throw;
			}
		}

		/// <summary>
		/// Get a list of galleries the current user can administer. Site administrators can view all galleries, while gallery
		/// administrators may have access to zero or more galleries.
		/// </summary>
		/// <returns>Returns an <see cref="IGalleryCollection" /> containing the galleries the current user can administer.</returns>
		[DataObjectMethod(DataObjectMethodType.Select)]
		public static IGalleryCollection GetGalleriesCurrentUserCanAdminister()
		{
			return UserController.GetGalleriesCurrentUserCanAdminister();
		}

		/// <summary>
		/// Persist the <paramref name="gallery" /> to the data store.
		/// </summary>
		/// <param name="gallery">The gallery to persist to the data store.</param>
		[DataObjectMethod(DataObjectMethodType.Insert)]
		public static void AddGallery(Business.Gallery gallery)
		{
			gallery.Save();
		}

		/// <summary>
		/// Permanently delete the specified <paramref name="gallery" /> from the data store, including all related records. This action cannot
		/// be undone.
		/// </summary>
		/// <param name="gallery">The gallery to delete.</param>
		[DataObjectMethod(DataObjectMethodType.Delete)]
		public static void DeleteGallery(Business.Gallery gallery)
		{
			gallery.Delete();

			ProfileController.DeleteProfileForGallery(gallery);
		}

		/// <summary>
		/// Persist the <paramref name="gallery" /> to the data store.
		/// </summary>
		/// <param name="gallery">The gallery to persist to the data store.</param>
		[DataObjectMethod(DataObjectMethodType.Update)]
		public static void UpdateGallery(Business.Gallery gallery)
		{
			gallery.Save();
		}

		/// <summary>
		/// Create a sample album and media object. This method is intended to be invoked once just after the application has been 
		/// installed.
		/// </summary>
		/// <param name="galleryId">The ID for the gallery where the sample objects are to be created.</param>
		public static void CreateSampleObjects(int galleryId)
		{
			if (Factory.LoadGallerySetting(galleryId).MediaObjectPathIsReadOnly)
			{
				return;
			}

			DateTime currentTimestamp = DateTime.Now;
			IAlbum sampleAlbum = null;
			foreach (IAlbum album in Factory.LoadRootAlbumInstance(galleryId).GetChildGalleryObjects(GalleryObjectType.Album))
			{
				if (album.DirectoryName == "Samples")
				{
					sampleAlbum = album;
					break;
				}
			}
			if (sampleAlbum == null)
			{
				// Create sample album.
				sampleAlbum = Factory.CreateEmptyAlbumInstance(galleryId);

				sampleAlbum.Parent = Factory.LoadRootAlbumInstance(galleryId);
				sampleAlbum.Title = "Samples";
				sampleAlbum.DirectoryName = "Samples";
				sampleAlbum.Summary = "Welcome to Gallery Server Pro!";
				sampleAlbum.CreatedByUserName = "System";
				sampleAlbum.DateAdded = currentTimestamp;
				sampleAlbum.LastModifiedByUserName = "System";
				sampleAlbum.DateLastModified = currentTimestamp;
				sampleAlbum.Save();
			}

			// Look for sample image in sample album.
			IGalleryObject sampleImage = null;
			foreach (IGalleryObject image in sampleAlbum.GetChildGalleryObjects(GalleryObjectType.Image))
			{
				if (image.Original.FileName == Constants.SAMPLE_IMAGE_FILENAME)
				{
					sampleImage = image;
					break;
				}
			}

			if (sampleImage == null)
			{
				// Sample image not found. Pull image from assembly and save to disk (if needed), then create a media object from it.
				string sampleDirPath = Path.Combine(Factory.LoadGallerySetting(galleryId).FullMediaObjectPath, sampleAlbum.DirectoryName);
				string sampleImageFilepath = Path.Combine(sampleDirPath, Constants.SAMPLE_IMAGE_FILENAME);

				if (!File.Exists(sampleImageFilepath))
				{
					Assembly asm = Assembly.GetExecutingAssembly();
					using (Stream stream = asm.GetManifestResourceStream(String.Concat("GalleryServerPro.Web.gs.images.", Constants.SAMPLE_IMAGE_FILENAME)))
					{
						if (stream != null)
						{
							BinaryWriter bw = new BinaryWriter(File.Create(sampleImageFilepath));
							byte[] buffer = new byte[stream.Length];
							stream.Read(buffer, 0, (int)stream.Length);
							bw.Write(buffer);
							bw.Flush();
							bw.Close();
						}
					}
				}

				if (File.Exists(sampleImageFilepath))
				{
					// Temporarily change a couple settings so that the thumbnail and compressed images are high quality.
					IGallerySettings gallerySettings = Factory.LoadGallerySetting(galleryId);
					int optTriggerSizeKb = gallerySettings.OptimizedImageTriggerSizeKb;
					int thumbImageJpegQuality = gallerySettings.ThumbnailImageJpegQuality;
					gallerySettings.ThumbnailImageJpegQuality = 95;
					gallerySettings.OptimizedImageTriggerSizeKb = 200;

					// Create the media object from the file.
					IGalleryObject image = Factory.CreateImageInstance(new FileInfo(sampleImageFilepath), sampleAlbum);
					image.Title = "Margaret, Skyler and Roger Martin (July 2010)";
					image.CreatedByUserName = "System";
					image.DateAdded = currentTimestamp;
					image.LastModifiedByUserName = "System";
					image.DateLastModified = currentTimestamp;
					image.Save();

					// Restore the default settings.
					gallerySettings.OptimizedImageTriggerSizeKb = optTriggerSizeKb;
					gallerySettings.ThumbnailImageJpegQuality = thumbImageJpegQuality;
				}
			}
		}

		/// <summary>
		/// Perform a synchronize according to the specified <paramref name="syncSettingsObject" />.
		/// When complete, update the <see cref="IGallerySettings.LastAutoSync" /> property to the current date/time and persist
		/// to the data store. The <paramref name="syncSettingsObject" /> is specified as <see cref="Object" /> so that this method can 
		/// be invoked on a separate thread using <see cref="System.Threading.Thread" />. Any exceptions that occur during the
		/// sync are caught and logged to the event log. NOTE: This method does not perform any security checks; the calling
		/// code must ensure the requesting user is authorized to run the sync.
		/// </summary>
		/// <param name="syncSettingsObject">The synchronize settings object. It must be of type <see cref="SynchronizeSettingsEntity" />.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="syncSettingsObject" /> is null.</exception>
		/// <exception cref="ArgumentException">Thrown when <paramref name="syncSettingsObject" /> is not of type 
		/// <see cref="SynchronizeSettingsEntity" />.</exception>
		public static void Synchronize(object syncSettingsObject)
		{
			if (syncSettingsObject == null)
			{
				throw new ArgumentNullException("syncSettingsObject");
			}

			SynchronizeSettingsEntity syncSettings = syncSettingsObject as SynchronizeSettingsEntity;

			if (syncSettings == null)
			{
				throw new ArgumentException(String.Format("The parameter must be an instance of SynchronizeSettingsEntity. Instead, it was {0}.", syncSettingsObject.GetType()));
			}

			IAlbum album = syncSettings.AlbumToSynchronize;

			AppErrorController.LogEvent(String.Format("INFO (not an error): {0} synchronization of album '{1}' (ID {2}) and all child albums has started.", syncSettings.SyncInitiator, album.Title, album.Id), album.GalleryId);

			try
			{
				SynchronizationManager synchMgr = new SynchronizationManager(album.GalleryId);

				synchMgr.IsRecursive = syncSettings.IsRecursive;
				synchMgr.OverwriteThumbnail = syncSettings.OverwriteThumbnails;
				synchMgr.OverwriteOptimized = syncSettings.OverwriteOptimized;
				synchMgr.RegenerateMetadata = syncSettings.RegenerateMetadata;

				synchMgr.Synchronize(Guid.NewGuid().ToString(), album, "Admin");

				if (syncSettings.SyncInitiator == SyncInitiator.AutoSync)
				{
					// Update the date/time of this auto-sync and save to data store.
					IGallerySettings gallerySettings = Factory.LoadGallerySetting(album.GalleryId, true);
					gallerySettings.LastAutoSync = DateTime.Now;
					gallerySettings.Save(false);

					// The above Save() only updated the database; now we need to update the in-memory copy of the settings.
					// We have to do this instead of simply calling gallerySettings.Save(true) because that overload causes the
					// gallery settings to be cleared and reloaded, and the reloading portion done by the AddMembershipDataToGallerySettings
					// function fails in DotNetNuke because there isn't a HttpContext.Current instance at this moment (because this code is
					// run on a separate thread).
					IGallerySettings gallerySettingsReadOnly = Factory.LoadGallerySetting(album.GalleryId, false);
					gallerySettingsReadOnly.LastAutoSync = gallerySettings.LastAutoSync;
				}
			}
			catch (Exception ex)
			{
				AppErrorController.LogError(ex, album.GalleryId);
			}

			AppErrorController.LogEvent(String.Format("INFO (not an error): {0} synchronization of album '{1}' (ID {2}) and all child albums is complete.", syncSettings.SyncInitiator, album.Title, album.Id), album.GalleryId);
		}

		#endregion

		#region Private Functions

		/// <summary>
		/// Initialize the components of the Gallery Server Pro application that do not require access to an HttpContext.
		/// This method is designed to be run at application startup. The business layer
		/// is initialized with the current trust level and a few configuration settings. The business layer also initializes
		/// the data store, including verifying a minimal level of data integrity, such as at least one record for the root album.
		/// </summary>
		/// <remarks>This is the only method, apart from those invoked through web services, that is not handled by the global error
		/// handling routine in Gallery.cs. This method wraps its calls in a try..catch that passes any exceptions to
		/// <see cref="AppErrorController.HandleGalleryException(Exception)"/>. If that method does not transfer the user to a friendly error page, the exception
		/// is re-thrown.</remarks>
		private static void InitializeApplication()
		{
			lock (_sharedLock)
			{
				if (AppSetting.Instance.IsInitialized)
					return;

				Business.Gallery.GalleryCreated += new EventHandler<GalleryCreatedEventArgs>(GalleryCreated);

				GallerySettings.GallerySettingsSaved += new EventHandler<GallerySettings.GallerySettingsEventArgs>(GallerySettingsSaved);

				GalleryObject.MetadataLoaded += new EventHandler(GalleryObjectMetadataLoaded);

				// Set web-related variables in the business layer and initialize the data store.
				InitializeBusinessLayer();

				UserController.ProcessInstallerFile();

				// Make sure installation has its own unique encryption key.
				ValidateEncryptionKey();
			}
		}

		/// <summary>
		/// Set up the business layer with information about this web application, such as its trust level and a few settings
		/// from the configuration file.
		/// </summary>
		/// <exception cref="GalleryServerPro.ErrorHandler.CustomExceptions.CannotWriteToDirectoryException">
		/// Thrown when Gallery Server Pro is unable to write to, or delete from, the media objects directory.</exception>
		private static void InitializeBusinessLayer()
		{
			// Determine the trust level this web application is running in and set to a global variable. This will be used 
			// throughout the application to gracefully degrade when we are not at Full trust.
			ApplicationTrustLevel trustLevel = Util.GetCurrentTrustLevel();

			// Get the application path so that the business layer (and any dependent layers) has access to it. Don't use 
			// HttpContext.Current.Request.PhysicalApplicationPath because in some cases HttpContext.Current won't be available
			// (for example, when the DotNetNuke search engine indexer causes this code to trigger).
			string physicalApplicationPath = AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.Length - 1);
			physicalApplicationPath = physicalApplicationPath.Replace("/", "\\");

			// Pass these values to our global app settings instance, where the values can be used throughout the application.
			AppSetting.Instance.Initialize(trustLevel, physicalApplicationPath, Constants.APP_NAME);
		}

		/// <summary>
		/// Verify that the encryption key in the application settings has been changed from its original, default value. The key is 
		/// updated with a new value if required. Each installation should have a unique key.
		/// </summary>
		private static void ValidateEncryptionKey()
		{
			// This function is called from a function using a lock, so we don't need to do our own locking.
			if (AppSetting.Instance.EncryptionKey.Equals(Constants.ENCRYPTION_KEY, StringComparison.Ordinal))
			{
				AppSetting.Instance.Save(null, null, null, Util.GenerateNewEncryptionKey(), null, null, null, null, null, null, null, null);
			}
		}

		/// <summary>
		/// Adds the user account information to gallery settings. Since the business layer does not have a reference to System.Web.dll,
		/// it could not load membership data when the gallery settings were first initialized. We know that information now, so let's
		/// populate the user accounts with the user data.
		/// </summary>
		private static void AddMembershipDataToGallerySettings()
		{
			// The UserAccount objects should have been created and initially populated with the UserName property,
			// so we'll use the user name to retrieve the user's info and populate the rest of the properties on each object.
			foreach (IGallery gallery in Factory.LoadGalleries())
			{
				IGallerySettings gallerySetting = Factory.LoadGallerySetting(gallery.GalleryId);

				// Populate user account objects with membership data
				foreach (IUserAccount userAccount in gallerySetting.UsersToNotifyWhenAccountIsCreated)
				{
					UserController.LoadUser(userAccount);
				}

				foreach (IUserAccount userAccount in gallerySetting.UsersToNotifyWhenErrorOccurs)
				{
					UserController.LoadUser(userAccount);
				}
			}
		}

		/// <summary>
		/// Adds GPS location URL metadata item to the <see cref="IGalleryObject.MetadataItems" />
		/// collection of <paramref name="galleryObject" />. If the gallery object's metadata does not contain GPS data or the
		/// visibility of the GPS map link is turned off, no action is taken.
		/// </summary>
		/// <param name="galleryObject">The gallery object.</param>
		/// <param name="metadataDisplaySettings">The metadata display settings.</param>
		/// <remarks>The metadata item is added with <see cref="IGalleryObjectMetadataItem.HasChanges" /> = <c>false</c> to prevent it 
		/// from getting persisted to the database. This allows the hyperlink to be regenerated from the template, thus incorporating the
		/// most recent template and other media object properties (such as title). Because the item is linked to the media object, it is
		/// automatically included in the cache of media objects.
		/// This function is identical to <see cref="AddGpsDestLocationWithMapLink" /> except it uses the destination GPS settings.</remarks>
		private static void AddGpsLocationWithMapLink(IGalleryObject galleryObject, IMetadataDefinitionCollection metadataDisplaySettings)
		{
			if (!metadataDisplaySettings.Find(FormattedMetadataItemName.GpsLocationWithMapLink).IsVisible)
				return; // The map link is disabled, so there is nothing to do.

			IGalleryObjectMetadataItemCollection metadata = galleryObject.MetadataItems;
			IGalleryObjectMetadataItem gpsLocation;

			if (metadata.TryGetMetadataItem(FormattedMetadataItemName.GpsLocation, out gpsLocation) && (!metadata.Contains(FormattedMetadataItemName.GpsLocationWithMapLink)))
			{
				// We have a GPS location but have not yet created the URL'd version. Do so now and add it to the collection.
				IGalleryObjectMetadataItem latitude;
				IGalleryObjectMetadataItem longitude;
				bool foundLatitude = metadata.TryGetMetadataItem(FormattedMetadataItemName.GpsLatitude, out latitude);
				bool foundLongitude = metadata.TryGetMetadataItem(FormattedMetadataItemName.GpsLongitude, out longitude);

				if (foundLatitude && foundLongitude)
				{
					string url = GetGpsMapUrl(galleryObject, latitude.Value, longitude.Value, gpsLocation.Value);

					if (!String.IsNullOrEmpty(url))
					{
						// Add to meta collection. Specify false for HasChanges to prevent it from getting persisted back to the database.
						galleryObject.MetadataItems.AddNew(int.MinValue, FormattedMetadataItemName.GpsLocationWithMapLink, Resources.GalleryServerPro.Metadata_GpsLocationWithMapLink, url, false);
					}
				}
			}
		}

		/// <summary>
		/// Adds GPS destination location URL metadata item to the <see cref="IGalleryObject.MetadataItems" />
		/// collection of <paramref name="galleryObject" />. If the gallery object's metadata does not contain GPS data or the
		/// visibility of the GPS map link is turned off, no action is taken.
		/// </summary>
		/// <param name="galleryObject">The gallery object.</param>
		/// <param name="metadataDisplaySettings">The metadata display settings.</param>
		/// <remarks>The metadata item is added with <see cref="IGalleryObjectMetadataItem.HasChanges" /> = <c>false</c> to prevent it 
		/// from getting persisted to the database. This allows the hyperlink to be regenerated from the template, thus incorporating the
		/// most recent template and other media object properties (such as title). Because the item is linked to the media object, it is
		/// automatically included in the cache of media objects.
		/// This function is identical to <see cref="AddGpsLocationWithMapLink" /> except it uses the primary (not destination) GPS settings.</remarks>
		private static void AddGpsDestLocationWithMapLink(IGalleryObject galleryObject, IMetadataDefinitionCollection metadataDisplaySettings)
		{
			if (!metadataDisplaySettings.Find(FormattedMetadataItemName.GpsDestLocationWithMapLink).IsVisible)
				return; // The map link is disabled, so there is nothing to do.

			IGalleryObjectMetadataItemCollection metadata = galleryObject.MetadataItems;
			IGalleryObjectMetadataItem gpsLocation;

			if (metadata.TryGetMetadataItem(FormattedMetadataItemName.GpsDestLocation, out gpsLocation) && (!metadata.Contains(FormattedMetadataItemName.GpsDestLocationWithMapLink)))
			{
				// We have a GPS location but have not yet created the URL'd version. Do so now and add it to the collection.
				IGalleryObjectMetadataItem latitude;
				IGalleryObjectMetadataItem longitude;
				bool foundLatitude = metadata.TryGetMetadataItem(FormattedMetadataItemName.GpsDestLatitude, out latitude);
				bool foundLongitude = metadata.TryGetMetadataItem(FormattedMetadataItemName.GpsDestLongitude, out longitude);

				if (foundLatitude && foundLongitude)
				{
					string url = GetGpsMapUrl(galleryObject, latitude.Value, longitude.Value, gpsLocation.Value);

					if (!String.IsNullOrEmpty(url))
					{
						// Add to meta collection. Specify false for HasChanges to prevent it from getting persisted back to the database.
						galleryObject.MetadataItems.AddNew(int.MinValue, FormattedMetadataItemName.GpsDestLocationWithMapLink, Resources.GalleryServerPro.Metadata_GpsDestLocationWithMapLink, url, false);
					}
				}
			}
		}

		private static string GetGpsMapUrl(IGalleryObject galleryObject, string latitude, string longitude, string gpsLocation)
		{
			//string urlTemplate = "<a href='http://bing.com/maps/default.aspx?sp=point.{GpsLatitude}_{GpsLongitude}_{TitleNoHtml}__{MediaObjectPageUrl}_{MediaObjectUrl}&style=a&lvl=13' target='_blank' title='View map'>{GpsLocation}</a>";
			//string urlTemplate = "<a href='http://maps.google.com/maps?q={GpsLatitude},{GpsLongitude}+({TitleNoHtml})' target='_blank' title='View map'>{GpsLocation}</a>";
			string urlTemplate = Factory.LoadGallerySetting(galleryObject.GalleryId).GpsMapUrlTemplate;

			if (String.IsNullOrEmpty(urlTemplate))
				return String.Empty;

			// Replace the tokens with actual values.
			urlTemplate = urlTemplate.Replace("{GpsLatitude}", latitude);
			urlTemplate = urlTemplate.Replace("{GpsLongitude}", longitude);
			urlTemplate = urlTemplate.Replace("{TitleNoHtml}", Util.UrlEncode(Util.RemoveHtmlTags(galleryObject.Title, true)));
			urlTemplate = urlTemplate.Replace("{MediaObjectPageUrl}", Util.UrlEncode(String.Concat(Util.GetHostUrl(), Util.GetUrl(PageId.mediaobject, "moid={0}", galleryObject.Id))));
			urlTemplate = urlTemplate.Replace("{MediaObjectUrl}", Util.UrlEncode(String.Concat(Util.GetHostUrl(), MediaObjectHtmlBuilder.GenerateUrl(galleryObject.GalleryId, galleryObject.Id, DisplayObjectType.Thumbnail))));
			urlTemplate = urlTemplate.Replace("{GpsLocation}", gpsLocation);

			return urlTemplate;
		}

		/// <summary>
		/// Handles the <see cref="Business.Gallery.GalleryCreated" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="GalleryServerPro.Business.GalleryCreatedEventArgs"/> instance containing the event data.</param>
		private static void GalleryCreated(object sender, GalleryCreatedEventArgs e)
		{
		}

		/// <summary>
		/// Handles the <see cref="Business.GallerySettings.GallerySettingsSaved" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private static void GallerySettingsSaved(object sender, GallerySettings.GallerySettingsEventArgs e)
		{
			// Finish populating those properties that weren't populated in the business layer.
			AddMembershipDataToGallerySettings();
		}

		/// <summary>
		/// Handles the <see cref="GalleryObject.MetadataLoaded" /> event of the <see cref="GalleryObject" /> class. Specifically, it
		/// adds run-time metadata items such as GPS location map URLs. It also sorts the metadata items.
		/// </summary>
		/// <param name="sender">The source of the event. Should always be an instance of <see cref="IGalleryObject" />.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private static void GalleryObjectMetadataLoaded(object sender, EventArgs e)
		{
			IGalleryObject galleryObject = sender as IGalleryObject;

			if (galleryObject == null) return;

			IMetadataDefinitionCollection metadataDisplayOptions = Factory.LoadGallerySetting(galleryObject.GalleryId).MetadataDisplaySettings;

			AddGpsLocationWithMapLink(galleryObject, metadataDisplayOptions);

			AddGpsDestLocationWithMapLink(galleryObject, metadataDisplayOptions);

			galleryObject.MetadataItems.ApplyDisplayOptions(metadataDisplayOptions);
		}

		#endregion
	}
}
