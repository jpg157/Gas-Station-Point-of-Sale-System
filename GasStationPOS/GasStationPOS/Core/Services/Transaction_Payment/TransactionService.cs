using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Models.ProductModels;
using GasStationPOS.Core.Data.Models.TransactionModels;
using GasStationPOS.Core.Data.Models.UserModels;
using GasStationPOS.Core.Data.Repositories.TransactionRepository;
using GasStationPOS.UI.MainFormDataSchemas.DTOs;

namespace GasStationPOS.Core.Services.Transaction_Payment
{
    public class TransactionService : ITransactionService
    {
        readonly ITransactionRepository transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            this.transactionRepository = transactionRepository;
        }

        public void CreateTransaction(PaymentMethod paymentMethod, decimal totalAmountDollars, decimal amountTenderedDollars, IEnumerable<ProductDTO> products)
        {
            Console.WriteLine("In TransactionService.CreateTransaction()");

            // validation of parameters
            if (!ValidateTransactionPaymentFields(totalAmountDollars, amountTenderedDollars, products)) {

                Console.WriteLine("ERROR: COULD NOT COMPLETE TRANSACTION");
                return;
            }

            Random random = new Random(); // ====== TransactionNumber for now is random but will eventually replace with autoincrementing id ======

            Transaction transaction = new Transaction
            {
                TransactionNumber               = random.Next(10000, 99999), // ====== TransactionNumber for now is random but will eventually replace with autoincrementing id ======
                TransactionFuelProductItems     = new List<Tuple<FuelProduct, decimal>>(),
                TransactionRetailProductItems   = new List<Tuple<RetailProduct, decimal>>(),
                PaymentMethod                   = paymentMethod,
                TotalAmountDollars              = totalAmountDollars,
                ChangeDollars                   = amountTenderedDollars - totalAmountDollars,
                TransactionDateTime             = DateTime.Now
            };

            // Go through the list of products passed in, and add the correct type of product to the list based on the Product subclass
            foreach (ProductDTO productDTO in products)
            {
                if (productDTO is RetailProductDTO)
                {
                    RetailProductDTO rpDTO = (RetailProductDTO)productDTO;

                    // Convert dto -> model via automapper
                    RetailProduct retailProduct = Program.GlobalMapper.Map<RetailProduct>(rpDTO);

                    Tuple<RetailProduct, decimal> rpTransactionTupleEntry = Tuple.Create(retailProduct, productDTO.Quantity);

                    // Add to TransactionRetailProductItems
                    transaction.TransactionRetailProductItems.Add(rpTransactionTupleEntry);
                }
                else if (productDTO is FuelProductDTO)
                {
                    FuelProductDTO fpDTO = (FuelProductDTO)productDTO;

                    // Convert to the dto -> model via automapper
                    FuelProduct fuelProduct = Program.GlobalMapper.Map<FuelProduct>(fpDTO);

                    Tuple<FuelProduct, decimal> fpTransactionTupleEntry = Tuple.Create(fuelProduct, productDTO.Quantity);

                    // Add to TransactionRetailProductItems
                    transaction.TransactionFuelProductItems.Add(fpTransactionTupleEntry);
                }
            }

            // validate the created transaction - calling validate using datavalidation class
            //================================ Jason TODO ================================

            // create transaction
            this.transactionRepository.Create(transaction);

        }

        private static bool ValidateTransactionPaymentFields(decimal totalAmountDollars, decimal amountTenderedDollars, IEnumerable<ProductDTO> products)
        {
            if (products == null) return false;

            decimal totalAmountDollarsProductsTest = 0.0m;

            foreach (ProductDTO product in products)
            {
                if (product == null) return false;
                totalAmountDollarsProductsTest += product.TotalPriceDollars;
            }

            if (totalAmountDollarsProductsTest != totalAmountDollars) return false;

            if (amountTenderedDollars < totalAmountDollars) return false;

            return true;
        }
    }
}
