using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationPOS.UI.Constants
{
    /// <summary>
    /// Button name prefixes used for groups of like-buttons 
    /// (ex. retail product add button, fuel pump button, quantity selection button).
    /// </summary>
    public static class ButtonNamePrefixes
    {
        public static readonly string RETAIL_BUTTON_PREFIX      = "btnRp";
        public static readonly string QUANTITY_BUTTON_PREFIX    = "btnQty";
        public static readonly string FUEL_PUMP_BUTTON_PREFIX   = "btnFP";
    }
}
