using BlazorApp1.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorApp1.Shared
{
    public class UserAuthStateProvider : AuthenticationStateProvider, IDisposable
    {

        public User CurrentUser { get; private set; } = new();
        //Field
        private readonly UserService _userService;

        public UserAuthStateProvider(UserService userService)
        {
            _userService=userService;
            AuthenticationStateChanged += OnAuthenticationStateChangedAsync;
        }

       /* public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            throw new NotImplementedException();
            //retutn true;
        }*/

        //Login Flow
        public async Task LoginAsync(string username, string password)
        {
            var principal = new ClaimsPrincipal();
            var user = _userService.LookupUserInDatabase(username, password);
            if(user is not null)
            {
                await _userService.PersistUserToBrowserAsync(user);
                principal = user.ToClaimsPrincipal();
            }

            NotifyAuthenticationStateChanged(Task.FromResult( new AuthenticationState(principal)));

        }
        //Logout Flow
        public async Task LogoutAsync()
        {
            await _userService.ClearBrowserUserDataAsync();
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new())));
        }

        //Override{GetAuthenticationStateAsync} method for Revisit Flow
        public override async Task<AuthenticationState> 
            GetAuthenticationStateAsync()
        {
            var principal = new ClaimsPrincipal();
            var user = await _userService.FetchUserFromBrowserAsync();

            if(user is not null)
            {
                var userInDatabase = _userService.LookupUserInDatabase(user.Username, user.Password);

                if(userInDatabase is not null)
                {
                    principal = userInDatabase.ToClaimsPrincipal();
                    CurrentUser = userInDatabase;
                }
            }
            return new(principal);

        }

        private async void 
            OnAuthenticationStateChangedAsync(Task<AuthenticationState> task)
        {
            var authenticationState = await task;
                if(authenticationState is not null)
            {
                CurrentUser = User.FromClaimsPrincipal(authenticationState.User);
            }
        }
        public void Dispose() => AuthenticationStateChanged -= OnAuthenticationStateChangedAsync;
    }
}
