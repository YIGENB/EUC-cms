///<reference path="jquery-1.3.2-vsdoc2.js" />

//用来存储Dimention2的选中位置
var currSKUId2Array = new Array();

/// <summary>
/// 判断字符串是否为null或为empty
/// </summary>
function IsNullOrEmpty(str) {
	if (str == null || str.length == 0) {
		return true;
	}
	return false;
}

/// <summary>
/// 简单的键值对实体
/// </summary>
function KeyValueItem(key, value) {
	this.key = key;
	this.value = value;
};

/// <summary>
/// 判断数组是否包含指定的键的对象，包含返回true，反之返回false
/// </summary>
function IsContainKey(key, keyValueItemArray) {
	for (var index in keyValueItemArray) {
		if (key == keyValueItemArray[index].key) {
			return true;
		}
	}
	return false;
};

/// <summary>
/// 从一个简单的键值对实体数组中根据key获取对象
/// </summary>
function GetItemByKey(key, keyValueItemArray) {
	for (var index in keyValueItemArray) {
		if (key == keyValueItemArray[index].key) {
			return keyValueItemArray[index];
		}
	}
	return null;
};

/// <summary>
/// 修改指定键值对实体数组中指定key的值，如果不存在key，在进行Add操作
/// </summary>
function AddOrUpdateToKeyValueArray(keyValueItem, keyValueItemArray) {
	var flag = false;
	for (var index in keyValueItemArray) {
		if (keyValueItem.key == keyValueItemArray[index].key) {
			keyValueItemArray[index].value = keyValueItem.value;
			flag = true;
		}
	}

	if (!flag) {
		keyValueItemArray.push(keyValueItem);
	}
};

/// <summary>
/// 根据StyleId从指定的Style列表Json对象中获取Style实体
/// </summary>
function GetStyleEntity(styleId, styleList) {
	for (var index in styleList) {
		if (styleId == styleList[index].StyleId) {
			return styleList[index];
		}
	}
};


/// <summary>
/// 根据StyleId从指定json对象中取到SKUItem List
/// </summary>
function GetSKUItems(styleId, skuJsonList) {
	//遍历存放当前页面所有style的维度信息的json对象
	for (var index in skuJsonList) {
		//循环json中与当前styleId匹配的数据
		if (styleId == skuJsonList[index].Key) {
			return skuJsonList[index].Value;
		}
	}
	return null;
};


/// <summary>
/// 判断某SKUDimention1对应的产品在合并后的的SKUItem列表中是否有库存
/// </summary>
function IsWarehouseBySKUDimention1(sKUDimentionId1, itemList) {
	//首先循环SKUItem列表
	for (var index in itemList) {
		if (sKUDimentionId1 == itemList[index].SKUDimentionId1 && itemList[index].IsStock == true) {
			return true;
		}
	}
	return false;
};

//根据SKUDimentionId1从指定SKUItemList中获取此SKUId所匹配的所有SKU Item
//skuItemId:一个SKU Item的SKUDimentionId1
//某个styleId相对应的SKU Item的集合
function GetSKUItemListBySkuId1(skuId1, itemList) {
	var skuItemArray = new Array();
	for (var index in itemList) {
		if (skuId1 == itemList[index].SKUDimentionId1) {
			skuItemArray.push(itemList[index]);
		}
	}
	return skuItemArray;
};

/// <summary>
/// 根据指定的itemId和styleId获取SkuItem对象
/// </summary>
function GetSkuItemByItemIdAndstyleId(styleId, itemId, styleSKUItemsJson) {
	for (var index in styleSKUItemsJson) {
		if (styleId == styleSKUItemsJson[index].Key) {
			for (var j in styleSKUItemsJson[index].Value) {
				if (itemId == styleSKUItemsJson[index].Value[j].ItemId) {
					return styleSKUItemsJson[index].Value[j];
				}
			}
		}
	}
};

//将字符串转换问Date对象
function StringToDate(str) {
	return new Date(Date.parse(str.replace(/-/g, "/")));
};

//判断某元素是否存在于指定数组中，存在返回true，否则返回false
function IsInArray(item, array) {
	for (var index in array) {
		if (item == array[index]) {
			return true;
		}
	}
	return false;
};

/// <summary>
/// 根据StyleId获取相对应的库存列表信息
/// </summary>
/// <param name="jsonWH"> 库存的hashTable形式json对象 </param>
/// <param name="styleId"> styleId </param>
function GeyWarehouseByStyleId(styleId, jsonWH) {
	//遍历存放当前页面所有style的维度信息的json对象
	for (var index in jsonWH) {
		//循环json中与当前styleId匹配的数据
		if (styleId == jsonWH[index].Key) {
			return jsonWH[index].Value;
		}
	}
	return null;
};

