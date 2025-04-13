using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using GasStationPOS.Core.Database.Json;
using GasStationPOS.Core.Data.Models.ProductModels;
using GasStationPOS.Core.Data.Database.Json.JsonToModelDTOs.Products;
using GasStationPOS.Core.Data.Repositories.Product;

namespace GasStationPOS.Core.Services.ProductCreation
{
    /// <summary>
    /// Service class for handling the storage and management of products.
    /// Provides functionality to add new (currently just barcode retail) products to the file, while ensuring existing data is preserved.
    /// </summary>
    public class ProductCreationService : IProductCreationService
    {

        readonly IBarcodeRetailProductRepository barcodeRetailProductRepository;

        /// <summary>
        /// ProductCreationService constructor
        /// </summary>
        /// <param name="barcodeRetailProductRepository"></param>
        public ProductCreationService(IBarcodeRetailProductRepository barcodeRetailProductRepository)
        {
            this.barcodeRetailProductRepository = barcodeRetailProductRepository;
        }

        /// <summary>
        /// Creates and adds a new <see cref="BarcodeRetailProduct"/> object and adds it in the data storage.
        /// </summary>
        /// <param name="newProduct">The new barcode retail product to add.</param>
        public void CreateBarcodeRetailProduct(string barcodeId, string productName, decimal unitPrice)
        {
            // Create new product object
            var newProduct = new BarcodeRetailProduct
            {
                Id = 0, // Id not used for now
                BarcodeId = barcodeId,
                ProductName = productName,
                UnitPriceDollars = unitPrice
            };

            barcodeRetailProductRepository.Create(newProduct);
        }
    }

}
