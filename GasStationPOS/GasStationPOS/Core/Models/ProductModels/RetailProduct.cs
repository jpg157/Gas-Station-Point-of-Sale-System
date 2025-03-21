using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationPOS.Core.Models.ProductModels
{

    // might change this later if we need more flexibility for category information
    enum RetailCategory
    {
        BEVERAGE,
        SNACKS,
        LOTTERY,
        AUTOMOTIVE,
        HOUSEHOLD_UTILS,
        ELECTRONICS_ACCESSORIES
    }

    class RetailProduct : Product
    {
        // might change this later if we need more flexibility for category information
        [Required(ErrorMessage = "Retail Category is required")]
        public RetailCategory RetailCategory { get; set; }


        [Range(0.0, 10000.0, ErrorMessage = "Volume must be between 0.0 and 100000.0 L")]
        public decimal? ProductVolumeLitres { get; set; } = null; // for liquids (ex. beverages)


        [StringLength(1, MinimumLength = 1, ErrorMessage = "Description must be between 1 and 500 characters")] // S, M, or L
        public string ProductSizeVariation { get; set; } = null; // for packaged (ex. snacks - S/M/L)

        public override string ToString()
        {
            string strRet = $@"
                {base.ToString()}
                Retail Category: {RetailCategory}
                Volume Litres: {ProductVolumeLitres}
                Size: {ProductSizeVariation}
                ";
            return strRet;
        }
    }
}