/// <summary>
/// 根据ProductId从库存列表信息中获取库存对象
/// </summary>
/// <param name="productId"> productId </param>
/// <param name="warehouseList"> 库存对象列表的集合 </param>
function GetWarehouseByProductId(productId, warehouseList) {
	for (var index in warehouseList) {
		if (productId == warehouseList[index].ProductId) {
			return warehouseList[index];
		}
	}
	return null;
};

/// <summary>
/// 根据当前styleID获取相应天天抢信息，如果不是天天抢商品，返回null
/// </summary>
/// <param name="styleId"> styleId </param>
/// <param name="scareBuyingList"> 天天抢对象列表的集合 </param>
function GetScareBuyingByStyleId(styleId, scareBuyingList) {
	for (var index in scareBuyingList) {
		if (styleId == scareBuyingList[index].StyleId) {
			return scareBuyingList[index];
		}
	}
	return null;
};

/// <summary>
/// 初始化一级维
/// </summary>
/// <param name="scareBuyingList"> 天天抢信息的对象列表 </param>
function InitDimention1(scareBuyingList) {
	$(".choicebox").each(function() {
		// 1、获取当前需要使用的对象
		var styleId = $(this).attr("styleId");
		var styleEntity = GetStyleEntity(styleId, styleEntityListJson);
		//注：此时的sku已包含库存信息
		var skuItemList = GetSKUItems(styleId, styleSKUItemsJson);

		// 2、在界面显示当前style的Dimention1TypeName和Dimention2TypeName
		var skuDimentionTypeName1 = styleEntity.SKUDimentionTypeName1.indexOf("颜色") >= 0 ? "颜&nbsp;&nbsp;色" : styleEntity.SKUDimentionTypeName1;
		var skuDimentionTypeName2 = styleEntity.SKUDimentionTypeName2.indexOf("尺寸") >= 0 ? "尺&nbsp;&nbsp;寸" : styleEntity.SKUDimentionTypeName2;
		$(this).find(".sku-color-title").prepend(skuDimentionTypeName1 + "：");
		$(this).find(".sku-size-title").prepend(skuDimentionTypeName2 + "：");

		// 3、绘制维度1
		var noStockSku1Array = new Array();
		for (var index in skuItemList) {
			//绘制维度1
			//判断此维度1是否已输出过
			var flagDimention1 = false;
			$(this).find(".sku-color-select li").each(function() {
				var skuDimentionId1 = $(this).attr("skuId1");
				if (skuItemList[index].SKUDimentionId1 == skuDimentionId1) {
					flagDimention1 = true;
				}
			});

			//判断此SKU1所对应的所有产品是否有库存
			var isStock = IsWarehouseBySKUDimention1(skuItemList[index].SKUDimentionId1, skuItemList);

			//将缺货的skuId1记入数组
			if (styleEntity.StockAllocateMode == 1 && !isStock) {
				var noStockSku1ArrayFlag = false;
				//判断维度1唯一值是否已存在数组noStockSku1Array中
				for (var i in noStockSku1Array) {
					if (skuItemList[index].SKUDimentionId1 == noStockSku1Array[i].SKUDimentionId1) {
						noStockSku1ArrayFlag = true;
						break;
					}
				}

				if (!noStockSku1ArrayFlag) {
					noStockSku1Array.push(skuItemList[index]);
				}
			}

			//如果未输出，则绘制维度1
			if (!flagDimention1) {
				//允许超卖或有库存才可输出
				if (styleEntity.StockAllocateMode == 0 || isStock == true) {
					PaintDimention1(skuItemList[index], $(this));
					// 3、绑定单击事件
					BindDimention1Click(skuItemList[index], styleEntity.StockAllocateMode, skuItemList, styleEntity, $(this), $(this).find(".sku-color-select li[skuid1=" + skuItemList[index].SKUDimentionId1 + "]"));
				}
			}
		}

		// 4、根据sku-color-select元素的数量，决定是否要显示维一的内容
		if ($(this).find(".sku-color-select li").size() == 1) {
			//维1元素仅1个，不显示维度1区域
			if ($(this).find(".sku-color-select li a").eq(0).attr("alt") == "单色") {
				$(this).find(".sku-color-title").hide();
				$(this).find(".sku-color-select").hide();
			}
		}

		// 5、默认选择第一个，并触发第一个元素的click事件
		$(this).find(".sku-color-select li:nth-child(1)").trigger("click");

		// 6、判断维度1的数量和纬度2的数量，若数量都为0，说明该商品不允许超买且没有库存，则隐藏购买区域，显示无库存提示信息
		if ($(this).find(".sku-color-select li").size() == 0 && $(this).find(".sku-size-select li").size() == 0) {
			$(this).html("您查看的商品已经售完,感谢您的关注");
			$(this).attr("class", "error");
			return;
		}

		//7、构建已售完的颜色区域
		if (noStockSku1Array.length > 0) {
			$(this).find(".sku-color-select").after("<div class='colornone mb10 clearfix'><span class='saleoutfont mr5'>售完：</span></div>");
			for (var i in noStockSku1Array) {
				$(this).find(".colornone").append("<img alt='" + noStockSku1Array[i].SKUDimentionName1
					+ "' title='很抱歉，" + noStockSku1Array[i].SKUDimentionName1 + " 颜色已售完。&#10;您可以选择其它颜色。"
					+ "' src='" + noStockSku1Array[i].SKUColorImageFileName + "' />");
			}
		}

		// 8、为维度1色块加上浮动事件
		BindSku1Event(styleId);
	});
};

