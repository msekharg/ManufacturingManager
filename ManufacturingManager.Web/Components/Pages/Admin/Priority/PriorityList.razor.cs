using Microsoft.JSInterop;
using PCSCaseManagement.UI.Components;


namespace PCSCaseManagement.Web.Components.Pages.Admin.Priority
{
    public partial class PriorityList
    {
        public IList<Core.Priority> _priorities = null!;
        public string? LogTitle { get; set; }
        public int RecordId { get; set; }
        public bool ShowDialogOpen { get; set; }

        public LookUpTableArgs Args { get; set; }
        public LookUpTableArgs ArgsView { get; set; }
        private async void OnDeleteDialogClose()
        {
            ShowDialogOpen = false;
            await InvokeAsync(StateHasChanged);
        }

        protected override async Task OnInitializedAsync()
        {
            Args = new()
            {
                TableName = "priorityTable",
                FilenameToExportExcel = "PCSCM_PriorityList",
                TitleInExcelFile = "List of Priorities"
            };
            _priorities = await PriorityRepository.GetPriorityList(true, false);
        }

        protected void ReditectToEditPage(int recordId)
        {
            SessionStorage.SetAsync("PriorityId", recordId);
            NavigationManager.NavigateTo("/EditPriority");
        }

        protected void ViewLog(int id, string desc)
        {
            ArgsView = new()
            {
                TableName = "lookUpPriority",
                FilenameToExportExcel = "PCSCMS_PriorityLogFor " + desc,
                TitleInExcelFile = "Tracking log for Priority " + desc

            };

            RecordId = id;
            LogTitle = "View log for priority " + desc;
            ShowDialogOpen = true;
            StateHasChanged();
        }

        protected async Task DeleteRecord(int id, string description)
        {
            var message = "Please confirm deletion of priority " + description;


            bool confirmation = await JsRuntime.InvokeAsync<bool>("confirm", message);
            if (confirmation)
            {
                bool retvalue = await PriorityRepository.Delete(id, CurrentUser.UserId);
                if (retvalue)
                {
                    _priorities = await PriorityRepository.GetPriorityList(true, false);
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
