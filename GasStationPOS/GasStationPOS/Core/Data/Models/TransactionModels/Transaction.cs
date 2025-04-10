using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GasStationPOS.Core.Data.Models.TransactionModels
{
    public enum PaymentMethod
    {
        CASH,
        CARD
    }
    public class Transaction
    {
        //[Required(ErrorMessage = "Id is required")]
        //public int Id { get; set; }

        [Required(ErrorMessage = "TransactionNumber is required")]
        public int TransactionNumber { get; set; } // might not need and can instead use Id

        [Required(ErrorMessage = "Transaction Fuel Product Items are required")]
        public List<TransactionFuelProductItem> TransactionFuelProductItems { get; set; } // TransactionFuelProductItem contains: FuelProduct, Quantity, TotalItemPriceDollars)

        [Required(ErrorMessage = "Transaction Retail Product Items are required")]
        public List<TransactionRetailProductItem> TransactionRetailProductItems { get; set; } // TransactionRetailProductItem contains: RetailProduct, Quantity, TotalItemPriceDollars)

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

        public override string ToString()
        {
            string transactionFPItemsStr = "";
            foreach (TransactionFuelProductItem tFPItem in TransactionFuelProductItems)
            {
                transactionFPItemsStr += $"{tFPItem}\n";
            }

            string transactionRPItemsStr = "";
            foreach (TransactionRetailProductItem tRPItem in TransactionRetailProductItems)
            {
                transactionRPItemsStr += $"{tRPItem}\n\n";
            }

            string strRet = 
                $"Transaction Number: {TransactionNumber}\n" +
                $"TransactionFuelProductItems:\n{transactionFPItemsStr}" +
                $"TransactionRetailProductItems:\n{transactionRPItemsStr}" +
                $"Payment Method: {PaymentMethod}\n" +
                $"Total Amount (Dollars): {TotalAmountDollars}\n" +
                $"Change (Dollars): {ChangeDollars}\n" +
                $"Transaction Date: {TransactionDateTime.ToString(TransactionConstants.TransactionDatetimeFormat)}";
            return strRet;
        }

        //[Required(ErrorMessage = "CashierId is required")]
        //public int CashierId { get; set; } // for Cashier Id and/or Cashier Name to store in transaction

        // IF TIME

        //[Required(ErrorMessage = "Loyalty Points Earned is required")]
        //public int LoyaltyPointsEarned { get; set; }

        //[Required(ErrorMessage = "GST (dollars) is required")]
        //public decimal GSTDollars { get; set; }

        //[Required(ErrorMessage = "PST (dollars) is required")]
        //public decimal PSTDollars { get; set; }
    }

    public class TransactionConstants
    {
        public static readonly string TransactionDatetimeFormat = "dddd, MMMM dd, yyyy HH:mm:ss";
        public static readonly int    INITIAL_TRANSACTION_NUM = 1;
    }
}
