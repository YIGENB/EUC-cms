
/****************
*ҳ���ʼ������
****************/

//�������ʶΪ��
if ($.os.webkit ? false : true && $.os.fennec ? false : true && $.os.ie ? false : true && $.os.opera ? false : true) {
    $.os.webkit = true;
    $.feat.cssPrefix = $.os.webkit ? "Webkit" : "";
}

//(function ($) {
//    $.ui.ready(function () {


// //window.addEventListener("load", showHeaderSortBox, false);
//    });


//})(af);
var loadIndex = function () {
    $.ui.toggleHeaderMenu();
};
/*ͼƬ�б� ����ͼƬ�߶�*/
var reviseHeightImg = function () {
    if ($(".reviseHeightImg .img").length > 0) {
        $("body").append("<style>.reviseHeightImg .img{height:" + $(".reviseHeightImg .img").width() * templateConfig.reviseheightimgratio + "px!important}</style>");
    }
};
window.addEventListener("load", reviseHeightImg, false);
window.onresize = function () { reviseHeightImg() };



/*ͼƬ�б� ����ͼƬ�߶�*/
$(function () {
    var reviseHeightImg1 = function () {
        if ($(".menuvice a").length > 0) {
            $("body").append("<style>.menuvice a:nth-child(7n+1){height:" + $(".menuvice a:nth-child(7n+1)").width() * 1 + "px!important}.menuvice a:nth-child(7n+2){height:" + $(".menuvice a:nth-child(7n+2)").width() * 0.45 + "px!important}.menuvice a:nth-child(7n+3){height:" + $(".menuvice a:nth-child(7n+3)").width() * 1 + "px!important}.menuvice a:nth-child(7n+4){height:" + $(".menuvice a:nth-child(7n+4)").width() * 1 + "px!important}.menuvice a:nth-child(7n+5){height:" + $(".menuvice a:nth-child(7n+5)").width() * 1 + "px!important}.menuvice a:nth-child(7n+6){height:" + $(".menuvice a:nth-child(7n+6)").width() * 1 + "px!important}.menuvice a:nth-child(7n+7){height:" + $(".menuvice a:nth-child(7n+7)").width() * 1 + "px!important}</style>");
        }
    };
    reviseHeightImg1();
    window.onresize = function () { reviseHeightImg1() };
});
