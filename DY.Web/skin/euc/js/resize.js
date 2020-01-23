$(function(){
	
	/*首页案例缩放比例*/


    $(window).resize(function () {


        $('.wedo_list ul').isotope('reLayout');

    });

    /* 筛选 */
    $('.wedo_select li').click(function () {
        //if($(this).hasClass("on")) return;
        $(this).addClass("on").siblings().removeClass("on");
        var selector = $(this).attr('data-filter');
        $('.wedo_list ul').isotope({ filter: selector });
        return false;
    });


    $(".iamge_show").fancybox({
        centerOnScroll: true,
        transitionIn: "elastic",
        transitionOut: "elastic",
        speedIn: 600,
        speedOut: 300
    });


    function numAdd(el) {
        var lastNum = el.data("num");
        el.text(0);
        var i = 0;
        var timer = setInterval(function () {
            el.text(i += 6);
            if (i >= lastNum) {
                clearInterval(timer);
                el.text(lastNum);
            }
            ;
        }, 110)
    }


    $(".priorities").waypoint(function (direction) {
        if (direction == "down") {
            numAdd($(".num").eq(0));
            numAdd($(".num").eq(1));
            numAdd($(".num").eq(2));
            numAdd($(".num").eq(3));
			numAdd($(".num").eq(4));
        }

    }, { offset: "64%"});

    $(".wedo").waypoint(function (direction) {
        if (direction == "down") {
            $('.wedo_list ul').isotope({ filter: "none" });
            $('.wedo_select li').first().click();

        }

    }, { offset: "64%"});


    //  SmoothScroll

    try {
        $.browserSelector();
        if ($("html").hasClass("chrome")) {
            $.smoothScroll();
        }
    } catch (err) {
    }


    //Placeholder

    if (typeof document.createElement("input").placeholder === 'undefined') {
        $('[placeholder]').focus(function () {
            var input = $(this);
            if (input.val() === input.attr('placeholder')) {
                input.val('');
                input.removeClass('placeholder');
            }
        }).blur(function () {
            var input = $(this);
            if (input.val() === '' || input.val() === input.attr('placeholder')) {
                input.addClass('placeholder');
                input.val(input.attr('placeholder'));
            }
        }).blur().parents('form').submit(function () {
            $(this).find('[placeholder]').each(function () {
                var input = $(this);
                if (input.val() === input.attr('placeholder')) {
                    input.val('');
                }
            });
        });
    }
	
	})