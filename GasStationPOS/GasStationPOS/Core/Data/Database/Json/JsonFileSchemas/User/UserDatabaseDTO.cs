using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GasStationPOS.Core.Data.Database.Json.JsonFileSchemas
{
    /// <summary>
    /// JSON file Data Transfer Object (DTO) for a retail product.
    /// 
    /// Author: Jason Lau
    /// </summary>
    public class UserDatabaseDTO
    {
        [JsonPropertyName("Id")]
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }

        [JsonPropertyName("Username")]
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Username must be between 4 and 50 characters")]
        public string Username { get; set; }

        [JsonPropertyName("Password")]
        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 50 characters")]
        public string Password { get; set; } // NEED TO HASH THIS BEFORE ADDING IT INTO THE DATABASE/JSON FILE - using PasswordEncryption class

        [JsonPropertyName("FirstName")]
        [Required(ErrorMessage = "FirstName is required")]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "FirstName must be between 1 and 1000 characters")]
        public string FirstName { get; set; }

        [JsonPropertyName("LastName")]
        [Required(ErrorMessage = "LastName is required")]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "LastName must be between 1 and 1000 characters")]
        public string LastName { get; set; }

        [JsonPropertyName("Email")]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [StringLength(320, MinimumLength = 7, ErrorMessage = "Email must be between 7 and 320 characters")]
        public string Email { get; set; }

        [JsonPropertyName("PhoneNumber")]
        [Required(ErrorMessage = "PhoneNumber is required")]
        [Phone]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "PhoneNumber must be 10 digits long")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "PhoneNumber must only contain digits")]
        public string PhoneNumber { get; set; } // need to remove all whitespace and "-" from the string value before assigning to PhoneNumber

        [JsonPropertyName("DateCreated")]
        [Required(ErrorMessage = "DateCreated is required")]
        public DateTime DateCreated { get; set; }

        [JsonPropertyName("DateTerminated")]
        [Required(ErrorMessage = "DateTerminated is required")]
        public DateTime? DateTerminated { get; set; } = null;

        [JsonPropertyName("Role")]
        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; } // database roles are stored as strings
    }
}
