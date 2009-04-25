///<reference path="jquery-1.3.2-vsdoc.js" />

String.prototype.format = function() {
    var args = arguments;
    return this.replace(/\{(\d+)\}/g, function(m, i) {
        return args[i];
    });
}
//V2 static
String.format = function() {
    if (arguments.length == 0)
        return null;
    var str = arguments[0];
    for (var i = 1; i < arguments.length; i++) {
        var re = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
        str = str.replace(re, arguments[i]);
    }
    return str;
}

//$.cookie('the_cookie', 'the_value', { expires: 7, path: '/', domain: 'jquery.com', secure: true });
jQuery.cookie = function(name, value, options) { if (typeof value != 'undefined') { options = options || {}; if (value === null) { value = ''; options.expires = -1; } var expires = ''; if (options.expires && (typeof options.expires == 'number' || options.expires.toUTCString)) { var date; if (typeof options.expires == 'number') { date = new Date(); date.setTime(date.getTime() + (options.expires * 24 * 60 * 60 * 1000)); } else { date = options.expires; } expires = '; expires=' + date.toUTCString(); } var path = options.path ? '; path=' + (options.path) : ''; var domain = options.domain ? '; domain=' + (options.domain) : ''; var secure = options.secure ? '; secure' : ''; document.cookie = [name, '=', encodeURIComponent(value), expires, path, domain, secure].join(''); } else { var cookieValue = null; if (document.cookie && document.cookie != '') { var cookies = document.cookie.split(';'); for (var i = 0; i < cookies.length; i++) { var cookie = jQuery.trim(cookies[i]); if (cookie.substring(0, name.length + 1) == (name + '=')) { cookieValue = decodeURIComponent(cookie.substring(name.length + 1)); break; } } } return cookieValue; } };

$.extend({
    loadScriptDom: function(src) {
        if (!src) return;
        var head = document.getElementsByTagName("head")[0];
        script = document.createElement('script');
        script.type = 'text/javascript';
        script.src = src;
        head.appendChild(script);
    },
    loadStyleDom: function(href) {
        if (!href) return;
        var head = document.getElementsByTagName("head")[0];
        css = document.createElement('link');
        css.type = 'text/css';
        css.href = href;
        css.rel = "stylesheet"
        head.appendChild(css);
    }
});

$.fn.pressEnter = function(fun) {
    // http://www.cssrain.cn/article.asp?id=1167
    this.keypress(function(e) {
        if (e.which == "13") {
            fun();
        }
    });
};

$.fn.clearLink = function(href) {
    var a = $("a[href]", this);

    a.each(function(idx) {
        var lnk = $(this);
        var att = lnk.attr("href");
        if (href == att)
            lnk.attr("href", "#");
    });
};

/**
* Clears the form data.  Takes the following actions on the form's input fields:
*  - input text fields will have their 'value' property set to the empty string
*  - select elements will have their 'selectedIndex' property set to -1
*  - checkbox and radio inputs will have their 'checked' property set to false
*  - inputs of type submit, button, reset, and hidden will *not* be effected
*  - button elements will *not* be effected
*/
$.fn.clearForm = function() {
    return this.each(function() {
        $('input,select,textarea', this).clearFields();
    });
};

/**
* Resets the form data.  Causes all form elements to be reset to their original value.
*/
$.fn.resetForm = function() {
    return this.each(function() {
        // guard against an input with the name of 'reset'
        // note that IE reports the reset function as an 'object'
        if (typeof this.reset == 'function' || (typeof this.reset == 'object' && !this.reset.nodeType))
            this.reset();
    });
};

$.fn.reload = function() {
    //重新载入
    var url = this.attr("url");
    if (url)
        this.load(url);
};

$.fn.extend({
    ajaxLoad: function() {
        var u = this.attr("url");
        if (u && u != "") {
            $(this).load(u, null, function() {
                eval("var t= $(this);t.ajaxInit();t.show();");
            });
        }
    }
});

$.fn.extend({
    ajaxInitDur: function() {

        var ajax = $("h1:[url][class=ajaxDuring],div:[url][class=ajaxDuring],p:[url][class-ajaxDuring]", this);

        ajax.each(function(i) {
            $(this).ajaxLoad();
        });
    }
});

