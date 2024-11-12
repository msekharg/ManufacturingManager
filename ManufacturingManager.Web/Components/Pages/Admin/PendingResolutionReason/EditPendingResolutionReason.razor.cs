using PCSCaseManagement.Core;
using PCSCaseManagement.UI.Components.MessageBox;
using PCSCaseManagement.Web.Data;

namespace PCSCaseManagement.Web.Components.Pages.Admin.PendingResolutionReason 
{
    public partial class EditPendingResolutionReason
    {
        private LookUpTable _pendingResolutionReasons = null!;
        private string _addEditTitle = "Add new Pending resolution reason";
        private string _errorMessage = "";
        public bool _showMessageBox;
        private readonly MessageBoxParameters _messageBoxParameters = new();

        protected override async Task OnInitializedAsync()
        {
            _pendingResolutionReasons = new LookUpTable
            {
                Active = true
            };
            var id = await SessionStorage.GetAsync<int>("PendingResolutionReasonId");
            if (id.Value != 0)
            {

                Core.PendingResolutionReason p = await Repository.PendingResolutionReason(id.Value);
                _pendingResolutionReasons.Id = p.PendingResolutionReasonId;
                _pendingResolutionReasons.Description = p.Description;
                _pendingResolutionReasons.Active = p.Active;
                _addEditTitle = $"Edit Pending resolution reason {p.Description}";
            }


        }
        private void MessageBoxClose()
        {
            _showMessageBox = false;
            StateHasChanged();
        }
        protected async Task Save()
        {
            //Clean ContactType List cached in AppCache class
            AppCache.PendingResolutionReasonList = null;
            Core.PendingResolutionReason pendingResolutionReason = new()
            {
                Active = _pendingResolutionReasons.Active,
                PendingResolutionReasonId = _pendingResolutionReasons.Id,
                Description = _pendingResolutionReasons.Description
            };
            bool retval;

            var currentUserId = CurrentUser.UserId;
            _messageBoxParameters.Title = pendingResolutionReason.PendingResolutionReasonId == 0 ? "New pending resolution reason added." : "Pending resolution reason Updated";
            _messageBoxParameters.PageToRedirect = @"/PendingResolutionReasonList";
            _messageBoxParameters.IsErrorMessage = false;
            if (pendingResolutionReason.PendingResolutionReasonId != 0)
                retval = await Repository.Update(pendingResolutionReason, currentUserId);
            else
                retval = await Repository.Add(pendingResolutionReason, currentUserId);
            if (retval)
            {
                var successMessage = $"Pending resolution reason {pendingResolutionReason.Description} ";
                successMessage += pendingResolutionReason.PendingResolutionReasonId != 0 ? " updated." : " created.";
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
            NavigationManager.NavigateTo("/PendingResolutionReasonList");
            return Task.CompletedTask;
        }


    }
}
