using ManufacturingManager.Components.UI.MessageBox;
using ManufacturingManager.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace ManufacturingManager.Web.Components.Pages.Admin.ColorCodeMatrix
{
    public partial class ColorCodeMatrixEdit
    {
        public EditContext _editContext = default!;
        private readonly Core.Models.ColorCodeMatrix _colorCodeMatrix = new();
        private string _addEditTitle = "Add new Color Code";
        private string _errorMessage = "";
        public bool _showMessageBox;
        private InputText? _inputColor;


        private readonly MessageBoxParameters _messageBoxParameters = new();

        protected override async Task OnInitializedAsync()
        {
            _editContext = new EditContext(_colorCodeMatrix);
            var id = await SessionStorage.GetAsync<int>("ColorCodeMatrixId");
            if (id.Value != 0)
            {
                Core.Models.ColorCodeMatrix codeMatrix = _dataEntryRepository.GetColorCodeMatrixById(id.Value);
               _colorCodeMatrix.ColorCodeMatrixId = codeMatrix.ColorCodeMatrixId;
               _colorCodeMatrix.Color = codeMatrix.Color;
               _colorCodeMatrix.HexColorCode = codeMatrix.HexColorCode;
               _colorCodeMatrix.RALColorCode = codeMatrix.RALColorCode;
               _colorCodeMatrix.PantoneColor = codeMatrix.PantoneColor;
                
                _addEditTitle = $"Edit ColorCode {_colorCodeMatrix.Color}";
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
            Core.Models.ColorCodeMatrix colorCodeMatrix = new()
            {
                ColorCodeMatrixId = _colorCodeMatrix.ColorCodeMatrixId,
                Color = _colorCodeMatrix.Color,
                HexColorCode = _colorCodeMatrix.HexColorCode,
                PantoneColor = _colorCodeMatrix.PantoneColor,
                RALColorCode = _colorCodeMatrix.RALColorCode
            };
            
            bool retval;
            int id;
            //var currentUserId = CurrentUser.UserId;
            _messageBoxParameters.Title = colorCodeMatrix.ColorCodeMatrixId == 0 ? "New Color Code added." : "Color Code Updated";
            _messageBoxParameters.PageToRedirect = @"/ColorCodeList";
            _messageBoxParameters.IsErrorMessage = false;
            if (colorCodeMatrix.ColorCodeMatrixId != 0)
                retval = await _dataEntryRepository.UpdateColorCode(colorCodeMatrix);
            else
            {
                id = await _dataEntryRepository.InsertColorCode(colorCodeMatrix);
                if (id > 0) 
                    retval = true;
                else 
                    retval = false;
            }
            
            if (retval)
            {
                var successMessage = $"Color {colorCodeMatrix.Color} ";
                successMessage += colorCodeMatrix.ColorCodeMatrixId != 0 ? " updated." : " created.";
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
            NavigationManager.NavigateTo("/ColorCodeList", false, true);
            return Task.CompletedTask;
        }
        private async Task FocusFirstError()
        {
            // repeat for all inputs
            if (_editContext.GetValidationMessages(() => _colorCodeMatrix.Color).Any())
            {
                await _inputColor!.Element!.Value.FocusAsync();
            }
        }


    }
}
