using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            { "10L GAS CAN", 3.99m }
        };
        private decimal subtotal = 0;
        private decimal tendered = 0; // Will be updated later

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

        // Product Button Click Event
        private void ProductButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null && productPrices.ContainsKey(btn.Text))
            {
                string productName = btn.Text;
                decimal price = productPrices[productName];
                decimal total = price * selectedQuantity;

                string spacing;
                if (productName.Length < 12)
                    spacing = "\t\t\t\t"; // Extra tab for very short names
                else if (productName.Length < 24)
                    spacing = "\t\t\t";   // Three tabs for regular names
                else
                    spacing = "\t\t";     // Spacing for longer names

                listCart.Items.Add($"{productName,-20}{spacing}{selectedQuantity,-5}\t{price,-8:F2}\t{total,-8:F2}");

                // Update subtotal
                subtotal += total;
                labelSubtotal.Text = $"${subtotal:F2}";

                // Update Remaining
                labelRemaining.Text = $"${(tendered - subtotal):F2}";

                // Reset selectedQuantity
                selectedQuantity = 1;

                // Show payment buttons
                btnPayCard.Visible = true;
                btnPayCash.Visible = true;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            listCart.Items.Clear();
            subtotal = 0;
            labelSubtotal.Text = "";
            labelRemaining.Text = "";

            // Hide payment buttons
            btnPayCard.Visible = false;
            btnPayCash.Visible = false;
        }
    }
}
