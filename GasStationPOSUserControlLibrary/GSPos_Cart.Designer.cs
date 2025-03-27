namespace GasStationPOSUserControlLibrary
{
    partial class GSPos_Cart
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
            this.pnlCart = new System.Windows.Forms.Panel();
            this.pnlSubtotal = new System.Windows.Forms.Panel();
            this.labelRemaining = new System.Windows.Forms.Label();
            this.labelTendered = new System.Windows.Forms.Label();
            this.labelSubtotal = new System.Windows.Forms.Label();
            this.remainingText = new System.Windows.Forms.Label();
            this.TenderedText = new System.Windows.Forms.Label();
            this.SubtotalText = new System.Windows.Forms.Label();
            this.listCart = new System.Windows.Forms.ListBox();
            this.labalCartHeader = new System.Windows.Forms.Label();
            this.pnlCart.SuspendLayout();
            this.pnlSubtotal.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCart
            // 
            this.pnlCart.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlCart.Controls.Add(this.pnlSubtotal);
            this.pnlCart.Controls.Add(this.listCart);
            this.pnlCart.Controls.Add(this.labalCartHeader);
            this.pnlCart.Location = new System.Drawing.Point(0, 0);
            this.pnlCart.Name = "pnlCart";
            this.pnlCart.Size = new System.Drawing.Size(441, 583);
            this.pnlCart.TabIndex = 4;
            // 
            // pnlSubtotal
            // 
            this.pnlSubtotal.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlSubtotal.BackColor = System.Drawing.Color.Tan;
            this.pnlSubtotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSubtotal.Controls.Add(this.labelRemaining);
            this.pnlSubtotal.Controls.Add(this.labelTendered);
            this.pnlSubtotal.Controls.Add(this.labelSubtotal);
            this.pnlSubtotal.Controls.Add(this.remainingText);
            this.pnlSubtotal.Controls.Add(this.TenderedText);
            this.pnlSubtotal.Controls.Add(this.SubtotalText);
            this.pnlSubtotal.Location = new System.Drawing.Point(0, 486);
            this.pnlSubtotal.Name = "pnlSubtotal";
            this.pnlSubtotal.Size = new System.Drawing.Size(441, 97);
            this.pnlSubtotal.TabIndex = 2;
            // 
            // labelRemaining
            // 
            this.labelRemaining.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRemaining.Location = new System.Drawing.Point(303, 55);
            this.labelRemaining.Name = "labelRemaining";
            this.labelRemaining.Size = new System.Drawing.Size(137, 44);
            this.labelRemaining.TabIndex = 5;
            this.labelRemaining.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelTendered
            // 
            this.labelTendered.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTendered.Location = new System.Drawing.Point(308, 27);
            this.labelTendered.Name = "labelTendered";
            this.labelTendered.Size = new System.Drawing.Size(128, 28);
            this.labelTendered.TabIndex = 4;
            this.labelTendered.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelSubtotal
            // 
            this.labelSubtotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSubtotal.Location = new System.Drawing.Point(299, 4);
            this.labelSubtotal.Name = "labelSubtotal";
            this.labelSubtotal.Size = new System.Drawing.Size(137, 23);
            this.labelSubtotal.TabIndex = 3;
            this.labelSubtotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // remainingText
            // 
            this.remainingText.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.remainingText.Location = new System.Drawing.Point(191, 55);
            this.remainingText.Name = "remainingText";
            this.remainingText.Size = new System.Drawing.Size(122, 44);
            this.remainingText.TabIndex = 2;
            this.remainingText.Text = "Remaining";
            this.remainingText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TenderedText
            // 
            this.TenderedText.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TenderedText.Location = new System.Drawing.Point(193, 27);
            this.TenderedText.Name = "TenderedText";
            this.TenderedText.Size = new System.Drawing.Size(100, 28);
            this.TenderedText.TabIndex = 1;
            this.TenderedText.Text = "Tendered";
            this.TenderedText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SubtotalText
            // 
            this.SubtotalText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SubtotalText.Location = new System.Drawing.Point(192, 4);
            this.SubtotalText.Name = "SubtotalText";
            this.SubtotalText.Size = new System.Drawing.Size(100, 23);
            this.SubtotalText.TabIndex = 0;
            this.SubtotalText.Text = "Subtotal";
            this.SubtotalText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // listCart
            // 
            this.listCart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listCart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listCart.FormattingEnabled = true;
            this.listCart.ItemHeight = 15;
            this.listCart.Location = new System.Drawing.Point(0, 32);
            this.listCart.Name = "listCart";
            this.listCart.Size = new System.Drawing.Size(441, 469);
            this.listCart.TabIndex = 1;
            // 
            // labalCartHeader
            // 
            this.labalCartHeader.BackColor = System.Drawing.Color.Tan;
            this.labalCartHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labalCartHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labalCartHeader.Location = new System.Drawing.Point(0, 0);
            this.labalCartHeader.Name = "labalCartHeader";
            this.labalCartHeader.Size = new System.Drawing.Size(441, 32);
            this.labalCartHeader.TabIndex = 0;
            this.labalCartHeader.Text = "Description                                                                    Qt" +
    "y              Price       Total";
            this.labalCartHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // GSPos_Cart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlCart);
            this.Name = "GSPos_Cart";
            this.Size = new System.Drawing.Size(441, 583);
            this.pnlCart.ResumeLayout(false);
            this.pnlSubtotal.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlCart;
        private System.Windows.Forms.Panel pnlSubtotal;
        private System.Windows.Forms.Label labelRemaining;
        private System.Windows.Forms.Label labelTendered;
        private System.Windows.Forms.Label labelSubtotal;
        private System.Windows.Forms.Label remainingText;
        private System.Windows.Forms.Label TenderedText;
        private System.Windows.Forms.Label SubtotalText;
        private System.Windows.Forms.ListBox listCart;
        private System.Windows.Forms.Label labalCartHeader;
    }
}
