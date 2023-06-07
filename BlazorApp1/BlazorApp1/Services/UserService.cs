using BlazorApp1.Shared;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace BlazorApp1.Services
{
    public class UserService
    {
        //Inject the Browser data storage service class
        private readonly ProtectedLocalStorage _protectedLocalStorage;
        public UserService(ProtectedLocalStorage protectedLocalStorage)
        {
            _protectedLocalStorage=protectedLocalStorage;
        }

        //(method)Look up user in the db
        public User? LookupUserInDatabase(string username, string password)
        {
            var usersFromDatabase = new List<User>()
            {
                new()
                {
                    Username = "blazorschool",
                    Password = "blazorschool"
                }
            };
            var foundUser = usersFromDatabase.SingleOrDefault(u => u.Username == username && u.Password == password);

            return foundUser;
        }

        //(method)persist user in browser data storage
        private readonly string _MybrowserStorageKey = "blazorSchoolIdentity";
        
        public async Task PersistUserToBrowserAsync(User user)
        {
            string userJson = JsonConvert.SerializeObject(user);
            await _protectedLocalStorage.SetAsync(_MybrowserStorageKey, userJson);
        }

        //(Method)Fetch user from browser data storage
        public async Task<User?>
            FetchUserFromBrowserAsync()
        {
            try
            {
                var storedUserResult = await _protectedLocalStorage.GetAsync<string>(_MybrowserStorageKey);
                if(storedUserResult.Success && !string.IsNullOrEmpty(storedUserResult.Value))
                {
                    var user = JsonConvert.DeserializeObject<User>(storedUserResult.Value);
                    return user;
                }
            }
            catch(InvalidOperationException)
            {

            }

            return null;
        }

        //method to clear user from browser data storage
        public async Task ClearBrowserUserDataAsync() =>
            await _protectedLocalStorage.DeleteAsync(_MybrowserStorageKey);

    }
}
