using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using GasStationPOS.UI.MainFormDataSchemas.DataSourceWrappers;
using GasStationPOS.UI.MainFormDataSchemas.DTOs;
using GasStationPOS.Core.Database.Json;
using GasStationPOS.Core.Data.Models.ProductModels;

namespace GasStationPOS.Core.Services.Processor
{
    /// <summary>
    /// A static class for handling the storage and management of barcode retail products in a JSON file.
    /// Provides functionality to add new products to the file, while ensuring existing data is preserved.
    /// </summary>
    public static class BarcodeRetailProductFileProcessor
    {
        // Path to the JSON file where barcode retail products are stored
        private static string filePath = JsonDBConstants.BARCODE_RETAIL_PRODUCTS_JSON_FILE_PATH;

        /// <summary>
        /// Adds a new barcode retail product to the JSON file.
        /// If the file exists, it loads the existing products, appends the new product,
        /// and saves the updated list back to the file.
        /// If the file does not exist, a new list is created with the new product and saved to the file.
        /// </summary>
        /// <param name="newProduct">The new barcode retail product to add.</param>
        public static void AddNewProductToFile(BarcodeRetailProduct newProduct)
        {
            // Initialize an empty product list wrapper
            BarcodeRetailProductListWrapper productList = new BarcodeRetailProductListWrapper();

            // If file exists, load existing data
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    // Deserialize the JSON data into the product list wrapper
                    productList = JsonSerializer.Deserialize<BarcodeRetailProductListWrapper>(json)
                                 ?? new BarcodeRetailProductListWrapper();
                }
            }

            // Add the new product to the list
            productList.BarcodeRetailProducts.Add(newProduct);

            // Serialize the updated product list and save it back to the file
            string updatedJson = JsonSerializer.Serialize(productList, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, updatedJson);
        }
    }

}
