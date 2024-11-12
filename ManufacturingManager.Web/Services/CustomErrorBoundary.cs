using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace ManufacturingManager.Web.Services
{
    public class CustomErrorBoundary : ErrorBoundary
    {
        [Inject] private IWebHostEnvironment Environment { get; set; }
        protected override Task OnErrorAsync(Exception exception)
        {
            if (!Environment.IsDevelopment())
            {
                // Logging.WriteToLog($"Unexpected error: {exception.Message}", LoggingCategoryEnum.Error, exception);
            }

            return Task.CompletedTask;
        }
    }
}

