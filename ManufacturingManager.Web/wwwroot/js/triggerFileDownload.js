window.triggerFileDownload = (args) => {
    //debugger;
    const anchorElement = document.createElement('a');
    anchorElement.href = args.url;
    anchorElement.download = args.fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
}
