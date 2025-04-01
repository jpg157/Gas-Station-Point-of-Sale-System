using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Models.ProductModels;


namespace GasStationPOS.Core.Data.Models.TransactionModels
{
    public enum PaymentMethod
    {
        CASH,
        CARD
    }

    public class Transaction
    {
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "TransactionNumber is required")]
        public int TransactionNumber { get; set; } // might not need and can instead use Id

        [Required(ErrorMessage = "Transaction Fuel Product Items are required")]
        public List<Tuple<FuelProduct, int>> TransactionFuelProductItems { get; set; } // Tuple (FuelProduct, quantity)

        [Required(ErrorMessage = "Transaction Retail Product Items are required")]
        public List<Tuple<RetailProduct, int>> TransactionRetailProductItems { get; set; } // Tuple (RetailProduct, quantity)

        [Required(ErrorMessage = "Payment Method is required")]
        public PaymentMethod PaymentMethod { get; set; }

        [Required(ErrorMessage = "Total Amount Dollars is required")]
        [Range(0.0, 10000.0, ErrorMessage = "TotalAmountDollars must be between 0.0 and 10000.0")]
        public decimal TotalAmountDollars { get; set; }

        [Required(ErrorMessage = "Change (dollars) is required")]
        [Range(0.0, 10000.0, ErrorMessage = "ChangeDollars must be between 0.0 and 10000.0")]
        public decimal ChangeDollars { get; set; }

        [Required(ErrorMessage = "Transaction Date Time is required")]
        public DateTime TransactionDateTime { get; set; }

        [Required(ErrorMessage = "CashierId is required")]
        public int CashierId { get; set; } // for Cashier Id and/or Cashier Name to store in transaction

        // IF TIME

        //[Required(ErrorMessage = "Loyalty Points Earned is required")]
        //public int LoyaltyPointsEarned { get; set; }

        //[Required(ErrorMessage = "GST (dollars) is required")]
        //public decimal GSTDollars { get; set; }

        //[Required(ErrorMessage = "PST (dollars) is required")]
        //public decimal PSTDollars { get; set; }
    }
}
