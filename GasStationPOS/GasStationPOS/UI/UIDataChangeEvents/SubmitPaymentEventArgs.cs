using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.Core.Models.TransactionModels;

namespace GasStationPOS.UI.UIDataChangeEvents
{
    public class SubmitPaymentEventArgs : EventArgs
    {
        public PaymentMethod PaymentMethod { get; }

        public SubmitPaymentEventArgs(PaymentMethod paymentMethod)
        {
            PaymentMethod = paymentMethod;
        }
    }
}
