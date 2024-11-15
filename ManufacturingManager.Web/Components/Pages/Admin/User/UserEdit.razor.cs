using ManufacturingManager.Components.UI.MessageBox;
using ManufacturingManager.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace ManufacturingManager.Web.Components.Pages.Admin.User
{
    public partial class UserEdit
    {
        public EditContext _editContext = default!;
        private readonly Core.User _user = new();
        private string _addEditTitle = "Add new User";
        private string _errorMessage = "";
        public bool _showMessageBox;
        public bool _showActiveDirectorySearch;
        private readonly MessageBoxParameters _messageBoxParameters = new();
        private IList<UserRole>? _profileList;
        private Core.User? _userFromAdSearch;
        // private FindUserInActiveDirectory? _adId;
        //Following properties are used to reference input elements in HTML and will be used in
        //FocusFirstError() after validation fails. The idea is to set focus in the first invalid input element
        private InputSelect<int> _inputProfile = null!;
        private InputText _inputEmail = null!;
        private InputText _inputVaLogon = null!;
        private InputText _inputFirstName = null!;
        private InputText _inputLastName = null!;

        protected override async Task OnInitializedAsync()
        {
            _showActiveDirectorySearch = false;
            _editContext = new EditContext(_user);
            _profileList = new List<UserRole>( UserRoleRepository.GetUserRoleList());

            if (_profileList.Count > 0)
            {
                UserRole profile = new UserRole
                {
                    UserRoleId = 0,
                    UserRoleName = "-- Select Profile  --"
                };
                _profileList.Insert(0, profile);

            }
            var id = await SessionStorage.GetAsync<int>("UserId");
            await SessionStorage.SetAsync("UserId", 0);
            if (id.Value != 0)
            {

                Core.User p = Repository.Users(id.Value);
                _user.UserId = p.UserId;
                _user.Email = p.Email;
                _user.FirstName = p.FirstName;
                _user.LastName = p.LastName;
                _user.MiddleName = p.MiddleName;
                _user.UserRoleId = p.UserRoleId;
                _user.IsActive = p.IsActive;
                _user.LoginName = p.LoginName;
                _addEditTitle = $"Edit User {p.FirstName} {p.LastName}";

            }
            else
            {
                _user.IsActive = true;
            }

        }
        private void MessageBoxClose()
        {
            _showMessageBox = false;
            StateHasChanged();
        }
        protected async Task Save()
        {
            var currentUserId = CurrentUser.UserId;
            _messageBoxParameters.Title = _user.UserId == 0 ? "New User added." : "User Updated";
            _messageBoxParameters.PageToRedirect = @"/UserList";
            _messageBoxParameters.IsErrorMessage = false;
            //Check if user already exists
            Core.User uExist = Repository.Users(_user.LoginName);
            string successMessage;
            if (uExist != null && uExist.UserId != _user.UserId)
            {
                successMessage = "There is an existing user for Account: " + _user.LoginName;
                _messageBoxParameters.Message = successMessage;
                _messageBoxParameters.IsErrorMessage = true;
                _messageBoxParameters.PageToRedirect = @"";
            }
            else
            {
                bool retval;
                if (_user.UserId != 0)
                {
                    int recordsUpdated = await Repository.Update(_user, currentUserId);
                    if(recordsUpdated > 0)
                        retval = true;
                    else
                    {
                        retval = false;
                    }
                }
                else
                    retval = await Repository.Add(_user, currentUserId);
                if (retval)
                {
                    successMessage = $"User {_user.FullName} ";
                    successMessage += _user.UserId != 0 ? " updated." : " created.";
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
            }

            _showMessageBox = true;
// StateHasChanged();

        }
        private async Task FocusFirstError()
        {
            // repeat for all inputs
            if (_editContext.GetValidationMessages(() => _user.Email).Any())
            {
                await _inputEmail.Element!.Value.FocusAsync();
            }
            else if (_editContext.GetValidationMessages(() => _user.LoginName).Any())
            {
                await _inputVaLogon.Element!.Value.FocusAsync();
            }
            else if (_editContext.GetValidationMessages(() => _user.FirstName).Any())
            {
                await _inputFirstName.Element!.Value.FocusAsync();
            }
            else if (_editContext.GetValidationMessages(() => _user.LastName).Any())
            {
                await _inputLastName.Element!.Value.FocusAsync();
            }
            else if (_editContext.GetValidationMessages(() => _user.UserRoleId).Any())
            {
                await _inputProfile.Element!.Value.FocusAsync();
            }
        }
        private void ShowFindUserInAd()
        {
            _showActiveDirectorySearch = true;
            StateHasChanged();
        }

        private void MyUserSelected(Core.User userReturned)
        {
            _userFromAdSearch = userReturned;// _adId?.UserReturned;// .EmailReturned;
            if (_userFromAdSearch != null)
            {
                //Find rest of information from AD
                _user.Email = _userFromAdSearch.Email;
                _user.FirstName = _userFromAdSearch.FirstName;
                _user.LastName = _userFromAdSearch.LastName;
                _user.MiddleName = _userFromAdSearch.MiddleName;
                _user.LoginName = _userFromAdSearch.LoginName;
                _user.PhoneNumber = _userFromAdSearch.PhoneNumber;
            }

            _showActiveDirectorySearch = false;
            StateHasChanged();
        }
        protected Task ReturnToList()
        {
            NavigationManager.NavigateTo("/UserList");
            return Task.CompletedTask;
        }
    }
}
