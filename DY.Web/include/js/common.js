var goods = new Object;  //购物车
var user = new Object;  //用户
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
*/
Ajax.call = function(url, params, callback, transferMode, responseType) {
    $.ajax({
        type: transferMode,
        url: url,
        data: params,
        success: callback,
        dataType: responseType,
        cache: false
    });
}
//添加收藏夹
goods.AddFavorite = function(goodsId) {
    Ajax.call('/tools/ajax.aspx', { act: 'AddFavorite', goods_id: goodsId }, function(data) {
        alert(data.message);
    }, 'POST', 'json');
}
//删除收藏夹
goods.deleteFavorite = function(favorite_id) {
    Ajax.call('/tools/ajax.aspx', { act: 'deleteFavorite', id: favorite_id }, function(data) {
        window.top.location.reload();
    }, 'POST', 'json');
}
//登录
user.Login = function (form, url) {
    $(form).ajaxSubmit({
        dataType: 'json',
        success: function (data) {
            if (data.error == 1) {
                alert(data.message);
            }
            else {
                window.location.href = url;
                //window.top.location.reload();
            }
        }
    });
    return false;
}
//密码找回
user.Forget = function (form, url) {
    $(form).ajaxSubmit({
        dataType: 'json',
        success: function (data) {
            if (data.error == 1) {
                alert(data.message);
            }
            else {
                alert(data.message);
                window.location.href = url;
                //window.top.location.reload();
            }
        }
    });
    return false;
}
//保存个人信息
user.SaveProfile = function (form) {
    $(form).ajaxSubmit({
        dataType: 'json',
        url: '/user.aspx?act=SaveProfile',
        resetForm: true,
        success: function (data) {
            if (data.error == 1) {
                return false;
            }
            else {
                alert(data.message);
            }
        }
    });
    return false;
}
//保存密码
user.SavePassword = function (form) {
    $(form).ajaxSubmit({
        dataType: 'json',
        beforeSubmit: function () {
            var validator = new Validator("passwordForm");
            validator.required("oldpassword", "原来密码不能为空");
            validator.required("password", "新密码不能为空");
            validator.required("confirm_password", "确认密码不能为空");
            validator.eqaul("password", "confirm_password", "密码与确认密码不一致");
            return validator.passed();
        },
        success: function (data) {
            alert(data.message);
        }
    });
    return false;
}
//保存密码提示信息
user.SavePasswordSafe = function (form) {
    $(form).ajaxSubmit({
        dataType: 'json',
        beforeSubmit: function () {
            var validator = new Validator("PasswordSafeForm");
            validator.required("question", "提示问题不能为空");
            validator.required("answer", "提示答案不能为空");
            return validator.passed();
        },
        success: function (data) {
            alert(data.message);
        }
    });
    return false;
}
//注册
user.Reg = function (form, url) {
    $(form).ajaxSubmit({
        dataType: 'json',
        beforeSubmit: function () {
            if ($("#captcha").val() == "") { return false; }
        },
        success: function (data) {
            if (data.error == 1) {
                alert(data.message);
            }
            else {
                alert("恭喜你注册成功!");
                window.location.href = url;
                //window.top.location.reload();
            }
        }
    });
    return false;
}
//退出登录
user.Logout = function() {
    Ajax.call('/login.aspx', { act: 'logout' }, function(data) {
        if (data.error == 1) {
            alert(data.message);
        }
        else {
            window.top.location.reload();
        }
    }, 'POST', 'json');
}

//ajax提交
function AjaxSubmit(form) {
    $(form).ajaxSubmit({
        dataType: 'json',
        resetForm: true,
        success: function(data) {
            if (data.message != "") {
                alert(data.message);
            }

            if (data.error == 0) {
                if (typeof (url) != "undefined") {
                    window.location.href = url;
                }
                if (typeof (captcha) != "undefined") {
                    ReloadCaptcha();
                }
            }
        }
    });
    return false;
}

var res = false;
function FormvSubmit(form) {
    $(form).find(".sbtn").attr("disabled", false);
    $(form).submit(function() { if (res) { AjaxSubmit(form); res = false; } return false; });
}


//刷新验证码
function ReloadCaptcha() {
    $('.captcha').attr('src', '/tools/verifyimagepage.aspx?' + Math.random());
}

/*添加到收藏夹*/
function addFavorite(val) {
    if (document.all) {
        window.external.addFavorite(window.location, val);
    }
    else if (window.sidebar) {
        window.sidebar.addPanel(val, window.location, "");
    }
}
/*复制url地址*/
function copyUrl() {
    var clipBoardContent = this.location.href;
    window.clipboardData.setData("Text", clipBoardContent);
    alert("链接地址复制成功!");
}
/*设为首页*/
function setHomePage(obj){ 
var aUrls=document.URL.split("/"); 
var vDomainName="http://"+aUrls[2]+"/"; 
try{//IE 
obj.style.behavior="url(#default#homepage)"; 
obj.setHomePage(vDomainName); 
}catch(e){//other 
if(window.netscape) {//ff 
try { 
netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect"); 
} 
catch (e) { 
alert("此操作被浏览器拒绝！\n请在浏览器地址栏输入“about:config”并回车\n然后将[signed.applets.codebase_principal_support]设置为'true'"); 
} 
var prefs = Components.classes['@mozilla.org/preferences-service;1'].getService(Components.interfaces.nsIPrefBranch); 
prefs.setCharPref('browser.startup.homepage',vDomainName); 
} 
} 
if(window.netscape)alert("ff"); 
} 
function addFavorite(){ 
var aUrls=document.URL.split("/"); 
var vDomainName="http://"+aUrls[2]+"/"; 
var description=document.title; 
try{//IE 
window.external.AddFavorite(vDomainName,description); 
}catch(e){//FF 
window.sidebar.addPanel(description,vDomainName,""); 
} 
} 

