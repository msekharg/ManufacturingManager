using System.Security.Claims;
using ManufacturingManager.Web.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;

namespace ManufacturingManager.Web.Services
{
    /// <summary CascadingAuthenticationState="in App.razor" />
    public class CustomAuthenticationStateProvider(UserLogged currentUser) : AuthenticationStateProvider
    {

        private UserLogged  _currentUser = currentUser;

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            ClaimsIdentity claimsIdentity;
            if (!string.IsNullOrEmpty(_currentUser.UserName) && !string.IsNullOrEmpty(_currentUser.Role))
            {
                var claims = new List<Claim>
                {
                    new (ClaimTypes.Name, _currentUser.UserName),
                    new(ClaimTypes.Role, _currentUser.Role)
                };
                claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
           
            }
            else
            {
                var claims = new List<Claim>
                {
                    new (ClaimTypes.Name, ""),
                    new (ClaimTypes.Role, "")
                };
                claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        
            }
        
            var principal = new ClaimsPrincipal(claimsIdentity);
        
            return await Task.FromResult(new AuthenticationState(principal));
        
        
        }
        
        public void MarkUserAsAuthenticated(UserLogged  userLogged)
        {
            var claims = new List<Claim>
            {
                new (ClaimTypes.Name, userLogged.UserName),
                new (ClaimTypes.Role, userLogged.Role),
                new ("userId", userLogged.UserId.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //var identity = new ClaimsIdentity(new[]
            //{
            //    new Claim(ClaimTypes.Name, userLogged.UserName ),
            //    new Claim(ClaimTypes.Role , userLogged.Role ),
            //}, "apiauth_type");
        
            var user = new ClaimsPrincipal(claimsIdentity);
        
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
        
        public void MarkUserAsLoggedOut()
        {
        
            _currentUser = new();
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        
        }
    }
}
