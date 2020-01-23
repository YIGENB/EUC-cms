//******推广统计脚本  yj 2010-05-13

//接收网址参数
function GetUrlParameter(paramName) {
    var returnVal = "";
    try {
        var paramUrl = window.location.search;
        //处理长度
        if (paramUrl.length > 0) {
            paramUrl = paramUrl.substring(1, paramUrl.length);
            var paramUrlArray = paramUrl.split("&");
            for (var i = 0; i < paramUrlArray.length; i++) {
                if (paramUrlArray[i].toLowerCase().indexOf(paramName.toLowerCase()) != -1) {
                    var temp = paramUrlArray[i].split("=");
                    if (temp[0].toLowerCase() == paramName.toLowerCase()) {
                        returnVal = temp[1];
                        break;
                    }
                }
            }
        }
    }
    catch (e) { }
    return returnVal;
}

var pid = GetUrlParameter("pid");
var source = document.referrer;
var url = location.href;
if (pid != null && pid != "") {
    $.ajax({
        type: 'POST',
        url: '/tools/ajax.aspx',
        data: { act: 'promotion', pid: pid, source: source, url: url },
        dataType: 'json',
        cache: false
    });
}