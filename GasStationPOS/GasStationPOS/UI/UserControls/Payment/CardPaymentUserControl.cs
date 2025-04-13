using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GasStationPOS.UI.UserControls.Payment
{
    /// <summary>
    /// Card payment user control for accepting card "tap" payments.
    /// </summary>
    public partial class CardPaymentUserControl: UserControl
    {
        public event EventHandler CardEnterButtonClick;

        public CardPaymentUserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// A button event handler to simulate card "tapping".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void keyEnterBtn_Click(object sender, EventArgs e)
        {
            CardEnterButtonClick?.Invoke(this, EventArgs.Empty);
        }
    }
}
