using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GasStationPOS.Core.Data.Database.Json.JsonToModelDTOs;
using GasStationPOS.Core.Data.Models.ProductModels;
using GasStationPOS.Core.Data.Models.TransactionModels;
using GasStationPOS.Core.Data.Models.UserModels;
using GasStationPOS.Core.Services;
using GasStationPOS.UI.MainFormDataSchemas.DTOs;

namespace GasStationPOS
{
    /// <summary>
    /// MappingProfiles class creates mappings between a Model and its corresponding DTO, for easier copying of data from the DTO instance to a model instance, and vice versa.
    /// It also ensures the UI data matches the required data in the data model.
    /// Can be used for UI - Model, and Database - Model
    /// Source:
    /// https://docs.automapper.org/en/stable/Configuration.html#profile-instances
    /// </summary>
    class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Map the relevant fields between Product and ProductDTO
            CreateProductMappingProfile();

            // Map the relevant fields between FuelProduct and FuelProductDTO
            CreateFuelProductMappingProfile();

            // Map the relevant fields between RetailProduct and RetailProductDTO
            CreateRetailProductMappingProfile();

            CreateUserMappingProfile();

            CreateTransactionMappingProfile();
        }

        /// <summary>
        /// Maps the relevant fields between Product and ProductDTO
        /// </summary>
        private void CreateProductMappingProfile()
        {
            // Product -> ProductDTO
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductNameDescription, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.UnitPriceDollars, opt => opt.MapFrom(src => src.UnitPriceDollars))
                .Include<RetailProduct, RetailProductDTO>() // mapping inheritance for RetailProduct -> RetailProductDTO
                .Include<FuelProduct, FuelProductDTO>();    // mapping inheritance for FuelProduct -> FuelProductDTO

            // ProductDTO -> Product
            CreateMap<ProductDTO, Product>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductNameDescription))
                .ForMember(dest => dest.UnitPriceDollars, opt => opt.MapFrom(src => src.UnitPriceDollars))
                .Include<RetailProductDTO, RetailProduct>() // mapping inheritance for RetailProductDTO -> RetailProduct
                .Include<FuelProductDTO, FuelProduct>();    // mapping inheritance for FuelProductDTO -> FuelProduct
        }

        /// <summary>
        /// Maps the relevant fields between FuelProduct and FuelProductDTO
        /// </summary>
        private void CreateFuelProductMappingProfile()
        {
            // PriceDollars for FuelProduct (inherited from Product) is the PRICE ($)/LITRE.
            // To display the price of the FuelProduct in cents (¢):
            // will need to manually divide the FuelProductDTO value by 100 to display in ¢

            // FuelProduct -> FuelProductDTO
            CreateMap<FuelProduct, FuelProductDTO>()
                 .ForMember(dest => dest.FuelGrade, opt => opt.MapFrom(src => src.FuelGrade));
            //.ForMember(dest => dest.FuelVolumeLitres, opt => opt.MapFrom(src => src.FuelVolumeLitres))
            //.ForMember(dest => dest.PriceDollars, opt => opt.MapFrom(src => src.PriceDollars)); // <need to manually divide the dto value by 100 to display in ¢>

            // FuelProductDTO -> FuelProduct
            CreateMap<FuelProductDTO, FuelProduct>()
                 .ForMember(dest => dest.FuelGrade, opt => opt.MapFrom(src => src.FuelGrade));
            //.ForMember(dest => dest.FuelVolumeLitres, opt => opt.MapFrom(src => src.FuelVolumeLitres))
            //.ForMember(dest => dest.PriceDollars, opt => opt.MapFrom(src => src.PriceDollars));
        }

        /// <summary>
        /// Maps the relevant fields between RetailProduct and RetailProductDTO
        /// </summary>
        private void CreateRetailProductMappingProfile()
        {
            // RetailProduct -> RetailProductDTO
            CreateMap<RetailProduct, RetailProductDTO>();
            ////     //.ForMember(dest => dest.RetailCategory, opt => opt.MapFrom(src => src.RetailCategory))
            ////     .ForMember(dest => dest.ProductVolumeLitres, opt => opt.MapFrom(src => src.ProductVolumeLitres))
            ////     .ForMember(dest => dest.ProductSizeVariation, opt => opt.MapFrom(src => src.ProductSizeVariation)); // <need to manually divide the dto value by 100 to display in ¢>

            //// RetailProductDTO -> RetailProduct
            CreateMap<RetailProductDTO, RetailProduct>();
            //     //.ForMember(dest => dest.RetailCategory, opt => opt.MapFrom(src => src.RetailCategory))
            //     .ForMember(dest => dest.ProductVolumeLitres, opt => opt.MapFrom(src => src.ProductVolumeLitres))
            //     .ForMember(dest => dest.ProductSizeVariation, opt => opt.MapFrom(src => src.ProductSizeVariation));


            // For creating clones of retail products when adding new items to user cart ===

            // RetailProductDTO -> RetailProductDTO
            CreateMap<RetailProductDTO, RetailProductDTO>()
                 //.ForMember(dest => dest.RetailCategory, opt => opt.MapFrom(src => src.RetailCategory))
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                 .ForMember(dest => dest.ProductNameDescription, opt => opt.MapFrom(src => src.ProductNameDescription))
                 .ForMember(dest => dest.UnitPriceDollars, opt => opt.MapFrom(src => src.UnitPriceDollars))
                 .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                 .ForMember(dest => dest.TotalPriceDollars, opt => opt.MapFrom(src => src.TotalPriceDollars));
                 //.ForMember(dest => dest.ProductVolumeLitres, opt => opt.MapFrom(src => src.ProductVolumeLitres))
                 //.ForMember(dest => dest.ProductSizeVariation, opt => opt.MapFrom(src => src.ProductSizeVariation));
        }

        private void CreateUserMappingProfile()
        {
            // ====== Database DTOs ======

            // User -> UserDatabaseDTO
            CreateMap<User, UserDatabaseDTO>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

            // UserDatabaseDTO -> User
            CreateMap<UserDatabaseDTO, User>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => Enum.Parse(typeof(UserRole), src.Role)));
        }
        
        private void CreateTransactionMappingProfile()
        {
            // ====== Database DTOs ======

            CreateMap<Transaction, TransactionDatabaseDTO>()
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.ToString()))
                .ForMember(dest => dest.TransactionDateTime, opt => opt.MapFrom(src => src.TransactionDateTime.ToString(TransactionConstants.TransactionDatetimeFormat))); // DateTime -> formatted string

            CreateMap<TransactionDatabaseDTO, Transaction>()
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => Enum.Parse(typeof(PaymentMethod), src.PaymentMethod)))
                .ForMember(dest => dest.TransactionDateTime, opt => opt.MapFrom(src => DateTime.ParseExact(src.TransactionDateTime, TransactionConstants.TransactionDatetimeFormat, new CultureInfo("en-CA")))); // formatted string -> DateTime
        }

        // more mappings here later (receipt etc.)

    }
}
