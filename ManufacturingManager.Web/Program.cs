using System.Reflection;
using FluentValidation.AspNetCore;
using log4net;
using log4net.Config;
using ManufacturingManager.Core;
using ManufacturingManager.Core.Repositories;
using ManufacturingManager.Web.Components;
using ManufacturingManager.Web.Configuration;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ManufacturingManager.Web.Data;
using ManufacturingManager.Web.Services;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
// Console.WriteLine($"Environment:{builder.Environment.EnvironmentName}");
//builder.Logging.AddLog4Net("log4net.config", true);
// Add services to the container.

builder.Configuration.AddJsonFile("appsettings.json", optional: false);
builder.Logging.AddConsole();
var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
builder.Services
    .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

// builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration);
builder.Services.AddHttpContextAccessor();

// builder.Services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
// {
//     options.TokenValidationParameters = new TokenValidationParameters
//     {
//         NameClaimType = "preferred_username", // Map to the appropriate claim
//         RoleClaimType = "roles"
//     };
// });

builder.Services.AddControllersWithViews().AddMicrosoftIdentityUI();
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


builder.Services
    .AddAuthorization(policy => policy.FallbackPolicy = policy.DefaultPolicy);

builder.Services.AddLogging(logging =>
{
    logging.AddLog4Net("log4net.config", true);
});

builder.Services.AddServerSideBlazor().AddCircuitOptions(options =>
{
    options.DetailedErrors = true;
     options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromSeconds(3);
    options.DisconnectedCircuitMaxRetained = 100;
    options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(3);
    options.JSInteropDefaultCallTimeout = TimeSpan.FromMinutes(1);
    options.MaxBufferedUnacknowledgedRenderBatches = 10;

});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(15);
    options.Cookie.Name = "PCSCMPortalSession";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// builder.Services.AddRazorPages();
builder.Services.AddSingleton<AppCache>();
builder.Services.AddScoped<UserLogged>();
builder.Services.AddScoped<ILoggingService, LoggingService>();
builder.Services.AddScoped<IdentityInformation>();
builder.Services.AddTransient<IDataEntryRepository, DataEntryRepository>();
builder.Services.AddScoped<LookUpTable>();
// builder.Services.AddScoped<Logging>();
builder.Services.AddSingleton<AppSettings>();
builder.Services.AddTransient<IUsersRepository, UsersRepository>();
builder.Services.AddTransient<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}


app.UseCookiePolicy(
    new CookiePolicyOptions
    {
        Secure = CookieSecurePolicy.Always
    });


app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto
});
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
  app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();
// app.MapBlazorHub();
 app.MapControllers();
// app.MapFallbackToPage("/_Host");

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.Run();