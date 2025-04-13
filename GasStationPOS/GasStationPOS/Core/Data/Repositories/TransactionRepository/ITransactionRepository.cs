using System.Collections.Generic;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Database.Json.JsonFileSchemas;
using GasStationPOS.Core.Data.Models.TransactionModels;

namespace GasStationPOS.Core.Data.Repositories.TransactionRepository
{
    /// <summary>
    /// Interface to repository class that does crud operations on the data source.
    /// </summary>
    public interface ITransactionRepository
    {
        /// <summary>
        /// Creates a new transaction in the transactions data storage.
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        Task Create(Transaction transaction);

        /// <summary>
        /// Gets all transactions from the data source as dtos.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TransactionDatabaseDTO>> GetAll();

        /// <summary>
        /// Deletes all transactions stored in the json file.
        /// Throws an error if unsuccessful.
        /// </summary>
        void DeleteAll();

        //void Update(Transaction transaction);
        //Transaction Get(int transactionNumber); // for searching transaction info
    }
}
