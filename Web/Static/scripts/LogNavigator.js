$.widget("cl.LogNavigator", {
    options: {
        host: '',
        folder: '',
        file: '',
        level: '',
        refresh: '',
        startupHost: '',
        startupFolder: ''
    },

    _create: function () {
        var me = this;

        $(me.options.refresh).button({
            icons: { primary: 'ui-icon-refresh' },
            text: false
        })
        .on('click', $.proxy(me._loadLog, me));
        
        $(me.options.host)
            .selectmenu({ width: '100%' })
            .on('selectmenuchange', function () {
                me._loadFolders();
            });

        $(me.options.folder)
            .selectmenu({ width: '100%' })
            .on('selectmenuchange', function () {
                me._loadLogLevels();
                me._loadFiles();
            });

        $(me.options.file)
            .selectmenu({ width: '100%' })
            .on('selectmenuchange', $.proxy(me._loadLog, me));

        $(me.options.level)
            .selectmenu({ width: '100%' })
            .on('selectmenuchange', function () {
                me._trigger('filter', null, { level: $(this).val() });
            });

        if (me.options.startupFolder) {
            $(me.element).one('lognavigatorloadingfolders', function (e, data) {
                data.selectedFolder = me.options.startupFolder;
            });
        }

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

    _loadFolders: function () {
        var ajaxData = { host: this.element.find(this.options.host).val() };
        if (this._trigger('loadingFolders', null, ajaxData)) {
            this._loadData(ajaxData, this.options.folder, '/Home/Folders');
        }
    },

    _loadFiles: function () {
        if (this._trigger('loadingFiles')) {
            this._loadData({
                host: this.element.find(this.options.host).val(),
                folder: this.element.find(this.options.folder).val()
            }, this.options.file, '/Home/Files');
        }
    },

    _loadLog: function () {
        var file = this.element.find(this.options.file).val();
        if (file) {
            var ajaxData = this._getReadData();
            this._trigger('read', null, ajaxData);
        }
    },

    _loadLogLevels: function () {
        if (this._trigger('loadingLogLevels')) {
            this._loadData({
                    host: this.element.find(this.options.host).val(),
                    folder: this.element.find(this.options.folder).val()
                },
                this.options.level, '/Home/LogLevels');
        }
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