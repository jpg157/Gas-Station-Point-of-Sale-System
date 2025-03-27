using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationPOSUserControlLibrary
{
    public class CartItem
    {
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }

        public CartItem(string description, decimal quantity, decimal price, decimal total)
        {
            Description = description;
            Quantity = quantity;
            Price = price;
            TotalPrice = total;
        }

        // Override ToString() to display the item in the list
        public override string ToString()
        {
            string spacing;
            if (Description.Length < 12)
                spacing = "\t\t\t\t"; // Extra tab for very short names
            else if (Description.Length < 24)
                spacing = "\t\t\t";   // Three tabs for regular names
            else
                spacing = "\t\t";     // Spacing for longer names

            return $"{Description,-20}{spacing}{Quantity,-5}\t{Price,-8:F2}\t{TotalPrice,-8:F2}";
        }
    }
}
