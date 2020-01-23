
var number_err = "请输入购买商品的数量";

function AddToCart(numberTag, goodId) {
    var numberValue = $("#"+numberTag).val();
    var msg="";
    if (numberValue == "") { msg += number_err; }

    
    if (!$("#cc span").hasClass("select")) {
        msg = "请选择购买商品的尺寸";
    }
    if (!$("#xh span").hasClass("select")) {
        msg = "请选择购买商品的型号";
    }

    if (parseInt(numberValue) < parseInt($("#goods_number").val())) {
        msg = "您购买了" + numberValue + "件，购买数量不能少于起订量" + $("#goods_number").val() + "件";
    }
    
    if (msg == "") {
        var gg = $("#msg1").text() + "," + $("#msg2").text();
        location.href = "/cart.aspx?act=add_to_cart&goods_id=" + goodId + "&qty=" + numberValue + "&goods_attr=" + gg;
    }
    else {
        alert(msg);
    }
}

//$(document).ready(function() {
//Ajax.call('/tools/ajax.aspx', { act: 'get_cart'}, function(data) { alert(data.message);}, 'POST', 'json');
//});