using GalleryServerPro.Business.Interfaces;

namespace GalleryServerPro.Web.Entity
{
	/// <summary>
	/// A simple object that contains synchronization settings.
	/// </summary>
	public class SynchronizeSettingsEntity
	{
		public IAlbum AlbumToSynchronize;
		public bool IsRecursive;
		public bool OverwriteThumbnails;
		public bool OverwriteOptimized;
		public bool RegenerateMetadata;
		public SyncInitiator SyncInitiator;
	}

	/// <summary>
	/// An enumeration that stores values for possible objects that can initiate a synchronization.
	/// </summary>
	public enum SyncInitiator
	{
		/// <summary>
		/// 
		/// </summary>
		Unknown = 0,
		/// <summary>
		/// 
		/// </summary>
		LoggedOnGalleryUser,
		/// <summary>
		/// 
		/// </summary>
		AutoSync,
		/// <summary>
		/// 
		/// </summary>
		RemoteApp
	}
}