$.extend({
    selectFromContainer: function(selector, containerId) {
        if (containerId)
            var container = $("#" + containerId);
        if (container && container.length != 0) {
            return $(selector, container);
        } else {
            return $(selector);
        }
    }
});

//预定义的检查模式 
var regArray = new Array(
new Array("int+0", "^\\d+$", "", "需要输入一个非负整数，请重新检查"), //非负整数（正整数 + 0） 
new Array("int+", "^[0-9]*[1-9][0-9]*$", "^\\d+$", "需要输入一个正整数，请重新检查"), //正整数 
new Array("int-0", "^((-\\d+)|(0+))$", "^(-|(-\\d+)|(0+))$", "需要输入一个非正整数，请重新检查"), //非正整数（负整数 + 0） 
new Array("int-", "^-[0-9]*[1-9][0-9]*$", "^(-|(-\\d+)|(0+))$", "需要输入一个负整数，请重新检查"), //负整数 
new Array("int", "^-?\\d+$", "^-|(-?\\d+)$", "需要输入一个整数，请重新检查"), //整数 
new Array("double+0", "^\\d+(\\.\\d+)?$", "^((\\d+\\.)|(\\d+(\\.\\d+)?))$", "需要输入一个非负浮点数，请重新检查"), //非负浮点数（正浮点数 + 0） 
new Array("double+", "^(([0-9]+\\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\\.[0-9]+)|([0-9]*[1-9][0-9]*))$", "^((\\d+\\.)|(\\d+(\\.\\d+)?))$", "需要输入一个正浮点数，请重新检查"), //正浮点数 
new Array("double-0", "^((-\\d+(\\.\\d+)?)|(0+(\\.0+)?))$", "^(-|(-\\d+\\.)|(0+\\.)|(-\\d+(\\.\\d+)?)|(0+(\\.0+)?))$", "需要输入一个非正浮点数，请重新检查"), //非正浮点数（负浮点数 + 0） 
new Array("double-", "^(-(([0-9]+\\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\\.[0-9]+)|([0-9]*[1-9][0-9]*)))$", "^(-|(-\\d+\\.?)|(-\\d+\\.\\d+))$", "需要输入一个负浮点数，请重新检查"), //负浮点数 
new Array("double", "^(-?\\d+)(\\.\\d+)?$", "^(-|((-?\\d+)(\\.\\d+)?)|(-?\\d+)\\.)$", "需要输入一个浮点数，请重新检查"), //浮点数
new Array("letter", "^[A-Za-z]+$", "", "您只能输入英文字母，请重新检查"), //由26个英文字母组成的字符串 
new Array("upperchar", "^[A-Z]+$", "", "您只能输入英文大写字母，请重新检查"), //由26个英文字母的大写组成的字符串 
new Array("lowerchar", "^[a-z]+$", "", "您只能输入英文小写字母，请重新检查"), //由26个英文字母的小写组成的字符串 
new Array("digitchar", "^[A-Za-z0-9]+$", "", "您只能输入数字和英文字母，请重新检查"), //由数字和26个英文字母组成的字符串
new Array("digitchar_", "^\\w+$", "", "您只能输入数字、英文字母和下划线，请重新检查"), //由数字、26个英文字母或者下划线组成的字符串
new Array("email", "^[\\w-]+(\\.[\\w-]+)*@[\\w-]+(\\.[\\w-]+)+$", "", "需要输入正确的email地址，请重新检查"), //email地址
new Array("url", "^[a-zA-z]+://(\\w+(-\\w+)*)(\\.(\\w+(-\\w+)*))*(\\?\\S*)?$", "^([a-zA-z]+:?)|([a-zA-z]+:/{1,2})|([a-zA-z]+://(\\w+(-\\w+)*))|([a-zA-z]+://(\\w+(-\\w+)*)(\\.(\\w+(-\\w+)*))*(\\?\\S*)?)$", "需要输入正确的url地址，请重新检查"), //url
new Array("chinese", "^[\\u4E00-\\u9FA5\\uF900-\\uFA2D]+$", "", "请输入中文"),
new Array("username", "^\\w+$", "", "用户名可由数字、26个英文字母或者下划线组成"),
new Array("idcard", "^[1-9]([0-9]{14}|[0-9]{17})$", "", "需要输入正确的身份证号码"),
new Array("zipcode", "^\\d{6}$", "", "需要输入正确的邮编"),
new Array("color", "^[a-fA-F0-9]{6}$", "", "需要输入正确的颜色"),
new Array("picture", "(.*)\\.(jpg|bmp|gif|ico|pcx|jpeg|tif|png|raw|tga)$", "", "图片必须是jpg|bmp|gif|ico|pcx|jpeg|tif|png|raw|tga"),
new Array("tel", "^(([0\\+]\\d{2,3}-)?(0\\d{2,3})-)?(\\d{7,8})(-(\\d{3,}))?$", "", "需要输入正确的电话号码 如:086-0512-52810434")
);

