using System.Security.Cryptography;
using System.Text;

namespace ScrewTurn.Wiki.Plugins.SqlCommon
{
    /// <summary>
    ///     Computes hashes.
    /// </summary>
    public static class Hash
    {
        /// <summary>
        ///     Computes the Hash code of a string.
        /// </summary>
        /// <param name="input">The string.</param>
        /// <returns>The Hash code.</returns>
        private static byte[] ComputeBytes(string input)
        {
            var sha1 = SHA1.Create();
            return sha1.ComputeHash(Encoding.ASCII.GetBytes(input));
        }

        /// <summary>
        ///     Computes the Hash code of a string and converts it into a Hex string.
        /// </summary>
        /// <param name="input">The string.</param>
        /// <returns>The Hash code, converted into a Hex string.</returns>
        public static string Compute(string input)
        {
            var bytes = ComputeBytes(input);
            var result = "";
            for (var i = 0; i < bytes.Length; i++)
            {
                result += string.Format("{0:X2}", bytes[i]);
            }
            return result;
        }
    }
}