using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Models.TransactionModels;
using GasStationPOS.UI.MainFormDataSchemas.DTOs;

namespace GasStationPOS.Core.Services.Transaction_Payment
{
    public class TransactionService : ITransactionService
    {
        public void CreateTransaction(PaymentMethod paymentMethod, IEnumerable<ProductDTO> product)
        {
            throw new NotImplementedException();
        }
    }
}

