$(function () {


    /*服务浮动*/

    $('.ser_float li').hover(function () {

        var ser_more = $(this).attr('data-more')
        $(this).find('a').html(ser_more)
        $(this).find('a').stop().animate({ "width": "130px" }, 450)

    }, function () {
        var ser_less = $(this).attr('data-less')
        $(this).find('a').html(ser_less)
        $(this).find('a').stop().animate({ "width": "68px" }, 450)

    })

    $(window).scroll(function () {

        var scrll_top = $(this).scrollTop();
        $('.ser_float').stop().animate({ "top": scrll_top + 250 + "px" })

    })

    /*关于我们选项卡*/
    var $about_this = $(".about_hisSel ul li")
    $about_this.click(function () {
        $(this).addClass('ab_cur').siblings().removeClass('ab_cur');
        var about_index = $about_this.index(this);     /*创建当前点击li所对应的li元素的索引*/
        $(".about_hisCon>div").eq(about_index).show().siblings().hide();    /*当前li点击对应索引显示同辈元素隐藏*/
    })


    $('.blog_list li').each(function () {

        $(this).find('.blog_share a:last').css("background", "none")

    })

    /*联系我们*/
    $('.contact_click').click(function () {

        $('.contact_show').show();
        $('.contact_show').find("iframe").attr("src", $('.contact_show').find("iframe").attr("src"));

    })

    $('.contact_shclose').click(function () {

        $('.contact_show').hide()

    })

    /*优势*/

    $('.advantage_list li:eq(4),.advantage_list li:eq(5),.advantage_list li:eq(6),.advantage_list li:eq(7)').addClass('on')

    $('.advantage_list li').hover(function () {

        $(this).find('.advan_show').stop().animate({ "top": "0" }, 350)

    }, function () {

        $(this).find('.advan_show').stop().animate({ "top": "150px" }, 350)

    })


    $('.advan_close').click(function () {

        $('.advantage_show').hide();

    })

    /*team*/
    $('.replay_team li').hover(function () {

        $(this).find('img').css("opacity", "1")
        $(this).find('.replay_Timg').animate({ "opacity": "0" }, 550)
        $(this).find('.replay_Simg').animate({ "opacity": "1" }, 550)

    }, function () {

        $(this).find('img').css("opacity", "0.6")
        $(this).find('.replay_Timg').animate({ "opacity": "1" }, 550)
        $(this).find('.replay_Simg').animate({ "opacity": "0" }, 550)

    })

    /*replay*/
    /*		$('.replay_img area').click(function(){
			
    $('.advantage_show').show()
			
    })			
    */
    /*custorm*/
    $(".service .container").slide({
        effect: "leftLoop",
        autoPlay: true,
        delayTime: 1000,
        interTime: 4000,
        mainCell: ".service_content ul",
        prevCell: ".service_prev",
        nextCell: ".service_next",
        trigger: "click"
    });

})