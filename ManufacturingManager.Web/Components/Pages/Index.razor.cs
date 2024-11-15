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

        protected override Task OnInitializedAsync()
        {
            _editContext = new EditContext(_login);
            return Task.CompletedTask;
        }

        protected async Task Login()
        {
            //Validating the user  in Active Directory.
            try
            {

                var userName =  HttpContextAccessor.HttpContext?.User.Identity?.Name;
                if (userName == null)
                {
                    // Logging.WriteToLog(@"Unable to find windowsUser from HttpContextAccessor ", LoggingCategoryEnum.Error);
                    _navigationManager.NavigateTo("/NoAccess");
                    return;
                }
                // HttpContextAccessor.HttpContext.Session.Initialize();
                
                //If customer selected users from dropdownlist _ Only available to lower environments
                if (!string.IsNullOrEmpty(_login.LoginName))
                {
                    userName = _login.LoginName;
                }
                //Find user in Repository
                User u = UsersRepository.Users(userName);
                if (u.UserId == 0 || !u.IsActive)
                {
                    if (u.UserId == 0)
                    {
                        // Logging.WriteToLog(@"User " + username + @" has no access to Case Management portal!", LoggingCategoryEnum.General);
                    }
                    else
                    {
                        // Logging.WriteToLog(@"User " + username + @" in Inactive in the Case Management portal!", LoggingCategoryEnum.General);
                    }
                    _navigationManager.NavigateTo("/NoAccess");
                    return;

                }
                //saving userid to Session Storage to let set up user identity and login after passing 
                //Check FSC Inactivity and Concurrent session
                
                await ProtectedSessionStorage.SetAsync("userId", u.UserId);
                await SetUpIdentityForUser(u.UserId);
                // _navigationManager.NavigateTo("/home");
                //TODO Review final implementation from Mani
                // NavigationManager.NavigateTo("/checkFscSecurity", true);
            }
            catch (Exception exception)

            {
                // Logging.WriteToLog(@"Error from ADLogic for User " + Identity.UserName + @" in the Case Management portal!", LoggingCategoryEnum.Error,exception);
                _navigationManager.NavigateTo("/NoAccess");
            }
        }
        
        private async Task SetUpIdentityForUser(int userId)
    {
        User u = _usersRepository.Users(userId);
        if (u.UserId == 0 || !u.IsActive)
        {
            if (u.UserId == 0)
            {
                // Logging.WriteToLog(@"User " + userId.ToString() + @" does not exists in Case Management portal!", LoggingCategoryEnum.General);
            }
            else
            {
                // Logging.WriteToLog(@"User " + u.FullName + @" in Inactive in the Case Management portal!", LoggingCategoryEnum.General);
            }
            _navigationManager.NavigateTo("/NoAccess");
            return;

        }
        _currentUser.UserId = u.UserId;
        _currentUser.UserName = u.FullName;
        _currentUser.LoginName = u.LoginName;
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
            // Logging.SetUpUserForLogger(userLogged);

            // Logging.WriteToLog(@"User " + _currentUser.VaLogon + @" logged in successfully", LoggingCategoryEnum.Debug);
            //flag to let Dispose save a log record when user close browser without login

             _navigationManager.NavigateTo("/home");
        }
        else
        {
            // Logging.WriteToLog(@"User {_currentUser.VaLogon} unable to login. ValidUser = {validUser}. Page = Home", LoggingCategoryEnum.Error);
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


