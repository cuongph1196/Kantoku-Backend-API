// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//datepicker plugin
//link
$('.date-picker').datepicker({
    autoclose: true,
    todayHighlight: true,
    //changeYear: true,
    dateFormat: 'dd/mm/yy'
}).mask('99/99/9999');
//$(".date-picker").keyup(function (e) {
//    if (46 == e.keyCode || 8 == e.keyCode || 9 == e.keyCode) {
//        var $this = $(this);
//        if ($this.val() == "__/__/____")
//            $this.val("");
//    }
//});

$('.month_year').datepicker({
    format: 'mm/yy',
    viewMode: "months", //this
    minViewMode: "months",//and this
    autoClose: true
}).mask('99/9999');

$(".month_year").keyup(function (e) {
    if (46 == e.keyCode || 8 == e.keyCode || 9 == e.keyCode) {
        var $this = $(this);
        if ($this.val() == "__/____")
            $this.val("");
    }
});
//formatMoney
//$(".clsMoney").priceFormat({ prefix: '', centsLimit: 0 });
$('.clsMoney').priceFormat({
    prefix: '',
    thousandsSeparator: ',',
    clearPrefix: false,
    clearOnEmpty: true,
    centsLimit: 0
});

//Date range picker
$('#reservation').daterangepicker()
//Date range picker with time picker
$('#reservationtime').daterangepicker({
    timePicker: true,
    timePickerIncrement: 30,
    locale: {
        format: 'MM/DD/YYYY hh:mm A'
    }
})
//Date range as a button
$('#daterange-btn').daterangepicker(
    {
        ranges: {
            'Today': [moment(), moment()],
            'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
            'Last 30 Days': [moment().subtract(29, 'days'), moment()],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        },
        startDate: moment().subtract(29, 'days'),
        endDate: moment()
    },
    function (start, end) {
        $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'))
    }
)