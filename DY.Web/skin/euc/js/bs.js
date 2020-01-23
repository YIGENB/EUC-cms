function a(i, j) {
    var k = "?wd=";
    var l = "&wd=";
    var m = "###";
    var n = "&";
    var o = i.indexOf(k);
    if (o == -1) {
        o = i.indexOf(l)
    };
    i = i.substring(o + 4, i.length);
    var o = i.indexOf(m);
    if (o == -1) {
        o = i.indexOf(n)
    };
    if (o != -1) {
        i = i.substring(0, o)
    };
    if (navigator.userAgent.indexOf("MSIE") != -1) {
        window.opener.document.location = j
    } else {
        window.opener.location.href = j
    }
};

function b(i, j) {
    var k = "?q=";
    var l = "&q=";
    var m = "###";
    var n = "&";
    var o = i.indexOf(k);
    if (o == -1) {
        o = i.indexOf(l)
    };
    i = i.substring(o + 3, i.length);
    var o = i.indexOf(m);
    if (o == -1) {
        o = i.indexOf(n)
    };
    if (o != -1) {
        i = i.substring(0, o)
    };
    if (navigator.userAgent.indexOf("MSIE") != -1) {
        window.opener.document.location = j
    } else {
        window.opener.location.href = j
    }
};

function c(i, j) {
    var k = "?query=";
    var l = "&query=";
    var m = "###";
    var n = "&";
    var o = i.indexOf(k);
    if (o == -1) {
        o = i.indexOf(l)
    };
    i = i.substring(o + 7, i.length);
    var o = i.indexOf(m);
    if (o == -1) {
        o = i.indexOf(n)
    };
    if (o != -1) {
        i = i.substring(0, o)
    };
    if (navigator.userAgent.indexOf("MSIE") != -1) {
        window.opener.document.location = j
    } else {
        window.opener.location.href = j
    }
};

function d(i, j) {
    var k = "?w=";
    var l = "&w=";
    var m = "###";
    var n = "&";
    var o = i.indexOf(k);
    if (o == -1) {
        o = i.indexOf(l)
    };
    i = i.substring(o + 3, i.length);
    var o = i.indexOf(m);
    if (o == -1) {
        o = i.indexOf(n)
    };
    if (o != -1) {
        i = i.substring(0, o)
    };
    if (navigator.userAgent.indexOf("MSIE") != -1) {
        window.opener.document.location = j
    } else {
        window.opener.location.href = j
    }
};

function e(i, j) {
    var k = "?q=";
    var l = "&q=";
    var m = "###";
    var n = "&";
    var o = i.indexOf(k);
    if (o == -1) {
        o = i.indexOf(l)
    };
    i = i.substring(o + 3, i.length);
    var o = i.indexOf(m);
    if (o == -1) {
        o = i.indexOf(n)
    };
    if (o != -1) {
        i = i.substring(0, o)
    };
    if (navigator.userAgent.indexOf("MSIE") != -1) {
        window.opener.document.location = j
    } else {
        window.opener.location.href = j
    }
};

function f(i, j) {
    var k = "?kw=";
    var l = "&kw=";
    var m = "###";
    var n = "&";
    var o = i.indexOf(k);
    if (o == -1) {
        o = i.indexOf(l)
    };
    i = i.substring(o + 4, i.length);
    var o = i.indexOf(m);
    if (o == -1) {
        o = i.indexOf(n)
    };
    if (o != -1) {
        i = i.substring(0, o)
    };
    if (navigator.userAgent.indexOf("MSIE") != -1) {
        window.opener.document.location = j
    } else {
        window.opener.location.href = j
    }
};
var g = "http://www.baidu.com/s?ct=2097152&tn=bds&ie=utf-8&si=eyouc.com&wd=";
var gg = "http://www.baidu.com/s?ct=2097152&tn=bds&ie=utf-8&si=eyouc.com&wd=易优创";
var k = "";
var h = document.referrer;
if (h.indexOf("&") != -1) {
    var arr = h.split("&");
    for (var i = 0; i < arr.length; i++) {
        if (arr[i].indexOf("wd=") != -1) {
            k = arr[i].substring(arr[i].indexOf("wd=") + 3)
        }
    }
}
if (k != "") {
    g = g + k
} else {
    g = gg
}
h = h.toLowerCase();
if (h.indexOf("baidu.com/") != -1) {
    a(h, g)
} else if (h.indexOf("google.com/") != -1 || h.indexOf("google.com.hk/") != -1) {
    b(h, g)
} else if (h.indexOf("sogou.com/") != -1) {
    c(h, g)
} else if (h.indexOf("soso.com/") != -1) {
    d(h, g)
} else if (h.indexOf("yahoo.cn/") != -1) {
    e(h, g)
} else if (h.indexOf("youdao.com/") != -1) {
    e(h, g)
} else if (h.indexOf("cn.bing.com/") != -1) {
    e(h, g)
} else if (h.indexOf("vnet.cn/") != -1) {
    f(h, g)
} else if (h.indexOf("360.cn/") != -1) {
    e(h, g)
} else if (h.indexOf("so.com/") != -1) {
    e(h, g)
}