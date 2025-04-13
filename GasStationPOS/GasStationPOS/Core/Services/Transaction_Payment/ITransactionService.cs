using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Models.TransactionModels;
using GasStationPOS.UI.MainFormDataSchemas.DataBindingSourceWrappers;
using GasStationPOS.UI.MainFormDataSchemas.DTOs;

namespace GasStationPOS.Core.Services.Transaction_Payment
{
    /// <summary>
    /// Interface of service for transaction operations 
    /// (create new transaction, get all transactions,
    /// delete all transactions, get previous) - methods called from main form.
    /// Uses transation repository to access data storage.
    /// 
    /// Author: Jason Lau
    /// 
    /// </summary>
    public interface ITransactionService
    {
        /// <summary>
        /// Creates a new transaction and stores in the data storage.
        /// This method is asyncronous.
        /// </summary>
        /// <param name="paymentMethod"></param>
        /// <param name="totalAmountDollars"></param>
        /// <param name="amountTenderedDollars"></param>
        /// <param name="products"></param>
        /// <returns></returns>
        Task<bool> CreateTransactionAsync(PaymentMethod paymentMethod, decimal totalAmountDollars, decimal amountTenderedDollars, IEnumerable<ProductDTO> products);

        /// <summary>
        /// Deletes all transactions stored in the data storage. Returns true if successful, otherwise false.
        /// </summary>
        bool DeleteAllTransactions();

        /// <summary>
        /// Returns the latest transaction number.
        /// (Used in transaction review)
        /// </summary>
        int LatestTransactionNumber { get; }

        /// <summary>
        /// Returns a collection of all the product dtos in the previous transaction (within the bounds of the first and current latest transaction numbers)
        /// and the amount tendered.
        /// Returns an empty collection if there were no previous transactions, or transaction was not found.
        /// This method is asyncronous (to avoid blocking during data source load operations from transactionRepository get all method)
        /// </summary>
        /// <param name="transactionNumber"></param>
        Task<Tuple<IEnumerable<ProductDTO>, decimal>> GetTransactionProductListAsync(int currentlyChosenTransactionNum);

        /// <summary>
        /// Returns a valid chosen transaction number used when indexing through previous transactions (within
        /// </summary>
        int GetChosenTransactionNumberWithinBounds(int chosenTransactionNum);
    }
}
