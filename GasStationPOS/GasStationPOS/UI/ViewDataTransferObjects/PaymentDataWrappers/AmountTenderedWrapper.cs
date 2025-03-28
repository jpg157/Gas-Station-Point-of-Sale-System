using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationPOS.UI.ViewDataTransferObjects.PaymentDataWrappers
{
    class AmountTenderedWrapper
    {
        public decimal AmountTendered { get; set; }

        public AmountTenderedWrapper(decimal amountTendered)
        {
            AmountTendered = amountTendered;
        }
    }
}
