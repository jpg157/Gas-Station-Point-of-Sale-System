using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GasStationPOS.UI.UIDataChangeEvents;
using GasStationPOS.UI.UIDataEventArgs;
using GasStationPOS.UI.ViewDataTransferObjects.ProductDTOs;

namespace GasStationPOS.UI.Views
{
    public interface IMainView
    {
        // Properties for Presenter and Main view to inherit/implement
        //int SelectedRetailProductQuantity { get; set; } // currently selected retail product quantity
        string FuelPriceAmountInputValue { get; set; }
        decimal FuelPrice { get; set; }

        // Events for user actions
        event EventHandler<UpdateQuantityEventArgs>         UpdateSelectedProductQuantityEvent; // update the value of SelectedProductQuantity
        event EventHandler<AddRetailProductToCartEventArgs> AddNewRetailProductToCartEvent;
        event EventHandler<AddFuelProductToCartEventArgs>   AddNewFuelProductToCartEvent; // get the entered fuel grade and input amount
        event EventHandler<RemoveProductEventArgs>          RemoveProductFromCartEvent;
        event EventHandler                                  RemoveAllProductsFromCartEvent; // attach event in the presenter that removes all from list and update listCart - don't need any event args since all data sources already stored in presenter
        event EventHandler<SubmitPaymentEventArgs>          SubmitPaymentEvent; // attach an event in the presenter that sends all the payment to services for validation



        // Bind the data sources of the products (fields product name and reference to product itself) with the data controls (the buttons)
        void SetRetailProductButtonDataFromSource(IEnumerable<RetailProductDTO> retailProductsDataList); // Set once by the presenter

        void SetProductQuantityButtonDataFromSource(IEnumerable<int> productQuantityDataList); // Set once by the presenter

        // For data updating BOTH ways (presenter -> view, view -> presenter)
        // - data will be updated multiple times
        // - done via event handlers
        void SetUserCartListBindingSource(BindingSource userCartProductsBindingSource); // data on both UI and presenter layer passed both ways (two way data binding)

        // For data updating ONE way (presenter -> view)
        // - data will be updated multiple times
        // - done via event handlers

        /// <summary>
        /// Sets the binding sources of the Labels:
        /// - Subtotal
        /// - Amount tendered
        /// - Amount remaining
        /// </summary>
        /// <param name="paymentDataBindingSource"></param>
        void SetPaymentInfoLabelsBindingSource(BindingSource paymentDataBindingSource);

        //void SetSubtotalBindingSource(BindingSource subtotalBindingSource); // for subtotal UI control to be data binded with the stored subtotal in the presenter
        //void SetAmountTenderedBindingSource(BindingSource amountTenderedBindingSource); // for amount tendered UI control to be data binded with the stored amount tendered in the presenter (updates once the user has paid)
        //void SetAmountRemainingBindingSource(BindingSource amountRemainingBindingSource); // for amount remaining UI control to be data binded with the stored amount remaining in the presenter
    }
}
