///<reference path="lib\jquery-1.3.2-vsdoc2.js" />
///<reference path="lib\jquery.cookie.js" />
///<reference path="StyleCommon.js" />
var loadCount = 0;
function LoadSKU() {

	//若单卖文字不存在，则不执行单卖逻辑判断（放到styleCommon）
	if (typeof (aloneMessage) != undefined && aloneMessage != null && aloneMessage.length > 0) {
		//是单卖产品，执行返回操作
		$(".choicebox").html(aloneMessage);
		$(".choicebox").attr("class", "error");
		return;
	}
	else {
		try {
			//1、库存处理
			CombSKUAndWH(styleEntityListJson, styleSKUItemsJson, productWarehouseJson);

			//2、天天抢逻辑判断
			if (scareBuyingItem != undefined || scareBuyingItem != null) { //判断是否是天天抢
				//是天天抢
				if (scareBuyingItem.ErrorMsg != null && scareBuyingItem.ErrorMsg.length > 0 && scareBuyingItem.ErrorMsg != "0") {
					var errorMsg = "";
					if (scareBuyingItem.ErrorMsg == "1") {
						errorMsg = "对不起，天天抢活动尚未开始或已结束";
					}

					if (scareBuyingItem.ErrorMsg == "2") {
						errorMsg = "对不起，商品已经被其他人抢购完了";
					}

					$("div.choicebox").html(errorMsg);
					$("div.choicebox").attr("class", "error");
					return;
				}
			}

			//初始化Dimention1
			InitDimention1();
			//5、特殊分类商品逻辑
			SpecialClassLogic();
		} catch (e) {
			loadCount++;
			if (loadCount <= 50)
				setTimeout("LoadSKU()", 100);
		}
	} 
}


$(document).ready(function() {
	$(".thumbpic li a").eq(0).attr("class", "cur");
	//1、为Largemap图片绑定over事件和Click事件
	BindPicEvent();
});

/// <summary>
/// 根据number获取
/// </summary>
///	<param name="number" type="int">
///		每个Matching所对应的编号
///	</param>
///	<param name="matchingJson" type="Element">
///		Mathcing所对应的复杂Json对象，具体请在StyleDetail.aspx查看属性MatchingJson
///	</param>
function GetMatchingByNumber(number, matchingList) {
	for (var i in matchingList) {
		if (number == matchingList[i].Number) {
			return matchingList[i]; 
		}
	}
	return null;
}

/// <summary>
/// 为LargMap图片加上mousemove事件，并去掉Click的链接功能
/// </summary>
function BindPicEvent() {
	$(".thumbpic li a").bind("mousemove", function() {
		//选中标识
		$(".thumbpic li a[class=cur]").attr("class", "");
		$(this).attr("class", "cur");

		var newPic = $(this).attr("href");
		$(".bigpic&.fl img").eq(0).attr("src", newPic);
	});

	$(".thumbpic li a").bind("click", function() {
		return false;
	});
};

function BindClearCookieButton() {
	$("#viewed .handle").bind("click", function() {
		$.cookie("LastView", "", { expires: -1, path: "/", domain: "m18.com", secure: true });
		$("#viewed ul").eq(0).hide();
		return false;
	});
};

function SpecialClassLogic() {
	//获取当前单品页的各级ClassId
	var classId3 = styleEntityListJson[0].ClassId3;
	var classId1 = classId3.substring(0, 2);
	var classId2 = classId3.substring(0, 4);

	var parentObj = $("div.choicebox");

	//1、 当除指定classId外，如果二维显示是“均码”，则不显示维度2，并已选择中不显示“均码”(详见bug 691)
	if (parentObj.find(".sku-size-select li").size() == 1) {

		if ((classId3 != "N30101" && classId3 != "N30102" && classId3 != "N30103" && classId3 != "N30104"
				&& classId3 != "N20201" && classId3 != "N20202" && classId3 != "N20203" && classId3 != "N20205"
				&& classId2 != "N703" && classId2 != "N704")) {

		}
		else {
			//1、隐藏维度2区域
			parentObj.find(".sku-size-title").hide();
			parentObj.find(".sku-size-select").hide();
			//2、隐藏已选择中的尺寸
			parentObj.find("span#sku-select-size").hide();
			//3、未当前维度1、维度2的所有按钮按钮绑定click事件
			parentObj.find(".sku-color-select li").bind("click", function() {
				parentObj.find("span#sku-select-size").hide();
			});
			parentObj.find(".sku-size-select li").bind("click", function() {
				parentObj.find("span#sku-select-size").hide();
			});
		}
	}
}

