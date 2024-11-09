
    console.log("plug in loaded");
    $.fn.DataTable.ext.pager.full_numbers_no_ellipses = function (page, pages) {

        
        var buttons = $.fn.DataTable.ext.pager.numbers_length;
        var half = Math.floor(buttons / 2);

        var range = function (len, start) {
            var end;

            if (typeof start === "undefined") {
                start = 0;
                end = len;
            } else {
                end = start;
                start = len;
            }
            var out = [];
            for (var i = start; i < end; i++) { out.push(i); }
            return out;
        };

        if (pages <= buttons) {
            numbers = range(0, pages);
        } else if (page <= half) {
            numbers = range(0, buttons);
        } else if (page >= pages - 1 - half) {
            numbers = range(pages - buttons, pages);
        } else {
            numbers = range(page - half, page + half + 1);
        }

        numbers.DT_el = 'span';
        return ['first', 'previous', numbers, 'next', 'last'];

    };
