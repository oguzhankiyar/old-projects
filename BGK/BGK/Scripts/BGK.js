$(function () {
    $.fn.Modal = function (settings) {
        settings = $.extend({ type: "", content: "", submit: "", cancel: "" }, settings);
        return this.each(function () {
            $.BGK.LoadingShow();
            $("div.modal, div.modalcontent").remove();
            $("body").append("<div class=\"modal\"><div class=\"modalcontent\"></div></div>");
            $("div.modal").fadeIn(500);
            var type = settings.type;
            if (type == "normal") {
                $.get(settings.content, function (msg) {
                    $("div.modalcontent").show().html("<span class=\"close\" onclick=\"$.BGK.ModalClose();\">x</span>" + msg).animate({
                        top: ($(window).height() - $("div.modalcontent").height()) / 2 - 50
                    }, 500).css("left", ($(document).width() - $("div.modalcontent").width()) / 2);
                    $.BGK.LoadingHide();
                });
            }
            else if (type == "form") {
                var submit = "";
                var cancel = "";
                if (settings.submit != "")
                    submit = "<input type=\"submit\" onclick=\"$.BGK.ModalSubmit();\" value=\"" + settings.submit + "\" />";
                if (settings.cancel != "")
                    cancel = "<input type=\"button\" onclick=\"$.BGK.ModalClose();\" value=\"" + settings.cancel + "\" />";
                $.get(settings.content, function (msg) {
                    $("div.modalcontent").show().html("<span class=\"close\" onclick=\"$.BGK.ModalClose();\">x</span>" + msg + "<div class=\"buttons\"><span class=\"info\">&nbsp;</span>" + submit + cancel + "</div>").animate({
                        top: ($(window).height() - $("div.modalcontent").height()) / 2 - 100
                    }, 500).css("left", ($(document).width() - $("div.modalcontent").width()) / 2);
                    $("div.modalcontent textarea:eq(0)").focus();
                    $("div.modalcontent input[type='text']:eq(0)").focus();
                    $("div.modalcontent input").each(function () {
                        $(this).attr("autocomplete", "off");
                    });
                    $.BGK.LoadingHide();
                });
            }
            else {
                $("div.modalcontent").show().html("<div class=\"" + type + "\">" + settings.content + "</div>").animate({
                    top: ($(window).height() - $("div.modalcontent").height()) / 2 - 50
                }, 500).css("left", ($(document).width() - $("div.modalcontent").width()) / 2);
                $.BGK.LoadingHide();
            }
        });
    }
});
$(document).keyup(function (e) {
    if (e.keyCode == 27) {
        if ($("div.modalcontent").text().length > 0)
            $.BGK.ModalClose();
    }
    else if (e.keyCode == 13) {
        $.BGK.ModalSubmit();
    }
});
$.BGK = {
    LoadingShow: function () {
        $("#loading").fadeIn();
    },
    LoadingHide: function () {
        $("#loading").fadeOut();
    },
    Modal: function (url) {
        $(this).Modal({ type: "normal", content: url });
    },
    FormModal: function (url, submit, cancel) {
        $(this).Modal({ type: "form", content: url, submit: submit, cancel: cancel });
    },
    SuccessModal: function (content, callback, second) {
        $(this).Modal({ type: "success", content: content });
        if (callback != null)
            setTimeout(callback, second);
        $.BGK.ModalClose(2000);
    },
    ErrorModal: function (content, callback, second) {
        $(this).Modal({ type: "error", content: content });
        if (callback != null)
            setTimeout(callback, second);
        $.BGK.ModalClose(2000);
    },
    ModalSubmit: function () {
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
            $.BGK.LoadingShow();
            $("div.modal").fadeOut(2000);
            $("div.modalcontent").animate({
                top: 0
            }, 500, function () {
                $("div.modal, div.modalcontent").remove();
                $.BGK.LoadingHide();
            });
        }, time);
    }
}
$(function () {
    $(window).scroll(function () {
        if ($(document).scrollTop() > $("div.header").position().top + 300) { $("#top").stop().fadeIn(500); }
        else { $("#top").fadeOut(500); }
    });
    $("#top").click(function () {
        $("html,body").stop().animate({ scrollTop: "0" }, 1000);
    });
    $("a").click(function () {
        $.BGK.LoadingShow();
        setTimeout(function () { $.BGK.LoadingHide(); }, 2000);
    });
});