using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoMapper;
using GasStationPOS.Core.Data.Repositories.Product;
using GasStationPOS.Core.Data.Repositories.TransactionRepository;
using GasStationPOS.Core.Data.Repositories.UserRepository;
using GasStationPOS.Core.Services.Auth;
using GasStationPOS.Core.Services.Inventory;
using GasStationPOS.Core.Services.Transaction_Payment;

namespace GasStationPOS
{
    /// <summary>
    /// Entry point of the program. 
    /// Creates an automapper that maps objects of different types to each other (simplifies copying of data)
    /// Uses dependency injection to initialize all required data base access repositories, 
    /// and services that the main form uses.
    /// 
    /// Author: Mansib Talukder
    /// Author: Jason Lau
    /// Author: Vincent Fung
    /// Date: 19 March 2025
    /// </summary>
    public static class Program
    {

        // Public global mapper object reference for DTO Model field mappings
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

            // ====== Dependency Injection ======

            // Data access layer
            IRetailProductRepository    retailProductRepository = new RetailProductRepository();
            IUserRepository             userRepository          = new UserRepository();
            ITransactionRepository      transactionRepository   = new TransactionRepository();

            // Services layer
            IInventoryService           inventoryService        = new InventoryService(retailProductRepository);
            ITransactionService         transactionService      = new TransactionService(transactionRepository);
            IAuthenticationService      authenticationService   = new AuthenticationService(userRepository);

            // UI layer
            MainForm mainForm = new MainForm(inventoryService, transactionService, authenticationService);

            Application.Run(mainForm);
        }
    }
}
