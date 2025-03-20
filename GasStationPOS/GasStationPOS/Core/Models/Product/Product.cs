using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GasStationPOS.Core.Models.Product
{
    abstract class Product
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string ProductName { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Range(0.0, 10000.0, ErrorMessage = "Price (dollars) must be between 0.0 and 10000.0")]
        public decimal PriceDollars { get; set; }

        public override string ToString()
        {
            string strRet = $@"
                Product Id: {Id}
                Product Name: {ProductName}
                Product Description: {Description}
                Product PriceDollars: {PriceDollars}
                ";
            return strRet;
        }
    }
}
