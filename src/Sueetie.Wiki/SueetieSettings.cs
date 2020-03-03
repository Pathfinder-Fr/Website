
using System;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;
using System.Globalization;
using System.Reflection;

namespace Sueetie.Wiki
{
    public class SueetieSettings
    {
        public static string RootDirectory
        {
            get { return System.Web.HttpRuntime.AppDomainAppPath; }
        }

        // Full path of wiki public directory:  ex: c:\inetpub\siteroot\wiki\public
        public static string WikiPublicDirectory(string groupName)
        {
            string _wikiPublicDirectory = HttpContext.Current.Server.MapPath("/wiki/public/");
            if (groupName != null)
                _wikiPublicDirectory = HttpContext.Current.Server.MapPath("/groups/" + groupName + "/wiki/public/");
            return _wikiPublicDirectory;
        }

        public static string UsersFile(string groupName)
        {
            return WikiPublicDirectory(groupName) + "Users.cs";
        }

        public static string LogFile(string groupName)
        {
            return WikiPublicDirectory(groupName) + "Log.cs";
        }

    }

}
