using PCSCaseManagement.Core;
using PCSCaseManagement.UI.Components.MessageBox;
using PCSCaseManagement.Web.Data;

namespace PCSCaseManagement.Web.Components.Pages.Admin.Priority
{
    public partial class PriorityEdit
    {
        private LookUpTable _priority = null!;
        private string _addEditTitle = "Add new Priority";
        private string _errorMessage = "";
        public bool _showMessageBox;
        private readonly MessageBoxParameters _messageBoxParameters = new();

        protected override async Task OnInitializedAsync()
        {
            _priority = new LookUpTable
            {
                Active = true
            };
            var id = await SessionStorage.GetAsync<int>("PriorityId");
            if (id.Value != 0)
            {

                Core.Priority p = Repository.Priority(id.Value);
                _priority.Id = p.PriorityId;
                _priority.Description = p.Description;
                _priority.Active = p.Active;
                _addEditTitle = $"Edit Priority {p.Description}";
            }


        }
        private void MessageBoxClose()
        {
            _showMessageBox = false;
            StateHasChanged();
        }
        protected async Task Save()
        {
            //Clean Priority List cached in AppCache class
            AppCache.PriorityList = null;
            Core.Priority priority = new()
            {
                Active = _priority.Active,
                PriorityId = _priority.Id,
                Description = _priority.Description
            };
            bool retval;

            var currentUserId = CurrentUser.UserId;
            _messageBoxParameters.Title = priority.PriorityId == 0 ? "New priority added." : "Priority Updated";
            _messageBoxParameters.PageToRedirect = @"/PriorityList";
            _messageBoxParameters.IsErrorMessage = false;
            if (priority.PriorityId != 0)
                retval = await Repository.Update(priority, currentUserId);
            else
                retval = await Repository.Add(priority, currentUserId);
            if (retval)
            {
                var successMessage = $"Priority {priority.Description} ";
                successMessage += priority.PriorityId != 0 ? " updated." : " created.";
                _messageBoxParameters.Message = successMessage;
            }
            else
            {
                //error message and log
                _errorMessage = "We found an unexpected Error. Please contact System Administrator Team.";
                _messageBoxParameters.Message = _errorMessage;
                _messageBoxParameters.IsErrorMessage = true;
                _messageBoxParameters.PageToRedirect = @"";

            }
            _showMessageBox = true;

            StateHasChanged();
        }

        protected Task ReturnToList()
        {
            NavigationManager.NavigateTo("/PriorityList");
            return Task.CompletedTask;
        }


    }
}
