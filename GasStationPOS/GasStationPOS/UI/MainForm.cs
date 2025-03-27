using GasStationPOS.MockDatabase;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using GasStationPOSUserControlLibrary;

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
        // Default quantity
        private int selectedQuantity = 1;

        // Dictionary to store product names and their associated prices
        private Dictionary<string, decimal> productPrices = new Dictionary<string, decimal>
        {
            { "HOT BEVERAGE 12OZ", 2.00m },
            { "HOT BEVERAGE 24OZ", 2.89m },
            { "ICE CUBE 2.3KG", 3.99m },
            { "PROPANE EXCHANGE", 29.99m },
            { "20LB PROPANE", 49.99m },
            { "WINDSHIELD WASHER", 12.99m },
            { "LOTTO TICKET $1", 1.00m },
            { "LOTTO TICKET $10", 10.00m },
            { "1L ENGINE OIL 5W30", 7.99m },
            { "1L ENGINE OIL 10W30", 11.99m },
            { "1L ENGINE OIL 5W20", 6.99m },
            { "1L ENGINE OIL 10W20", 10.99m },
            { "FIREWOOD", 13.99m },
            { "PC COOLANT 1.85L", 19.99m },
            { "PC COOLANT 3.78L", 33.99m },
            { "10L GAS CAN", 3.99m },
            { "PIZZA SLICE SMALL", 1.29m },
            { "PIZZA SLICE LARGE", 2.09m },
            { "HOTDOG", 1.49m },
            { "POTATO WEDGES", 2.29m },
            { "CHICKEN SANDWICH", 3.99m },
            { "VEG SANDWICH", 2.49m },
            { "DASANI WATER BOTTLE 591ML", 2.69m },
            { "PURELIFE WATER BOTTLE 500ML", 2.59m },
            { "FIJI WATER BOTTLE 500ML", 3.49m },
            { "SMART WATER BOTTLE 350ML", 3.59m }
        };

        // Variables to track the subtotal and the amount tendered by the customer
        private decimal subtotal = 0;
        private decimal tendered = 0; // Will be updated later

        // Flag to manage the activation state of fuel pumps
        private bool fuelPumpsActivated = false;

        // Fuel prices for different fuel types
        private decimal fuelRegularPriceCAD = 1.649m;
        private decimal fuelPlusPriceCAD = 1.849m;
        private decimal fuelSupremePriceCad = 2.049m;

        // Variable to store the input for fuel amount and selected fuel price
        private string fuelAmountInput = "";
        private decimal fuelPrice = 0.00m;

        // Path for the Mock Database
        private readonly string dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MockDatabase", "data.json");
        private Database database = new Database();

        /// <summary>
        /// Constructor to initialize the MainForm
        /// </summary>
        public MainForm()
        {
            setupDatabase(); // Setup the user database
            InitializeComponent();
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
        /// Event handler for selecting the quantity of products
        /// </summary>
        private void QuantityButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                selectedQuantity = int.Parse(btn.Text.Replace("x", ""));
            }
        }


        /// <summary>
        /// Helper Method to toggle the visibility of pay buttons
        /// </summary>
        private void UpdatePayButtonVisibility()
        {
            if (gsPos_Cart.GetCartItemsCount() == 0)
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
        /// Method to update the UI after adding items to the cart
        /// </summary>
        private void UpdateAfterAddingToCart()
        {
            // Updates the subtitle and ramining labels
            gsPos_Cart.UpdateSubtitleAndRemainingLabels(
                $"${subtotal:F2}",
                $"${(tendered - subtotal):F2}"
                );

            // Reset selectedQuantity
            selectedQuantity = 1;

            // Show payment buttons
            UpdatePayButtonVisibility();
        }

        /// <summary>
        /// Event handler for adding products to the cart
        /// </summary>
        private void ProductButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null && productPrices.ContainsKey(btn.Text))
            {
                string productName = btn.Text;
                decimal price = productPrices[productName];
                decimal total = price * selectedQuantity;

                CartItem cartItem = new CartItem(productName, selectedQuantity, price, total);

                gsPos_Cart.AddItemToCart(cartItem);

                // Update subtotal
                subtotal += total;
                UpdateAfterAddingToCart();
            }
        }

        /// <summary>
        /// // Event handler for clearing the cart
        /// </summary>
        private void btnClear_Click(object sender, EventArgs e)
        {
            gsPos_Cart.ClearCart();

            // Handled by the program
            subtotal = 0;
            selectedQuantity = 1;

            // Hide payment buttons
            UpdatePayButtonVisibility();
        }

        /// <summary>
        /// Event handler for selecting a cart item
        /// </summary>
        private void GsPos_Cart_CartItemSelected(object sender, EventArgs e)
        {
            if (gsPos_Cart.IsListCartEmpty())
            {
                string selectedItem = gsPos_Cart.GetListCartItemString();
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
        /// Event handler for the back button click
        /// </summary>
        private void btnBack_Click(object sender, EventArgs e)
        {
            reset();
        }

        /// <summary>
        /// Event handler for removing an item from the cart
        /// </summary>
        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            if (gsPos_Cart.IsListCartEmpty())
            {
                // Get the selected item from the list
                var selectedItem = gsPos_Cart.GetCartItem();

                if (selectedItem != null)
                {
                    // Remove the selected item from the list
                    gsPos_Cart.RemoveItemFromCart(selectedItem);

                    // Show pnlProducts, pnlBottomNavMain
                    HidePanels();
                    pnlProducts.Visible = true;
                    pnlBottomNavMain.Visible = true;

                    // Update the subtotal after removing the item
                    subtotal -= selectedItem.TotalPrice;

                    // Updates the subtitle and remaining labels
                    gsPos_Cart.UpdateSubtitleAndRemainingLabels(
                        $"${subtotal:F2}",
                        $"${(tendered - subtotal):F2}"
                        );

                    // if the cart is empty, hide payment buttons
                    UpdatePayButtonVisibility();
                }
            }
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
                int pumpNumber = int.Parse(btn.Name.Replace("btnFuelPump", ""));
                labelPumpNum.Text = $"PUMP {pumpNumber}"; // Update label with the correct pump number

                // Show the pnlFuelTypeSelection panel
                HidePanels();
                pnlFuelTypeSelect.Visible = true;
                pnlBottomNavBack.Visible = true;

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
        private void btnFuelType_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                // Extract fuel type from the button's text
                string fuelType = btn.Text.ToUpper();

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
        /// Updates the fuel price label by converting the input fuel amount to a decimal format and displaying it.
        /// </summary>
        private void UpdateFuelPriceLabel()
        {
            if (string.IsNullOrEmpty(fuelAmountInput))
            {
                labelFuelPrice.Text = "0.00"; // Default if empty
                return;
            }

            // Convert input to decimal format (X.YY)
            decimal amount = decimal.Parse(fuelAmountInput) / 100;
            labelFuelPrice.Text = amount.ToString("0.00");
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
                        fuelPrice = fuelRegularPriceCAD;
                        break;
                    case "PLUS":
                        fuelPrice = fuelPlusPriceCAD;
                        break;
                    case "SUPREME":
                        fuelPrice = fuelSupremePriceCad;
                        break;
                }

                // Calculate quantity up to three decimal places
                decimal quantity = Math.Round(total / fuelPrice, 3);

                // Create a CartItem and add it to the cart
                CartItem newItem = new CartItem(labelFuelType.Text, quantity, fuelPrice, total);
                gsPos_Cart.AddItemToCart(newItem);

                subtotal += total;

                reset();
                UpdateAfterAddingToCart();
            }
            else
            {
                MessageBox.Show("Please enter a valid fuel amount.");
            }
        }

        /// <summary>
        /// Sets up the user database by reading the JSON file and deserializing it.
        /// </summary>
        private void setupDatabase()
        {
            try
            {
                // Ensure the database file exists
                if (!File.Exists(dataPath))
                {
                    MessageBox.Show($"Database file not found at: {dataPath}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Read and deserialize JSON
                string jsonString = File.ReadAllText(dataPath);
                database = JsonSerializer.Deserialize<Database>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new Database(); // Ensure database is never null

                // Ensure Accounts list is initialized
                if (database.Accounts == null)
                {
                    database.Accounts = new List<Account>();
                    MessageBox.Show("Warning: No accounts found in database.", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading database: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles all the login logic.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            // Retrieve user input
            string enteredAccountID = textBoxAccountID.Text.Trim();
            string enteredPassword = textBoxPassword.Text;

            // Check if inputs are empty
            if (string.IsNullOrEmpty(enteredAccountID) || string.IsNullOrEmpty(enteredPassword))
            {
                labelLoginError.Text = "Error: Please enter both username and password.";
                labelLoginError.Visible = true;
                return;
            }

            // Validate user credentials
            if (!AuthenticationHelper.ValidateAccount(enteredAccountID, enteredPassword, database))
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
