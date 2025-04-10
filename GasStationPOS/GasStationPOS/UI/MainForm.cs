using GasStationPOS.Core.Data.Models.ProductModels;
using GasStationPOS.Core.Data.Models.TransactionModels;
using GasStationPOS.Core.Database.Json;
using GasStationPOS.Core.Services;
using GasStationPOS.Core.Services.Auth;
using GasStationPOS.Core.Services.Inventory;
using GasStationPOS.Core.Services.Transaction_Payment;
using GasStationPOS.UI;
using GasStationPOS.UI.Constants;
using GasStationPOS.UI.MainFormDataSchemas.DataSourceWrappers;
using GasStationPOS.UI.MainFormDataSchemas.DTOs;
using GasStationPOS.UI.UIFormValidation;
using GasStationPOS.UI.UserControls.Payment;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GasStationPOS
{
    /// <summary>
    /// POSMode enum is used to track the current mode/state of the system.
    /// Examples: Review of previous transactions mode, transaction mode.
    /// </summary>
    public enum POSMode
    {
        PREVIOUS_TRANSACTION_REVIEW,
        TRANSACTION
    }

    /// <summary>
    /// MainForm class for managing the Gas Station Point of Sale (POS) system. 
    /// It handles user interactions with the interface, such as adding products to the cart, 
    /// updating subtotal and remaining balance, and processing transactions. The form includes 
    /// functionality for displaying current date and time, selecting quantities, and managing the cart. 
    /// It also manages fuel transactions, including selecting fuel types and updating fuel price amounts.
    /// 
    /// Author: Mansib Talukder
    /// Author: Jason Lau
    /// Author: Vincent Fung
    /// Date: 19 March 2025
    ///
    public partial class MainForm : Form
    {
        // ======================== SERVICES ========================
        private readonly IInventoryService inventoryService; // for retrieving all retail and fuel product data to display to the UI
        private readonly ITransactionService transactionService;
        private readonly IAuthenticationService authenticationService;

        // ======================== BINDING SOURCES ========================

        // binding sources allow for UI control data (Text, Label, ListBox contents, etc.) to be AUTOMATICALLY updated when connected to a data source
        // - updates to the data source will be syncronized UI

        // USER CART data binding source (CONTENTS WILL CHANGE)
        private readonly BindingSource userCartProductsBindingSource;

        // FUEL INPUT data binding source (CONTENTS WILL CHANGE)
        private readonly BindingSource fuelInputDataBindingSource;

        // SUBTOTAL, AMOUNT TENDERED, AMOUNT REMAINING data binding source (WILL CHANGE)
        private readonly BindingSource paymentDataBindingSource;

        // ======================== CONSTANT DATA USED BY APPLICATION - USED TO SET BUTTON DATA AND LABELS ========================

        // RETAIL PRODUCT BUTTONS UNDERLYING DATA (CONTENTS DO NOT CHANGE)
        private readonly IEnumerable<RetailProductDTO> retailProductsDataList;

        private readonly IEnumerable<int> fuelProductPumpNumberList;

        // UPDATE QUANTITY BUTTONS UNDERLYING DATA (CONTENTS DO NOT CHANGE)
        private readonly IEnumerable<int> retailProductQuantitiesList;

        // ======================== STATE RELATED DATA FOR APPLICATION ========================

        // USER CART DATA SOURCE - contents will change
        private readonly BindingList<ProductDTO> userCartProductsDataList; // data changes when user adds/removes items from the cart

        // Stores user input fuel product and pump related information - contents will change
        private readonly FuelInputDataWrapper fuelInputDataWrapper;

        // Payment related Data sources - contents will change
        // Stores variables to track the subtotal, the amount remaining and the amount tendered by the customer
        private readonly PaymentDataWrapper paymentDataWrapper;

        // Currently selected product quantity
        private int currentSelectedProductQuantity;

        // Flag to manage the activation state of fuel pumps
        private bool fuelPumpsActivated;

        // Variable to store the input for fuel amount and selected fuel price
        private string fuelAmountInput;

        private List<Button> haltedPumps = new List<Button>();
        private bool isHaltMode = false;
        private bool allPumpsHalted = false;  // Flag to track the halted state of all pumps

        // Mode of the point of service system (previous transaction review, transaction)
        private POSMode posMode;

        // Currently selected transaction for previous transaction review
        private int currentlyChosenTransactionNum;

        /// <summary>
        /// Constructor to initialize the MainForm
        /// </summary>
        public MainForm(IInventoryService inventoryService,
                        ITransactionService transactionService,
                        IAuthenticationService authenticationService) // dependency injection of services
        {
            InitializeComponent();

            // === Initilize required services ===
            this.inventoryService = inventoryService;
            this.transactionService = transactionService;
            this.authenticationService = authenticationService;

            // === Initialize Main View STATE Data ===
            this.posMode = POSMode.TRANSACTION;
            this.currentlyChosenTransactionNum = TransactionConstants.INITIAL_TRANSACTION_NUM;

            // === Initilize Main View UI Underlying Data ===

            currentSelectedProductQuantity = QuantityConstants.DEFAULT_RETAIL_PRODUCT_QUANTITY_VALUE; // Initial value of the current selected product quantity

            fuelPumpsActivated = false;

            // RETAIL PRODUCT BUTTON DATA
            this.retailProductsDataList = inventoryService.GetAllRetailProductData(); // call inventory service which retreives the data from DB)

            // FUEL PUMP NUMBER BUTTON DATA
            this.fuelProductPumpNumberList = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };

            // PRODUCT QUANTITY BUTTON DATA
            this.retailProductQuantitiesList = new List<int> { 1, 2, 3, 4, 5, 10, 25, 50, 100, 999 };

            // LIST CART DATA (Initially Empty - set by user input) 
            this.userCartProductsDataList = new BindingList<ProductDTO>();

            // FUEL PRODUCT INPUT DATA (Set by user input)
            this.fuelInputDataWrapper = new FuelInputDataWrapper(); // contains selected pump number, entered fuel price amount

            // PAYMENT DATA
            this.paymentDataWrapper = new PaymentDataWrapper(); // contains subtotal, amountRemaining, and amountTendered

            // === Binding sources that view ui controls will bind to ===

            this.userCartProductsBindingSource = new BindingSource();
            this.fuelInputDataBindingSource = new BindingSource();
            this.paymentDataBindingSource = new BindingSource();

            this.userCartProductsBindingSource.DataSource = this.userCartProductsDataList; // set UI data binding source to the corresponding data stored in the object
            this.fuelInputDataBindingSource.DataSource = this.fuelInputDataWrapper;
            this.paymentDataBindingSource.DataSource = this.paymentDataWrapper;

            // === Update button labels and tag attributes using data sources ===

            this.SetRetailProductButtonDataFromSource(retailProductsDataList);
            this.SetFuelPumpButtonDataFromSource(fuelProductPumpNumberList);
            this.SetProductQuantityButtonDataFromSource(retailProductQuantitiesList);

            this.SetFuelGradeButtonData();

            // === Bind UI controls with the Binding sources ===

            this.SetUserCartListBindingSource(userCartProductsBindingSource); // set ui for the user cart (the ListBox)
            this.SetFuelProductInfoLabelsBindingSource(fuelInputDataBindingSource); // set the UI labels for selected pump number, fuel grade, and fuel price in the main form to the binding source data attribute
            this.SetPaymentInfoLabelsBindingSource(paymentDataBindingSource); // set the UI labels for subtotal, amountTendered, and amountRemaining in the main form to the binding source data attribute

            // === ADD EVENT HANDLERS TO EVENTS IN MAIN FORM ===
            AssociateMainFormEvents();

            // === ADD EVENT HANDLERS TO EVENTS IN LOGIN FORM ===
            AssociateLoginFormEvents();

            Application.ApplicationExit += Application_ApplicationExit;
        }

        #region Button Label and Tag Setters

        /// <summary>
        /// Retail product UI Buttons: 
        /// 
        /// The label of each button is set to a value based on the corresponding retail product that it corresponds to
        /// Retail Product data is IEnumerable<RetailProductDTO> (data converted from json file / database in the presenter)
        /// 
        /// Method is called in the presenter - not in this main form class.
        /// </summary>
        private void SetRetailProductButtonDataFromSource(IEnumerable<RetailProductDTO> retailProductsDataList)
        {
            foreach (RetailProductDTO retailProductDTO in retailProductsDataList)
            {
                // get the corresponding button name based on the retail product (ex. productId of 1 -> "btnRp1")
                string retailBtnName = $"{ButtonNamePrefixes.RETAIL_BUTTON_PREFIX}{retailProductDTO.Id}";

                // find the button with the retail button name (ex. find button with name btnRp1
                Button retailProductButton = (Button)this.Controls.Find(retailBtnName, true).First();

                // 1. Set the text of the button to the name attribute in the RetailProductDTO object
                retailProductButton.Text = retailProductDTO.ProductNameDescription;

                // 2. Attach the retailProductDTO object to the button using the Tag attribute (to know which retail product that the clicked button was associated with)
                retailProductButton.Tag = retailProductDTO;

                // ====== EVENT RELATED ======

                // ( More efficient to put here instead of in AssociateAndRaiseViewEvents() )

                // 3. Associate MainFormDataUpdater.AddNewRetailProductToCart function to the click event of each retail product button
                RetailProductDTO rpDtoReference = ((RetailProductDTO)retailProductButton.Tag);

                retailProductButton.Click += delegate {
                    MainFormDataUpdater.AddNewRetailProductToCart(
                        this.userCartProductsDataList,
                        rpDtoReference,
                        this.paymentDataWrapper,
                        ref currentSelectedProductQuantity
                    );
                };

                // Add UI updating method to event handler (UpdatePayButtonVisibility will execute after the item was added)
                retailProductButton.Click += delegate { UpdatePayButtonVisibility(); }; // Note: if the first event handler throws an exception, it will prevent subsequent handlers from executing, unless the exception is caught and handled.
            }
        }

        private void SetFuelPumpButtonDataFromSource(IEnumerable<int> fuelPumpNumberList)
        {
            foreach (int fuelPumpNumber in fuelPumpNumberList)
            {
                string fuelPumpButtonName = $"{ButtonNamePrefixes.FUEL_PUMP_BUTTON_PREFIX}{fuelPumpNumber}";

                Button fuelPumpButton = (Button)this.Controls.Find(fuelPumpButtonName, true).First();

                // 1. Set the text of the button to the fuel pump number
                fuelPumpButton.Text = $"{fuelPumpNumber}";

                // 2. Attack the index of the fuel pump number value to the button using the Tag attribute (to know which fuel pump number that the clicked button was associated with)
                fuelPumpButton.Tag = fuelPumpNumber; // save the value to change the quantity to

                // ====== EVENT RELATED ======

                // ( More efficient to put here instead of in AssociateAndRaiseViewEvents() )

                // 3. Associate MainFormDataUpdater.UpdateSelectedPumpNumber function to the click event of each pump number update button
                fuelPumpButton.Click += btnFuelPump_Click;
            }
        }

        /// <summary>
        /// Set the fuel grade buttons text, underlying data, and event handler for a click event.
        /// </summary>
        private void SetFuelGradeButtonData()
        {
            btnFuelRegular.Text = FuelGrade.REGULAR.ToString();
            btnFuelRegular.Tag = FuelGrade.REGULAR;
            btnFuelRegular.Click += delegate { HandleFuelGradeBtnClick(FuelGrade.REGULAR); };

            btnFuelPlus.Text = FuelGrade.PLUS.ToString();
            btnFuelPlus.Tag = FuelGrade.PLUS;
            btnFuelPlus.Click += delegate { HandleFuelGradeBtnClick(FuelGrade.PLUS); };

            btnFuelSupreme.Text = FuelGrade.SUPREME.ToString();
            btnFuelSupreme.Tag = FuelGrade.SUPREME;
            btnFuelSupreme.Click += delegate { HandleFuelGradeBtnClick(FuelGrade.SUPREME); };
        }

        /// <summary>
        /// Update quantity UI Buttons:
        /// 
        /// The label of each button is set to a value based on the corresponding quantity that it corresponds to
        /// Quantity data is IEnumerable<int> (data converted from json file / database in the presenter)
        /// 
        /// Method is called in the presenter - not in this main form class.
        /// </summary>
        private void SetProductQuantityButtonDataFromSource(IEnumerable<int> retailProductQuantityDataList)
        {
            foreach (int productQuantityNum in retailProductQuantityDataList)
            {
                string quantityButtonName = $"{ButtonNamePrefixes.QUANTITY_BUTTON_PREFIX}{productQuantityNum}";

                Button updateQuantityButton = (Button)this.Controls.Find(quantityButtonName, true).First();

                // 1. Set the text of the button to the quantity number
                updateQuantityButton.Text = $"{productQuantityNum}x";

                // 2. Attack the index of the quantity number value to the button using the Tag attribute (to know which quantity that the clicked button was associated with)
                updateQuantityButton.Tag = productQuantityNum; // save the value to change the quantity to

                // ====== EVENT RELATED ======

                // ( More efficient to put here instead of in AssociateAndRaiseViewEvents() )

                // 3. Associate MainFormDataUpdater.UpdateSelectedProductQuantity function to the click event of each product quantity update button
                updateQuantityButton.Click += delegate {
                    MainFormDataUpdater.UpdateSelectedProductQuantity(
                        ref currentSelectedProductQuantity,
                        (int)updateQuantityButton.Tag
                    );
                };
            }
        }

        #endregion

        #region Binding Source Setters

        /// <summary>
        /// Bind the data source (a collection of ProductDTOs) with the ListCart UI:
        /// 
        /// Gets and sets data from List<ProductDTO> stored in presenter
        /// Bind the product ListBox list data with a list of ProductDTOs (selectedRetailProductsList) stored in the presenter
        /// 
        /// Two way data binding between the listCart and the userCartProductsBindingSource BindingSource data.
        /// 
        /// Method is called in the presenter - not in this main form class.
        /// </summary>
        private void SetUserCartListBindingSource(BindingSource userCartProductsBindingSource)
        {
            listCart.DataSource = userCartProductsBindingSource; // see if this works, otherwise use a DataGridView class
        }

        /// <summary>
        /// Sets the binding sources of the Labels:
        /// labelPumpNum   <-> fuelProductInputDataBindingSource - FuelPumpNumber (decimal) 
        /// labelFuelType   <-> fuelProductInputDataBindingSource - EnteredFuelGrade (decimal)
        /// labelFuelPrice  <-> fuelProductInputDataBindingSource - EnteredFuelPrice (decimal)
        /// </summary>
        /// <param name="fuelProductInputDataBindingSource"></param>
        private void SetFuelProductInfoLabelsBindingSource(BindingSource fuelProductInputDataBindingSource)
        {
            bool formattingEnabled = true; // boolean to determine if the created Binding object will automatically format the payment decimal value
            decimal defaultValueWhenDataSourceNull = PaymentConstants.INITIAL_AMOUNT_DOLLARS; // default value in case the data source "EnteredFuelGrade" is null
            string twoDecimalPlacesString = "C2"; // tells Binding object to format the data source to 2 decimal places (proper currency format)

            CultureInfo currencyCulture = new CultureInfo("en-CA"); // en-CA for Canadian Dollars

            // Need DataSourceUpdateMode.OnPropertyChanged so it knows to change the value in the the UI control when the datasource (ex. EnteredFuelPrice) changes value
            this.labelPumpNum.DataBindings.Add(new Binding("Text", fuelProductInputDataBindingSource, "FuelPumpNumberStr", formattingEnabled, DataSourceUpdateMode.OnPropertyChanged));
            this.labelFuelType.DataBindings.Add(new Binding("Text", fuelProductInputDataBindingSource, "EnteredFuelGrade", formattingEnabled, DataSourceUpdateMode.OnPropertyChanged));
            this.labelFuelPrice.DataBindings.Add(new Binding("Text", fuelProductInputDataBindingSource, "EnteredFuelPrice", formattingEnabled, DataSourceUpdateMode.OnPropertyChanged, defaultValueWhenDataSourceNull, twoDecimalPlacesString, currencyCulture));
        }

        /// <summary>
        /// Sets the binding sources of the Labels:
        /// labelSubtotal   <-> paymentDataBindingSource - Subtotal (decimal)
        /// labelTendered   <-> paymentDataBindingSource - AmountTendered (decimal)
        /// labelRemaining  <-> paymentDataBindingSource - AmountRemaining (decimal)
        /// </summary>
        /// <param name="paymentDataBindingSource"></param>
        private void SetPaymentInfoLabelsBindingSource(BindingSource paymentDataBindingSource)
        {
            bool formattingEnabled = true; // boolean to determine if the created Binding object will automatically format the payment decimal value
            decimal defaultValueWhenDataSourceNull = PaymentConstants.INITIAL_AMOUNT_DOLLARS; // default value in case the data source is null
            string twoDecimalPlacesString = "C2"; // tells Binding object to format the data source to 2 decimal places (proper currency format)

            CultureInfo currencyCulture = new CultureInfo("en-CA"); // en-CA for Canadian Dollars

            // Need DataSourceUpdateMode.OnPropertyChanged so it knows to change the value in the the UI control when the datasource (ex. Subtotal) changes value
            this.labelSubtotal.DataBindings.Add(new Binding("Text", paymentDataBindingSource, "Subtotal", formattingEnabled, DataSourceUpdateMode.OnPropertyChanged, defaultValueWhenDataSourceNull, twoDecimalPlacesString, currencyCulture));
            this.labelTendered.DataBindings.Add(new Binding("Text", paymentDataBindingSource, "AmountTendered", formattingEnabled, DataSourceUpdateMode.OnPropertyChanged, defaultValueWhenDataSourceNull, twoDecimalPlacesString, currencyCulture));
            this.labelRemaining.DataBindings.Add(new Binding("Text", paymentDataBindingSource, "AmountRemaining", formattingEnabled, DataSourceUpdateMode.OnPropertyChanged, defaultValueWhenDataSourceNull, twoDecimalPlacesString, currencyCulture));
        }

        #endregion

        #region Attach Eventhandlers to events of single standalone Buttons in the main form UI

        /// <summary>
        /// Attaches EventHandler objects to buttons, that do not have data binding properties with data stored in the presenter (retail, fuel and quantity buttons do)
        /// </summary>
        private void AssociateMainFormEvents()
        {
            // === Clear 1 item from cart Button ===

            btnRemoveItem.Click += btnRemoveItem_Click_v2;

            // === Clear ALL products from cart Button ===

            btnClear.Click += delegate { // using delegate operator allows for anonymous methods that return an EventHandler object ex: delegate {<can put any code to handle event in here>}
                MainFormDataUpdater.RemoveAllProductsFromCart(userCartProductsDataList, paymentDataWrapper, ref currentSelectedProductQuantity);
            };
            btnClear.Click += delegate { UpdatePayButtonVisibility(); };

            // === Payment Button ===

            // Call transactionService.CreateTransaction function when either btnPayCard or btnPayCash buttons are clicked (passing in the respective payment method)
            // (Need to assign the corresponding PaymentMethod enum member in paramater so transactionService can can handle the correct payment type
            btnPayCard.Click += PayCardButton_Click;
            this.cardPaymentUserControl.CardEnterButtonClick += async delegate { await ConfirmPaymentButton_Click(PaymentMethod.CARD); }; // subscribe ConfirmPaymentButton_Click function to the CardEnterButtonClick EventHandler

            btnPayCash.Click += PayCashButton_Click;
            this.cashPaymentUserControl.CashEnterButtonClick += async delegate { await ConfirmPaymentButton_Click(PaymentMethod.CASH); }; // subscribe ConfirmPaymentButton_Click function to the CashEnterButtonClick EventHandler

            // === Review Buttons ===

            btnReview.Click         += ShowReviewAndCurrentlySelectedTransaction;
            btnReviewForward.Click  += ShowNextTransaction;
            btnReviewBackward.Click += ShowPreviousTransaction;
        }

        private void AssociateLoginFormEvents()
        {
            // === Login Button ===
            buttonLogin.Click += buttonLogin_Click;
        }

        #endregion

        /// <summary>
        /// Event triggered when the form is loaded
        /// </summary>
        private void MainForm_Load(object sender, EventArgs e)
        {
            timerDateTime.Start(); // Start the timer on form load
        }

        /// <summary>
        /// Event triggered to update the date and time display
        /// </summary>
        private void timerDateTime_Tick(object sender, EventArgs e)
        {
            // Update the label with the current date and time
            lblDateTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// Helper Method to toggle the visibility of pay buttons
        /// </summary>
        private void UpdatePayButtonVisibility()
        {
            if (listCart.Items.Count == 0)
            {
                btnPayCard.Visible = false;
                btnPayCash.Visible = false;
                btnPayFuel.Visible = true;
            }
            else
            {
                btnPayCard.Visible = true;
                btnPayCash.Visible = true;
                btnPayFuel.Visible = false;
            }
        }

        /// <summary>
        /// Helper method to hide all modified panels
        /// </summary>
        private void HidePanels()
        {
            pnlProducts.Visible = false;
            pnlBottomNavMain.Visible = false;
            pnlBottomNavBack.Visible = false;
            pnlAddFuelAmount.Visible = false;
            pnlFuelConfirmation.Visible = false;
            pnlFuelTypeSelect.Visible = false;
            pnlSelectCartItem.Visible = false;
            pnlHaltConfirmation.Visible = false;
            pnlHaltAllConfirmation.Visible = false;
            pnlReview.Visible = false;
            this.cashPaymentUserControl.Visible = false;
            this.cardPaymentUserControl.Visible = false;
        }

        /// <summary>
        /// Event handler for selecting a cart item
        /// </summary>
        private void listCart_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listCart.SelectedIndex != -1 && listCart.SelectedItem != null)
            {
                string selectedItem = listCart.SelectedItem.ToString();
                string itemName = selectedItem.Split('\t')[0].Trim();  // Extract item name

                // Update the label in pnlSelectCartItem with the selected item name
                labelSelectedItem.Text = itemName;

                // Show pnlSelectCartItem, pnlBottomNavBack
                HidePanels();
                pnlSelectCartItem.Visible = true;
                pnlBottomNavBack.Visible = true;
            }
        }

        /// <summary>
        /// Helper method to reset the UI to its initial state
        /// </summary>
        private void reset()
        {
            // Show pnlProducts, pnlBottomNavMain
            HidePanels();

            // Unhighlight fuel pumps
            UnhighlightFuelPumps();
            fuelPumpsActivated = false;

            pnlProducts.Visible = true;
            pnlBottomNavMain.Visible = true;

            // Reset the fuel pump number, selected fuel grade, price amount, and quantity
            this.fuelInputDataWrapper.ResetPaymentRelatedDataSourcesToInitValues();
            fuelAmountInput = "";

            // Enable Halt Buttons
            btnHaltPump.Enabled = true;
            btnHaltAllPumps.Enabled = true;

            // Enable item selection
            listCart.SelectionMode = SelectionMode.One;
        }

        /// <summary>
        /// Event handler for removing an item from the cart
        /// </summary>
        private void btnRemoveItem_Click_v2(object sender, EventArgs e)
        {
            if (listCart.SelectedIndex != -1 && listCart.SelectedItem != null)
            {
                // Get the selected item from the list                                  
                var selectedProduct = listCart.SelectedItem as ProductDTO;

                if (selectedProduct != null)
                {
                    // Remove the product from the cart
                    MainFormDataUpdater.RemoveProductFromCart(userCartProductsDataList, selectedProduct, paymentDataWrapper); // (remove from the userCartProductsDataList list - will update the listbox UI automatically)

                    // Show pnlProducts, pnlBottomNavMain
                    HidePanels();
                    pnlProducts.Visible = true;
                    pnlBottomNavMain.Visible = true;

                    // if the cart is empty, hide payment buttons
                    UpdatePayButtonVisibility();
                }
            }
        }

        /// <summary>
        /// Event handler for the back button click
        /// </summary>
        private void btnBack_Click(object sender, EventArgs e)
        {
            reset();

            if (posMode == POSMode.PREVIOUS_TRANSACTION_REVIEW)
            {
                // clear the user list cart data source of previous transaction items
                MainFormDataUpdater.RemoveAllProductsFromCart(
                    userCartProductsDataList,
                    paymentDataWrapper,
                    ref currentSelectedProductQuantity
                );

                // set system back to transaction mode
                posMode = POSMode.TRANSACTION;
            }
        }

        /// <summary>
        /// Highlights the fuel pump buttons 
        /// by changing their border color to gold and border size to 3.
        /// </summary>
        private void HighlightFuelPumps()
        {
            UpdateFuelPumpsAppearance(Color.Gold, 3);
        }

        /// <summary>
        /// Resets the appearance of all fuel pump buttons 
        /// by changing their border color to black and border size to 1.
        /// </summary>
        private void UnhighlightFuelPumps()
        {
            UpdateFuelPumpsAppearance(Color.Black, 1);
        }

        /// <summary>
        /// Handles the "Halt Pump" button click
        /// </summary>
        private void btnHaltPump_Click(object sender, EventArgs e)
        {
            // Prevent halt actions when haltAll is in progress
            if (allPumpsHalted)
            {
                // Ensure that individual pump halt cannot happen when haltAll is active
                return;
            }

            else
            {
                // Show pnlHaltConfirmation, pnlBottomNavBack
                HidePanels();
                pnlHaltConfirmation.Visible = true;
                pnlBottomNavBack.Visible = true;

                isHaltMode = true;
                fuelPumpsActivated = true;
            }
        }

        /// <summary>
        /// Updates the appearance of all fuel pump buttons to the entered borderColor and borderSize.
        /// If borderColor < 1, borderColor will be set to 1.
        /// </summary>
        /// <param name="borderColor"></param>
        /// <param name="borderSize"></param>
        private void UpdateFuelPumpsAppearance(Color borderColor, int borderSize)
        {
            int numFuelPumps = this.fuelProductPumpNumberList.Count();

            if (borderSize < 1) borderSize = 1;

            for (int i = 1; i <= numFuelPumps; i++)
            {
                Button btn = Controls.Find($"{ButtonNamePrefixes.FUEL_PUMP_BUTTON_PREFIX}{i}", true).FirstOrDefault() as Button;

                // Skip updating the appearance for halted pumps
                if (haltedPumps.Contains(btn))
                {
                    continue;
                }

                if (btn != null)
                {
                    btn.FlatAppearance.BorderColor = borderColor;
                    btn.FlatAppearance.BorderSize = borderSize;
                }
            }
        }

        /// <summary>
        /// Handles the "Pay Fuel" button click event, hides other panels, shows the fuel confirmation panel,
        /// and highlights the fuel pumps for selection.
        /// </summary>
        private void btnPayFuel_Click(object sender, EventArgs e)
        {
            // Show pnlFuelConfirmation, pnlBottomNavBack
            HidePanels();
            pnlFuelConfirmation.Visible = true;
            pnlBottomNavBack.Visible = true;

            // Highlight fuel pump buttons (set border color to yellow and border size to 3)
            HighlightFuelPumps();
            fuelPumpsActivated = true;

            // Disable Halt functionality
            btnHaltAllPumps.Enabled = false;
            btnHaltPump.Enabled = false;
        }

        /// <summary>
        /// Handles the fuel pump selection by updating the pump number label 
        /// and navigating to the fuel type selection panel. 
        /// Highlights the selected fuel pump and unhighlights the others.
        /// </summary>
        private void btnFuelPump_Click(object sender, EventArgs e)
        {
            if (!fuelPumpsActivated) return;

            Button fuelPumpButton = sender as Button;
            if (fuelPumpButton == null) return;

            if (isHaltMode)
            {
                HandleHaltMode(fuelPumpButton);
            }
            else
            {
                HandlePumpSelection(fuelPumpButton);
            }
        }

        /// <summary>
        /// Handles the logic when a fuel pump is in halt mode.
        /// </summary>
        private void HandleHaltMode(Button fuelPumpButton)
        {
            // Prevent handling halt mode when haltAll is active
            if (allPumpsHalted) return;

            if (haltedPumps.Contains(fuelPumpButton))
            {
                UnhaltPump(fuelPumpButton);
            }
            else
            {
                HaltPump(fuelPumpButton);
            }

            isHaltMode = false;
            reset();
        }

        /// <summary>
        /// Halts the selected fuel pump by changing its label, font, background color, and text color.
        /// The button's original state (label, font, background color, and text color) is saved 
        /// so that it can be restored when unhalting the pump.
        /// </summary>
        /// <param name="fuelPumpButton">The button representing the fuel pump that is being halted.</param>
        private void HaltPump(Button fuelPumpButton)
        {
            // Store the original label, properties, and pump number
            fuelPumpButton.Tag = new ButtonState(fuelPumpButton.Text, fuelPumpButton.Font, fuelPumpButton.BackColor, fuelPumpButton.ForeColor, (int)fuelPumpButton.Tag);

            // Change the button's label, alignment, font, and colors
            fuelPumpButton.Text = "H";
            fuelPumpButton.Font = new Font(fuelPumpButton.Font.FontFamily, 44, FontStyle.Bold);
            fuelPumpButton.BackColor = Color.FromArgb(232, 32, 32);
            fuelPumpButton.ForeColor = Color.White;

            haltedPumps.Add(fuelPumpButton);
        }

        /// <summary>
        /// Restores the original appearance of a previously halted fuel pump.
        /// The button's label, font, background color, and text color are restored to their original state.
        /// </summary>
        /// <param name="fuelPumpButton">The button representing the fuel pump that is being unhalted.</param>
        private void UnhaltPump(Button fuelPumpButton)
        {
            // Retrieve the original button state from the Tag
            var originalState = (ButtonState)fuelPumpButton.Tag;

            // Restore the original label, alignment, font, colors, and pump number
            fuelPumpButton.Text = originalState.Label;
            fuelPumpButton.Font = originalState.Font;
            fuelPumpButton.BackColor = originalState.BackColor;
            fuelPumpButton.ForeColor = originalState.ForeColor;
            fuelPumpButton.Tag = originalState.PumpNumber;  // Restore the pump number

            haltedPumps.Remove(fuelPumpButton);
        }


        /// <summary>
        /// A helper class to store the original state of a button (label, font, alignment, colors).
        /// </summary>
        private class ButtonState
        {
            public string Label { get; }
            public Font Font { get; }
            public Color BackColor { get; }
            public Color ForeColor { get; }
            public int PumpNumber { get; }

            public ButtonState(string label, Font font, Color backColor, Color foreColor, int pumpNumber)
            {
                Label = label;
                Font = font;
                BackColor = backColor;
                ForeColor = foreColor;
                PumpNumber = pumpNumber;
            }
        }

        /// <summary>
        /// Shows the confirmation panel for halting all pumps.
        /// </summary>
        private void btnHaltAllPumps_Click(object sender, EventArgs e)
        {
            HidePanels();
            pnlHaltAllConfirmation.Visible = true;
            pnlBottomNavBack.Visible = true;
            isHaltMode = false;
        }

        /// <summary>
        /// Halts or unhalts all fuel pumps depending on their current state.
        /// </summary>
        private void haltAll()
        {
            int numFuelPumps = this.fuelProductPumpNumberList.Count();

            // If all pumps are currently halted, unhalt them
            if (allPumpsHalted)
            {
                for (int i = 1; i <= numFuelPumps; i++)
                {
                    Button btn = Controls.Find($"{ButtonNamePrefixes.FUEL_PUMP_BUTTON_PREFIX}{i}", true).FirstOrDefault() as Button;

                    if (btn != null && haltedPumps.Contains(btn))
                    {
                        UnhaltPump(btn);  // Unhalt each pump
                        haltedPumps.Remove(btn);
                    }
                }
            }
            else
            {
                // If not all pumps are halted, halt them
                for (int i = 1; i <= numFuelPumps; i++)
                {
                    Button btn = Controls.Find($"{ButtonNamePrefixes.FUEL_PUMP_BUTTON_PREFIX}{i}", true).FirstOrDefault() as Button;

                    if (btn != null && !haltedPumps.Contains(btn))
                    {
                        haltedPumps.Add(btn);  // Mark the pump as halted
                        HaltPump(btn);  // Halt the pump
                    }
                }
            }

            // Toggle the halted state flag
            allPumpsHalted = !allPumpsHalted;
            reset();
        }

        /// <summary>
        /// Clicking the button once halts all pumps, and clicking it again restores them.
        /// </summary>
        private void btnHaltAllPumpsYes_Click(object sender, EventArgs e)
        {
            haltAll();
        }


        /// <summary>
        /// Handles the logic when a fuel pump is selected in normal mode.
        /// </summary>
        private void HandlePumpSelection(Button fuelPumpButton)
        {
            if (haltedPumps.Contains(fuelPumpButton)) return;

            // Update selected pump number
            MainFormDataUpdater.UpdateSelectedPumpNumber(this.fuelInputDataWrapper, (int)fuelPumpButton.Tag);

            // Show fuel type selection panel
            ShowFuelTypeSelectionPanel();

            // Highlight the selected fuel pump
            HighlightSelectedPump(fuelPumpButton);
        }

        /// <summary>
        /// Displays the fuel type selection panel.
        /// </summary>
        private void ShowFuelTypeSelectionPanel()
        {
            HidePanels();
            pnlFuelTypeSelect.Visible = true;
            pnlBottomNavBack.Visible = true;
        }

        /// <summary>
        /// Highlights the selected fuel pump.
        /// </summary>
        private void HighlightSelectedPump(Button fuelPumpButton)
        {
            UnhighlightFuelPumps();
            fuelPumpButton.FlatAppearance.BorderColor = Color.Gold;
            fuelPumpButton.FlatAppearance.BorderSize = 3;
        }


        /// <summary>
        /// Handles the fuel type selection by updating the label with the selected fuel type 
        /// and showing the fuel amount input panel.
        /// </summary>
        private void HandleFuelGradeBtnClick(FuelGrade fuelGrade)
        {
            // Update FuelInputDataWrapper Fuel Type attribute (MainFormDataUpdater handles data updating logic)
            MainFormDataUpdater.UpdateSelectedFuelGrade(this.fuelInputDataWrapper, fuelGrade);

            // Show the add fuel amount panel
            HidePanels();
            pnlAddFuelAmount.Visible = true;
            pnlBottomNavBack.Visible = true;
        }

        /// <summary>
        /// Handles the numeric input for the fuel price calculator by appending or replacing input values.
        /// Updates the fuel price label accordingly.
        /// </summary>
        private void btnFuelCalculator_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                string value = btn.Text;

                // If input is "0", "00", "000", append normally
                if (value == "0" || value == "00" || value == "000")
                {
                    fuelAmountInput += value;
                }

                // If input is a preset amount (e.g., "10.00"), replace entire input
                else if (value.Contains("."))
                {
                    fuelAmountInput = value.Replace(".", ""); // Store without decimal
                }

                // Otherwise, append to the input
                else
                {
                    fuelAmountInput += value;
                }

                UpdateFuelPriceLabel();
            }
        }

        /// <summary>
        /// Updates the fuel price label and underlying fuel price data source after converting the input fuel amount to a decimal format.
        /// </summary>
        private void UpdateFuelPriceLabel()
        {
            decimal enteredFuelPrice;

            if (string.IsNullOrEmpty(fuelAmountInput))
            {
                enteredFuelPrice = 0.00m; // Default if empty
            }
            else
            {
                // Convert input to decimal format (X.YY)
                enteredFuelPrice = decimal.Parse(fuelAmountInput) / 100.0m;
            }

            MainFormDataUpdater.UpdateEnteredFuelPrice(this.fuelInputDataWrapper, enteredFuelPrice);
        }

        /// <summary>
        /// Handles the backspace input for the fuel price calculator, removing the last character from the fuel input.
        /// Updates the fuel price label accordingly.
        /// </summary>
        private void btnFuelCalculatorBackspace_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(fuelAmountInput))
            {
                fuelAmountInput = fuelAmountInput.Substring(0, fuelAmountInput.Length - 1);
                UpdateFuelPriceLabel();
            }
        }

        /// <summary>
        /// Validates the entered fuel amount, calculates the total price, and adds the fuel to the cart if valid.
        /// If invalid, shows an error message.
        /// </summary>
        private void btnFuelCalculatorEnter_Click(object sender, EventArgs e)
        {
            if (this.fuelInputDataWrapper.EnteredFuelPrice <= 0.0m)
            {
                MessageBox.Show("Please enter a valid fuel amount.");
                return;
            }

            // Get the data attributes from the fuelInputDataWrapper data source class
            int fuelPumpNumber = this.fuelInputDataWrapper.FuelPumpNumber;
            FuelGrade fuelGrade = this.fuelInputDataWrapper.EnteredFuelGrade;
            decimal totalEnteredfuelPrice = this.fuelInputDataWrapper.EnteredFuelPrice;
            decimal resultingFuelQuantity = this.fuelInputDataWrapper.FuelQuantityLitres; // quantity gets automatically updated in the class when fuel price and grade change

            // generate a random id (temporary solution)
            Random random = new Random();
            int fuelProductId = random.Next(10000);

            // create product name / description
            string fuelProductNameDescription = $"{this.fuelInputDataWrapper.FuelPumpNumberStr} {fuelGrade}";

            // Create a FuelProductDTO and add it to the product dto data list

            FuelProductDTO fuelProductDTO = new FuelProductDTO
            {
                Id = fuelProductId,
                ProductNameDescription = fuelProductNameDescription,
                UnitPriceDollars = FuelGradeUtils.GetFuelPrice(fuelGrade),
                Quantity = resultingFuelQuantity, // the total volume of the fuel product
                TotalPriceDollars = totalEnteredfuelPrice,
                FuelGrade = fuelGrade,
                PumpNumber = fuelPumpNumber,
            };

            // add to list of ProductDTOs
            MainFormDataUpdater.AddNewFuelProductToCart(this.userCartProductsDataList, fuelProductDTO, this.paymentDataWrapper, this.fuelInputDataWrapper);

            reset();

            UpdatePayButtonVisibility();
        }



        // === PAYMENT Button Event Handlers ===

        /// <summary>
        /// Card payment button event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PayCardButton_Click(object sender, EventArgs e)
        {
            HidePanels();

            // Make back button visible
            pnlBottomNavBack.Visible = true;

            // Show cardPaymentUserControl1 user control
            this.cardPaymentUserControl.Visible = true;
        }

        /// <summary>
        /// Cash payment button event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PayCashButton_Click(object sender, EventArgs e)
        {
            HidePanels();

            // Make back button visible
            pnlBottomNavBack.Visible = true;

            // Show cash payment user control
            this.cashPaymentUserControl.Visible = true;

        }

        /// <summary>
        /// Event Handler that is attached to both the confirm payment 
        /// buttons of the card payment user control, and the cash payment user control.
        /// </summary>
        /// <param name="paymentMethod"></param>
        /// <returns></returns>
        private async Task ConfirmPaymentButton_Click(PaymentMethod paymentMethod)
        {
            if (paymentMethod == PaymentMethod.CARD)
            {
                // card pays exact amount - set amount remaining back to the default initial amount
                paymentDataWrapper.AmountRemaining = PaymentConstants.INITIAL_AMOUNT_DOLLARS;
            }
            else if (paymentMethod == PaymentMethod.CASH)
            {
                decimal amountEntered = this.cashPaymentUserControl.CashInputAmountDollars;

                paymentDataWrapper.AmountRemaining -= amountEntered;

                // if there there is still an remaining amount to be paid
                if (paymentDataWrapper.AmountRemaining > PaymentConstants.INITIAL_AMOUNT_DOLLARS)
                {
                    //Reset the cash payment user control input and display data
                    this.cashPaymentUserControl.Reset();
                    this.cashPaymentUserControl.Visible = false;
                    reset(); // reset other UI state
                    return;
                }
            }

            decimal amountTendered;

            // amountTendered should be equal to SUBTOTAL + CHANGE (dollars)
            amountTendered = paymentDataWrapper.Subtotal - paymentDataWrapper.AmountRemaining; // if AmountRemaining is (-), then subtracting adds the extra amount paid

            // create transaction asyncronously using transaction service, passing in the payment method parameter
            bool transactionSuccessful = await transactionService.CreateTransactionAsync(paymentMethod,
                                                            paymentDataWrapper.Subtotal,
                                                            amountTendered,
                                                            this.userCartProductsDataList);

            // if transaction was not successful
            if (!transactionSuccessful)
            {
                MessageBox.Show("ERROR: Could not complete transaction", "Payment Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // hide/close the card payment user control
            this.cardPaymentUserControl.Visible = false;

            // reset all entered values in cash payment user control, and hide/close the card payment user control
            this.cashPaymentUserControl.Reset();
            this.cashPaymentUserControl.Visible = false;

            // Update the data source and the amount tendered UI label
            paymentDataWrapper.AmountTendered = amountTendered;

            MessageBox.Show("Payment successful!", "Payment Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // clear the ui cart and reset payment amounts (by resetting ui data sources to defaults)
            MainFormDataUpdater.RemoveAllProductsFromCart(this.userCartProductsDataList, this.paymentDataWrapper, ref this.currentSelectedProductQuantity);

            UpdatePayButtonVisibility();

            // reset other UI state
            reset();
        }

        // ======================================= LOGIN FORM ============================================

        /// <summary>
        /// Handles all the login logic.
        /// </summary>
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            // Retrieve user input
            string enteredUsername = textBoxAccountID.Text.Trim();
            string enteredPassword = textBoxPassword.Text;

            // Validate user inputs (check if they are empty)
            bool validationSuccessful = LoginFormValidator.ValidateFields(enteredUsername, enteredPassword);

            if (!validationSuccessful)
            {
                labelLoginError.Text = "Error: Please enter both username and password.";
                labelLoginError.Visible = true;
                return;
            }

            // Validate user credentials
            bool authenticationSuccessful = authenticationService.Authenticate(enteredUsername, enteredPassword);

            if (!authenticationSuccessful)
            {
                labelLoginError.Text = "Error: Username or password incorrect.";
                labelLoginError.Visible = true;
                return;
            }

            // Successful login
            tabelLayoutPanelLogin.Visible = false;  // Hide the login panel
            textboxBarcode.Focus(); // Focus on the barcode textbox for instant scanning

            MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ======================================== BARCODE SCANNER ============================================

        /// <summary>
        /// Handles the logic for when a key is pressed in the barcode textbox, specifically when the Enter key is pressed.
        /// It checks if the scanned barcode exists in the inventory and adds the corresponding product to the cart if found.
        /// </summary>
        private void textboxBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Get the text that the scanner wrote to the label
                string scannedBarcode = textboxBarcode.Text.Trim();

                if (string.IsNullOrWhiteSpace(scannedBarcode))
                    return;

                BarcodeRetailProductDTO barcodeRetailProductExists = this.inventoryService.CheckAndReturnIfBarcodeRetailProductExits(scannedBarcode);

                //RetailProductDTO rpDto = this.inventoryService.CheckAndReturnIfBarcodeRetailProductExits(scannedBarcode);

                // if the scanned product doesn't exist (null)
                if (barcodeRetailProductExists == null)
                {
                    MessageBox.Show("Product not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                // if it exists
                else
                {
                    MainFormDataUpdater.AddNewRetailProductToCart(
                        this.userCartProductsDataList,
                        barcodeRetailProductExists,
                        this.paymentDataWrapper,
                        ref currentSelectedProductQuantity
                    );

                    UpdatePayButtonVisibility();
                }

                // Clear textboxBarcode
                textboxBarcode.Text = "";
            }
        }

        /// <summary>
        /// Handles the logic for when the barcode textbox loses focus.
        /// Sets the focus back to the textbox after a short delay to allow other interactions.
        /// </summary>
        private void textboxBarcode_Leave(object sender, EventArgs e)
        {
            // Set the focus back after a small delay
            Timer timer = new Timer();
            timer.Interval = 100;
            timer.Tick += (s, args) =>
            {
                textboxBarcode.Focus();
                timer.Stop();
            };
            timer.Start();
        }

        // Prints the reciept
        private void btnPrintReceipt_Click(object sender, EventArgs e)
        {
            ReceiptPrinter rp = new ReceiptPrinter();
            rp.printReceipt();
        }


        // ======================================== REVIEW ============================================

        private void ShowPreviousTransaction(object sender, EventArgs e)
        {
            currentlyChosenTransactionNum--;
            currentlyChosenTransactionNum = transactionService.GetChosenTransactionNumberWithinBounds(currentlyChosenTransactionNum);
            ShowTransaction();
        }

        private void ShowNextTransaction(object sender, EventArgs e)
        {
            currentlyChosenTransactionNum++;
            currentlyChosenTransactionNum = transactionService.GetChosenTransactionNumberWithinBounds(currentlyChosenTransactionNum);
            ShowTransaction();
        }

        private void ShowReviewAndCurrentlySelectedTransaction(object sender, EventArgs e)
        {
            // don't allow review if the user is in a current transaction
            if ((posMode == POSMode.TRANSACTION) && (userCartProductsDataList.Count > 0))
            {
                MessageBox.Show("Unable to open transaction review. Current transaction is in progress.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Set current point of service system mode to "review of previous transactions"
            posMode = POSMode.PREVIOUS_TRANSACTION_REVIEW;

            HidePanels();

            pnlReview.Visible = true;
            pnlBottomNavBack.Visible = true;

            // Disable item selection from ui listbox
            listCart.SelectionMode = SelectionMode.None;

            // show transaction of the currently selected number
            ShowTransaction();
        }

        private async void ShowTransaction()
        {
            labelReview.Text = $"Transaction Review {currentlyChosenTransactionNum} of {transactionService.LatestTransactionNumber}";

            // get a list of product dtos from one of the previous transactions, and update user list cart (READ ONLY) - CLEAR THE USER CART AFTER
            IEnumerable<ProductDTO> previousTransactionProducts = await transactionService.GetTransactionProductListAsync(currentlyChosenTransactionNum);

            // clear all if there were any products in the cart data source
           MainFormDataUpdater.RemoveAllProductsFromCart(
                userCartProductsDataList,
                paymentDataWrapper,
                ref currentSelectedProductQuantity
            );

            // add all items to userCartProductsDataList
            foreach (ProductDTO productDTO in previousTransactionProducts)
            {
                if (productDTO is RetailProductDTO)
                {
                    MainFormDataUpdater.AddNewRetailProductToCart(
                        this.userCartProductsDataList,
                        (RetailProductDTO)productDTO,
                        this.paymentDataWrapper,
                        ref this.currentSelectedProductQuantity
                    );
                }
                else if (productDTO is FuelProductDTO)
                {
                    MainFormDataUpdater.AddNewFuelProductToCart(
                        this.userCartProductsDataList,
                        (FuelProductDTO)productDTO,
                        this.paymentDataWrapper,
                        this.fuelInputDataWrapper
                    );
                }
            }
        }

        //private async void btnReview_Click(object sender, EventArgs e)
        //{
        //    // don't allow review if the user is in a current transaction
        //    if ((posMode == POSMode.TRANSACTION) && (userCartProductsDataList.Count > 0))
        //    {
        //        MessageBox.Show("Unable to open transaction review. Current transaction is in progress.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }

            //    HidePanels();

            //    pnlReview.Visible = true;
            //    pnlBottomNavBack.Visible = true;

            //    // Set current point of service system mode to "review of previous transactions"
            //    posMode = POSMode.PREVIOUS_TRANSACTION_REVIEW;

            //    // Disable item selection from ui listbox
            //    listCart.SelectionMode = SelectionMode.None;

            //    // get a list of product dtos from one of the previous transactions, and update user list cart (READ ONLY) - CLEAR THE USER CART AFTER
            //    IEnumerable<ProductDTO> previousTransactionProducts = await transactionService.GetTransactionProductListAsync(currentlyChosenTransactionNum);

            //    // clear all if there were any products in the cart data source
            //    MainFormDataUpdater.RemoveAllProductsFromCart(
            //        userCartProductsDataList,
            //        paymentDataWrapper,
            //        ref currentSelectedProductQuantity
            //    );

            //    // add all items to userCartProductsDataList
            //    foreach (ProductDTO productDTO in previousTransactionProducts)
            //    {
            //        if (productDTO is RetailProductDTO)
            //        {
            //            MainFormDataUpdater.AddNewRetailProductToCart(
            //                this.userCartProductsDataList,
            //                (RetailProductDTO)productDTO,
            //                this.paymentDataWrapper,
            //                ref this.currentSelectedProductQuantity
            //            );
            //        }
            //        else if (productDTO is FuelProductDTO)
            //        {
            //            MainFormDataUpdater.AddNewFuelProductToCart(
            //                this.userCartProductsDataList,
            //                (FuelProductDTO)productDTO,
            //                this.paymentDataWrapper,
            //                this.fuelInputDataWrapper
            //            );
            //        }
            //    }
            //}

            // === App Exit ===
        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            bool transactionsDeletionSuccessful = transactionService.DeleteAllTransactions();

            if (!transactionsDeletionSuccessful)
            {
                MessageBox.Show($"Error clearing data source data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
