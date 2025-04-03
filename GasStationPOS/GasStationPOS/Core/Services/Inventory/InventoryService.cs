using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Repositories.Product;
using GasStationPOS.Core.Data.Models.ProductModels;
using GasStationPOS.UI.MainFormDataSchemas.DTOs;

namespace GasStationPOS.Core.Services.Inventory
{
    public class InventoryService : IInventoryService
    {
        private IRetailProductRepository retailProductRepository;

        /// <summary>
        /// Constructor for InventoryService.
        /// Dependency injection of RetailProductRepository which accesses the database for retail product data.
        /// </summary>
        /// <param name="retailProductRepository"></param>
        public InventoryService(IRetailProductRepository retailProductRepository)
        {
            this.retailProductRepository = retailProductRepository;
        }

        /// <summary>
        /// Gets all retail product data in the form of RetailProduct model data classes, 
        /// and converts them into the equivalent DTO.
        /// </summary>
        /// <returns></returns>
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
    }
}
