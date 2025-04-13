using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GasStationPOS.Core.Services.Utils
{
    /// <summary>
    /// Password encyption static utility class for hashing password.
    /// </summary>
    public static class PasswordEncryption
    {
        /// <summary>
        /// Hashes the password if it is not null.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string HashPassword(string password)
        {
            if (password == null) return "";

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
