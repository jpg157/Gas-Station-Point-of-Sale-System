using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Models.ProductModels;

namespace GasStationPOS.Core.Services.ProductCreation
{
    /// <summary>
    /// Interface for service class for handling the storage and management of products.
    /// Defines operations for creating and adding new barcode retail products to the data source.
    /// </summary>
    public interface IProductCreationService
    {
        /// <summary>
        /// Creates and adds a new <see cref="BarcodeRetailProduct"/> object and adds it in the data storage. 
        /// </summary>
        /// <param name="barcodeId"></param>
        /// <param name="productName"></param>
        /// <param name="unitPrice"></param>
        void CreateBarcodeRetailProduct(string barcodeId, string productName, decimal unitPrice);
    }
}
