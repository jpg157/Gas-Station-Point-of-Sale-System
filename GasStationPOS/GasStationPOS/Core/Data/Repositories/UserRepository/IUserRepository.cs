using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Models.UserModels;

namespace GasStationPOS.Core.Data.Repositories.UserRepository
{
    public interface IUserRepository
    {
        /// <summary>
        /// Gets the user with a matching entered username and hashed password from the json database.
        /// Returns the User object if found, otherwise returns null.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="hashedPassword"></param>
        /// <returns></returns>
        User Get(string username, string hashedPassword);
    }
}
