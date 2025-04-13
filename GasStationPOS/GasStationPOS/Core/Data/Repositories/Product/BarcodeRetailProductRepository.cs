using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Database.Json.JsonFileSchemas;
using GasStationPOS.Core.Data.Database.Json.JsonToModelDTOs.Products;
using GasStationPOS.Core.Data.Models.ProductModels;
using GasStationPOS.Core.Database.Json;

namespace GasStationPOS.Core.Data.Repositories.Product
{
    /// <summary>
    /// Repository class that does crud operations on the data source.
    /// Data source is currently a json file.
    /// 
    /// Author: Mansib Talukder
    /// 
    /// </summary>
    public class BarcodeRetailProductRepository : IBarcodeRetailProductRepository
    {
        public BarcodeRetailProduct Get(string enteredBarcodeId)
        {
            BarcodeRetailProduct bcRetailProduct;

            string jsonData = File.ReadAllText(JsonFileConstants.BARCODE_RETAIL_PRODUCTS_JSON_FILE_PATH);

            using (JsonDocument document = JsonDocument.Parse(jsonData))
            {
                JsonElement root = document.RootElement;
                JsonElement bcRetailProductsElement = root.GetProperty("BarcodeRetailProducts");

                JsonElement matchingBCRPProductElement = bcRetailProductsElement.EnumerateArray().FirstOrDefault(bcRetailProductElement =>
                    bcRetailProductElement.GetProperty("BarcodeId").GetString() == enteredBarcodeId
                );

                // If barcode retail product was found:

                // JsonElement ValueKind property is set to Undefined
                // when the JsonElement does not contain any valid data
                // (an empty JsonElement is returned when no match is found in the FirstOrDefault query)
                if (matchingBCRPProductElement.ValueKind != JsonValueKind.Undefined) // if the json element returned is NOT empty and contains data
                {
                    string bcrpJsonData = matchingBCRPProductElement.GetRawText();

                    // For this to work, the Barcode Retail Product json schema has to match the BarcodeRetailProduct model class structure
                    bcRetailProduct = JsonSerializer.Deserialize<BarcodeRetailProduct>(bcrpJsonData);

                    return bcRetailProduct;
                }

                // If barcode retail product was not found:
                return null;
            }
        }

        /// <summary>
        /// Creates and adds a new barcode retail product to the JSON file.
        /// If the file exists, it loads the existing products, appends the new product,
        /// and saves the updated list back to the file.
        /// If the file does not exist, a new list is created with the new product and saved to the file.
        /// </summary>
        /// <param name="newProduct">The new barcode retail product to add.</param>
        public void Create(BarcodeRetailProduct newProduct)
        {
            // Initialize an empty product list wrapper
            BarcodeRetailProductsJSONStructure productList = new BarcodeRetailProductsJSONStructure();

            // Path to the JSON file where barcode retail products are stored
            string filePath = JsonFileConstants.BARCODE_RETAIL_PRODUCTS_JSON_FILE_PATH;

            // If file exists, load existing data
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    // Deserialize the JSON data into the product list wrapper
                    productList = JsonSerializer.Deserialize<BarcodeRetailProductsJSONStructure>(json)
                                 ?? new BarcodeRetailProductsJSONStructure();
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
