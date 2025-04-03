using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;

namespace GasStationPOS.UI.UserControls.Payment
{
    partial class CardPaymentUserControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.keyEnterBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // keyEnterBtn
            // 
            this.keyEnterBtn.BackColor = System.Drawing.Color.Gray;
            this.keyEnterBtn.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.keyEnterBtn.Font = new System.Drawing.Font("Segoe UI", 18F);
            this.keyEnterBtn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.keyEnterBtn.Location = new System.Drawing.Point(0, 130);
            this.keyEnterBtn.Name = "keyEnterBtn";
            this.keyEnterBtn.Size = new System.Drawing.Size(370, 68);
            this.keyEnterBtn.TabIndex = 0;
            this.keyEnterBtn.Text = "Key Enter";
            this.keyEnterBtn.UseVisualStyleBackColor = false;
            this.keyEnterBtn.Click += new System.EventHandler(this.keyEnterBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 18F);
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(16, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(335, 64);
            this.label1.TabIndex = 1;
            this.label1.Text = "Customer insert, swipe, tap, or\r\n                scan card";
            // 
            // CardPaymentUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Firebrick;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.keyEnterBtn);
            this.Name = "CardPaymentUserControl";
            this.Size = new System.Drawing.Size(370, 198);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Button keyEnterBtn;
        private Label label1;
    }
}
