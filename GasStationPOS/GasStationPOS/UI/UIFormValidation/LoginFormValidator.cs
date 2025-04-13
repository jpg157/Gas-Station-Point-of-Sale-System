using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationPOS.UI.UIFormValidation
{
    /// <summary>
    /// Validator class for the login form.
    /// </summary>
    public static class LoginFormValidator
    {
        /// <summary>
        /// Validates the username and password string values.
        /// </summary>
        public static bool ValidateFields(string enteredUsername, string enteredPassword)
        {
            // Check if inputs are empty
            if (string.IsNullOrEmpty(enteredUsername) || string.IsNullOrEmpty(enteredPassword))
            {
                return false;
            }
            return true;
        }
    }
}
