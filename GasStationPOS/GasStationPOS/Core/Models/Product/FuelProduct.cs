using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationPOS.Core.Models.Product
{

    enum FuelType
    {
        GASOLINE,
        DIESEL
    }

    class FuelProduct : Product
    {
        [Required]
        public FuelType FuelType { get; set; }

        [Required]
        [Range(0.0, 10000.0, ErrorMessage = "Fuel volume must be between 0.0 and 10000.0 L")]
        public decimal FuelVolumeLitres { get; set; }

        public override string ToString()
        {
            string strRet = $@"
                {base.ToString()}
                Fuel Type: {FuelType}
                Fuel Volume Litres: {FuelVolumeLitres}
                ";
            return strRet;
        }
    }
}
