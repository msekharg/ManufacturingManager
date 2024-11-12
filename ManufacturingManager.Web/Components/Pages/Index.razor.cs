using ManufacturingManager.Core;
using ManufacturingManager.Web.Data;
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
                    NavigationManager.NavigateTo("/NoAccess");
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
                    NavigationManager.NavigateTo("/NoAccess");
                    return;

                }
                //saving userid to Session Storage to let set up user identity and login after passing 
                //Check FSC Inactivity and Concurrent session
                
                await ProtectedSessionStorage.SetAsync("userId", u.UserId);

                NavigationManager.NavigateTo("/home");
                //TODO Review final implementation from Mani
                // NavigationManager.NavigateTo("/checkFscSecurity", true);
            }
            catch (Exception exception)

            {
                // Logging.WriteToLog(@"Error from ADLogic for User " + Identity.UserName + @" in the Case Management portal!", LoggingCategoryEnum.Error,exception);
                NavigationManager.NavigateTo("/NoAccess");
            }
        }
    }
}


