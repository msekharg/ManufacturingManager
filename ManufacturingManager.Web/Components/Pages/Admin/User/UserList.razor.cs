
using ManufacturingManager.Components.UI;

namespace ManufacturingManager.Web.Components.Pages.Admin.User
{
    public partial class UserList
    {
        public IList<Core.User> _users = null!;
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
                TableName = "UsersTable",
                FilenameToExportExcel = "PCSCMS_UsersList",
                TitleInExcelFile = "List of Users"
            };
            _users = (IList<Core.User>)await UserRepository.GetUserList(false);
        }

        protected void ReditectToEditPage(int recordId)
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

        //protected async Task DeleteRecord(int id, string description)
        //{
        //    var message = "Please confirm deletion of user " + description;


        //    bool confirmation = await _jsRuntime.InvokeAsync<bool>("confirm", message);
        //    if (confirmation)
        //    {
        //        bool retvalue = await _userRepository. .Delete(id, _currentUser.UserId);
        //        if (retvalue)
        //        {
        //            _priorities = await _priorityRepository.GetPriorityList(true,false);
        //            StateHasChanged();
        //        }
        //        else
        //        {
        //            await _jsRuntime.InvokeVoidAsync("alert", "Unable to delete this record");

        //        }
        //    }
        //}
    }
}


