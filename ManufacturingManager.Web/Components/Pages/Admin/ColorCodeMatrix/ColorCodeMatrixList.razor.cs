using ManufacturingManager.Components.UI;
using Microsoft.JSInterop;

namespace ManufacturingManager.Web.Components.Pages.Admin.ColorCodeMatrix
{
    public partial class ColorCodeMatrixList
    {
        public IList<Core.Models.ColorCodeMatrix> _colorCodeMatrices = null!;
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
                TableName = "categoryTable",
                FilenameToExportExcel = "PCSCMS_CategoryList",
                TitleInExcelFile = "List of Categories"
            };
            _colorCodeMatrices =  _DataEntryRepository.GetColorCodeMatrix().ToList();
        }

        protected void ReditectToEditPage(int recordId)
        {
            SessionStorage.SetAsync("CategoryId", recordId);
            NavigationManager.NavigateTo("/EditCategory");
        }



        protected async void ViewLog(int id, string desc)
        {
            ArgsView = new()
            {
                TableName = "lookUpCategory",
                FilenameToExportExcel = "PCSCMS_CategoryLogFor " + desc,
                TitleInExcelFile = "Tracking log for Category " + desc

            };

            RecordId = id;
            LogTitle = "View log for Category " + desc;
            ShowDialogOpen = true;
            await InvokeAsync(StateHasChanged);
        }
        protected async Task DeleteRecord(int id, string description)
        {
            var message = "Please confirm deletion of category " + description;


            bool confirmation = await JsRuntime.InvokeAsync<bool>("confirm", message);
            if (confirmation)
            {
                bool retvalue = true;//await _DataEntryRepository.DeleteColorCode(id, CurrentUser.UserId);
                if (retvalue)
                {
                    _colorCodeMatrices =  _DataEntryRepository.GetColorCodeMatrix().ToList();
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    await JsRuntime.InvokeVoidAsync("alert", "Unable to delete this record");

                }
            }
        }
    }
}
