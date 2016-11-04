//关闭广告条
$('#news-ticker-close').click(function (e) {
    $('.news-ticker').remove();
    $('.quick-sidebar').css('top', '50px');
});

//初始化全局搜索
$('#topbar-search').on('click', function (e) {
    $(this).addClass('open');
    $(this).find('.form-control').focus();

    $('#topbar-search .form-control').on('blur', function (e) {
        $(this).closest('#topbar-search').removeClass('open');
        $(this).unbind('blur');
    });
});

//初始化左侧菜单
$('#sidebar').css('min-height', '100%');
$('#side-menu').metisMenu();
$(window).on("load resize", function () {
    if ($(this).width() < 768) {
        $('body').removeClass();
        $('div.sidebar-collapse').addClass('collapse');
    } else {
        $('body').addClass($.cookie('menu_style') + ' ' + $.cookie('header'));
        $('div.sidebar-collapse').removeClass('collapse');
        $('div.sidebar-collapse').css('height', 'auto');
    }

    if ($('#sidebar').height() > $('#page-wrapper').height()) {
        $('#wrapper').css('height', $('#sidebar').height());
    }
});

//初始化回到顶部
$(window).scroll(function () {
    if ($(this).scrollTop() < 200) {
        $('#totop').fadeOut();
    } else {
        $('#totop').fadeIn();
    }
});
$('#totop').on('click', function () {
    $('html, body').animate({ scrollTop: 0 }, 'fast');
    return false;
});

//初始化表单checkbox、radio
$('input[type="checkbox"]:not(".switch")').iCheck({
    checkboxClass: 'icheckbox_minimal-grey',
    increaseArea: '20%' // optional
}).each(function (n) {
    var obj = $(this);

    var array = '';

    if (obj.attr('data-value')) {
        array = ',' + obj.attr('data-value') + ',';
    }

    var cur = ',' + obj.val() + ',';

    if (array.indexOf(cur) >= 0) {
        obj.iCheck('check');
    }
});
$('input[type="radio"]:not(".switch")').iCheck({
    radioClass: 'iradio_minimal-grey',
    increaseArea: '20%' // optional
}).each(function (n) {
    var obj = $(this);

    if (obj.val() == obj.attr('data-value')) {
        obj.iCheck('check');
    }
});

// 初始化select2
$('.select2').select2();

$('.select2').each(function (n) {
    var obj = $(this);

    var id = obj.attr('data-value');

    if (id) {
        obj.select2('val', id);
    }
    else {
        obj.select2('val', '');
    }
})

// 初始化datetimepicker
$('.datetimepicker-default').datetimepicker({
    format: "YYYY-MM-DD HH:mm"
});

$('.datetimepicker-disable-date').datetimepicker({
    pickDate: false,
    format: "HH:mm"
});

$('.datetimepicker-disable-time').datetimepicker({
    pickTime: false,
    format: "YYYY-MM-DD"
});

$('.datetimepicker-start').datetimepicker({
    format: "YYYY-MM-DD HH:mm"
});

$('.datetimepicker-end').datetimepicker({
    format: "YYYY-MM-DD HH:mm"
});

$('.datetimepicker-start').on("change.dp", function (e) {
    $('.datetimepicker-end').data("DateTimePicker").setStartDate(e.date);
});

$('.datetimepicker-end').on("change.dp", function (e) {
    $('.datetimepicker-start').data("DateTimePicker").setEndDate(e.date);
});

// 初始化列表全选插件
$('.checkall').on('ifChecked ifUnchecked', function (event) {
    if (event.type == 'ifChecked') {
        $(this).closest('table').find('input[type=checkbox]').iCheck('check');
    } else {
        $(this).closest('table').find('input[type=checkbox]').iCheck('uncheck');
    }
});

//初始化amy插件
var amy = {};

amy.alert = function (msg, callback) {
    var temp = '<div id="amy-alert" class="modal fade in" aria-hidden="false" role="dialog" tabindex="-1" style="display: block;">' +
                    '<div class="modal-dialog">' +
                        '<div class="modal-content">' +
                            '<div class="modal-body">${content}</div>' +
                            '<div class="modal-footer">' +
                                '<button class="btn btn-primary sure" data-dismiss="modal" type="button">确定</button>' +
                            '</div>' +
                        '</div>' +
                    '</div>' +
                '</div>';

    var obj = $('#amy-alert');

    if (!obj[0]) {
        $("body").append(temp.replace('${content}', msg || 'What are you 弄啥嘞！'));

        $('#amy-alert .sure').on('click', function () {
            $('#amy-alert').remove();

            if (callback)
                callback();
        })

        //$('#amy-alert .cancel').on('click', function () {
        //    $('#amy-alert').remove();
        //})
    }
}

