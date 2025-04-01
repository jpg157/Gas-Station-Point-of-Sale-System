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

        //[Required(ErrorMessage = "Fuel Volume is required")]
        //[Range(0.0, 10000.0, ErrorMessage = "Fuel volume must be between 0.0 and 10000.0 L")]
        //public decimal FuelVolumeLitres { get; set; }



        //// Unique DTO fields (not included in the model)


        //[Required(ErrorMessage = "PumpNumber is required")]
        //public int PumpNumber { get; set; }

        //Override ToString() method to display something like:
        // PUMP 1 REGULAR <quantity in litres> <price (dollars) per litre> <total price (dollars) (= quantity * price)>
    }
}
