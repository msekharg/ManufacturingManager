using Microsoft.JSInterop;
using PCSCaseManagement.UI.Components;


namespace PCSCaseManagement.Web.Components.Pages.Admin.ResolutionType

{
    public partial class ResolutionTypeList
    {
        public IList<Core.ResolutionType> _resolutionTypes = null!;
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
                TableName = "resolutionTypeTable",
                FilenameToExportExcel = "PCSCMS_ResolutionTypeList",
                TitleInExcelFile = "List of Resolution types"
            };
            _resolutionTypes = await Repository.GetResolutionTypeList(true, false);
        }

        protected void ReditectToEditPage(int recordId)
        {
            SessionStorage.SetAsync("ResolutionTypeId", recordId);
            NavigationManager.NavigateTo("/EditResolutionType");
        }



        protected async void ViewLog(int id, string desc)
        {
            ArgsView = new()
            {
                TableName = "lookUpResolutionType",
                FilenameToExportExcel = "PCSCMS_ResolutionTypeLogFor " + desc,
                TitleInExcelFile = "Tracking log for Resolution type " + desc

            };

            RecordId = id;
            LogTitle = "View log for resolution Type " + desc;
            ShowDialogOpen = true;
            await InvokeAsync(StateHasChanged);

        }
        protected async Task DeleteRecord(int id, string description)
        {
            var message = "Please confirm deletion of resolution type " + description;


            bool confirmation = await JsRuntime.InvokeAsync<bool>("confirm", message);
            if (confirmation)
            {
                bool retvalue = await Repository.Delete(id, CurrentUser.UserId);
                if (retvalue)
                {
                    _resolutionTypes = await Repository.GetResolutionTypeList(true, false);
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
