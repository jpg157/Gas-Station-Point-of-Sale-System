using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationPOS.Core.Database.Json
{
    public class JsonDBConstants
    {
        // (Will change and add other db constants if we migrate to SQL database)
        private static readonly string BASE_DIRECTORY_PATH = AppDomain.CurrentDomain.BaseDirectory; // Base directory of the application
        private static readonly string PATH_TO_JSON_DB = Path.Combine("Core", "Data", "Database", "Json");
        private static readonly string JSON_DB_NAME = "MockDatabase"; // JSON database directory name

        // JSON File names ===
        private static readonly string RETAIL_PRODUCTS_JSON_FILE_NAME   = "retail_products.json";
        private static readonly string FUEL_PRODUCTS_JSON_FILE_NAME     = "fuel_products.json";
        private static readonly string USERS_JSON_FILE_NAME             = "users.json";
        private static readonly string TRANSACTIONS_JSON_FILE_NAME      = "transactions.json";
        private static readonly string BARCODE_RETAIL_PRODUCTS_JSON_FILE_NAME = "barcode_retail_products.json";
        //...

        // Public constants ===
        public static readonly string RETAIL_PRODUCTS_JSON_FILE_PATH            = Path.Combine(BASE_DIRECTORY_PATH, PATH_TO_JSON_DB, JSON_DB_NAME, RETAIL_PRODUCTS_JSON_FILE_NAME);
        public static readonly string USERS_JSON_FILE_PATH                      = Path.Combine(BASE_DIRECTORY_PATH, PATH_TO_JSON_DB, JSON_DB_NAME, USERS_JSON_FILE_NAME);
        public static readonly string TRANSACTIONS_JSON_FILE_PATH               = Path.Combine(BASE_DIRECTORY_PATH, PATH_TO_JSON_DB, JSON_DB_NAME, TRANSACTIONS_JSON_FILE_NAME);
        public static readonly string BARCODE_RETAIL_PRODUCTS_JSON_FILE_PATH    = Path.Combine(BASE_DIRECTORY_PATH, PATH_TO_JSON_DB, JSON_DB_NAME, BARCODE_RETAIL_PRODUCTS_JSON_FILE_NAME);
    }
}
