using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Repositories.Product;
using GasStationPOS.Core.Data.Models.ProductModels;
using GasStationPOS.UI.MainFormDataSchemas.DTOs;
using GasStationPOS.Core.Data.Models.UserModels;

namespace GasStationPOS.Core.Services.Inventory
{

    /// <summary>
    /// Service class that provides inventory-related operations such as retrieving product data and checking product availability by barcode.
    /// 
    /// Author: Jason Lau
    /// 
    /// </summary>
    public class InventoryService : IInventoryService
    {
        private IRetailProductRepository        retailProductRepository;
        private IBarcodeRetailProductRepository barcodeRetailProductRepository;

        /// <summary>
        /// Constructor for InventoryService.
        /// Dependency injection of RetailProductRepository which accesses the database for retail product data.
        /// </summary>
        /// <param name="retailProductRepository"></param>
        public InventoryService(IRetailProductRepository retailProductRepository, IBarcodeRetailProductRepository barcodeRetailProductRepository)
        {
            this.retailProductRepository        = retailProductRepository;
            this.barcodeRetailProductRepository = barcodeRetailProductRepository;
        }

        /// <summary>
        /// Gets all retail product data in the form of RetailProduct model data classes, 
        /// and converts them into the equivalent DTO.
        /// </summary>
        public IEnumerable<RetailProductDTO> GetAllRetailProductData()
        {
            IEnumerable<RetailProduct> retailProductModelDataList;
            List<RetailProductDTO> retailProductDTODataList;

            retailProductModelDataList = this.retailProductRepository.GetAll();
            retailProductDTODataList = new List<RetailProductDTO>();

            foreach (RetailProduct rp in retailProductModelDataList)
            {
                // Map the RetailProduct model -> new instance of RetailProductDTO
                // (Equivalent to creating a new RetailProductDTO and copying all of the required fields from the model instance)
                RetailProductDTO rpDTO = Program.GlobalMapper.Map<RetailProductDTO>(rp);

                retailProductDTODataList.Add(rpDTO);
            }

            return retailProductDTODataList;
        }

        /// <summary>
        /// Checks if the barcode retail product with the entered barcodeId exists.
        /// Returns it if it exists, otherwise returns null.
        /// </summary>
        /// <param name="barcode"></param>
        public BarcodeRetailProductDTO CheckAndReturnIfBarcodeRetailProductExits(string barcode)
        {
            //bool barcodeRetailProductExistsInDb = false;

            BarcodeRetailProduct bcRetailProduct;

            bcRetailProduct = barcodeRetailProductRepository.Get(barcode);

            if (bcRetailProduct == null) return null;

            // Step 2: Map the BarcodeRetailProduct model to a DTO
            BarcodeRetailProductDTO barcodeRetailProductDTO = Program.GlobalMapper.Map<BarcodeRetailProductDTO>(bcRetailProduct);

            return barcodeRetailProductDTO;
        }
    }
}
