using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationPOS.UI.MainFormDataSchemas.DTOs
{
    /// <summary>
    /// Abstract base class representing a data transfer object (DTO) for a product.
    /// Provides common properties such as product name, unit price, quantity, and total price.
    /// Derived classes can implement additional specific fields and functionality.
    /// 
    /// Author: Jason Lau
    /// Date: 10 April 2025
    /// 
    /// </summary>
    public abstract class ProductDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier for the product.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name or description of the product.
        /// This property is required, and its length must be between 1 and 30 characters.
        /// </summary>
        [Required(ErrorMessage = "ProductNameDescription is required")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "ProductNameDescription must be between 1 and 30 characters")]
        public string ProductName { get; set; }

        // Commented out properties for description (potentially future use)
        // [Required(ErrorMessage = "Description is required")]
        // [StringLength(500, MinimumLength = 1, ErrorMessage = "Description must be between 1 and 500 characters")]
        // public string Description { get; set; }

        /// <summary>
        /// Gets or sets the unit price of the product in dollars.
        /// This property is required, and its value must be between 0.0 and 10,000.0 dollars.
        /// </summary>
        [Required(ErrorMessage = "Price (dollars) is required")]
        [Range(0.0, 10000.0, ErrorMessage = "Unit Price (dollars) must be between 0.0 and 10000.0")]
        public decimal UnitPriceDollars { get; set; }

        // Unique DTO fields (not included in the model)

        /// <summary>
        /// Gets or sets the quantity of the product.
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Gets or sets the total price of the product (Quantity * UnitPrice).
        /// </summary>
        public decimal TotalPriceDollars { get; set; }

        /// <summary>
        /// Returns a string representation of the product with details like name, quantity, 
        /// unit price, and total price, formatted for display in a list.
        /// </summary>
        /// <returns>A formatted string representing the product.</returns>
        public override string ToString()
        {
            string spacing;
            if (ProductName.Length <= 12)
                spacing = "\t\t\t\t"; // Extra tab for very short names
            else if (ProductName.Length < 24)
                spacing = "\t\t\t";   // Three tabs for regular names
            else
                spacing = "\t\t";     // Spacing for longer names

            return $"{ProductName,-20}{spacing}{Quantity,-5}\t{UnitPriceDollars,-8:F2}\t{TotalPriceDollars,-8:F2}";
        }
    }

}
