using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationPOS.Core.Data.Models.ProductModels
{
    /// <summary>
    /// Fuel grade of the fuel product.
    /// </summary>
    public enum FuelGrade
    {
        REGULAR,
        PLUS,
        SUPREME
    }

    /// <summary>
    /// FuelProduct data model class.
    /// 
    /// Author: Jason Lau
    /// </summary>
    public class FuelProduct : Product
    {
        // PriceDollars for FuelProduct (inherited from Product) is the PRICE ($)/LITRE.
        // To display the price of the FuelProduct in cents (¢), will need to divide PriceDollars by 100.0.

        [Required(ErrorMessage = "Fuel Grade is required")]
        public FuelGrade FuelGrade { get; set; }

        [Required(ErrorMessage = "PumpNumber is required")]
        public int PumpNumber { get; set; }

        public override string ToString()
        {
            // create product name / description
            string fuelProductNameDescription = $"{ProductName}";
            string strRet =
                $"{base.ToString()}\n" +
                $"{fuelProductNameDescription}\n" +
                $"{FuelGrade}" +
                $"{PumpNumber}";
            return strRet;
        }
    }

    /// <summary>
    /// Util class (not part of the data model)
    /// - used for calculations for displaying in UI and storing in DB)
    /// </summary>
    public static class FuelGradeUtils
    {
        /// <summary>
        /// Returns the fuel price (in dollars / litre) for the entered FuelGrade
        /// </summary>
        /// <param name="fuelGrade"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
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
    }
}
