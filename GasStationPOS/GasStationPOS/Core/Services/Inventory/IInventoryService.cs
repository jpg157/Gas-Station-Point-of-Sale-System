using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Models.ProductModels;
using GasStationPOS.UI.MainFormDataSchemas.DTOs;

namespace GasStationPOS.Core.Services.Inventory
{
    public interface IInventoryService
    {
        /// <summary>
        /// Gets all retail product data in the form of RetailProduct model data classes, 
        /// and converts them into the equivalent DTO.
        /// </summary>
        IEnumerable<RetailProductDTO> GetAllRetailProductData();

        /// <summary>
        /// Checks if the barcode retail product with the entered barcodeId exists.
        /// Returns it if it exists, otherwise returns null.
        /// </summary>
        /// <param name="barcode"></param>
        BarcodeRetailProductDTO CheckAndReturnIfBarcodeRetailProductExits(string barcode);
    }
}
