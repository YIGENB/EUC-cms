//微信设置

function setShare(title, url) {
    jiathis_config.title = title;
    jiathis_config.url = url;
}
var jiathis_config = {
    url: "",
    summary: "",
    title: "}",
    shortUrl: true,
    hideMore: false
}
/****/
if (typeof Utils != 'object') {
    alert('Utils object doesn\'t exists.');
}

var listTable = new Object;

listTable.query = "list";
listTable.filter = new Object;
listTable.url = location.href.lastIndexOf("?") == -1 ? location.href.substring((location.href.lastIndexOf("/")) + 1) : location.href.substring((location.href.lastIndexOf("/")) + 1, location.href.lastIndexOf("?"));
listTable.url += "?is_ajax=1";


/**
* 创建一个可修改区
*/
listTable.edit = function(obj, fieldName, id) {
    var tag = obj.firstChild.tagName;

    if (typeof (tag) != "undefined" && tag.toLowerCase() == "input") {
        return;
    }

    /* 保存原始的内容 */
    var org = obj.innerHTML;
    var val = Browser.isIE ? obj.innerText : obj.textContent;

    /* 创建一个输入框 */
    var txt = document.createElement("INPUT");
    txt.value = (val == 'N/A') ? '' : val;
    txt.style.width = "50%";

    /* 隐藏对象中的内容，并将输入框加入到对象中 */
    obj.innerHTML = "";
    obj.appendChild(txt);
    txt.focus();

    /* 修改区输入事件处理函数 */
    txt.onkeypress = function(e) {
        var evt = Utils.fixEvent(e);
        var obj = Utils.srcElement(e);

        if (evt.keyCode == 13) {
            obj.blur();

            return false;
        }

        if (evt.keyCode == 27) {
            obj.parentNode.innerHTML = org;
        }
    }

    /* 修改区失去焦点的处理函数 */
    txt.onblur = function(e) {
        if (Utils.trim(txt.value).length > 0 && org != Utils.trim(txt.value)) {
            var arge = "act=edit_field_value&fieldName=" + fieldName + "&val=" + encodeURIComponent(Utils.trim(txt.value)) + "&id=" + id;

            Ajax.call(listTable.url, arge, function(res) {
                if (res.message) {
                    alert(res.message);
                }

                obj.innerHTML = (res.error == 0) ? res.content : org;
            }, "POST", "json");
        }
        else {
            obj.innerHTML = org;
        }
    }
}

/**
* 切换状态
*/
listTable.toggle = function(obj, fieldName, id) {

    var val = (obj.src.match(/true.gif/i)) ? "0" : "1";

    var arge = "act=edit_field_value&fieldName=" + fieldName + "&val=" + val + "&id=" + id;

    Ajax.call(this.url, arge, function(res) {
        if (res.error > 0) {
            alert(res.message);
        }

        if (res.error == 0) {
            obj.src = (res.content == "1") ? 'images/true.gif' : 'images/false.gif';
        }
    }, "POST", "json");
}
/**
* 删除树列表中的一个记录
*/
listTable.treeremove = function (id, cfm, opt, type) {
    if (opt == null) {
        opt = "remove";
    }
    var thisid = "#deltr" + id;
    this.filter.type = type; //jxjy
    if (confirm(cfm)) {
        var args = "act=" + opt + "&id=" + id + this.compileFilter();
        Ajax.call(this.url, args, function (data) {
            if (data.error > 0) {
                alert(data.message);
            }
            else {
                $(thisid).closest('tr').remove();
            }
        }, "GET", "json");
    }
}
/**
* 导出
*/
listTable.importOut = function(type) {
    var ids = "";
    if (type == "select") {
        $('#listForm :checkbox[name="checkboxes"]:checked').each(function() {
            ids += $(this).attr('value') + ',';
        });
    }

    location.href = this.url + "&act=" + this.query + "&import=true&ids=" + ids + this.compileFilter();
}

