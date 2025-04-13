using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GasStationPOS.Core.Data.Database.Json.JsonFileSchemas;
using GasStationPOS.Core.Data.Models.ProductModels;
using GasStationPOS.Core.Data.Models.TransactionModels;
using GasStationPOS.Core.Data.Models.UserModels;
using GasStationPOS.Core.Services;
using GasStationPOS.UI.MainFormDataSchemas.DTOs;

namespace GasStationPOS
{
    /// <summary>
    /// MappingProfiles class creates mappings between a Model and its corresponding DTO, for easier copying of instance fields from the DTO instance to a model instance, and vice versa.
    /// Addionally, it is used for creating deep copies of model or dto objects.
    /// Can be used for mapping between UI DTOs - Model, and JSON file DTOs - Model
    /// Source:
    /// https://docs.automapper.org/en/stable/Configuration.html#profile-instances
    /// 
    /// Author: Jason Lau
    /// Date: 27 March 2025
    /// 
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

            // Map the relevant fields between BarcodeRetailProduct and BarcodeRetailProductDTO
            CreateBarcodeRetailProductMappingProfile();

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
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.UnitPriceDollars, opt => opt.MapFrom(src => src.UnitPriceDollars))
                .Include<RetailProduct, RetailProductDTO>() // mapping inheritance for RetailProduct -> RetailProductDTO
                .Include<FuelProduct, FuelProductDTO>()     // mapping inheritance for FuelProduct -> FuelProductDTO
                .Include<BarcodeRetailProduct, BarcodeRetailProductDTO>();    // mapping inheritance for BarcodeRetailProduct -> BarcodeRetailProductDTO

            // ProductDTO -> Product
            CreateMap<ProductDTO, Product>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.UnitPriceDollars, opt => opt.MapFrom(src => src.UnitPriceDollars))
                .Include<RetailProductDTO, RetailProduct>() // mapping inheritance for RetailProductDTO -> RetailProduct
                .Include<FuelProductDTO, FuelProduct>()     // mapping inheritance for FuelProductDTO -> FuelProduct
                .Include<BarcodeRetailProductDTO, BarcodeRetailProduct>();    // mapping inheritance for BarcodeRetailProductDTO -> BarcodeRetailProduct

            // ProductDTO -> Product (For deep copy)
            CreateMap<ProductDTO, ProductDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.UnitPriceDollars, opt => opt.MapFrom(src => src.UnitPriceDollars))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.TotalPriceDollars, opt => opt.MapFrom(src => src.TotalPriceDollars))
                .Include<RetailProductDTO, RetailProductDTO>() // mapping inheritance for RetailProductDTO -> RetailProductDTO
                .Include<FuelProductDTO, FuelProductDTO>()    // mapping inheritance for FuelProductDTO -> FuelProductDTO
                .Include<BarcodeRetailProductDTO, BarcodeRetailProductDTO>();    // mapping inheritance for BarcodeRetailProductDTO -> BarcodeRetailProductDTO
        }

        /// <summary>
        /// Maps the relevant fields between FuelProduct and FuelProductDTO
        /// </summary>
        private void CreateFuelProductMappingProfile()
        {
            // FuelProduct -> FuelProductDTO
            CreateMap<FuelProduct, FuelProductDTO>()
                .ForMember(dest => dest.FuelGrade, opt => opt.MapFrom(src => src.FuelGrade))
                .ForMember(dest => dest.PumpNumber, opt => opt.MapFrom(src => src.PumpNumber));

            // FuelProductDTO -> FuelProduct
            CreateMap<FuelProductDTO, FuelProduct>()
                .ForMember(dest => dest.FuelGrade, opt => opt.MapFrom(src => src.FuelGrade))
                .ForMember(dest => dest.PumpNumber, opt => opt.MapFrom(src => src.PumpNumber));
            

            // For creating clones of fuel products when adding new items to user cart ===

            // FuelProductDTO -> FuelProductDTO
            CreateMap<FuelProductDTO, FuelProductDTO>()
                .ForMember(dest => dest.FuelGrade, opt => opt.MapFrom(src => src.FuelGrade))
                .ForMember(dest => dest.PumpNumber, opt => opt.MapFrom(src => src.PumpNumber));
        }

        /// <summary>
        /// Maps the relevant fields between RetailProduct and RetailProductDTO
        /// </summary>
        private void CreateRetailProductMappingProfile()
        {
            // RetailProduct -> RetailProductDTO
            CreateMap<RetailProduct, RetailProductDTO>();
                //.ForMember(dest => dest.RetailCategory, opt => opt.MapFrom(src => src.RetailCategory))

            // RetailProductDTO -> RetailProduct
            CreateMap<RetailProductDTO, RetailProduct>();
                 //.ForMember(dest => dest.RetailCategory, opt => opt.MapFrom(src => src.RetailCategory))


            // For creating clones of retail products when adding new items to user cart ===

            // RetailProductDTO -> RetailProductDTO
            CreateMap<RetailProductDTO, RetailProductDTO>();
                 //.ForMember(dest => dest.RetailCategory, opt => opt.MapFrom(src => src.RetailCategory))
        }

        private void CreateBarcodeRetailProductMappingProfile()
        {
            // BarcodeRetailProduct -> BarcodeRetailProductDTO
            CreateMap<BarcodeRetailProduct, BarcodeRetailProductDTO>()
                .ForMember(dest => dest.BarcodeId, opt => opt.MapFrom(src => src.BarcodeId));

            // BarcodeRetailProductDTO -> BarcodeRetailProduct
            CreateMap<BarcodeRetailProductDTO, BarcodeRetailProduct>()
                    .ForMember(dest => dest.BarcodeId, opt => opt.MapFrom(src => src.BarcodeId));

            // For creating clones of barcode retail products when adding new items to user cart ===

            // BarcodeRetailProductDTO -> BarcodeRetailProductDTO
            CreateMap<BarcodeRetailProductDTO, BarcodeRetailProductDTO>()
                    .ForMember(dest => dest.BarcodeId, opt => opt.MapFrom(src => src.BarcodeId));
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
    }
}
