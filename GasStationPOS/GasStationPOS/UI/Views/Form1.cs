using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GasStationPOS.Core.Models.ProductModels;
using GasStationPOS.Core.Models.TransactionModels;
using GasStationPOS.UI;
using GasStationPOS.UI.Constants;
using GasStationPOS.UI.UIDataChangeEvents;
using GasStationPOS.UI.UIDataEventArgs;
using GasStationPOS.UI.ViewDataTransferObjects.ProductDTOs;
using GasStationPOS.UI.Views;

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
    /// Date: 27 March 2025
    ///
    public partial class MainForm : Form, IMainView
    {
        // Flag to manage the activation state of fuel pumps
        private bool fuelPumpsActivated = false;

        // ========= Begin Region IMainView Interface Implementation =========


        //// Variables to track the subtotal and the amount tendered by the customer
        private decimal subtotal = 0;                                               //  to Delete once add fuel implemented ----

        // Fuel prices for different fuel types
        private decimal fuelRegularPriceCAD = 1.649m;                               //  to Delete once add fuel implemented ----
        private decimal fuelPlusPriceCAD = 1.849m;                                  //  to Delete once add fuel implemented ----
        private decimal fuelSupremePriceCad = 2.049m;                               //  to Delete once add fuel implemented ----

        // Variable to store the input for fuel amount and selected fuel price
        private string fuelAmountInput = "";
        private decimal fuelPrice = 0.00m;


        public string FuelPriceAmountInputValue
        {
            get { return labelFuelPrice.Text; }
            set { labelFuelPrice.Text = value; }
        }
        public decimal FuelPrice
        {
            get { return fuelPrice; }
            set { fuelPrice = value; }
        }

        // SET UI LABELS AND DATA SOURCES (all of these methods are called in the MainPresenter) ===

        // TWO-WAY DATA BINDING BETWEEN PRESENTER DATA AND UI - CAN BE UPDATED MULTIPLE TIMES
        // (presenter -> view) and (view -> presenter)

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
        public void SetUserCartListBindingSource(BindingSource userCartProductsBindingSource)
        {
            listCart.DataSource = userCartProductsBindingSource; // see if this works, otherwise use a DataGridView class
        }

        public event EventHandler<UpdateQuantityEventArgs>          UpdateSelectedProductQuantityEvent;
        public event EventHandler<AddRetailProductToCartEventArgs>  AddNewRetailProductToCartEvent;
        public event EventHandler<AddFuelProductToCartEventArgs>    AddNewFuelProductToCartEvent; //TODO
        public event EventHandler<RemoveProductEventArgs>           RemoveProductFromCartEvent;
        public event EventHandler                                   RemoveAllProductsFromCartEvent;
        public event EventHandler<SubmitPaymentEventArgs>           SubmitPaymentEvent; //TODO - once payment UI is done



        // ONE WAY, MULTIPLE TIMES UI UPDATING (presenter -> view)

        /// <summary>
        /// Sets the binding sources of the Labels:
        /// - Subtotal (decimal)
        /// - Amount tendered (decimal)
        /// - Amount remaining (decimal)
        /// </summary>
        /// <param name="paymentDataBindingSource"></param>
        public void SetPaymentInfoLabelsBindingSource(BindingSource paymentDataBindingSource)
        {
            bool    formattingEnabled               = true; // boolean to determine if the created Binding object will automatically format the payment decimal value
            decimal defaultValueWhenDataSourceNull  = PaymentConstants.INITIAL_AMOUNT_DOLLARS; // default value in case the data source is null
            string  twoDecimalPlacesString          = "C2"; // tells Binding object to format the data source to 2 decimal places (proper currency format)

            // Need DataSourceUpdateMode.OnPropertyChanged so it knows to change the value in the the UI control when the datasource (ex. Subtotal) changes value
            this.labelSubtotal.DataBindings.Add(new Binding("Text", paymentDataBindingSource, "Subtotal", formattingEnabled, DataSourceUpdateMode.OnPropertyChanged, defaultValueWhenDataSourceNull, twoDecimalPlacesString));
            this.labelTendered.DataBindings.Add(new Binding("Text", paymentDataBindingSource, "AmountTendered", formattingEnabled, DataSourceUpdateMode.OnPropertyChanged, defaultValueWhenDataSourceNull, twoDecimalPlacesString));
            this.labelRemaining.DataBindings.Add(new Binding("Text", paymentDataBindingSource, "AmountRemaining", formattingEnabled, DataSourceUpdateMode.OnPropertyChanged, defaultValueWhenDataSourceNull, twoDecimalPlacesString));
        }

        // ONE TIME UI UPDATING (on load) (presenter -> view)

        /// <summary>
        /// Retail product UI Buttons: 
        /// 
        /// The label of each button is set to a value based on the corresponding retail product that it corresponds to
        /// Retail Product data is IEnumerable<RetailProductDTO> (data converted from json file / database in the presenter)
        /// 
        /// Method is called in the presenter - not in this main form class.
        /// </summary>
        public void SetRetailProductButtonDataFromSource(IEnumerable<RetailProductDTO> retailProductsDataList)
        {
            foreach (RetailProductDTO retailProductDTO in retailProductsDataList)
            {
                // get the corresponding button name based on the retail product (ex. productId of 1 -> "btnRp1")
                string retailBtnName        = ButtonNameUtils.GetRetailButtonNameFromProductId(retailProductDTO.Id);

                // find the button with the retail button name (ex. find button with name btnRp1
                Button retailProductButton  = (Button) this.Controls.Find(retailBtnName, true).First();

                // 1. Set the text of the button to the name attribute in the RetailProductDTO object
                retailProductButton.Text = retailProductDTO.ProductNameDescription;

                // 2. Attach the retailProductDTO object to the button using the Tag attribute (to know which retail product that the clicked button was associated with)
                retailProductButton.Tag  = retailProductDTO;

                // 3. Associate the same "add product to cart" EventHandler delegate for each retail button
                // - Subscribe AddNewRetailProductToCartEvent delegates -> Add Product button click event
                // - Pass in the tag value into the custom event argument for the presenter to be able to access the value
                retailProductButton.Click += delegate { AddNewRetailProductToCartEvent?.Invoke(this, new AddRetailProductToCartEventArgs((RetailProductDTO) retailProductButton.Tag)); };

                // Add UI updating method to event handler (UpdatePayButtonVisibility will execute after the item was added)
                retailProductButton.Click += delegate { UpdatePayButtonVisibility(); }; // Note: if the first event handler throws an exception, it will prevent subsequent handlers from executing, unless the exception is caught and handled.
            }
        }



        //TODO: FUEL PRODUCT BUTTON DATA SOURCE SETTING ================================================================================================================================================



        /// <summary>
        /// Update quantity UI Buttons:
        /// 
        /// The label of each button is set to a value based on the corresponding quantity that it corresponds to
        /// Quantity data is IEnumerable<int> (data converted from json file / database in the presenter)
        /// 
        /// Method is called in the presenter - not in this main form class.
        /// </summary>
        public void SetProductQuantityButtonDataFromSource(IEnumerable<int> retailProductQuantityDataList)
        {
            foreach (int productQuantityNum in retailProductQuantityDataList)
            {
                string quantityButtonName = ButtonNameUtils.GetQuantityButtonNameFromQuantityNumber(productQuantityNum);

                Button updateQuantityButton = (Button)this.Controls.Find(quantityButtonName, true).First();

                // 1. Set the text of the button to the quantity number
                updateQuantityButton.Text = $"{productQuantityNum.ToString()}x";

                // 2. Attack the index of the quantity number value to the button using the Tag attribute (to know which quantity that the clicked button was associated with)
                updateQuantityButton.Tag = productQuantityNum; // save the value to change the quantity to

                // 3. Associate the same "update selected product quantity" EventHandler delegate for each retail button
                // - Subscribe UpdateSelectedProductQuantityEvent delegates -> Update retail product quantity button click event
                // - Pass in the tag value into the custom event argument for the presenter to be able to access the value
                updateQuantityButton.Click += delegate { UpdateSelectedProductQuantityEvent?.Invoke(this, new UpdateQuantityEventArgs((int) updateQuantityButton.Tag)); };
            }
        }

        /// <summary>
        /// Attaches EventHandler objects to buttons, that do not have data binding properties with data stored in the presenter (retail, fuel and quantity buttons do)
        /// </summary>
        private void AssociateAndRaiseViewEvents()
        {
            // Subscribe btnRemoveItem_Click_v2 EventHandler form method -> Remove product button click event
            btnRemoveItem.Click += btnRemoveItem_Click_v2;

            // Subscribe RemoveAllProductsFromCartEvent EventHandler delegate -> Clear / Remove all products button click event
            btnClear.Click += delegate { RemoveAllProductsFromCartEvent?.Invoke(this, EventArgs.Empty); }; // using delegate operator than creates an anonymous method, taking an EventHandler into parameters
            btnClear.Click += delegate { UpdatePayButtonVisibility(); };

            // Subscribe SubmitPaymentEvent EventHandler delegate -> Card payment and Cash payment button click events
            // (Need to assign the corresponding PaymentMethod enum member in the SubmitPaymentEventArgs so presenter can handle the correct payment type)
            btnPayCard.Click += delegate { SubmitPaymentEvent?.Invoke(this, new SubmitPaymentEventArgs(PaymentMethod.CARD)); }; // event handler invocation passing in CARD to custom event args
            btnPayCash.Click += delegate { SubmitPaymentEvent?.Invoke(this, new SubmitPaymentEventArgs(PaymentMethod.CASH)); }; // CASH
        }

        // ========= End Region of IMainView Interface Implementation =========


        // ========= Begin Region of Re-used Updated Event handlers =========


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
                    // Invoke the remove product from cart event handler - (delegate data updating to the event handler attached in the presenter)
                    this.RemoveProductFromCartEvent?.Invoke(this, new RemoveProductEventArgs(selectedProduct));

                    // Show pnlProducts, pnlBottomNavMain
                    HidePanels();
                    pnlProducts.Visible = true;
                    pnlBottomNavMain.Visible = true;

                    // if the cart is empty, hide payment buttons
                    UpdatePayButtonVisibility();
                }
            }
        }

        // ========= End Region of Re-used Updated Event handlers =========


        /// <summary>
        /// Constructor to initialize the MainForm
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            AssociateAndRaiseViewEvents();
        }


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

                HidePanels();

                // Show pnlSelectCartItem, pnlBottomNavBack
                pnlSelectCartItem.Visible   = true;
                pnlBottomNavBack.Visible    = true;
            }
        }

        /// <summary>
        /// Helper method to reset the UI to its initial state
        /// </summary>
        private void reset()                    // TODO
        {
            
            HidePanels();

            // Unhighlight fuel pumps
            UnhighlightFuelPumps();
            fuelPumpsActivated = false;

            // Show pnlProducts, pnlBottomNavMain
            pnlProducts.Visible = true;
            pnlBottomNavMain.Visible = true;

            // Reset the fuel price amount
            labelFuelPrice.Text = "0.00";       //TODO: ADD DATA BIND FOR labelFuelPrice
            fuelAmountInput = "";               // TODO: move functionality the presenter, and invoke an event handler that a Presenter method is attached to
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
            for (int i = 1; i <= 8; i++)
            {
                Button btn = Controls.Find($"btnFuelPump{i}", true).FirstOrDefault() as Button;
                if (btn != null)
                {
                    btn.FlatAppearance.BorderColor = Color.Gold;
                    btn.FlatAppearance.BorderSize = 3;
                }
            }
        }

        /// <summary>
        /// Resets the appearance of all fuel pump buttons 
        /// by changing their border color to black and border size to 1.
        /// </summary>
        private void UnhighlightFuelPumps()
        {
            for (int i = 1; i <= 8; i++)
            {
                Button btn = Controls.Find($"btnFuelPump{i}", true).FirstOrDefault() as Button;
                if (btn != null)
                {
                    btn.FlatAppearance.BorderColor = Color.Black;
                    btn.FlatAppearance.BorderSize = 1;
                }
            }
        }


        // ==================================================================== BELOW NOT REFACTORED YET ====================================================================


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
            Button btn = sender as Button;

            // Check if the button is valid
            if (btn != null)
            {
                // Update the label with the corresponding pump number
                int pumpNumber = int.Parse(btn.Name.Replace("btnFuelPump", ""));                            // TODO: move functionality the presenter, and invoke an event handler that a Presenter method is attached to
                labelPumpNum.Text = $"PUMP {pumpNumber}"; // Update label with the correct pump number      //TODO: ADD DATA BIND FOR labelPumpNum

                // Show the pnlFuelTypeSelection panel
                HidePanels();
                pnlFuelTypeSelect.Visible   = true;
                pnlBottomNavBack.Visible    = true;

                UnhighlightFuelPumps();

                // Highlight the selected fuel pump
                btn.FlatAppearance.BorderColor = Color.Gold;
                btn.FlatAppearance.BorderSize = 3;
            }
        }

        /// <summary>
        /// Handles the fuel type selection by updating the label with the selected fuel type 
        /// and showing the fuel amount input panel.
        /// </summary>
        private void btnFuelType_Click(object sender, EventArgs e) //to update
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                // Extract fuel type from the button's text
                string fuelType = btn.Text.ToUpper();

                //EventHandler that takes (fuelType) and presenter calls fuelGetFuelGradeFromLabel(fuelType) to update

                // Update label with pump number and fuel type
                labelFuelType.Text = $"{labelPumpNum.Text} {fuelType}";

                // Show the add fuel amount panel
                HidePanels();
                pnlAddFuelAmount.Visible = true;
                pnlBottomNavBack.Visible = true;
            }
        }

        /// <summary>
        /// Handles the numeric input for the fuel price calculator by appending or replacing input values.
        /// Updates the fuel price label accordingly.
        /// </summary>
        private void btnFuelCalculator_Click(object sender, EventArgs e) //to update
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                string value = btn.Text;

                // If input is "0", "00", "000", append normally
                if (value == "0" || value == "00" || value == "000")
                {
                    fuelAmountInput += value;                                        // to update ===
                }

                // If input is a preset amount (e.g., "10.00"), replace entire input
                else if (value.Contains("."))
                {
                    fuelAmountInput = value.Replace(".", ""); // Store without decimal     // to update ===
                }

                // Otherwise, append to the input
                else
                {
                    fuelAmountInput += value;                                        // to update ===
                }

                UpdateFuelPriceLabel();
            }
        }

        /// <summary>
        /// Updates the fuel price label by converting the input fuel amount to a decimal format and displaying it.
        /// </summary>
        private void UpdateFuelPriceLabel() //CHECK
        {
            if (string.IsNullOrEmpty(fuelAmountInput))      // to update ===
            {
                labelFuelPrice.Text = "0.00"; // Default if empty
                return;
            }

            // Convert input to decimal format (X.YY)
            decimal amount = decimal.Parse(fuelAmountInput) / 100;                                        // to update ===
            labelFuelPrice.Text = amount.ToString("0.00");
        }

        /// <summary>
        /// Handles the backspace input for the fuel price calculator, removing the last character from the fuel input.
        /// Updates the fuel price label accordingly.
        /// </summary>
        private void btnFuelCalculatorBackspace_Click(object sender, EventArgs e) //CHECK
        {
            if (!string.IsNullOrEmpty(fuelAmountInput))                                        // to update ===
            {
                fuelAmountInput = fuelAmountInput.Substring(0, fuelAmountInput.Length - 1);                                        // to update ===
                UpdateFuelPriceLabel();
            }
        }

        /// <summary>
        /// Validates the entered fuel amount, calculates the total price, and adds the fuel to the cart if valid.
        /// If invalid, shows an error message.
        /// </summary>
        private void btnFuelCalculatorEnter_Click(object sender, EventArgs e) //CHECK
        {
            if (decimal.TryParse(labelFuelPrice.Text, out decimal total) && total > 0)
            {
                // Extract pump number and fuel type
                string[] fuelInfo = labelFuelType.Text.Split(' ');
                int pumpNumber = int.Parse(fuelInfo[1]); // Get pump number
                string fuelType = fuelInfo[2]; // Get fuel type

                // Determine the price per liter based on the fuel type
                switch (fuelType.ToUpper())
                {
                    case "REGULAR":
                        fuelPrice = fuelRegularPriceCAD;                                        // move to presenter and call FuelPriceUtils class to return the fuel price based on the FuelGrade enum ===
                        break;
                    case "PLUS":
                        fuelPrice = fuelPlusPriceCAD;                                        // move to presenter and call FuelPriceUtils class to return the fuel price based on the FuelGrade enum ===
                        break;
                    case "SUPREME":
                        fuelPrice = fuelSupremePriceCad;                                        // move to presenter and call FuelPriceUtils class to return the fuel price based on the FuelGrade enum ===
                        break;
                }

                decimal quantity = Math.Round(total / fuelPrice, 3);

                // Create a CartItem and add it to the cart
                CartItem newItem = new CartItem(labelFuelType.Text, quantity, fuelPrice, total);      // to update
                listCart.Items.Add(newItem);
                
                subtotal += total;                                        // to update ===

                reset();
                //UpdateAfterAddingToCart();
            }
            else
            {
                MessageBox.Show("Please enter a valid fuel amount.");
            }
        }
    }
}
