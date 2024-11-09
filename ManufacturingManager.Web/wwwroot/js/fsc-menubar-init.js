
(function () {
    setTimeout(function () {
        var fscMenubar = document.getElementById('fscMenubar');
        if (fscMenubar) {
            console.log("menubar detected");
            var menubar = new Menubar(fscMenubar);
            menubar.init();
        }
    }, 500);
}());