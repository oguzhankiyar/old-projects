/*********************************************************************************************************/
/**
 * syntaxhighlight plugin for CKEditor 3.x for SyntaxHighlighter 3.0.83
 * Released: On 2011-06-24
 * Download: http://www.harrisonhills.org/techresources
 * Original plugin written by Lajox found at http://code.google.com/p/lajox
 */
/*********************************************************************************************************/

CKEDITOR.plugins.add('syntaxhighlight',
      {
          requires: 'dialog',
          lang: 'en,pl', // %REMOVE_LINE_CORE%
          icons: 'syntaxhighlight', // %REMOVE_LINE_CORE%
          onLoad: function () {
              if (CKEDITOR.config.syntaxhighlight_class) {
                  CKEDITOR.addCss(
                      'pre.' + CKEDITOR.config.syntaxhighlight_class + ' {' +
                          CKEDITOR.config.syntaxhighlight_style +
                          '}'
                  );
              }
          },
          init: function (editor) {
              editor.addCommand('syntaxhighlight', new CKEDITOR.dialogCommand('syntaxhighlight'));
              editor.ui.addButton && editor.ui.addButton('syntaxhighlight',
                  {
                      label: editor.lang.syntaxhighlight.title,
                      icon: this.path + 'images/syntaxhighlight.gif',
                      command: 'syntaxhighlight',
                      toolbar: 'insert,99'
                  });

              if (editor.contextMenu) {
                  editor.addMenuGroup('code');
                  editor.addMenuItem('syntaxhighlight',
                      {
                          label: editor.lang.syntaxhighlight.edit,
                          icon: this.path + 'images/syntaxhighlight.gif',
                          command: 'syntaxhighlight',
                          group: 'code'
                      });
                  editor.contextMenu.addListener(function (element) {
                      if (element)
                          element = element.getAscendant('pre', true);
                      if (element && !element.isReadOnly() && element.hasClass(editor.config.syntaxhighlight_class))
                          return { syntaxhighlight: CKEDITOR.TRISTATE_OFF };
                      return null;
                  });
              }

              
              CKEDITOR.dialog.add(b, this.path + "dialogs/syntaxhighlight.js");
          }
      });