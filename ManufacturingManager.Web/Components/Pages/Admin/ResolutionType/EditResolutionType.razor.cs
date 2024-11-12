using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using PCSCaseManagement.Core;
using PCSCaseManagement.UI.Components.MessageBox;

namespace PCSCaseManagement.Web.Components.Pages.Admin.ResolutionType  
{
    public partial class EditResolutionType
    {
        public EditContext _editContext = default!;
        private readonly Core.ResolutionType _resolutionTypes = new();
        private string _addEditTitle = "Add new Resolution type";
        private string _errorMessage = "";
        public bool _showMessageBox;
        private readonly MessageBoxParameters _messageBoxParameters = new();
        private InputText? _inputDescription;
        protected override async Task OnInitializedAsync()
        {
            _editContext = new EditContext(_resolutionTypes);
            _resolutionTypes.Active = true;

            var id = await SessionStorage.GetAsync<int>("ResolutionTypeId");
            if (id.Value != 0)
            {

                Core.ResolutionType p = await Repository.ResolutionType(id.Value);
                _resolutionTypes.ResolutionTypeId = p.ResolutionTypeId;
                _resolutionTypes.Description = p.Description;
                _resolutionTypes.DefaultResolution = p.DefaultResolution;
                _resolutionTypes.Active = p.Active;
                _addEditTitle = $"Edit Resolution type {p.Description}";
            }


        }
        private void MessageBoxClose()
        {
            _showMessageBox = false;
            StateHasChanged();
        }
        protected async Task Save()
        {
            //Clean   List cached in AppCache class
            AppCache.ResolutionTypeList = null;
            Core.ResolutionType resolutionType = new()
            {
                Active = _resolutionTypes.Active,
                ResolutionTypeId = _resolutionTypes.ResolutionTypeId,
                DefaultResolution = _resolutionTypes.DefaultResolution,
                Description = _resolutionTypes.Description
            };
            bool retval;

            var currentUserId = CurrentUser.UserId;
            _messageBoxParameters.Title = resolutionType.ResolutionTypeId == 0 ? "New Resolution type added." : "Resolution type Updated";
            _messageBoxParameters.PageToRedirect = @"/ResolutionTypeList";
            _messageBoxParameters.IsErrorMessage = false;
            if (resolutionType.ResolutionTypeId != 0)
                retval = await Repository.Update(resolutionType, currentUserId);
            else
                retval = await Repository.Add(resolutionType, currentUserId);
            if (retval)
            {
                var successMessage = $"Resolution type {resolutionType.Description} ";
                successMessage += resolutionType.ResolutionTypeId != 0 ? " updated." : " created.";
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
            NavigationManager.NavigateTo("/ResolutionTypeList");
            return Task.CompletedTask;
        }
        private async Task FocusFirstError()
        {
            // repeat for all inputs
            if (_editContext.GetValidationMessages(() => _resolutionTypes.Description).Any())
            {
                await _inputDescription!.Element!.Value.FocusAsync();
            }
        }

    }
}
