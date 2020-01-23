
(function($){
$.fn.quickAd = function(settings){
settings = $.extend({
width:700,
height:396,
html:'我是广告内容',
/*top:130,*/
sec:5, //广告显示时长，单位秒
border:true //显示外框
},settings);
var fkxc_ad = 0;
var bodyWidth = $(window).width();
var _adBodyContainerID = "bigAd_"+settings.width;
var _adCloseContainerID = "bitAdClose_"+settings.width;
var closeHtml = '<a href="javascript:;" id="__close_ad">关 闭</a>';
//广告内容容器
var _adContent = '<div id="'+_adBodyContainerID+'"></div>';
//关闭按钮容器
var _adCloseBtn = '<div id="'+_adCloseContainerID+'">'+closeHtml+'</div>';
var self = $(this);
$(this).empty().html(_adContent+_adCloseBtn);
$('#__close_ad').click(function(){
window.clearTimeout(fkxc_ad);
self.slideUp(1200);
})
$('#'+_adBodyContainerID).empty().html(settings.html).css({
'width':settings.width+'px',
'height':settings.height+'px',
'left':(bodyWidth - settings.width) / 2 + 'px'
}).fadeIn('fast');
$('#'+_adCloseContainerID).css({
'position': 'absolute',
 'z-index': 20001
}).show();
var daojishi = function (s) {
fkxc_ad = setInterval(function () {
if (s == 0) {
self.slideUp(1200);
}
$("#__sec").text(s);
s--;
}, 1000);
}
daojishi(settings.sec)
}
})(jQuery) 