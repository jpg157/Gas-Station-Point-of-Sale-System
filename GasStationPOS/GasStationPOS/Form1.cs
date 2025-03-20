using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace GasStationPOS
{
    public partial class MainForm : Form
    {
        private int selectedQuantity = 1; // Default quantity
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
        private decimal subtotal = 0;
        private decimal tendered = 0; // Will be updated later
        private bool fuelPumpsActivated = false;

        // Fuel prices
        private decimal fuelRegularPriceCAD = 1.649m;
        private decimal fuelPlusPriceCAD = 1.849m;
        private decimal fuelSupremePriceCad = 2.049m;
        private string fuelAmountInput = "";
        private decimal fuelPrice = 0.00m; // Holds the selected fuel price

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            timerDateTime.Start(); // Start the timer on form load
        }

        private void timerDateTime_Tick(object sender, EventArgs e)
        {
            // Update the label with the current date and time
            lblDateTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        // Quantity Button Click Event
        private void QuantityButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                selectedQuantity = int.Parse(btn.Text.Replace("x", ""));
            }
        }


        // Helper Method to update the visibility of pay buttons
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

        // Hides visibility modified panels
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


        private void UpdateAfterAddingToCart()
        {
            labelSubtotal.Text = $"${subtotal:F2}";

            // Update Remaining
            labelRemaining.Text = $"${(tendered - subtotal):F2}";

            // Reset selectedQuantity
            selectedQuantity = 1;

            // Show payment buttons
            UpdatePayButtonVisibility();
        }

        // Product Button Click Event
        private void ProductButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null && productPrices.ContainsKey(btn.Text))
            {
                string productName = btn.Text;
                decimal price = productPrices[productName];
                decimal total = price * selectedQuantity;

                CartItem cartItem = new CartItem(productName, selectedQuantity, price, total);

                listCart.Items.Add(cartItem);

                // Update subtotal
                subtotal += total;
                UpdateAfterAddingToCart();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            listCart.Items.Clear();
            subtotal = 0;
            selectedQuantity = 1;
            labelSubtotal.Text = "";
            labelRemaining.Text = "";

            // Hide payment buttons
            UpdatePayButtonVisibility();
        }

        // Click on Cart Item
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

        // Back Button Click
        private void btnBack_Click(object sender, EventArgs e)
        {
            reset();
        }

        // Remove Item Button Click
        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            if (listCart.SelectedIndex != -1 && listCart.SelectedItem != null)
            {
                // Get the selected item from the list
                var selectedItem = listCart.SelectedItem as CartItem;

                if (selectedItem != null)
                {
                    // Remove the selected item from the list
                    listCart.Items.Remove(selectedItem);

                    // Show pnlProducts, pnlBottomNavMain
                    HidePanels();
                    pnlProducts.Visible = true;
                    pnlBottomNavMain.Visible = true;

                    // Update the subtotal after removing the item
                    subtotal -= selectedItem.TotalPrice;
                    labelSubtotal.Text = $"${subtotal:F2}"; // Update the label displaying the subtotal

                    // Update Remaining
                    labelRemaining.Text = $"${(tendered - subtotal):F2}";

                    // if the cart is empty, hide payment buttons
                    UpdatePayButtonVisibility();
                }
            }
        }

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
                pnlBottomNavBack.Visible= true;

                UnhighlightFuelPumps();

                // Highlight the selected fuel pump
                btn.FlatAppearance.BorderColor = Color.Gold;
                btn.FlatAppearance.BorderSize = 3;

            }
        }

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

        private void btnFuelCalculatorBackspace_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(fuelAmountInput))
            {
                fuelAmountInput = fuelAmountInput.Substring(0, fuelAmountInput.Length - 1);
                UpdateFuelPriceLabel();
            }
        }

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
                listCart.Items.Add(newItem);

                subtotal += total;

                reset();
                UpdateAfterAddingToCart();
            }
            else
            {
                MessageBox.Show("Please enter a valid fuel amount.");
            }
        }
    }
}
