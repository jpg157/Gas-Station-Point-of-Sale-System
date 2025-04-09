using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Models.ProductModels;
using GasStationPOS.UI.MainFormDataSchemas.DTOs;

namespace GasStationPOS.Core.Services.Inventory
{
    public interface IInventoryService
    {
        IEnumerable<RetailProductDTO> GetAllRetailProductData();
        BarcodeRetailProductDTO CheckAndReturnIfBarcodeRetailProductExits(string barcode);
    }
}
