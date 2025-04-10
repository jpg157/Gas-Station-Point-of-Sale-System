using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Models.ProductModels;
using GasStationPOS.UI.MainFormDataSchemas.DTOs;

namespace GasStationPOS.UI.MainFormDataSchemas.DataSourceWrappers
{
    /// <summary>
    /// A wrapper class for holding a list of barcode retail products.
    /// This class encapsulates a list of <see cref="BarcodeRetailProduct"/> objects, 
    /// making it easier to manage and manipulate a collection of barcode retail products.
    /// 
    /// Author: Mansib Talukder
    /// Date: 10 April 2025
    /// 
    /// </summary>
    public class BarcodeRetailProductListWrapper
    {
        /// <summary>
        /// Gets or sets the list of barcode retail products.
        /// Initializes as an empty list by default.
        /// </summary>
        public List<BarcodeRetailProduct> BarcodeRetailProducts { get; set; } = new List<BarcodeRetailProduct>();
    }

}
