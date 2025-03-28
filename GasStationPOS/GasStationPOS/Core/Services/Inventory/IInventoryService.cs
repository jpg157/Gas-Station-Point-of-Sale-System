using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.Core.Models.ProductModels;
using GasStationPOS.UI.ViewDataTransferObjects.ProductDTOs;

namespace GasStationPOS.Core.Services.Inventory
{
    public interface IInventoryService
    {
        IEnumerable<RetailProductDTO> GetAllRetailProductData();

        IEnumerable<FuelProductDTO> GetAllFuelProductData();
    }
}
