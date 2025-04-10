using System.Collections.Generic;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Database.Json.JsonToModelDTOs;
using GasStationPOS.Core.Data.Models.TransactionModels;

namespace GasStationPOS.Core.Data.Repositories.TransactionRepository
{
    public interface ITransactionRepository
    {
        Task Create(Transaction transaction);
        Task<IEnumerable<TransactionDatabaseDTO>> GetAll();
        void DeleteAll();
        //void Update(Transaction transaction);
        //void Delete(int transactionNumber);
        //Transaction Get(int transactionNumber); // for searching transaction info
    }
}