function doInputEvent() {

    //得到触发事件的类型
    var type = window.event.type;
    //得到触发元素的值。
    var el = window.event.srcElement;
    var value = window.event.srcElement.value;
    if (type == "keypress") { //如果是键盘按下事件，得到键盘按下后的值 
        var keyCode = window.event.keyCode;
        if (typeof (el.upper) != "undefined") { //如果定义了转换大写 
            if (keyCode >= 97 && keyCode <= 122)
                keyCode = window.event.keyCode = keyCode - 32;
        }
        else if (typeof (el.lower) != "undefined") { //如果定义了转换小写 
            if (keyCode >= 65 && keyCode <= 90)
                keyCode = window.event.keyCode = keyCode + 32;
        }
        value += String.fromCharCode(keyCode);
    }
    else if (type == "paste") {
        value += window.clipboardData.getData("Text");
    }
    //如果触发元素的值为空，则表示用户没有输入，不接受检查。 
    if (value == "") return;
    //如果触发元素没有设置reg属性，则返回不进行任何检查。
    if (typeof (el.reg) == "undefined") return;
    //如果触发元素没有定义check属性，则在按键和粘贴事件中不做检查
    if ((type == "keypress" || type == "paste") && typeof (el.check) == "undefined") return;
    //如果没有通过检查模式，出现的错误信息 
    var msg = "";
    //得到检查模式
    var reg = el.reg;
    //正则表达式对象 
    var regExp = null;
    //从预定义的检查模式中查找正则表达式对象 
    for (var i = 0; i < regArray.length; i++) {
        if (regArray[i][0] == reg) {
            if ((type == "keypress" || type == "paste") && regArray[i][2] != "")
                regExp = new RegExp(regArray[i][2]); //查找到预定义的检查模式 
            else
                regExp = new RegExp(regArray[i][1]); //查找到预定义的检查模式 
            msg = regArray[i][3]; //定义预定义的报错信息 
            break; //查找成功，退出循环 
        }
    }
    if (regExp == null) { //如果没有查找到预定义的检查模式，说明reg本身就为正则表达式对象。
        if ((type == "keypress" || type == "paste") && typeof (el.regcheck) != "undefined")
            regExp = new RegExp(el.regcheck); //按照用户自定义的正则表达式生成正则表达式对象。 
        else
            regExp = new RegExp(reg); //按照用户自定义的正则表达式生成正则表达式对象。 
        msg = "输入错误，请重新检查"; //错误信息 
    }
    //检查触发元素的值符合检查模式，直接返回。 
    if (regExp.test(value)) return;
    if (type == "blur") { //如果是失去焦点并且检查不通过，则需要出现错误警告框。 
        //判断用户是否自己定义了错误信息
        if (typeof (el.msg) != "undefined")
            msg = el.msg;
        //显示错误信息 
        alert(msg);
        //将焦点重新聚回触发元素
        el.focus();
        el.select();
    }
    else { //如果是键盘按下或者粘贴事件并且检查不通过，则取消默认动作。 
        //取消此次键盘按下或者粘贴操作 
        window.event.returnValue = false;
    }
}

