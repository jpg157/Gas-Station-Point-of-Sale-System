using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationPOS.UI.UIFormValidation
{
    public static class LoginFormValidator
    {
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
