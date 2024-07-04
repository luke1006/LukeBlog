//form数据提交，用于数据新增和编辑
function DataPost(postUrl, data) {
    var result;
    $.ajax({
        type: "post",
        async: false,
        dateType: "json",
        url: postUrl,
        data: data,
        contentType: false, //传文件必须！
        processData: false, //传文件必须！
        success: function (data) {
            result = data
        }
    });
    return result;
}
//显示临时选择的图片或视频
function LoadFile(tr_id, ctrl_id, ctrl_file_id) {
    document.getElementById(tr_id).className = "";
    var reader = new FileReader();
    reader.onload = function (e) {
        //var obj = document.getElementById(ctrl_file_id);
        document.getElementById(ctrl_file_id).src = e.target.result;
    };
    reader.readAsDataURL(document.getElementById(ctrl_id).files[0]);
}
//查询列表数据
function Search(postUrl, keyword) {
    var parms = new Object();
    parms.key = keyword;
    $.ajax({
        type: "post",
        async: false,
        dateType: "json",
        url: postUrl,
        data: parms,
        success: function (res) {
            document.getElementById('table-container').innerHTML = res.data;
        }
    });
}

function LoadAllAlbum() {
    var result;
    $.ajax({
        type: "post",
        dateType: "json",
        async: false,
        url: "/Album/GetAlbumSelect",
        success: function (res) {
            result = res
        }
    });
    return result;
}
//检察登录状态
function CheckSignState() {
    $.ajax({
        type: "post",
        dateType: "json",
        async: false,
        url: "/Website/GetSignStatus",
        success: function (res) {
            if (res.data == false) {
                //alert("请重新登录");
                //window.location.href = "../login.html";
            }

        }
    });

}






function getCheckedValues() {
    var checkboxes = document.getElementsByName('option');
    var selectedValues = [];
    for (var i = 0; i < checkboxes.length; i++) {
        if (checkboxes[i].checked) {
            selectedValues.push(checkboxes[i].value);
        }
    }

    var btnReply = document.getElementById("btnReply");


    switch (selectedValues.length) {
        case 0:
            document.getElementById('btnEdit').disabled = true;
            document.getElementById('btnDel').disabled = true;
            if (btnReply != undefined) {
                btnReply.disabled = true;
            }

            break;
        case 1:
            document.getElementById('btnEdit').disabled = false;
            document.getElementById('btnDel').disabled = false;
            if (btnReply != undefined) {
                btnReply.disabled = false;
            }
            break;
        default:
            document.getElementById('btnEdit').disabled = true;
            document.getElementById('btnDel').disabled = false;
            if (btnReply != undefined) {
                btnReply.disabled = true;
            }
            break;
    }
}

function Delete(deleteUrl, parms) {
    var result;
    if (confirm("您确认要删除吗？") == true) {
        $.ajax({
            type: "post",
            dateType: "json",
            async: false,
            url: deleteUrl + parms,
            success: function (res) {
                result = res;
            }
        });
    }
    return result;
}

function Opreate(obj, editUrl, delUrl) {
    var checkboxes = document.getElementsByName('option');
    var selectedValues = [];
    for (var i = 0; i < checkboxes.length; i++) {
        if (checkboxes[i].checked) {
            selectedValues.push(checkboxes[i].value);
        }
    }
    if (obj.id == "btnDel") {
        var parms = new Object();
        parms = selectedValues;
        var returnData = Delete(delUrl, selectedValues);
        alert(returnData.status);
        window.location.reload();
    }
    else {
        window.location.href = editUrl + selectedValues[0];
    }
}

function ReturnExistInfo(ParmsUrl, PostUrl) {
    var result;
    const urlParams = new URLSearchParams(ParmsUrl);
    const paramValue = urlParams.get('id');
    $.ajax({
        type: "post",
        dateType: "json",
        async: false,
        url: PostUrl + paramValue,
        success: function (res) {
            result = res;
        }
    });
    return result;
}
///////////////////////////////JSON时间格式转换
//先扩展一下javascript的Date类型,增加一个函数,用于返回我们想要的 yyyy-MM-dd HH:mm:ss 这种时间格式
Date.prototype.format = function (format) {
    var date = {
        "M+": this.getMonth() + 1,
        "d+": this.getDate(),
        "h+": this.getHours(),
        "m+": this.getMinutes(),
        "s+": this.getSeconds(),
        "q+": Math.floor((this.getMonth() + 3) / 3),
        "S+": this.getMilliseconds()
    };

    if (/(y+)/i.test(format)) {
        format = format.replace(RegExp.$1, (this.getFullYear() + '').substr(4 - RegExp.$1.length));
    }

    for (var k in date) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? date[k] : ("00" + date[k]).substr(("" + date[k]).length));
        }
    }

    return format;
}
//对外暴露的函数,替换掉/Date( )/
function convertTime(jsonTime, format) {
    var date = new Date(parseInt(jsonTime.replace("/Date(", "").replace(")/", ""), 10));
    var formatDate = date.format(format);
    return formatDate;
}
///////////////////////////////JSON时间格式转换

