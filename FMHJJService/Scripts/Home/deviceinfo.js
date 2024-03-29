﻿
$(function () {
    //1.初始化input控件
    var oFileInput = new FileInput();
    oFileInput.Init("txt_file", "/device/DeviceUpdate");

    //2.注册导入按钮的事件
    $("#btnFileInput").click(function () {
        $("#myModal1").modal();
    });
});

//初始化fileinput
var FileInput = function () {
    var oFile = new Object();

    //初始化fileinput控件（第一次初始化）
    oFile.Init = function (ctrlName, uploadUrl) {
        var control = $('#' + ctrlName);

        //初始化上传控件的样式
        control.fileinput({
            language: 'zh', //设置语言
            uploadUrl: uploadUrl, //上传的地址
            //allowedFileExtensions: ['jpg', 'gif', 'png'],//接收的文件后缀
            showUpload: false, //是否显示上传按钮
            showCaption: false,//是否显示标题
            browseClass: "btn btn-primary", //按钮样式     
            //dropZoneEnabled: false,//是否显示拖拽区域
            //minImageWidth: 50, //图片的最小宽度
            //minImageHeight: 50,//图片的最小高度
            //maxImageWidth: 1000,//图片的最大宽度
            //maxImageHeight: 1000,//图片的最大高度
            //maxFileSize: 0,//单位为kb，如果为0表示不限制文件大小
            //minFileCount: 0,
            maxFileCount: 100,
            enctype: 'multipart/form-data',
            validateInitialCount: true,
            previewFileIcon: "<i class='glyphicon glyphicon-king'></i>",

            msgFilesTooMany: "选择上传的文件数量({n}) 超过允许的最大数值{m}！",
        });

        //导入文件上传完成之后的事件
        $("#txt_file").on("fileuploaded", function (event, data, previewId, index) {
            $("#myModal1").modal("hide");
            var data = data.response.lstOrderImport;
            if (data == undefined) {
                //toastr.error('文件格式类型不正确');
                return;
            }
            //1.初始化表格
            var oTable = new TableInit();
            oTable.Init(data);
            $("#div_startimport").show();
        });
    }
    return oFile;
};