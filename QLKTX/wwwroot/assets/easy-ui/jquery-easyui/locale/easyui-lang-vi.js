if ($.fn.pagination){
	$.fn.pagination.defaults.beforePageText = 'Trang';
	$.fn.pagination.defaults.afterPageText = 'của {pages}';
	$.fn.pagination.defaults.displayMsg = 'Hiển thị {from} tới {to} của {total} mục';
}
if ($.fn.datagrid){
	$.fn.datagrid.defaults.loadMsg = 'Đang xử lý, vui lòng chờ ...';
}
if ($.fn.treegrid && $.fn.datagrid){
	$.fn.treegrid.defaults.loadMsg = $.fn.datagrid.defaults.loadMsg;
}
if ($.messager){
	$.messager.defaults.ok = 'Ok';
	$.messager.defaults.cancel = 'Hủy';
}
$.map(['validatebox','textbox','passwordbox','filebox','searchbox',
		'combo','combobox','combogrid','combotree',
		'datebox','datetimebox','numberbox',
		'spinner','numberspinner','timespinner','datetimespinner'], function(plugin){
	if ($.fn[plugin]){
		$.fn[plugin].defaults.missingMessage = 'Trường này là bắt buộc.';
	}
});
if ($.fn.validatebox){
	$.fn.validatebox.defaults.rules.email.message = 'Vui lòng nhập địa chỉ email hợp lệ.';
	$.fn.validatebox.defaults.rules.url.message = 'Vui lòng nhập một URL hợp lệ.';
	$.fn.validatebox.defaults.rules.length.message = 'Vui lòng nhập giá trị từ {0} đến {1}.';
	$.fn.validatebox.defaults.rules.remote.message = 'Vui lòng sửa trường này.';
}
if ($.fn.calendar){
	$.fn.calendar.defaults.weeks = ['S','M','T','W','T','F','S'];
	$.fn.calendar.defaults.months = ['Th1', 'Th2', 'Th3', 'Th4', 'Th5', 'Th6', 'Th7', 'Th8', 'Th9', 'Th10', 'Th11', 'Th12'];
}
if ($.fn.datebox){
	$.fn.datebox.defaults.currentText = 'Hôm nay';
	$.fn.datebox.defaults.closeText = 'Đóng';
	$.fn.datebox.defaults.okText = 'Ok';
}
if ($.fn.datetimebox && $.fn.datebox){
	$.extend($.fn.datetimebox.defaults,{
		currentText: $.fn.datebox.defaults.currentText,
		closeText: $.fn.datebox.defaults.closeText,
		okText: $.fn.datebox.defaults.okText
	});
}
