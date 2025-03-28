using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.Core.Models.ProductModels;

namespace GasStationPOS.UI.ViewDataTransferObjects.ProductDTOs
{
    public class RetailProductDTO : ProductDTO
    {
        //// might change this later if we need more flexibility for category information
        //[Required(ErrorMessage = "Retail Category is required")]
        //public RetailCategory RetailCategory { get; set; }


        [Range(0.0, 10000.0, ErrorMessage = "Volume must be between 0.0 and 100000.0 L")]
        public decimal? ProductVolumeLitres { get; set; } = null; // for liquids (ex. beverages)

        public ProductSizeVariation? ProductSizeVariation { get; set; } = null; // for packaged (ex. snacks - SMALL/MEDIUM/LARGE)



        // Unique DTO fields (not included in the model)



    }
}
