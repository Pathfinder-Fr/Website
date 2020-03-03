﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalleryServerPro.Business.Interfaces;

namespace GalleryServerPro.Business
{
	/// <summary>
	/// A collection of <see cref="IGalleryControlSettings" /> objects. There is a maximum of one item for each instance of a Gallery
	/// control that is used in an application. An item will exist in this collection only if at least one control-specific setting
	/// has been saved for a particular control.
	/// </summary>
	public class GalleryControlSettingsCollection : Collection<IGalleryControlSettings>, IGalleryControlSettingsCollection
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GallerySettingsCollection"/> class.
		/// </summary>
		public GalleryControlSettingsCollection()
			: base(new List<IGalleryControlSettings>())
		{
		}

		/// <summary>
		/// Adds the gallery control settings to the current collection.
		/// </summary>
		/// <param name="galleryControlSettings">The gallery control settings to add to the current collection.</param>
		public void AddRange(IEnumerable<IGalleryControlSettings> galleryControlSettings)
		{
			foreach (IGalleryControlSettings galleryControlSetting in galleryControlSettings)
			{
				this.Add(galleryControlSetting);
			}
		}

		/// <summary>
		/// Determines whether the <paramref name="galleryControlSettings"/> is already a member of the collection. An object is considered a member
		/// of the collection if they both have the same <see cref="IGalleryControlSettings.ControlId" />.
		/// </summary>
		/// <param name="galleryControlSettings">An <see cref="IGalleryControlSettings"/> to determine whether it is a member of the current collection.</param>
		/// <returns>Returns <c>true</c> if <paramref name="galleryControlSettings"/> is a member of the current collection;
		/// otherwise returns <c>false</c>.</returns>
		public new bool Contains(IGalleryControlSettings galleryControlSettings)
		{
			foreach (IGalleryControlSettings galleryControlSettingsInCollection in (List<IGalleryControlSettings>)Items)
			{
				if (galleryControlSettingsInCollection.ControlId == galleryControlSettings.ControlId)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Adds the specified gallery control settings.
		/// </summary>
		/// <param name="galleryControlSettings">The gallery control settings to add.</param>
		public new void Add(IGalleryControlSettings galleryControlSettings)
		{
			if (galleryControlSettings == null)
				throw new ArgumentNullException("galleryControlSettings", "Cannot add null to an existing GalleryControlSettingsCollection. Items.Count = " + Items.Count);

			base.Add(galleryControlSettings);
		}

		/// <summary>
		/// Find the gallery control settings in the collection that matches the specified <paramref name="controlId" />. If no matching object is found,
		/// null is returned.
		/// </summary>
		/// <param name="controlId">The ID that uniquely identifies the control containing the gallery.</param>
		/// <returns>Returns an <see cref="IGalleryControlSettings" />object from the collection that matches the specified <paramref name="controlId" />,
		/// or null if no matching object is found.</returns>
		public IGalleryControlSettings FindByControlId(string controlId)
		{
			List<IGalleryControlSettings> galleryControlSettings = (List<IGalleryControlSettings>)Items;

			return galleryControlSettings.Find(delegate(IGalleryControlSettings gallery)
			{
				return (gallery.ControlId.Equals(controlId, StringComparison.OrdinalIgnoreCase));
			});
		}
	}
}
