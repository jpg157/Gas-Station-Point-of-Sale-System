using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.Core.Models.ProductModels;

namespace GasStationPOS.Core.DatabaseDTOs
{
    class RetailProductListWrapper
    {
        public List<RetailProduct> RetailProducts { get; set; } // The C# property needs to match the JSON key (case-insensitive by default)
    }
}
