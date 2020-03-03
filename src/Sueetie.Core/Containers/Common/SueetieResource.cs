// -----------------------------------------------------------------------
// <copyright file="SueetieResource.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a z_Sueetie_Local.
    /// </summary>
    [Serializable]
    public class SueetieResource
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}