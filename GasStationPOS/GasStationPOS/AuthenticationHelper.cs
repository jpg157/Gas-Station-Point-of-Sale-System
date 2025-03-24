using GasStationPOS.MockDatabase;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace GasStationPOS
{

    public class AuthenticationHelper
    {
        /// <summary>
        /// Hashes a password using SHA-256.
        /// </summary>
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        /// <summary>
        /// Validates user credentials by decrypting stored passwords and comparing hashes.
        /// </summary>
        public static bool ValidateAccount(string accountID, string enteredPassword, Database database)
        {
            // Hash the entered password
            string enteredHashedPassword = HashPassword(enteredPassword);

            // Find the user
            var userAccount = database.Accounts.FirstOrDefault(acc => acc.AccountID == accountID);

            // Compare hashed passwords directly
            return userAccount != null && userAccount.Password == enteredHashedPassword;
        }
    }
}
