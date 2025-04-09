using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationPOS.UI.MainFormDataSchemas.DTOs
{
    public class BarcodeRetailProductDTO : RetailProductDTO
    {
        [Required(ErrorMessage = "BarcodeId is required")]
        public string BarcodeId { get; set; }
    }
}
