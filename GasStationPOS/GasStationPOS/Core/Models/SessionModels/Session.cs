using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.Core.Models.ProductModels;
using GasStationPOS.Core.Models.TransactionModels;

namespace GasStationPOS.Core.Models.SessionModels
{
    enum SessionStatus
    {
        OPEN,
        CLOSED,
        PAUSED
    }

    class Session
    {
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "SessionStartTime is required")]
        public DateTime SessionStartTime { get; set; }

        [Required(ErrorMessage = "Cashier Id is required")]
        public int CashierId { get; set; }

        [Required(ErrorMessage = "SessionEndTime is required")]
        public DateTime SessionEndTime { get; set; }

        [Required(ErrorMessage = "SessionStatus is required")]
        public SessionStatus SessionStatus { get; set; }

        [Required(ErrorMessage = "Session Transactions are required")]
        public List<Transaction> SessionTransactions { get; set; }

        // might change this later if we need more flexibility for category information
        [Required(ErrorMessage = "RetailCategoryTotalSales is required")]
        public Dictionary<RetailCategory, decimal> RetailCategoryTotalSales { get; set; } // <retailCategory, total sales ($) for that category>

        [Required(ErrorMessage = "TotalSalesAmount is required")]
        [Range(0.0, 10000.0, ErrorMessage = "TotalSalesAmount must be between 0.0 and 10000.0")]
        public decimal TotalSalesAmount { get; set; }

        [Required(ErrorMessage = "TotalCardSalesAmount is required")]
        [Range(0.0, 10000.0, ErrorMessage = "TotalCardSalesAmount must be between 0.0 and 10000.0")]
        public decimal TotalCardSalesAmount { get; set; }

        [Required(ErrorMessage = "TotalCashSalesAmount is required")]
        [Range(0.0, 10000.0, ErrorMessage = "TotalCashSalesAmount must be between 0.0 and 10000.0")]
        public decimal TotalCashSalesAmount { get; set; }

        [Required(ErrorMessage = "TotalChangeGiven is required")]
        [Range(0.0, 10000.0, ErrorMessage = "TotalChangeGiven must be between 0.0 and 10000.0")]
        public decimal TotalChangeGiven { get; set; }

        //public Boolean SessionIsClosed { get; set; } = false;
    }
}
