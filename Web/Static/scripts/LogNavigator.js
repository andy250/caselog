$.widget("cl.LogNavigator", {
    options: {
        host: '',
        folder: '',
        file: '',
        level: '',
        refresh: ''
    },

    _create: function () {
        var me = this;

        $(this.options.refresh).button({
            icons: { primary: 'ui-icon-refresh' }
        })
        .on('click', function () { me._trigger('read', null, me._getReadData()); });

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
                me.element.find(destSelector).empty();
                me.element.find(destSelector).append(result.trim()).selectmenu('refresh').trigger('selectmenuchange');
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
        var file = this.element.find(this.options.file).val();
        if (file) {
            this._trigger('read', null, this._getReadData());
        }
    },

    _loadLogLevels: function() {
        this._loadData({
                host: this.element.find(this.options.host).val(),
                folder: this.element.find(this.options.folder).val()
            },
            this.options.level, '/Home/LogLevels');
    },

    _getReadData: function() {
        return {
            host: this.element.find(this.options.host).val(),
            folder: this.element.find(this.options.folder).val(),
            file: this.element.find(this.options.file).val(),
            level: this.element.find(this.options.level).val()
        };
    }
});