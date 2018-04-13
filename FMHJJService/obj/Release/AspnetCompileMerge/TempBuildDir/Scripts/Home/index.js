Date.prototype.addHours = function (h) {
    this.setTime(this.getTime() + (h * 60 * 60 * 1000));
    return this;
}

Date.prototype.Format = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1,
        "d+": this.getDate(),
        "H+": this.getHours(),
        "m+": this.getMinutes(),
        "s+": this.getSeconds(),
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度
        "S": this.getMilliseconds() //毫秒
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
    if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
};

function isInt(a) {
    return parseInt(a, 10) == a ? true : false;
}

function isFloat(a) {
    return parseFloat(a, 10) == a ? true : false;
}

function getParameter(name) {
    var query = window.location.href;  //获取网址字符串
    query = decodeURIComponent(query);
    var iLen = name.length;
    var iStart = query.indexOf(name);
    if (iStart < 0)
    {
        return "";
    }
    iStart += iLen + 1;
    var iEnd = query.indexOf("&", iStart);
    return (iEnd < 0) ? query.substr(iStart) : query.substr(iStart, iEnd);
}

var userAgent = window.navigator.userAgent.toLowerCase();
var isIE = true;
var rIe11 = /rv:([\d.]+)\) like gecko/,
    rMsie = /(msie\s|trident.*rv:)([\w.]+)/,
    rFirefox = /(firefox)\/([\w.]+)/,
    rOpera = /(opera).+version\/([\w.]+)/,
    rChrome = /(chrome)\/([\w.]+)/,
    rSafari = /version\/([\w.]+).*(safari)/;
var browserMatch = uaMatch(userAgent);
if (browserMatch.browser != "IE") {
    isIE = false;
}

function uaMatch(ua) {
    var match = rIe11.exec(ua);
    if (match != null) {
        return { browser: "IE", version: match[2] || "0" };
    }
    var match = rMsie.exec(ua);
    if (match != null) {
        return { browser: "IE", version: match[2] || "0" };
    }
    var match = rFirefox.exec(ua);
    if (match != null) {
        return { browser: match[1] || "", version: match[2] || "0" };
    }
    var match = rOpera.exec(ua);
    if (match != null) {
        return { browser: match[1] || "", version: match[2] || "0" };
    }
    var match = rChrome.exec(ua);
    if (match != null) {
        return { browser: match[1] || "", version: match[2] || "0" };
    }
    var match = rSafari.exec(ua);
    if (match != null) {
        return { browser: match[2] || "", version: match[1] || "0" };
    }
    if (match != null) {
        return { browser: "", version: "0" };
    }
}

var msie11 = isIE && /rv:([\d.]+)\) like gecko/.test(userAgent);
var msie10 = isIE && /msie 10\.0/i.test(userAgent);
var msie9 = isIE && /msie 9\.0/i.test(userAgent);
var msie8 = isIE && /msie 8\.0/i.test(userAgent);
var msie7 = isIE && /msie 7\.0/i.test(userAgent);
var msie6 = !msie8 && !msie7 && isIE && /msie 6\.0/i.test(userAgent);

function layerConfirm(msg, t) {
    top.layer.confirm(msg, function (index) {
        t.onclick = "";
        t.click();
        top.layer.close(index);
    });
}

function getEventKeyCode() {
    return event.keyCode || event.which;
}

function executeScript(html) {
    var reg = /<script[^>]*>([^\x00]+)$/i;
    //对整段HTML片段按<\/script>拆分
    var htmlBlock = html.split("<\/script>");
    for (var i in htmlBlock) {
        var blocks;//匹配正则表达式的内容数组，blocks[1]就是真正的一段脚本内容，因为前面reg定义我们用了括号进行了捕获分组
        if (blocks = htmlBlock[i].match(reg)) {
            //清除可能存在的注释标记，对于注释结尾-->可以忽略处理，eval一样能正常工作
            var code = blocks[1].replace(/<!--/, '');
            try {
                eval(code) //执行脚本
            }
            catch (e) {
            }
        }
    }
}

function doFileExport(inName, inStr) {
    var xlsWin = null;
    var width = 1; var height = 1;
    var openPara = "left=" + (window.screen.width / 2 + width / 2) + ",top=" + (window.screen.height + height / 2) +
     ",scrollbars=no,width=" + width + ",height=" + height;
    xlsWin = window.open("", "_blank", openPara);
    xlsWin.document.write(inStr);
    xlsWin.document.close();
    xlsWin.document.execCommand('Saveas', true, inName);
    xlsWin.close();
}

