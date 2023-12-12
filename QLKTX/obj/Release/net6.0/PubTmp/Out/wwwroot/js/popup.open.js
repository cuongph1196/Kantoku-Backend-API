
//
function OpenUrlDetail(funcID, masterKey, transDate, isInsert, otherParams = "") {
    $('#indexDialog').dialog('showMask', 'Loading...'); //show loading
    var funcInfo = getOneDataFromApiSyncParams("/Home/GetFunctionInfo?FunctionID=" + funcID, funcID, null);
    if (!isInsert)
        funcInfo.Url = funcInfo.Url + '&masterKey=' + masterKey + '&transDate=' + transDate + otherParams;
    else 
        funcInfo.Url = funcInfo.Url + otherParams;
    $('#IFrameEntry').attr('src', funcInfo.Url);
    $('#IFrameEntry').attr('loading', 'lazy');
    $('#IFrameEntry').on('load', function () {
        $('#indexDialog').dialog('hideMask');  // hide the loading message
    });

    setTimeout(function () {
        $('#indexDialog').dialog({
            draggable: false,
            resizable: false,
            loadingMessage: 'Loading...',
            top: 10,
            width: '90%',
            height: window.innerHeight - 100,
            modal: true,
            closed: true,
            iconCls: 'icon-home',
            title: /*funcInfo.FunctionName ||*/ 'Thêm/ Cập nhật',
            collapsible: false,
            minimizable: false,
            maximizable: false,
            cache: true,
            enableClosed: false,
            onBeforeClose: function () {
                var dlg = $(this);
                var opts = dlg.dialog('options');
                if (opts.enableClosed) {
                    return true;
                }
                $("#indexLayout").removeClass("noscroll");
            }
        }).dialog('open');
        //scroll top
        document.body.scrollTop = 0; // For Safari
        document.documentElement.scrollTop = 0; // For Chrome, Firefox, IE and Opera
        $("#indexLayout").addClass("noscroll");
    }, 0);
}

function OpenNewUrlOnIframe(mode, funcID, rowKey, otherParams = "") {
    var funcInfo = getOneDataFromApiSyncParams("/Home/GetFunctionInfo?FunctionID=" + funcID, funcID, null);
    $('#indexDialog').window('setTitle', 'New Title');
    if (funcInfo.Url !== "") {
        var url = "";
        switch (mode) {
            case "edit":
                url += funcInfo.Url + '&masterKey=' + rowKey + otherParams;
                break;
            case "add":
                url += funcInfo.Url + otherParams;
                break;
            default:
        }
        location.assign(window.location.origin + url);
        //window.top.document.getElementsByClassName('panel-title panel-with-icon').innerHTML = "whatever";
        //window.top.document.getElementById('indexDialog').window('setTitle', 'New Title');
        //$('#indexDialog').window('setTitle', 'New Title');
        //console.log("ád", $('#indexDialog').window('setTitle', 'New Title'));
    }
}

function OpenUserInfo(userID, otherParams = "") {
    $('#indexDialog').dialog('showMask', 'Loading...'); //show loading
    var url = "/UserInfo" + otherParams;
    $('#IFrameEntry').attr('src', url);
    $('#IFrameEntry').attr('loading', 'lazy');
    $('#IFrameEntry').on('load', function () {
        $('#indexDialog').dialog('hideMask');  // hide the loading message
    });

    setTimeout(function () {
        $('#indexDialog').dialog({
            draggable: false,
            resizable: false,
            loadingMessage: 'Loading...',
            top: 10,
            width: '70%',
            height: '70%',
            modal: true,
            closed: true,
            iconCls: 'icon-home',
            title: 'Thông tin người dùng',
            collapsible: false,
            minimizable: false,
            maximizable: false,
            cache: true,
            enableClosed: false,
            onBeforeClose: function () {
                var dlg = $(this);
                var opts = dlg.dialog('options');
                if (opts.enableClosed) {
                    return true;
                }
                $("#indexLayout").removeClass("noscroll");
            }
        }).dialog('open');
        //scroll top
        document.body.scrollTop = 0; // For Safari
        document.documentElement.scrollTop = 0; // For Chrome, Firefox, IE and Opera
        $("#indexLayout").addClass("noscroll");
    }, 0);
}

//Reload url
function ReloadUrlOnIframe() {
    window.location.reload();
}

//open url
function OpenByUrl(funcID, otherParams = "") {
    var funcInfo = getOneDataFromApiSyncParams("/Home/GetFunctionInfo?FunctionID=" + funcID, funcID, null);
    if (funcInfo.Url !== "") {
        window.open(funcInfo.Url + otherParams);
    }
}