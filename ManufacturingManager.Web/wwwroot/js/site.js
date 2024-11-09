// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

window.blazor_setExitEvent = function (dotNetHelper) {
    blazor_dotNetHelper = dotNetHelper;
    window.addEventListener("beforeunload", blazor_SetSPAClosed);
}

var blazor_dotNetHelper;

window.blazor_SetSPAClosed = function () {
    blazor_dotNetHelper.invokeMethodAsync('SPASessionClosed');
    blazor_dotNetHelper.dispose();
}
