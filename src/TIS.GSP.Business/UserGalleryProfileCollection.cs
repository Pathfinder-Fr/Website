using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalleryServerPro.Business.Interfaces;

namespace GalleryServerPro.Business
{
	/// <summary>
	/// A collection of <see cref="IUserGalleryProfile" /> objects.
	/// </summary>
	[Serializable]
	public class UserGalleryProfileCollection : Collection<IUserGalleryProfile>, IUserGalleryProfileCollection
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="UserGalleryProfileCollection"/> class.
		/// </summary>
		public UserGalleryProfileCollection()
			: base(new List<IUserGalleryProfile>())
		{
		}

		/// <summary>
		/// Determines whether the <paramref name="profile"/> is already a member of the collection. An object is considered a member
		/// of the collection if they both have the same <see cref="IUserGalleryProfile.GalleryId"/>.
		/// </summary>
		/// <param name="profile">An <see cref="IUserGalleryProfile"/> to determine whether it is a member of the current collection.</param>
		/// <returns>
		/// Returns <c>true</c> if <paramref name="profile"/> is a member of the current collection;
		/// otherwise returns <c>false</c>.
		/// </returns>
		public new bool Contains(IUserGalleryProfile profile)
		{
			foreach (IUserGalleryProfile userAccountInCollection in (List<IUserGalleryProfile>)Items)
			{
				if (userAccountInCollection.GalleryId == profile.GalleryId)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Adds the specified user profile.
		/// </summary>
		/// <param name="profile">The user profile to add.</param>
		public new void Add(IUserGalleryProfile profile)
		{
			if (profile == null)
				throw new ArgumentNullException("profile", "Cannot add null to an existing UserGalleryProfileCollection. Items.Count = " + Items.Count);

			base.Add(profile);
		}

		/// <summary>
		/// Adds the gallery profiles to the current collection.
		/// </summary>
		/// <param name="galleryProfiles">The gallery profiles to add to the current collection.</param>
		public void AddRange(IEnumerable<IUserGalleryProfile> galleryProfiles)
		{
			foreach (IUserGalleryProfile galleryProfile in galleryProfiles)
			{
				this.Add(galleryProfile);
			}
		}

		/// <summary>
		/// Find the user account in the collection that matches the specified <paramref name="galleryId" />. If no matching object is found,
		/// null is returned.
		/// </summary>
		/// <param name="galleryId">The ID of the gallery.</param>
		/// <returns>Returns an <see cref="IUserGalleryProfile" />object from the collection that matches the specified <paramref name="galleryId" />,
		/// or null if no matching object is found.</returns>
		public IUserGalleryProfile FindByGalleryId(int galleryId)
		{
			List<IUserGalleryProfile> userAccounts = (List<IUserGalleryProfile>)Items;

			return userAccounts.Find(delegate(IUserGalleryProfile gallery)
			{
				return (gallery.GalleryId == galleryId);
			});
		}

		/// <summary>
		/// Creates a new instance of an <see cref="IUserGalleryProfile"/> object. This method can be used by code that only has a
		/// reference to the interface layer and therefore cannot create a new instance of an object on its own.
		/// </summary>
		/// <param name="galleryId">The ID of the gallery.</param>
		/// <returns>
		/// Returns a new instance of an <see cref="IUserGalleryProfile"/> object.
		/// </returns>
		public IUserGalleryProfile CreateNewUserGalleryProfile(int galleryId)
		{
			return new UserGalleryProfile(galleryId);
		}

		/// <summary>
		/// Creates a new collection containing deep copies of the items it contains.
		/// </summary>
		/// <returns>Returns a new collection containing deep copies of the items it contains.</returns>
		public IUserGalleryProfileCollection Copy()
		{
			IUserGalleryProfileCollection copy = new UserGalleryProfileCollection();

			foreach (IUserGalleryProfile galleryProfile in (List<IUserGalleryProfile>)Items)
			{
				copy.Add(galleryProfile.Copy());
			}

			return copy;
		}
	}
}
