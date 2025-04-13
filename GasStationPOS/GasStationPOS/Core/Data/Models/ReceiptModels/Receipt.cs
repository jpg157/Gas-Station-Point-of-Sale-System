using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Models.TransactionModels;

namespace GasStationPOS.Core.Data.Models.ReceiptModels
{
    /// <summary>
    /// Receipt data model class. Not used.
    /// </summary>
    public class Receipt
    {
        public const string VENDOR_NAME             = "Shakestack Gas Station";
        public const string VENDOR_STORE_NUM        = "0001";
        public const string VENDOR_PHONE_NUMBER     = "778-888-8888";
        public const string VENDOR_STREET_ADDRESS   = "8888 Willingdon Ave";
        public const string VENDOR_LOCATION_ADDRESS = "Burnaby, BC, V5B 8P8";

        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "TransactionDetails is required")]
        public Transaction TransactionDetails { get; set; }
    }
}
