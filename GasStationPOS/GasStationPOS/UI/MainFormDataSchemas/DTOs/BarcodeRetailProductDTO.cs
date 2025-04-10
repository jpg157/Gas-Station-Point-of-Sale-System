using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationPOS.UI.MainFormDataSchemas.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for a barcode retail product.
    /// Inherits from <see cref="RetailProductDTO"/> and adds a required BarcodeId property.
    /// 
    /// Author: Mansib Talukder
    /// Date: 10 April 2025
    /// 
    /// </summary>
    public class BarcodeRetailProductDTO : RetailProductDTO
    {
        /// <summary>
        /// Gets or sets the unique barcode identifier for the retail product.
        /// This property is required, and an error message will be shown if not provided.
        /// </summary>
        [Required(ErrorMessage = "BarcodeId is required")]
        public string BarcodeId { get; set; }
    }

}
