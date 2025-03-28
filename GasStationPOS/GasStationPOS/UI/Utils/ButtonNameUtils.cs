using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.UI.Constants;

namespace GasStationPOS.UI
{
    /// <summary>
    /// Temporary workaround Util class to convert the product ids in 
    /// retail_products.json to the button strings in the main form.
    /// Idk how else to map the retail product ids to the button names.
    static class ButtonNameUtils
    {
        /// <summary>
        /// Returns the retail button name when passed a retail product Id (currently all are stored in json database)
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static string GetRetailButtonNameFromProductId(int productId)
        {
            //// If the productId does not exist in the list
            //if (productId < 1 || productId > 26) // Assuming the IDs are within this range
            //{
            //    throw new InvalidOperationException("Retail productId does not exist in the list.");
            //}

            return $"{ButtonNamePrefixes.RETAIL_BUTTON_PREFIX}{productId}";
        }

        /// <summary>
        /// Returns the quantity button name when passed a quantity number (currently all are stored in json database)
        /// </summary>
        /// <param name="quantityNum"></param>
        /// <returns></returns>
        public static string GetQuantityButtonNameFromQuantityNumber(int quantityNum)
        {
            //// If the quantityNum does not exist in the list
            //if (quantityNum < 1 || quantityNum > 26) // Assuming the IDs are within this range
            //{
            //    throw new InvalidOperationException("Retail quantityNum does not exist in the list.");
            //}

            return $"{ButtonNamePrefixes.QUANTITY_BUTTON_PREFIX}{quantityNum}";
        }
    }
}
