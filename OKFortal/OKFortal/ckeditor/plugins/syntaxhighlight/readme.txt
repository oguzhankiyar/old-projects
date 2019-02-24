﻿
/*********************************************************************************************************/
/**
 * syntaxhighlight plugin for CKEditor 3.x for SyntaxHighlighter 3.0.83
 * Released: On 2011-06-24
 * Download: http://www.harrisonhills.org/techresources
 * Original plugin written by Lajox found at http://code.google.com/p/lajox
 */
/*********************************************************************************************************/

/**************************************************************************************************************
syntaxhighlight plugin for CKEditor 3.x

 --Code Syntax Highlight Plugin

Plugin Description： CKEditor 3.x SyntaxHighlight Plugin 1.3

Related :
SyntaxHighlighter 3.0.83 from http://alexgorbatchev.com/wiki/SyntaxHighlighter

***************************************************************************************************************/


/**************Help Begin***************/

1. Upload syntaxhighlight folder to  ckeditor/plugins/

2. Configured in the ckeditor/config.js :
    Add to config.toolbar a value 'syntaxhighlight'
e.g. 

config.toolbar = 
[
    [ 'Source', '-', 'Bold', 'Italic', 'syntaxhighlight' ]
];


3. Again Configured in the ckeditor/config.js ,
   Expand the extra plugin 'syntaxhighlight' such as:

config.extraPlugins='myplugin1,myplugin2,syntaxhighlight';

4. Modify the default language in syntaxhighlight/plugin.js
	Just the line:
		lang : ['en'],

5. In your page files between <title> and </title> ,add these content like these:

<script type="text/javascript" src="/editor/custom/ckeditor/plugins/syntaxhighlight/scripts/shCore.js"></script>
<script type="text/javascript" src="/editor/custom/ckeditor/plugins/syntaxhighlight/scripts/shBrushA3.js"></script>
<script type="text/javascript" src="/editor/custom/ckeditor/plugins/syntaxhighlight/scripts/shBrushBash.js"></script>
<script type="text/javascript" src="/editor/custom/ckeditor/plugins/syntaxhighlight/scripts/shBrushColdFusion.js"></script>
<script type="text/javascript" src="/editor/custom/ckeditor/plugins/syntaxhighlight/scripts/shBrushCpp.js"></script>
<script type="text/javascript" src="/editor/custom/ckeditor/plugins/syntaxhighlight/scripts/shBrushCSharp.js"></script>
<script type="text/javascript" src="/editor/custom/ckeditor/plugins/syntaxhighlight/scripts/shBrushCss.js"></script>
<script type="text/javascript" src="/editor/custom/ckeditor/plugins/syntaxhighlight/scripts/shBrushDelphi.js"></script>
<script type="text/javascript" src="/editor/custom/ckeditor/plugins/syntaxhighlight/scripts/shBrushDiff.js"></script>
<script type="text/javascript" src="/editor/custom/ckeditor/plugins/syntaxhighlight/scripts/shBrushErlang.js"></script>
<script type="text/javascript" src="/editor/custom/ckeditor/plugins/syntaxhighlight/scripts/shBrushGroovy.js"></script>
<script type="text/javascript" src="/editor/custom/ckeditor/plugins/syntaxhighlight/scripts/shBrushJava.js"></script>
<script type="text/javascript" src="/editor/custom/ckeditor/plugins/syntaxhighlight/scripts/shBrushJavaFX.js"></script>
<script type="text/javascript" src="/editor/custom/ckeditor/plugins/syntaxhighlight/scripts/shBrushJScript.js"></script>
<script type="text/javascript" src="/editor/custom/ckeditor/plugins/syntaxhighlight/scripts/shBrushPerl.js"></script>
<script type="text/javascript" src="/editor/custom/ckeditor/plugins/syntaxhighlight/scripts/shBrushPhp.js"></script>
<script type="text/javascript" src="/editor/custom/ckeditor/plugins/syntaxhighlight/scripts/shBrushPlain.js"></script>
<script type="text/javascript" src="/editor/custom/ckeditor/plugins/syntaxhighlight/scripts/shBrushPowerShell.js"></script>
<script type="text/javascript" src="/editor/custom/ckeditor/plugins/syntaxhighlight/scripts/shBrushPython.js"></script>
<script type="text/javascript" src="/editor/custom/ckeditor/plugins/syntaxhighlight/scripts/shBrushRuby.js"></script>
<script type="text/javascript" src="/editor/custom/ckeditor/plugins/syntaxhighlight/scripts/shBrushScala.js"></script>
<script type="text/javascript" src="/editor/custom/ckeditor/plugins/syntaxhighlight/scripts/shBrushSql.js"></script>
<script type="text/javascript" src="/editor/custom/ckeditor/plugins/syntaxhighlight/scripts/shBrushVb.js"></script>
<script type="text/javascript" src="/editor/custom/ckeditor/plugins/syntaxhighlight/scripts/shBrushXml.js"></script>
 <link type="text/css" rel="stylesheet" href="/editor/custom/ckeditor/plugins/syntaxhighlight/styles/shCore.css"/>
 <link type="text/css" rel="stylesheet" href="/editor/custom/ckeditor/plugins/syntaxhighlight/styles/shThemeDefault.css"/>
 <script type="text/javascript">
  SyntaxHighlighter.all();
 </script>


OK, That's it!

Note:
1. CKEditor editor highlights the code can not be a complete show results, articles to be published later, that is generated after the page display properly highlighted results

2. In ckeditor/plugins/syntaxhighlight/styles/ There are a few css styles:
shThemeDefault.css
shThemeDjango.css
shThemeEclipse.css
shThemeEmacs.css
shThemeFadeToGrey.css
shThemeMDUltra.css
shThemeMidnight.css
shThemeRDark.css
 You Can change the style such as:
<link type="text/css" rel="stylesheet" href="/editor/custom/ckeditor/plugins/syntaxhighlight/styles/shThemeDjango.css"/>

3. Syntaxhighlight parameters on the configuration please refer to the official explanation.

/**************Help End***************/
