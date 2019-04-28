/*获取URL参数值*/
function request(url, paras) {
    var paraString = url.substring(url.indexOf("?") + 1, url.length).split("&");
    var paraObj = {}
    for (i = 0; j = paraString[i]; i++) {
        paraObj[j.substring(0, j.indexOf("=")).toLowerCase()] = j.substring(j.indexOf("=") + 1, j.length);
    }
    var returnValue = paraObj[paras.toLowerCase()];
    if (typeof (returnValue) == "undefined") {
        return "";
    } else {
        return returnValue;
    }
}

/*过滤JSON脚本字符*/
function valueReplace(v) {
    v = v.toString().replace(new RegExp('(["\"])', 'g'), "\\\"");
    return v;
}

function tableloading() {
    //    $(".week_pro tr").mouseover(function () {
    //        $(this).addClass("altrow");
    //    }).mouseout(function () {
    //        $(this).toggleClass("altrow");
    //    });
    //    var trs = $(".week_pro").find("tr");
    //    for (i = 0; i < trs.length; i++) {
    //        var obj = trs[i];
    //        if (i % 2 == 0)
    //            obj.style.backgroundColor = "#F5FAFE";
    //    }
    //光棒效果
    var _bgColor;  //存放原始的 背景颜色
        //所有的行鼠标移进时变色
        $(".week_pro tr").hover(function () {
            _bgColor = $(this)[0].style.backgroundColor;
            $(this).css({ 'background-color': '#f5f5f5'/*, 'font-weight': 'bolder'*/ });
        },
        //移除还原颜色
    function () {
        var cssObj = { 'background-color': _bgColor, 'font-weight': '' }
        $(this).css(cssObj);

        //交替行变色
        var trs = $(".week_pro").find("tr");
        for (i = 0; i < trs.length; i++) {
            var obj = trs[i];
            if (i % 2 == 0)
                obj.style.backgroundColor = "#F5FAFE";
        }
    });
}

/*投票动态执行*/
function votechart() {
    var max = "barred";
    var middle = "baryellow";
    var min = "barblue";

    var maxValue = 0;
    var minValue = 0;

    var maxIndex = 0;
    var minIndex = 0;

    $(".charts").each(function (i, item) {
        var a = parseInt($(item).attr("w"));

        if (i == 0) {
            minValue = a;
            minIndex = i;
        }

        if (a > maxValue) {
            maxValue = a;
            maxIndex = i;
        } else if (a < minValue) {
            minValue = a;
            minIndex = i;
        }

    });

    $(".charts").each(function (i, item) {
        var addStyle = "";
        var divindex = parseInt($(item).attr("divindex"));
        if (divindex == maxIndex) {
            addStyle = max;
        } else if (divindex == minIndex) {
            addStyle = min;
        } else {
            addStyle = middle;
        }

        $(item).addClass(addStyle);
        var a = $(item).attr("w");
        $(item).animate({
            width: a + "%"
        }, 1000);
    });

}

function checkmail(email) {
    var myreg = /^([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$/;
    if (!myreg.test(email)) {
        return false;
    }
    else {
        return true;
    }
}

/*执行父节点关闭管理菜单事件*/
$(document).ready(function () {
    if (self.frameElement && self.frameElement.tagName == "IFRAME") {
        $(document).bind("click", function (e) {
            parent.window.panelmenu(false);
        });
        /*设备判断*/
        var browser = {
            versions: function () {
                var u = navigator.userAgent, app = navigator.appVersion;
                return {
                    trident: u.indexOf('Trident') > -1, //IE内核
                    presto: u.indexOf('Presto') > -1, //opera内核
                    webKit: u.indexOf('AppleWebKit') > -1, //苹果、谷歌内核
                    gecko: u.indexOf('Gecko') > -1 && u.indexOf('KHTML') == -1, //火狐内核
                    mobile: !!u.match(/AppleWebKit.*Mobile.*/) || !!u.match(/AppleWebKit/), //是否为移动终端
                    ios: !!u.match(/(i[^;]+\;(U;)? CPU.+Mac OS X)/), //ios终端
                    android: u.indexOf('Android') > -1 || u.indexOf('Linux') > -1, //android终端或者uc浏览器
                    iPhone: u.indexOf('iPhone') > -1 || u.indexOf('Mac') > -1, //是否为iPhone或者QQHD浏览器
                    iPad: u.indexOf('iPad') > -1, //是否iPad
                    webApp: u.indexOf('Safari') == -1 //是否web应该程序，没有头部与底部
                };
            } (),
            language: (navigator.browserLanguage || navigator.language).toLowerCase()
        }
        var isauto = false;
        if (browser.versions.iPad) {
            isauto = true;
        }
        else if (browser.versions.iPhone) {
            isauto = true;
        }
        if (isauto) {
            parent.window.frameauto($(window).height());
        }
    }
});

/*电子邮箱*/
function isEmail(str) {
    var reg = /^(\w)+(\.\w+)*@(\w)+((\.\w{2,3}){1,3})$/;
    return reg.test(str);
}
/*身份证正则 18位*/
function ckID18(idcard) {
    var re = /^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}([0-9]|X)$/;
    return re.test(idcard);
}
/*身份证正则 15位*/
function ckID15(idcard) {
    var re = /^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$/;
    return re.test(idcard);
}
/*手机号码 150,153,156,158,159，157，188，189*/
function IsMobile(mobile) {
    var re = /^(13[0-9]|15[0|1|2|3|5|6|7|8|9]|18[0|1|2|3|5|6|7|8|9]|17[7])\d{8}$/;
    return re.test(mobile);
}
/*手机号码或联系电话(可带区号)*/
function IsTelphone(phone) {
    var re = /^(0[0-9]{2,3}\-)?([2-9][0-9]{6,7})+(\-[0-9]{1,4})?$|(^(13[0-9]|15[0|3|6|7|8|9]|18[8|9])\d{8}$)/;
    return re.test(phone);
}

/*判断数字*/
function CheckNumber(num) {
    var re = /^[0-9]*$/;
    return re.test(num);
}