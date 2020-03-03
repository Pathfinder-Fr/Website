// -----------------------------------------------------------------------
// <copyright file="SueetieTagQuery.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a z_Sueetie_TagQuery.
    /// </summary>
    [Serializable]
    public class SueetieTagQuery
    {
        private int _applicationTypeID;
        private int _applicationID;
        private int _cloudTagNum = 100;
        private bool _isRestricted = true;

        public int ApplicationTypeID
        {
            get { return this._applicationTypeID; }
            set { this._applicationTypeID = value; }
        }

        public int ApplicationID
        {
            get { return this._applicationID; }
            set { this._applicationID = value; }
        }

        public int CloudTagNum
        {
            get { return this._cloudTagNum; }
            set { this._cloudTagNum = value; }
        }

        public bool IsRestricted
        {
            get { return this._isRestricted; }
            set { this._isRestricted = value; }
        }
    }
}