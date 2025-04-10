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
        bool DeleteAllTransactions();

        // Used in transaction review
        int LatestTransactionNumber { get; }
        Task<Tuple<IEnumerable<ProductDTO>, decimal>> GetTransactionProductListAsync(int currentlyChosenTransactionNum);

        /// <summary>
        /// Returns a valid chosen transaction number used when indexing through previous transactions (within
        /// </summary>
        int GetChosenTransactionNumberWithinBounds(int chosenTransactionNum);
    }
}
