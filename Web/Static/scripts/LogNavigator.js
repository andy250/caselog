$.widget("cl.LogNavigator", {
    options: {
        host: '',
        folder: '',
        file: ''
    },

    _create: function () {
        $(this.options.host).selectmenu({ width: '100%' });
        $(this.options.folder).selectmenu({ width: '100%' });
        $(this.options.file).selectmenu({ width: '100%' });
    }
});