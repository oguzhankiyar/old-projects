$(function () {
    $.fn.Modal = function (settings) {
        settings = $.extend({ type: "", content: "", submit: "", cancel: "" }, settings);
        return this.each(function () {
            $.OK.LoadingShow();
            $("div.modal, div.modalcontent").remove();
            $("body").append("<div class=\"modal\"><div class=\"modalcontent\"></div></div>");
            $("div.modal").fadeIn(500);
            var type = settings.type;
            if (type == "normal") {
                $.post(settings.content, function (msg) {
                    $("div.modalcontent").show().html("<span class=\"close\" onclick=\"$.OK.ModalClose();\">x</span>" + msg).animate({
                        top: ($(window).height() - $("div.modalcontent").height()) / 2 - 50
                    }, 500).css("left", ($(document).width() - $("div.modalcontent").width()) / 2);
                    $.OK.LoadingHide();
                });
            }
            else if (type == "form") {
                var submit = "";
                var cancel = "";
                if (settings.submit != "") {
                    submit = "<input type=\"submit\" onclick=\"$.OK.ModalSubmit();\" value=\"" + settings.submit + "\" />";
                }
                if (settings.cancel != "") {
                    cancel = "<input type=\"button\" onclick=\"$.OK.ModalClose();\" value=\"" + settings.cancel + "\" />";
                }
                $.post(settings.content, function (msg) {
                    $("div.modalcontent").show().html("<span class=\"close\" onclick=\"$.OK.ModalClose();\">x</span>" + msg + "<span class=\"info\">&nbsp;</span><span class=\"buttons\">" + submit + cancel + "</span>").animate({
                        top: ($(window).height() - $("div.modalcontent").height()) / 2 - 100
                    }, 500).css("left", ($(document).width() - $("div.modalcontent").width()) / 2);
                    $("div.modalcontent textarea:eq(0)").focus();
                    $("div.modalcontent input[type='text']:eq(0)").focus();
                    $("div.modalcontent input").each(function () {
                        $(this).attr("autocomplete", "off");
                    });
                    $.OK.LoadingHide();
                });
            }
            else {
                $("div.modalcontent").show().html("<div class=\"" + type + "\">" + settings.content + "</div>").animate({
                    top: ($(window).height() - $("div.modalcontent").height()) / 2 - 50
                }, 500).css("left", ($(document).width() - $("div.modalcontent").width()) / 2);
                $.OK.LoadingHide();
            }
        });
    }
});
$(document).keyup(function (e) {
    if (e.keyCode == 27) {
        if ($("div.modalcontent").text().length > 0) 
            $.OK.ModalClose();
    }
});
$.OK = {
    LoadingShow: function () {
        $("#loading").fadeIn();
    },
    LoadingHide: function () {
        $("#loading").fadeOut();
    },
    OpenNewPage: function (url) {
        window.location.href = url;
    },
    setCookie: function (name, value, days) {
        if (days) {
            var date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            var expires = "; expires=" + date.toGMTString();
        }
        else
            var expires = "";
        document.cookie = name + "=" + value + expires + "; path=/";
    },
    getCookie: function (name) {
        var nameEQ = name + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') c = c.substring(1, c.length);
            if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
        }
        return null;
    },
    deleteCookie: function (name) {
        $.OK.setCookie(name, "", -1);
    },
    Modal: function (url) {
        $(this).Modal({ type: "normal", content: url });
    },
    FormModal: function (url, submit, cancel) {
        $(this).Modal({ type: "form", content: url, submit: submit, cancel: cancel });
    },
    SuccessModal: function (content) {
        $(this).Modal({ type: "success", content: content });
        $.OK.ModalClose(2000);
    },
    ErrorModal: function (content) {
        $(this).Modal({ type: "error", content: content });
        $.OK.ModalClose(2000);
    },
    ModalSubmit: function() {
        if (!$("div.modalcontent form").valid()) {
            $("div.modalcontent form").submit();
        }
        else {
            for (instance in CKEDITOR.instances)
                CKEDITOR.instances[instance].updateElement();
            $.post($("div.modalcontent form").attr("action"), $("div.modalcontent form").serialize(), function (msg) {
                $("div.modalcontent .info").html(msg);
            });
        }
    },
    ModalClose: function (time) {
        setTimeout(function () {
            $.OK.LoadingShow();
            $("div.modal").fadeOut(2000);
            $("div.modalcontent").animate({
                top: 0
            }, 500, function () {
                $("div.modal, div.modalcontent").remove();
                $.OK.LoadingHide();
            });
        }, time);
    }
}


$(function () {
    $(window).scroll(function () {
        if ($(document).scrollTop() > $("div.top").position().top + 300) { $("#top").fadeIn(500); }
        else { $("#top").fadeOut(500); }
    });
    $("#top").click(function () {
        $("html,body").stop().animate({ scrollTop: "0" }, 1000);
    });
    $("div.menu ul li, a").click(function () {
        $.OK.LoadingShow();
        setTimeout(function () { $.OK.LoadingHide(); }, 2000);
    });
});

(function () {
    var cx = '003372465442428061994:rfegoo9cdok';
    var gcse = document.createElement('script'); gcse.type = 'text/javascript'; gcse.async = true;
    gcse.src = (document.location.protocol == 'https:' ? 'https:' : 'http:') +
        '//www.google.com.tr/cse/cse.js?cx=' + cx;
    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(gcse, s);
})();