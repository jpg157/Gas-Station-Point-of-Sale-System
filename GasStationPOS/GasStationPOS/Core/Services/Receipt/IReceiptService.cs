using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationPOS.Core.Services.Receipt
{
    /// <summary>
    /// Interface for the ReceiptService class responsible for printing and generating receipts.
    /// </summary>
    public interface IReceiptService
    {
        /// <summary>
        /// Prints the receipt to the console and saves it to a file.
        /// Returns true if successful, otherwise returns false.
        /// </summary>
        bool PrintReceipt(int transactionNumber);
    }
}
