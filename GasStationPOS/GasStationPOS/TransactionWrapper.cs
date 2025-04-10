using GasStationPOS.Core.Data.Models.TransactionModels;
using System.Collections.Generic;

namespace GasStationPOS
{
    /// <summary>
    /// A wrapper class for holding a list of transactions.
    /// This class encapsulates a collection of <see cref="Transaction"/> objects, making it easier to manage 
    /// and manipulate transaction data as a group.
    /// </summary>
    class TransactionWrapper
    {
        /// <summary>
        /// Gets or sets the list of transactions.
        /// </summary>
        public List<Transaction> Transactions { get; set; }
    }

}
