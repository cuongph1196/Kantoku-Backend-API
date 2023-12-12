
var ProjectCF = {
    configs: {
        pageSize: 25,
        pageIndex: 1,
        baseApi: '/api'
    },
    unflattern: function (arr) {
        var tree = [],
            mappedArr = {},
            arrElem,
            mappedElem;

        // First map the nodes of the array to an object -> create a hash table.
        for (var i = 0, len = arr.length; i < len; i++) {
            arrElem = arr[i];
            mappedArr[arrElem.id] = arrElem;
            mappedArr[arrElem.id]['children'] = [];
        }
        for (var id in mappedArr) {
            if (mappedArr.hasOwnProperty(id)) {
                mappedElem = mappedArr[id];
                // If the element is not at the root level, add it to its parent array of children.
                if (mappedElem.parentid) {
                    //mappedElem.state = 'closed';
                    mappedArr[mappedElem['parentid']]['children'].push(mappedElem);
                }
                // If the element is at the root level, add it to first level elements array.
                else {
                    tree.push(mappedElem);
                }
            }
        }
        return tree;
    },
    unflatternStartMenu: function (arr) {
        var treeStartMenu = [],
            mappedArr = {},
            arrElem,
            mappedElem;

        // First map the nodes of the array to an object -> create a hash table.
        for (var i = 0, len = arr.length; i < len; i++) {
            arrElem = arr[i];
            mappedArr[arrElem.idMapp] = arrElem;
            mappedArr[arrElem.idMapp]['menus'] = [];
        }
        for (var idMapp in mappedArr) {
            if (mappedArr.hasOwnProperty(idMapp)) {
                mappedElem = mappedArr[idMapp];
                // If the element is not at the root level, add it to its parent array of children.
                if (mappedElem.parentid) {
                    //mappedElem.state = 'closed';
                    console.log('mappedElem', mappedElem);
                    mappedArr[mappedElem['parentid']]['menus'].push(mappedElem);
                }
                // If the element is at the root level, add it to first level elements array.
                else {
                    treeStartMenu.push(mappedElem);
                }
            }
        }
        return treeStartMenu;
    },
    confirm: function (message, okCallback) {
        bootbox.confirm({
            message: message,
            buttons: {
                confirm: {
                    label: 'Đồng ý',
                    className: "btn-success btn-sm",
                },
                cancel: {
                    label: 'Hủy',
                    className: 'btn-danger'
                }
            },
            callback: function (result) {
                if (result === true) {
                    okCallback();
                }
            }
        });
    }
};

function ajaxindicatorstart(text) {
    //setTimeout(function () {
    var $preloader = $(".preloader");
    if ($preloader.length == 0) {
        var path = location.protocol + "//" + location.host + "/";
        $("body .wrapper").append(
            `<div class="preloader flex-column justify-content-center align-items-center">
                <img class="animation__shake" src="${path}assets/img/AdminLTELogo.png" alt="AdminLTELogo" height="60" width="60">
                <p>${text}</p>
            </div>`
        );
        //
    } else {
        $(".preloader p").html(text);
    }
    if ($preloader.children().is(":hidden")) {
        //setTimeout(function () {
        $preloader = $(".preloader");
        $preloader.css({ 'height': '100vh', 'opacity': '0.8' });
        $preloader.children().show();
        //}, 200);
    }
    //}, 1000);
};

function ajaxindicatorstop() {
    setTimeout(function () {
        var $preloader = $(".preloader");
        if ($preloader) {
            $preloader.css('height', 0);
            setTimeout(function () {
                $preloader.children().hide();
            }, 200);
        }
    }, 800);
};

//get data
function getDataFromApiSyncParams(pUrl, funcId, params) {
    var arr = [];
    $.ajax({
        url: pUrl,
        type: 'GET',
        headers: { 'FunctionID': funcId },
        dataType: 'json',
        data: params,
        async: false,
        success: function (response) {
            if (response.Success) {
                arr = response.Data.Items;
            }
        }
    });
    return arr;
}

