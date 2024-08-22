/*const { post } = require("jquery");*/

$(document).ready(function () {
    $('#myCategoryTable').dataTable({
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
});
