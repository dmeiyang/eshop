(function ($) {
    $.extend($.fn, {
        myValidate: function (submitHandler) {
            this.validate({
                errorPlacement: function (error, element) {
                    error.insertAfter(element);
                },
                //focusInvalid: false, // do not focus the last invalid input
                //ignore: "",
                rules: getRules(this),

                //highlight: function (element) { // 验证未通过，处理函数
                //    $(element).addClass('validate-fail');
                //},

                //unhighlight: function (element) { // 验证通过，处理函数
                //    $(element).addClass('validate-success').removeClass('validate-fail').siblings('.validate-error').remove();
                //},

                success: function (label) {
                },

                submitHandler: function (form) {
                    submitHandler(form);
                }
            });
        }
    });

    function getRules(from) {
        //验证规则请参考：http://www.runoob.com/jquery/jquery-plugin-validate.html
        var ruleArray = [];
        from.find('input[required]').each(function (i) {
            var domObj = $(this);
            var array = [];

            array['required'] = true;

            if (domObj.attr('data-type')) {
                array[domObj.attr('data-type')] = true;
            }

            if (domObj.attr('rangelength')) {
                array['rangelength'] = $.parseJSON(domObj.attr('rangelength'));
            }

            if (domObj.attr('range')) {
                array['range'] = $.parseJSON(domObj.attr('range'));
            }

            if (domObj.attr('minlength')) {
                array['minlength'] = domObj.attr('minlength') - 0;
            }

            if (domObj.attr('maxlength')) {
                array['maxlength'] = domObj.attr('maxlength') - 0;
            }

            ruleArray[domObj.attr('name')] = arrayToObj(array);
        });

        return arrayToObj(ruleArray);
    }

    function arrayToObj(array) {
        var obj = {};
        for (var v in array) {
            obj[v] = array[v];
        }
        return obj;
    }
}(jQuery));