function getOneDataFromApiSyncParams(pUrl, funcId, params) {
    var arr = [];
    $.ajax({
        url: pUrl,
        type: 'GET',
        headers: { 'FunctionID': funcId },
        dataType: 'json',
        data: params,
        async: false,
        success: function (response) {
            if (response.Success) {
                arr = response.Data;
            }
        }
    });
    return arr;
}

//init datalist no checkbox
function initDataList(responseData, dataListId) {
    if (responseData.length > 0) {
        $('#' + dataListId).datalist({
            data: responseData,
            checkbox: false,
            lines: true,
            valueField: 'Id',
            textField: 'NameCompare'
        });
    }
}

//getData combo
function getDataCombobox(pUrl) {
    var arr = [];
    $.ajax({
        url: pUrl,
        type: 'GET',
        dataType: 'json',
        async: false,
        success: function (response) {
            if (response.Success) {
                arr = response.Data.Items;
            }
        }
    });
    return arr;
}

function getDataComboTree(pUrl) {
    var arr = [];
    $.ajax({
        url: pUrl,
        type: 'GET',
        dataType: 'json',
        async: false,
        success: function (response) {
            var data = [];
            if (response.Success) {
                $.each(response.Data.Items, function (i, item) {
                    data.push({
                        id: item.Id,
                        text: item.NameCompare,
                        text1: item.Name,
                        parentid: item.ParentId,
                        isparent: item.IsParent,
                        levelid: item.LevelId,
                        departmentlevelid: item.DepartmentLevelId,
                        departmentpermissIds: item.JsonDepartPermissIDs
                    });
                });
                arr = ProjectCF.unflattern(data);
            }
        }
    });
    return arr;
}

//getData Autocomplete
function initAutoCompletexPartner(buildingKey, pMinLength, cboLookupSearch, txtLookupSearchID, txtLookupSearchName, txtLookupSearchOther1, txtLookupSearchOther2, txtLookupSearchOther3, pRequired = false, pDefaultValue = "", pDefaultText = "") {
    $("#" + cboLookupSearch + "").combobox({
        mode: 'remote',
        valueField: 'id',
        textField: 'namecompare',
        labelPosition: 'top',
        icons: [{
            iconCls: 'icon-clear',
            handler: function (e) {
                $(e.data.target).combobox('clear');
            }
        }],
        prompt: VNMessageR.COMMON_00022,
        required: pRequired,
        loader: function (param, success, error) {
            if (param.q === undefined && pDefaultValue && pDefaultValue !== "") {
                $("[id$=" + cboLookupSearch + "]").combobox("setValue", pDefaultValue);
                $("[id$=" + cboLookupSearch + "]").combobox("setText", pDefaultText);
            }
            var urlAuto = ProjectCF.configs.baseApi + `/Common/autocomplex/partner?buildingKey=${buildingKey}&prefix=`;
            var pQuery = param.q || pDefaultValue || '';
            if (pQuery.length < pMinLength) { return false; }
            $.ajax({
                type: "GET",
                url: urlAuto + pQuery.toLowerCase(),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.Success) {
                        var items = $.map(data.Data.Items, function (item, index) {
                            return {
                                id: item.Id,
                                name: item.Name,
                                namecompare: item.NameCompare,
                                other1: item.Other1,
                                other2: item.Other2,
                                other3: item.Other3,
                                other4: item.Other4,
                                other5: item.Other5
                            };
                        });
                        success(items);
                    }
                },
                error: function () {
                    error.apply(this, arguments);
                }
            });
        },
        onSelect: function (record) {
            if (record) {
                $("[id$=" + txtLookupSearchID + "]").val(record.id);
                $("[id$=" + txtLookupSearchName + "]").val(record.name);
                $("[id$=" + txtLookupSearchOther1 + "]").val(record.other1);
                $("[id$=" + txtLookupSearchOther2 + "]").val(record.other2);
                $("[id$=" + txtLookupSearchOther3 + "]").val(record.other3);
            }
        },
        onUnselect: function () {
            $("[id$=" + txtLookupSearchID + "]").val('');
            $("[id$=" + txtLookupSearchName + "]").val('');
            $("[id$=" + txtLookupSearchOther1 + "]").val('');
            $("[id$=" + txtLookupSearchOther2 + "]").val('');
            $("[id$=" + txtLookupSearchOther3 + "]").val('');
        },
        validType: 'inList["' + "#" + cboLookupSearch + '"]'
    });
}

