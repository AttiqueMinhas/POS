var A = A || {};
A.SalesHistory = {
    searchBy: '',
    table: '',
    Params: {
        RegistrationDate: '',
        SaleNumber: '',
        DocumentType: '',
        ClientDocument: '',
        ClientName: '',
        Total: 0.0,
        StartDate: '',
        EndDate: ''
    },
    ProductList: [],
    search: function () {
        debugger;
        let request;

        if (A.SalesHistory.searchBy == 'Dates') {
            // Get the raw input values
            let startDateStr = $('#txtStartDate').val(); // mm/dd/yyyy
            let endDateStr = $('#txtEndDate').val();     // mm/dd/yyyy

            // Convert to yyyy-MM-dd
            let startParts = startDateStr.split('/');
            let endParts = endDateStr.split('/');

            let formattedStartDate = `${startParts[2]}-${startParts[0].padStart(2, '0')}-${startParts[1].padStart(2, '0')}`;
            let formattedEndDate = `${endParts[2]}-${endParts[0].padStart(2, '0')}-${endParts[1].padStart(2, '0')}`;

            A.SalesHistory.StartDate = formattedStartDate;
            A.SalesHistory.EndDate = formattedEndDate;

            request = {
                startDate: A.SalesHistory.StartDate,
                endDate: A.SalesHistory.EndDate
            };
        } else if (A.SalesHistory.searchBy == 'Sale Number') {
            A.SalesHistory.SaleNumber = $('#txtSaleNumber').val();
            request = {
                SaleNumber: A.SalesHistory.SaleNumber
            };
        }

        $.ajax({
            url: '/Sales/SaleHistory',
            type: 'post',
            data: JSON.stringify(request),
            contentType: 'application/json;charset=utf-8',
            dataType: 'json',
            success: function (response) {
                debugger;
                let data = response.sale ? [response.sale] : response.sales;
                A.SalesHistory.table.clear().rows.add(data).draw();
            },
            error: function (error) {
                debugger;
                console.log(error);
            }
        });
    },
    getSaleBySaleNumber: function (SaleNumber) {
        debugger
        //let SaleNumber = $('#txtSaleNumber').val();
        //let SaleNumber = $('.view-sale').attr('data-id');
        let request = {
            SaleNumber: SaleNumber
        }
        
        debugger
        $.ajax({
            url: '/Sales/GetSaleBySaleNumber',
            type: 'post',
            data: JSON.stringify(request),
            contentType: 'application/json;charset=utf-8',
            dataType: 'json',
            success: function (response) {
                debugger;
                let productData = response.products;

                let fullDateTime = response.sale.createdAT;
                let dateOnly = fullDateTime.split('T')[0]; // Works if string is in "2024-07-10T15:42:30"

                if (!dateOnly) {
                    dateOnly = fullDateTime.split(' ')[0]; // Works if string is in "2024-07-10 15:42:30"
                }

                $('#txtRegDate').val(dateOnly);

                $('#txtDocType').val(response.sale.documentType);
                $('#txtSTotal').val(response.sale.subTotal);
                $('#txtSNumber').val(response.sale.saleNumber);
                $('#txtClntDoc').val(response.sale.customerDocument);
                $('#txtTaxess').val(response.sale.totalTaxes);
                $('#txtRegUser').val(response.sale.registerUser);
                $('#txtClntName').val(response.sale.clientName);
                $('#txtTotall').val(response.sale.total);

                A.SalesHistory.DetailSaleDataTable.clear().rows.add(productData).draw();
                // Show modal
                $('#viewSaleModal').modal('show');
            },
            error: function (error) {
                debugger;
                console.log(error);
            }
        });
    },

    generateSalePDF: function () {
        debugger
    // Initialize jsPDF
        const { jsPDF } = window.jspdf;
        const doc = new jsPDF();

        // Get all the data from the modal
        const regDate = $('#txtRegDate').val();
        const saleNumber = $('#txtSNumber').val();
        const regUser = $('#txtRegUser').val();
        const docType = $('#txtDocType').val();
        const clientDoc = $('#txtClntDoc').val();
        const clientName = $('#txtClntName').val();
        const subTotal = $('#txtSTotal').val();
        const taxes = $('#txtTaxess').val();
        const total = $('#txtTotall').val();

        // Get product data from the table
        const products = [];
        $('#saleProductDetailTable tbody tr').each(function () {
            products.push({
                name: $(this).find('td:eq(0)').text(),
                quantity: $(this).find('td:eq(1)').text(),
                price: $(this).find('td:eq(2)').text(),
                total: $(this).find('td:eq(3)').text()
            });
        });

        // Add store information
        doc.setFontSize(18);
        doc.text('Sample Store', 105, 15, { align: 'center' });
        doc.setFontSize(12);
        doc.text('Address: 2140 Smith Road', 105, 22, { align: 'center' });
        doc.text('Email: store@example.com', 105, 28, { align: 'center' });

        // Add sale header information
        doc.setFontSize(14);
        doc.text('Sale Detail', 14, 40);

        // Create a table for the header info
        doc.autoTable({
            startY: 45,
            head: [['Registration Date', 'Sale Number', 'Register User']],
            body: [[regDate, saleNumber, regUser]],
            theme: 'grid',
            headStyles: { fillColor: [220, 220, 220] }
        });

        doc.autoTable({
            startY: doc.lastAutoTable.finalY + 10,
            head: [['Document Type', 'Client Document', 'Client Name']],
            body: [[docType, clientDoc, clientName]],
            theme: 'grid',
            headStyles: { fillColor: [220, 220, 220] }
        });

        doc.autoTable({
            startY: doc.lastAutoTable.finalY + 10,
            head: [['Sub Total', 'Taxes', 'Total']],
            body: [[subTotal, taxes, total]],
            theme: 'grid',
            headStyles: { fillColor: [220, 220, 220] }
        });

        // Add products section
        doc.setFontSize(14);
        doc.text('Product', 14, doc.lastAutoTable.finalY + 15);

        // Prepare product data for the table
        const productRows = products.map(p => [p.name, p.quantity, p.price, p.total]);

        doc.autoTable({
            startY: doc.lastAutoTable.finalY + 20,
            head: [['Product', 'Quantity', 'Price', 'Total']],
            body: productRows,
            theme: 'grid',
            headStyles: { fillColor: [220, 220, 220] }
        });

        // Add footer with sale number and client info
        doc.setFontSize(12);
        doc.text('SALE NUMBER', 14, doc.lastAutoTable.finalY + 20);
        doc.text(saleNumber, 14, doc.lastAutoTable.finalY + 26);

        doc.text('CLIENT', 14, doc.lastAutoTable.finalY + 36);
        doc.text(clientName, 14, doc.lastAutoTable.finalY + 42);
        doc.text(clientDoc, 14, doc.lastAutoTable.finalY + 48);

        // Save the PDF
        doc.save(`Sale_${saleNumber}.pdf`);
    },
    //search: function () {
    //    debugger
    //    let request;
    //    if (A.SalesHistory.searchBy == 'Dates') {
    //        A.SalesHistory.StartDate = $('#txtStartDate').val();
    //        A.SalesHistory.EndDate = $('#txtEndDate').val();
    //        request = {
    //            startDate: A.SalesHistory.StartDate,
    //            endDate: A.SalesHistory.EndDate
    //        }
    //    }
    //    else if (A.SalesHistory.searchBy == 'Sale Number') {
    //        A.SalesHistory.SaleNumber = $('#txtSaleNumber').val();
    //        request = {
    //            SaleNumber: A.SalesHistory.SaleNumber
    //        }
    //    }
    //    debugger
    //    $.ajax({
    //        url: '/Sales/SaleHistory',
    //        type: 'post',
    //        data: JSON.stringify(request),
    //        contentType: 'application/json;charset=utf-8',
    //        dataType: 'json',
    //        success: function (response) {
    //            debugger
    //            let data = response.sale ? [response.sale] : response.sales;
    //            A.SalesHistory.table.clear().rows.add(data).draw();
    //        },
    //        error: function (error) {
    //            debugger
    //            console.log(error);
    //        }
    //    });
    //},
    initDateTimePickers: function () {
        debugger
        $('.dpicker').each(function (i, e) {
            //let container = $(e).parents('.input-group').attr('id')
            //    ? '#' + $(e).parents('.input-group').attr('id')
            //    : 'body'; // Default to 'body' if no id is present.

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
    init: function () {
        debugger
        //$('#datepicker').datepicker({
        //    format: 'mm/dd/yyyy', // Date format
        //    autoclose: true,      // Close when date is selected
        //    todayHighlight: true,   // Highlight today's date
        //    clearBtn: true,
        //    todayBtn: 'linked',
        //    showOnFocus: false
        //});

        $('#startdate').show();
        $('#labelStartDate').show();
        $('#enddate').show();
        $('#labelEndDate').show();
    }
}
$(function () {
    A.SalesHistory.initDateTimePickers();
    A.SalesHistory.init();

    A.SalesHistory.table = $('#mySaleHistoryTable').DataTable({
        paging: false,
        searching: false,
        info: false,
        lengthChange: false,
        ordering: false,
        columns: [
            {
                data: 'createdAT',
                render: function (data) {
                    if (!data) return '';
                    const date = new Date(data);
                    return date.toLocaleDateString(); // or format it however you want
                }
            },
            { data: 'saleNumber' },
            { data: 'documentType' },
            { data: 'customerDocument' },
            { data: 'clientName' },
            { data: 'total' },
            {
                data: null,
                render: function (data, type, row) {
                    return `
                    <button class="btn btn-sm btn-outline-primary view-sale" data-id="${row.saleNumber}" title="View">
                        <i class="fa-regular fa-eye"></i>
                    </button>
                `;
                },
                orderable: false
            }
        ],
        destroy: true // allow re-initialization
    });
    $(document).on('change', '#Searchby', function () {
        debugger
        A.SalesHistory.searchBy = $('#Searchby').val();
        if (A.SalesHistory.searchBy == 'Dates') {
            $('#saleNumber').hide();
            $('#labelSaleNumber').hide();
            $('#startdate').show();
            $('#labelStartDate').show();
            $('#enddate').show();
            $('#labelEndDate').show();


        }
        else if (A.SalesHistory.searchBy == 'Sale Number') {
            $('#saleNumber').show();
            $('#labelSaleNumber').show();
            //$('#startdate').hide();
            //$('#labelStartDate').hide();
            //$('#enddate').hide();
            //$('#labelEndDate').hide();


            $('#startdate').css('display', 'none');
            $('#labelStartDate').css('display', 'none');
            $('#enddate').css('display', 'none');
            $('#labelEndDate').css('display', 'none');
        }
    });

    $('#btnSearch').on('click', function () {
        A.SalesHistory.search();
    });


    //$(document).on('click', '.view-sale' , function () {
    //    A.SalesHistory.getSaleBySaleNumber();
    //});

    // Inside your document.ready or script
    $('#mySaleHistoryTable').on('click', '.view-sale', function () {
        let saleNumber = $(this).data('id'); // or .attr('data-id')

        // Now you can use saleNumber to make an AJAX call or pass it to your controller
        console.log("Clicked saleNumber:", saleNumber);

        // Example: Call your method
        A.SalesHistory.getSaleBySaleNumber(saleNumber);
    });


    //saleProductDetailTable
    A.SalesHistory.DetailSaleDataTable = $('#saleProductDetailTable').DataTable({
        paging: false,
        searching: false,
        info: false,
        lengthChange: false,
        ordering: false,
        columns: [
            { data: 'productName' },
            { data: 'quantity' },
            { data: 'price' },
            { data: 'total' }
        ],
        destroy: true // allow re-initialization
    });

    // Add this click handler after your modal show code
    //$('#btnPrintSale').on('click', function () {
    //    A.SalesHistory.generateSalePDF();
    //});

    $('#btnPrintSale').on('click', function () {
        // Get all the data from the modal
        const saleData = {
            saleNumber: $('#txtSNumber').val(),
            storeName: "Example Store",
            address: "Street: 2140 Smith Road",
            email: "store@example.com",
            clientName: $('#txtClntName').val(),
            clientDocument: $('#txtClntDoc').val(),
            subTotal: $('#txtSTotal').val(),
            taxes: $('#txtTaxess').val(),
            total: $('#txtTotall').val(),
            products: []
        };
        debugger
        // Get product data from the table
        $('#saleProductDetailTable tbody tr').each(function () {
            saleData.products.push({
                Name: $(this).find('td:eq(0)').text(),
                Quantity: parseInt($(this).find('td:eq(1)').text()),
                Price: parseFloat($(this).find('td:eq(2)').text().replace(/[^0-9.]/g, '')),
                Total: parseFloat($(this).find('td:eq(3)').text().replace(/[^0-9.]/g, ''))
            });
        });

        // Call the server to generate PDF
        $.ajax({
            url: '/Sales/GenerateReceiptPdf',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(saleData),
            xhrFields: {
                responseType: 'blob'
            },
            success: function (response) {
                // Open PDF in new tab
                var blob = new Blob([response], { type: 'application/pdf' });
                var url = URL.createObjectURL(blob);
                window.open(url, '_blank');
            },
            error: function (error) {
                console.error('Error generating PDF:', error);
                alert('Error generating PDF. Please try again.');
            }
        });
    });


});