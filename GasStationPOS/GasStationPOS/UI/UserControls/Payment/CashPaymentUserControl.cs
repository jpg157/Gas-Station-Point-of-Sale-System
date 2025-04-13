using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GasStationPOS.UI.Constants;

namespace GasStationPOS.UI.UserControls.Payment
{
    /// <summary>
    /// Cash payment user control for accepting cash input, 
    /// and contains a cash value property that can be read by the MainForm.
    /// </summary>
    public partial class CashPaymentUserControl : UserControl
    {
        // Variable to store the input for cash amount tendered
        private string  cashInputAmountStr;
        private decimal cashInputAmountNumDollars;
        public decimal CashInputAmountDollars
        {
            get => cashInputAmountNumDollars;
            set
            {
                cashInputAmountNumDollars   = value;
            }
        }

        public event EventHandler CashEnterButtonClick;

        public CashPaymentUserControl()
        {
            InitializeComponent();
            CashInputAmountDollars  = PaymentConstants.INITIAL_AMOUNT_DOLLARS;
        }

        /// <summary>
        /// Handles enter button click event, by delegating to CashEnterButtonClick EventHandler object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCashEnter_Click(object sender, EventArgs e)
        {
            CashEnterButtonClick?.Invoke(sender, EventArgs.Empty);
        }

        /// <summary>
        /// Click event handler for cash payment buttons.
        /// </summary>
        private void btnCashPayment_Click(object sender, EventArgs e)
        {
            HandleNumericInput(ref cashInputAmountStr, labelCashAmount, sender);
        }

        /// <summary>
        /// Handles numeric input for fuel or cash payment calculators.
        /// Updates the corresponding input value and label.
        /// </summary>
        private void HandleNumericInput(ref string inputField, Label label, object sender)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                string value = btn.Text;

                // If input is "0", "00", or "000", append normally
                if (value == "0" || value == "00" || value == "000")
                {
                    inputField += value;
                }
                // If input is a preset amount (e.g., "10.00"), replace the entire input
                else if (value.Contains("."))
                {
                    inputField = value.Replace(".", ""); // Store without decimal
                }
                // Otherwise, append to the input
                else
                {
                    inputField += value;
                }

                UpdateAmountEntered(inputField, label);
            }
        }

        /// <summary>
        /// Updates the label by converting the input amount to a decimal format.
        /// </summary>
        private void UpdateAmountEntered(string inputField, Label label)
        {
            decimal cashInputAmountNum;

            if (string.IsNullOrEmpty(inputField))
            {
                cashInputAmountNum = 0.00m; // Default if empty
            }
            else
            {
                // Convert input to decimal format (X.YY)
                cashInputAmountNum = decimal.Parse(inputField) / 100.0m;
            }

            // update the cash input amount (string label)
            label.Text = cashInputAmountNum.ToString("0.00");

            // update the cash input amount (decimal)
            CashInputAmountDollars = cashInputAmountNum;
        }

        /// <summary>
        /// Handles the backspace input for the cash payment, removing the last character from the cash input label.
        /// Updates the cash payment label accordingly.
        /// </summary>
        private void btnCashBackspace_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cashInputAmountStr))
            {
                cashInputAmountStr = cashInputAmountStr.Substring(0, cashInputAmountStr.Length - 1);
                UpdateAmountEntered(cashInputAmountStr, labelCashAmount);
            }
        }

        /// <summary>
        /// Resets the cash amount input and corresponding label text.
        /// </summary>
        public void Reset()
        {
            cashInputAmountStr      = "";
            labelCashAmount.Text    = "0.00";
            CashInputAmountDollars  = PaymentConstants.INITIAL_AMOUNT_DOLLARS;
        }
    }
}
