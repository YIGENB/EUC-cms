jQuery().ajaxStart(function() {
    showLodding();
}).ajaxStop(function() {
    hideLodding();
}).ajaxError(function(a, b, e) {
    throw e;
});

//2015.03.18
$(function () {
    
    //火狐不能点击下拉与文本框
    //WinMove();
//    if (!navigator.userAgent.match(/chrome|safari/i) && navigator.userAgent.indexOf('Firefox') < 0) {
//        window.location = "/admin/homepage.htm";
//    }
    //清除缓存
    $('#ClearCache').click(function () {
        Ajax.call($(this).attr('href'), '', function (json) {
            //gritter_leftTopTip('提示信息', json.message, 3000);
            alert_toastr(json.message);
        }, 'GET', 'json');
        return false;
    });

    /**
    * 检查货号是否存在
    */
    function checkGoodsSn(goods_sn, goods_id) {
        if (goods_sn == '') {
            return;
        }

        var callback = function (res) {
            if (res.error > 0) {
                alert_toastr(res.message, "警告", 4);
            }
        }
        Ajax.call('goods.aspx?is_ajax=1&act=check_goods_sn', "goods_sn=" + goods_sn + "&goods_id=" + goods_id, callback, "GET", "JSON");
    }

    function hideLodding() {
        $('.loadding').hide();
    }

    function showLodding() {
        $('.loadding').show();
    }
});





