using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.UI.Constants;

namespace GasStationPOS.UI.MainFormDataSchemas.DataSourceWrappers
{
    /// <summary>
    /// PaymentDataWrapper class contains subtotal, amounttendered, and amountremaining
    /// 
    /// Author: Jason Lau
    /// Date: 27 March 2025
    /// 
    /// </summary>
    public class PaymentDataWrapper : INotifyPropertyChanged
    {
        // UI is automatically updated if these properties in this class is the DataSource in a Binding object, and that Binding object is attached to a UI control
        // PropertyChangedEventHandler is required for automatic UI updating when the properties change
        public event PropertyChangedEventHandler PropertyChanged;

        private decimal subtotal;
        private decimal amountTendered;
        private decimal amountRemaining;
        public decimal Subtotal
        {
            get => subtotal;
            set
            {
                subtotal = value;
                OnPropertyChanged(nameof(Subtotal)); // nameof returns a string of the property's name
            }
        }
        public decimal AmountTendered
        {
            get => amountTendered;
            set
            {
                amountTendered = value;
                OnPropertyChanged(nameof(AmountTendered));
            }
        }
        public decimal AmountRemaining
        {
            get => amountRemaining;
            set
            {
                amountRemaining = value;
                OnPropertyChanged(nameof(AmountRemaining));
            }
        }

        public PaymentDataWrapper()
        {
            Subtotal = PaymentConstants.INITIAL_AMOUNT_DOLLARS;
            AmountTendered = PaymentConstants.INITIAL_AMOUNT_DOLLARS;
            AmountRemaining = PaymentConstants.INITIAL_AMOUNT_DOLLARS;
        }

        /// <summary>
        /// Updates the subtotal and amount remaining data sources based on the given priceChange value.
        /// PriceChange should be:
        /// - Positive value when updating for product addition
        /// - Negative value when updating for product removal
        /// </summary>
        /// <param name="priceChange"></param>
        public void UpdatePaymentRelatedDataSources(decimal priceChange)
        {
            Subtotal        += priceChange;
            AmountRemaining += priceChange;
        }

        /// <summary>
        /// Resets the subtotal and amountRemaining to their initial dollar amount values (0.0m).
        /// </summary>
        public void ResetPaymentRelatedDataSourcesToInitValues()
        {
            Subtotal        = PaymentConstants.INITIAL_AMOUNT_DOLLARS;
            AmountTendered  = PaymentConstants.INITIAL_AMOUNT_DOLLARS;
            AmountRemaining = PaymentConstants.INITIAL_AMOUNT_DOLLARS;
        }

        // IMPORTANT: Need to Call this method in the setter methods of all properties that are the data sources in data bindings with the UI, and PASS IN NAME OF THE PROPERTY THAT CHANGED
        protected void OnPropertyChanged(string propertyName)
        {
            // Invoke the PropertyChanged event handler, passing in the NAME OF THE PROPERTY
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
