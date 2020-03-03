using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Sueetie.Wiki {

	/// <summary>
	/// Describes a User.
	/// </summary>
	[Serializable]
	public class UserInfo {

		private string username;
		private string email;
		private bool active;
		private DateTime dateTime;
		private bool admin;

		/// <summary>
		/// Initializes a new instance of the <b>UserInfo</b> class.
		/// </summary>
		/// <param name="username">The Username.</param>
		/// <param name="email">The Email.</param>
		/// <param name="active">Specifies whether the Account is active or not.</param>
		/// <param name="dateTime">The creation DateTime.</param>
		/// <param name="admin">Specifies whether the User is an Admin or not.</param>
		/// <param name="provider">The Users Storage Provider that manages the User.</param>
		public UserInfo(string username, string email) {
			this.username = username;
			this.email = email;
		}

		/// <summary>
		/// Gets the Username.
		/// </summary>
		public string Username {
			get { return username; }
		}

		/// <summary>
		/// Gets or sets the Email.
		/// </summary>
		public string Email {
			get { return email; }
			set { email = value; }
		}

		/// <summary>
		/// Gets or sets a value specifying whether the Account is active or not.
		/// </summary>
		public bool Active {
			get { return active; }
			set { active = value; }
		}

		/// <summary>
		/// Gets the creation DateTime.
		/// </summary>
		public DateTime DateTime {
			get { return dateTime; }
		}

		/// <summary>
		/// Gets or sets a value a value specifying whether the User is an Admin or not.
		/// </summary>
		public bool Admin {
			get { return admin; }
			set { admin = value; }
		}

		/// <summary>
		/// Converts the current instance to a string.
		/// </summary>
		/// <returns>The string.</returns>
		public override string ToString() {
			return username;
		}

	}

	/// <summary>
	/// Provides a method for comparing two <b>UserInfo</b> objects, comparing their Username.
	/// </summary>
	/// <remarks>The comparison is <b>case unsensitive</b>.</remarks>
	public class UsernameComparer : IComparer<UserInfo> {

		/// <summary>
		/// Compares two UserInfo objects, comparing their Username.
		/// </summary>
		/// <param name="x">The first object.</param>
		/// <param name="y">The second object.</param>
		/// <returns>The comparison result (-1, 0 or 1).</returns>
		public int Compare(UserInfo x, UserInfo y) {
			return StringComparer.OrdinalIgnoreCase.Compare(x.Username, y.Username);
		}

	}

}