/**
* 批量
*/
listTable.bacthAction = function(act, type) {
    var go = false;
    if (act == "CompletelyDelete") {
        go = confirm("删除的数据无法恢复，确定执行该操作吗？");
    }
    if (go) {
        var ids = "";
        $('#listForm :checkbox[name="checkboxes"]:checked').each(function() {
            ids += $(this).attr('value') + ',';
        });
        var arge = "act=" + act + "&ids=" + ids;
        if (type != "") {
            this.filter.type = type; //jxjy
        }
        Ajax.call(this.url, arge, function(res) {
            if (res.error > 0) {
                alert(res.message);
            }

            listTable.loadList();
        }, "POST", "json");
    }
}

/**
* 批量
*/
listTable.bacth = function(fieldName, val) {
    var ids = "";
    $('#listForm :checkbox[name="checkboxes"]:checked').each(function() {
        ids += $(this).attr('value') + ',';
    });
    var arge = "act=edit_field_values&fieldName=" + fieldName + "&val=" + val + "&ids=" + ids;
    //alert(arge);
    Ajax.call(this.url, arge, function(res) {
        if (res.error > 0) {
            alert(res.message);
        }

        listTable.loadList();
    }, "POST", "json");
}

/**
* 单个
*/
listTable.update = function(fieldName, val, id) {
    var arge = "act=edit_field_value&fieldName=" + fieldName + "&val=" + val + "&id=" + id;

    Ajax.call(this.url, arge, function(res) {
        if (res.error > 0) {
            alert(res.message);
        }

        listTable.loadList();
    }, "POST", "json");
}

/**
* 单个，不刷新页面
*/
listTable.update_no = function(fieldName, val, id) {
    var arge = "act=edit_field_value&fieldName=" + fieldName + "&val=" + val + "&id=" + id;

    Ajax.call(this.url, arge, function(res) {
        if (res.error > 0) {
            alert(res.message);
        }

    }, "POST", "json");
}

/**
* 切换排序方式
*/
listTable.sort = function(sort_by, sort_order) {
    var args = "&act=" + this.query + "&sort_by=" + sort_by + "&sort_order=";
    if (this.filter.sort_by == sort_by) {
        args += this.filter.sort_order == "desc" ? "asc" : "desc";
    }
    else {
        args += "desc";
    }
    for (var i in this.filter) {
       
        if (typeof (this.filter[i]) != "function" && i != "sort_order" && i != "sort_by") {
            args += "&" + i + "=" + this.filter[i];
        }
    }

    this.filter['page_size'] = this.getPageSize();
    //alert(args)
    Ajax.call(this.url, args, this.listCallback, "GET", "json");
}

/**
* 翻页
*/
listTable.gotoPage = function(page) {
    if (page != null) this.filter['page'] = page;

    if (this.filter['page'] > this.pageCount) this.filter['page'] = 1;

    this.filter['page_size'] = this.getPageSize();

    this.loadList(this.getPageSize());
}

/**
* 产品属性翻页分类ID
*/
listTable.gotoPageCategory = function (page, cagegoryid, type_id) {
    if (page != null) this.filter['page'] = page;

    if (this.filter['page'] > this.pageCount) this.filter['page'] = 1;

    this.filter['page_size'] = this.getPageSize();

    var args = "act=" + this.query + this.compileFilter() + "&attr_type=" + type_id + "&type_id=" + cagegoryid;
    Ajax.call(this.url, args, this.listCallback, "GET", "json");
}

/**
* 载入列表
*/
listTable.loadList = function() {
    var args = "act=" + this.query + this.compileFilter();
    Ajax.call(this.url, args, this.listCallback, "GET", "json");
}

/**
* 载入属性列表
*/
listTable.loadListgs = function (attr_type, type_id) {
    var args = "act=" + this.query + this.compileFilter() + "&&type_id=" + type_id + "&attr_type=" + attr_type;
    Ajax.call(this.url, args, this.listCallback, "GET", "json");
}
/**
* 载入列表
*/
listTable.loadListto = function (pid) {
    var args = "act=" + this.query + this.compileFilter() + "&pid=" + pid;
    Ajax.call(this.url, args, this.listCallback, "GET", "json");
}