function initAutoCompletex(routes, pMinLength, cboLookupSearch, txtLookupSearchID, txtLookupSearchName, txtLookupSearchOther1, txtLookupSearchOther2, txtLookupSearchOther3, pRequired = false, pDefaultValue = "", pDefaultText = "") {
    $("#" + cboLookupSearch + "").combobox({
        mode: 'remote',
        valueField: 'id',
        textField: 'namecompare',
        labelPosition: 'top',
        icons: [{
            iconCls: 'icon-clear',
            handler: function (e) {
                $(e.data.target).combobox('clear');
            }
        }],
        prompt: VNMessageR.COMMON_00022,
        required: pRequired,
        loader: function (param, success, error) {
            if (param.q === undefined && pDefaultValue && pDefaultValue !== "") {
                $("[id$=" + cboLookupSearch + "]").combobox("setValue", pDefaultValue);
                $("[id$=" + cboLookupSearch + "]").combobox("setText", pDefaultText);
            }
            var urlAuto = ProjectCF.configs.baseApi + "/Common/autocomplex/" + routes + "?prefix=";
            var pQuery = param.q || pDefaultValue || '';
            if (pQuery.length < pMinLength) { return false; }
            $.ajax({
                type: "GET",
                url: urlAuto + pQuery.toLowerCase(),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.Success) {
                        var items = $.map(data.Data.Items, function (item, index) {
                            return {
                                id: item.Id,
                                name: item.Name,
                                namecompare: item.NameCompare,
                                other1: item.Other1,
                                other2: item.Other2,
                                other3: item.Other3,
                                other4: item.Other4,
                                other5: item.Other5
                            };
                        });
                        success(items);
                    }
                },
                error: function () {
                    error.apply(this, arguments);
                }
            });
        },
        onSelect: function (record) {
            if (record) {
                $("[id$=" + txtLookupSearchID + "]").val(record.id);
                $("[id$=" + txtLookupSearchName + "]").val(record.name);
                $("[id$=" + txtLookupSearchOther1 + "]").val(record.other1);
                $("[id$=" + txtLookupSearchOther2 + "]").val(record.other2);
                $("[id$=" + txtLookupSearchOther3 + "]").val(record.other3);
            }
        },
        onUnselect: function () {
            $("[id$=" + txtLookupSearchID + "]").val('');
            $("[id$=" + txtLookupSearchName + "]").val('');
            $("[id$=" + txtLookupSearchOther1 + "]").val('');
            $("[id$=" + txtLookupSearchOther2 + "]").val('');
            $("[id$=" + txtLookupSearchOther3 + "]").val('');
        },
        validType: 'inList["' + "#" + cboLookupSearch + '"]'
    });
}

var loaderAutocompletePartnerInGrid = function (param, success, error) {
    var urlAuto = ProjectCF.configs.baseApi + "/Common/autocomplex/partner?prefix=";
    var pQuery = param.q || '';
    if (pQuery.length <= 2) { return false; }
    $.ajax({
        type: "GET",
        //headers: { 'FunctionID': functionId },
        url: urlAuto + pQuery.toLowerCase(),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.Success) {
                var items = $.map(data.Data.Items, function (item, index) {
                    return {
                        id: item.Id,
                        namecompare: item.NameCompare,
                        name: item.Name,
                        defaultaccId: item.DefaultAccountID,
                        other1: item.Other1,
                        other2: item.Other2,
                        other3: item.Other3,
                        other4: item.Other4,
                        other5: item.Other5
                    };
                });
                success(items);
            }
        },
        error: function () {
            error.apply(this, arguments);
        }
    });
};

