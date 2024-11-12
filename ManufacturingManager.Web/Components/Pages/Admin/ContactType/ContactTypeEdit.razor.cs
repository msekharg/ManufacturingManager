using PCSCaseManagement.Core;
using PCSCaseManagement.UI.Components.MessageBox;
using PCSCaseManagement.Web.Data;

namespace PCSCaseManagement.Web.Components.Pages.Admin.ContactType
{
    public partial class ContactTypeEdit
    {
        private LookUpTable _contactType = null!;
        private string _addEditTitle = "Add new Contact type";
        private string _errorMessage = "";
        public bool _showMessageBox;
        private readonly MessageBoxParameters _messageBoxParameters = new();

        protected override async Task OnInitializedAsync()
        {
            _contactType = new LookUpTable
            {
                Active = true
            };
            var id = await SessionStorage.GetAsync<int>("ContactTypeId");
            if (id.Value != 0)
            {

                Core.ContactType p = await Repository.ContactType(id.Value);
                _contactType.Id = p.ContactTypeId;
                _contactType.Description = p.Description;
                _contactType.Active = p.Active;
                _addEditTitle = $"Edit Contact Type {p.Description}";
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
            AppCache.ContactTypeList = null;
            Core.ContactType contactType = new()
            {
                Active = _contactType.Active,
                ContactTypeId = _contactType.Id,
                Description = _contactType.Description
            };
            bool retval;

            var currentUserId = CurrentUser.UserId;
            _messageBoxParameters.Title = contactType.ContactTypeId == 0 ? "New contact type added." : "Contact Type Updated";
            _messageBoxParameters.PageToRedirect = @"/ContactTypeList";
            _messageBoxParameters.IsErrorMessage = false;
            if (contactType.ContactTypeId != 0)
                retval = await Repository.Update(contactType, currentUserId);
            else
                retval = await Repository.Add(contactType, currentUserId);
            if (retval)
            {
                var successMessage = $"Contact Type {contactType.Description} ";
                successMessage += contactType.ContactTypeId != 0 ? " updated." : " created.";
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
            NavigationManager.NavigateTo("/ContactTypeList");
            return Task.CompletedTask;
        }


    }
}