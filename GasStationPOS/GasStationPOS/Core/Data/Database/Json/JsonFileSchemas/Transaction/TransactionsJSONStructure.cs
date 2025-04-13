using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationPOS.Core.Data.Database.Json.JsonFileSchemas
{
    public class TransactionsJSONStructure
    {
        public List<TransactionDatabaseDTO> Transactions { get; set; } // The C# property needs to match the JSON key (case-insensitive by default)
    }
}
