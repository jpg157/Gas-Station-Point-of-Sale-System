using GasStationPOS.Core.Data.Models.ProductModels;
using GasStationPOS.Core.Data.Models.TransactionModels;
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
        // ======================== CONSTANTS (move to a different file) ========================
        // Fuel prices for different fuel types
        private static readonly decimal fuelRegularPriceCAD = 1.649m;
        private static readonly decimal fuelPlusPriceCAD = 1.849m;
        private static readonly decimal fuelSupremePriceCad = 2.049m;

        // ======================== SERVICES ========================
        private readonly IInventoryService      inventoryService; // for retrieving all retail and fuel product data to display to the UI
        private readonly ITransactionService    transactionService;
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

        // ======================== CONSTANT DATA FOR APPLICATION ========================

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
        private decimal fuelPrice;

        /// <summary>
        /// Constructor to initialize the MainForm
        /// </summary>
        public MainForm(IInventoryService       inventoryService, 
                        ITransactionService     transactionService,
                        IAuthenticationService  authenticationService) // dependency injection of services
        {
            //setupDatabase(); // Setup the user database ================================================================================================================================================
            InitializeComponent();

            // === Initilize required services ===
            this.inventoryService   = inventoryService;
            this.transactionService = transactionService;
            this.authenticationService = authenticationService;

            // === Initilize Main View UI Underlying Data ===

            currentSelectedProductQuantity = QuantityConstants.DEFAULT_RETAIL_PRODUCT_QUANTITY_VALUE; // Initial value of the current selected product quantity

            fuelPumpsActivated = false;


            fuelAmountInput = "";
            fuelPrice = 0.00m;

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

            this.userCartProductsBindingSource  = new BindingSource();
            this.fuelInputDataBindingSource     = new BindingSource();
            this.paymentDataBindingSource       = new BindingSource();

            this.userCartProductsBindingSource.DataSource   = this.userCartProductsDataList; // set UI data binding source to the corresponding data stored in the object
            this.fuelInputDataBindingSource.DataSource      = this.fuelInputDataWrapper;
            this.paymentDataBindingSource.DataSource        = this.paymentDataWrapper;

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
            btnFuelRegular.Tag  = FuelGrade.REGULAR;
            btnFuelRegular.Click += delegate { HandleFuelGradeBtnClick(FuelGrade.REGULAR); };

            btnFuelPlus.Text    = FuelGrade.PLUS.ToString();
            btnFuelPlus.Tag     = FuelGrade.PLUS;
            btnFuelPlus.Click += delegate { HandleFuelGradeBtnClick(FuelGrade.PLUS); };

            btnFuelSupreme.Text = FuelGrade.SUPREME.ToString();
            btnFuelSupreme.Tag  = FuelGrade.SUPREME;
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
            this.cardPaymentUserControl.KeyEnterButtonClicked += async delegate { await ConfirmPaymentButton_Click(PaymentMethod.CARD);  };

            btnPayCash.Click += PayCashButton_Click;                                      // ============================================ CASH PAYMENT UI - TODO ============================================
            //this.cashPaymentUserControl.EnterButtonClicked += async delegate { await ConfirmPaymentButton_Click(PaymentMethod.CASH);  }; // ============================================ CASH PAYMENT UI - TODO ============================================
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
            }
            else
            {
                btnPayCard.Visible = true;
                btnPayCash.Visible = true;
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
            pnlFuelConfirmation.Visible = false;
            pnlFuelTypeSelect.Visible = false;
            pnlSelectCartItem.Visible = false;
            pnlAddFuelAmount.Visible = false;
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

            // Reset the fuel price amount
            labelFuelPrice.Text = "0.00";
            fuelAmountInput = "";
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
                if (btn != null)
                {
                    btn.FlatAppearance.BorderColor  = borderColor;
                    btn.FlatAppearance.BorderSize   = borderSize;
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
        }

        /// <summary>
        /// Handles the fuel pump selection by updating the pump number label 
        /// and navigating to the fuel type selection panel. 
        /// Highlights the selected fuel pump and unhighlights the others.
        /// </summary>
        private void btnFuelPump_Click(object sender, EventArgs e)
        {
            if (fuelPumpsActivated == false)
            {
                return;
            }

            // Get the clicked button
            Button fuelPumpButton = sender as Button;

            // Check if the button is valid
            if (fuelPumpButton != null)
            {
                // Update currently selected pump number (this value is stored in this.fuelInputDataWrapper)
                // (Which automatically updates the labels with the corresponding pump number)
                MainFormDataUpdater.UpdateSelectedPumpNumber(this.fuelInputDataWrapper, (int)fuelPumpButton.Tag);

                // Show the pnlFuelTypeSelection panel
                HidePanels();
                pnlFuelTypeSelect.Visible = true;
                pnlBottomNavBack.Visible = true;

                UnhighlightFuelPumps();

                // Highlight the selected fuel pump
                fuelPumpButton.FlatAppearance.BorderColor = Color.Gold;
                fuelPumpButton.FlatAppearance.BorderSize = 3;
            }
        }

        /// <summary>
        /// Handles the fuel type selection by updating the label with the selected fuel type 
        /// and showing the fuel amount input panel.
        /// </summary>
        private void HandleFuelGradeBtnClick(FuelGrade fuelGrade)
        {
            //// Update label with pump number and fuel type
            //labelFuelType.Text = $"{labelPumpNum.Text} {fuelType}";

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

            //labelFuelPrice.Text = amount.ToString("0.00");
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
            int         fuelPumpNumber              = this.fuelInputDataWrapper.FuelPumpNumber;
            FuelGrade   fuelGrade                   = this.fuelInputDataWrapper.EnteredFuelGrade;
            decimal     totalEnteredfuelPrice       = this.fuelInputDataWrapper.EnteredFuelPrice;
            decimal     resultingFuelQuantity       = this.fuelInputDataWrapper.FuelQuantityLitres; // quantity gets automatically updated in the class when fuel price and grade change

            // generate a random id (temporary solution)
            Random random = new Random();
            int fuelProductId = random.Next(10000);

            // create product name / description
            string fuelProductNameDescription = $"{this.fuelInputDataWrapper.FuelPumpNumberStr} {fuelGrade}";

            // Create a FuelProductDTO and add it to the product dto data list

            FuelProductDTO fuelProductDTO = new FuelProductDTO{
                Id                      = fuelProductId,
                ProductNameDescription  = fuelProductNameDescription,
                UnitPriceDollars        = FuelGradeUtils.GetFuelPrice(fuelGrade),
                Quantity                = resultingFuelQuantity, // the total volume of the fuel product
                TotalPriceDollars       = totalEnteredfuelPrice,
                FuelGrade               = fuelGrade,
                PumpNumber              = fuelPumpNumber,
            };

            // add to list of ProductDTOs
            MainFormDataUpdater.AddNewFuelProductToCart(this.userCartProductsDataList, fuelProductDTO, this.paymentDataWrapper, this.fuelInputDataWrapper);

            reset();
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
            //TODO ================================================================================================================================================================================================================
        }

        /// <summary>
        /// Event Handler that is attached to both the confirm payment 
        /// buttons of the card payment user control, and the cash payment user control.
        /// </summary>
        /// <param name="paymentMethod"></param>
        /// <returns></returns>
        private async Task ConfirmPaymentButton_Click(PaymentMethod paymentMethod)
        {
            decimal amountTendered = 0.0m;

            // CHANGE this if statement LATER ====================================================================================================================================================
            if (paymentMethod == PaymentMethod.CARD)
            {
                // card pays exact amount (set tendered amount equal to the subtotal)
                amountTendered = paymentDataWrapper.Subtotal;
            }
            else if (paymentMethod == PaymentMethod.CASH)
            {
                // TODO - ONCE CASH IS IMPLEMENTED CHANGE
                amountTendered = paymentDataWrapper.Subtotal;
            }

            // create transaction asyncronously using transaction service, passing in the payment method parameter
            bool transactionSuccessful = await transactionService.CreateTransactionAsync(paymentMethod,
                                                            paymentDataWrapper.Subtotal,
                                                            amountTendered,
                                                            this.userCartProductsDataList);

            if (!transactionSuccessful)
            {
                MessageBox.Show("ERROR: Could not complete transaction", "Payment Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // hide/close the card payment user control
            this.cardPaymentUserControl.Visible = false;

            // Update the data source fo the amount tendered UI label
            paymentDataWrapper.AmountTendered = amountTendered;

            MessageBox.Show("Payment successful!", "Payment Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // clear the ui cart and reset payment amounts (by resetting ui data sources to defaults)
            MainFormDataUpdater.RemoveAllProductsFromCart(this.userCartProductsDataList, this.paymentDataWrapper, ref this.currentSelectedProductQuantity);

            // reset other UI state
            reset();
        }

        // ============================================ CASH PAYMENT UI - TODO ============================================
        //private void PayCashButton_Click(object sender, EventArgs e)
        //{
        //    //HidePanels();

        //    //// Show cardPaymentUserControl1 user control
        //    //this.cashPaymentUserControl.Visible = true;
        //}

        //private void CashPaymentConfirmed_Click(object sender, EventArgs e)
        //{
        //    //// create card payment
        //    //transactionService.CreateTransaction(PaymentMethod.CASH, this.userCartProductsDataList)

        //    //this.cardPaymentUserControl.Visible = false;

        //    //reset();
        //}


        // === LOGIN FORM ===

        /// <summary>
        /// Handles all the login logic.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            // Retrieve user input
            string enteredUsername  = textBoxAccountID.Text.Trim();
            string enteredPassword  = textBoxPassword.Text;

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
            MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
