using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Models.ProductModels;

namespace GasStationPOS.Core.Data.Repositories.Product
{
    /// <summary>
    /// Interface to repository class that does crud operations on the data source.
    /// </summar
    public interface IBarcodeRetailProductRepository
    {
        /// <summary>
        /// Gets the barcode retail product based on the enterd barcode id. 
        /// If a product with that barcode is not found, returns null.
        /// </summary>
        /// <param name="enteredBarcodeId"></param>
        /// <returns></returns>
        BarcodeRetailProduct Get(string enteredBarcodeId);

        /// <summary>
        /// Creates and adds a new barcode retail product to the JSON file.
        /// If the file exists, it loads the existing products, appends the new product,
        /// and saves the updated list back to the file.
        /// If the file does not exist, a new list is created with the new product and saved to the file.
        /// </summary>
        /// <param name="newProduct">The new barcode retail product to add.</param>
        void Create(BarcodeRetailProduct newProduct);
    }
}