//init combobox chung cho hệ thống
function initComboBox(responseData, cboBoxId, pRequired, pPanelHeight, pEditable = true, pDefaultValue) {
    //if (responseData.length > 0) {
    $('#' + cboBoxId).combobox({
        editable: pEditable,
        selectOnNavigation: false,
        data: responseData,
        required: pRequired,
        panelHeight: pPanelHeight,
        valueField: 'Id',
        textField: 'NameCompare',
        icons: [{
            iconCls: 'icon-clear',
            handler: function (e) {
                $(e.data.target).combobox('clear');
            }
        }],
        onLoadSuccess: function () {
            $('#' + cboBoxId).combobox('setValue', pDefaultValue);
        },
        inputEvents: $.extend({}, $.fn.combobox.defaults.inputEvents, {
            blur: function (e) {
                $.fn.combobox.defaults.keyHandler.enter.call(e.data.target);
            }
        }),
        validType: 'inList["' + "#" + cboBoxId + '"]',
        limitToList: true
    });
    //}
}
//init combobox chung cho hệ thống not valid
function initComboBoxNotValid(responseData, cboBoxId, pRequired, pPanelHeight, pEditable = true, pDefaultValue, isMultiple = false) {
    //if (responseData.length > 0) {
    $('#' + cboBoxId).combobox({
        editable: pEditable,
        selectOnNavigation: false,
        data: responseData,
        required: pRequired,
        panelHeight: pPanelHeight,
        multiple: isMultiple,
        valueField: 'Id',
        textField: 'NameCompare',
        icons: [{
            iconCls: 'icon-clear',
            handler: function (e) {
                $(e.data.target).combobox('clear');
            }
        }],
        onLoadSuccess: function () {
            if (isMultiple) {
                $('#' + cboBoxId).combobox('setValues', pDefaultValue);
            }
            else {
                $('#' + cboBoxId).combobox('setValue', pDefaultValue);
            }
        },
        limitToList: true
    });
    //}
}

//init combobox chung cho hệ thống
function initComboBoxExists(responseData, cboBoxId, pRequired, pPanelHeight, pEditable = true, pDefaultValue, isMultiple = false) {
    //if (responseData.length > 0) {
    $('#' + cboBoxId).combobox({
        editable: pEditable,
        selectOnNavigation: false,
        data: responseData,
        required: pRequired,
        panelHeight: pPanelHeight,
        multiple: isMultiple,
        valueField: 'Id',
        textField: 'NameCompare',
        icons: [{
            iconCls: 'icon-clear',
            handler: function (e) {
                $(e.data.target).combobox('clear');
            }
        }],
        onLoadSuccess: function () {
            if (isMultiple) {
                $('#' + cboBoxId).combobox('setValues', pDefaultValue);
            }
            else {
                $('#' + cboBoxId).combobox('setValue', pDefaultValue);
            }
        },
        limitToList: true,
        validType: `exists['#${cboBoxId}']`
    });
    //}
}

//ko chọn node cha
function initComboTree(responseData, cboTreeId, pRequired, pPanelHeight, defaultValue) {
    $('#' + cboTreeId).combotree({
        lines: true,
        enableFiltering: true,
        data: responseData,
        required: pRequired,
        panelHeight: pPanelHeight,
        icons: [{
            iconCls: 'icon-clear',
            handler: function (e) {
                $(e.data.target).combotree('clear');
            }
        }],
        onBeforeSelect: function (node) {
            //check isparent
            if (node.children?.length > 0) {
                return false;
            }
            return true;
        },
        validType: 'validCombotree["' + "#" + cboTreeId + '"]',
        onLoadSuccess: function () {
            $('#' + cboTreeId).combotree("setValue", defaultValue);
        }
    });
}

