using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Models.ProductModels;

namespace GasStationPOS.UI.MainFormDataSchemas.DTOs
{
    public class FuelProductDTO : ProductDTO
    {
        [Required(ErrorMessage = "Fuel Grade is required")]
        public FuelGrade FuelGrade { get; set; }

        [Required(ErrorMessage = "PumpNumber is required")]
        public int PumpNumber { get; set; }

        // Unique DTO fields (not included in the model)

        // Override ToString() to display the item in the list
        public override string ToString()
        {
            // create product name / description
            string fuelProductNameDescription = $"{ProductNameDescription}";

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
