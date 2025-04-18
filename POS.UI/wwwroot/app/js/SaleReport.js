var A = A || {};
A.SalesReport = {
    table: '',
    initDateTimePickers: function () {
        debugger
        $('.dpicker').each(function (i, e) {

            let name = $(e).attr('name');
            $(e).datepicker({
                format: 'mm/dd/yyyy',
                autoclose: true,
                todayHighlight: true,
                //container: container,
                clearBtn: true,
                todayBtn: 'linked',
                showOnFocus: false,
            });
        });
    },
} 
$(function () {
    A.SalesReport.initDateTimePickers();
    //A.SalesHistory.init();
    A.SalesReport.table = $('#saleReportTable').dataTable({
        "paging": true,
        "searching": true,
        "ordering": true,
        scrollX: true,
        scrollY: true,
        "dom": 'Bfrtip', // Position of the buttons
        "buttons": [
            {
                extend: 'excelHtml5',
                text: '<i class="fa fa-file-excel"></i> Export to Excel',
                className: 'btn btn-success btn-export',
                titleAttr: 'Export to Excel'
            }
        ]
    });
    

    // Handle Export
    //$('#btnExport').click(function () {
    //    table.button('.buttons-excel').trigger();
    //});

    //// Handle row count change
    //$('#rowCount').change(function () {
    //    table.page.len($(this).val()).draw();
    //});


    //$('#btnSearch').on('click', function () {
    //    A.SalesReport.search();
    //});

});