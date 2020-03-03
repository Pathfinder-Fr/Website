// -----------------------------------------------------------------------
// <copyright file="SueetieRole.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a Sueetie_Role.
    /// </summary>
    [Serializable]
    public class SueetieRole
    {
        public int SueetieRoleID { get; set; }
        public Guid RoleID { get; set; }
        public string RoleName { get; set; }
        public bool IsGroupAdminRole { get; set; }
        public bool IsGroupUserRole { get; set; }
        public bool IsBlogOwnerRole { get; set; }
    }
}