//chọn node cha
function initComboTreeSelectParent(responseData, cboTreeId, pRequired, pPanelHeight, defaultValue) {
    $('#' + cboTreeId).combotree({
        lines: true,
        hasDownArrow: true,
        data: responseData,
        required: pRequired,
        panelHeight: pPanelHeight,
        icons: [{
            iconCls: 'icon-clear',
            handler: function (e) {
                $(e.data.target).combotree('clear');
            }
        }],
        validType: 'validCombotreeSelectParent["' + "#" + cboTreeId + '"]',
        onLoadSuccess: function () {
            $('#' + cboTreeId).combotree("setValue", defaultValue);
        }
    });
}

//Valid_Entry
function required(requiredList, i) {
    requiredList[i].style.borderColor = "red";
}

function reset_effect(requiredList, i) {
    requiredList[i].style.borderColor = "#D5D5D5";
}

function ValidateControlForm(requiredList) {
    var flag = true;
    if (requiredList.length > 0) {
        for (var i = 0; i < requiredList.length; i++) {
            if (requiredList[i].value.trim() === '') {
                required(requiredList, i);
                flag = false;
            } else {
                reset_effect(requiredList, i);
            }
        }
    }
    return flag;
}

//valid datagrid
function ValidateDatagridRows(datagrid_id) {
    var isValid = true;
    var abDetailPartnerRows = $(`#${datagrid_id}`).datagrid('getRows');
    if (abDetailPartnerRows.length <= 0) {
        isValid = false;
    }
    else {
        jQuery.each(abDetailPartnerRows, function (index, item) {
            if ($(`#${datagrid_id}`).datagrid('validateRow', index) === false) {
                isValid = false;
            }
        });
    }
    return isValid;
}

function ValidateDatagridChecked(datagrid_id) {
    var isValid = true;
    var abDetailPartnerRows = $(`#${datagrid_id}`).datagrid('getChecked');
    if (abDetailPartnerRows.length <= 0) {
        isValid = false;
    }
    else {
        jQuery.each(abDetailPartnerRows, function (index, item) {
            if ($(`#${datagrid_id}`).datagrid('validateRow', index) === false) {
                isValid = false;
            }
        });
    }
    return isValid;
}

//Toastr
function ToastrException(err) {
    if (err.status === 401 || err.status === 403) {
        toastr.error(VNMessageR.COMMON_00008);
    } else if (err.status === 400) {
        if (err.responseJSON && err.responseJSON.error.length > 0) {
            toastr.error(err.responseJSON.error[0]);
        }
    } else if (err.status === 300) {
        toastr.error(err.responseJSON.error[0]);
    } else if (err.status === 408) {
        toastr.error(VNMessageR.COMMON_00009);
    } else {
        toastr.error(VNMessageR.COMMON_00006);
    }
}

function ToastrErr(msg) {
    if (msg.Message === "F_ID_EXISTS") {
        toastr.error(VNMessageR.COMMON_00004);
    } else if (msg.Message === "F_ID_NEXISTS") {
        toastr.error(VNMessageR.COMMON_00005);
    } else if (msg.Data === -2) {
        toastr.error(VNMessageR.COMMON_00004);
    } else if (msg.StatusCode === 403) {
        toastr.error(msg.Message);
    } else if (msg.Message !== "") {
        toastr.error(msg.Message);
    } else {
        toastr.error(VNMessageR.COMMON_00006);
    }
}

function GetSortColumnNameFromIndex(colIndex, list) {
    var sortColumn = '';
    $.each(list, function (index, obj) {
        if (index === colIndex) {
            sortColumn = obj.data;
            return false;
        }
    });
    return sortColumn;
}

