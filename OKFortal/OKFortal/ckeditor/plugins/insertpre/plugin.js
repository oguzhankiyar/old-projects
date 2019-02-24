/*
 Copyright (c) 2003-2012, CKSource - Frederico Knabben. All rights reserved.
 For licensing, see LICENSE.html or http://ckeditor.com/license
 */

CKEDITOR.plugins.add( 'insertpre',
	{
		requires: 'dialog',
		lang : 'en,pl', // %REMOVE_LINE_CORE%
		icons: 'insertpre', // %REMOVE_LINE_CORE%
		onLoad : function()
		{
			if ( CKEDITOR.config.insertpre_class )
			{
				CKEDITOR.addCss(
					'pre.' + CKEDITOR.config.insertpre_class + ' {' +
						CKEDITOR.config.insertpre_style +
						'}'
				);
			}
		},
		init : function( editor )
		{
			editor.addCommand( 'insertpre', new CKEDITOR.dialogCommand( 'insertpre' ) );
			editor.ui.addButton && editor.ui.addButton( 'InsertPre',
				{
					label : editor.lang.insertpre.title,
					icon : this.path + 'icons/insertpre.png',
					command : 'insertpre',
					toolbar: 'insert,99'
				} );

			if ( editor.contextMenu )
			{
			    editor.addMenuGroup('code');
			    editor.addMenuItem('insertpre',
					{
					    label: editor.lang.insertpre.edit,
					    icon: this.path + 'icons/insertpre.png',
					    command: 'insertpre',
					    group: 'code'
					});
				editor.contextMenu.addListener( function( element )
				{
					if ( element )
						element = element.getAscendant( 'pre', true );
					if ( element && !element.isReadOnly() && element.hasClass( editor.config.insertpre_class ) )
						return { insertpre : CKEDITOR.TRISTATE_OFF };
					return null;
				});
			}

			CKEDITOR.dialog.add("insertpre", function (c) {
			    var a = function (f) {
			        f = f.replace(/<br>/g, "\n");
			        f = f.replace(/&amp;/g, "&");
			        f = f.replace(/&lt;/g, "<");
			        f = f.replace(/&gt;/g, ">");
			        f = f.replace(/&quot;/g, '"');
			        return f
			    };
			    var e = function (f) {
			        var f = new Object();
			        f.hideGutter = false;
			        f.hideControls = false;
			        f.collapse = false;
			        f.codeTitleChecked = true;
			        f.codeTitle = "";
			        f.htmlScript = false;
			        f.firstLineChecked = false;
			        f.firstLine = 0;
			        f.highlightChecked = false;
			        f.highlight = null;
			        f.lang = null; f.code = "";
			        return f
			    };
			    var b = function (i) {
			        var h = e();
			        return h
			    };
			    var bt = function (i) {
			        var h = e();
			        return h
			    };
			    var d = function (g) {
			        var f = "brush: " + g.lang + ";";
			        return f
			    };
			    var dt = function (g) {
			        var f = "";
			        return f
			    };
			    return {
			        title: editor.lang.insertpre.title,
			        minWidth: 500,
			        minHeight: 400,
			        onShow: function () {
			            var i = this.getParentEditor();
			            var h = i.getSelection();
			            var g = h.getStartElement();
			            var k = g && g.getAscendant("pre", true);
			            var j = "";
			            var f = null;
			            if (k) {
			                code = a(k.getHtml());
			                f = b(k.getAttribute("class"));
			                f.codeTitle = k.getAttribute("title");
			                if (f.codeTitle != "") { f.codeTitleChecked = true; }
			                f.code = code;
			            } else { f = e() }
			            this.setupContent(f)
			        },
			        onOk: function () {
			            var h = this.getParentEditor();
			            var g = h.getSelection();
			            var f = g.getStartElement();
			            var k = f && f.getAscendant("pre", true);
			            var i = e();
			            this.commitContent(i);
			            var j = d(i);
			            var jt = dt(i);
			            if (k) {
			                k.setAttribute("class", j);
			                k.setAttribute("title", jt);
			                k.setText(i.code)
			            } else {
			                var l = new CKEDITOR.dom.element("pre");
			                l.setAttribute("class", j);
			                l.setText(i.code);

			                //h.insertHtml("");			
			                h.insertElement(l);

			            }
			        },
			        contents: [
                        {
                            id: "source",
                            label: editor.lang.insertpre.sourceTab,
                            accessKey: "S",
                            elements: [
                            {
                                type: "vbox",
                                children: [
                                {
                                    id: "cmbLang",
                                    type: "select",
                                    labelLayout: "horizontal",
                                    label: "Select Language",
                                    "default": "cpp",
                                    widths: ["25%", "75%"],
                                    items: [["Action Script 3", "AS3"], ["Bash / Shell", "shell"], ["Cold Fusion", "cf"], ["C Sharp (C#)", "csharp"], ["C / C++", "cpp"], ["CSS", "css"], ["Delphi / Pascal", "delphi"], ["Diff", "diff"], ["Erlang", "erlang"], ["Groovy", "groovy"], ["HTML / XML", "xml"], ["JavaScript", "js"], ["Java", "java"], ["JavaFX", "javafx"], ["Perl", "perl"], ["PHP", "php"], ["Plain Text", "text"], ["Power Shell", "ps"], ["Python", "python"], ["Ruby", "ruby"], ["Scala", "scala"], ["SQL", "sql"], ["Visual Basic", "vb"]],
                                    setup: function (f) { if (f.lang) { this.setValue(f.lang) } },
                                    commit: function (f) { f.lang = this.getValue() }
                                }]
                            },
                            {
                                type: "textarea",
                                id: "hl_code",
                                rows: 22,
                                style: "width: 100%",
                                setup: function (f) { if (f.code) { this.setValue(f.code) } },
                                commit: function (f) { f.code = this.getValue() }
                            }
                            ]
                        }
			        ]
			    }
			});
		}
	} );

if (typeof(CKEDITOR.config.insertpre_style) == 'undefined')
	CKEDITOR.config.insertpre_style = 'background-color:#F8F8F8;border:1px solid #DDD;padding:10px;';
if (typeof(CKEDITOR.config.insertpre_class)  == 'undefined')
	CKEDITOR.config.insertpre_class = 'prettyprint';