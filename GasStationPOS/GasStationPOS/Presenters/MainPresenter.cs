using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GasStationPOS.Core.Models.ProductModels;
using GasStationPOS.Core.Repositories.Product;
using GasStationPOS.Core.Services.Inventory;
using GasStationPOS.UI.Constants;
using GasStationPOS.UI.UIDataChangeEvents;
using GasStationPOS.UI.UIDataEventArgs;
using GasStationPOS.UI.ViewDataTransferObjects;
using GasStationPOS.UI.ViewDataTransferObjects.PaymentDataWrappers;
using GasStationPOS.UI.ViewDataTransferObjects.ProductDTOs;
using GasStationPOS.UI.Views;

namespace GasStationPOS.Presenters
{
    /// <summary>
    /// Presenter class handles:
    /// - Passing data to be processed/stored, and retrieving to display to the view (via services).
    /// - Frontend view - user updating and retrieve functionality (updating quantity, cart items, etc.)
    /// </summary>
    public class MainPresenter
    {
        private readonly IMainView          mainView; // main form reference
        private readonly IInventoryService  inventoryService; // for retrieving all retail and fuel product data to display to the UI

        #region Main view form UI binding sources
        // BINDING SOURCES ===

        // binding sources allow for UI control data (Text, Label, ListBox contents, etc.) to be AUTOMATICALLY updated when connected to a data source
        // - updates to the data source will be syncronized UI
        private readonly BindingSource userCartProductsBindingSource;
        
        private readonly BindingSource paymentDataBindingSource;
        #endregion

        #region Data sources for each BindingSource
        // DATA SOURCES ===

        // data sources are the underlying data stored in each corresponding BindingSource object

        // USER CART DATA SOURCE (CONTENTS WILL CHANGE)
        private readonly BindingList<ProductDTO> userCartProductsDataList; // data changes when user adds/removes items from the cart

            // RETAIL PRODUCT BUTTONS DATA SOURCE (CONTENTS DO NOT CHANGE)
        private readonly IEnumerable<RetailProductDTO> retailProductsDataList; // for storing each of the retail products, each product is "connected" to a UI button (each product retreived from json file/database by presenter and db service)

            // UPDATE QUANTITY BUTTONS DATA SOURCE (CONTENTS DO NOT CHANGE)
        private readonly IEnumerable<int> retailProductQuantitiesList;

        // Payment related Data sources
        private readonly PaymentDataWrapper paymentDataWrapper;
        #endregion

        // For saving the currently selected product quantity value that user presses
        private decimal currentSelectedProductQuantity; // note: assign this to the ProductDTO Quantity field when the next product is added, and for calculating total price of product

        /// <summary>
        /// Main presenter class is responsible for getting data to/from the view, updating the view when user action occurs, 
        /// and passing user entered data along to services.
        /// The MainView and the required Services are passed into MainPresenter constructor (dependency injection).
        /// </summary>
        /// <param name="mainView"></param>
        /// <param name="retailProductRepository"></param>
        public MainPresenter(IMainView mainView, IInventoryService inventoryService)
        {
            this.mainView                   = mainView;
            this.inventoryService           = inventoryService;

            #region Initilize Data sources
            // LOAD RETAIL PRODUCT DATA FROM INVENTORY SERVICE (which retreives the data from DB)
            // Load the retailProductDataList view data (RetailProductDTO names to be used for button label display)
            this.retailProductsDataList = inventoryService.GetAllRetailProductData();

            // INITIALIZE PRODUCT QUANTITIES
            this.retailProductQuantitiesList = new List<int> { 1, 2, 3, 4, 5, 10, 25, 50, 100, 999 };

            // INITIALIZE LIST CART DATA (INITIALLY EMPTY) 
            // Initialize userCartProductsList to an empty list of ProductDTOs,
            // then use it to initialize userCartProductDTOList data binding source
            this.userCartProductsDataList    = new BindingList<ProductDTO>();

            this.paymentDataWrapper     = new PaymentDataWrapper(); // contains subtotal, amountRemaining, and amountTendered
            #endregion

            // Initial value of the current selected product quantity
            currentSelectedProductQuantity = QuantityConstants.DEFAULT_QUANTITY_VALUE;

            #region Initilize Binding sources using the data sources

            // Create the binding sources that the view will use
            this.userCartProductsBindingSource      = new BindingSource();
            this.paymentDataBindingSource           = new BindingSource();
            
            // set UI data binding source to the corresponding data stored in this presenter
            this.userCartProductsBindingSource.DataSource   = this.userCartProductsDataList;
            this.paymentDataBindingSource.DataSource        = this.paymentDataWrapper;
            #endregion

            #region Attach Binding sources to the mainview datasources
            // Set the retailProducts, userCartProducts, and retailProductQuantities data to the view UI components
            // (IMainView interface defined methods, implemented in the main form class)
            this.mainView.SetRetailProductButtonDataFromSource(retailProductsDataList);
            this.mainView.SetProductQuantityButtonDataFromSource(retailProductQuantitiesList);

            this.mainView.SetUserCartListBindingSource(userCartProductsBindingSource);

            // set the UI labels for subtotal, amountTendered, and amountRemaining in the main form to the binding source data
            this.mainView.SetPaymentInfoLabelsBindingSource(paymentDataBindingSource);
            #endregion

            // Subscribe event handler methods to the user action view events
            AddEventHandlersToEachUserActionEvent();
        }


