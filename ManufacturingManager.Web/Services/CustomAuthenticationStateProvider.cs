using System.Security.Claims;
using ManufacturingManager.Core;
using ManufacturingManager.Core.Repositories;
using ManufacturingManager.Web.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace ManufacturingManager.Web.Services
{
    /// <summary CascadingAuthenticationState="in App.razor" />
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private UserLogged _currentUser;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUsersRepository _usersRepository;
        private readonly ILoggingService _loggingService;
        private NavigationManager _navigationManager;

        public CustomAuthenticationStateProvider(UserLogged currentUser, IHttpContextAccessor httpContextAccessor, IUsersRepository usersRepository, ILoggingService loggingService, NavigationManager navigationManager)
        {
            _currentUser = currentUser;
            _httpContextAccessor = httpContextAccessor;
            _usersRepository = usersRepository;
            _loggingService = loggingService;
            _navigationManager = navigationManager;
        }
        
        private ClaimsPrincipal CurrentUser { get; set; }

        private ClaimsPrincipal GetUser(string userName, string id, string role)
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.Sid, id)
            }, "Authentication type");
            return new ClaimsPrincipal(identity);
        }
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = _httpContextAccessor?.HttpContext?.User;
        
            _loggingService.Debug($"In CustomAuthenticationStateProvider: {user?.Identity?.Name}");
            try
            {
                if (_httpContextAccessor.HttpContext != null && user != null && user.Identity.IsAuthenticated)
                {
                    var userName = user.Identities.FirstOrDefault().Name;

                    if (!string.IsNullOrEmpty(userName))
                    {
                        var userInfo = _usersRepository.Users(userName);
                        if (userInfo.IsAdministrator)
                            _currentUser.Role = "Administrator";
                        else if (userInfo.IsUser)
                            _currentUser.Role = "User";
                        else if (userInfo.IsViewer)
                            _currentUser.Role = "Viewer";

                        _currentUser.UserName = userName;
                        _currentUser.UserId = userInfo.UserId;
                        _currentUser.Email = userInfo.Email;

                        var claims = new List<Claim>
                        {
                            new(ClaimTypes.Name, _currentUser.UserName),
                            new(ClaimTypes.Role, _currentUser.Role)
                        };

                        var currentUserRoles = user.Claims.Where(x => x.Type == ClaimTypes.Role).Select(c => c.Value);
                        var addNewRoles = claims.Where(x => x.Type == ClaimTypes.Role).Select(c => c.Value).ToList()
                            .Except(currentUserRoles);

                        if (addNewRoles.Any())
                        {
                            user.AddIdentity(new ClaimsIdentity(new List<Claim>()
                                { new(ClaimTypes.Role, _currentUser.Role) }));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _loggingService.Error($"Exception happened during CustomAuthenticationStateProvider.GetAuthenticationStateAsync: {ex.Message}, Stack: {ex.InnerException}");
                _navigationManager.NavigateTo("/Error");
            }

            if(user == null)
                user = new ClaimsPrincipal();
            _loggingService.Debug($"In CustomAuthenticationStateProvider: with user null");
            return Task.FromResult(new AuthenticationState(user));
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
            
            var user = new ClaimsPrincipal(claimsIdentity);
            GetAuthenticationStateAsync();
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
        
        public void MarkUserAsLoggedOut()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity());
            _httpContextAccessor.HttpContext.User = user;
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        
        }
    }
}
