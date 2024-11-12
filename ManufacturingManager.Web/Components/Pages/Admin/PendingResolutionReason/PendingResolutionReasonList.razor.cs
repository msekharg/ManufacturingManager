using Microsoft.JSInterop;
using PCSCaseManagement.UI.Components;


namespace PCSCaseManagement.Web.Components.Pages.Admin.PendingResolutionReason

{
    public partial class PendingResolutionReasonList
    {
        public IList<Core.PendingResolutionReason> _pendingresolutionReasons = null!;
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
                TableName = "pendingResolutionTable",
                FilenameToExportExcel = "PCSCMS_PendingResolutionReasonList",
                TitleInExcelFile = "List of Pending resolution reasons"
            };
            _pendingresolutionReasons = await Repository.GetPendingResolutionReasonList(true, false);
        }

        protected void ReditectToEditPage(int recordId)
        {
            SessionStorage.SetAsync("PendingResolutionReasonId", recordId);
            NavigationManager.NavigateTo("/EditPendingResolutionReason");
        }



        protected void ViewLog(int id, string desc)
        {
            ArgsView = new()
            {
                TableName = "lookUpPendingResolutions",
                FilenameToExportExcel = "PCSCMS_PendingResolutionLogFor " + desc,
                TitleInExcelFile = "Tracking log for Pending resolution " + desc

            };

            RecordId = id;
            LogTitle = "View log for Pending Resolution " + desc;
            ShowDialogOpen = true;
            StateHasChanged();

        }

        protected async Task DeleteRecord(int id, string description)
        {
            var message = "Please confirm deletion of pending resolution reason " + description;


            bool confirmation = await JsRuntime.InvokeAsync<bool>("confirm", message);
            if (confirmation)
            {
                bool retvalue = await Repository.Delete(id, CurrentUser.UserId);
                if (retvalue)
                {
                    _pendingresolutionReasons = await Repository.GetPendingResolutionReasonList(true, false);
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

