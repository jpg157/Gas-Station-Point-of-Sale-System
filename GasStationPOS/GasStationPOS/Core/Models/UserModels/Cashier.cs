using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationPOS.Core.Models.UserModels
{
    class Cashier : User
    {
        public Cashier()
        {
            Role = UserRole.CASHIER;
        }
    }
}
