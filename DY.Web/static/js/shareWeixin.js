$(function () {
    var scripts = document.createElement("script");
    scripts.src = "http://res.wx.qq.com/open/js/jweixin-1.0.0.js";
    var tag = $("head").append(scripts);
    /*
    * 注意：
    * 1. 所有的JS接口只能在公众号绑定的域名下调用，公众号开发者需要先登录微信公众平台进入“公众号设置”的“功能设置”里填写“JS接口安全域名”。
    * 2. 如果发现在 Android 不能分享自定义内容，请到官网下载最新的包覆盖安装，Android 自定义分享接口需升级至 6.0.2.58 版本及以上。
    * 3. 完整 JS-SDK 文档地址：http://mp.weixin.qq.com/wiki/7/aaa137b55fb2e0456bf8dd9148dd613f.html
    *
    * 如有问题请通过以下渠道反馈：
    * 邮箱地址：weixin-open@qq.com
    * 邮件主题：【微信JS-SDK反馈】具体问题
    * 邮件内容说明：用简明的语言描述问题所在，并交代清楚遇到该问题的场景，可附上截屏图片，微信团队会尽快处理你的反馈。
    */

    var wxJsApiConfigFu = function (result) {
        wx.config({
            debug: false,
            appId: result.appId,
            timestamp: result.timestamp,
            nonceStr: result.nonceStr,
            signature: result.signature,
            jsApiList: [
          'checkJsApi',
          'onMenuShareTimeline',
          'onMenuShareAppMessage',
          'onMenuShareQQ',
          'onMenuShareWeibo',
          'hideMenuItems',
          'showMenuItems',
          'hideAllNonBaseMenuItem',
          'showAllNonBaseMenuItem',
          'translateVoice',
          'startRecord',
          'stopRecord',
          'onRecordEnd',
          'playVoice',
          'pauseVoice',
          'stopVoice',
          'uploadVoice',
          'downloadVoice',
          'chooseImage',
          'previewImage',
          'uploadImage',
          'downloadImage',
          'getNetworkType',
          'openLocation',
          'getLocation',
          'hideOptionMenu',
          'showOptionMenu',
          'closeWindow',
          'scanQRCode',
          'chooseWXPay',
          'openProductSpecificView',
          'addCard',
          'chooseCard',
          'openCard'
        ]
        });
        if ("undefined" != typeof shareWeixinData) {
            if (shareWeixinData.img == "" & document.getElementsByTagName("img").length > 0) { shareWeixinData.img = document.getElementsByTagName("img")[0].src; };

            wx.ready(function () {
                //发送给好友
                wx.onMenuShareAppMessage({
                    title: shareWeixinData.title,
                    desc: shareWeixinData.desc,
                    link: shareWeixinData.link,
                    imgUrl: shareWeixinData.img,
                    trigger: function (res) {
                        //alert('用户点击发送给朋友');
                    },
                    success: function (res) {
                        //alert('已分享');
                    },
                    cancel: function (res) {
                        //alert('已取消');
                    },
                    fail: function (res) {
                        //alert(JSON.stringify(res));
                    }
                });

                // 分享到朋友圈
                wx.onMenuShareTimeline({
                    title: shareWeixinData.title,
                    link: shareWeixinData.link,
                    imgUrl: shareWeixinData.img,
                    trigger: function (res) {
                        //alert('用户点击分享到朋友圈');
                    },
                    success: function (res) {
                        // alert('已分享');
                    },
                    cancel: function (res) {
                        //alert('已取消');
                    },
                    fail: function (res) {
                        //alert(JSON.stringify(res));
                    }
                });

                // 分享到微博
                wx.onMenuShareWeibo({
                    title: shareWeixinData.title,
                    desc: shareWeixinData.desc,
                    link: shareWeixinData.link,
                    imgUrl: shareWeixinData.img,
                    trigger: function (res) {
                        //alert('用户点击分享到微博');
                    },
                    complete: function (res) {
                        //alert(JSON.stringify(res));
                    },
                    success: function (res) {
                        //alert('已分享');
                    },
                    cancel: function (res) {
                        //alert('已取消');
                    },
                    fail: function (res) {
                        // alert(JSON.stringify(res));
                    }
                });
                if (shareWeixinData.toggle) {
                    //document.getElementById("shareWeixinBtnBox").style.display = "block";
                    document.getElementById("shareWeixinFriend").onclick = function () {
                        var classA = new shareWeixin({ share: "friend" });
                        classA.show();
                    };
                    document.getElementById("shareWeixinTimeline").onclick = function () {
                        var classA = new shareWeixin({ share: "timeline" });
                        classA.show();
                    };
                    document.getElementById("shareWeixinWeibo").onclick = function () {
                        var classA = new shareWeixin({ share: "weibo" });
                        classA.show();
                    }
                };

            });
        }
    };

    setInterval(autoResult, 10000);

    function autoResult() {
        jQuery.post("/api/weixin_jsapi.aspx", { url: location.href.split('#')[0] }, function (result) {
            wxJsApiConfigFu(result);
        }, "json");
    }
})