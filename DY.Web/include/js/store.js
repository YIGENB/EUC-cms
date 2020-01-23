/* $Id : store.js 5052 2010-03-26 10:30:13Z gudufy $ */
var store = new Object; //购物

$(document).ready(function() {
    store.MiniCart();
});

/* *
* 显示迷你购物车
*/
store.MiniCart = function() {
    Ajax.call('/cart.aspx?act=mini_cart', '', function(data) {
        $('#mini_cart').html(data);
    }, 'GET', 'html');
}

/* *
* 更改购物车商品数量
*/
store.ChangeNumber = function(obj, act, rec_id) {
    var ipt = null;
    if (act == 'cut') {
        ipt = $(obj).next();
    } else {
        ipt = $(obj).prev();
    }

    var number = $(ipt).val();
    if (act == 'cut') {
        if (number > 1) number--;
    } else {
        if (number >= 1) number++;
    }

    $(ipt).val(number);

    Ajax.call('cart.aspx?act=change_number&is_ajax=1', 'rec_id=' + rec_id + '&number=' + number, function(json) {
        $('#content').html(json.content);
    }, 'GET', 'json');
}

/* *
* 删除购物车中的商品
*/
store.RemoveCartGoods = function(rec_id) {
    Ajax.call('cart.aspx?act=remove_cart_goods&is_ajax=1', 'rec_id=' + rec_id + '', function(json) {
        $('#content').html(json.content);
    }, 'GET', 'json');
}

/* *
* 改变配送方式
*/
store.ChangeDeliveryType = function(delivery_id) {
    Ajax.call('checkout.aspx?act=change_delivery&is_ajax=1', 'bonus=' + $('#bonus').val() + '&delivery_id=' + delivery_id, function(json) {
        $('#order-price').html(json.content);
    }, 'GET', 'json');
}

/* *
* 优惠券验证
*/
store.ValidataBonus = function() {
    var bonus_sn = $('#bonus_sn').val();
    if (bonus_sn == '') {
        alert('请输入优惠券号码');
        return false;
    }
    var is_validata = false;
    Ajax.call('checkout.aspx?act=validata_bonus&is_ajax=1', 'delivery_fee=' + $('#delivery_fee').val() + '&bonus_sn=' + bonus_sn, function(json) {
        if (json.error > 0) {
            alert(json.message);
        }
        else {
            $('#bonus-tip').text('该优惠券可以正常使用。');
            $('#order-price').html(json.content);
            is_validata = true;
        }
    }, 'GET', 'json');

    return is_validata;
}

/* *
 * 检查提交的订单表单
 */
store.checkOrderForm = function(frm) {
    var paymentSelected = false;
    var shippingSelected = false;
    var consigneeidSelected = false;
    var shippingValue = 0;
    var paymentValue = 0;

    // 检查是否选择了支付配送方式
    for (i = 0; i < frm.elements.length; i++) {
        if (frm.elements[i].name == 'shipping' && frm.elements[i].checked) {
            shippingSelected = true;
            shippingValue = frm.elements[i].value;
        }

        if (frm.elements[i].name == 'payment' && frm.elements[i].checked) {
            paymentSelected = true;
            paymentValue = frm.elements[i].value;

        }

        if (frm.elements[i].name == 'consigneeid' && frm.elements[i].checked) {
            consigneeidSelected = true;
        }
    }

//    if (frm.elements['country'] && frm.elements['country'].value == 0) {
//        alert("请选择您所在的省。");
//        return false;
//    }

//    if (frm.elements['province'] && frm.elements['province'].value == 0 && frm.elements['province'].length > 1) {
//        alert("请选择您所在的省。");
//        frm.elements['province'].focus();
//        return false;
//    }

//    if (frm.elements['city'] && frm.elements['city'].value == 0 && frm.elements['city'].length > 1) {
//        alert("请选择您所在的市。");
//        frm.elements['city'].focus();
//        return false;
//    }

//    if (frm.elements['district'] && frm.elements['district'].value == 0 && frm.elements['district'].length > 1) {
//        alert("请选择您所在的区。");
//        frm.elements['district'].focus();
//        return false;
//    }

//    if (Utils.isEmpty(frm.elements['consignee'].value)) {
//        alert("请输入收货人姓名");
//        frm.elements['consignee'].focus();
//        return false;
//    }

//    if (frm.elements['address'] && Utils.isEmpty(frm.elements['address'].value)) {
//        alert("请输入收货人详细地址");
//        frm.elements['address'].focus();
//        return false;
//    }

//    if ($('#tel1').val() == '' && $('#tel2').val() == '' && $('#tel3').val() == '') {
//        if (Utils.isEmpty(frm.elements['mobile'].value)) {
//            alert("请输入收货人的手机号码或电话号码");
//            frm.elements['mobile'].focus();
//            return false;
//        }
//        else {
//            if (!Utils.isMobile(frm.elements['mobile'].value)) {
//                alert("收货人的手机号码格式错误");
//                frm.elements['mobile'].focus();
//                return false;
//            }
//        }
//    }
//    else {
//        if ($('#tel1').val() == '') {
//            alert('请输入电话区号');
//            $('#tel1').focus();
//            return false;
//        }

//        if ($('#tel2').val() == '') {
//            alert('请输入电话号码');
//            $('#tel2').focus();
//            return false;
//        }
//        else {
//            if (!Utils.isTel($('#tel2').val())) {
//                alert('电话号码格式不正确');
//                $('#tel2').focus();
//                return false;
//            }
//        }
//    }

    if (!paymentSelected) {
        alert("您必须选定一个支付方式。");
        return false;
    }

    if (!shippingSelected) {
        alert("您必须选定一个配送方式。");
        return false;
    }

    //如果输入了优惠券号码，则验证
//    if ($('#bonus_sn').val() != '') {
//        return store.ValidataBonus();
//    }

    return true;
}

/* *
* 商品规格选择
*/
function specSelect(spec_id, val_id, goods_id) {
    Ajax.call('/goods-detail.aspx?act=get_product&is_ajax=1', 'spec_id=' + spec_id + '&val_id=' + val_id + '&goods_id=' + goods_id, function(data) {
        if (data.error > 0) {
            alert(data.message);
        }
        else {
            $('.shop-price').text('￥' + data.shop_price);
            $('.mk-price').text('￥' + data.mk_price);
            $('.js-price').text('￥' + (data.mk_price - data.shop_price).toFixed(2));
        }
    }, 'GET', 'json');

    $('.spec-item a[id*="spec-' + spec_id + '"]').each(function(i) {
        if ($(this).attr('id') == 'spec-' + spec_id + '-' + val_id) {
            $(this).addClass('selected');
        }
        else {
            $(this).removeClass('selected');
        }
    });
    var selectSpec = '';
    var selectSpecVal = '';
    var selectSpecValId = '';
    $('.spec-item a.selected').each(function() {
        selectSpec += '“' + $(this).attr('title') + '”、';
        selectSpecVal += '' + $(this).attr('title') + ',';
        selectSpecValId += '' + $(this).attr('rel') + ',';
    });
    if (selectSpec != '') {
        selectSpec = selectSpec.substring(0, selectSpec.length - 1);
        selectSpecVal = selectSpecVal.substring(0, selectSpecVal.length - 1);
        selectSpecValId = selectSpecValId.substring(0, selectSpecValId.length - 1);
    }

    $('.spec-selected-value').text(selectSpec);
    $('#goods_attr').val(selectSpecVal);
    $('#goods_attr_id').val(selectSpecValId);
}