function openFile(url) {
    var openDocObj;
    try {
        openDocObj = new ActiveXObject("SharePoint.OpenDocuments.1");
        openDocObj.ViewDocument(GetLocationUrl(url));
    } catch (e) {
        alert("请确认：\r\n1. Microsoft Office or WPS 已经安装。\r\n2. Internet 选项=>安全=>设置 \"Enable unsafe ActiveX\"。");
    }
}

function downloadFile(url) {
    try {
        var elemIF = document.createElement("iframe");
        elemIF.src = url;
        elemIF.style.display = "none";
        document.body.appendChild(elemIF);
    } catch (e) {
        top.layer.msg(e.message);
    }    
}

//获取指定页面完整URL地址
function GetLocationUrl(url) {
    var LocationUrl = "";
    LocationUrl = top.window.location.host;
    LocationUrl = "http://" + LocationUrl + url;
    return LocationUrl;
}

function clickTrAddClass() {
    var lastSel;
    var lastClass;
    $(".table tr:not(:first)").click(function () {
        var th = $(this).children("th");
        if (th.length < 1) {
            if (lastSel && lastSel != this)
            {
                $(lastSel).addClass(lastClass);
            }
            var currentClass = $(this).attr("class");
            if (!$(this).hasClass("active") && currentClass != "")
            {
                lastSel = this;
                lastClass = currentClass;
                $(this).removeClass(currentClass);
            }
            $(this).addClass("active");
            $(this).nextAll().removeClass("active");
            $(this).prevAll().removeClass("active");
        }
    });
}

function openHcmDetail(src) {
    top.layer.open({
        type: 2,
        title: '火车煤详细信息',
        shadeClose: true,
        shade: 0.8,
        area: ['100%', '100%'],
        content: src //iframe的url
        //, end: function () {
        //    location.reload();
        //}
    });
}

function openSymDetail(src) {
    top.layer.open({
        type: 2,
        title: '水运煤详细信息',
        shadeClose: true,
        shade: 0.8,
        area: ['100%', '100%'],
        content: src //iframe的url
        //, end: function () {
        //    location.reload();
        //}
    });
}

function openCyReportDetail(src,til) {
    top.layer.open({
        type: 2,
        title: til,
        shadeClose: true,
        shade: 0.8,
        area: ['90%', '90%'],
        content: src//iframe的url
        //,end: function () {
        //    location.reload();
        //}
    });
}

function openDialog(src, til, width, height, reload, layeropen) {
    if (layeropen) {
        layer.open({
            type: 2,
            title: til,
            shadeClose: true,
            shade: 0.8,
            area: [width, height],
            content: src, //iframe的url
            end: function () {
                if (reload) {
                    location.reload();
                }
            }
        });
    }
    else {
        top.layer.open({
            type: 2,
            title: til,
            shadeClose: true,
            shade: 0.8,
            area: [width, height],
            content: src, //iframe的url
            end: function () {
                if (reload) {
                    location.reload();
                }

            }
        });
    }
}

function openDeviceDetail(src) {
    top.layer.open({
        type: 2,
        title: '子设备详细信息',
        shadeClose: true,
        shade: 0.8,
        area: ['90%', '90%'],
        content: src //iframe的url       
    });
}

function openDeviceDetail(src, width, height) {
    top.layer.open({
        type: 2,
        title: '子设备详细信息',
        shadeClose: true,
        shade: 0.8,
        area: [width, height],
        content: src //iframe的url        
    });
}

function openDevice(src) {
    top.layer.open({
        type: 2,
        title: '设备信息',
        shadeClose: true,
        shade: 0.8,
        area: ['80%', '80%'],
        content: src //iframe的url        
    });
}

$(function () {
    //if ($('.table-headerfixed > tbody') != null) $('.table-headerfixed > tbody').height($(window).height() == 0 ? (544 - 161) : $(window).height() - 161);
    //ie10以下采用jquery.validate.js进行表单验证
    if (isIE && !msie10 && !msie11) {
        $("form").validate({ meta: "validate" });
        $("form").submit(function (e) {
            return $("form").valid();
        });
    }
});