//---------Convert data to tree data-----------//
function convertToTreeData(rows, isCollapse) {
    function exists(rows, parentid) {
        for (var i = 0; i < rows.length; i++) {
            if (rows[i].id === parentid) return true;
        }
        return false;
    }
    var nodes = [];
    // get the top level nodes
    for (var j = 0; j < rows.length; j++) {
        var rowJ = rows[j];
        if (!exists(rows, rowJ.parentid)) {
            nodes.push({
                id: rowJ.id,
                name: rowJ.name,
                text: rowJ.text,
                parentid: rowJ.parentid,
                isparent: true,
                active: rowJ.active,
                notes: rowJ.notes,
                levelid: rowJ.levelid,
                module: rowJ.module,
                display: rowJ.display,
                isuserdev: rowJ.isuserdev,
                checked: rowJ.checked,
                other: rowJ.other,
                other1: rowJ.other1,
                other2: rowJ.other2,
                other3: rowJ.other3
            });
        }
    }

    var toDo = [];
    for (var g = 0; g < nodes.length; g++) {
        toDo.push(nodes[g]);
    }

    while (toDo.length) {
        var node = toDo.shift();    // the parent node
        // get the children nodes
        for (var k = 0; k < rows.length; k++) {
            var row = rows[k];
            if (row.parentid === node.id) {
                var child = {
                    id: row.id,
                    name: row.name,
                    text: row.text,
                    parentid: row.parentid,
                    isparent: false,
                    active: row.active,
                    notes: row.notes,
                    levelid: row.levelid,
                    module: row.module,
                    display: row.display,
                    isuserdev: row.isuserdev,
                    checked: row.checked,
                    other: row.other,
                    other1: row.other1,
                    other2: row.other2,
                    other3: row.other3
                };
                if (node.children) {
                    node.children.push(child);
                } else {
                    if (isCollapse) {
                        node.state = 'closed';
                    }
                    node.children = [child];
                }
                toDo.push(child);
            }
        }
    }
    return nodes;
}

//phân trang datagrid
function pagerFilter(data) {
    if (typeof data.length === 'number' && typeof data.splice === 'function') {
        data = {
            total: data.length,
            rows: data
        };
    }
    var dg = $(this);
    var opts = dg.datagrid('options');
    var pager = dg.datagrid('getPager');
    pager.pagination({
        onSelectPage: function (pageNum, pageSize) {
            opts.pageNumber = pageNum;
            opts.pageSize = pageSize;
            pager.pagination('refresh',
                {
                    pageNumber: pageNum,
                    pageSize: pageSize
                });
            dg.datagrid('loadData', data);
        }
    });
    if (!data.originalRows) {
        data.originalRows = (data.rows);
    }
    var start = (opts.pageNumber - 1) * parseInt(opts.pageSize);
    var end = start + parseInt(opts.pageSize);
    data.rows = (data.originalRows.slice(start, end));
    return data;
}

//cuongph- check save data before next page
function pagerFilterCheck(data) {
    if (typeof data.length === 'number' && typeof data.splice === 'function') {
        data = {
            total: data.length,
            rows: data
        };
    }
    var dg = $(this);
    var opts = dg.datagrid('options');
    var pager = dg.datagrid('getPager');
    for (var i = 0; i < data.rows.length; i++) {
        dg.datagrid('endEdit', i);
    }
    pager.pagination({
        onSelectPage: function (pageNum, pageSize) {
            if (confirm("Vui lòng kiểm tra đã lưu dữ liệu trước khi chuyển trang chưa ?")) {
                opts.pageNumber = pageNum;
                opts.pageSize = pageSize;
                pager.pagination('refresh',
                    {
                        pageNumber: pageNum,
                        pageSize: pageSize
                    });
                dg.datagrid('loadData', data);
            }
        }
    });
    if (!data.originalRows) {
        data.originalRows = (data.rows);
    }
    var start = (opts.pageNumber - 1) * parseInt(opts.pageSize);
    var end = start + parseInt(opts.pageSize);
    data.rows = (data.originalRows.slice(start, end));
    return data;
}

