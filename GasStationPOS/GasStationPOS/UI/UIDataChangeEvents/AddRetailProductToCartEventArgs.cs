using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.UI.ViewDataTransferObjects.ProductDTOs;

namespace GasStationPOS.UI.UIDataEventArgs
{
    public class AddRetailProductToCartEventArgs : EventArgs
    {
        public RetailProductDTO RetailProductDTO { get; }
        public AddRetailProductToCartEventArgs(RetailProductDTO retailProductDTO)
        {
            RetailProductDTO = retailProductDTO;
        }
    }
}
