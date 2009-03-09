///<reference path="jquery-1.3.2-vsdoc.js" />
///<reference path="ext/ext-jquery-adapter.js" />
///<reference path="ext/ext-core-debug.js" />

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

//默认的验证函数
$.fn.extend({
    isValid: function() {
        return true;
    }
});

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

$.fn.extend({
    //ajaxInit
    ajaxInit: function() {

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
                $(this).click(function() {
                    $.post(url, null, function(res) {

                        var success = typeof (res.Success) == "undefined" ? true : res.Success;

                        if (res.Msg) {
                            alert(res.Msg);
                        }
                    }, "json");

                    return false;
                });
            }
        });
    },
    //ajaxFormInit
    ajaxFormInit: function(cb) {

        var btnSub = $("input[type=submit],button[type=submit],a[class=submit],img[class=submit]", this);
        //alert(btnSub.length);
        btnSub.each(function(idx) {
            var me = $(this);
            var f = me.parents("form[class=ajax]:first");
            //alert(f.length);
            if (f.length != 0) {

                if (me.is("a,img")) {//1.31 
                    //me.is(a img) 1.2.6
                    // alert('is a img');
                    me.attr("href", "#");

                    $(this).click(function() {
                        f.submit();
                        return false;
                    });
                }

                me.removeAttr("onclick");

                f.attr("SubmitDisabledControls", true);
                /*
                f.ajaxError(function() {
                $(this).fadeIn("fast");
                });

                f.ajaxSend(function(evt, request, settings) {
                $(this).fadeOut('normal');
                });

                f.ajaxComplete(function(event, request, settings) {
                $(this).fadeIn("normal");
                });
                */
                f.ajaxError(function(event, request, settings, thrownError) {
                    var container = $("#" + f.attr("id") + "_Container:first");
                    var error = $("div[class=error]:first", container);
                    var ul = $("ul:first", error);
                    var d = new Date();
                    ul.append('<li><img class="icon" src="/Content/icons/error.png" />' + d.toLocaleTimeString() + "&nbsp;" + settings.url + '</li>');
                    error.show();
                });

                f.submit(function() {

                    if (!f.isValid()) { return false; }

                    if (f.attr("class") == "ajax") {

                        var serializedForm = f.serialize();

                        var act = f.attr("action") || window.location.toString();

                        var callback = (cb && typeof (cb) == "function") ? cb : function(res) {

                            var success = typeof (res.Success) == "undefined" ? true : res.Success;

                            var containerId = res.ContainerId;

                            if (containerId) {
                                var sucDiv = $("#" + containerId + " div[class=message] div[class=success]:first");
                                var errDiv = $("#" + containerId + " div[class=message] div[class=error]:first");
                            }
                            else {
                                var sucDiv = $("div[class=message] div[class=success]:first");
                                var errDiv = $("div[class=message] div[class=error]:first");
                            }

                            var uls = $("ul:first", sucDiv);
                            var ule = $("ul:first", errDiv);

                            uls.empty();
                            ule.empty();

                            if (res.Msg) {

                                uls.html("<li>" + res.Msg + "</li>");

                                sucDiv.show();
                            } else {
                                sucDiv.hide();
                            }

                            if (res.Errors) {

                                var errstr = "";
                                var i = 1;
                                for (var err in res.Errors) {
                                    errstr += "<li><span>" + i + ".</span>" + res.Errors[err] + "</li>";
                                    i++;
                                    //alert(res.Errors[err]);
                                }

                                ule.html(errstr);

                                errDiv.show();
                            } else {
                                errDiv.hide();
                            }
                        }

                        /*
                        Ext.Ajax.request({
                        url: act,
                        data: '',
                        params: { method: 'post' },
                        success: function(res) {
                        var o = Ext.util.JSON.decode(res.responseText);
                        if (o.Msg) {
                        }
                        },
                        failure: function(res) {
                        //  var respText = Ext.util.JSON.decode(res.responseText);
                        var container = $("#" + f.attr("id") + "_Container:first");
                        var error = $("div[class=error]:first", container);
                        var ul = $("ul:first", error);
                        ul.append("<li>出错页面:" + res.responseText + "</li>");
                        error.show();
                        }
                        });
                        */

                        $.post(act, serializedForm, callback, "json");

                        return false;
                    }
                    return true;
                });
            }
        });
    }
});

$(document).ready(function() {
    $(this).ajaxInit();
    setTimeout("$(document).ajaxInitDur();", 4000);
    if ($("form[class=ajax]").length != 0)
        $(this).ajaxFormInit();

});
