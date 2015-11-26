$.ajaxSetup({
    error: function (jqXHR, textStatus, errorThrown) {
        $('<div>').text(errorThrown).dialog({
            buttons: {
                Ok: function () {
                    $(this).dialog("close");
                }
            }
        });
    }
});

String.prototype.format = function () {
    var s = this,
        i = arguments.length;

    while (i--) {
        s = s.replace(new RegExp('\\{' + i + '\\}', 'gm'), arguments[i]);
    }
    return s;
};