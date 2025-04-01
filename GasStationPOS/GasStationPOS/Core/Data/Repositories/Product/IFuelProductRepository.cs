using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Models.ProductModels;

namespace GasStationPOS.Core.Data.Repositories.Product
{
    public interface IFuelProductRepository
    {
        void Create(FuelProduct fuelProductModel);
        void Update(FuelProduct fuelProductModel);
        void Delete(int productId);
        IEnumerable<FuelProduct> GetAll();
        IEnumerable<FuelProduct> GetByValue(string fuelProductName); // for searching product info
    }
}
