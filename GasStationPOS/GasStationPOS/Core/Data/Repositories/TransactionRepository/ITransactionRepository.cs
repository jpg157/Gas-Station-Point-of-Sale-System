using GasStationPOS.Core.Data.Models.TransactionModels;

namespace GasStationPOS.Core.Data.Repositories.TransactionRepository
{
    public interface ITransactionRepository
    {
        void Create(Transaction transaction);
        //void Update(Transaction transaction);
        //void Delete(int transactionNumber);
        //Transaction Get(int transactionNumber); // for searching transaction info
        //IEnumerable<Transaction> GetAll();
    }
}
