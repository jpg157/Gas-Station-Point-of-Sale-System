using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.Core.Models.ProductModels;
using GasStationPOS.UI.ViewDataTransferObjects.ProductDTOs;
using GasStationPOS.Core.Repositories.Product;
using GasStationPOS;

namespace GasStationPOS.Core.Services.Inventory
{
    class InventoryService : IInventoryService
    {
        private IRetailProductRepository    retailProductRepository;
        //private IFuelProductRepository      fuelProductRepository; //TODO

        /// <summary>
        /// Constructor for InventoryService.
        /// Dependency injection of RetailProductRepository and FuelProductRepository which access the database for retail and fuel product data.
        /// </summary>
        /// <param name="retailProductRepository"></param>
        /// <param name="fuelProductRepository"></param>
        public InventoryService(IRetailProductRepository retailProductRepository//, 
            //IFuelProductRepository fuelProductRepository
            )
        {
            this.retailProductRepository    = retailProductRepository;
            //this.fuelProductRepository      = fuelProductRepository;
        }

        /// <summary>
        /// Gets all retail product data in the form of RetailProduct model data classes, 
        /// and converts them into the equivalent DTO.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RetailProductDTO> GetAllRetailProductData()
        {
            IEnumerable<RetailProduct>      retailProductModelDataList;
            List<RetailProductDTO>          retailProductDTODataList;

            retailProductModelDataList  = this.retailProductRepository.GetAll();
            retailProductDTODataList    = new List<RetailProductDTO>();

            foreach (RetailProduct rp in retailProductModelDataList)
            {
                // Map the RetailProduct model -> new instance of RetailProductDTO
                // (Equivalent to creating a new RetailProductDTO and copying all of the required fields from the model instance)
                RetailProductDTO rpDTO = Program.GlobalMapper.Map<RetailProductDTO>(rp);

                retailProductDTODataList.Add(rpDTO);
            }

            return retailProductDTODataList;
        }

        public IEnumerable<FuelProductDTO> GetAllFuelProductData()
        {
            //TODO - should be the same as GetAllRetailProductData but data is FuelProductDTO instead
            throw new NotImplementedException();
        }
    }
}
