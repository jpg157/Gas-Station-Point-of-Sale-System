using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Models.ProductModels;

namespace GasStationPOS.Core.Data.Repositories.Product
{
    public interface IBarcodeRetailProductRepository
    {
        /// <summary>
        /// Gets the barcode retail product based on the enterd barcode id. 
        /// If a product with that barcode is not found, returns null.
        /// </summary>
        /// <param name="enteredBarcodeId"></param>
        /// <returns></returns>
        BarcodeRetailProduct Get(string enteredBarcodeId);
    }
}
