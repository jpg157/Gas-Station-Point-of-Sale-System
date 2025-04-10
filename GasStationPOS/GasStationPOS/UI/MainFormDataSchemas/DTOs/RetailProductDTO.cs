using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Models.ProductModels;

namespace GasStationPOS.UI.MainFormDataSchemas.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for a retail product.
    /// Inherits from <see cref="ProductDTO"/> and can include additional fields specific to retail products.
    /// Currently, it does not include any additional fields, but there is a placeholder for potential 
    /// retail category information that could be added later for more flexibility.
    /// 
    /// Author: Jason Lau
    /// Date: 10 April 2025
    /// 
    /// </summary>
    public class RetailProductDTO : ProductDTO
    {
        //// might change this later if we need more flexibility for category information
        //[Required(ErrorMessage = "Retail Category is required")]
        //public RetailCategory RetailCategory { get; set; }

        // Unique DTO fields (not included in the model)

    }
}
