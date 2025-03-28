using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoMapper;
using GasStationPOS.Core.Repositories.Product;
using GasStationPOS.Core.Services.Inventory;
using GasStationPOS.Presenters;
using GasStationPOS.UI.ViewDataTransferObjects;
using GasStationPOS.UI.Views;

namespace GasStationPOS
{
    internal static class Program
    {

        // Public global mapper object reference for DTO Model field mappings
        // Presenter classes that uses this should use dependency injection to inject this instance into the class
        public static IMapper GlobalMapper { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Configure AutoMapper for mapping DTOs to and from Models
            // Source:
            // https://docs.automapper.org/en/stable/Getting-started.html
            // https://docs.automapper.org/en/stable/Configuration.html#profile-instances
            var configuration = new MapperConfiguration(cfg =>
            {
                // Add the profile to the mapper configuration
                cfg.AddProfile<MappingProfiles>();
            });
            // Initialize and create the global Mapper object instance with the DTO Model field mappings
            GlobalMapper = configuration.CreateMapper();



            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Dependency Injection
            IMainView mainView                                  = new MainForm();
            IRetailProductRepository retailProductRepository    = new RetailProductRepository();
            IInventoryService inventoryService                  = new InventoryService(retailProductRepository);
            MainPresenter mainPresenter                         = new MainPresenter(mainView, inventoryService);

            // Cast the existing main view into Form
            // (this works because mainView interface has a reference to MainForm via L.S.Principle)
            // Pass into Application.Run
            MainForm mainForm = mainView as MainForm;

            Application.Run(mainForm);
        }
    }
}
