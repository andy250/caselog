$.widget("cl.LogNavigator", {
    options: {
        host: '',
        folder: '',
        file: '',
        level: ''
    },

    _create: function () {
        var me = this;

        $(this.options.host)
            .selectmenu({ width: '100%' })
            .on('selectmenuchange', function () {
                me._loadFolders();
            });

        $(this.options.folder)
            .selectmenu({ width: '100%' })
            .on('selectmenuchange', function () {
                me._loadLogLevels();
                me._loadFiles();
            });

        $(this.options.file)
            .selectmenu({ width: '100%' })
            .on('selectmenuchange', function() {
                me._loadLog();
            });

        $(this.options.level)
            .selectmenu({ width: '100%' })
            .on('selectmenuchange', function () {
                me._trigger('filter', null, { level: $(this).val() });
            });

        $(this.options.host).trigger('selectmenuchange');
    },

    _loadData: function(data, destSelector, url) {
        var me = this;
        $.ajax({
            url: url,
            type: 'get',
            data: data
        })
            .done(function(result) {
                me.element.find(destSelector)
                    .empty()
                    .append(result)
                    .selectmenu('refresh')
                    .trigger('selectmenuchange');
            });
    },

    _loadFolders: function() {
        this._loadData({ host: this.element.find(this.options.host).val() }, this.options.folder, '/Home/Folders');
    },

    _loadFiles: function() {
        this._loadData({
            host: this.element.find(this.options.host).val(),
            folder: this.element.find(this.options.folder).val()
        }, this.options.file, '/Home/Files');
    },

    _loadLog: function () {
        this._trigger('read', null, {
            host: this.element.find(this.options.host).val(),
            folder: this.element.find(this.options.folder).val(),
            file: this.element.find(this.options.file).val(),
            level: this.element.find(this.options.level).val()
        });
    },

    _loadLogLevels: function() {
        this._loadData({
                host: this.element.find(this.options.host).val(),
                folder: this.element.find(this.options.folder).val()
            },
            this.options.level, '/Home/LogLevels');
    }
});