function getCoordinate(obj) {
    var pos =
  {
      "x": 0, "y": 0
  }

    pos.x = document.body.offsetLeft;
    pos.y = document.body.offsetTop;

    do {
        pos.x += obj.offsetLeft;
        pos.y += obj.offsetTop;

        obj = obj.offsetParent;
    }
    while (obj.tagName.toUpperCase() != 'BODY')

    return pos;
}

function showCatalog(obj) {
    var pos = getCoordinate(obj);
    var div = document.getElementById('ECS_CATALOG');

    if (div && div.style.display != 'block') {
        div.style.display = 'block';
        div.style.left = pos.x + "px";
        div.style.top = (pos.y + obj.offsetHeight - 1) + "px";
    }
}

function hideCatalog(obj) {
    var div = document.getElementById('ECS_CATALOG');

    if (div && div.style.display != 'none') div.style.display = "none";
}

function display_mode(str) {
    document.getElementById('display').value = str;
    setTimeout(doSubmit, 0);
    function doSubmit() { document.forms['listform'].submit(); }
}


/* 修复IE6以下版本PNG图片Alpha */
function fixpng() {
    var arVersion = navigator.appVersion.split("MSIE")
    var version = parseFloat(arVersion[1])

    if ((version >= 5.5) && (document.body.filters)) {
        for (var i = 0; i < document.images.length; i++) {
            var img = document.images[i]
            var imgName = img.src.toUpperCase()
            if (imgName.substring(imgName.length - 3, imgName.length) == "PNG") {
                var imgID = (img.id) ? "id='" + img.id + "' " : ""
                var imgClass = (img.className) ? "class='" + img.className + "' " : ""
                var imgTitle = (img.title) ? "title='" + img.title + "' " : "title='" + img.alt + "' "
                var imgStyle = "display:inline-block;" + img.style.cssText
                if (img.align == "left") imgStyle = "float:left;" + imgStyle
                if (img.align == "right") imgStyle = "float:right;" + imgStyle
                if (img.parentElement.href) imgStyle = "cursor:hand;" + imgStyle
                var strNewHTML = "<span " + imgID + imgClass + imgTitle
           + " style=\"" + "width:" + img.width + "px; height:" + img.height + "px;" + imgStyle + ";"
           + "filter:progid:DXImageTransform.Microsoft.AlphaImageLoader"
           + "(src=\'" + img.src + "\', sizingMethod='scale');\"></span>"
                img.outerHTML = strNewHTML
                i = i - 1
            }
        }
    }
}

function hash(string, length) {
    var length = length ? length : 32;
    var start = 0;
    var i = 0;
    var result = '';
    filllen = length - string.length % length;
    for (i = 0; i < filllen; i++) {
        string += "0";
    }
    while (start < string.length) {
        result = stringxor(result, string.substr(start, length));
        start += length;
    }
    return result;
}

function stringxor(s1, s2) {
    var s = '';
    var hash = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ';
    var max = Math.max(s1.length, s2.length);
    for (var i = 0; i < max; i++) {
        var k = s1.charCodeAt(i) ^ s2.charCodeAt(i);
        s += hash.charAt(k % 52);
    }
    return s;
}

var evalscripts = new Array();
function evalscript(s) {
    if (s.indexOf('<script') == -1) return s;
    var p = /<script[^\>]*?src=\"([^\>]*?)\"[^\>]*?(reload=\"1\")?(?:charset=\"([\w\-]+?)\")?><\/script>/ig;
    var arr = new Array();
    while (arr = p.exec(s)) appendscript(arr[1], '', arr[2], arr[3]);
    return s;
}

function $$(id) {
    return document.getElementById(id);
}

function appendscript(src, text, reload, charset) {
    var id = hash(src + text);
    if (!reload && in_array(id, evalscripts)) return;
    if (reload && $$(id)) {
        $$(id).parentNode.removeChild($$(id));
    }
    evalscripts.push(id);
    var scriptNode = document.createElement("script");
    scriptNode.type = "text/javascript";
    scriptNode.id = id;
    //scriptNode.charset = charset;
    try {
        if (src) {
            scriptNode.src = src;
        }
        else if (text) {
            scriptNode.text = text;
        }
        $$('append_parent').appendChild(scriptNode);
    }
    catch (e)
  { }
}

