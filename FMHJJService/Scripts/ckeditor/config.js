/**
 * @license Copyright (c) 2003-2016, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function (config) {
    basePath = '/Scripts/ckeditor/';   //配置CKEditor的根路径  
    config.language = 'zh-cn';  // 中文
    config.font_names='宋体/宋体;黑体/黑体;仿宋/仿宋_GB2312;楷体/楷体_GB2312;隶书/隶书;幼圆/幼圆;微软雅黑/微软雅黑;' + config.font_names;
    config.tabSpaces = 4;       // 当用户键入TAB时，编辑器走过的空格数，当值为0时，焦点将移出编辑框
    config.toolbar = "Custom_RainMan";    // 工具条配置
    config.toolbar_Custom_RainMan = [
        ['Undo', 'Redo', '-', 'SelectAll', 'RemoveFormat'],        
        ['Bold', 'Italic', 'Underline', 'Strike', '-', 'NumberedList', 'BulletedList'],
        ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
        ['Link', 'Image', 'Flash', 'Table', 'HorizontalRule'],
        '/',
        ['Styles', 'Format', 'Font', 'FontSize'],
        ['SpecialChar', 'TextColor', 'BGColor', 'Maximize', 'Source']
    ];
    config.allowedContent = true;
    config.image_previewText = ' ';
    //config.enterMode = CKEDITOR.ENTER_BR;
    //config.shiftEnterMode = CKEDITOR.ENTER_P;
    //config.startupMode = 'source';
    config.autoParagraph = false;
    //config.filebrowserUploadUrl = '/Scripts/asp.net/upload_json.ashx?Type=file'; //上传文件的保存路径
    //config.filebrowserImageUploadUrl = '/Scripts/asp.net/upload_json.ashx?Type=image'; //上传图片的保存路径
    //config.filebrowserFlashUploadUrl = '/Scripts/asp.net/upload_json.ashx?Type=flash'; //上传flash的保存路径
};

//CKEDITOR.on('instanceReady', function (ev) {
//    with (ev.editor.dataProcessor.writer) {
//        setRules("p", { indent: false, breakAfterOpen: false, breakBeforeClose: false });
//        setRules("h1", { indent: false, breakAfterOpen: false, breakBeforeClose: false });
//        setRules("h2", { indent: false, breakAfterOpen: false, breakBeforeClose: false });
//        setRules("h3", { indent: false, breakAfterOpen: false, breakBeforeClose: false });
//        setRules("h4", { indent: false, breakAfterOpen: false, breakBeforeClose: false });
//        setRules("h5", { indent: false, breakAfterOpen: false, breakBeforeClose: false });
//        setRules("div", { indent: false, breakAfterOpen: false, breakBeforeClose: false });
//        setRules("table", { indent: false, breakAfterOpen: false, breakBeforeClose: false });
//        setRules("tr", { indent: false, breakAfterOpen: false, breakBeforeClose: false });
//        setRules("td", { indent: false, breakAfterOpen: false, breakBeforeClose: false });
//        setRules("iframe", { indent: false, breakAfterOpen: false, breakBeforeClose: false });
//        setRules("li", { indent: false, breakAfterOpen: false, breakBeforeClose: false });
//        setRules("ul", { indent: false, breakAfterOpen: false, breakBeforeClose: false });
//        setRules("ol", { indent: false, breakAfterOpen: false, breakBeforeClose: false });
//    }
//});
