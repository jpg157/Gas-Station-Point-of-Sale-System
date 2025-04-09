using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationPOS.Core.Data.Models.ProductModels
{
    public class BarcodeRetailProduct : RetailProduct
    {
        [Required(ErrorMessage = "BarcodeId is required")]
        public string BarcodeId { get; set; }

        // For testing
        public override string ToString()
        {
            string strRet =
                $"{base.ToString()}\n" +
                $"BarcodeId: {BarcodeId}";
            return strRet;
        }
    }
}
