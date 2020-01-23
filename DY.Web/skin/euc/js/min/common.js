/* js Document */
/* Author: zq */
/* Time: 2014/10/9*/

$(function(){
	
	
	
	/*导航*/
	$('.nav li').hover(function(){

	    $(this).find('p a').stop().animate({ "margin-top": "-32px" }, 350)
		
		},function(){

		    $(this).find('p a').stop().animate({ "margin-top": "0" }, 350)
			
			})
			
	/*头部电话*/
	/*$('.h_tel').hover(function(){
		
		$(this).stop().animate({"width":"160px"},450)
		$(this).find('span').css("display","inline")
		
		},function(){
			
			$(this).stop().animate({"width":"32px"},450)
			
			})	*/	
	
	/*公共案例hover效果*/
	$('.wedo_list li').hover(function(){
		
		$(this).find('.wedo_show').animate({"bottom":"0"},350)
		$(this).find('.wedo_img img').animate({"opacity":"1"},150)
		
		},function(){
			
			$(this).find('.wedo_show').animate({"bottom":"-60"},350)
			$(this).find('.wedo_img img').animate({"opacity":"0.6"},150)
			
			})		
			
	/*公共team*/
	var l = $(".team_Mlist li").length;
   				 	
    $(".team_Mlist ul").width(1124 * l);

    var newsIndex = 1;
    $(".team_r").click(function () {
        if (newsIndex < l) {
            var $wrap = $('.team_Mlist ul')
            $wrap.stop(true, true).animate({
                marginLeft: -newsIndex * 140
            }, 800);
			$('.team_Mlist ul li').eq(newsIndex).addClass('team_on').siblings().removeClass('team_on')
			var team_index = $('.team_Mlist ul li').eq(newsIndex).attr('data-ask')
			$('.team_ask').html(team_index)
            newsIndex++
        }
       
    });

    $(".team_l").click(function () {
        if (newsIndex > 1) {
            var $wrap = $('.team_Mlist ul')
            $wrap.stop(true, true).animate({
                marginLeft: -(newsIndex-2) * 140
            }, 800);
			$('.team_Mlist ul li').eq(newsIndex-2).addClass('team_on').siblings().removeClass('team_on')
			var team_index = $('.team_Mlist ul li').eq(newsIndex-2).attr('data-ask')
			$('.team_ask').html(team_index)
            newsIndex--
        }
        
    })		
	
	$('.team_Mlist li').eq(3).addClass('team_on')
	var team_7Text = $('.team_Mlist li:eq(3)').attr('data-ask')
	$('.team_ask').html(team_7Text)
	$('.team_Mlist li').hover(function(){
		
		var team_text = $(this).attr('data-ask')
		$('.team_ask').html(team_text)
		
		$(this).addClass('team_on').siblings().removeClass('team_on')
		
		})			
		
		/*浮动qq*/
	
	$(window).scroll(function(){
		
		var float_top = $(this).scrollTop();
		$('.float_qq').stop().animate({"top":float_top+200+"px"})
		
		})
	
	$('.float_qq2').hover(function(){

	    $(this).animate({ "left": "-70px", "width": "126px" }, 350)
		
		},function(){
			
			$(this).animate({"left":"0","width":"50px"},350)
			
			})	
	
	$('.float_qq3').hover(function(){

	    $(this).animate({ "left": "-116px", "width": "172px" }, 350)
		
		},function(){

		    $(this).animate({ "left": "0", "width": "50px" }, 350)
			
			})	
	
	$('.float_qq4').hover(function(){
		
		$(this).find('.float_shwx').show();
		
		},function(){
			
			$(this).find('.float_shwx').hide();
			
			})		
			
	$('.foot_dshare3').hover(function(){
		
		$('.foot_shWx').show();
		
		},function(){
			
			$('.foot_shWx').hide();
			
			})	
			
		$('.foot_dshare2').hover(function(){
		
		$('.foot_shTelWx').show();
		
		},function(){
			
			$('.foot_shTelWx').hide();
			
			})			
		
	})