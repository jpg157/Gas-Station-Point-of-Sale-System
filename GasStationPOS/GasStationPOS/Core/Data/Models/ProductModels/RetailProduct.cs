using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationPOS.Core.Data.Models.ProductModels
{

    // might change this later if we need more flexibility for category information
    public enum RetailCategory
    {
        BEVERAGE,
        SNACKS,
        LOTTERY,
        AUTOMOTIVE,
        HOUSEHOLD_UTILS,
        ELECTRONICS_ACCESSORIES
    }
    public enum ProductSizeVariation
    {
        SMALL,
        MEDIUM,
        LARGE
    }

    public class RetailProduct : Product
    {
        //// might change this later if we need more flexibility for category information
        //[Required(ErrorMessage = "Retail Category is required")]
        //public RetailCategory RetailCategory { get; set; }

        // For testing
        public override string ToString()
        {
            string strRet =
                $"{base.ToString()}";
                //$"Retail Category: {RetailCategory}";
            return strRet;
        }
    }
}
