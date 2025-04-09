using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GasStationPOS.Core.Data.Database.Json.JsonToModelDTOs;
using GasStationPOS.Core.Data.Models.UserModels;
using GasStationPOS.Core.Database.Json;

namespace GasStationPOS.Core.Data.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        /// <summary>
        /// Gets the user with the entered username and hashed password from the json database.
        /// Returns the User object if found, otherwise returns null.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="hashedPassword"></param>
        /// <returns></returns>
        public User Get(string username, string hashedPassword)
        {
            UserDatabaseDTO userDbDTO;
            User            user;

            string jsonData = File.ReadAllText(JsonDBConstants.USERS_JSON_FILE_PATH);

            using (JsonDocument document = JsonDocument.Parse(jsonData))
            {
                JsonElement root = document.RootElement;
                JsonElement usersElement = root.GetProperty("Users");

                JsonElement matchingUserElement = usersElement.EnumerateArray().FirstOrDefault(userElement =>
                    userElement.GetProperty("Username").GetString() == username &&
                    userElement.GetProperty("Password").GetString() == hashedPassword);

                // If user was found:

                // JsonElement ValueKind property is set to Undefined
                // when the JsonElement does not contain any valid data
                // (an empty JsonElement is returned when no match is found in the FirstOrDefault query)
                if (matchingUserElement.ValueKind != JsonValueKind.Undefined) // if the json element returned is NOT empty and contains data
                {
                    string userJsonData = matchingUserElement.GetRawText();

                    // For this to work, the user json schema has to match the UserDatabaseDTO class structure
                    userDbDTO = JsonSerializer.Deserialize<UserDatabaseDTO>(userJsonData);

                    // Map the fields in the UserDatabaseDTO -> User
                    user = Program.GlobalMapper.Map<User>(userDbDTO);

                    return user;
                }

                // If user was not found:
                return null;

            }
        }
    }
}
