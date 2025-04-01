using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationPOS.Core.Services.Auth
{
    public interface IAuthenticationService
    {
        bool Authenticate(string username, string userPwd);
    }
}
