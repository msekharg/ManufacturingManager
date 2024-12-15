using ManufacturingManager.Components.UI;
using Microsoft.JSInterop;

namespace ManufacturingManager.Web.Components.Pages.Admin.ClampsPositioning
{
    public partial class ClampsPositioningList
    {
        public IList<Core.Models.ClampsPositioning> ClampsPositioningsList = null!;
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
                TableName = "clampsPositioningTable",
                FilenameToExportExcel = "ClampsPositioning",
                TitleInExcelFile = "List of Clamps Positionings",
            };
            ClampsPositioningsList =  _DataEntryRepository.GetClampsPositioning().ToList();
        }

        private void RedirectToEditPage(int clampsPositioningId)
        {
            SessionStorage.SetAsync("ClampsPositioningId", clampsPositioningId);
            NavigationManager.NavigateTo("/EditClampsPositioning");
        }

        private async void ViewLog(int id, string desc)
        {
            ArgsView = new()
            {
                TableName = "lookUpClampsPositioning",
                FilenameToExportExcel = "ClampsPositioning for " + desc,
                TitleInExcelFile = "Tracking configurations for Clamps Positioning " + desc
            };

            RecordId = id;
            LogTitle = "View log for Configuration " + desc;
            ShowDialogOpen = true;
            await InvokeAsync(StateHasChanged);
        }

        private async Task DeleteRecord(Core.Models.ClampsPositioning clampsPositioning, string description)
        {
            var message = "Please confirm deletion of Configuration " + description;


            bool confirmation = await JsRuntime.InvokeAsync<bool>("confirm", message);
            if (confirmation)
            {
                int retvalue = await _DataEntryRepository.DeleteClampsPositioning(clampsPositioning);
                if (retvalue > 0)
                {
                    ClampsPositioningsList =  _DataEntryRepository.GetClampsPositioning().ToList();
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
