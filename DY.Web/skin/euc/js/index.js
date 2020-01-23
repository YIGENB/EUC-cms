$(function(){
	
	/*首页Business*/
	function isScrolledIntoView(elem) {
        var docViewTop = $(window).scrollTop();
        var docViewBottom = docViewTop + $(window).height();
        var elemTop = $(elem).offset().top;
        if (elemTop + 50 < docViewBottom) {
            return true
        } else {
            return false
        }
    }

    function animateTop(item, time,callback) {
        if ($(item).attr('init') == 'false'&& isScrolledIntoView($(item).parent()) ){
            $(item).attr('init', 'true');
            setTimeout(function(){
                $(item).animate({'bottom': '0'}, 800, 'easeOutCubic');
                callback;
            },time)
        }
    }

    function ftAnimate(item, time) {
        if ($(item).attr('init') == 'false'&& isScrolledIntoView($(item))) {
            $(item).attr('init', 'true');
            setTimeout(function(){
                $(item).animate({'bottom': '0'}, 800, null);
            },time)
        }
    }

    function animateBot(item, time, callback) {
        $(item).delay(time).animate({'top': '650px'}, 1200, 'easeOutCubic', callback)
    }


    var floor1Init = false,
        boxElemets = $('.J_Box'),
        box2Elemets = $('.J_Box2'),
        tileA = $('.tileA .tile'),
        tileB = $('.tileB .tile'),
        footTags = $('.foot-tags div'),
        fromNav3 = true;

    $.each(tileA, function () {
        $(this).attr('init', 'false');
    });

    $.each(tileB, function () {
        $(this).attr('init', 'false');
    });

    $.each(boxElemets, function () {
        $(this).attr('init', 'false');
    });
    $.each(box2Elemets, function () {
        $(this).attr('init', 'false');
    });
    $.each(footTags, function () {
        $(this).attr('init', 'false');
    });
    $.each($('.services div'), function () {
        $(this).attr('init', 'false');
    });

    function animateInit(){
        $.each(boxElemets, function () {
            if ($(this).attr('init') == 'false' && isScrolledIntoView($(this))) {
                $(this).attr('init', 'true');
                $(this).animate({'left': '50%'}, 1000, 'easeOutCubic');
            }
        });

        $.each(tileA, function () {
            if ($(this).attr('init') == 'false' && isScrolledIntoView($(this))) {
                $(this).attr('init', 'true');
                $(this).stop().animate({ 'left': '50%' }, 1000, 'easeOutCubic');
            }
        });

        $.each(tileB, function () {
            if ($(this).attr('init') == 'false' && isScrolledIntoView($(this))) {
                $(this).attr('init', 'true');
                $(this).stop().animate({ 'left': '50%' }, 1000, 'easeOutCubic');
            }
        });

        ftAnimate('.foot-tag1', 0);
        ftAnimate('.foot-tag2', 100);
        ftAnimate('.foot-tag3', 200);
        ftAnimate('.foot-tag4', 300);
        ftAnimate('.foot-tag5', 400);

        ftAnimate('.s1', 0);
        ftAnimate('.s2', 100);
        ftAnimate('.s3', 200);
        ftAnimate('.s4', 300);

        if (!floor1Init) {
            animateTop('.star1', 0);
            animateTop('.star2', 200);
            animateTop('.star3', 400);
            animateTop('.star4', 600,function(){
                floor1Init = true;
            });
        }

    }
    animateInit();
    $(window).scroll(function () {
        animateInit();
    });
	
	/*首页Business当前效果*/
	$('.floor-content3 li').hover(function(){
		
		$(this).addClass('floor_on')
		$(this).find('.business_ho').stop().fadeTo(500, 1)
		$(this).find('.business_icon1').stop().animate({ "right": "-122px" }, 800)
		$(this).find('.business_icon2').stop().animate({ "left": "66px" }, 800)
		$(this).find('.business_text1').stop().animate({ "left": "-220px" }, 800)
		$(this).find('.business_text2').stop().animate({ "right": "18px" }, 800)
		
		},function(){
			$(this).removeClass('floor_on')
			$(this).find('.business_ho').stop().fadeTo(500, 0)
			$(this).find('.business_icon1').stop().animate({ "right": "66px" }, 800)
			$(this).find('.business_icon2').stop().animate({ "left": "-122px" }, 800)
			$(this).find('.business_text1').stop().animate({ "left": "18px" }, 800)
			$(this).find('.business_text2').stop().animate({ "right": "-220px" }, 800)
			
			})
		
	/*首页blog选项卡*/
	var $blog_this = $(".blog_sel a")
			$blog_this.hover(function(){
			$(this).addClass('blog_on').siblings().removeClass('blog_on');
			var blog_index = $blog_this.index(this);     /*创建当前点击li所对应的li元素的索引*/
			$(".blog_selCon>div").eq(blog_index).fadeIn(150).siblings().hide();    /*当前li点击对应索引显示同辈元素隐藏*/
		})	
		
	/*首页浮动定位*/
	$(window).scroll(function(){
		
		var scrll_top = $(this).scrollTop();
		$('.float').stop().animate({"top":scrll_top+200+"px"})
		
		})	
	
	$('.float_list li').hover(function(){
		
		var float_text = $(this).attr('data-name')
		
		$(this).find('a').html(float_text)
		$(this).find('a').css({"background-color":"#c70c1f","opacity":"1"})
		
		$(this).find('a').stop().animate({"width":"170px"},450)
		
		},function(){
		
			var float_less = $(this).attr('data-text')
			$(this).find('a').html(float_less)
			$(this).find('a').css({"background-color":"#000","opacity":"0.7"})
			$(this).find('a').stop().animate({"width":"30px"},450)
			
			})		
			
		$('.float_list li').click(function(){
			
			$(this).addClass('floag_on').siblings().removeClass('floag_on')
			
			})	
			
	})		