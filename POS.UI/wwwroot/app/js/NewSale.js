
var A = A || {};
A.NewSale = {
    Params: {
        productId: 0,
        productName: '',
        productBrand: '',
        productCategory: '',
        description: '',
        price: 0,
        quantity: 0,
        type: 0,
        subTotal: 0.0,
        totalTax: 0,
        total: 0,
        saleTotal: 0,
        typeDocumentSaleID: 0,
        customerDocument: 0
    },
    ProductList: [],
    isValid: function () {
        $('#saleType').val()
    }
} 

$(document).ready(function () {
    // Toggle dropdown visibility & rotate icon
    $("#dropdownTrigger").click(function () {
        //$("#dropdownContent").toggle();
        $("#dropdownContent").show()
        $("#dropdownIcon").toggleClass("rotate-180");
    });

    
    $('#searchProduct').on('input', function () {
        debugger
        var searchQuery = $(this).val();
        if (searchQuery.length >= 1) { // Only search if 2 or more characters are entered
            $.ajax({
                url: '/Sales/SearchProducts',
                type: 'GET',
                data: { query: searchQuery },
                success: function (data) {
                    debugger
                    $('#searchResults').empty();
                    data.forEach(function (product) {
                        debugger
                        A.NewSale.Params.productId = product.product_Id;
                        A.NewSale.Params.productName = product.product_Name;
                        A.NewSale.Params.price = product.price;
                        A.NewSale.Params.productBrand = product.brand;
                        A.NewSale.Params.productCategory = product.category_Name;
                        A.NewSale.Params.description = product.description;
                        $('#searchResults').append(`<a class="dropdown-item form-control" style="width:530px;" href="#" data-product-id="${product.product_Id}" data-product-name="${product.product_Name}" data-product-price="${product.price}"><div class="row"><div class="col-2 text-center p-0 pt-2"><img src="${product.iPath}" style="width:50px;height:50px;" /></div><div class="col-10 p-0 pt-2 lh-1"><p class="mb-2">${product.brand}</p><p class="mb-2">${product.product_Name}</p></div></div></a>`);
                    });
                    $('#searchResults').show();

                    
                }
            });
        } else {
            $('#searchResults').hide();
        }
    });

    // Handle product selection from dropdown
    $(document).on('click', '.dropdown-item', function () {
        debugger
        
        A.NewSale.Params.productId = $(this).data('product-id');
        A.NewSale.Params.productName = $(this).data('product-name');
        A.NewSale.Params.price = $(this).data('product-price');
        $('#modalTitle').text(A.NewSale.Params.productBrand);
        $('#txtDescription').text(A.NewSale.Params.description);
        $('#exampleModal').modal('show');
        

        // Clear search input and hide dropdown
        $('#searchProduct').val('');
        $('#searchResults').hide();

        // Recalculate totals
        //calculateTotals();
    });

    // Handle product removal
    $(document).on('click', '.remove-product', function () {
        $(this).closest('tr').remove();
        //calculateTotals();
    });

    // Handle quantity change
    $(document).on('click', '#setQuantity', function () {
        debugger
        //$('#exampleModal').modal('show');
        A.NewSale.Params.subTotal = 0.0;
        A.NewSale.Params.quantity = $('#txtQuantity').val();
        A.NewSale.Params.total = A.NewSale.Params.price * A.NewSale.Params.quantity;
        $('#exampleModal').modal('hide');
        // Add product to the table
        $('#productTableBody').append(`
                    <tr data-product-id="${A.NewSale.Params.productId}">
                        <td>${A.NewSale.Params.productName}</td>
                        <td>${A.NewSale.Params.quantity}</td>
                        <td class="price">${A.NewSale.Params.price}</td>
                        <td class="total">${A.NewSale.Params.total}</td>
                        <td><button class="btn btn-danger btn-sm remove-product"><i class="bi bi-trash"></i></button></td>
                    </tr>`
        );

        $('#productTableBody tr').each(function () {
            debugger
            A.NewSale.Params.subTotal += parseFloat($(this).find('.total').text());
        });

        debugger
        A.NewSale.Params.totalTax = A.NewSale.Params.subTotal * 0.18;

        A.NewSale.Params.saleTotal = A.NewSale.Params.subTotal + A.NewSale.Params.totalTax;

        $('#subTotal').val(A.NewSale.Params.subTotal.toFixed(2));
        $('#totalTaxes').val(A.NewSale.Params.totalTax.toFixed(2));
        $('#total').val(A.NewSale.Params.saleTotal.toFixed(2));

        //Inserting into list of products
        A.NewSale.ProductList.push(
            {
                ProductID: A.NewSale.Params.productId,
                ProductBrand: A.NewSale.Params.productBrand, 
                ProductName: A.NewSale.Params.productName,
                ProductCategory: A.NewSale.Params.productCategory,
                Quantity: A.NewSale.Params.quantity,
                Price: A.NewSale.Params.price,
                Total: A.NewSale.Params.total
            });
    });
    /*$(document).on('click', '#searchProduct', function () { console.log("Test") });*/
    $(document).on('submit', '.newSaleForm', function (e) {
        debugger
        e.preventDefault(); // Prevent default form submission

        // Check if the form is valid
        if (!$(this).valid()) {
            return; // Stop if validation fails
        }
        let request = {
            sale: {
                TypeDocumentSaleID: document.getElementById('saleType').value,
                CustomerDocument: document.getElementById('txtclientId').value,
                ClientName: document.getElementById('txtclientName').value,
                SubTotal: A.NewSale.Params.subTotal,
                Total: A.NewSale.Params.saleTotal,
                TotalTaxes: A.NewSale.Params.totalTax
            },
            productList: A.NewSale.ProductList
        };
        $.ajax({
            url: '/Sales/AddNewSale',
            type: 'post',
            data: JSON.stringify(request),
            contentType: 'application/json;charset=utf-8',
            dataType: 'json',
            success: function (response) {
                debugger
                console.log(response);
                Swal.fire({
                    title: "Registered!",
                    icon: "success",
                    text: `Sale Number: ${response.saleNumber}`,
                    draggable: false
                });
                document.getElementById('saleType').value = "";
                document.getElementById('txtclientId').value = "";
                document.getElementById('txtclientName').value = "";
                document.getElementById('subTotal').value = "";
                document.getElementById('totalTaxes').value = "";
                document.getElementById('total').value = "";
                $('#productTableBody').remove();
            },
            error: function (error) {
                debugger
                console.log(error);
            }
        });
    });
});