//disable form
function disableAllField(isDisabled) {
    //input[type='file']
    $(".modal.fade, #formEntry").find("input[type='text'],input[type='number'],input[type='date'],input[type='email'],input[type='checkbox'], select, textarea").prop("disabled", isDisabled);
    $('.modal.fade .easyui-combobox, #formEntry .easyui-combobox').combobox(isDisabled ? 'disable' : 'enable');
    $('.modal.fade .easyui-combotree, #formEntry .easyui-combotree').combotree(isDisabled ? 'disable' : 'enable');
    $('.modal.fade .l-btn, #formEntry .l-btn').linkbutton(isDisabled ? 'disable' : 'enable');
    //$(".easyui-fluid .datagrid-view .datagrid-body .datagrid-btable").css("pointer-events", isDisabled ? "none" : "auto");
    if (isDisabled) {
        $(".modal.fade .easyui-fluid .datagrid-view .datagrid-body, #formEntry .easyui-fluid .datagrid-view .datagrid-body").addClass("datagrid-disabled");
    } else {
        $(".modal.fade .easyui-fluid .datagrid-view .datagrid-body, #formEntry .easyui-fluid .datagrid-view .datagrid-body").removeClass("datagrid-disabled");
    }
}
//disable button
function checkShowAllField(is_Approved, is_Deleted) {
    var flagDisabled = false;
    if (is_Approved) {
        $("[id*=btnUpdate]").prop("disabled", true);
        $("[id*=btnDelete]").prop("disabled", true);
        if (document.getElementById("btnRestore"))
            $("[id*=btnRestore]").prop("disabled", true);
        if (document.getElementById("divApprove")) {
            $("[id*=btnApproveUpdate]").prop("disabled", false);
            document.getElementById("divApprove").style.display = "none";
        }
        flagDisabled = true;
    } else {
        $("[id*=btnUpdate]").prop("disabled", false);
        $("[id*=btnDelete]").prop("disabled", false);
        if (document.getElementById("btnRestore"))
            $("[id*=btnRestore]").prop("disabled", false);
        if (document.getElementById("divReject")) {
            $("[id*=btnApproveUpdate]").prop("disabled", false);
            document.getElementById("divReject").style.display = "none";
        }
        flagDisabled = false;
    }

    if (is_Deleted) {
        if (document.getElementById("divDelete")) {
            document.getElementById("divDelete").style.display = "none";
        }
    } else {
        if (document.getElementById("divRestore")) {
            document.getElementById("divRestore").style.display = "none";
        }
    }

    disableAllField(flagDisabled);
}

function checkShowAllFieldNotDisable(is_Approved, is_Deleted) {
    if (is_Approved) {
        $("[id*=btnUpdate]").prop("disabled", true);
        $("[id*=btnDelete]").prop("disabled", true);
        $("[id*=btnRestore]").prop("disabled", true);
        if (document.getElementById("divApprove")) {
            $("[id*=btnApproveUpdate]").prop("disabled", false);
            document.getElementById("divApprove").style.display = "none";
        }
    } else {
        $("[id*=btnUpdate]").prop("disabled", false);
        $("[id*=btnDelete]").prop("disabled", false);
        $("[id*=btnRestore]").prop("disabled", false);
        if (document.getElementById("divReject")) {
            $("[id*=btnApproveUpdate]").prop("disabled", false);
            document.getElementById("divReject").style.display = "none";
        }
    }

    if (is_Deleted) {
        if (document.getElementById("divDelete")) {
            document.getElementById("divDelete").style.display = "none";
        }
    } else {
        if (document.getElementById("divRestore")) {
            document.getElementById("divRestore").style.display = "none";
        }
    }
}

function onLockFormEntryChanged() {
    $("#formEntry").find("input[type='text'],input[type='hidden'],input[type='number'],input[type='date'],input[type='email'],input[type='checkbox'], select, textarea").change(function () {
        lockApprove(true);
    });

    $('#formEntry .easyui-combobox').combobox({
        onChange: function (newValue, oldValue) {
            if (oldValue) {
                lockApprove(true);
            }
        }
    });
    $('#formEntry .easyui-combotree').combotree({
        onChange: function (newValue, oldValue) {
            if (oldValue) {
                lockApprove(true);
            }
        }
    });
}