/// <summary>
/// 绑定维度1的事件
/// </summary>
function BindDimention1Click(skuItem, stockAllocateMode, itemList, styleEntity, parentObj, objDom) {
	objDom.click(function() {
		//1、更改选中状态
		//更改维1当前li元素的选中状态
		parentObj.find(".sku-color-select li[class=cur]").attr("class", "");
		parentObj.find(".sku-color-select li[skuId1=" + skuItem.SKUDimentionId1 + "]").attr("class", "cur");

		// 2、更改维度1所关联的界面文字
		var currStyleId = styleEntity.StyleId;
		if (currStyleId.indexOf("-") >= 0) {
			var index = currStyleId.indexOf("-");
			currStyleId = styleEntity.StyleId.substring(0, index);
		}
		parentObj.find(".sku-color-title .hl2").text(currStyleId + skuItem.SKUDimentionId1 + "-" + skuItem.SKUDimentionName1);

		//3、按照逻辑绘制维度2的Html
		//获得当前skuDimentionId1所关联的所有SKU Item
		var skuItemList = GetSKUItemListBySkuId1(skuItem.SKUDimentionId1, itemList);
		//绘制维度2，需要先清空目前维度2的内容
		parentObj.find(".sku-size-select").html("");
		for (var index in skuItemList) {
			//根据逻辑判断来绘制SKUDomimention2的Html
			//允许超卖或有库存才绘制维度2
			if (stockAllocateMode == 0 || skuItemList[index].IsStock) {//有库存
				//绘制维度2
				PaintDimention2(true, skuItemList[index], parentObj);
				//4、为维度2绑定事件
				BindDimention2Click(skuItemList[index], styleEntity, parentObj, parentObj.find(".sku-size-select li[skuid2=" + skuItemList[index].SKUDimentionId2 + "]"));
			}

			//不允许超卖且无库存的判断
			if (stockAllocateMode == 1 && !skuItemList[index].IsStock) {
				//绘制维度2
				PaintDimention2(false, skuItemList[index], parentObj);
				//4、为维度2绑定事件
				BindDimention2Click(skuItemList[index], styleEntity, parentObj, parentObj.find(".sku-size-select li[skuid2=" + skuItemList[index].SKUDimentionId2 + "]"));
			}
		}

		// 5、为维度2排序
		parentObj.find(".sku-size-select a[class=none]").parent("li").insertAfter(parentObj.find(".sku-size-select a[class=]").parent("li:last"));

		// 6、触发选中元素的click事件，
		if (IsContainKey(styleEntity.StyleId, currSKUId2Array)) {
			//如果上次有选择
			var currSKUId2 = GetItemByKey(styleEntity.StyleId, currSKUId2Array).value;
			//上次选择的还在且可以被选中就触发它
			if (parentObj.find(".sku-size-select li[skuId2=" + currSKUId2 + "]").size() > 0 && parentObj.find(".sku-size-select li[skuId2=" + currSKUId2 + "] a").attr("class") != "none") {
				parentObj.find(".sku-size-select li[skuId2=" + currSKUId2 + "]").trigger("click");
			}
			else {//上次选择的在这次没了，也选择第一个
				parentObj.find(".sku-size-select li:nth-child(1)").trigger("click");
			}
		}
		else {
			parentObj.find(".sku-size-select li:nth-child(1)").trigger("click");
		}

		// 7、如果维度1仅有1个，维度2也仅有1个，那么维度1、维度2和已选择区域都不显示

		if (parentObj.find(".sku-color-select li").size() == 1 && parentObj.find(".sku-size-select li").size() == 1) {

			if (parentObj.find(".sku-color-select li a").eq(0).attr("alt") == 'null' || parentObj.find(".sku-color-select li a").eq(0).attr("alt") == "单色") {
				parentObj.find(".sku-color-title").hide();
				parentObj.find(".sku-color-select").hide();
			}

			//若唯一一个维度2的值是null，则不显示维度2区域和已选择区域
			if (parentObj.find(".sku-size-select li").eq(0).text() == 'null选中') {
				parentObj.find(".sku-size-title").hide();
				parentObj.find(".sku-size-select").hide();
				parentObj.find(".sku-selected-title").hide();
			}
		}

		// 8、添加查看我的尺码功能

		try {
			sizeLink();
		}
		catch (e) {

		}

		var classId1 = styleEntity.ClassId3.substring(0, 2);
		var classId2 = styleEntity.ClassId3.substring(0, 4);
		var classId3 = styleEntity.ClassId3;

		//9、当一级分类为：FY、N4、N2或二级分类为N502的时候， 如果二维（即尺寸维）只有一个001时，不显示二维度，包括“尺寸”两个字。整个二维不显示。
		if (parentObj.find(".sku-size-select li").size() == 1
				&& (classId1 == "FY" || classId1 == "N4" || classId1 == "N2" || classId2 == "N502")
				&& parentObj.find(".sku-size-select a").eq(0).text() == "001") {

			parentObj.find(".sku-size-title").hide();
			parentObj.find(".sku-size-select").hide();
		}

		

		//11、去掉链接效果
		return false;
	});
};

