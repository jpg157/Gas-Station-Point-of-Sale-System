using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Models.ProductModels;
using GasStationPOS.UI.Constants;
using GasStationPOS.UI.MainFormDataSchemas.DataBindingSourceWrappers;
using GasStationPOS.UI.MainFormDataSchemas.DTOs;

namespace GasStationPOS.UI
{
    /// <summary>
    /// Main form data updater class is a utility class used by the 
    /// main form to update all of its UI related data. 
    /// Data binded UI is automatically updated when the data changes.
    /// 
    /// Author: Mansib Talukder
    /// Author: Jason Lau
    /// Author: Vincent Fung
    /// Date: 19 March 2025
    /// </summary>
    public static class MainFormDataUpdater
    {
        // Event handlers for updating Data (not UI) ===

        // All data that will be updated should be passed
        // from the MainForm as REFERENCES, not values

        /// <summary>
        /// Updates the currently selected retail product quantity to the value of newQuantity.
        /// </summary>
        /// <param name="currentSelectedQuantityRef"></param>
        /// <param name="newQuantity"></param>
        public static void UpdateSelectedProductQuantity(ref int currentSelectedQuantityRef, int newQuantity)
        {
            currentSelectedQuantityRef = newQuantity; // update the currently selected quantity to the value passed int argument
        }

        /// <summary>
        /// Updates the FuelPumpNumber in the fuelInputDataWrapper object.
        /// </summary>
        /// <param name="fuelInputDataWrapper"></param>
        /// <param name="selectedFuelPumpNumber"></param>
        public static void UpdateSelectedPumpNumber(FuelInputDataWrapper fuelInputDataWrapper, int selectedFuelPumpNumber)
        {
            fuelInputDataWrapper.FuelPumpNumber = selectedFuelPumpNumber;
        }

        /// <summary>
        /// Updates the EnteredFuelGrade in the fuelInputDataWrapper object.
        /// </summary>
        /// <param name="fuelInputDataWrapper"></param>
        /// <param name="selectedFuelGrade"></param>
        public static void UpdateSelectedFuelGrade(FuelInputDataWrapper fuelInputDataWrapper, FuelGrade selectedFuelGrade)
        {
            fuelInputDataWrapper.EnteredFuelGrade = selectedFuelGrade;
        }

        /// <summary>
        /// Updates the entered fuel price value variable stored in the UI.
        /// </summary>
        /// <param name="fuelInputDataWrapper"></param>
        /// <param name="newEnteredFuelPrice"></param>
        public static void UpdateEnteredFuelPrice(FuelInputDataWrapper fuelInputDataWrapper, decimal newEnteredFuelPrice)
        {
            fuelInputDataWrapper.EnteredFuelPrice = newEnteredFuelPrice;
        }

        /// <summary>
        /// Adds a retail product to the cart by making a copy of the existing selected retail product dto (stored "beneath the selected retail product button" 
        /// and adding to the list cart data source.
        /// Updates payment information fields accordingly.
        /// </summary>
        /// <param name="userCartProductsDataList"></param>
        /// <param name="rpDTO"></param>
        /// <param name="paymentDataWrapper"></param>
        /// <param name="currentSelectedQuantityRef"></param>
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
            currentSelectedQuantityRef = QuantityConstants.DEFAULT_RETAIL_PRODUCT_QUANTITY_VALUE;
        }

        /// <summary>
        /// Adds a fuel product to the list cart data source. Updates payment information fields accordingly.
        /// </summary>
        /// <param name="userCartProductsDataList"></param>
        /// <param name="fpDTO"></param>
        /// <param name="paymentDataWrapper"></param>
        /// <param name="fuelInputDataWrapper"></param>
        public static void AddNewFuelProductToCart(
            BindingList<ProductDTO> userCartProductsDataList,
            FuelProductDTO fpDTO,
            PaymentDataWrapper paymentDataWrapper,
            FuelInputDataWrapper fuelInputDataWrapper
        )
        {
            decimal priceChange;

            if (fpDTO == null) return;

            // create deep copy of the FuelProductDTO
            FuelProductDTO fpDTOCopy = Program.GlobalMapper.Map<FuelProductDTO>(fpDTO); // (fpDTO is the source of the UI FuelProductDTO data for each add - from userCartProductsDataList)

            userCartProductsDataList.Add(fpDTOCopy); // UI is automatically updated bc of the BindingSource attached

            // Addition increases the price (+)
            priceChange = fpDTOCopy.TotalPriceDollars;
            paymentDataWrapper.UpdatePaymentRelatedDataSources(priceChange);

            // reset fuel input vals
            fuelInputDataWrapper.ResetPaymentRelatedDataSourcesToInitValues();
        }

        /// <summary>
        /// Removes the selected product from the list cart data source, if it exists. UI updated via data binding.
        /// </summary>
        /// <param name="userCartProductsDataList"></param>
        /// <param name="productToRemove"></param>
        /// <param name="paymentDataWrapper"></param>
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

        /// <summary>
        /// Removes all products form the list cart data source. UI updated via data binding.
        /// </summary>
        /// <param name="userCartProductsDataList"></param>
        /// <param name="paymentDataWrapper"></param>
        /// <param name="currentSelectedProductQuantity"></param>
        public static void RemoveAllProductsFromCart(
            BindingList<ProductDTO> userCartProductsDataList, 
            PaymentDataWrapper paymentDataWrapper,
            ref int currentSelectedProductQuantity
        ) {
            // remove all elements from the user cart BindingList data source
            userCartProductsDataList.Clear(); // UI is automatically updated bc of the BindingSource attached

            // reset currentSelectedProductQuantity value to the default
            currentSelectedProductQuantity = QuantityConstants.DEFAULT_RETAIL_PRODUCT_QUANTITY_VALUE;

            // reset subtotal and amountRemaining
            paymentDataWrapper.ResetPaymentRelatedDataSourcesToInitValues(); // UI is automatically updated bc of the BindingSource attached
        }
    }
}
