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
    public partial class CardPaymentUserControl: UserControl
    {
        public event EventHandler CardEnterButtonClick;

        public CardPaymentUserControl()
        {
            InitializeComponent();
        }

        private void keyEnterBtn_Click(object sender, EventArgs e)
        {
            CardEnterButtonClick?.Invoke(this, EventArgs.Empty);
        }
    }
}
