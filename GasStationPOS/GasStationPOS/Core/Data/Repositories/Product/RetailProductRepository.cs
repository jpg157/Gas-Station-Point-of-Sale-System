using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Database.Json.JsonFileSchemas;
using GasStationPOS.Core.Data.Models.ProductModels;
using GasStationPOS.Core.Database;
using GasStationPOS.Core.Database.Json;

namespace GasStationPOS.Core.Data.Repositories.Product
{
    /// <summary>
    /// Repository class that does crud operations on the data source.
    /// Data source is currently a json file.
    /// 
    /// Author: Jason Lau
    /// 
    /// </summary>
    public class RetailProductRepository : IRetailProductRepository
    {
        /// <summary>
        /// Gets all retail products from the json file.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RetailProduct> GetAll()
        {
            List<RetailProduct> retailProductDataList;

            using (FileStream openStream = File.OpenRead(JsonFileConstants.RETAIL_PRODUCTS_JSON_FILE_PATH))
            {
                RetailProductsJSONStructure wrapper    = JsonSerializer.Deserialize<RetailProductsJSONStructure>(openStream);
                retailProductDataList               = wrapper?.RetailProducts ?? new List<RetailProduct>(); // set retailProductDataList to RetailProductDataList in wrapper if not null, otherwise create new List
            }

            return retailProductDataList;
        }
    }
}
