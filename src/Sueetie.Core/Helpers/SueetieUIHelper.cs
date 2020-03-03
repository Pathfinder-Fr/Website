using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web.UI.WebControls;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Security;

namespace Sueetie.Core
{
    public static class SueetieUIHelper
    {

        #region Time Zones

        public static void PopulateTimeZoneList(DropDownList list)
        {
            PopulateTimeZoneList(list, SiteSettings.Instance.DefaultTimeZone);
        }

        public static void PopulateTimeZoneList(DropDownList list, string selectedTimeZone)
        {
            List<SueetieResource> sueetieResources = SueetieLocalizer.GetTimeZones();
            foreach (SueetieResource sueetieResource in sueetieResources)
            {
                //list.Items.Add(new ListItem(sueetieResource.Value, Convert.ToString(GetHourOffsetFromNode(sueetieResource.Key) * 60)));
                list.Items.Add(new ListItem(sueetieResource.Value, Convert.ToInt32(GetHourOffsetFromNode(sueetieResource.Key) * 60).ToString()));
            }

            if (!list.Page.IsPostBack && list.Items.FindByValue(selectedTimeZone) != null)
                list.SelectedValue = selectedTimeZone;
        }

        public static decimal GetHourOffsetFromNode(string resourceKey)
        {
            // calculate hours -- can use prefix of either UTC or GMT...
            decimal hours = 0;

            try
            {
                hours = decimal.Parse(resourceKey.Replace("UTC", string.Empty).Replace("GMT", string.Empty), CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                hours = Convert.ToDecimal(resourceKey.Replace(".", ",").Replace("UTC", string.Empty).Replace("GMT", string.Empty));
            }

            return hours;
        }

        #endregion

        #region Determine Theme

        private static readonly Regex MOBILE_REGEX = new Regex(SueetieConfiguration.Get().Core.MobileDevices, RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// Gets a value indicating whether the client is a mobile device.
        /// </summary>
        /// <value><c>true</c> if this instance is mobile; otherwise, <c>false</c>.</value>
        public static bool IsMobile
        {
            get
            {
                HttpContext context = HttpContext.Current;
                if (context != null)
                {
                    HttpRequest request = context.Request;
                    if (request.Browser.IsMobileDevice)
                        return true;

                    if (!string.IsNullOrEmpty(request.UserAgent) && MOBILE_REGEX.IsMatch(request.UserAgent))
                        return true;
                }

                return false;
            }
        }


        #endregion

        #region User Authorized for function

        public static bool IsUserAuthorized(string _roles)
        {
            bool authorized = false;
            if (string.IsNullOrEmpty(_roles))
                return true;
            else
            {
                string[] urlRoles = _roles.ToLower().Split(',');
                string[] userRoles = SueetieContext.Current.UserRoles;
                foreach (string urlRole in urlRoles)
                {
                    foreach (string userRole in userRoles)
                    {
                        if (urlRole.Trim().Equals(userRole.Trim().ToLower()))
                        {
                            authorized = true;
                            break;
                        }
                    }
                }
            }
            return authorized;
        }

        #endregion

        #region Formatting

        public static void AddLine(Page _page)
        {
            _page.Header.Controls.Add(new LiteralControl(Environment.NewLine));
        }

        #endregion

        #region Application

        public static AspNetHostingPermissionLevel GetCurrentTrustLevel()
        {
            foreach (AspNetHostingPermissionLevel trustLevel in
              new[]
          {
            AspNetHostingPermissionLevel.Unrestricted, AspNetHostingPermissionLevel.High, 
            AspNetHostingPermissionLevel.Medium, AspNetHostingPermissionLevel.Low, AspNetHostingPermissionLevel.Minimal
          })
            {
                try
                {
                    new AspNetHostingPermission(trustLevel).Demand();
                }
                catch (SecurityException)
                {
                    continue;
                }

                return trustLevel;
            }

            return AspNetHostingPermissionLevel.None;
        }

        #endregion

        #region Reports

        public static string DisplayDateRange(int _daySpan)
        {
            string _displayedDateRange = SueetieLocalizer.GetString("dayspan_na");
            switch (_daySpan)
            {
                case (int)SueetieDaySpan.Day:
                    _displayedDateRange = SueetieLocalizer.GetString("dayspan_day");
                    break;
                case (int)SueetieDaySpan.Week:
                    _displayedDateRange = SueetieLocalizer.GetString("dayspan_week");
                    break;
                case (int)SueetieDaySpan.Month:
                    _displayedDateRange = SueetieLocalizer.GetString("dayspan_month");
                    break;
                case (int)SueetieDaySpan.Quarter:
                    _displayedDateRange = SueetieLocalizer.GetString("dayspan_quarter");
                    break;
                case (int)SueetieDaySpan.Year:
                    _displayedDateRange = SueetieLocalizer.GetString("dayspan_year");
                    break;
                default:
                    break;
            }
            return _displayedDateRange;
        }

        #endregion

        #region Theme Urls

        public static string ThemedImage(string filename)
        {
            return "/themes/" + SueetieContext.Current.Theme + "/images/" + filename;
        }

        #endregion
    }
}
