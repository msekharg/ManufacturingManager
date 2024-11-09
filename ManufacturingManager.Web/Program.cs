using System.Reflection;
using FluentValidation.AspNetCore;
using ManufacturingManager.Core.Repositories;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ManufacturingManager.Web.Data;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

var builder = WebApplication.CreateBuilder(args);
Console.WriteLine($"Environment:{builder.Environment.EnvironmentName}");
//builder.Logging.AddLog4Net("log4net.config", true);
// Add services to the container.


builder.Services
    .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

builder.Services
    .AddAuthorization(policy => policy.FallbackPolicy = policy.DefaultPolicy);

builder.Services
    .AddControllersWithViews()
    .AddMicrosoftIdentityUI();

//builder.Services.AddControllers()
  //  .AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(Assembly.Load("ManufacturingManager.Core")));

builder.Configuration.AddJsonFile("appsettings.json", optional: false);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true;});
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddTransient<IDataEntryRepository, DataEntryRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();