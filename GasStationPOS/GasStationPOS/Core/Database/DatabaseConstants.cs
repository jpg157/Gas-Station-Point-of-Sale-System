using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationPOS.Core.Database
{
    class DatabaseConstants
    {
        // (Will change and add other db constants once we migrate to SQL database)

        private static readonly string BASE_DIRECTORY_PATH = AppDomain.CurrentDomain.BaseDirectory; // Base directory of the application
        private static readonly string JSON_DB_DIRECTORY_NAME = "Temp_Database"; // JSON database directory name

        // JSON File names ===
        private static readonly string RETAIL_PRODUCTS_JSON_FILE_NAME   = "retail_products.json";
        private static readonly string FUEL_PRODUCTS_JSON_FILE_NAME     = "fuel_products.json";
        //...

        // Public constants ===
        public static readonly string RETAIL_PRODUCTS_JSON_FILE_PATH = Path.Combine(BASE_DIRECTORY_PATH, JSON_DB_DIRECTORY_NAME, RETAIL_PRODUCTS_JSON_FILE_NAME);
    }
}
