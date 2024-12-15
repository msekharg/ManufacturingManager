using ManufacturingManager.Components.UI.MessageBox;
using ManufacturingManager.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace ManufacturingManager.Web.Components.Pages.Admin.ClampsPositioning
{
    public partial class ClampsPositioningEdit
    {
        public EditContext _editContext = default!;
        private readonly Core.Models.ClampsPositioning _clampsPositioning = new();
        private string _addEditTitle = "Add new Clamps Position";
        private string _errorMessage = "";
        public bool _showMessageBox;
        private InputText? _inputCXPX;


        private readonly MessageBoxParameters _messageBoxParameters = new();

        protected override async Task OnInitializedAsync()
        {
            _editContext = new EditContext(_clampsPositioning);
            var id = await SessionStorage.GetAsync<int>("ClampsPositioningId");
            if (id.Value != 0)
            {
                Core.Models.ClampsPositioning clampsPositioning = _dataEntryRepository.GetClampsPositioningById(id.Value);
               _clampsPositioning.ClampsPositioningId = clampsPositioning.ClampsPositioningId;
               _clampsPositioning.CXPX = clampsPositioning.CXPX;
               _clampsPositioning.CX = clampsPositioning.CX;
               _clampsPositioning.Clamp1 = clampsPositioning.Clamp1;
               _clampsPositioning.Clamp2 = clampsPositioning.Clamp2;
               _clampsPositioning.PX = clampsPositioning.PX;
               _clampsPositioning.Clamp3 = clampsPositioning.Clamp3;
               _clampsPositioning.Clamp4 = clampsPositioning.Clamp4;
               _clampsPositioning.HoleSetDrilled = clampsPositioning.HoleSetDrilled;
                
                _addEditTitle = $"Edit Clamps Position: {_clampsPositioning.CXPX}";
            }
        }
        private void MessageBoxClose()
        {
            _showMessageBox = false;
            StateHasChanged();
        }
        protected async Task Save()
        {
            AppCache.ColorCodeMatrices = null;
            Core.Models.ClampsPositioning clampsPositioning = new()
            {
                ClampsPositioningId = _clampsPositioning.ClampsPositioningId,
                CXPX = _clampsPositioning.CXPX,
                CX = _clampsPositioning.CX,
                Clamp1 = _clampsPositioning.Clamp1,
                Clamp2 = _clampsPositioning.Clamp2,
                PX = _clampsPositioning.PX,
                Clamp3 = _clampsPositioning.Clamp3,
                Clamp4 = _clampsPositioning.Clamp4,
                HoleSetDrilled = _clampsPositioning.HoleSetDrilled,
            };
            
            bool retval;
            int id;
            //var currentUserId = CurrentUser.UserId;
            _messageBoxParameters.Title = clampsPositioning.ClampsPositioningId == 0 ? "New Color Code added." : "Color Code Updated";
            _messageBoxParameters.PageToRedirect = @"/ClampsPositioningList";
            _messageBoxParameters.IsErrorMessage = false;
            if (clampsPositioning.ClampsPositioningId != 0)
                id = await _dataEntryRepository.UpdateClampsPositioning(clampsPositioning);
            else
            {
                id = await _dataEntryRepository.InsertClampsPositioning(clampsPositioning);
            }
            
            if (id > 0) 
                retval = true;
            else 
                retval = false;
            
            if (retval)
            {
                var successMessage = $"Clamps Position: {clampsPositioning.CXPX} ";
                successMessage += clampsPositioning.ClampsPositioningId != 0 ? " updated." : " created.";
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
            // StateHasChanged();
        }

        protected Task ReturnToList()
        {
            NavigationManager.NavigateTo("/ClampsPositioningList", false, true);
            return Task.CompletedTask;
        }
        private async Task FocusFirstError()
        {
            // repeat for all inputs
            if (_editContext.GetValidationMessages(() => _clampsPositioning.CXPX).Any())
            {
                await _inputCXPX!.Element!.Value.FocusAsync();
            }
        }


    }
}
