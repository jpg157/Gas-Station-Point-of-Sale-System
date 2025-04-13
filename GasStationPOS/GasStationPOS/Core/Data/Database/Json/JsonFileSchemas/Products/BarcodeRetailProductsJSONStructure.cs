using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Models.ProductModels;

namespace GasStationPOS.Core.Data.Database.Json.JsonToModelDTOs.Products
{
    /// <summary>
    /// Barcode retail products JSON file structure used by system.text.json serialize and deserialize methods.
    /// Class structure should match the json file structure.
    /// 
    /// A wrapper class for holding a list of barcode retail products retreieved from the json file.
    /// This class encapsulates a list of <see cref="BarcodeRetailProduct"/> objects, 
    /// making it easier to manage and manipulate a collection of barcode retail products.
    /// 
    /// Author: Mansib Talukder
    /// Date: 10 April 2025
    /// 
    /// </summary>
    public class BarcodeRetailProductsJSONStructure
    {
        /// <summary>
        /// Gets or sets the list of barcode retail products.
        /// Initializes as an empty list by default.
        /// </summary>
        public List<BarcodeRetailProduct> BarcodeRetailProducts { get; set; } = new List<BarcodeRetailProduct>(); // The C# property needs to match the JSON key (case-insensitive by default)
    }

}