/// <summary>
/// 绑定维度2的事件
/// </summary>
function BindDimention2Click(skuItem, styleEntity, parentObj, objDom) {
	objDom.click(function() {
		if (objDom.find("a").attr("class") == "none") {
			return false;
		}

		//设置当前选中的Dimention2
		var currSKUId2 = new KeyValueItem();
		currSKUId2.key = styleEntity.StyleId;
		currSKUId2.value = skuItem.SKUDimentionId2;
		AddOrUpdateToKeyValueArray(currSKUId2, currSKUId2Array);

		//1. 更改维2当前li元素的选中状态
		parentObj.find(".sku-size-select li[class=cur]").attr("class", "");
		parentObj.find(".sku-size-select li[skuId2=" + skuItem.SKUDimentionId2 + "]").attr("class", "cur");

		//2. 更改维度2所关联的界面文字
		parentObj.find(".sku-size-title .hl2").text(skuItem.SKUDimentionName2);

		//3. 已选择文字
		var skuName2 = skuItem.SKUDimentionName2 == "001" ? "" : "\"" + skuItem.SKUDimentionName2 + "\"";
		var text = "\"" + skuItem.SKUDimentionName1 + "\" <span id='sku-select-size'>" + skuName2 + "</span>";

		parentObj.find(".sku-selected-title .hl2").html(text);

		//4. 库存状态文字
		parentObj.find(".sku-notify").text(skuItem.WHShowMsg);

		//5. 为加入购物车的按钮<a>标签加入自定义属性itemId
		parentObj.find(".sku-addcart").attr("itemId", skuItem.ItemId);

		//6、去掉链接效果
		return false;
	});
};


/// <summary>
/// 为维度1的a标签绑定click事件和mousemove事件
/// </summary>
function BindSku1Event(styleId) {
	$(".choicebox[styleid=" + styleId + "] .sku-color-select a").bind("mousemove", function() {
		//更换大图
		var imgSrc = $(this).find("img").eq(0).attr("src");
		if (imgSrc.indexOf("COLOR") > -1) {
			var newPic = imgSrc.replace(/COLOR/, "LARGE");
			$(".bigpic&.fl img").eq(0).attr("src", newPic);
		}
	});
	$(".choicebox[styleid=" + styleId + "] .sku-color-select a").bind("mousemove", function() {
		var imgSrc = $(this).find("img").eq(0).attr("src");
		if (imgSrc.indexOf("COLOR") > -1) {
			var newPic = imgSrc.replace(/COLOR/, "SMALL").replace(/B/, "S");
			$(".itemlist99&.clearfix img[styleId=" + styleId + "]").eq(0).attr("src", newPic);
			//套组页使用
			//alert($(".showchoice .p-layout img[styleId=" + styleId + "]").size());
			$(".showchoice .p-layout img[styleId=" + styleId + "]").eq(0).attr("src", newPic);
		}
	});

};

