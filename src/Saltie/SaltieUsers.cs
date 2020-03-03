#region Using

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Web.Configuration;
using System.Web;
using System.IO;
using System.Web.UI;
using Saltie.Core;
using Sueetie.Core;

#endregion

namespace Saltie.Core
{
    /// <summary>
    /// The proxy class for communication between the business objects and the providers.
    /// </summary>
    /// 
    public static class SaltieUsers
    {

        #region Saltie Testing

        public static int GetContentCount(int userid)
        {
            SaltieDataProvider _provider = SaltieDataProvider.LoadProvider();
            return _provider.GetContentCount(userid);
        }

        #endregion

    }
}