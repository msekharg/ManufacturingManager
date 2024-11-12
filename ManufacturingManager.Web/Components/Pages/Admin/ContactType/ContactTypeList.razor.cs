using Microsoft.JSInterop;
using PCSCaseManagement.UI.Components;


namespace PCSCaseManagement.Web.Components.Pages.Admin.ContactType 
{
    public partial class ContactTypeList
    {
        public IList<Core.ContactType> _contactTypes = null!;
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
                TableName = "contactTypeTable",
                FilenameToExportExcel = "PCSCM_ContactTypeList",
                TitleInExcelFile = "List of Contact types"
            };
            _contactTypes = await Repository.GetContactTypeList(true, false);
        }

        protected void ReditectToEditPage(int recordId)
        {
            SessionStorage.SetAsync("ContactTypeId", recordId);
            NavigationManager.NavigateTo("/EditContactType");
        }



        protected void ViewLog(int id, string desc)
        {
            ArgsView = new()
            {
                TableName = "lookUpContact",
                FilenameToExportExcel = "PCSCMS_ContactTypeLogFor " + desc,
                TitleInExcelFile = "Tracking log for Contact type " + desc

            };

            RecordId = id;
            LogTitle = "View log for Contact Type " + desc;
            ShowDialogOpen = true;
            StateHasChanged();

        }
        protected async Task DeleteRecord(int id, string description)
        {
            var message = "Please confirm deletion of contact type " + description;


            bool confirmation = await JsRuntime.InvokeAsync<bool>("confirm", message);
            if (confirmation)
            {
                bool retvalue = await Repository.Delete(id, CurrentUser.UserId);
                if (retvalue)
                {
                    _contactTypes = await Repository.GetContactTypeList(true, false);
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