$.fn.extend({
    validateRequired: function() {
        return ({ validate: $.trim(ipt.val()).length != 0, msg: "是必填项" });
    },
    validateInput: function() {

        var ipt = $(this);
        var reg = $.trim(ipt.attr("reg"));

        var isRequired = ipt.attr("class") == "input-validation-error";
        if (isRequired) {

            var r = $.trim(ipt.val()).length != 0;
            if (!r) {
                //alert(isRequired);
                return ({ validate: false, msg: "是必填项" });
            }
        }

        if (!reg || reg.length == 0) {
            return { validate: true };
        }

        var regExp = null;
        var msg = null;
        //从预定义的检查模式中查找正则表达式对象 
        for (var i = 0; i < regArray.length; i++) {
            if (regArray[i][0] == reg) {
                regExp = new RegExp(regArray[i][1]); //查找到预定义的检查模式 
                msg = regArray[i][3]; //定义预定义的报错信息 
                break; //查找成功，退出循环 
            }
        }
        if (regExp) {
            return ({ validate: regExp.test(ipt.val()), msg: msg });
        }
        return { validate: true };
    }
});

//默认的验证函数
$.fn.extend({
    isValid: function() {
        var fm = this;
        //<input type="hidden" name="formContainerId" value="fm_Container" />
        var containerId = $("input[type=hidden][name=formContainerId]:first", this).val();
        var errDiv = $.getErrorContainer(containerId);
        errDiv.hide();
        var ule = $("ul:first", errDiv);
        ule.empty();
        var errors = "";
        var isValid = true;

        var inputs = $("input[type=text],input[type=password]", fm);

        inputs.each(function() {
            var ipt = $(this);
            var v = ipt.validateInput();
            if (!v.validate) {
                var lbl = $("label[for=" + ipt.attr("name") + "]", fm).text().replace(":", "");
                errors += "<li>" + $.getErrorIcon + lbl + v.msg + "</li>";
            }
            isValid = isValid && v.validate;
        });

        if (!isValid) {
            errDiv.html(errors);
            errDiv.show();
        }

        return isValid;
    }
});

$.fn.extend({
    bindClientFormValidate: function() {
        var inputs = $("input[type=text][reg]", this);

        inputs.each(function() {
            var ipt = $(this);
            var reg = ipt.attr("reg");
            if (typeof (reg) == "string" && reg != "") {
                ipt.keypress(doInputEvent);
                //ipt.blur(doInputEvent);
                //ipt.bind('paste', doInputEvent);
            }
        });
    }
});

$.extend({
    getMsgIcon: "<img src='/Content/icons/tag_green.png' alt='' class='icon' />",
    getErrorIcon: "<img src='/Content/icons/server_error.png' alt='' class='icon' />",

    getMsgContainer: function(containerId) {
        return $.selectFromContainer("div[class=message] div[class=success]:first", containerId);
    },
    getErrorContainer: function(containerId) {
        return $.selectFromContainer("div[class=message] div[class=error]:first", containerId);
    },
    ajaxActionCallback: function(res) {
    },
    ajaxFormCallback: function(res) {
        var success = typeof (res.Success) == "undefined" ? true : res.Success;
        var containerId = res.ContainerId;
        var sucDiv = $.getMsgContainer(containerId);
        var errDiv = $.getErrorContainer(containerId);
        var uls = $("ul:first", sucDiv);
        var ule = $("ul:first", errDiv);
        uls.empty();
        ule.empty();
        if (res.Msg) {
            uls.html("<li>" + $.getMsgIcon + res.Msg + "&nbsp;&nbsp;(" + new Date().toLocaleTimeString() + ")</li>");
            var options = {};
            //callback function to bring a hidden box back
            var cbBlind = function() {
                //                                    setTimeout(function() {
                //                                        sucDiv.removeAttr('style').hide().fadeOut();
                //                                    }, 1000);
            };
            sucDiv.show("blind", options, 500, cbBlind);
        } else {
            sucDiv.hide();
        }
        if (res.Errors) {
            errDiv.hide();
            var errstr = "";
            var i = 1;
            for (var err in res.Errors) {
                errstr += "<li>" + $.getErrorIcon + "<span>" + i + ".</span>" + res.Errors[err] + "</li>";
                i++;
            }
            ule.html(errstr);
            errDiv.show();
        } else {
            errDiv.hide();
        }
    }
});

