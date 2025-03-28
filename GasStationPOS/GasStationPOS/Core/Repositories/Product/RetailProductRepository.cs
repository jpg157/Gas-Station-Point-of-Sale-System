using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GasStationPOS.Core.Database;
using GasStationPOS.Core.DatabaseDTOs;
using GasStationPOS.Core.Models.ProductModels;

namespace GasStationPOS.Core.Repositories.Product
{
    class RetailProductRepository : IRetailProductRepository
    {
        public IEnumerable<RetailProduct> GetAll()
        {
            List<RetailProduct> retailProductDataList;

            //string filePath     = DatabaseConstants.RETAIL_PRODUCTS_JSON_FILE_PATH;
            //string jsonString   = File.ReadAllText(filePath);
            //RetailProductListWrapper wrapper = JsonSerializer.Deserialize<RetailProductListWrapper>(jsonString);
            //retailProductDataList = wrapper?.RetailProducts ?? new List<RetailProduct>(); // set retailProductDataList to RetailProductDataList in wrapper if not null, otherwise create new List


            using (FileStream openStream = File.OpenRead(DatabaseConstants.RETAIL_PRODUCTS_JSON_FILE_PATH))
            {
                RetailProductListWrapper wrapper = JsonSerializer.Deserialize<RetailProductListWrapper>(openStream);
                retailProductDataList = wrapper?.RetailProducts ?? new List<RetailProduct>(); // set retailProductDataList to RetailProductDataList in wrapper if not null, otherwise create new List
            }

            return retailProductDataList;
        }
    }
}