/**
* 删除列表中的一个记录
*/
listTable.remove = function(id, cfm, opt, type) {
    if (opt == null) {
        opt = "remove";
    }
    this.filter.type = type; //jxjy
    if (confirm(cfm)) {
        var args = "act=" + opt + "&id=" + id + this.compileFilter();
        Ajax.call(this.url, args, function(data) {
            if (data.error > 0) {
                alert(data.message);
            }
            else {
                listTable.loadList();
            }
        }, "GET", "json");
    }
}
/**
* 删除列表中的一个记录
*/
    listTable.gsremoves = function (id, cfm, opt, type) {
    if (opt == null) {
        opt = "remove";
    }
    if (confirm(cfm)) {
        var args = "act=" + opt + "&id=" + id + this.compileFilter();
        Ajax.call(this.url, args, function (data) {
//            if (data.error > 0) {
//                alert(data.message);
//            }
//            else {
                location.reload();
            //}
        }, "GET", "json");
    }
}
/**
* 删除列表中的一个记录
*/
listTable.removes = function (id, cfm, typeid, opt, type) {
    if (opt == null) {
        opt = "remove";
    }
    this.filter.type = type; //jxjy
    if (confirm(cfm)) {
        var args = "act=" + opt + "&id=" + id + this.compileFilter();
        Ajax.call(this.url, args, function (data) {
            if (data.error > 0) {
                alert(data.message);
            }
            else {
                window.location = "/admin/goods_type.aspx?act=list&attr_type=" + typeid + "";
            }
        }, "GET", "json");
    }
}
/**
* 删除列表中的一个记录
*/
listTable.removeto = function (id, cfm, pid) {
    if (confirm(cfm)) {
        var args = "act=remove&id=" + id + this.compileFilter();
        Ajax.call(this.url, args, function (data) {
            if (data.error > 0) {
                alert(data.message);
            }
            else {
                listTable.loadListto(pid);
            }
        }, "GET", "json");
    }
}

/**
* 移动列表位置
*/
listTable.order = function(move_act, cat_id) {
    var args = "act=order&move_act=" + move_act + "&id=" + cat_id + this.compileFilter();
    Ajax.call(this.url, args, function(data) {
        if (data.error > 0) {
            alert(data.message);
        }
        else {
            listTable.loadList();
        }
    }, "GET", "json");
}

listTable.changePageSize = function(e) {
    var evt = Utils.fixEvent(e);
    if (evt.keyCode == 13) {
        listTable.gotoPage();
        return false;
    };
}

listTable.getPageSize = function() {
    var ps = 14;

    var pageSize = $('#pageSize');

    if (pageSize) {
        ps = Utils.isInt($(pageSize).val()) ? $(pageSize).val() : 14;
    }

    return ps;
}

listTable.listCallback = function(result) {
    if (result.error > 0) {
        alert(result.message);
    }
    else {
        try {
            $('#listDiv').html(result.content);

            if (typeof result.filter == "object") {
                listTable.filter = result.filter;
            }

            listTable.pageCount = result.page_count;

            $('#page-batch button.btnSubmit').attr('disabled', true);
        }
        catch (e) {
            alert(e.message);
        }
    }
}

listTable.selectAll = function(obj, chk) {
    if (chk == null) {
        chk = 'checkboxes';
    }
    $('.selectall').attr('checked', obj.checked);

    var elems = obj.form.getElementsByTagName("INPUT");

    for (var i = 0; i < elems.length; i++) {
        if (elems[i].name == chk || elems[i].name == chk + "[]") {
            if (!elems[i].disabled)
                elems[i].checked = obj.checked;
        }
    }
}

listTable.compileFilter = function() {
    var args = '';
    for (var i in this.filter) {
        if (typeof (this.filter[i]) != "function" && typeof (this.filter[i]) != "undefined") {
            args += "&" + i + "=" + encodeURIComponent(this.filter[i]);
        }
    }

    return args;
}

