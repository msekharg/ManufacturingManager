
using ManufacturingManager.Components.UI;
using Microsoft.JSInterop;

namespace ManufacturingManager.Web.Components.Pages.Admin.User
{
    public partial class UserList
    {
        public IList<Core.User> Users = null!;
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
                TableName = "UserTable",
                FilenameToExportExcel = "PCSCMS_UsersList",
                TitleInExcelFile = "List of Users"
            };
            Users = (IList<Core.User>)await UserRepository.GetUserList(false);
        }

        protected void RedirectToEditPage(int recordId)
        {
            SessionStorage.SetAsync("UserId", recordId);
            NavigationManager.NavigateTo("/EditUser");
        }
        
        protected void ViewLog(int id, string desc)
        {
            ArgsView = new()
            {
                TableName = "lookUpUser",
                FilenameToExportExcel = "PCSCMS_UserLogFor " + desc,
                TitleInExcelFile = "Tracking log for User " + desc

            };

            RecordId = id;
            LogTitle = "View log for User " + desc;
            ShowDialogOpen = true;
            StateHasChanged();
        }

        protected async Task DeleteRecord(Core.User user, string description)
        {
            var message = "Please confirm deletion of user " + description;
            
            bool confirmation = await JsRuntime.InvokeAsync<bool>("confirm", message);
            if (confirmation)
            {
                int retvalue = await UserRepository.Delete(user);
                if (retvalue > 0)
                {
                    Users = (IList<Core.User>)await UserRepository.GetUserList(false);
                    StateHasChanged();
                }
                else
                {
                    await JsRuntime.InvokeVoidAsync("alert", "Unable to delete this record");
                }
            }
        }
    }
}


