using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Models.ProductModels;

namespace GasStationPOS.Core.Data.Models.TransactionModels
{
    public class TransactionRetailProductItem
    {
        public RetailProduct RetailProduct { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalItemPriceDollars { get; set; }

        public override string ToString()
        {
            string strRet = $"{RetailProduct}\nQuantity: {Quantity}, TotalItemPriceDollars: {TotalItemPriceDollars}";
            return strRet;
        }
    }
}
