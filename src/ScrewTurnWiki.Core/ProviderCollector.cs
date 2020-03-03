using System.Collections.Generic;
using System.Linq;

namespace ScrewTurn.Wiki
{

    /// <summary>
    /// Implements a generic Provider Collector.
    /// </summary>
    /// <typeparam name="T">The type of the Collector.</typeparam>
    public class ProviderCollector<T>
    {
        private readonly List<T> list;

        private List<T> listCopy;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ProviderCollector()
        {
            list = new List<T>(3);
        }

        /// <summary>
        /// Adds a Provider to the Collector.
        /// </summary>
        /// <param name="provider">The Provider to add.</param>
        public void AddProvider(T provider)
        {
            lock (this)
            {
                list.Add(provider);
                listCopy = null;
            }
        }

        /// <summary>
        /// Removes a Provider from the Collector.
        /// </summary>
        /// <param name="provider">The Provider to remove.</param>
        public void RemoveProvider(T provider)
        {
            lock (this)
            {
                list.Remove(provider);
                listCopy = null;
            }
        }

        /// <summary>
        /// Gets all the Providers (copied array).
        /// </summary>
        public IList<T> AllProviders
        {
            get
            {
                if (listCopy == null)
                {
                    lock (this)
                    {
                        if (listCopy == null)
                        {
                            listCopy = list.ToList();
                        }
                    }
                }

                return listCopy;
            }
        }

        /// <summary>
        /// Gets a Provider, searching for its Type Name.
        /// </summary>
        /// <param name="typeName">The Type Name.</param>
        /// <returns>The Provider, or null if the Provider was not found.</returns>
        public T GetProvider(string typeName)
        {
            lock (this)
            {
                var provider = list.FirstOrDefault(i => i.GetType().FullName == typeName);
                if (provider == null)
                    return default;
                return provider;
            }
        }
    }

}
