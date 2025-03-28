using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.UI.ViewDataTransferObjects.ProductDTOs;

namespace GasStationPOS.UI.UIDataChangeEvents
{
    public class AddFuelProductToCartEventArgs : EventArgs
    {
        public FuelProductDTO FuelProductDTO { get; }
        public AddFuelProductToCartEventArgs(FuelProductDTO fuelProductDTO)
        {
            FuelProductDTO = fuelProductDTO;
        }
    }
}
