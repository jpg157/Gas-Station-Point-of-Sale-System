using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Database.Json.JsonFileSchemas;
using GasStationPOS.Core.Data.Models.TransactionModels;
using GasStationPOS.Core.Database.Json;

namespace GasStationPOS.Core.Data.Repositories.TransactionRepository
{
    /// <summary>
    /// Repository class that does crud operations on the data source.
    /// Data source is currently a json file.
    /// 
    /// Author: Jason Lau
    /// 
    /// </summary>
    public class TransactionRepository : ITransactionRepository
    {
        /// <summary>
        /// Creates a new transaction in the json file transactions list.
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public async Task Create(Transaction transaction)
        {
            // create a TransactionDTO object from a Transaction object
            TransactionDatabaseDTO transactionDatabaseDTO = Program.GlobalMapper.Map<TransactionDatabaseDTO>(transaction);

            // Add the transactionDatabaseDTO object to the db (json file)
            // Need to:

            // 1. read and deserialize all of the json file into an object
            TransactionsJSONStructure transactionsJSONObjectWrapper;
            using (FileStream readStream = File.OpenRead(JsonFileConstants.TRANSACTIONS_JSON_FILE_PATH)) // open transactions.json file in read mode
            {
                try
                {
                    transactionsJSONObjectWrapper = await JsonSerializer.DeserializeAsync<TransactionsJSONStructure>(readStream);

                    if (transactionsJSONObjectWrapper?.Transactions == null) // if the json file "Transactions" property contained no transactions
                    {
                        transactionsJSONObjectWrapper.Transactions = new List<TransactionDatabaseDTO>();
                    }
                }
                catch (JsonException) // if the json file was empty
                {
                    transactionsJSONObjectWrapper = new TransactionsJSONStructure();
                    transactionsJSONObjectWrapper.Transactions = new List<TransactionDatabaseDTO>();
                }
            }

            // 2. add the DTO to the wrapper object Transactions list
            transactionsJSONObjectWrapper.Transactions.Add(transactionDatabaseDTO);

            // 3. write and serialize the json object to the same json file (overwriting previous contents with the additional transation added)
            using (FileStream writeStream = File.Create(JsonFileConstants.TRANSACTIONS_JSON_FILE_PATH)) // File.Create mode overwrites the existing transactions.json file
            {
                await JsonSerializer.SerializeAsync(writeStream, transactionsJSONObjectWrapper, new JsonSerializerOptions{WriteIndented = true});
            }
        }

        /// <summary>
        /// Gets all transactions from the json file.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TransactionDatabaseDTO>> GetAll()
        {
            TransactionsJSONStructure transactionsJSONObjectWrapper;
            using (FileStream readStream = File.OpenRead(JsonFileConstants.TRANSACTIONS_JSON_FILE_PATH)) // open transactions.json file in read mode
            {
                try
                {
                    transactionsJSONObjectWrapper = await JsonSerializer.DeserializeAsync<TransactionsJSONStructure>(readStream);

                    if (transactionsJSONObjectWrapper?.Transactions == null) // if the json file "Transactions" property contained no transactions
                    {
                        transactionsJSONObjectWrapper.Transactions = new List<TransactionDatabaseDTO>();
                    }
                }
                catch (JsonException) // if the json file was empty
                {
                    transactionsJSONObjectWrapper = new TransactionsJSONStructure();
                    transactionsJSONObjectWrapper.Transactions = new List<TransactionDatabaseDTO>();
                }
            }
            // returned the list of transactions stored in the json structure
            return transactionsJSONObjectWrapper.Transactions;
        }

        /// <summary>
        /// Deletes all transactions stored in the json file.
        /// Throws an error if unsuccessful.
        /// </summary>
        public void DeleteAll()
        {
            // Clear the file by writing an empty string to it
            File.WriteAllText(JsonFileConstants.TRANSACTIONS_JSON_FILE_PATH, string.Empty);
        }
    }
}
