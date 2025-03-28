using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationPOS.UI.UIDataEventArgs
{
    public class UpdateQuantityEventArgs : EventArgs
    {
        public int SelectedQuantity { get; }
        public UpdateQuantityEventArgs(int selectedQuantity)
        {
            SelectedQuantity = selectedQuantity;
        }
    }
}