function in_array(needle, haystack) {
    if (typeof needle == 'string' || typeof needle == 'number') {
        for (var i in haystack) {
            if (haystack[i] == needle) {
                return true;
            }
        }
    }
    return false;
}

var pmwinposition = new Array();

var userAgent = navigator.userAgent.toLowerCase();
var is_opera = userAgent.indexOf('opera') != -1 && opera.version();
var is_moz = (navigator.product == 'Gecko') && userAgent.substr(userAgent.indexOf('firefox') + 8, 3);
var is_ie = (userAgent.indexOf('msie') != -1 && !is_opera) && userAgent.substr(userAgent.indexOf('msie') + 5, 3);
function pmwin(action, param) {
    var objs = document.getElementsByTagName("OBJECT");
    if (action == 'open') {
        for (i = 0; i < objs.length; i++) {
            if (objs[i].style.visibility != 'hidden') {
                objs[i].setAttribute("oldvisibility", objs[i].style.visibility);
                objs[i].style.visibility = 'hidden';
            }
        }
        var clientWidth = document.body.clientWidth;
        var clientHeight = document.documentElement.clientHeight ? document.documentElement.clientHeight : document.body.clientHeight;
        var scrollTop = document.body.scrollTop ? document.body.scrollTop : document.documentElement.scrollTop;
        var pmwidth = 800;
        var pmheight = clientHeight * 0.9;
        if (!$$('pmlayer')) {
            div = document.createElement('div'); div.id = 'pmlayer';
            div.style.width = pmwidth + 'px';
            div.style.height = pmheight + 'px';
            div.style.left = ((clientWidth - pmwidth) / 2) + 'px';
            div.style.position = 'absolute';
            div.style.zIndex = '999';
            document.body.appendChild(div);
            $$('pmlayer').innerHTML = '<div style="width: 800px; margin: 5px auto; text-align: left">' +
        '<div style="width: 800px; height: ' + pmheight + 'px; padding: 1px; background: #FFFFFF; border: 4px solid #68b1e5; position: relative; left: -6px; top: -3px">' +
        '<div onmousedown="pmwindrag(event, 1)" onmousemove="pmwindrag(event, 2)" onmouseup="pmwindrag(event, 3)" style="cursor: move; position: relative; left: 0px; top: 0px; width: 800px; height: 30px; margin-bottom: -30px;"></div>' +
        '<a href="###" onclick="pmwin(\'close\')"><img style="position: absolute; right: 20px; top: 15px" src="images/close.gif" title="关闭" /></a>' +
        '<iframe id="pmframe" name="pmframe" style="width:' + pmwidth + 'px;height:100%" allowTransparency="true" frameborder="0"></iframe></div></div>';
        }
        $$('pmlayer').style.display = '';
        $$('pmlayer').style.top = ((clientHeight - pmheight) / 2 + scrollTop) + 'px';
        if (!param) {
            pmframe.location = 'pm.php';
        }
        else {
            pmframe.location = 'pm.php?' + param;
        }
    }
    else if (action == 'close') {
        for (i = 0; i < objs.length; i++) {
            if (objs[i].attributes['oldvisibility']) {
                objs[i].style.visibility = objs[i].attributes['oldvisibility'].nodeValue;
                objs[i].removeAttribute('oldvisibility');
            }
        }
        hiddenobj = new Array();
        $$('pmlayer').style.display = 'none';
    }
}

var pmwindragstart = new Array();
function pmwindrag(e, op) {
    if (op == 1) {
        pmwindragstart = is_ie ? [event.clientX, event.clientY] : [e.clientX, e.clientY];
        pmwindragstart[2] = parseInt($$('pmlayer').style.left);
        pmwindragstart[3] = parseInt($$('pmlayer').style.top);
        doane(e);
    }
    else if (op == 2 && pmwindragstart[0]) {
        var pmwindragnow = is_ie ? [event.clientX, event.clientY] : [e.clientX, e.clientY];
        $$('pmlayer').style.left = (pmwindragstart[2] + pmwindragnow[0] - pmwindragstart[0]) + 'px';
        $$('pmlayer').style.top = (pmwindragstart[3] + pmwindragnow[1] - pmwindragstart[1]) + 'px';
        doane(e);
    }
    else if (op == 3) {
        pmwindragstart = [];
        doane(e);
    }
}

