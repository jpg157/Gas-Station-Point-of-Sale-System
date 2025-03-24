using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GasStationPOS.MockDatabase
{
    /// <summary>
    /// Represents the database that stores user accounts.
    /// </summary>
    class Database
    {
        [JsonPropertyName("Accounts")]
        public List<Account> Accounts { get; set; } = new List<Account>();
    }

    /// <summary>
    /// Represents a user account in the database.
    /// </summary>
    class Account
    {
        [JsonPropertyName("AccountID")]
        public string AccountID { get; set; } = "";

        [JsonPropertyName("Password")]
        public string Password { get; set; } = "";
    }
}