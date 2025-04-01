using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Models.ProductModels;

namespace GasStationPOS.Core.Data.Repositories.Product
{
    public class FuelProductRepository : IFuelProductRepository
    {
        public IEnumerable<FuelProduct> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FuelProduct> GetByValue(string fuelProductName)
        {
            throw new NotImplementedException();
        }
    }
}