listTable.searchData = function() {
    var checkboxfield = "";
    var checkboxfieldvals = "";
    $('#searchForm :checkbox:checked').each(function() {
        checkboxfield += $(this).attr('name') + ',';
        checkboxfieldvals += $(this).attr('value') + ',';
    });
    $('#searchForm :radio:checked').each(function () {
        checkboxfield = $(this).attr('data') + ',';
        checkboxfieldvals = $(this).attr('value') + ',';
    });
    listTable.filter['field'] = $('#searchForm select[name="field"]').val();
    listTable.filter['target'] = $('#searchForm select[name="target"]').val();
    listTable.filter['val'] = $('#searchForm :input[name="val"]').val();
    listTable.filter['checkboxfield'] = checkboxfield;
    listTable.filter['checkboxfieldvals'] = checkboxfieldvals;
    listTable.filter['cat_id'] = $('#searchForm select[name="cat_id"]').val();
    listTable.loadList();

    if (checkboxfield != "" || listTable.filter['val'] != "") {
        $('#searchForm :input[value="ALL"]').attr('disabled', false);
    }
    else {
        $('#searchForm :input[value="ALL"]').attr('disabled', true);
    }
}

listTable.changeField = function() {
    var label = $('#searchForm select[name="field"] option:selected').parent().attr('label');

    $('#searchForm select[name="target"]').empty();

    if (label == "数值") {
        $('#searchForm select[name="target"]').append('<option value="isnum">等于</option><option value="gt">大于</option><option value="lt">小于</option>');
    }
    else {
        $('#searchForm select[name="target"]').append('<option value="like">包含</option><option value="istext">等于</option>');
    }
}

listTable.searchReset = function(btnAll) {
    document.getElementById('searchForm').reset();
    $(btnAll).attr('disabled', true);

    listTable.searchData();
}

document.onmousemove = function(e) {
    var obj = Utils.srcElement(e);
    if (typeof (obj.onclick) == 'function' && obj.onclick.toString().indexOf('listTable.edit') != -1) {
        $(obj).mouseover(function() {
            $(this).css({ border: '1px solid #e7eaec' }).attr('title', '点击修改内容');
        }).mouseout(function() {
            $(this).css({ border: 'none' });
        });
    }
}

$(document).ready(function() {
    $('#page-batch button.btnSubmit').attr('disabled', true);
    if (document.getElementById("listDiv")) {
        document.getElementById("listDiv").onmouseover = function(e) {
            obj = Utils.srcElement(e);

            if (obj) {
                if (obj.parentNode.tagName.toLowerCase() == "tr") row = obj.parentNode;
                else if (obj.parentNode.parentNode.tagName.toLowerCase() == "tr") row = obj.parentNode.parentNode;
                else return;

                for (i = 0; i < row.cells.length; i++) {
                    if (row.cells[i].tagName != "TH") row.cells[i].style.backgroundColor = '#F4FAFB';
                }
            }

        }

        document.getElementById("listDiv").onmouseout = function(e) {
            obj = Utils.srcElement(e);

            if (obj) {
                if (obj.parentNode.tagName.toLowerCase() == "tr") row = obj.parentNode;
                else if (obj.parentNode.parentNode.tagName.toLowerCase() == "tr") row = obj.parentNode.parentNode;
                else return;

                for (i = 0; i < row.cells.length; i++) {
                    if (row.cells[i].tagName != "TH") row.cells[i].style.backgroundColor = '#FFF';
                }
            }
        }
    }

    document.getElementById("listDiv").onclick = function(e) {
        var obj = Utils.srcElement(e);

        if (obj.tagName == "INPUT" && obj.type == "checkbox") {
            if (!document.forms['listForm']) {
                return;
            }
            var nodes = document.forms['listForm'].elements;
            var checked = false;

            for (i = 0; i < nodes.length; i++) {
                if (nodes[i].checked) {
                    checked = true;
                    break;
                }
            }

            if ($(".btnSubmit")) {
                $(".btnSubmit").attr('disabled', !checked);
            }
            if (checked) {
                $('#page-batch input.btnSubmit').css({ 'cursor': "pointer", "color": "#000000" });
            }
            else {
                $('#page-batch input.btnSubmit').css({ 'cursor': "inherit", "color": "#666666" });
            }
        }
    }
});