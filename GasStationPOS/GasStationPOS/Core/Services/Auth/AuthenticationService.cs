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
    public class AuthenticationService : IAuthenticationService
    {
        readonly IUserRepository userRepository;
        public AuthenticationService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public bool Authenticate(string username, string userPwd)
        {
            bool userExistsInDb = false;

            string hashedPassword = PasswordEncryption.HashPassword(userPwd);

            User user = userRepository.Get(username, hashedPassword);

            if (user != null)
            {
                userExistsInDb = true;
            }

            return userExistsInDb;
        }
    }
}
