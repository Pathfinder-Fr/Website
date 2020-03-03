// -----------------------------------------------------------------------
// <copyright file="SueetieCache.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;
    using System.Web;
    using System.Web.Caching;

    public class SueetieCache
    {
        /// <summary>
        /// Default instance
        /// </summary>
        private static SueetieCache _currentInstance;

        /// <summary>
        /// Default static constructor
        /// </summary>
        static SueetieCache()
        {
            // create new instance as a default cache instance
            _currentInstance = new SueetieCache();
        }

        /// <summary>
        /// Gets current instance of SueetieCache class
        /// </summary>
        public static SueetieCache Current
        {
            get { return _currentInstance; }
        }

        // cache object to work with
        private Cache _cache;


        /// <summary>
        /// Default constuctor uses HttpContext.Current as source for obtaining Cache object
        /// </summary>
        public SueetieCache()
            : this(HttpContext.Current.Cache)
        {
        }

        /// <summary>
        /// Initializes class with specified Cache object
        /// </summary>
        /// <param name="cache">Cache to work with</param>
        public SueetieCache(Cache cache)
        {
            this._cache = cache;
        }

        /// <summary>
        /// Indexer for obtaining and setting cache keys
        /// </summary>
        /// <param name="key">Cache key to get or set</param>
        /// <returns>Value cached under specified key</returns>
        public object this[string key]
        {
            get { return this._cache[key]; }
            set { this._cache[key] = value; }
        }

        /// <summary>
        /// Adds item to the cache.
        /// </summary>
        /// <param name="key">Key identifying item in cache.</param>
        /// <param name="value">Cached value.</param>
        /// <param name="dependencies">Cache dependencies, invalidating cache.</param>
        /// <param name="absoluteExpiration">
        /// Absolute expiration date. When used, sliding expiration has to be set to
        /// Cache.NoSlidingExpiration.
        /// </param>
        /// <param name="slidingExpiration">
        /// Sliding expiration of cache item. When used, absolute expiration has to be set to
        /// Cache.NoAbsoluteExpiration.
        /// </param>
        /// <param name="priority">
        /// Relative cost of object in cache. When system evicts objects from cache, objects with lower cost
        /// are removed first.
        /// </param>
        /// <param name="onRemoveCallback">Delegate that is called upon cache item remova. Can be null.</param>
        /// <returns>Cached item.</returns>
        public object Add(string key, object value, CacheDependency dependencies,
            DateTime absoluteExpiration, TimeSpan slidingExpiration,
            CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
        {
            return this._cache.Add(
                key,
                value,
                dependencies,
                absoluteExpiration,
                slidingExpiration,
                priority,
                onRemoveCallback
                );
        }

        public void InsertMax(string key, object value, CacheDependency dependencies)
        {
            this._cache.Insert(
                key,
                value,
                dependencies,
                DateTime.MaxValue,
                Cache.NoSlidingExpiration,
                CacheItemPriority.AboveNormal,
                null
                );
        }

        public void InsertMax(string key, object value)
        {
            this._cache.Insert(
                key,
                value,
                null,
                DateTime.MaxValue,
                Cache.NoSlidingExpiration,
                CacheItemPriority.AboveNormal,
                null
                );
        }

        public void InsertMinutes(string key, object value, CacheDependency dependencies)
        {
            this.InsertMinutes(key, value, dependencies, 30);
        }

        public void InsertMinutes(string key, object value, CacheDependency dependencies, int minutes)
        {
            this._cache.Insert(
                key,
                value,
                dependencies,
                DateTime.Now.AddMinutes(30),
                Cache.NoSlidingExpiration,
                CacheItemPriority.Low,
                null
                );
        }

        public void InsertMinutes(string key, object value)
        {
            this.InsertMinutes(key, value, 30);
        }

        public void InsertMinutes(string key, object value, int minutes)
        {
            this._cache.Insert(
                key,
                value,
                null,
                DateTime.Now.AddMinutes(minutes),
                Cache.NoSlidingExpiration,
                CacheItemPriority.Low,
                null
                );
        }

        /// <summary>
        /// Inserts item to the cache.
        /// </summary>
        /// <param name="key">Key identifying item in cache.</param>
        /// <param name="value">Cached value.</param>
        public void Insert(string key, object value)
        {
            this._cache.Insert(key, value);
        }

        /// <summary>
        /// Inserts item to the cache.
        /// </summary>
        /// <param name="key">Key identifying item in cache.</param>
        /// <param name="value">Cached value.</param>
        /// <param name="dependencies">Cache dependencies, invalidating cache.</param>
        public void Insert(string key, object value, CacheDependency dependencies)
        {
            this._cache.Insert(key, value, dependencies);
        }

        /// <summary>
        /// Inserts item to the cache.
        /// </summary>
        /// <param name="key">Key identifying item in cache.</param>
        /// <param name="value">Cached value.</param>
        /// <param name="dependencies">Cache dependencies, invalidating cache.</param>
        /// <param name="absoluteExpiration">
        /// Absolute expiration date. When used, sliding expiration has to be set to
        /// Cache.NoSlidingExpiration.
        /// </param>
        /// <param name="slidingExpiration">
        /// Sliding expiration of cache item. When used, absolute expiration has to be set to
        /// Cache.NoAbsoluteExpiration.
        /// </param>
        public void Insert(string key, object value, CacheDependency dependencies,
            DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            this._cache.Insert(key, value, dependencies, absoluteExpiration, slidingExpiration);
        }

        /// <summary>
        /// Inserts item to the cache.
        /// </summary>
        /// <param name="key">Key identifying item in cache.</param>
        /// <param name="value">Cached value.</param>
        /// <param name="dependencies">Cache dependencies, invalidating cache.</param>
        /// <param name="absoluteExpiration">
        /// Absolute expiration date. When used, sliding expiration has to be set to
        /// Cache.NoSlidingExpiration.
        /// </param>
        /// <param name="slidingExpiration">
        /// Sliding expiration of cache item. When used, absolute expiration has to be set to
        /// Cache.NoAbsoluteExpiration.
        /// </param>
        /// <param name="priority">
        /// Relative cost of object in cache. When system evicts objects from cache, objects with lower cost
        /// are removed first.
        /// </param>
        /// <param name="onRemoveCallback">Delegate that is called upon cache item remova. Can be null.</param>
        /// <returns>Cached item.</returns>
        public void Insert(string key, object value, CacheDependency dependencies,
            DateTime absoluteExpiration, TimeSpan slidingExpiration,
            CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
        {
            this._cache.Insert(
                key,
                value,
                dependencies,
                absoluteExpiration,
                slidingExpiration,
                priority,
                onRemoveCallback
                );
        }


        /// <summary>
        /// Removes specified key from cache
        /// </summary>
        /// <param name="key">Key to remove</param>
        /// <returns>Value removed from cache, null if no such key was cached</returns>
        public object Remove(string key)
        {
            return this._cache.Remove(key);
        }

        /// <summary>
        /// Removes all keys for which given predicate returns true.
        /// </summary>
        /// <param name="predicate">Predicate for matching cache keys.</param>
        public void Remove(Predicate<string> predicate)
        {
            // get enumarator
            var key = this._cache.GetEnumerator();

            // cycle through cache keys
            while (key.MoveNext())
            {
                // remove cache item if predicate returns true
                if (predicate(key.Key.ToString())) this._cache.Remove(key.Key.ToString());
            }
        }

        /// <summary>
        /// Clear all cache entries from memory.
        /// </summary>
        public void Clear()
        {
            // get enumarator
            var key = this._cache.GetEnumerator();

            // cycle through cache keys
            while (key.MoveNext())
            {
                // and remove them one by one
                this._cache.Remove(key.Key.ToString());
            }
        }
    }
}