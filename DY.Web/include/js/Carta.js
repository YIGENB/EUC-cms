/*
** 用户不需要登录可以购买商品
** 在用户将商品增加到购物车的时候不做登录验证
** 会员级别D:为多钻石会员，G：为金钻会员
*/
//绑定页面的事件
$(document).ready(function() {
	$(".addcart").bind("click", function() {
		var mySKUDiv = $(this).parents("div[styleId]");
		try {
			var myCart2 = new Cart(mySKUDiv.attr("styleId"), mySKUDiv.attr("maxQty"), mySKUDiv.attr("isVip"), mySKUDiv.attr("memberLevel"), mySKUDiv.attr("isScare"));
		} catch (e) {
		}
	});
})
/*
**单品页前台购物车JS类
**使用方法：var car = new Cart(styleId,maxQty,isVip,memberLevel,isScare);
**/
var Cart = function(styleID, maxQty, isVip, memberLevel, isScare) {
	return Cart.func.init(styleID, maxQty, isVip, memberLevel, isScare);
};

//方法的入口
Cart.func = Cart.prototype = {
    init: function(styleID, maxQty, isVip, memberLevel, isScare) {
        this.styleID = styleID;
        this.maxQty = maxQty;
        this.isVip = isVip;
        this.memberLevel = memberLevel;
        this.isScare = isScare;
        this.realMemberLevel = -1;
        this.AddToCart();
    },
    AddToCart: function() {
        var myObject = this;
        if (this.ValidatePost()) {
            var price = $("#stylePrice").text();
            var point = $("#stylePricePoints").text();
            //数据正确性验证,如果通过验证了，则检查是否需要登录，
            //如果是天天抢和会员专属就需要登录后才可以执行相关的操作
            if (parseInt(this.isScare) == 1) {
                MiniLogin.Action(myObject, "", "<li>抢购商品要登录后才能购买</li>");
            }
            else if (parseInt(this.isVip) == 1) { //1.表示会员商品
                MiniLogin.Action(myObject, "", "<li>商品为金钻卡会员独享，请先登录！</li>");
            }
            //如果是积份购买的商品，在价格< 10 积分 > 0 的时候需要有户登录
            else if (point != "") {
                if (parseFloat(price) < 10) {
                    MiniLogin.Action(myObject, "", "");
                }
                else {
                    this.CreateUrl();
                }
            }
            else {
                this.CreateUrl();
            }
        }
    },
    //创建一个发送到购物车的链接
    CreateUrl: function(opt) {
        if (opt) {
            this.styleID = opt;
        }
        var obj = $("div[styleId='" + this.styleID + "']");
        var itemID = obj.find(".sku-addcart").attr("itemId"); //取出用户选择的itemID
        var chooseCount = obj.find("input:first").val(); //取出购买数量
        var goodsID = obj.find(".sku-addcart").attr("rel"); //取出商品ID
        var url = "/cart.aspx?act=add_to_cart&goods_id=" + goodsID + "&itemid=" + itemID + "&qty=" + chooseCount + "";
        location.href = url;
    },
    //在提交到服务器前进行相关的验证操作
    ValidatePost: function() {
        var obj = $("div[styleId='" + this.styleID + "']");
        var itemID = obj.find(".sku-addcart").attr("itemId"); //取出用户选择的itemID
        if (itemID == undefined) {//如果不存在此属性说明用户没有进行选择，真接返回
            return false;
        }
        if (parseInt(this.isScare) == 0) { //非天天抢验证
            //第一步检查用户是否选择了颜色
            if (obj.find(".sku-color-select li[class=cur]").size() <= 0) {
                this.ShowForm({ msg: "请您选择商品颜色或尺寸！" });
                return false;
            }
            //第二步用户选择的尺寸检查
            if (obj.find(".sku-size-select li[class=cur]").size() <= 0) {
                this.ShowForm({ msg: "请您选择商品颜色或尺寸！" });
                return false;
            }
        }
        //第三步检查用户输入的是否是数字
        var chooseCount = obj.find("input:first").val();
        if (isNaN(chooseCount)) {
            this.ShowForm({ msg: "您输入的件数不正确，请重新输入！" });
            return false;
        }
        //第四步检查是否有库存压力，如果有则不进行出售
        var opt = {};
        var myStyleID = this.styleID;
        //第五步单笔最大数量，为0 或者是NULL 的时候表示不限制
        //如果不限制


        //有库存或充许超卖
        if (parseInt(this.isScare) == 0) { //不是天天抢的库存检查操作
            if (parseInt(chooseCount) > 50) {
                this.ShowForm({ msg: "您购买了" + chooseCount + "件的本商品，已超出允许的最大数量50件!" });
                return false;

            }
            if (this.maxQty) {
                if (this.maxQty > 0) { //单笔最大订购数量
                    if (parseInt(chooseCount) > parseInt(this.maxQty)) {
                        this.ShowForm({ msg: "您一次最多只能购买" + this.maxQty + "件，请重新输入！" });
                        return false;
                    }
                }
            }
        }




        //第六步是否为天天抢，如果是刚检查开始时间，和结束时间
        var scareBuyingItem = $("#divCache").data("scareBuyingList"); //取出天天抢
        if (parseInt(this.isScare) == 1) {
            if (scareBuyingItem != undefined || scareBuyingItem != null) { //判断是否是天天抢
                //是天天抢
                if ($.trim(scareBuyingItem.ErrorMsg) == "1") {
                    this.ShowForm({ msg: "对不起，天天抢活动尚未开始或已结束" });
                    return false;
                }
                if ($.trim(scareBuyingItem.ErrorMsg) == "2") {
                    this.ShowForm({ msg: "对不起，商品已经被其他人抢购完了" });
                    return false;
                }
                //				else {
                //					if (parseInt(chooseCount) > scareBuyingItem.MaxQtyPer) {
                //						this.ShowForm({ msg: "您一次最多只能购买" + scareBuyingItem.MaxQtyPer + "件，请重新输入！" });
                //						return false;
                //					}
                //				}
            }
        }
        return true;
    },
    //提示信息
    ShowForm: function(opt) {
        if (opt.mStyleID) {
            this.styleID = opt.mStyleID;
        }
        var msg = $("div[styleId='" + this.styleID + "']").find(" .sku-error-message");
        msg.html(opt.msg).show();
        setTimeout(function() {
            msg.hide();
        }, 3000);
    },
    //关闭提示信息
    CloseFrom: function() {
        $("div[styleId='" + this.styleID + "']").find(".sku-error-message").html("").hide();
    }
    ,
    //方法的回调
    Callback: function() {
        var styleID = this.styleID;
        var memberShipId = $("div").data("blah"); //如果没有取到就到后台去取用户的级别GetMemberShipId
        var showMessage = this.ShowForm;
        var careateUrl = this.CreateUrl;
        //如果是天天抢在用户登录以后就转到购物车
        //		if (parseInt(this.isScare) == 1) {
        //			//alert("天天抢增加到购物车！");
        //			this.CreateUrl();
        //		}
        //var currentMemberLevel = this.memberLevel; //保存当前系统要的会员级别
        //如是会员专属就在用户登录以后检查会员的级别
        if (parseInt(this.isVip) == 1) { //1.表示会员商品
            if (memberShipId == undefined) {
                $.ContentLoader(ajaxServiceAddress.MemberShip, "{}",
                function(data) {
                    var result = eval('(' + data + ')');
                    var myJson = result.d;
                    if (myJson == "G" || myJson == "D") {
                        careateUrl(styleID);
                    }
                    else {
                        showMessage({ msg: "感谢您对麦网活动的支持，产品仅限金钻卡用户购买！", mStyleID: styleID });
                    }
                },
                 function() {
                     showMessage({ msg: "服务器超时！", mStyleID: styleID });
                 }
                 );
            }
            else {
                if (memberShipId == "G" || memberShipId == "D") { //会员级别的检查，如果检查通过了，就进行增加到购物车的操作
                    careateUrl(styleID);
                }
                else {
                    showMessage({ msg: "感谢您对麦网活动的支持，产品仅限金钻卡用户购买！", mStyleID: styleID });
                }
            }
        }
        else {
            //其它要等录的也直接去购物车
            this.CreateUrl();
        }

    }
};



//----------------------------------Static Method-----------------------
$.ContentLoader = function(url, data, callback, error) {
	$.ajax({
		url: url,
		data: data,
		type: "POST",
		contentType: "application/json; charset=utf-8",
		success: callback,
		error: error
	});
};