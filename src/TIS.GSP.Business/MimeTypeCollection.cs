using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalleryServerPro.Business.Interfaces;

namespace GalleryServerPro.Business
{
	/// <summary>
	/// Represents a set of MIME types.
	/// </summary>
	public class MimeTypeCollection : Collection<IMimeType>, IMimeTypeCollection
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MimeTypeCollection"/> class.
		/// </summary>
		public MimeTypeCollection()
			: base(new List<IMimeType>())
		{
		}

		/// <summary>
		/// Adds the specified MIME type.
		/// </summary>
		/// <param name="mimeType">The MIME type to add.</param>
		public new void Add(IMimeType mimeType)
		{
			if (mimeType == null)
				throw new ArgumentNullException("mimeType", "Cannot add null to an existing MimeTypeCollection. Items.Count = " + Items.Count);

			base.Add(mimeType);
		}

		/// <summary>
		/// Find the MIME type in the collection that matches the specified <paramref name="fileExtension" />. If no matching object is found,
		/// null is returned. It is not case sensitive.
		/// </summary>
		/// <param name="fileExtension">A string representing the file's extension, including the period (e.g. ".jpg", ".avi").
		/// It is not case sensitive.</param>
		/// <returns>Returns an <see cref="IMimeType" />object from the collection that matches the specified <paramref name="fileExtension" />,
		/// or null if no matching object is found.</returns>
		public IMimeType Find(string fileExtension)
		{
			List<IMimeType> mimeTypes = (List<IMimeType>)Items;

			return mimeTypes.Find(delegate(IMimeType gallery)
			{
				return (gallery.Extension.Equals(fileExtension, StringComparison.OrdinalIgnoreCase));
			});
		}
	}
}