/*推荐给朋友*/
var content;
content = "";
content = "给你推荐个好东东：[title]， 赶紧去看看，网址是：";
content += location.href;
function SendToMyFriend(title) {
    content = content.replace("[title]", title);
    window.clipboardData.setData("Text", content);
    alert("已经把信息拷贝到粘贴板里面，请把内容通过即时通讯工具发送给你的好朋友广而告之。");
}
/*改变文件大小*/
function FontSize(size) {
    $('#PageContent *').css({ "font-size": size + "px", "line-height": (size + 10) + "px" });
}

/*图片切换显示 */
function changeImage(tagname, i) {
    var tag = tagname + i;
    document.getElementById("infoImg").src = document.getElementById(tag).src.replace("ico_", "info_");
    document.getElementById("originalImg").href = document.getElementById(tag).src.replace("ico_", "_");
}

/*单页分页*/
function go_desc_page(page_index, page_count) {
    for (var i = 0; i < page_count; i++) {
        if (i + 1 == page_index) {
            document.getElementById("desc_page" + (i + 1)).style.display = "";
            document.getElementById("desc_page_link" + (i + 1)).className = "desc_page_link_cur";
        } else {
            document.getElementById("desc_page" + (i + 1)).style.display = "none";
            document.getElementById("desc_page_link" + (i + 1)).className = "desc_page_link";
        }
    }
}

//商品属性搜索
    function p_ser() {
        var str = "";
        var obj = document.getElementsByName("s");
        for (var i = 0; i < obj.length; i++) {
            if (obj[i].checked) {
                if (i == (obj.length - 1))
                    str += obj[i].value;
                else
                    str += obj[i].value + ",";
            }
        }
        window.location.href = "/goods.aspx?f=s&k=" + encodeURI(str);
    }

//热门搜索
function chesearch() {

    var pr_key = $("#pr_key").val();
    var cat_id = $("input[name='ddlRegType']:checked").val();

    if (pr_key == "") {
        alert("请输入查询信息");
        $("#pr_key").focus();
        return false;
    }
    else {

        if (cat_id == "0") {
            window.location.href = "/goods.aspx?f=s&k=" + encodeURI(pr_key) + "&cat_id=" + encodeURI(cat_id) + "";
        } if (cat_id == "1") {
            window.location.href = "/goods.aspx?f=s&k=" + encodeURI(pr_key) + "&cat_id=" + encodeURI(cat_id) + "";
        } if (cat_id == "2") {
            window.location.href = "/cms-search.aspx?f=s&k=" + encodeURI(pr_key) + "";
        }
        return true;
    }
}

//产品搜索
function pr_ser() {
    var pr_key = $("#pr_key").val();

    if (pr_key == "") {
        alert("请输入产品名称！")
        return false;
    }
    var url = "/product.aspx?f=s&k=";

    window.location.href = url + encodeURI(pr_key);
}
//文章搜索
function wz_ser() {
    var wz_key = $("#wz_key").val();
    if (wz_key == "") {
        alert("请输入后查询！")
        return false;
    }
    var url = "/cms/search/";

    window.location.href = url + wz_key+".aspx";
}


//停止事件冒泡
function stopPropagation(event) {
    var e = event || window.event;
    if (e.stopPropagation)
        e.stopPropagation();
    e.cancelBubble = true;
}
//停止默认事件
function preventDefault(event) {
    var e = event || window.event;
    if (e.preventDefault)
        e.preventDefault();
    e.returnValue = false;
}
//禁用右键菜单
//document.oncontextmenu = function(e) {
//    alert("鲁班坊家具 版权所有\t");
//    stopPropagation(e);
//    preventDefault(e);
//}


/* *
* 优惠券验证
*/
ValidataCard = function() {
    var card_num = $('#card_num').val();
    if (card_num == '') {
        alert('请输入隐藏码');
        $('#card_num').focus();
        return false;
    }

    var is_validata = false;
    Ajax.call('/tools/ajax.aspx?act=ggcard', 'card_num=' + card_num, function(json) {
        if (json.error > 0) {
            alert(json.message);
        }
        else {
            $('#card_num-tip').text(json.message);
            is_validata = true;
        }
    }, 'GET', 'json');

    return is_validata;
}

