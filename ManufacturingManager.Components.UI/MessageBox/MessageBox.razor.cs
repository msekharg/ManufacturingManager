
/*Summary

 This component is used to show a pop Up message receiving call MessageBoxParameters as parameter 
 In the component that use this component you can implementing as
 define a boolean variable ShowDialogOpen and initialize it as false to avoid this popup appears
 
 public bool ShowMessageBox { get; set; }
 private MessageBoxParameters _messageBoxParameters = new();

 at the end of your HTML part include
  @if (ShowMessageBox)
{
    <MessageBox MessageBoxParameters="@_messageBoxParameters"  OnClose="@MessageBoxClose"></MessageBox>
}
To show this popup , for example in the method save, you can show a message if you have an error or a different one if it
was ok, and redirect to another page
if (saveOk) -- this is your condition 
 {
    _messageBoxParameters.Message="Everything is ok.....etc";
    _messageBoxParameters.Title = "Record Added"
    _messageBoxParameters.PageToRedirect = @"/MyList"; 
    _messageBoxParameters.isErrorMessage  = false;
 }
else 
 {
   _messageBoxParameters.Message=" Something wrong.....etc";
    _messageBoxParameters.Title = "Unable to save the record"
    _messageBoxParameters.PageToRedirect = @"";  --No Redirect, then when user clicks OK it will be in the same page
    _messageBoxParameters.isErrorMessage  = true;
 }

and define a method to close the pop up
  private void MessageBoxClose()
    {
        ShowMessageBox = false;
        StateHasChanged();
    }
508 Note.
When firstRender is true in OnAfterRender,we set focus in the first element defined in the html code  as => @ref="_focusFirstElementFor508"
And, to avoid losing focus inside the popup window, add a method  KeyPress(KeyboardEventArgs e) in the last element into the pop up.
for example call this method in @onkeydown, in Ok button 
 <button type="button" class="btn btn-primary" data-bs-dismiss="modal" @onclick="@ClickOk" 
                        @onkeydown="@KeyPress"
                        role="button" aria-pressed ="false">Ok</button>
 This will set the focus in the first element defined in the html code => @ref="_focusFirstElementFor508" if tab key is pressed in the last element

*/

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace ManufacturingManager.Components.UI.MessageBox
{
    public partial class MessageBox : ComponentBase
    {

        [Parameter] public MessageBoxParameters? MessageBoxParameters { get; set; }
        [Parameter] public EventCallback<bool> OnClose { get; set; }
        
        [Inject]
        public NavigationManager _NavigationManager { get; set; }
        private string? ModalCss { set; get; }
        private string? BorderCss { set; get; }

        private ElementReference _focusFirstElementFor508;
        protected override void OnInitialized()
        {
            if (MessageBoxParameters is { IsErrorMessage: true })
            {
                ModalCss = "bg-danger";
                BorderCss = "border-danger";
            }
            else
            {
                ModalCss = "bg-success";
                BorderCss = "border-success";
            }
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {

                //Set Focus 1 first element for 508 users
                SetFocusFirstElement();
            }
        }

        public Task ClickOk()
        {
            if (!string.IsNullOrEmpty(MessageBoxParameters?.PageToRedirect))
            {
                _NavigationManager.NavigateTo(MessageBoxParameters.PageToRedirect);
            }

            return OnClose.InvokeAsync(true);
        }

        public void KeyPress(KeyboardEventArgs e)
        {
         
            if (e.Key == "Tab")
               SetFocusFirstElement();
            
        }

        public void QuitOnEscape(KeyboardEventArgs e)
        {
           if (e.Key == "Escape")
                ClickOk();
        }

        public async void SetFocusFirstElement()
        {
            await _focusFirstElementFor508.FocusAsync();
        }

    }
}

