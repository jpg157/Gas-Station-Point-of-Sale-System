using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Models.ProductModels;

namespace GasStationPOS.UI.MainFormDataSchemas.DTOs
{
    public class RetailProductDTO : ProductDTO
    {
        //// might change this later if we need more flexibility for category information
        //[Required(ErrorMessage = "Retail Category is required")]
        //public RetailCategory RetailCategory { get; set; }

        // Unique DTO fields (not included in the model)

    }
}
