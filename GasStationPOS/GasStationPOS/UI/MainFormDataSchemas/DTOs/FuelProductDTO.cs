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
    /// Data Transfer Object (DTO) for a fuel product.
    /// Inherits from <see cref="ProductDTO"/> and includes additional properties specific to fuel products,
    /// such as FuelGrade and PumpNumber.
    /// 
    /// Author: Jason Lau
    /// Date: 10 April 2025
    /// 
    /// </summary>
    public class FuelProductDTO : ProductDTO
    {
        /// <summary>
        /// Gets or sets the grade of the fuel for the product.
        /// This property is required and an error message will be shown if not provided.
        /// </summary>
        [Required(ErrorMessage = "Fuel Grade is required")]
        public FuelGrade FuelGrade { get; set; }

        /// <summary>
        /// Gets or sets the pump number associated with the fuel product.
        /// This property is required and an error message will be shown if not provided.
        /// </summary>
        [Required(ErrorMessage = "PumpNumber is required")]
        public int PumpNumber { get; set; }

        // Unique DTO fields (not included in the model)

        /// <summary>
        /// Returns a string representation of the fuel product with details like name, quantity, 
        /// unit price, and total price, formatted for display in a list.
        /// </summary>
        /// <returns>A formatted string representing the fuel product.</returns>
        public override string ToString()
        {
            // create product name / description
            string fuelProductNameDescription = $"{ProductName}";

            string spacing;
            if (fuelProductNameDescription.Length < 12)
                spacing = "\t\t\t\t"; // Extra tab for very short names
            else if (fuelProductNameDescription.Length < 24)
                spacing = "\t\t\t";   // Three tabs for regular names
            else
                spacing = "\t\t";     // Spacing for longer names

            return $"{fuelProductNameDescription,-20}{spacing}{Quantity,-5:F3}\t{UnitPriceDollars,-8:F2}\t{TotalPriceDollars,-8:F2}";
        }
    }

}
