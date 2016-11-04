(function ($) {
    $.extend($.fn, {
        multuploader: function (opt) {
            var container = this;

            var option = {
                pick: {
                    id: container.find('.filepicker'),//'#filePicker',
                    label: '点击选择图片'
                },
                dnd: container.find('.queueList'),//'#uploader .queueList',
                paste: document.body,

                accept: {
                    title: 'Images',
                    extensions: 'gif,jpg,jpeg,bmp,png',
                    mimeTypes: 'image/*'
                },

                // swf文件路径
                swf: '/content/mgr/vendors/webuploader/Uploader.swf',
                server: '/admin/plugin/uploadsliverfile',
                disableGlobalDnd: true,

                chunked: true,
                fileNumLimit: 300,
                fileSizeLimit: 5 * 1024 * 1024,    // 200 M
                fileSingleSizeLimit: 1 * 1024 * 1024    // 50 M
            };

            option = $.extend(option, opt);

            // 图片容器
            var queue = $('<ul class="filelist"></ul>').appendTo(container.find('.queueList'));

            // 状态栏，包括进度和控制按钮
            var statusBar = container.find('.statusBar');

            // 文件总体选择信息
            var info = statusBar.find('.info');

            // 上传按钮
            var btn_upload = container.find('.uploadBtn');

            // 没选择文件之前的内容。
            var placeHolder = container.find('.placeholder');

            // 总体进度条
            var progress = statusBar.find('.progress').hide();

            // 添加的文件数量
            var fileCount = 0;

            // 添加的文件总大小
            var fileSize = 0;

            // 优化retina, 在retina下这个值是2
            var ratio = window.devicePixelRatio || 1;

            // 缩略图大小
            var thumbnailWidth = 110 * ratio;
            var thumbnailHeight = 110 * ratio;

            // 可能有pedding, ready, uploading, confirm, done.
            var state = 'pedding';

            // 所有文件的进度信息，key为file id
            var percentages = {};

            var supportTransition = (function () {
                var s = document.createElement('p').style,
                    r = 'transition' in s ||
                          'WebkitTransition' in s ||
                          'MozTransition' in s ||
                          'msTransition' in s ||
                          'OTransition' in s;
                s = null;
                return r;
            })();

            if (!WebUploader.Uploader.support()) {
                alert('Web Uploader 不支持您的浏览器！如果你使用的是IE浏览器，请尝试升级 flash 播放器');
                throw new Error('WebUploader does not support the browser you are using.');
            }

            // 实例化
            var uploader = WebUploader.create(option);

            // 添加“添加文件”的按钮
            uploader.addButton({
                id: container.find('.filePicker2'),//'#filePicker2',
                label: '继续添加'
            });

            // 当有文件添加进来时执行，负责view的创建
            function addFile(file) {
                var $li = $('<li id="' + file.id + '">' +
                        '<p class="title">' + file.name + '</p>' +
                        '<p class="imgWrap"></p>' +
                        '<p class="progress"><span></span></p>' +
                        '</li>'),

                    $btns = $('<div class="file-panel">' +
                        '<span class="cancel">删除</span>' +
                        '<span class="rotateRight">向右旋转</span>' +
                        '<span class="rotateLeft">向左旋转</span></div>').appendTo($li),
                    $prgress = $li.find('p.progress span'),
                    container = $li.find('p.imgWrap'),
                    $info = $('<p class="error"></p>'),

                    showError = function (code) {
                        switch (code) {
                            case 'exceed_size':
                                text = '文件大小超出';
                                break;

                            case 'interrupt':
                                text = '上传暂停';
                                break;

                            default:
                                text = '上传失败，请重试';
                                break;
                        }

                        $info.text(text).appendTo($li);
                    };

                if (file.getStatus() === 'invalid') {
                    showError(file.statusText);
                } else {
                    // @todo lazyload
                    container.text('预览中');
                    uploader.makeThumb(file, function (error, src) {
                        if (error) {
                            container.text('不能预览');
                            return;
                        }

                        var img = $('<img src="' + src + '">');
                        container.empty().append(img);
                    }, thumbnailWidth, thumbnailHeight);

                    percentages[file.id] = [file.size, 0];
                    file.rotation = 0;
                }

                file.on('statuschange', function (cur, prev) {
                    if (prev === 'progress') {
                        $prgress.hide().width(0);
                    } else if (prev === 'queued') {
                        $li.off('mouseenter mouseleave');
                        $btns.remove();
                    }

                    // 成功
                    if (cur === 'error' || cur === 'invalid') {
                        console.log(file.statusText);
                        showError(file.statusText);
                        percentages[file.id][1] = 1;
                    } else if (cur === 'interrupt') {
                        showError('interrupt');
                    } else if (cur === 'queued') {
                        percentages[file.id][1] = 0;
                    } else if (cur === 'progress') {
                        $info.remove();
                        $prgress.css('display', 'block');
                    } else if (cur === 'complete') {
                        $li.append('<span class="success"></span>');
                    }

                    $li.removeClass('state-' + prev).addClass('state-' + cur);
                });

                $li.on('mouseenter', function () {
                    $btns.stop().animate({ height: 30 });
                });

                $li.on('mouseleave', function () {
                    $btns.stop().animate({ height: 0 });
                });

                $btns.on('click', 'span', function () {
                    var index = $(this).index(),
                        deg;

                    switch (index) {
                        case 0:
                            uploader.removeFile(file);
                            return;

                        case 1:
                            file.rotation += 90;
                            break;

                        case 2:
                            file.rotation -= 90;
                            break;
                    }

                    if (supportTransition) {
                        deg = 'rotate(' + file.rotation + 'deg)';
                        container.css({
                            '-webkit-transform': deg,
                            '-mos-transform': deg,
                            '-o-transform': deg,
                            'transform': deg
                        });
                    } else {
                        container.css('filter', 'progid:DXImageTransform.Microsoft.BasicImage(rotation=' + (~~((file.rotation / 90) % 4 + 4) % 4) + ')');
                        // use jquery animate to rotation
                        // $({
                        //     rotation: rotation
                        // }).animate({
                        //     rotation: file.rotation
                        // }, {
                        //     easing: 'linear',
                        //     step: function( now ) {
                        //         now = now * Math.PI / 180;

                        //         var cos = Math.cos( now ),
                        //             sin = Math.sin( now );

                        //         container.css( 'filter', "progid:DXImageTransform.Microsoft.Matrix(M11=" + cos + ",M12=" + (-sin) + ",M21=" + sin + ",M22=" + cos + ",SizingMethod='auto expand')");
                        //     }
                        // });
                    }


                });

                $li.appendTo(queue);
            }

            // 负责view的销毁
            function removeFile(file) {
                var $li = $('#' + file.id);

                delete percentages[file.id];
                updateTotalProgress();
                $li.off().find('.file-panel').off().end().remove();
            }

            function updateTotalProgress() {
                var loaded = 0,
                    total = 0,
                    spans = progress.children(),
                    percent;

                $.each(percentages, function (k, v) {
                    total += v[0];
                    loaded += v[0] * v[1];
                });

                percent = total ? loaded / total : 0;

                spans.eq(0).text(Math.round(percent * 100) + '%');
                spans.eq(1).css('width', Math.round(percent * 100) + '%');
                updateStatus();
            }

            function updateStatus() {
                var text = '', stats;

                if (state === 'ready') {
                    text = '选中' + fileCount + '张图片，共' +
                            WebUploader.formatSize(fileSize) + '。';
                } else if (state === 'confirm') {
                    stats = uploader.getStats();
                    if (stats.uploadFailNum) {
                        text = '已成功上传' + stats.successNum + '张照片至XX相册，' +
                            stats.uploadFailNum + '张照片上传失败，<a class="retry" href="#">重新上传</a>失败图片或<a class="ignore" href="#">忽略</a>'
                    }

                } else {
                    stats = uploader.getStats();
                    text = '共' + fileCount + '张（' +
                            WebUploader.formatSize(fileSize) +
                            '），已上传' + stats.successNum + '张';

                    if (stats.uploadFailNum) {
                        text += '，失败' + stats.uploadFailNum + '张';
                    }
                }

                info.html(text);
            }

            function setState(val) {
                var file, stats;

                if (val === state) {
                    return;
                }

                btn_upload.removeClass('state-' + state);
                btn_upload.addClass('state-' + val);
                state = val;

                switch (state) {
                    case 'pedding':
                        placeHolder.removeClass('element-invisible');
                        queue.parent().removeClass('filled');
                        queue.hide();
                        statusBar.addClass('element-invisible');
                        uploader.refresh();
                        break;

                    case 'ready':
                        placeHolder.addClass('element-invisible');
                        container.find('.filePicker2').removeClass('element-invisible');
                        queue.parent().addClass('filled');
                        queue.show();
                        statusBar.removeClass('element-invisible');
                        uploader.refresh();
                        break;

                    case 'uploading':
                        container.find('.filePicker2').addClass('element-invisible');
                        progress.show();
                        btn_upload.text('暂停上传');
                        break;

                    case 'paused':
                        progress.show();
                        btn_upload.text('继续上传');
                        break;

                    case 'confirm':
                        progress.hide();
                        btn_upload.text('开始上传').addClass('disabled');

                        stats = uploader.getStats();
                        if (stats.successNum && !stats.uploadFailNum) {
                            setState('finish');
                            return;
                        }
                        break;
                    case 'finish':
                        stats = uploader.getStats();
                        if (stats.successNum) {
                            alert('上传成功');
                        } else {
                            // 没有成功的图片，重设
                            state = 'done';
                            location.reload();
                        }
                        break;
                }

                updateStatus();
            }

            uploader.onUploadProgress = function (file, percentage) {
                var $li = $('#' + file.id),
                    $percent = $li.find('.progress span');

                $percent.css('width', percentage * 100 + '%');
                percentages[file.id][1] = percentage;
                updateTotalProgress();
            };

            uploader.onFileQueued = function (file) {
                fileCount++;
                fileSize += file.size;

                if (fileCount === 1) {
                    placeHolder.addClass('element-invisible');
                    statusBar.show();
                }

                addFile(file);
                setState('ready');
                updateTotalProgress();
            };

            uploader.onFileDequeued = function (file) {
                fileCount--;
                fileSize -= file.size;

                if (!fileCount) {
                    setState('pedding');
                }

                removeFile(file);
                updateTotalProgress();

            };

            uploader.on('all', function (type) {
                var stats;
                switch (type) {
                    case 'uploadFinished':
                        setState('confirm');
                        break;

                    case 'startUpload':
                        setState('uploading');
                        break;

                    case 'stopUpload':
                        setState('paused');
                        break;

                }
            });

            uploader.onError = function (code) {
                alert('Eroor: ' + code);
            };

            btn_upload.on('click', function () {
                if ($(this).hasClass('disabled')) {
                    return false;
                }

                if (state === 'ready') {
                    uploader.upload();
                } else if (state === 'paused') {
                    uploader.upload();
                } else if (state === 'uploading') {
                    uploader.stop();
                }
            });

            info.on('click', '.retry', function () {
                uploader.retry();
            });

            info.on('click', '.ignore', function () {
                alert('todo');
            });

            btn_upload.addClass('state-' + state);

            updateTotalProgress();
        },

        sliveruploader: function (opt, success) {

            var container = this;

            var GUID = WebUploader.Base.guid();//一个GUID

            var option = {
                swf: '/content/mgr/vendors/webuploader/Uploader.swf',
                server: '/admin/plugin/uploadsliverfile',

                //指定选择文件的按钮容器，可指定按钮名称innerHTML，是否可多文件上传multiple等
                pick: {
                    id: container.find('.js-picker'),
                    multiple: false
                },
                //指定接受哪些类型的文件，由于目前还有ext转mimeType表，所以这里需要分开指定
                accept: {
                    title: 'Images',
                    extensions: 'gif,jpg,jpeg,bmp,png',
                    mimeTypes: 'image/*'
                },
                resize: false,
                chunked: true,//开始分片上传
                chunkSize: 2048000,//每一片的大小
                formData: {
                    guid: GUID //自定义参数，待会儿解释
                }
            };

            option = $.extend(option, opt);

            var uploader = new WebUploader.Uploader(option);

            uploader.on('fileQueued', function (file) {
                container.find('.filename').html("文件名：" + file.name);
                container.find(".state").html('等待上传');
                container.find(".progress-bar").width('0%');
                container.find(".progress-bar").text('0%');
                container.find('js-uploader').removeAttr('disabled');
            });

            uploader.on('uploadProgress', function (file, percentage) {
                container.find(".progress-bar").width(percentage * 100 + '%');
                container.find(".progress-bar").text(parseInt(percentage * 100) + '%');
            });

            uploader.on('uploadSuccess', function (file) {
                $.post(option.mergeserver || '/admin/plugin/mergesliverfile', { guid: GUID, fileName: file.name }, function (data) {
                    container.find(".progress-bar").removeClass('progress-bar-striped').removeClass('active').removeClass('progress-bar-info').addClass('progress-bar-success');
                    container.find(".state").html("上传成功...");
                    success(data);
                });
            });

            uploader.on('uploadError', function () {
                container.find(".progress-bar").removeClass('progress-bar-striped').removeClass('active').removeClass('progress-bar-info').addClass('progress-bar-danger');
                container.find(".state").html("上传失败...");
            });

            container.find(".js-uploader").click(function () {
                uploader.upload();
                container.find(".js-uploader").text("上传");
                container.find('.js-uploader').attr('disabled', 'disabled');
                container.find(".progress-bar").addClass('progress-bar-striped').addClass('active');
                container.find(".state").html("上传中...");
            });

            container.find(".js-pause").click(function () {
                uploader.stop(true);
                container.find('.js-uploader').removeAttr('disabled');
                container.find(".js-uploader").text("继续上传");
                container.find(".state").html("暂停中...");
                container.find(".progress-bar").removeClass('progress-bar-striped').removeClass('active');
            });
        }
    });
}(jQuery));