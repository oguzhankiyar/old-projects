$(function () {
    $.sh.IncludeCSS("shCore.css");
    $.sh.IncludeCSS("shThemeDefault.css");
    $.sh.IncludeJS("shBrushXml.js");
    $.sh.IncludeJS("shBrushVb.js");
    $.sh.IncludeJS("shBrushSql.js");
    $.sh.IncludeJS("shBrushScala.js");
    $.sh.IncludeJS("shBrushSass.js");
    $.sh.IncludeJS("shBrushRuby.js");
    $.sh.IncludeJS("shBrushPython.js");
    $.sh.IncludeJS("shBrushPowerShell.js");
    $.sh.IncludeJS("shBrushPlain.js");
    $.sh.IncludeJS("shBrushPhp.js");
    $.sh.IncludeJS("shBrushPerl.js");
    $.sh.IncludeJS("shBrushJavaFX.js");
    $.sh.IncludeJS("shBrushJava.js");
    $.sh.IncludeJS("shBrushGroovy.js");
    $.sh.IncludeJS("shBrushJScript.js");
    $.sh.IncludeJS("shBrushErlang.js");
    $.sh.IncludeJS("shBrushDiff.js");
    $.sh.IncludeJS("shBrushDelphi.js");
    $.sh.IncludeJS("shBrushCss.js");
    $.sh.IncludeJS("shBrushCSharp.js");
    $.sh.IncludeJS("shBrushCpp.js");
    $.sh.IncludeJS("shBrushColdFusion.js");
    $.sh.IncludeJS("shBrushBash.js");
    $.sh.IncludeJS("shBrushAS3.js");
    SyntaxHighlighter.all();
});
$.sh = {
    IncludeCSS: function (source) {
        var css = document.createElement("link");

        css.rel = "stylesheet";
        css.href = "/syntaxhighlighter/styles/" + source;

        document.body.appendChild(css);
    },
    IncludeJS: function (source) {
        var js = document.createElement("script");

        js.type = "text/javascript";
        js.src = "/syntaxhighlighter/scripts/" + source;

        document.body.appendChild(js);
    }
}