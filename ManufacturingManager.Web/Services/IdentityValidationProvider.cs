using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace ManufacturingManager.Web.Services
{
    public class IdentityValidationProvider<TUser> : RevalidatingServerAuthenticationStateProvider where TUser : class
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IdentityOptions _options;

        public IdentityValidationProvider(ILoggerFactory loggerFactory, IServiceScopeFactory scopeFactory,
            IOptions<IdentityOptions> optionsAccessor) : base(loggerFactory)
        {
            _scopeFactory = scopeFactory;
            _options = optionsAccessor.Value;
        }

        protected override async Task<bool> ValidateAuthenticationStateAsync(AuthenticationState authenticationState, CancellationToken cancellationToken)
        {
            var scope= _scopeFactory.CreateScope();
            try
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<TUser>>();
                
                return await ValidateSecurityStampAsync(userManager, authenticationState.User);
            }
            finally
            {
                if (scope is IAsyncDisposable asyncDisposable)
                {
                    await asyncDisposable.DisposeAsync();
                }
                else
                {
                    scope.Dispose();
                }
            }
        }

        private async Task<bool> ValidateSecurityStampAsync(UserManager<TUser> userManager, ClaimsPrincipal principal)
        {
            var user = await userManager.GetUserAsync(principal);
            if (user == null)
            {
                return false;
            }

            var principalStamp = principal.FindFirstValue(_options.ClaimsIdentity.SecurityStampClaimType);
            var userStamp = await userManager.GetSecurityStampAsync(user);
            return principalStamp == userStamp;
        }
        //Frequency to revalidate the authentication state
        protected override TimeSpan RevalidationInterval => TimeSpan.FromSeconds(30);
    }
}
