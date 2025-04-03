using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.UI.Constants;
using GasStationPOS.UI.MainFormDataSchemas.DataSourceWrappers;
using GasStationPOS.UI.MainFormDataSchemas.DTOs;

namespace GasStationPOS.UI
{
    public static class MainFormDataUpdater
    {
        #region Event handlers that are attached to the view

        // Event handlers for updating Data (not UI) ===

        // All data that will be updated should be passed
        // from the MainForm as REFERENCES, not values

        public static void UpdateSelectedProductQuantity(ref int currrentSelectedQuantityRef, int newQuantity)
        {
            currrentSelectedQuantityRef = newQuantity; // update the currently selected quantity to the value passed int argument
        }

        public static void AddNewRetailProductToCart(
            BindingList<ProductDTO> userCartProductsDataList, 
            RetailProductDTO        rpDTO,
            PaymentDataWrapper      paymentDataWrapper,
            ref int                 currentSelectedQuantityRef
        ) {
            decimal priceChange;

            if (rpDTO == null) return;

            // create deep copy of the RetailProduct
            RetailProductDTO rpDTOCopy = Program.GlobalMapper.Map<RetailProductDTO>(rpDTO); // (rpDTO is the source of the UI RetailProductDTO data for each add - from userCartProductsDataList)

            // Calculate the total price of the retail product based on current quantity selected
            // Update the product quantity to display in UI
            rpDTOCopy.TotalPriceDollars = currentSelectedQuantityRef * rpDTOCopy.UnitPriceDollars;
            rpDTOCopy.Quantity = currentSelectedQuantityRef;

            userCartProductsDataList.Add(rpDTOCopy); // UI is automatically updated bc of the BindingSource attached

            // Addition increases the price (+)
            priceChange = rpDTOCopy.TotalPriceDollars;
            paymentDataWrapper.UpdatePaymentRelatedDataSources(priceChange);

            // Reset selectedQuantity to the default value
            currentSelectedQuantityRef = QuantityConstants.DEFAULT_QUANTITY_VALUE;
        }

        //public static void AddNewFuelProductToCart(object sender, AddFuelProductToCartEventArgs e)
        //{
        //    //TODO

        //    //decimal priceChange;

        //    // FuelGradeUtils // use this for getting fuel prices and FuelGrade enum from label
        //    throw new NotImplementedException();

        //    //priceChange = fpDTOCopy.TotalPriceDollars;
        //    //UpdatePaymentRelatedDataSources(priceChange);
        //}

        public static void RemoveProductFromCart(
            BindingList<ProductDTO> userCartProductsDataList,
            ProductDTO productToRemove, // this productDTO is either a RetailProductDTO or a FuelProductDTO (using LSPrinciple)
            PaymentDataWrapper paymentDataWrapper
        ) {
            decimal priceChange;

            if (productToRemove != null && userCartProductsDataList.Contains(productToRemove))
            {
                userCartProductsDataList.Remove(productToRemove); // UI is automatically updated bc of the BindingSource attached
            }

            // Removal reduces the price (-)
            priceChange = -(productToRemove.TotalPriceDollars);
            paymentDataWrapper.UpdatePaymentRelatedDataSources(priceChange);
        }

        public static void RemoveAllProductsFromCart(
            BindingList<ProductDTO> userCartProductsDataList, 
            PaymentDataWrapper paymentDataWrapper,
            ref int currentSelectedProductQuantity
        ) {
            // remove all elements from the user cart BindingList data source
            userCartProductsDataList.Clear(); // UI is automatically updated bc of the BindingSource attached

            // reset currentSelectedProductQuantity value to the default
            currentSelectedProductQuantity = QuantityConstants.DEFAULT_QUANTITY_VALUE;

            // reset subtotal and amountRemaining
            paymentDataWrapper.ResetPaymentRelatedDataSourcesToInitValues(); // UI is automatically updated bc of the BindingSource attached
        }
        #endregion
    }
}
