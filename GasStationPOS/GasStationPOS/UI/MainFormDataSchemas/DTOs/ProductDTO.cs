using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationPOS.UI.MainFormDataSchemas.DTOs
{
    public abstract class ProductDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "ProductNameDescription is required")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "ProductNameDescription must be between 1 and 30 characters")]
        public string ProductNameDescription { get; set; }

        //[Required(ErrorMessage = "Description is required")]
        //[StringLength(500, MinimumLength = 1, ErrorMessage = "Description must be between 1 and 500 characters")]
        //public string Description { get; set; }

        [Required(ErrorMessage = "Price (dollars) is required")]
        [Range(0.0, 10000.0, ErrorMessage = "Price (dollars) must be between 0.0 and 10000.0")]
        public decimal PriceDollars { get; set; }



        // Unique DTO fields (not included in the model)


        public decimal Quantity { get; set; }
        public decimal TotalPriceDollars { get; set; }

        // Override ToString() to display the item in the list
        public override string ToString()
            {
                string spacing;
                if (ProductNameDescription.Length < 12)
                    spacing = "\t\t\t\t"; // Extra tab for very short names
                else if (ProductNameDescription.Length < 24)
                    spacing = "\t\t\t";   // Three tabs for regular names
                else
                    spacing = "\t\t";     // Spacing for longer names

                return $"{ProductNameDescription,-20}{spacing}{Quantity,-5}\t{PriceDollars,-8:F2}\t{TotalPriceDollars,-8:F2}";
            }
        }
}
