// FSC Footer v1.0.0
//affixed masthead and footer adjustment for content height
export function footerAffix() {
   /* alert("adjust footer");
     debugger;*/
    window.$(document).ready(function () {
       
        var setFooter = function() {
            var footerHeight = window.$(".footer").height();
            var mainContainerPadding = footerHeight + 30; 
            window.$('.main-container').css('padding-bottom', mainContainerPadding);
            
        };

        setTimeout(function() {
                setFooter();
            },
            300);
        window.$(window).on('resize', setFooter);
    });

}

