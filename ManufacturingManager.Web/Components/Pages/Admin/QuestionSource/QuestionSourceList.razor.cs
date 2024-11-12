using Microsoft.JSInterop;
using PCSCaseManagement.UI.Components;


namespace PCSCaseManagement.Web.Components.Pages.Admin.QuestionSource
{
    public partial class QuestionSourceList
    {
        public IList<Core.QuestionSource> _questionSources = null!;
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
                TableName = "questionSourceTable",
                FilenameToExportExcel = "PCSCMS_QuestionSourceList",
                TitleInExcelFile = "List of Question sources"
            };
            _questionSources = await Repository.GetQuestionSourceList(true, false);
        }

        protected void ReditectToEditPage(int recordId)
        {
            SessionStorage.SetAsync("QuestionSourceId", recordId);
            NavigationManager.NavigateTo("/EditQuestionSource");
        }



        protected async void ViewLog(int id, string desc)
        {
            ArgsView = new()
            {
                TableName = "lookUpQuestions",
                FilenameToExportExcel = "PCSCMS_QuestionSourceLogFor " + desc,
                TitleInExcelFile = "Tracking log for Question source " + desc

            };

            RecordId = id;
            LogTitle = "View log for Question source " + desc;
            ShowDialogOpen = true;
            await InvokeAsync(StateHasChanged);

        }
        protected async Task DeleteRecord(int id, string description)
        {
            var message = "Please confirm deletion of Question Source " + description;


            bool confirmation = await JsRuntime.InvokeAsync<bool>("confirm", message);
            if (confirmation)
            {
                bool retvalue = await Repository.Delete(id, CurrentUser.UserId);
                if (retvalue)
                {
                    _questionSources = await Repository.GetQuestionSourceList(true, false);
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

