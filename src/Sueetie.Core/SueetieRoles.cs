// -----------------------------------------------------------------------
// <copyright file="SueetieRoles.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Sueetie.Core
{
    public static class SueetieRoles
    {
        static string ApplicationName = SueetieConfiguration.Get().Core.ApplicationName;

        public static string[] GetRolesForUser(string username)
        {
            if (username == null)
                return Array.Empty<string>();


            username = username.ToLowerInvariant();
            if (!UserRoleCache.TryGetValue(username, out var rolesArray))
            {
                rolesArray = Array.Empty<string>();
                var roleNames = new List<string>();
                var roles = SueetieDataProvider.LoadProvider().GetRoles(ApplicationName, username);
                if (roles != null)
                {
                    foreach (var role in roles)
                    {
                        roleNames.Add(role);
                    }

                    UserRoleCache[username] = rolesArray = roleNames.ToArray();
                }
            }

            return rolesArray;
        }

        static Dictionary<string, string[]> UserRoleCache
        {
            get
            {
                var key = GenerateCacheKey("UserRoleDictionary");
                var userRoleDic = HttpContext.Current.Cache[key] as Dictionary<string, string[]>;
                if (userRoleDic == null)
                {
                    userRoleDic = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);
                    HttpContext.Current.Cache[key] = userRoleDic;
                }

                return userRoleDic;
            }
        }

        private static string GenerateCacheKey(string name)
        {
            return string.Format("SueetieRoleProvider-{0}-{1}", name, ApplicationName);
        }

        public static List<SueetieRole> GetSueetieRoleList()
        {
            var key = SueetieRolesListCacheKey();

            var sueetieRoles = SueetieCache.Current[key] as List<SueetieRole>;
            if (sueetieRoles == null)
            {
                var provider = SueetieDataProvider.LoadProvider();
                sueetieRoles = provider.GetSueetieRoleList();
                SueetieCache.Current.InsertMax(key, sueetieRoles);
            }

            return sueetieRoles;
        }

        public static List<SueetieRole> GetSueetieGroupAdminRoleList()
        {
            var a = from role in GetSueetieRoleList()
                    where role.IsGroupAdminRole
                    select role;
            return a.ToList();
        }

        public static List<SueetieRole> GetSueetieGroupUserRoleList()
        {
            var a = from role in GetSueetieRoleList()
                    where role.IsGroupUserRole
                    select role;
            return a.ToList();
        }

        public static List<SueetieRole> GetSueetieBlogOwnerRoleList()
        {
            var a = from role in GetSueetieRoleList()
                    where role.IsBlogOwnerRole
                    select role;
            return a.ToList();
        }

        public static string SueetieRolesListCacheKey()
        {
            return string.Format("SueetieRolesList-{0}", SueetieConfiguration.Get().Core.SiteUniqueName);
        }

        public static void CreateSueetieRole(SueetieRole sueetieRole)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.CreateSueetieRole(sueetieRole);
        }

        public static Guid GetAspnetRoleID(string rolename)
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.GetAspnetRoleID(rolename);
        }

        public static void DeleteSueetieRole(string rolename)
        {
            var dp = SueetieDataProvider.LoadProvider();
            var wasDeleted = dp.DeleteSueetieRole(rolename);
            if (wasDeleted)
            {
                Roles.DeleteRole(rolename);
                ClearRolesListCache();
            }
        }

        public static void UpdateSueetieRole(bool isGroupAdminRole, bool isGroupUserRole, bool isBlogOwnerRole, string RoleName)
        {
            var provider = SueetieDataProvider.LoadProvider();
            var _role = new SueetieRole
            {
                RoleName = RoleName,
                IsGroupUserRole = isGroupUserRole,
                IsGroupAdminRole = isGroupAdminRole,
                IsBlogOwnerRole = isBlogOwnerRole
            };
            provider.UpdateSueetieRole(_role);
            ClearRolesListCache();
        }

        public static void ClearRolesListCache()
        {
            SueetieCache.Current.Remove(SueetieRolesListCacheKey());
        }
    }
}