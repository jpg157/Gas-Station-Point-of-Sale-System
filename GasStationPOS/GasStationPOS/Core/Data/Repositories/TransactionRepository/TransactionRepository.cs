using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Database.Json.JsonToModelDTOs;
using GasStationPOS.Core.Data.Models.TransactionModels;
using GasStationPOS.Core.Database.Json;

namespace GasStationPOS.Core.Data.Repositories.TransactionRepository
{
    public class TransactionRepository : ITransactionRepository
    {
        public async Task Create(Transaction transaction)
        {
            // create a TransactionDTO object from a Transaction object
            TransactionDatabaseDTO transactionDatabaseDTO = Program.GlobalMapper.Map<TransactionDatabaseDTO>(transaction);

            // Add the transactionDatabaseDTO object to the db (json file)
            // Need to:

            // 1. read and deserialize all of the json file into an object
            TransactionsJSONStructure transactionsJSONObjectWrapper;
            using (FileStream readStream = File.OpenRead(JsonDBConstants.TRANSACTIONS_JSON_FILE_PATH)) // open transactions.json file in read mode
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
            using (FileStream writeStream = File.Create(JsonDBConstants.TRANSACTIONS_JSON_FILE_PATH)) // File.Create mode overwrites the existing transactions.json file
            {
                await JsonSerializer.SerializeAsync(writeStream, transactionsJSONObjectWrapper, new JsonSerializerOptions{WriteIndented = true});
            }
        }
    }
}
