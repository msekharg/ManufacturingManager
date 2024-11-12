using System.Reflection;
using FluentValidation.AspNetCore;
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

var builder = WebApplication.CreateBuilder(args);
// Console.WriteLine($"Environment:{builder.Environment.EnvironmentName}");
//builder.Logging.AddLog4Net("log4net.config", true);
// Add services to the container.

builder.Configuration.AddJsonFile("appsettings.json", optional: false);
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpContextAccessor();
builder.Services
    .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddControllersWithViews();

builder.Services
    .AddAuthorization(policy => policy.FallbackPolicy = policy.DefaultPolicy);

// builder.Services
//     .AddControllersWithViews()
//     .AddMicrosoftIdentityUI();
// builder.Services.AddControllers();
// builder.Logging.AddLog4Net("log4net.config", true);

//builder.Services.AddControllers()
  //  .AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(Assembly.Load("ManufacturingManager.Core")));

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
builder.Services.AddScoped<IdentityInformation>();
builder.Services.AddTransient<IDataEntryRepository, DataEntryRepository>();
builder.Services.AddScoped<LookUpTable>();
// builder.Services.AddScoped<Logging>();
builder.Services.AddSingleton<AppSettings>();
builder.Services.AddTransient<IUsersRepository, UsersRepository>();
builder.Services.AddTransient<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddDistributedMemoryCache();
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

app.UseHttpsRedirection();
app.UseCookiePolicy(
    new CookiePolicyOptions
    {
        Secure = CookieSecurePolicy.Always
    });
app.UseSession();
app.UseStaticFiles();
// app.UseForwardedHeaders(new ForwardedHeadersOptions
// {
//     ForwardedHeaders = ForwardedHeaders.XForwardedProto
// });
app.UseRouting();
 app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();
// app.MapControllers();
// app.MapFallbackToPage("/_Host");

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.Run();