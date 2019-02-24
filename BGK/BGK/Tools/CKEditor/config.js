/**
 * @license Copyright (c) 2003-2012, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.html or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function (config) {
    config.uiColor = '#f1f1f1';
    config.autoParagraph = false;
    config.resizable = false;
    config.toolbarCanCollapse = true;
    config.resize_enabled = false;
    config.enterMode = CKEDITOR.ENTER_BR;
    config.shiftEnterMode = CKEDITOR.ENTER_P;
    config.removePlugins = 'elementspath';
    config.extraPlugins = 'insertpre';
	config.font_names = 'Segoe UI; Trebuchet MS; Calibri; Comic Sans MS; Tahoma; Verdana; Courier New';
	config.entities = false;
    config.toolbar_Standard = [
        { name: 'basicstyles', groups: ['basicstyles', 'cleanup'], items: ['Bold', 'Italic', 'Underline', 'Strike'] },
        { name: 'colors', items: ['TextColor', 'BGColor'] },
        { name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote'] },
        { name: 'justify', items: ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', '-'] },
        '/',
        { name: 'links', items: ['FontSize', 'Link', 'Unlink', 'Anchor', 'InsertPre'] },
        { name: 'insert', items: ['Image', 'Flash', 'Table', 'Smiley', 'SpecialChar'] },
        { name: 'tools', items: ['Maximize', '-', 'About'] }
    ];
};
