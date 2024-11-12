// using System.Net.Http.Headers;
// using Microsoft.Identity.Client;
//
// namespace ManufacturingManager.Web.Services
// {
//     public class AuthProvider : IAuthenticationProvider
//     {
//         private ClientCredentialProvider _msalClient;
//         private string[] _scopes;
//         AuthenticationResult _token;
//         private static log4net.ILog _log;
//
//         public AuthProvider(string clientID, string clientSecret, string tenatID, log4net.ILog logger, string[] scopes)
//         //public AuthProvider(string clientID, string clientSecret, string tenatID, string[] scopes)
//         {
//             _scopes = scopes;
//              _log = logger;
//             IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
//                 .Create(clientID)
//                 .WithClientSecret(clientSecret)
//                 .WithTenantId(tenatID)
//                 .Build();
//             _msalClient = new ClientCredentialProvider(confidentialClientApplication);
//         }
//
//         public async Task<string> GetAccessToken()
//         {
//             if (_token == null ||
//                 _token.ExpiresOn.AddMinutes(1) >= DateTimeOffset.Now) // If access token is expired or will expire in the next minute, get a new one
//             {
//                 if (_log != null)
//                     _log.Debug("Retrieving access token.");
//                 _token = await _msalClient.ClientApplication.AcquireTokenForClient(_scopes).ExecuteAsync();
//             }
//             return _token.AccessToken;
//         }
//
//         // The Graph SDK will call this function each time it makes a Graph call.
//         public async Task AuthenticateRequestAsync(HttpRequestMessage requestMessage)
//         {
//             requestMessage.Headers.Authorization =
//                 new AuthenticationHeaderValue("bearer", await GetAccessToken());
//         }
//     }
//
// }