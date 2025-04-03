using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Models.TransactionModels;
using GasStationPOS.UI.MainFormDataSchemas.DataSourceWrappers;
using GasStationPOS.UI.MainFormDataSchemas.DTOs;

namespace GasStationPOS.Core.Services.Transaction_Payment
{
    public interface ITransactionService
    {
        Task<bool> CreateTransactionAsync(PaymentMethod paymentMethod, decimal totalAmountDollars, decimal amountTenderedDollars, IEnumerable<ProductDTO> products);
    }
}
