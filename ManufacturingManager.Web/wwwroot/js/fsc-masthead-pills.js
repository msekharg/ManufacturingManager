// FSC Pills Masthead v1.1.0 
$(document).ready(function () {
    var fscPills = $(".stacked-nav .nav.nav-pills.fsc-pills-nav > li");
    //PILLS NAV
    $(fscPills).click(function (e) {
        var currentPill = this;
        var currentPillChild = $(this).children('ul').children('li');
        if ($(currentPill).children("ul").length) {
            $(currentPillChild).click(function () {
                $(fscPills).removeClass('active');
                $(currentPill).addClass('active');
            });
        } else {
            $(fscPills).removeClass('active');
            $(currentPill).addClass('active');
        }
    });
    $('.fsc-pills-nav a').focusin(function (e) {
        var isDropdown = $(this).parents().hasClass('fsc-pills-nav');
        if (!isDropdown) {
            fscPills.removeClass('open');
        }
    });
    $(fscPills).focusin(function (e) {
        var currentPill = this;
        if (!$(currentPill).hasClass('open')) {
            $(fscPills).removeClass('open');
        }
    });
    $("#skipLink").click(function () {
        if ($('#pageContent').length == 0) {
            $(".main-container input,  .main-container textarea, .main-container a").first().focus();
        } else {
            $("#pageContent").focus();
        }
    });
});