function doane(event) {
    e = event ? event : window.event;
    if (is_ie) {
        e.returnValue = false;
        e.cancelBubble = true;
    }
    else if (e) {
        e.stopPropagation();
        e.preventDefault();
    }
}

//控制DIV显示隐藏
function Display(obj, targetobj, downclass, upclass) {
    this.obj = obj;
    this.Targetobj = document.getElementById(targetobj);
    this.DownClass = downclass;
    this.UpClass = upclass;
}

Display.prototype._switch = function () {
    this.Targetobj.style.display = this.Targetobj.style.display == "" ? "none" : "";
    this.obj.className = this.obj.className == this.DownClass ? this.UpClass : this.DownClass;
}

function copy(con) {
    window.clipboardData.setData("Text", con);
    alert("已复制");
}

function getPosition() {
    var top = document.documentElement.scrollTop;
    var left = document.documentElement.scrollLeft;
    var height = document.documentElement.clientHeight;
    var width = document.documentElement.clientWidth;

    return { top: top, left: left, height: height, width: width };
}

//取得父窗口浏览器可见区高度
function getClientHeight() {
    var clientHeight = 0;
    if (parent.document.body.clientHeight && parent.document.documentElement.clientHeight) {
        var clientHeight = (parent.document.body.clientHeight < parent.document.documentElement.clientHeight) ? parent.document.body.clientHeight : parent.document.documentElement.clientHeight;
    }
    else {
        var clientHeight = (parent.document.body.clientHeight > parent.document.documentElement.clientHeight) ? parent.document.body.clientHeight : parent.document.documentElement.clientHeight;
    }
    return clientHeight;
}

//function getPos(o) {
//    var t = o.offsetTop;
//    var l = o.offsetLeft;
//    while (o = o.offsetParent) {
//        t += o.offsetTop;
//        l += o.offsetLeft;
//    }
//    var pos = { top: t, left: l };
//    return pos;
//}
function getPos(obj) {
    var x = y = 0;
    while (obj.offsetParent) {
        x += obj.offsetLeft;
        y += obj.offsetTop;
        obj = obj.offsetParent;
    }
    return { "x": x, "y": y };
}

function pop() {
    if (navigator.userAgent.indexOf("MSIE") != -1) {
        this.classname = "ie_pop_style";
        this.iframe_class = "ie_pop_iframe_style";
    }
    else {
        this.classname = "firefox_pop_style";
        this.iframe_class = "firefox_iframe_style";
    }
}

pop.prototype.show = function () {
    var _pop = parent.document.createElement("div");
    _pop.id = "sys_pop";
    _pop.style.width = "100%";
    _pop.style.height = "" + getClientHeight() + "px";
    _pop.className = this.classname;

    var _frame = parent.document.createElement("iframe");
    _frame.frameBorder = "0";
    _frame.src = "http://www.163.com";
    _frame.className = this.iframe_class;

    parent.document.body.appendChild(_pop);
    parent.document.body.appendChild(_frame);
}

window.onload = function () {
    //new pop().show();
}