function lockApprove(flagChanged) {
    if (document.getElementById("divApprove")) {
        $("[id*=btnApprove]").prop("disabled", flagChanged);
    }
    if (document.getElementById("divReject")) {
        $("[id*=btnReject]").prop("disabled", flagChanged);
    }
}

function getFloat(number) {
    var result = parseFloat(number);
    return isNaN(result) ? 0 : result;
}

function getInt(number) {
    var result = parseInt(number);
    return isNaN(result) ? 0 : result;
}

//get parameter from url
function GetURLParameter(sParam) {
    var sPageURL = window.location.search.substring(1);
    var sURLVariables = sPageURL.split('&');
    for (var i = 0; i < sURLVariables.length; i++) {
        var sParameterName = sURLVariables[i].split('=');
        if (sParameterName[0] === sParam) {
            return sParameterName[1];
        }
    }
}

//lock form
function disableAllForm(isDisabled) {
    //input[type='file']
    $(".modal.fade, #formEntry").find("input[type='text'],input[type='number'],input[type='date'],input[type='email'],input[type='checkbox'], select, textarea").prop("disabled", isDisabled);
    $('.modal.fade .easyui-combobox, #formEntry .easyui-combobox').combobox(isDisabled ? 'disable' : 'enable');
    $('.modal.fade .easyui-combotree, #formEntry .easyui-combotree').combotree(isDisabled ? 'disable' : 'enable');
    $('.modal.fade .l-btn, #formEntry .l-btn').linkbutton(isDisabled ? 'disable' : 'enable');
    //$(".easyui-fluid .datagrid-view .datagrid-body .datagrid-btable").css("pointer-events", isDisabled ? "none" : "auto");
    if (isDisabled) {
        $(".modal.fade .easyui-fluid .datagrid-view .datagrid-body, #formEntry .easyui-fluid .datagrid-view .datagrid-body").addClass("datagrid-disabled");
    } else {
        $(".modal.fade .easyui-fluid .datagrid-view .datagrid-body, #formEntry .easyui-fluid .datagrid-view .datagrid-body").removeClass("datagrid-disabled");
    }
    $(".modal.fade .btn, #formEntry .btn").prop("disabled", isDisabled);
}

function ACCToString(number) {
    var s = number.toString();
    var numberWords = ["không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín"];
    var layer = ["", "nghìn", "triệu", "tỷ"];

    var i, j, unit, dozen, hundred;
    var str = " ";
    var booAm = false;
    var decS = 0;

    try {
        decS = parseFloat(s);
    }
    catch (e) {
        // empty
    }
    if (decS < 0) {
        decS = -decS;
        s = decS.toString();
        booAm = true;
    }
    i = s.length;
    if (i === 0)
        str = numberWords[0] + str;
    else {
        j = 0;
        while (i > 0) {
            unit = parseInt(s.substr(i - 1, 1));
            i--;
            if (i > 0)
                dozen = parseInt(s.substr(i - 1, 1));
            else
                dozen = -1;
            i--;
            if (i > 0)
                hundred = parseInt(s.substr(i - 1, 1));
            else
                hundred = -1;
            i--;
            if ((unit > 0) || (dozen > 0) || (hundred > 0) || (j === 3))
                str = layer[j] + str;
            j++;
            if (j > 3) j = 1;
            if ((unit === 1) && (dozen > 1))
                str = "một " + str;
            else {
                if ((unit === 5) && (dozen > 0))
                    str = "lăm " + str;
                else if (unit > 0)
                    str = numberWords[unit] + " " + str;
            }
            if (dozen < 0)
                break;
            else {
                if ((dozen === 0) && (unit > 0)) str = "lẻ " + str;
                if (dozen === 1) str = "mười " + str;
                if (dozen > 1) str = numberWords[dozen] + " mươi " + str;
            }
            if (hundred < 0) break;
            else {
                if ((hundred > 0) || (dozen > 0) || (unit > 0)) str = numberWords[hundred] + " trăm " + str;
            }
            str = " " + str;
        }
    }
    if (booAm) str = "Âm " + str;
    return str + "đồng chẵn";
}

//End