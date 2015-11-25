$.widget("cl.LogReader", {
    options: {
        log: '',
        logEntry: ''
    },

    _create: function () {
    },

    read: function(data) {
        var me = this;
        $.ajax({
                url: '/Home/ReadFile',
                data: data,
                type: 'get'
            })
            .done(function(result) {
                me.element.find(me.options.log).html(result);
                me.filter(data);
                me.element.scrollTop(me.element[0].scrollHeight);
            });
    },

    filter: function (data) {
        if (data.level === "ALL") {
            this.element.find(this.options.logEntry).show();
        } else {
            this.element.find(this.options.logEntry).hide().filter('[data-level=' + data.level + ']').show();
        }
    }
});