function pop_close(_pop) {
    var obj = document.getElementById(_pop);

    if (obj) {
        document.body.removeChild(obj);
    }
}
/*
显示文件上传窗口
obj 点击显示窗口的对象
upload_type 文件上传类型，有file,photo,video
target_obj 目标接收返回地址的对象
save_folder 文件保存路径
*/
function upload(obj, upload_type, target_obj, save_folder) {
    var pos = getPos(obj);

    if (document.getElementById('pop_upload'))
        pop_close();

    var pop = document.createElement("div");
    pop.id = "pop_upload";
    pop.className = "pop";

    pop.style.left = pos.x + 5 + "px";
    pop.style.top = pos.y + obj.offsetHeight + "px";
    pop.style.display = "block";
    pop.style.position = "absolute";

    document.body.appendChild(pop);

    var close = document.createElement("a");
    close.innerHTML = "×";
    close.title = "关闭";
    close.href = "javascript:pop_close('pop_upload');";
    pop.appendChild(close);

    var iframe = document.createElement("iframe");
    iframe.src = "/tools/upload.aspx?t=" + escape(upload_type) + "&target_obj=" + escape(target_obj) + "&save_folder=" + escape(save_folder) + "";
    iframe.width = "100%";
    iframe.height = "60";
    iframe.frameBorder = "0";

    document.getElementById(pop.id).appendChild(iframe);

    iframe.src = iframe.src;

}
/*上传带返回小图链接*/
function upload(obj, upload_type, target_obj, save_folder, imgobj) {
    var pos = getPos(obj);

    if (document.getElementById('pop_upload'))
        pop_close();

    var pop = document.createElement("div");
    pop.id = "pop_upload";
    pop.className = "pop";

    pop.style.left = pos.x + 5 + "px";
    pop.style.top = pos.y + obj.offsetHeight + "px";
    pop.style.display = "block";
    pop.style.position = "absolute";

    document.body.appendChild(pop);

    var close = document.createElement("a");
    close.innerHTML = "×";
    close.title = "关闭";
    close.href = "javascript:pop_close('pop_upload');";
    pop.appendChild(close);

    var iframe = document.createElement("iframe");
    iframe.src = "/tools/upload.aspx?t=" + escape(upload_type) + "&target_obj=" + escape(target_obj) + "&imgobj=" + escape(imgobj) + "&save_folder=" + escape(save_folder) + "";
    iframe.width = "100%";
    iframe.height = "60";
    iframe.frameBorder = "0";

    document.getElementById(pop.id).appendChild(iframe);

    iframe.src = iframe.src;

}

/*删除html标签*/
function DelHtml(Word) {
    a = Word.indexOf("<");
    b = Word.indexOf(">");
    len = Word.length;
    c = Word.substring(0, a);
    if (b == -1)
        b = a;
    d = Word.substring((b + 1), len);
    Word = c + d;
    tagCheck = Word.indexOf("<");
    if (tagCheck != -1)
        Word = DelHtml(Word);
    return Word;
}

/*
自动获取关键词
*/
function GetKeywords(titleName, contentName, keywordsInputName) {
    $.ajax({
        type: "GET",
        url: "http://keyword.discuz.com/related_kw.html",
        data: "title=" + DelHtml($(':input[name="' + titleName + '"]').val()) + "&content=" + DelHtml($(':input[name="' + contentName + '"]').val()) + "&ics=gbk&ocs=utf-8",
        dataType: "xml",
        success: function (xml) {
            $(':input[name="' + keywordsInputName + '"]').val('');
            $(xml).find('kw').each(function () {
                if ($(':input[name="' + keywordsInputName + '"]').val() != "") {
                    $(':input[name="' + keywordsInputName + '"]').val($(':input[name="' + keywordsInputName + '"]').val() + ',' + $(this).text());
                }
                else {
                    $(':input[name="' + keywordsInputName + '"]').val($(this).text());
                }
            })
        }
    });
}

var Ajax = new Object;
/* *
* 调用此方法发送HTTP请求。
*
* @public
* @param   {string}    url             请求的URL地址
* @param   {mix}       params          发送参数
* @param   {Function}  callback        回调函数
* @param   {string}    ransferMode     请求的方式，有"GET"和"POST"两种
* @param   {string}    responseType    响应类型，有"JSON"、"XML"和"TEXT"三种
* @param   {boolean}   asyn            是否异步请求的方式
* @param   {boolean}   quiet           是否安静模式请求
*/
Ajax.call = function (url, params, callback, transferMode, responseType, asyn, quiet) {
    $.ajax({
        type: transferMode,
        url: url,
        data: params,
        success: callback,
        dataType: responseType.toString().toLowerCase(),
        async: asyn,
        timeout: 36000000,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //document.writeln(url + "&" + params);
            //alert("error:" + textStatus + "，来源：" + url + "&" + params); 
        },
        cache: false
    });
}

