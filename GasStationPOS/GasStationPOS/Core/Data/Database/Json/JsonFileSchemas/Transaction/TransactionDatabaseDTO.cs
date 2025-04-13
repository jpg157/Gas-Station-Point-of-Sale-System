using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Models.ProductModels;
using GasStationPOS.Core.Data.Models.TransactionModels;

namespace GasStationPOS.Core.Data.Database.Json.JsonFileSchemas
{
    /// <summary>
    /// JSON file Data Transfer Object (DTO) for a transaction.
    /// Includes string versions of the DateTime and PaymentMethod enum fields.
    /// 
    /// Author: Jason Lau
    /// </summary>
    public class TransactionDatabaseDTO
    {
        //[Required(ErrorMessage = "Id is required")]
        //public int Id { get; set; }

        [JsonPropertyName("TransactionNumber")]
        [Required(ErrorMessage = "TransactionNumber is required")]
        public int TransactionNumber { get; set; } // might not need and can instead use Id

        [JsonPropertyName("TransactionFuelProductItems")]
        [Required(ErrorMessage = "Transaction Fuel Product Items are required")]
        public List<TransactionFuelProductItem> TransactionFuelProductItems { get; set; } // TransactionFuelProductItem contains: FuelProduct, Quantity, TotalItemPriceDollars)

        [JsonPropertyName("TransactionRetailProductItems")]
        [Required(ErrorMessage = "Transaction Retail Product Items are required")]
        public List<TransactionRetailProductItem> TransactionRetailProductItems { get; set; } // TransactionRetailProductItem contains: RetailProduct, Quantity, TotalItemPriceDollars)

        [JsonPropertyName("PaymentMethod")]
        [Required(ErrorMessage = "Payment Method is required")]
        public string PaymentMethod { get; set; }

        [JsonPropertyName("TotalAmountDollars")]
        [Required(ErrorMessage = "Total Amount Dollars is required")]
        [Range(0.0, 10000.0, ErrorMessage = "TotalAmountDollars must be between 0.0 and 10000.0")]
        public decimal TotalAmountDollars { get; set; }

        [JsonPropertyName("ChangeDollars")]
        [Required(ErrorMessage = "Change (dollars) is required")]
        [Range(0.0, 10000.0, ErrorMessage = "ChangeDollars must be between 0.0 and 10000.0")]
        public decimal ChangeDollars { get; set; }

        [JsonPropertyName("TransactionDateTime")]
        [Required(ErrorMessage = "Transaction Date Time is required")]
        public string TransactionDateTime { get; set; }

        //[Required(ErrorMessage = "CashierId is required")]
        //public int CashierId { get; set; } // for Cashier Id and/or Cashier Name to store in transaction

        public override string ToString()
        {
            string strRet = $@"
                Transaction Number: {TransactionNumber}
                TransactionFuelProductItems: {TransactionFuelProductItems}
                TransactionRetailProductItems: {TransactionRetailProductItems}
                Payment Method: {PaymentMethod}
                Total Amount (Dollars): {TotalAmountDollars}
                Change (Dollars): {ChangeDollars}
                Transaction Date: {TransactionDateTime}
                ";
            return strRet;
        }

        // IF TIME

        //[Required(ErrorMessage = "Loyalty Points Earned is required")]
        //public int LoyaltyPointsEarned { get; set; }

        //[Required(ErrorMessage = "GST (dollars) is required")]
        //public decimal GSTDollars { get; set; }

        //[Required(ErrorMessage = "PST (dollars) is required")]
        //public decimal PSTDollars { get; set; }
    }
}
