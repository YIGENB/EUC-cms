/*重写自定义菜单类*/
var bindCustom = function (pid) {
    var name = $.trim($(".AppId").val());
    var password = $.trim($(".AppSecret").val());
    if (name == "" || password == "") {
        //$(".alertSimpleContent").html("不能输入空！");
        //$(".alertSimple").show();
        alert("不能输入空！");
    }
    else {
        $.ajax({
            url: "weixin_set.aspx",
            type: "POST", dataType: "json",
            timeout: "10000",
            async: "true",
            data: { "act": "setAppid",
                "appid": name,
                "appsecret": password,
                "pid": pid
            }, success: function (data) {
                if (data.error == "0") {
                    $(".openCustomMenu").hide();
                    $(".openCustomMenuok").show();
                    $(".openMenu").hide();
                    $(".closeMenu").show();
                } else {
                    alert("自定义菜单开启失败！");
                }
            }
        });
    }
}
$(function () {
    $(".openMenu").click(function () {
        $(".openCustomMenu").show();
    })
    $(".CustomCloseOk").click(function () {
        $(".openCustomMenuok").hide();
        location.reload();
    })
    $(".CustomClose").click(function () {
        $(".openCustomMenu").hide();
    })
    /*点击菜单展现编辑区*/
    $(".Modify").click(function () {
        var menuid = $(this).attr("menuid");
        var menuname = $(this).attr("title");
        var trigger_word = $(this).attr("trigger_word");
        var type = $(this).attr("addtype");
        var parent = $(this).attr("parent");
        $("#menuname").val(menuname);
        $("#trigger_word").val(trigger_word);
        $("#addtype").val(type);
        $("#menuid").val(menuid);
        $("#parent_id").val(parent);
        $("#index").val($(this).index());
        $(".Initialize").hide();
        $(".FunctionCer").show();
    })
    /*删除菜单*/
    $(".CustomDelete").click(function () {
        var del = $(this).parent();
        var menuid = $(this).parent().find(".Modify").attr("menuid");
        if (confirm("确定删除吗？")) {
            $.ajax({
                url: "weixin_menu.aspx",
                type: "POST", dataType: "json",
                timeout: "10000",
                async: "true",

                data: { "act": "remove",
                    "id": menuid
                }, success: function (data) {
                    location.reload();
                }
            });
        }
    })
    /*添加*/
    $(".addMenu").click(function () {
        $("#menuname").val("");
        $("#trigger_word").val("");
        $("#addtype").val("0");
        $("#parent_id").val($(this).attr("parent"));
        $(".Initialize").hide();
        $(".FunctionCer").show();
    })
})

var SaveMenu = function () {
    var menuid = $("#menuid").val();
    var type = $("#addtype").val() == "1" ? "edit" : "add";
    var name = $("#menuname").val();
    var index = $("#index").val();
    var pid = $("#wxpid").val();
    var trigger_word = $("#trigger_word").val();
    var parent_id = $("#parent_id").val();
    if (name == "" || trigger_word == "") {
        //$(".alertSimpleContent").html("不能输入空！");
        //$(".alertSimple").show();
        alert("不能输入空！");
    }
    else {
        $.ajax({
            url: "weixin_menu.aspx",
            type: "POST", dataType: "json",
            timeout: "10000",
            async: "true",
            data: { "act": type,
                "name": name,
                "trigger_word": trigger_word,
                "parent_id": parent_id,
                "id": menuid,
                "pid": pid
            }, success: function (data) {
                if (data.error == 0) {
                    $(".CustomPop").find(".Success").html("修改成功！");
                    $(".openCustomMenu").hide();
                    $(".openCustomMenuok").show();
                    $(".openMenu").hide();
                    $(".closeMenu").show();
                    //                    alert(name);
                    //                    $(".Modify").eq(index).html(name);
                } else {
                    alert(data.content);
                }
            }
        });
    }
}