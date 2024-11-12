using PCSCaseManagement.Core;
using PCSCaseManagement.UI.Components.MessageBox;
using PCSCaseManagement.Web.Data;

namespace PCSCaseManagement.Web.Components.Pages.Admin.QuestionSource
{
    public partial class EditQuestionSource
    {
        private LookUpTable _questionSource = null!;
        private string _addEditTitle = "Add new Question Source";
        private string _errorMessage = "";
        public bool _showMessageBox;
        private readonly MessageBoxParameters _messageBoxParameters = new();

        protected override async Task OnInitializedAsync()
        {
            _questionSource = new LookUpTable
            {
                Active = true
            };
            var id = await SessionStorage.GetAsync<int>("QuestionSourceId");
            if (id.Value != 0)
            {

                Core.QuestionSource p = await Repository.QuestionSource(id.Value);
                _questionSource.Id = p.QuestionSourceId;
                _questionSource.Description = p.Description;
                _questionSource.Active = p.Active;
                _addEditTitle = $"Edit Question Source {p.Description}";
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
            AppCache.QuestionSourceList = null;
            Core.QuestionSource questionSource = new()
            {
                Active = _questionSource.Active,
                QuestionSourceId = _questionSource.Id,
                Description = _questionSource.Description
            };
            bool retval;

            var currentUserId = CurrentUser.UserId;
            _messageBoxParameters.Title = questionSource.QuestionSourceId == 0 ? "New question source added." : "Question source Updated";
            _messageBoxParameters.PageToRedirect = @"/QuestionSourceList";
            _messageBoxParameters.IsErrorMessage = false;
            if (questionSource.QuestionSourceId != 0)
                retval = await Repository.Update(questionSource, currentUserId);
            else
                retval = await Repository.Add(questionSource, currentUserId);
            if (retval)
            {
                var successMessage = $"Question source {questionSource.Description} ";
                successMessage += questionSource.QuestionSourceId != 0 ? " updated." : " created.";
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
            NavigationManager.NavigateTo("/QuestionSourceList");
            return Task.CompletedTask;
        }


    }
}
