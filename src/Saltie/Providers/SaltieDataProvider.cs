#region Using

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Web.Configuration;
using System.Web;
using System.IO;
using Saltie.Core;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Web.Caching;
using System.Data.SqlClient;
using System.Data;
using Sueetie.Core;

#endregion

namespace Saltie.Core
{

    public abstract partial class SaltieDataProvider : ProviderBase
    {
        #region Provider model

        private static string providerKey = "SaltieSqlDataProvider";
        private static SaltieDataProvider _provider;
        private static object _lock = new object();

        public static SaltieDataProvider Provider
        {
            get { LoadProvider(); return _provider; }
        }

        public static SaltieDataProvider LoadProvider()
        {
            _provider = SueetieCache.Current[providerKey] as SaltieDataProvider;
            if (_provider == null)
            {
                lock (_lock)
                {
                    if (_provider == null)
                    {

                        SueetieConfiguration sueetieConfig = SueetieConfiguration.Get();
                        List<SueetieProvider> sueetieProviders = sueetieConfig.SueetieProviders;

                        SueetieProvider _p = sueetieProviders.Find(delegate(SueetieProvider sp) { return sp.Name == "SaltieSqlDataProvider"; });
                        _provider = Activator.CreateInstance(Type.GetType(_p.ProviderType), new object[] { _p.ConnectionString }) as SaltieDataProvider;
                        SueetieCache.Current.InsertMax(providerKey, _provider, new CacheDependency(sueetieConfig.ConfigPath));

                    }
                }
            }
            return _provider;
        }

        #endregion

        #region Saltie Testing

        public abstract int GetContentCount(int userid);

        #endregion

        #region Populate

        #endregion

    }

}
