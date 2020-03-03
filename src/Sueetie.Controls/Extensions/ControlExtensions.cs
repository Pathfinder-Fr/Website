using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sueetie.Core;

namespace Sueetie.Controls
{
    public static class ControlsExtensions
    {
        public static string ControlPath(this string viewName)
        {
            SueetieApplication _currentApplication = SueetieApplications.Current;
            string _currentTheme = SueetieContext.Current.Theme;
            string path = string.Empty;

            switch ((SueetieApplicationType)_currentApplication.ApplicationTypeID)
            {
                case SueetieApplicationType.Unknown:
                case SueetieApplicationType.CMS:
                case SueetieApplicationType.Marketplace:
                    path = "/Themes/" + _currentTheme + "/Views/" + viewName + ".ascx";
                    break;
                case SueetieApplicationType.Blog:
                case SueetieApplicationType.Forum:
                case SueetieApplicationType.Wiki:
                    path = "/" + _currentApplication.ApplicationKey + "/themes/" + _currentTheme + "/views/" + viewName + ".ascx";
                    break;
                case SueetieApplicationType.MediaGallery:
                    path = "/" + _currentApplication.ApplicationKey + "/gs/styles/" + _currentTheme + "/views/" + viewName + ".ascx";
                    break;
                default:
                    break;
            }

            return path;
        }

        public static string ControlHeaderPath(this string headerViewName)
        {
            SueetieApplication _currentApplication = SueetieApplications.Current;
            string _currentTheme = SueetieContext.Current.Theme;
            string path = string.Empty;

            switch ((SueetieApplicationType)_currentApplication.ApplicationTypeID)
            {
                case SueetieApplicationType.Unknown:
                case SueetieApplicationType.CMS:
                case SueetieApplicationType.Marketplace:
                    path = "/Themes/" + _currentTheme + "/Views/" + headerViewName + ".ascx";
                    break;
                case SueetieApplicationType.Blog:
                case SueetieApplicationType.Forum:
                case SueetieApplicationType.Wiki:
                    path = "/" + _currentApplication.ApplicationKey + "/themes/" + _currentTheme + "/views/" + headerViewName + ".ascx";
                    break;
                case SueetieApplicationType.MediaGallery:
                    path = "/" + _currentApplication.ApplicationKey + "/gs/styles/" + _currentTheme + "/views/" + headerViewName + ".ascx";
                    break;
                default:
                    break;
            }

            return path;
        }

    }
}
