// -----------------------------------------------------------------------
// <copyright file="SueetieQStringHelper.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;
    using System.Collections.Generic;
    using System.Web;

    public class SueetieQStringHelper
    {
        private Dictionary<string, long> _idDictionary;

        public Dictionary<string, long> Params
        {
            get
            {
                if (this._idDictionary == null)
                {
                    this._idDictionary = new Dictionary<string, long>();
                }

                return this._idDictionary;
            }
        }

        public long this[string idName]
        {
            get
            {
                if (this.Params.ContainsKey(idName))
                {
                    return this.Params[idName];
                }

                return -1;
            }
        }

        public bool ContainsKey(string idName)
        {
            return this.Params.ContainsKey(idName);
        }

        /// <summary>
        /// False to ErrorOnInvalid
        /// </summary>
        /// <param name="idName"></param>
        public SueetieQStringHelper(string idName)
            : this(idName, false)
        {
        }

        /// <summary>
        /// False on ErrorOnInvalid
        /// </summary>
        /// <param name="idNames"></param>
        public SueetieQStringHelper(string[] idNames)
            : this(idNames, false)
        {
        }

        public SueetieQStringHelper(string idName, bool errorOnInvalid)
        {
            this.InitIDs(new[] { idName }, new[] { errorOnInvalid });
        }

        public SueetieQStringHelper(string[] idNames, bool errorOnInvalid)
        {
            var failInvalid = new bool[idNames.Length];

            for (var i = 0; i < failInvalid.Length; i++)
            {
                failInvalid[i] = errorOnInvalid;
            }

            this.InitIDs(idNames, failInvalid);
        }

        public SueetieQStringHelper(string[] idNames, bool[] errorOnInvalid)
        {
            this.InitIDs(idNames, errorOnInvalid);
        }

        private void InitIDs(string[] idNames, bool[] errorOnInvalid)
        {
            if (idNames.Length != errorOnInvalid.Length)
            {
                throw new Exception("idNames and errorOnInvalid variables must be the same array length.");
            }

            for (var i = 0; i < idNames.Length; i++)
            {
                if (!this.Params.ContainsKey(idNames[i]))
                {
                    long idConverted = -1;

                    if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString[idNames[i]]) && long.TryParse(HttpContext.Current.Request.QueryString[idNames[i]], out idConverted))
                    {
                        this.Params.Add(idNames[i], idConverted);
                    }
                }
            }
        }
    }
}