using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationPOS.Core.Models.ProductModels
{
    enum FuelGrade
    {
        REGULAR,
        PLUS,
        SUPREME
    }

    class FuelProduct : Product
    {
        // PriceDollars for FuelProduct (inherited from Product) is the PRICE ($)/LITRE.
        // To display the price of the FuelProduct in cents (¢), will need to divide PriceDollars by 100.0.

        [Required(ErrorMessage = "Fuel Grade is required")]
        public FuelGrade FuelGrade { get; set; }

        [Required(ErrorMessage = "Fuel Volume is required")]
        [Range(0.0, 10000.0, ErrorMessage = "Fuel volume must be between 0.0 and 10000.0 L")]
        public decimal FuelVolumeLitres { get; set; }

        public override string ToString()
        {
            string strRet = $@"
                {base.ToString()}
                Fuel Grade: {FuelGrade}
                Fuel Volume Litres: {FuelVolumeLitres}
                ";
            return strRet;
        }
    }
}
