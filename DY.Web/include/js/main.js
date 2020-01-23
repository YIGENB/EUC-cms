function initLoadMore(a) { 
    var b = 1,
    c = {
        debug: !1,
        url: "",
        targetSelector: "",
        loadmoreSelector: ".j-loadmore",
        tpl: "",
        data: {
            p: b
        },
        callback: null
    },
    d = $.extend(!0, {},c, a)
   
    $(window).scroll(function() {
        var a = $(window).scrollTop() >= $(document).height() - $(window).height() ? !0 : !1;
        if (a) {
            var c = $(d.loadmoreSelector);
            return d.data.p = b = parseInt(b) + 1,           
            $.ajax({
                url: d.url,
                type: "post",
                dataType: "json",
                data: d.data,
                beforeSend: function() {
                    c.find(".loadmore-icon").css("display", "inline-block")
                },
                success: function(a) {       
                    if (d.debug && console.log(a), a.length) {                        
                        var b = _.template(d.tpl, {
                            dataset: a
                        }),                                   
                        e = $(b);                          
                        $(d.targetSelector).append(e);                        
                    } else c.siblings(".j-noMoreData").css("display", "block"),
                    c.hide();
                    c.find(".loadmore-icon").hide(),
                    d.callback && d.callback(e)
                }
            }),
            !1
        }
    })
}
$(function() {
    $(document).on("touchend", ".j-showNavSub dt",
    function() {
        $(this).siblings(".nav-item-sub").toggle(),
        $(this).parents(".nav-item").siblings(".nav-item").find(".nav-item-sub").hide()
    }),
    $(document).on("touchmove", document,
    function() {
        $(".nav-item-sub").hide()
    });
    var a = $("#order_type").val();
    a || $(".morder_con").eq(0).show(),
    $(".morder_nav section").click(function() {
     
        var a = $(".morder_nav section").index(this);
        $(this).addClass("cur").siblings().removeClass("cur"),
        $(".morder_con").eq(a).show().siblings(".morder_con").hide()
    }),
    $(".coupons_con ul li").eq(0).show(),
    $(".coupons_nav section").click(function() {      
        var a = $(".coupons_nav section").index(this);
        $(this).addClass("cur").siblings().removeClass("cur"),
        $(".coupons_con ul li").eq(a).show().siblings().hide()
    }),
    $.aLert = function(a) {
        var b = {
            title: "是否继续",
            callback: null,
            clickOK: null,
            clickCancel: null
        },
        c = $.extend(!0, {},
        b, a),
        d = "<div class='J-hyd'><div class='title'>" + c.title + "</div><div class='button'><button type='button' class='J-okClk'>确定</button><button type='button' class='butcall J-noClk' >取消</button></div></div>";
        $("body").append(d);
        var e = $(".J-hyd").height(),
        f = $(window).height(),
        g = (f - e) / 2,
        h = ($(window).width() - 260) / 2;
        $(".J-hyd").css({
            top: g,
            left: h
        }),
        $("body").append('<div class="J-back"></div>').css({
            height: "100%",
            overflow: "hidden"
        }),
        $(".J-back").css("height", f);
        var i = function() {
            $(".J-hyd").hide(),
            $(".J-back").hide(),
            $("body").css({
                height: "auto",
                overflow: "auto"
            })
        };
        $(document).on("click", ".J-okClk",
        function() {
            c.callback && c.callback(),
            c.clickOK && c.clickOK(),
            i()
        }),
        $(document).on("click", ".J-noClk",
        function() {
            c.callback && c.callback(),
            c.clickCancel && c.clickCancel(),
            i()
        })
    },
    $.Error = function(a, b) {
        var b = b ? b: 1500,
        c = '<section class="Errormes">' + a + "</section>";
        $(".Errormes").length < 1 && $("body").append(c);
        var d = $(".Errormes").width(),
        e = $(".Errormes").height(),
        f = ($(window).width() - d) / 2,
        g = ($(window).height() - e) / 2;
        $(".Errormes").css({
            left: f,
            top: g
        }),
        $(".Errormes").animate({
            opacity: "1"
        },
        100,
        function() {
            var a = $(this);
            setTimeout(function() {
                a.fadeOut(300,
                function() {
                    a.remove()
                })
            },
            b)
        })
    },
    $.addone = function(a, b) {
        var c = $("#J-addone"),
        d = $("#J-addone-cartnum").text();
        c.text("+" + a).show().delay(400).fadeOut(b),
        $("#J-addone-cartnum").text(parseInt(d) + parseInt(a))
    }
});