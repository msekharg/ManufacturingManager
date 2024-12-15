using ManufacturingManager.Components.UI.MessageBox;
using ManufacturingManager.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace ManufacturingManager.Web.Components.Pages.Admin.MidRailConfiguration
{
    public partial class MidRailConfigurationEdit
    {
        public EditContext _editContext = default!;
        private readonly Core.Models.MidRailConfiguration _midRailConfiguration = new();
        private string _addEditTitle = "Add new MidRail Configuration";
        private string _errorMessage = "";
        public bool _showMessageBox;
        private InputText? _inputPartNumber;
        private InputNumber<int> _inputHeight = null!;
        private InputNumber<decimal> _inputThickness = null!;
        private InputNumber<int> _inputLength = null!;
        private InputNumber<decimal> _inputRailWeight = null!;
        
        private readonly MessageBoxParameters _messageBoxParameters = new();

        protected override async Task OnInitializedAsync()
        {
            _editContext = new EditContext(_midRailConfiguration);
            //_category.Active = true;
            var id = await SessionStorage.GetAsync<int>("MidRailConfigurationId");
            if (id.Value != 0)
            {
                Core.Models.MidRailConfiguration midRailConfiguration = _dataEntryRepository.GetMidRailConfigurationById(id.Value);
                _midRailConfiguration.MidRailConfigurationId = midRailConfiguration.MidRailConfigurationId;
                _midRailConfiguration.PartNumber = midRailConfiguration.PartNumber;
                _midRailConfiguration.Height = midRailConfiguration.Height;
                _midRailConfiguration.Thickness = midRailConfiguration.Thickness;
                _midRailConfiguration.Length = midRailConfiguration.Length;
                
                _addEditTitle = $"Edit configuration {_midRailConfiguration.PartNumber}";
            }
        }
        private void MessageBoxClose()
        {
            _showMessageBox = false;
            StateHasChanged();
        }
        protected async Task Save()
        {
            AppCache.ClampsPositioningConfigurations = null;
            Core.Models.MidRailConfiguration midRailConfiguration = new()
            {
                MidRailConfigurationId = _midRailConfiguration.MidRailConfigurationId,
                PartNumber = _midRailConfiguration.PartNumber,
                Height = _midRailConfiguration.Height,
                Thickness = Math.Round((decimal)_midRailConfiguration.Thickness, 2),
                Length = _midRailConfiguration.Length,
                RailWeight = Math.Round((decimal)_midRailConfiguration.RailWeight , 3)
                   };
            bool retval;
            int recordsUpdated = 0;
            int id;
            //var currentUserId = CurrentUser.UserId;
            _messageBoxParameters.Title = midRailConfiguration.MidRailConfigurationId == 0 ? "New Configuration added." : "Configuration Updated";
            _messageBoxParameters.PageToRedirect = @"/MidRailConfigurationList";
            _messageBoxParameters.IsErrorMessage = false;
            if (midRailConfiguration.MidRailConfigurationId != 0)
            {
                recordsUpdated = await _dataEntryRepository.UpdateMidRailConfiguration(midRailConfiguration);
                retval = recordsUpdated > 0;
            }
            else
            {
                id = await _dataEntryRepository.InsertMidRailConfiguration(midRailConfiguration);
                retval = id > 0;
            }

            if (retval)
            {
                var successMessage = $"PartNumber {midRailConfiguration.PartNumber} successfully";
                successMessage += midRailConfiguration.MidRailConfigurationId != 0 ? " updated." : " created.";
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
            NavigationManager.NavigateTo("/MidRailConfigurationList", false, true);
            return Task.CompletedTask;
        }
        private async Task FocusFirstError()
        {
            // repeat for all inputs
            if (_editContext.GetValidationMessages(() => _midRailConfiguration.PartNumber).Any())
            {
                await _inputPartNumber!.Element!.Value.FocusAsync();
            }
            else if (_editContext.GetValidationMessages(() => _midRailConfiguration.Height).Any())
            {
                await _inputHeight!.Element!.Value.FocusAsync();
            }
            else if (_editContext.GetValidationMessages(() => _midRailConfiguration.Thickness).Any())
            {
                await _inputThickness!.Element!.Value.FocusAsync();
            }
            else if (_editContext.GetValidationMessages(() => _midRailConfiguration.Length).Any())
            {
                await _inputLength!.Element!.Value.FocusAsync();
            }
            else if (_editContext.GetValidationMessages(() => _midRailConfiguration.RailWeight).Any())
            {
                await _inputRailWeight!.Element!.Value.FocusAsync();
            }
           
        }
    }
}
