// -----------------------------------------------------------------------
// <copyright file="ClassifiedsPhoto.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a z_sueetie_cfds_Photo.
    /// </summary>
    [Serializable]
    public class ClassifiedsPhoto
    {
        public int PhotoId { get; set; }
        public int AdId { get; set; }
        public bool IsMainPreview { get; set; }
        public DateTime DateCreated { get; set; }
    }
}