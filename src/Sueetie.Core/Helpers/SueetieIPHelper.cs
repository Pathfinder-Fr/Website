// -----------------------------------------------------------------------
// <copyright file="SueetieIPHelper.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    public static class SueetieIPHelper
    {
        /// <summary>
        /// Converts an array of strings into a ulong representing a 4 byte IP address
        /// </summary>
        /// <param name="ip">string array of numbers</param>
        /// <returns>ulong represending an encoding IP address</returns>
        static public ulong Str2IP(string[] ip)
        {
            if (ip.Length < 2)
            {
                throw new Exception("Invalid ip address." + ip);
            }

            ulong num = 0, tNum;
            for (var i = 0; i < ip.Length; i++)
            {
                num <<= 8;
                if (ulong.TryParse(ip[i], out tNum))
                {
                    num |= tNum;
                }
            }

            return num;
        }

        static public ulong IPStrToLong(string ipAddress)
        {
            // not sure why it gives me this for local users on firefox--but it does...
            if (ipAddress == "::1") ipAddress = "127.0.0.1";

            var ip = ipAddress.Split('.');
            return Str2IP(ip);
        }

        /// <summary>
        /// Verifies that an ip and mask aren't banned
        /// </summary>
        /// <param name="ban">Banned IP</param>
        /// <param name="chk">IP to Check</param>
        /// <returns>true if it's banned</returns>
        static public bool IsBanned(string ban, string chk)
        {
            var bannedIP = ban.Trim();
            if (chk == "::1" || string.IsNullOrEmpty(chk)) chk = "127.0.0.1";

            var ipmask = bannedIP.Split('.');
            var ip = bannedIP.Split('.');

            for (var i = 0; i < ipmask.Length; i++)
            {
                if (ipmask[i] == "*")
                {
                    ipmask[i] = "0";
                    ip[i] = "0";
                }
                else
                    ipmask[i] = "255";
            }

            var banmask = Str2IP(ip);
            var banchk = Str2IP(ipmask);
            var ipchk = Str2IP(chk.Split('.'));

            return (ipchk & banchk) == banmask;
        }

        // Returns #.#.#.* of full IP Address
        static public string GetIPMask(string IP)
        {
            var ip = "127.0.0.1";
            if (!string.IsNullOrEmpty(IP))
            {
                ip = IP.Substring(0, IP.LastIndexOf(".") + 1) + "*";
            }
            return ip;
        }
    }
}