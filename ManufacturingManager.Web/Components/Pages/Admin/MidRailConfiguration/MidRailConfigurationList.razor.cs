using ManufacturingManager.Components.UI;
using Microsoft.JSInterop;

namespace ManufacturingManager.Web.Components.Pages.Admin.MidRailConfiguration
{
    public partial class MidRailConfigurationList
    {
        public IList<Core.Models.MidRailConfiguration> MidRailConfigurations = null!;
        public string? LogTitle { get; set; }
        public int RecordId { get; set; }
        public bool ShowDialogOpen { get; set; }
        public LookUpTableArgs Args { get; set; }
        public LookUpTableArgs ArgsView { get; set; }
        private void OnDeleteDialogClose()
        {
            ShowDialogOpen = false;
            StateHasChanged();
        }
        protected override async Task OnInitializedAsync()
        {
            Args = new()
            {
                TableName = "midRailConfigurationTable",
                FilenameToExportExcel = "MidRailConfigurations",
                TitleInExcelFile = "List of MidRailConfigurations",
            };
            MidRailConfigurations =  _DataEntryRepository.GetMidRailConfiguration().ToList();
        }

        private void RedirectToEditPage(int midRailConfigurationId)
        {
            SessionStorage.SetAsync("MidRailConfigurationId", midRailConfigurationId);
            NavigationManager.NavigateTo("/EditMidRailConfiguration");
        }

        private async void ViewLog(int id, string desc)
        {
            ArgsView = new()
            {
                TableName = "lookUpMidRailConfigurations",
                FilenameToExportExcel = "MidRailConfigurations for " + desc,
                TitleInExcelFile = "Tracking configurations for Mid Rail " + desc
            };

            RecordId = id;
            LogTitle = "View log for Configuration " + desc;
            ShowDialogOpen = true;
            await InvokeAsync(StateHasChanged);
        }

        private async Task DeleteRecord(int id, string description)
        {
            var message = "Please confirm deletion of Configuration " + description;


            bool confirmation = await JsRuntime.InvokeAsync<bool>("confirm", message);
            if (confirmation)
            {
                bool retvalue = true;//await _DataEntryRepository.DeleteColorCode(id, CurrentUser.UserId);
                if (retvalue)
                {
                    MidRailConfigurations =  _DataEntryRepository.GetMidRailConfiguration().ToList();
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    await JsRuntime.InvokeVoidAsync("alert", "Unable to delete this record");

                }
            }
        }
    }
}
