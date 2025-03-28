using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationPOS.UI.ViewDataTransferObjects.PaymentDataWrappers
{
    class AmountRemainingWrapper
    {
        public decimal AmountRemaining { get; set; }

        public AmountRemainingWrapper(decimal amountRemaining)
        {
            AmountRemaining = amountRemaining;
        }
    }
}
