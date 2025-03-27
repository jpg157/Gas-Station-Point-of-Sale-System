using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GasStationPOSUserControlLibrary;

namespace GasStationPOS.UI
{
    public partial class Test : Form
    {
        public Test()
        {
            GSPos_Cart gSPos_Cart = new GSPos_Cart();
            InitializeComponent();
        }
    }
}
