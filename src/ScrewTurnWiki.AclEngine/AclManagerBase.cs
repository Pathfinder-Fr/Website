using System;
using System.Collections.Generic;
using System.Linq;

namespace ScrewTurn.Wiki.AclEngine
{
    /// <summary>
    ///     Implements a base class for an ACL Manager.
    /// </summary>
    /// <remarks>All instance and static members are <b>thread-safe</b>.</remarks>
    public abstract class AclManagerBase : IAclManager
    {
        private List<AclEntry> entries;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:AclManagerBase" /> abstract class.
        /// </summary>
        public AclManagerBase()
        {
            entries = new List<AclEntry>(100);
        }

        /// <summary>
        ///     Stores a new ACL entry.
        /// </summary>
        /// <param name="resource">The controlled resource.</param>
        /// <param name="action">The action on the controlled resource.</param>
        /// <param name="subject">The subject whose access to the resource/action is controlled.</param>
        /// <param name="value">The value of the entry.</param>
        /// <returns><c>true</c> if the entry is stored, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">
        ///     If <paramref name="resource" />, <paramref name="action" /> or
        ///     <paramref name="subject" /> are <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     If <paramref name="resource" />, <paramref name="action" /> or
        ///     <paramref name="subject" /> are empty.
        /// </exception>
        public bool StoreEntry(string resource, string action, string subject, Value value)
        {
            if (resource == null) throw new ArgumentNullException("resource");
            if (resource.Length == 0) throw new ArgumentException("Resource cannot be empty", "resource");
            if (action == null) throw new ArgumentNullException("action");
            if (action.Length == 0) throw new ArgumentException("Action cannot be empty", "action");
            if (subject == null) throw new ArgumentNullException("subject");
            if (subject.Length == 0) throw new ArgumentException("Subject cannot be empty", "subject");

            var result = new AclEntry(resource, action, subject, value);

            lock (this)
            {
                var index = entries.FindIndex(delegate(AclEntry x) { return AclEntry.Equals(x, result); });
                if (index >= 0)
                {
                    var removed = entries[index];
                    entries.RemoveAt(index);
                    OnAclChanged(new[] {removed}, Change.EntryDeleted);
                }
                entries.Add(result);
                OnAclChanged(new[] {result}, Change.EntryStored);
            }

            return true;
        }

