using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.UI.ViewDataTransferObjects.ProductDTOs;

namespace GasStationPOS.UI.UIDataEventArgs
{
    public class RemoveProductEventArgs : EventArgs
    {
        public ProductDTO ProductDTO { get; }
        public RemoveProductEventArgs(ProductDTO productDTO)
        {
            ProductDTO = productDTO;
        }
    }
}
