using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Database.Json.JsonToModelDTOs;
//using GasStationPOS.Core.Data.Database.Json.JsonToModelDTOs.Transaction;
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

        private int transactionId = 1;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            this.transactionRepository = transactionRepository;
        }

        public async Task<bool> CreateTransactionAsync(PaymentMethod paymentMethod, decimal totalAmountDollars, decimal amountTenderedDollars, IEnumerable<ProductDTO> products)
        {
            

            // validate parameters
            if (!ValidateTransactionPaymentFields(totalAmountDollars, amountTenderedDollars, products)) {
                return false; // unsuccessful transaction
            }

            Random random = new Random(); // ====== TransactionNumber for now is random but will eventually replace with autoincrementing id ======

            Transaction transaction = new Transaction
            {
                TransactionNumber               = transactionId,
                TransactionFuelProductItems     = new List<TransactionFuelProductItem>(),
                TransactionRetailProductItems   = new List<TransactionRetailProductItem>(),
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

                    // Create transaction retail product item list entry
                    TransactionRetailProductItem rpTransactionEntry = new TransactionRetailProductItem
                    {
                        RetailProduct           = retailProduct,
                        Quantity                = rpDTO.Quantity,
                        TotalItemPriceDollars   = rpDTO.TotalPriceDollars
                    };
                        
                    // Add to TransactionRetailProductItems
                    transaction.TransactionRetailProductItems.Add(rpTransactionEntry);
                }
                else if (productDTO is FuelProductDTO)
                {
                    FuelProductDTO fpDTO = (FuelProductDTO)productDTO;

                    // Convert to the dto -> model via automapper
                    FuelProduct fuelProduct = Program.GlobalMapper.Map<FuelProduct>(fpDTO);

                    // Create transaction fuel product item list entry
                    TransactionFuelProductItem fpTransactionEntry = new TransactionFuelProductItem
                    {
                        FuelProduct             = fuelProduct,
                        Quantity                = fpDTO.Quantity,
                        TotalItemPriceDollars   = fpDTO.TotalPriceDollars
                    };

                    // Add to TransactionRetailProductItems
                    transaction.TransactionFuelProductItems.Add(fpTransactionEntry);
                }
            }

            // validate the created transaction - calling validate using datavalidation class
            //================================ Jason TODO ================================

            // create transaction asyncronously
            await this.transactionRepository.Create(transaction);

            transactionId++;
            return true; // successful transaction
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