////添加邮件订阅
//function InsertEmail(form, formnae) {
//    $(form).ajaxSubmit({
//        beforeSubmit: function() {
//            var validator = new Validator(formnae);
//            validator.required("name", "姓名不能为空");
//            validator.isEmail("email", "邮件格式错误", true);
//            validator.required("tel", "电话不能为空");
//            //validator.required("url", "url不能为空");
//            return validator.passed();
//        },
//        dataType: 'json',
//        resetForm: true,
//        success: function(data) {
//        alert(data.message);
//        }
//    });
//    return false;
//}
$(function() {
    $("#down").click(function() {
        $('#download').modal({
            containerCss: {
                height: 100,
                padding: 0,
                width: 200
            }
        });
    });
    $("#download img").click(function() {
        $.modal.close();
    })
})

////详细抓潜预定跳转
//function addOnline() {
//    var name = $("#name").val(); if (name == "") { alert("联系人不能为空！"); return false; }
//    var email = $("#email").val(); if (email == "") { alert("邮箱不能为空！"); return false; }
//    var tel = $("#tel").val();
//    var hash = $("#hash").val(); if (email == "") { alert("手机不能为空！"); return false; }
//    var remark = $("#remark").val();
//    var city = $("#city").val();
//    $.ajax({
//        type: 'POST',
//        url: '/tools/ajax.aspx?act=InsertEmail',
//        data: { name: name, email: email, tel: tel, hash: hash, remark: remark
//        , city: city
//        },
//        success: function(data) {
//            alert("提交成功");
//            window.location = "/";
//        },
//        dataType: 'josn',
//        cache: false
//    });
//    return false;
//}
//添加邮件订阅
function InsertEmail1(form, formnae) {
    $(form).ajaxSubmit({
        beforeSubmit: function() {
            var validator = new Validator(formnae);
            validator.required("name", "联系人不能为空");
            validator.isEmail("email", "邮件格式错误", true);
            validator.required("hash", "电话不能为空");
            validator.required("mobile", "手机不能为空");
            validator.required("city", "公司地址不能为空");
            validator.required("remark", "公司名称不能为空");
            return validator.passed();
        },
        dataType: 'json',
        resetForm: true,
        success: function(data) {
            alert(data.message);
        }
    });
    return false;
}
//添加邮件订阅
function InsertEmail2(form, formnae) {
    $(form).ajaxSubmit({
        beforeSubmit: function() {
            var validator = new Validator(formnae);
            validator.required("name1", "姓名不能为空");
            validator.isEmail("email1", "邮件格式错误", true);
            validator.required("tel1", "电话不能为空");
            validator.required("city1", "城市不能为空");
            validator.required("remark1", "留言不能为空");
            return validator.passed();
        },
        dataType: 'json',
        resetForm: true,
        success: function(data) {
        alert(data.message);
        $.modal.close();
        }
    });
    return false;
}
function getPos(o) {
    var t = o.offsetTop;
    var l = o.offsetLeft;
    while (o = o.offsetParent) {
        t += o.offsetTop;
        l += o.offsetLeft;
    }
    var pos = { top: t, left: l };
    return pos;
}

function pop_close(_pop) {
    var obj = document.getElementById(_pop);

    if (obj) {
        obj.parentNode.removeChild(obj);
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
    pop.style.top = 710+'px';
    pop.style.left = 680+'px';

    obj.parentNode.appendChild(pop);

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


//详细抓潜预定跳转
function addgrab() {
    var companyname = $("#uname").val(); if (companyname == "") { alert("请输入公司名！"); return false; }
    var companyurl = $("#web").val();
    var companyaddress = $("#dizhi").val();
    var companyscale = $(":radio[@name=xiaos][@checked]").val();
    var predictcount = $("#predictcount").val(); if (predictcount == "") { alert("请输入预定数量！"); return false; }
    var monthcount = $("#monthcount").val();
    var monthcount = $("#monthlyConsumption").val(); 
    var yearcount = $("#yearcount").val();
    var askfor = $("#askfor").val();
    var photo = $("#photo").val();
    $.ajax({
        type: 'POST',
        url: '/tools/ajax.aspx?act=InsertGrab',
        data: { companyname: companyname, companyurl: companyurl, companyaddress: companyaddress, companyscale: companyscale, predictcount: predictcount
        , monthcount: monthcount, yearcount: yearcount, askfor: askfor, photo: photo
        },
        success: function(data) {
            alert("提交成功");
            window.location = "/";
        },
        dataType: 'josn',
        cache: false
    });
    return false;
}


var times = 60;
var intervalid;
var sendsms=function(id,des) {
    times = 60;
    var phone = $("#"+id).val();
    if (phone != "") {
        jQuery.post("/tools/ajax.aspx?act=sendsms", { "phone": phone, des: des }, function (data) {
            intervalid = setInterval("showTime()", 1000);
        })
    }
    else {
        alert("手机号不能为空")
    }
}

//显示倒数秒数
var showTime = function () {
    times -= 1;
    var obj = $("#send");
    $(obj).val("获取验证码(" + times + ")");
    $(obj).attr("disabled", "disabled")
    $(obj).css("color", "#666")
    if (times < 1) {
        $(obj).attr("disabled", "");
        $(obj).css("color", "");
        $(obj).val("获取验证码");
        clearInterval(intervalid);
    }
}
