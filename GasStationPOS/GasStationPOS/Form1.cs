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

    }
}
