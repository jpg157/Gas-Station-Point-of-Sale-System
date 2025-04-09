using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Database.Json.JsonToModelDTOs;
using GasStationPOS.Core.Data.Models.ProductModels;
using GasStationPOS.Core.Database.Json;

namespace GasStationPOS.Core.Data.Repositories.Product
{
    public class BarcodeRetailProductRepository : IBarcodeRetailProductRepository
    {
        public BarcodeRetailProduct Get(string enteredBarcodeId)
        {
            BarcodeRetailProduct bcRetailProduct;

            string jsonData = File.ReadAllText(JsonDBConstants.BARCODE_RETAIL_PRODUCTS_JSON_FILE_PATH);

            using (JsonDocument document = JsonDocument.Parse(jsonData))
            {
                JsonElement root = document.RootElement;
                JsonElement bcRetailProductsElement = root.GetProperty("BarcodeRetailProducts");

                JsonElement matchingBCRPProductElement = bcRetailProductsElement.EnumerateArray().FirstOrDefault(bcRetailProductElement =>
                    bcRetailProductElement.GetProperty("BarcodeId").GetString() == enteredBarcodeId
                );

                // If user was found:

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
    }
}
