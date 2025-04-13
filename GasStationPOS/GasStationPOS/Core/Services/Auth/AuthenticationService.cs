using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Models.UserModels;
using GasStationPOS.Core.Data.Repositories.UserRepository;
using GasStationPOS.Core.Services.Utils;

namespace GasStationPOS.Core.Services.Auth
{
    /// <summary>
    /// Service for authenticating a user.
    /// 
    /// Author: Jason Lau
    /// 
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        readonly IUserRepository userRepository;
        public AuthenticationService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Checks if the user with username and userPwd exist.
        /// Returns true if it does, otherwise returns false.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="userPwd"></param>
        /// <returns></returns>
        public bool Authenticate(string username, string userPwd)
        {
            bool userExistsInJsonFile = false;

            string hashedPassword = PasswordEncryption.HashPassword(userPwd);

            User user = userRepository.Get(username, hashedPassword);

            if (user != null)
            {
                userExistsInJsonFile = true;
            }

            return userExistsInJsonFile;
        }
    }
}
