using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Models.ProductModels;

namespace GasStationPOS.Core.Data.Models.TransactionModels
{
    /// <summary>
    /// Data class for storing transaction fuel product items in the json file.
    /// 
    /// Author: Jason Lau
    /// </summary>
    public class TransactionFuelProductItem
    {
        public FuelProduct FuelProduct { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalItemPriceDollars { get; set; }

        public override string ToString()
        {
            string strRet = $"{FuelProduct}\nQuantity: {Quantity}, TotalItemPriceDollars: {TotalItemPriceDollars}";
            return base.ToString();
        }
    }
}