        // Attach all event handlers to events in main form ===

        /// <summary>
        /// Attaches the Event handlers to the events in the main form.
        /// </summary>
        private void AddEventHandlersToEachUserActionEvent()
        {
            this.mainView.UpdateSelectedProductQuantityEvent    += UpdateSelectedProductQuantity;
            this.mainView.AddNewRetailProductToCartEvent        += AddNewRetailProductToCart;
            this.mainView.AddNewFuelProductToCartEvent          += AddNewFuelProductToCart;
            this.mainView.RemoveProductFromCartEvent            += RemoveProductFromCart;
            this.mainView.RemoveAllProductsFromCartEvent        += RemoveAllProductsFromCartEvent;
            this.mainView.SubmitPaymentEvent                    += SubmitPaymentEvent;
        }


        #region Event handlers that are attached to the view

        // Event handlers ===

        // All data used to change and data here are custom event argument classes that override EventArgs.
        // These classes each contain specific property (ex. SelectedQuantity passed from the main form)
        // (Can think of it like a request object in web dev, where the request stores data)
        // https://learn.microsoft.com/en-us/dotnet/standard/events/

        private void UpdateSelectedProductQuantity(object sender, UpdateQuantityEventArgs e)
        {
            this.currentSelectedProductQuantity = e.SelectedQuantity; // update the currently selected quantity to the value passed into the event argument
        }

        private void AddNewRetailProductToCart(object sender, AddRetailProductToCartEventArgs e)
        {
            decimal priceChange;

            RetailProductDTO rpDTO      = e.RetailProductDTO; // get the retail product dto - passed through via custom EventArgs subclass

            if (rpDTO == null) return;

            // create deep copy of the RetailProduct
            RetailProductDTO rpDTOCopy  = Program.GlobalMapper.Map<RetailProductDTO>(rpDTO); // (rpDTO is the source of the UI RetailProductDTO data for each add - from userCartProductsDataList)

            // Calculate the total price of the retail product based on current quantity selected
            // Update the product quantity to display in UI
            rpDTOCopy.TotalPriceDollars = currentSelectedProductQuantity * rpDTOCopy.PriceDollars;
            rpDTOCopy.Quantity = currentSelectedProductQuantity;

            this.userCartProductsDataList.Add(rpDTOCopy); // UI is automatically updated bc of the BindingSource attached

            // Addition increases the price (+)
            priceChange = rpDTOCopy.TotalPriceDollars;
            paymentDataWrapper.UpdatePaymentRelatedDataSources(priceChange);

            // Reset selectedQuantity to the default value
            this.currentSelectedProductQuantity = QuantityConstants.DEFAULT_QUANTITY_VALUE;

            //test (remove after)
            Console.WriteLine(userCartProductsDataList.Count);
        }

        private void AddNewFuelProductToCart(object sender, AddFuelProductToCartEventArgs e)
        {
            //TODO

            //decimal priceChange;

            // FuelGradeUtils // use this for getting fuel prices and FuelGrade enum from label
            throw new NotImplementedException();

            //priceChange = fpDTOCopy.TotalPriceDollars;
            //UpdatePaymentRelatedDataSources(priceChange);
        }

        private void RemoveProductFromCart(object sender, RemoveProductEventArgs e)
        {
            decimal priceChange;

            // this productDTO is either a RetailProductDTO or a FuelProductDTO
            // (using Liskov Substitution Principle)
            ProductDTO productDTO = e.ProductDTO;

            if (productDTO != null && userCartProductsDataList.Contains(productDTO))
            {
                this.userCartProductsDataList.Remove(productDTO); // UI is automatically updated bc of the BindingSource attached
            }

            // Removal reduces the price (-)
            priceChange = -(productDTO.TotalPriceDollars);
            paymentDataWrapper.UpdatePaymentRelatedDataSources(priceChange);

            //test (remove after)
            Console.WriteLine(userCartProductsDataList.Count);
        }

        private void RemoveAllProductsFromCartEvent(object sender, EventArgs e)
        {
            // remove all elements from the user cart BindingList data source
            this.userCartProductsDataList.Clear(); // UI is automatically updated bc of the BindingSource attached

            // reset currentSelectedProductQuantity value to the default
            this.currentSelectedProductQuantity = QuantityConstants.DEFAULT_QUANTITY_VALUE;

            // reset subtotal and amountRemaining
            paymentDataWrapper.ResetPaymentRelatedDataSourcesToInitValues(); // UI is automatically updated bc of the BindingSource attached

            //test (remove after)
            Console.WriteLine(userCartProductsDataList.Count);
        }

        private void SubmitPaymentEvent(object sender, EventArgs e)
        {
            //TODO: once UI is done for CASH and CARD payments - subscribe SubmitPaymentEvent EventHandler delegate

            //SubmitPaymentEvent() -> call the service

            //  this.subtotal += PAID AMOUNT FROM USER;
            //  this.amountTenderedFormatted = $"${amountTendered:F2}";

            throw new NotImplementedException();
        }
        #endregion


        // Util methods ===
    }
}


