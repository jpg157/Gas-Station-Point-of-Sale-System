using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Models.ProductModels;

namespace GasStationPOS.Core.Data.Database.Json.JsonFileSchemas
{
    /// <summary>
    /// Retail products JSON file structure used by system.text.json serialize and deserialize methods.
    /// Class structure should match the json file structure.
    /// 
    /// Author: Jason Lau
    /// 
    /// </summary>
    public class RetailProductsJSONStructure
    {
        /// <summary>
        /// Gets or sets the list of retail products.
        /// </summary>
        public List<RetailProduct> RetailProducts { get; set; } // The C# property needs to match the JSON key (case-insensitive by default)
    }
}
