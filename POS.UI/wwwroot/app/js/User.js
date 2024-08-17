/*const { post } = require("jquery");*/

$(document).ready(function () {

    $('#userForm').on('submit', function (event) {
        debugger
        event.preventDefault(); //prevent the default form submission

        var formData = new FormData($('#userForm')[0]);

        $.ajax({
            url: '/User/RegisterUser', // URL of the action method
            type: 'POST',
            data: formData,
            processData: false, // Important: Prevent jQuery from processing the data
            contentType: false, // Important: Prevent jQuery from setting content type
            success: function (response) {
                if (response.success) {
                    console.log('Form submitted successfully');
                    // Handle success response
                    console.log('Form submitted successfully');
                    // Redirect to the Users action
                    window.location.href = '/User/Users';
                } else { 
                    // Clear previous errors
                    $('.spanErrorMsg').text('');

                    // Process and display errors
                    for (var key in response) {
                        if (response.hasOwnProperty(key)) {
                            var errorMsg = response[key];
                            /*$('#' + key + 'Error').text(errorMsg);*/
                            var errorid = '#' + key;
                            $(errorid).text(errorMsg);
                        }
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error('AJAX request failed:', textStatus, errorThrown);
            }
        });

    });

    $('#emailtxt').on('focusout', function () {
        var email = $('#emailtxt').val();
        $.ajax({
            url: '/User/CheckEmail',
            type: 'Post',
            data: JSON.stringify({ email: email }), // Serialize data to JSON
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (response) {
                if (!response.isValid) {
                    $('#EmailError').text(response.message);
                }
                else {
                    $('#EmailError').text("");
                }
            },
            error: function (err) {
                console.log(err.message)
            }
        });
    });

    $(document).on('click', '.edit-btn', function () {
        debugger
        var id = $(this).data('id'); // Retrieve the user ID from the data-id attribute
        // Now you can use userId to load user details or perform other actions
        //console.log('Edit user with ID:', userId);
        $.ajax({
            url: '/User/GetUserByID',
            type: 'Post',
            data: { id: id }, // Send data as query parameter
            //contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (response) {
                debugger
                if (!response.isValid) {
                    //$('#EmailError').text(response.message);
                    $('#nametxt').val(response.userName);
                    $('#emailtxt').val(response.email);
                    $('#phonetxt').val(response.phone);
                    $('#roletxt').val(response.role);
                    $('#passwordtxt').val(response.password);
                    $('#passwordtxt').prop("disabled", true);
                    if (response.state == true) {
                        $("#statetxt option[value=True]").attr('selected', true);
                    }
                    else {
                        $("#statetxt option[value=False]").attr('selected', true);
                    }
                    $("#userImage").attr("src", response.imagePath);
                    $('#exampleModal').modal('show');
                }
                else {
                    //$('#EmailError').text("");
                }
            },
            error: function (err) {
                debugger
                console.log(err.message)
            }
        })
    });

    $('#myTable').dataTable({
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
  

    $('#fileInput').on('change', function (event) {
        var output = document.getElementById('userImage');
        var file = event.target.files[0];

        if (file) {
            var objectURL = URL.createObjectURL(file);
            output.src = objectURL;

            output.onload = function () {
                URL.revokeObjectURL(objectURL); // Free memory after image has loaded
            }
        }
    });
    $('#addUserLinkBtn').on('click', function (e) {
        e.preventDefault(); // Prevent the default anchor tag behavior
        clearFields();
        $('#myModal').modal('show'); // Show the modal if needed
    });
    function clearFields() {
        $('#nametxt').val('');
        $('#emailtxt').val('');
        $('#phonetxt').val('');
        $('#roletxt').val('');
        $('#passwordtxt').val('');
        $('#statetxt').val('');
        $("#userImage").attr("src", '/assets/default_user_Image.png');
    }


});