$.fn.extend({
    ajaxLinkInit: function() {

        var lnkAjax = $("a[class=ajax][href][target]", this);

        lnkAjax.each(function(idx) {
            var target = $("#" + this.target + ":first");
            if (target.size() != 0) {
                var href = this.href;
                $(this).click(function() {
                    if (href && href != "") {
                        target.load(href, null, function() {
                            eval("$(this).ajaxInit()");
                        });
                    }
                    return false;
                });
            } else {
                $(this).click(function() {
                    return false;
                });
            }
        });

        var linkSubmit = $("a[class=submitlink][href]", this);

        linkSubmit.each(function(idx) {

            var url = this.href;
            if (url && url != "") {
                var submitlink = $(this);

                submitlink.click(function() {

                    var lnk = event.srcElement;
                    $.post(url, null, function(res) {
                        var success = typeof (res.Success) == "undefined" ? true : res.Success;
                        if (res.Msg) {
                            alert(res.Msg);
                        }

                        var ajaxContainer = $(lnk).parents("div[url]:first");
                        ajaxContainer.ajaxLoad();

                    }, "json");
                    return false;
                });
                submitlink.attr("href", "#");
            }
        });
    }
});

$.fn.extend({
    //ajaxInit
    ajaxInit: function() {

        var isReload = this.attr("loaded") == "1";
        this.attr("loaded", "1");

        var a = $("a", this);
        //去掉超链接虚线框
        a.bind("focus", function() {
            if (this.blur) {
                this.blur();
            }
        });

        //去掉超链接下划线
        a.css("text-decoration", "none");

        //对class包含ajax并且有正确的url标签的对象自动执行ajax内容请求
        //var ajax = $("div[url]", this);

        var ajax = $("h1:not([class=ajaxDuring])[url],div:not([class=ajaxDuring])[url],p:not([class=ajaxDuring])[url]", this);

        ajax.each(function(i) {
            $(this).ajaxLoad();
        });

        this.ajaxLinkInit();
    },
    bindClientValidateForm: function() {
        var f = $(this);
        //        var me = $("input[type=submit],button[type=submit]:first", f);
        //        me.click(function() {
        //            return f.isValid();
        //        });
        f.submit(function() {
            if (!f.isValid()) {
                return false;
            }
            return true;
        });
    },
    bindAjaxPostForm: function(cb) {
        var f = $(this);
        var me = $("input[type=submit],button[type=submit],a[class=submit],img[class=submit]:first", f);
        if (me.is("a,img")) {
            me.attr("href", "#");
            $(this).click(function() {
                f.submit();
                return false;
            });
        }

        me.removeAttr("onclick");

        var ajaxError = function(event, request, settings, thrownError) {
            var container = $("#" + f.attr("id") + "_Container:first");
            var error = $("div[class=error]:first", container);
            var ul = $("ul:first", error);
            var d = new Date();
            ul.append('<li>' + $.getErrorIcon + d.toLocaleTimeString() + "&nbsp;" + settings.url + '</li>');
            error.show();
        };

        f.ajaxError(ajaxError);

        f.submit(function() {
            if (!f.isValid()) {
                return false;
            }
            else {
                var serializedForm = f.serialize();
                var act = f.attr("action") || window.location.toString();
                var callback = (cb && typeof (cb) == "function") ? cb : $.ajaxFormCallback;
                $.post(act, serializedForm, callback, "json");
                return false;
            }
            return true;
        });
    },
    //ajaxFormInit
    ajaxFormInit: function(cb) {

        var forms = $("form");
        forms.each(function() {
            var f = $(this);
            f.attr("SubmitDisabledControls", true);
            var isAjax = f.attr("class") == "ajax";
            f.bindClientValidateForm(f);
            if (isAjax)
                f.bindAjaxPostForm(f);
        });
    }
});

$(document).ready(function() {
    var doc = $(this);
    doc.ajaxInit();
    setTimeout("$(document).ajaxInitDur();", 4000);
    doc.ajaxFormInit();
});
