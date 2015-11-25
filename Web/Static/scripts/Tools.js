$.widget("cl.Tools", {
    _create: function () {
        this.element.find('#reload-config')
            .button()
            .on('click', function () {
                $.ajax({ url: '/Home/ReloadConfig', type: 'get' }).done(function () { location.reload(true); });
            });
    }
});
