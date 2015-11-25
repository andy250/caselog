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