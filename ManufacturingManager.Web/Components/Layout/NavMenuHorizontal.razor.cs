using ManufacturingManager.Web.Data;
using ManufacturingManager.Web.Services;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;

namespace ManufacturingManager.Web.Components.Layout

{
    public partial class NavMenuHorizontal : IDisposable 
    {
        private ProtectedBrowserStorageResult<string> _userName;
        public string? CurrentLocation { get; set; }
      
        protected override async Task OnInitializedAsync()
        {
            _userName = await SessionStorage.GetAsync<string>("CurrentUserName");
            NavigationManager.LocationChanged += LocationChanged!;
        }

        public async void Dispose()
        {
            //Logout();
            bool saveLog = false;
            try
            {
                if (SessionStorage != null)
                {
                    var val = await SessionStorage.GetAsync<bool>("AvoidLogInDispose");
                    saveLog = !val.Value;
                }
            }
            catch { saveLog = true; }

            if (saveLog)
            {
                // Logging.WriteToLog(
                //     @"User " + CurrentUser.VaLogon +
                //     @" logged out closing the browser without choosing Logoff option from the main menu.",
                //     LoggingCategoryEnum.Debug);
            }
            NavigationManager.LocationChanged -= LocationChanged!;
        }
        public void Logout()
        {
            SessionStorage.SetAsync("AvoidLogInDispose",true);

            // Logging.WriteToLog(@"User " + CurrentUser.VaLogon + @" logged out",LoggingCategoryEnum.Debug );
             ((CustomAuthenticationStateProvider)AuthenticationStateProvider).MarkUserAsAuthenticated(CurrentUser);
            CurrentUser = new UserLogged();
            NavigationManager.NavigateTo("logout", true);

            //var url = $"{_navigationManager.BaseUri}/";
            //_js.InvokeVoidAsync("OpenWindow", url);
        }

        /// <summary>
        /// SkipToContent
        /// This is used to skip to content.
        /// Note that we have the following line in App.Razor
        ///  <FocusOnNavigate RouteData="@routeData" Selector="h2" />
        /// That means the page is located at the first <h2> when page is loaded and we define Page title using <h2> tag
        /// then, this Skip link will reload the page pointing to first element which is the h2
        /// I defined the id for initial h2 as startContent
        /// </summary>
        async void SkipToContent()
        {
            await Js.InvokeVoidAsync("focusElement", "startContent");
        }

        void LocationChanged(object sender,LocationChangedEventArgs  e)
        {
              CurrentLocation = e.Location;
              StateHasChanged();
        }

        //void IDisposable.Dispose()
        //{
        //    _navigationManager.LocationChanged -= LocationChanged;
        //}

    }
}
