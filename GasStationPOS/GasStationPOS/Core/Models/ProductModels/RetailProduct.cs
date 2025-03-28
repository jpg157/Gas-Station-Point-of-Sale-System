using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationPOS.Core.Models.ProductModels
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

    class RetailProduct : Product
    {
        //// might change this later if we need more flexibility for category information
        //[Required(ErrorMessage = "Retail Category is required")]
        //public RetailCategory RetailCategory { get; set; }


        [Range(0.0, 10000.0, ErrorMessage = "Volume must be between 0.0 and 100000.0 L")]
        public decimal? ProductVolumeLitres { get; set; } = null; // for liquids (ex. beverages)

        public ProductSizeVariation? ProductSizeVariation { get; set; } = null; // for packaged (ex. snacks - S/M/L)

        // For testing
        public override string ToString()
        {
            string strRet = $@"
                {base.ToString()}
                Volume Litres: {ProductVolumeLitres}
                Size: {ProductSizeVariation}
                ";//Retail Category: {RetailCategory}
            return strRet;
        }
    }
}