/// <summary>
/// 绘制维度1中的1个色块
/// </summary>
/// <param name="skuItem"> 一个skuItem对象 </param>
///	<param name="parentObj"> 当前总结点元素的jquery包装集对象 </param>
function PaintDimention1(skuItem, parentObj) {
	//找到ul元素向其插入li元素
	parentObj.find(".sku-color-select").append("<li skuId1='" + skuItem.SKUDimentionId1 + "'></li>");
	//向li节点中添加<a>节点
	parentObj.find(".sku-color-select li[skuId1=" + skuItem.SKUDimentionId1 + "]").append("<a href='#' title='" + skuItem.SKUDimentionName1 + "' alt='" + skuItem.SKUDimentionName1 + "'></a>");
	//向<a>节点中添加<img>节点
	parentObj.find(".sku-color-select li[skuId1=" + skuItem.SKUDimentionId1 + "] a").append("<img src='" + skuItem.SKUColorImageFileName + "' alt='" + skuItem.SKUDimentionName1 + "' />");
	//向li节点中添加<span>节点
	parentObj.find(".sku-color-select li[skuId1=" + skuItem.SKUDimentionId1 + "]").append("<span>选中</span>");
};

/// <summary>
/// 绘制维度1中的1个色块
/// </summary>
/// <param name="noneflag"> 一个标识位，true标识允许超买，维度2正常显示；false标识不允许超买，维度2做灰色处理 </param>
/// <param name="skuItem"> 一个skuItem对象 </param>
///	<param name="parentObj"> 当前总结点元素的jquery包装集对象 </param>
function PaintDimention2(noneflag, skuItem, parentObj) {
	//向ul节点中添加<li>节点
	parentObj.find(".sku-size-select").append("<li skuId2='" + skuItem.SKUDimentionId2 + "'></li>");
	//向li节点中添加<a>节点
	if (noneflag) {
		parentObj.find(".sku-size-select li[skuId2=" + skuItem.SKUDimentionId2 + "]").append("<a href='#' title='" + skuItem.SKUDimentionName2 + "' alt='" + skuItem.SKUDimentionName2 + "'>" + skuItem.SKUDimentionName2 + "</a>");
	}
	else {
		parentObj.find(".sku-size-select li[skuId2=" + skuItem.SKUDimentionId2 + "]").append("<a class='none' href='#' title='尺寸：" + skuItem.SKUDimentionName2 + " 已售完' alt='尺寸：" + skuItem.SKUDimentionName2 + " 已售完'>" + skuItem.SKUDimentionName2 + "</a>");
	}
	//向li节点中添加<span>节点
	parentObj.find(".sku-size-select li[skuId2=" + skuItem.SKUDimentionId2 + "]").append("<span>选中</span>");
};


///	<summary>
///	将库存信息合并到SKU对象中
///	<summary>
function CombSKUAndWH(styleEntityList, styleSKUItemsJson, productWarehouseJson) {
	//三层循环
	for (var i in styleEntityList) {
		//循环StyleEntity
		var skuItemList = GetSKUItems(styleEntityList[i].StyleId, styleSKUItemsJson);
		var stockAllocateMode = styleEntityList[i].StockAllocateMode;

		for (var j in skuItemList) {
			//循环SKU
			var whItem = GetWarehouseByProductId(skuItemList[j].ProductId, productWarehouseJson);

			if (whItem == undefined || whItem == null) {//用来容错
				skuItemList[j].IsStock = false;
				skuItemList[j].QtyAvailable = 0;
				skuItemList[j].WHShowMsg = "热销商品预计一周到货";
			}
			else {
				//为SkuItem添加库存信息
				skuItemList[j].IsStock = whItem.IsStock;
				skuItemList[j].QtyAvailable = whItem.QtyAvailable;
				skuItemList[j].WHShowMsg = GenerateWarehourseMsg(stockAllocateMode, whItem.IsStock);
			}
		}
	}
};

function GenerateWarehourseMsg(stockAllocateMode, isStock) {
	if (isStock) {
		return "库存有货，立即发出";
	}

	if (stockAllocateMode == 0) {
		//无库存压力，允许超卖
		return "热销商品预计一周到货";
	}

	return "";
}

//此jQuery静态方法用于常用缓存的处理
function jQueryCache(cID, key, value) {
	var contain = $("<div id='" + cID + "' style=\"display:none;\"></div>");
	$("body").append(contain);
	$("#" + cID).data(key, value);
};

