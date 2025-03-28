using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GasStationPOS.Core.Models.ProductModels
{
    abstract class Product 
    {
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Product Name is required")]
        [StringLength(30, MinimumLength = 1, ErrorMessage  = "Product name must be between 1 and 30 characters")]
        public string ProductName { get; set; }

        //[Required(ErrorMessage = "Description is required")]
        //[StringLength(500, MinimumLength = 1, ErrorMessage = "Description must be between 1 and 500 characters")]
        //public string Description { get; set; }

        [Required(ErrorMessage = "Price (dollars) is required")]
        [Range(0.0, 10000.0, ErrorMessage = "Price (dollars) must be between 0.0 and 10000.0")]
        public decimal PriceDollars { get; set; }

        public override string ToString()
        {
            string strRet = $@"
                Product Id: {Id}
                Product Name: {ProductName}
                Product PriceDollars: {PriceDollars}
                ";//Product Description: {Description}
            return strRet;
        }
    }
}