amy.confirm = function (msg, callback) {
    var temp = '<div id="amy-confirm" class="modal fade in" aria-hidden="false" role="dialog" tabindex="-1" style="display: block;">' +
                    '<div class="modal-dialog">' +
                        '<div class="modal-content">' +
                            '<div class="modal-body">${content}</div>' +
                            '<div class="modal-footer">' +
                                '<button class="btn btn-default cancel" data-dismiss="modal" type="button">取消</button>' +
                                '<button class="btn btn-primary sure" data-dismiss="modal" type="button">确定</button>' +
                            '</div>' +
                        '</div>' +
                    '</div>' +
                '</div>';

    var obj = $('#amy-confirm');

    if (!obj[0]) {
        $("body").append(temp.replace('${content}', msg || 'What are you 弄啥嘞！'));

        $('#amy-confirm .cancel').on('click', function () {
            $('#amy-confirm').remove();
        })

        $('#amy-confirm .sure').on('click', function () {
            $('#amy-confirm').remove();
            callback();
        })
    }
}

amy.modal = function (url, data, callback) {
    var id = amy.guid();

    var temp = '<div id="' + id + '" class="modal fade in" aria-hidden="false" aria-labelledby="modal-login-label" role="dialog" tabindex="-1" style="display: block;">' +
                    '<div class="modal-dialog">' +
                        '<div class="modal-content">' +
                            '${content}' +
                        '</div>' +
                    '</div>' +
                '</div>';

    $.get(url, data, function (res) {
        $("body").append(temp.replace('${content}', res || 'What are you 弄啥嘞！'));

        $('#' + id + ' .close').on('click', function () {
            $('#' + id).remove();
        })

        $('#' + id + ' .sure').on('click', function () {
            $('#amy-form').myValidate(function (form) {
                $(form).ajaxSubmit({
                    success: function (res) {
                        //表单数据转换成json对象
                        var serializeObj = {};
                        $($(form).serializeArray()).each(function () {
                            serializeObj[this.name] = this.value;
                        });

                        callback(serializeObj, res);
                        $('#' + id).remove();
                    },
                    error: function () {

                    },
                    dataType: "json"
                })
            });
        })
    })
};

amy.notific8 = function (msg, setting) {
    var option = {
        theme: 'teal',//teal、amethyst、ruby、tangerine、lemon、lime、ebony、smoke
        sticky: false,//true、false
        horizontalEdge: 'top',//top、bottom
        verticalEdge: 'right',//right、left
        heading: '系统提醒',
        life: 2000//3秒
    };

    option = $.extend({}, option, setting);

    $.notific8(msg || 'What are you 弄啥嘞！', option);
}

amy.toastr = function (type, msg, title, setting) {
    toastr.options = {//参数设置，若用默认值可以省略以下面代
        closeButton: false, //是否显示关闭按钮
        debug: false, //是否使用debug模式
        positionClass: "toast-top-right",//弹出窗的位置
        showDuration: "300",//显示的动画时间
        hideDuration: "1000",//消失的动画时间
        timeOut: "3000", //展现时间
        extendedTimeOut: "1000",//加长展示时间
        showEasing: "swing",//显示时的动画缓冲方式
        hideEasing: "linear",//消失时的动画缓冲方式
        showMethod: "fadeIn",//显示时的动画方式
        hideMethod: "fadeOut" //消失时的动画方式
    };

    toastr.options = $.extend({}, toastr.options, setting);

    toastr[type](msg, title);//toastr.success(msg, title)
}

//生成guid
amy.guid = function () {
    var guid = "";
    for (var i = 1; i <= 32; i++) {
        var n = Math.floor(Math.random() * 16.0).toString(16);
        guid += n;
        if ((i == 8) || (i == 12) || (i == 16) || (i == 20))
            guid += "";
    }
    return guid + "";
};

String.prototype.removeBlankSpace = function () {
    return this.replace(/\s+/g, "");
}

String.prototype.removeEnter = function () {
    return this.replace(/[\r\n]/g, "");
}