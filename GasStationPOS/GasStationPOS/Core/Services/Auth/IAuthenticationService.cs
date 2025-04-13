using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationPOS.Core.Services.Auth
{
    /// <summary>
    /// Interface to service for authenticating a user.
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Checks if the user with username and userPwd exist.
        /// Returns true if it does, otherwise returns false.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="userPwd"></param>
        /// <returns></returns>
        bool Authenticate(string username, string userPwd);
    }
}