        /// <summary>
        ///     Deletes an ACL entry.
        /// </summary>
        /// <param name="resource">The controlled resource.</param>
        /// <param name="action">The action on the controlled resource.</param>
        /// <param name="subject">The subject whose access to the resource/action is controlled.</param>
        /// <returns><c>true</c> if the entry is deleted, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">
        ///     If <paramref name="resource" />, <paramref name="action" /> or
        ///     <paramref name="subject" /> are <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     If <paramref name="resource" />, <paramref name="action" /> or
        ///     <paramref name="subject" /> are empty.
        /// </exception>
        public bool DeleteEntry(string resource, string action, string subject)
        {
            if (resource == null) throw new ArgumentNullException("resource");
            if (resource.Length == 0) throw new ArgumentException("Resource cannot be empty", "resource");
            if (action == null) throw new ArgumentNullException("action");
            if (action.Length == 0) throw new ArgumentException("Action cannot be empty", "action");
            if (subject == null) throw new ArgumentNullException("subject");
            if (subject.Length == 0) throw new ArgumentException("Subject cannot be empty", "subject");

            var result = new AclEntry(resource, action, subject, Value.Deny);

            lock (this)
            {
                var index = entries.FindIndex(delegate(AclEntry x) { return AclEntry.Equals(x, result); });
                if (index >= 0)
                {
                    var entry = entries[index];
                    entries.RemoveAt(index);
                    OnAclChanged(new[] {entry}, Change.EntryDeleted);
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        ///     Deletes all the ACL entries for a resource.
        /// </summary>
        /// <param name="resource">The controlled resource.</param>
        /// <returns><c>true</c> if the entries are deleted, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="resource" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="resource" /> is empty.</exception>
        public bool DeleteEntriesForResource(string resource)
        {
            if (resource == null) throw new ArgumentNullException("resource");
            if (resource.Length == 0) throw new ArgumentException("Resource cannot be empty", "resource");

            lock (this)
            {
                var indexesToRemove = new List<int>(30);
                var entriesToRemove = new List<AclEntry>(30);
                for (var i = 0; i < entries.Count; i++)
                {
                    if (entries[i].Resource == resource)
                    {
                        indexesToRemove.Add(i);
                        entriesToRemove.Add(entries[i]);
                    }
                }

                if (indexesToRemove.Count > 0)
                {
                    // Work in opposite direction to preserve smaller indexes
                    for (var i = indexesToRemove.Count - 1; i >= 0; i--)
                    {
                        entries.RemoveAt(indexesToRemove[i]);
                    }

                    OnAclChanged(entriesToRemove.ToArray(), Change.EntryDeleted);

                    return true;
                }
                return false;
            }
        }

        /// <summary>
        ///     Deletes all the ACL entries for a subject.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <returns><c>true</c> if the entries are deleted, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="subject" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="subject" /> is empty.</exception>
        public bool DeleteEntriesForSubject(string subject)
        {
            if (subject == null) throw new ArgumentNullException("subject");
            if (subject.Length == 0) throw new ArgumentException("Subject cannot be empty", "subject");

            lock (this)
            {
                var indexesToRemove = new List<int>(30);
                var entriesToRemove = new List<AclEntry>(30);
                for (var i = 0; i < entries.Count; i++)
                {
                    if (entries[i].Subject == subject)
                    {
                        indexesToRemove.Add(i);
                        entriesToRemove.Add(entries[i]);
                    }
                }

                if (indexesToRemove.Count > 0)
                {
                    // Work in opposite direction to preserve smaller indexes
                    for (var i = indexesToRemove.Count - 1; i >= 0; i--)
                    {
                        entries.RemoveAt(indexesToRemove[i]);
                    }

                    OnAclChanged(entriesToRemove.ToArray(), Change.EntryDeleted);

                    return true;
                }
                return false;
            }
        }

        /// <summary>
        ///     Renames a resource.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="newName">The new name of the resource.</param>
        /// <returns><c>true</c> if the resource is renamed, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="resource" /> or <paramref name="newName" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="resource" /> or <paramref name="newName" /> are empty.</exception>
        public bool RenameResource(string resource, string newName)
        {
            if (resource == null) throw new ArgumentNullException("resource");
            if (resource.Length == 0) throw new ArgumentException("Resource cannot be empty", "resource");

            if (newName == null) throw new ArgumentNullException("newName");
            if (newName.Length == 0) throw new ArgumentException("New Name cannot be empty", "newName");

            lock (this)
            {
                var entries = RetrieveEntriesForResource(resource);
                var renamed = false;

                foreach (var entry in entries)
                {
                    var deleted = DeleteEntry(entry.Resource, entry.Action, entry.Subject);
                    if (deleted)
                    {
                        var stored = StoreEntry(newName, entry.Action, entry.Subject, entry.Value);
                        if (stored) renamed = true;
                        else return false;
                    }
                    else return false;
                }

                return renamed;
            }
        }

        /// <summary>
        ///     Retrieves all the ACL entries for a resource.
        /// </summary>
        /// <returns>The entries.</returns>
        public IEnumerable<AclEntry> RetrieveAllEntries()
        {
            lock (this)
            {
                return entries.ToList();
            }
        }

        /// <summary>
        ///     Retrieves all the ACL entries for a resource.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <returns>The entries.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="resource" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="resource" /> is empty.</exception>
        public IEnumerable<AclEntry> RetrieveEntriesForResource(string resource)
        {
            if (resource == null) throw new ArgumentNullException("resource");
            if (resource.Length == 0) throw new ArgumentException("Resource cannot be empty", "resource");

            lock (this)
            {
                var result = new List<AclEntry>(10);

                foreach (var e in entries)
                {
                    if (e.Resource == resource) result.Add(e);
                }

                return result.ToArray();
            }
        }

        /// <summary>
        ///     Retrieves all the ACL entries for a subject.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <returns>The entries.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="subject" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="subject" /> is empty.</exception>
        public IEnumerable<AclEntry> RetrieveEntriesForSubject(string subject)
        {
            if (subject == null) throw new ArgumentNullException("subject");
            if (subject.Length == 0) throw new ArgumentException("Subject cannot be empty", "subject");

            lock (this)
            {
                var result = new List<AclEntry>(10);

                foreach (var e in entries)
                {
                    if (e.Subject == subject) result.Add(e);
                }

                return result.ToArray();
            }
        }

        /// <summary>
        ///     Initializes the manager data.
        /// </summary>
        /// <param name="entries">The ACL entries.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="entries" /> is <c>null</c>.</exception>
        public void InitializeData(IEnumerable<AclEntry> entries)
        {
            if (entries == null) throw new ArgumentNullException("entries");

            lock (this)
            {
                this.entries = new List<AclEntry>(entries);
            }
        }

        /// <summary>
        ///     Gets the total number of ACL entries.
        /// </summary>
        public int TotalEntries
        {
            get
            {
                lock (this)
                {
                    return entries.Count;
                }
            }
        }

        /// <summary>
        ///     Event fired when an ACL entry is stored or deleted.
        /// </summary>
        public event EventHandler<AclChangedEventArgs> AclChanged;

        /// <summary>
        ///     Handles the invokation of <see cref="IAclManager.AclChanged" /> event.
        /// </summary>
        /// <param name="entries">The changed entries.</param>
        /// <param name="change">The change.</param>
        protected void OnAclChanged(AclEntry[] entries, Change change)
        {
            if (AclChanged != null)
            {
                AclChanged(this, new AclChangedEventArgs(entries, change));
            }
        }
    }
}