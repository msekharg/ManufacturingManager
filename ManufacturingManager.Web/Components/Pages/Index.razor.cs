using System.Reflection;
using System.Security.Claims;
using ManufacturingManager.Core;
using ManufacturingManager.Web.Data;
using ManufacturingManager.Web.Services;
using Microsoft.AspNetCore.Components.Forms;

namespace ManufacturingManager.Web.Components.Pages
{
    public partial class Index  
    {
        public EditContext _editContext = default!;
        readonly UserLogged _login = new();

        protected override async Task OnInitializedAsync()
        {
            LoggingService.Debug("Inside Index start");
            try
            {
                var buttonClicked = ProtectedSessionStorage.GetAsync<bool>("isButtonClicked");
                var user = await _authenticationStateProvider.GetAuthenticationStateAsync();
                var userName = user?.User.Identities?.FirstOrDefault()?.Name;
                LoggingService.Debug($"Username is {userName}");
                if (user?.User.Identity != null && !string.IsNullOrEmpty(userName) && user?.User.Identity.IsAuthenticated == true)
                {
                    LoggingService.Debug($"User logged in is {userName}");
                    _navigationManager.NavigateTo("/Home");
                }
                else
                {
                    _navigationManager.NavigateTo("/ErrorPage");
                }
            }
            catch (Exception ex)
            {
                LoggingService.Error($"Exception happened during login message - {ex.Message}");
            }
        }

        protected void Login()
        {
            ProtectedSessionStorage.SetAsync("isLoginClicked", true);
           _navigationManager.NavigateTo("MicrosoftIdentity/Account/SignIn", true);
        }
        
        private async Task SetUpIdentityForUser(int userId)
    {
        User u = _usersRepository.Users(userId);
        if (u.UserId == 0 || !u.IsActive)
        {
            if (u.UserId == 0)
            {
                 LoggingService.Debug(@"User " + userId.ToString() + @" does not exists in Case Management portal!");
            }
            else
            {
                 LoggingService.Debug(@"User " + u.FullName + @" in Inactive in the Case Management portal!");
            }
            _navigationManager.NavigateTo("/NoAccess");
            return;

        }
        _currentUser.UserId = u.UserId;
        _currentUser.UserName = u.FullName;
        //Setup role
        // Individual user
        bool validUser = true;

        if (u.UserRoleId == 1)
        {
            _currentUser.Role = "Administrator";
        }
        else if (u.UserRoleId == 2)
        {
            _currentUser.Role = "User";
        }
        else if (u.UserRoleId == 3)
        {
            _currentUser.Role = "Viewer";
        }
        else
        {
            //Unable to get user
            validUser = false;
        }

        if (validUser && LoginUser(_currentUser))
        {
            //await _usersRepository.UpdateLoginDate(u.UserId);
            string userLogged = $"{_currentUser.UserName} ({_currentUser.UserId})";
            //Set up the user to be used in log4net
             LoggingService.SetUpUserForLogger(userLogged, null);

             LoggingService.Debug($"User  {_currentUser.UserName}  logged in successfully");
            //flag to let Dispose save a log record when user close browser without login

             _navigationManager.NavigateTo("/home");
        }
        else
        {
             LoggingService.Error($"User {_currentUser.UserName} unable to login. ValidUser = {validUser}. Page = Home");
            _navigationManager.NavigateTo("/");
        }
    }

    private bool LoginUser(UserLogged userLogged)
    {
         ((CustomAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(userLogged);
        
        return true;
    }
    }
}


