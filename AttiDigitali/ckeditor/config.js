/*
Copyright (c) 2003-2011, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function( config )
{
	// Define changes to default configuration here. For example:
	config.language = 'it';
	config.toolbar = 'Full_Provvedimenti';
 
config.toolbar_Full_Provvedimenti =
[
    ['Save','Preview'],
    ['Cut','Copy','Paste','PasteText','PasteFromWord','-','Print', 'SpellChecker'],
    ['Undo','Redo','-','Find','Replace','-','SelectAll','RemoveFormat'],
    '/',
    ['Bold','Italic','Underline','Strike','-','Subscript','Superscript'],
    ['NumberedList','BulletedList','-','Outdent','Indent','Blockquote'],
    ['JustifyLeft','JustifyCenter','JustifyRight','JustifyBlock'],
    ['Table','HorizontalRule','SpecialChar','PageBreak'],
    '/',
    ['Format','Font','FontSize'],
    ['TextColor','BGColor']
];
 
config.toolbar_Basic =
[
    ['Preview']
];

};
