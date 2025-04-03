using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Models.ProductModels;
using GasStationPOS.UI.Constants;

namespace GasStationPOS.UI.MainFormDataSchemas.DataSourceWrappers
{
    /// <summary>
    /// FuelInputDataWrapper class stores data from user input fuel products. It should be data binded with the corresponding UI controls.
    /// Stores variables:
    /// - FUEL PUMP NUMBER
    /// - ENTERED FUEL PRICE (Dollars / Litre)
    /// - FUEL GRADE
    /// - FUEL QUANTITY (Litres)
    /// 
    /// When fuel price or fuel grade is changed, Fuel Quantity is automatically recalculated to account for the value change.
    /// </summary>
    public class FuelInputDataWrapper : INotifyPropertyChanged
    {
        private static readonly int DEFAULT_PUMP_NUMBER_VALUE = 0; // non-existent pump number value

        // UI is automatically updated if these properties in this class is the DataSource in a Binding object, and that Binding object is attached to a UI control
        // PropertyChangedEventHandler is required for automatic UI updating when the properties change
        public event PropertyChangedEventHandler PropertyChanged;

        private int         fuelPumpNumber;
        private string      fuelPumpNumberStr;
        private FuelGrade   enteredFuelGrade;
        private decimal     enteredFuelPrice;
        private decimal     fuelQuantityLitres;

        public int FuelPumpNumber
        {
            get => fuelPumpNumber;
            set
            {
                fuelPumpNumber = value;
                OnPropertyChanged(nameof(FuelPumpNumber)); // nameof returns a string of the property's name

                // update string representation as well
                FuelPumpNumberStr = $"PUMP {fuelPumpNumber}"; // update FuelPumpNumberStr property
            }
        }
        public string FuelPumpNumberStr
        {
            get => fuelPumpNumberStr;
            set
            {
                fuelPumpNumberStr = value;
                OnPropertyChanged(nameof(FuelPumpNumberStr));
            }
        }
        public FuelGrade EnteredFuelGrade
        {
            get => enteredFuelGrade;
            set
            {
                enteredFuelGrade = value;

                // Update resulting fuel quantity when the fuel grade changes
                UpdateFuelQuantityForFuelProduct();

                OnPropertyChanged(nameof(EnteredFuelGrade));
            }
        }
        public decimal EnteredFuelPrice
        {
            get => enteredFuelPrice;
            set
            {
                enteredFuelPrice = value;

                // Update resulting fuel quantity when the fuel price changes
                UpdateFuelQuantityForFuelProduct();

                OnPropertyChanged(nameof(EnteredFuelPrice));
            }
        }
        public decimal FuelQuantityLitres
        {
            get => fuelQuantityLitres;
            private set
            {
                fuelQuantityLitres = value;
                OnPropertyChanged(nameof(FuelQuantityLitres));
            }
        }

        /// <summary>
        /// Constructor sets the EnteredFuelGrade, EnteredFuelPrice, and FuelQuantity to their default values.
        /// </summary>
        public FuelInputDataWrapper()
        {
            EnteredFuelGrade    = FuelGrade.REGULAR;
            EnteredFuelPrice    = PaymentConstants.INITIAL_AMOUNT_DOLLARS;
            FuelQuantityLitres  = QuantityConstants.DEFAULT_FUEL_PRODUCT_QUANTITY_VALUE;
        }

        /// <summary>
        /// Sets the FuelQuantity data source based on FuelGrade and the EnteredFuelPrice value.
        /// <param name="priceChange"></param>
        private void UpdateFuelQuantityForFuelProduct()
        {
            // get fuel price ($/L) for the currently entered grade value
            decimal fuelPriceDollarsPerLitre = FuelGradeUtils.GetFuelPrice(this.EnteredFuelGrade);

            // resulting fuel quantity: $ / ($ / L)
            FuelQuantityLitres = EnteredFuelPrice / fuelPriceDollarsPerLitre;
        }

        /// <summary>
        /// Resets the FuelPumpNumber, EnteredFuelGrade, EnteredFuelPrice, and FuelQuantity to their default values.
        /// </summary>
        public void ResetPaymentRelatedDataSourcesToInitValues()
        {
            FuelPumpNumber      = DEFAULT_PUMP_NUMBER_VALUE;
            EnteredFuelGrade    = FuelGrade.REGULAR;
            EnteredFuelPrice    = PaymentConstants.INITIAL_AMOUNT_DOLLARS;
            FuelQuantityLitres  = QuantityConstants.DEFAULT_FUEL_PRODUCT_QUANTITY_VALUE;
        }

        // IMPORTANT: Need to Call this method in the setter methods of all properties that are the data sources in data bindings with the UI, and PASS IN NAME OF THE PROPERTY THAT CHANGED
        protected void OnPropertyChanged(string propertyName)
        {
            // Invoke the PropertyChanged event handler, passing in the NAME OF THE PROPERTY
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
