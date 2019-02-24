$(function () {
    createPages();

    var page = 0;
    $("body").css("width", $("div.container").width());
    $("body").css("height", $(window).height());
    $("div.section").css("max-width", $(window).width());
    $("body").mousewheel(function (event, delta) {
        var pageCount = $("div.sections div.section").length;
        if (page == 0 && delta > 0)
            page = 0;
        else if (page == pageCount - 1 && delta < 0)
            page = pageCount - 1;
        else if (delta < 0)
            page++;
        else
            page--;

        changePage(page);
        event.preventDefault();
    });
    $(window).keyup(function (e) {
        var pageCount = $("div.sections div.section").length;
        if (e.keyCode == 38 || e.keyCode == 39) {
            if (page != pageCount - 1)
                changePage(++page);
        }
        else if (e.keyCode == 37 || e.keyCode == 40) {
            if (page != 0)
                changePage(--page);
        }
    });
    $("div.pages ul li").click(function () {
        page = $("div.pages ul li").index(this);
        changePage(page);
    });
});
function openPage(url) {
    var urlTo;
    if (url.indexOf('?') > -1)
        urlTo = url.toString() + "&partial=yes";
    else
        urlTo = url.toString() + "?partial=yes";
    $.get(urlTo, function (data) {
        $("div.content").html(data);
        window.history.pushState(null, "", url);
        createPages();
    });
}
function changePage(page) {
    var left = $("div.sections div.section:eq(" + page + ")").position().left + 30;
    
    $("div.container").stop().animate({ 'left': -1 * left });

    $("div.pages ul li").removeClass("active");
    $("div.pages ul li:eq(" + page + ")").addClass("active");

}
function createPages() {
    var count = $("div.container div.section").length;
    var oldHtml = $("div.content").html();
    pageCount = count;
    var pagesHtml = "";
    for (var i = 0; i < count; i++)
        pagesHtml += "<li>" + i + "</li>";
    $("div.content").html(oldHtml + "<div class=\"pages\"><ul>" + pagesHtml + "</ul></div>");
    changePage(0);
}