/* *
* 截取小数位数
*/
function advFormatNumber(value, num) // 四舍五入
{
    var a_str = formatNumber(value, num);
    var a_int = parseFloat(a_str);
    if (value.toString().length > a_str.length) {
        var b_str = value.toString().substring(a_str.length, a_str.length + 1);
        var b_int = parseFloat(b_str);
        if (b_int < 5) {
            return a_str;
        }
        else {
            var bonus_str, bonus_int;
            if (num == 0) {
                bonus_int = 1;
            }
            else {
                bonus_str = "0."
                for (var i = 1; i < num; i++)
                    bonus_str += "0";
                bonus_str += "1";
                bonus_int = parseFloat(bonus_str);
            }
            a_str = formatNumber(a_int + bonus_int, num)
        }
    }
    return a_str;
}

function formatNumber(value, num) // 直接去尾
{
    var a, b, c, i;
    a = value.toString();
    b = a.indexOf('.');
    c = a.length;
    if (num == 0) {
        if (b != -1) {
            a = a.substring(0, b);
        }
    }
    else {
        if (b == -1) {
            a = a + ".";
            for (i = 1; i <= num; i++) {
                a = a + "0";
            }
        }
        else {
            a = a.substring(0, b + num + 1);
            for (i = c; i <= b + num; i++) {
                a = a + "0";
            }
        }
    }
    return a;
}

//修改模板
var updateTlp = function (obj) {
    if (confirm("你确定修改显示模板？ ")) {
        $(obj).hide();
        $(obj).next("select").show();
    }

}

//UI自带gritter弹出消息
var alert_gritter = function (title, content, time) {
    $.gritter.add({
        title: title,
        text: content,
        time: time
    });
}

/* *
* type：1成功，2信息，3警告，4错误
*position:toast-top-right(右上)，toast-bottom-right（右下），toast-bottom-left（左下），toast-top-left（左上），toast-top-full-width（顶部全宽），toast-bottom-full-width（底部全宽），toast-top-center（顶部居中），toast-bottom-center（底部居中）
*/
var alert_toastr = function (content, title, type, position) {
    if (type == null) {
        type = 1;
    }
    if (position == null) {
        position = "toast-top-center";
    }
    if (title == null) {
        title = "提示信息";
    }
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "progressBar": true,
        "positionClass": position,
        "onclick": null,
        "showDuration": "400",
        "hideDuration": "1000",
        "timeOut": "7000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
    switch (type) {
        case 1:
            toastr.success(content, title);
            break;
        case 2:
            toastr.info(content, title);
            break;
        case 3:
            toastr.warning(content, title);
            break;
        case 4:
            toastr.error(content, title);
            break;
    }

}

/* *
* obj：显示对象
*start:开始标签
*end：结束标签http://seo.chinaz.com/template/default/images/spinner.gif
*/
var getSeo = function (obj, start, end, type) {
    type = type == null ? 0 : type;
    jQuery.post("updatemoreimg/ajax.aspx?act=baidu_seo", { s: start, e: end, type: type }, function (data) {
        if (data.content != "") {
            $(obj).html(data.content);
        }
    }, "json");
}

/* *
* obj：显示对象
*/
var getWord = function (title, content) {
    var tags = $("#tag").val();
    var keyword = $("textarea[name='pagekeywords']").val();
    var titleenc = document.getElementById(title).value;
    var contentenc = DelHtml(document.getElementById(content).value);
    if ((keyword == "" || keyword == null) && (tags == "" || tags == null)) {
        jQuery.post("updatemoreimg/ajax.aspx?act=words", { title: titleenc, content: contentenc, s: 'rel="nofollow">', e: "</a>" }, function (data) {
            if (data.content != "") {
                $("textarea[name='pagekeywords']").val(data.content);
                //$("input[name='pagetitle']").val(data.content + '-' + data.message); 标题需要获取关键词来处理
                for (var i = 0; i < 4; i++) {
                    tags += data.content.split(',')[i] + ",";
                }
                $("#tag").val(tags.substring(0, tags.lastIndexOf(',')));
            }
        }, "json");
    }
}