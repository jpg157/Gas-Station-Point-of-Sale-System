using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Models.ProductModels;

namespace GasStationPOS.Core.Data.Repositories.Product
{
    /// <summary>
    /// Interface to repository class that does crud operations on the data source.
    /// </summary>
    public interface IRetailProductRepository
    {
        /// <summary>
        /// Gets all retail products from the data storage srource.
        /// </summary>
        /// <returns></returns>
        IEnumerable<RetailProduct> GetAll();

        //       void Create(RetailProduct retailProductModel);
        //       void Update(RetailProduct retailProductModel);
        //       void Delete(int productId);
        //       IEnumerable<RetailProduct> GetByValue(string fuelProductName); // for searching product info
    }
}
