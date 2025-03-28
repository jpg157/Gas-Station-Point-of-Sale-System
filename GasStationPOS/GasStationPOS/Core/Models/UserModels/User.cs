using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GasStationPOS.Core.Models.UserModels
{
    enum UserRole
    {
        ADMIN,
        CASHIER,
        MANAGER
    }
     
    abstract class User
    {
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Username must be between 4 and 50 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 50 characters")]
        public string Password { get; set; } // NEED TO HASH THIS BEFORE ADDING IT INTO THE DATABASE/JSON FILE - using PasswordEncryption class

        [Required(ErrorMessage = "FirstName is required")]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "FirstName must be between 1 and 1000 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "LastName must be between 1 and 1000 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [StringLength(320, MinimumLength = 7, ErrorMessage = "Email must be between 7 and 320 characters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required")]
        [Phone]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "PhoneNumber must be 10 digits long")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "PhoneNumber must only contain digits")]
        public string PhoneNumber { get; set; } // need to remove all whitespace and "-" from the string value before assigning to PhoneNumber

        [Required(ErrorMessage = "DateCreated is required")]
        public DateTime DateCreated { get; set; }

        [Required(ErrorMessage = "DateTerminated is required")]
        public DateTime? DateTerminated { get; set; } = null;

        [Required(ErrorMessage = "Role is required")]
        public UserRole Role { get; set; } // To be set by child classes
    }
}
