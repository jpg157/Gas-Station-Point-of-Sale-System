using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationPOS.Core.Models.ProductModels
{
    public enum FuelGrade
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

        //[Required(ErrorMessage = "Fuel Volume is required")]
        //[Range(0.0, 10000.0, ErrorMessage = "Fuel volume must be between 0.0 and 10000.0 L")]
        //public decimal FuelVolumeLitres { get; set; }

        public override string ToString()
        {
            string strRet = $@"
                {base.ToString()}
                Fuel Grade: {FuelGrade}
                ";//Fuel Volume Litres: {FuelVolumeLitres}
            return strRet;
        }
    }

    /// <summary>
    /// Util class (not part of the data model
    /// - used for calculations for displaying in UI and storing in DB)
    /// </summary>
    static class FuelGradeUtils
    {
        public static decimal GetFuelPrice(FuelGrade fuelGrade)
        {
            switch (fuelGrade)
            {
                case FuelGrade.REGULAR: return 1.649m;
                case FuelGrade.PLUS:    return 1.849m;
                case FuelGrade.SUPREME: return 2.049m;
                default: throw new InvalidOperationException("Invalid fuel grade.");
            };
        }

        public static FuelGrade GetFuelGradeFromLabel(string fuelGradeLabel)
        {
            switch (fuelGradeLabel)
            {
                case "Regular": return FuelGrade.REGULAR;
                case "Plus":    return FuelGrade.PLUS;
                case "Supreme": return FuelGrade.SUPREME;
                default: throw new InvalidOperationException("Invalid fuel grade label.");
            }
            ;
        }
    }
}
