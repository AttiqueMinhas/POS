/*const { post } = require("jquery");*/

$(document).ready(function () {

    $('#userForm').on('submit', function (event) {
        debugger
        event.preventDefault(); //prevent the default form submission

        var formData = new FormData($('#userForm')[0]);
        var userId = $('#userId').val();
        // If no new image is selected, use the existing image path
        if (!$('#fileInput').val()) {
            formData.append('Image', $('#existingImagePath').val());
        }
        var url = userId > 0 ? '/User/EditUser' : '/User/RegisterUser'; // Choose URL based on action

        $.ajax({
            url: url, // URL of the action method
            type: 'POST',
            data: formData,
            processData: false, // Important: Prevent jQuery from processing the data
            contentType: false, // Important: Prevent jQuery from setting content type
            success: function (response) {
                debugger
                if (response.isValid) {
                    
                    $('#exampleModal').modal('hide');
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
    //Edit Functionality
    $(document).on('click', '.edit-btn', function () {
        debugger
        //setting up the Note Msg for Edit Modal
        $('#pswdNoteMsg').text('Note: Password cannot be modified from here.');
        var id = $(this).data('id'); // Retrieve the user ID from the data-id attribute
        $('#userId').val(id);
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
                if (response.isValid) {
                    //$('#EmailError').text(response.message);
                    $('#nametxt').val(response.userData.userName);
                    $('#emailtxt').val(response.userData.email);
                    $('#phonetxt').val(response.userData.phone);
                    $('#roletxt').val(response.userData.role);
                    $('#passwordtxt').val(response.userData.password);
                    $('#passwordtxt').prop("disabled", true);
                    if (response.userData.state == true) {
                        $("#statetxt option[value=True]").attr('selected', true);
                    }
                    else {
                        $("#statetxt option[value=False]").attr('selected', true);
                    }
                    $("#userImage").attr("src", response.userData.imagePath);
                    $('#existingImagePath').val(response.userData.imagePath); // Set the hidden field value
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
    //Delete User
    $(document).on('click', '.delete-btn', function () {
        debugger
        $('#delUserConfirmationModal').modal('show');
        var id = $(this).data('id'); // Retrieve the user ID from the data-id attribute
        //$('#userId').val(id);
        $('#delUserConfirmationModal button.ok').off().on('click', function () {
            debugger
            // close window
            // Now you can use userId to load user details or perform other actions
            //console.log('Edit user with ID:', userId);
            $.ajax({
                url: '/User/DeleteUser',
                type: 'Post',
                data: { id: id }, // Send data as query parameter
                //contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    debugger
                    if (response.isValid) {
                        console.log("User Deleted Successfully");
                        $('#modal-custom-confirmation').modal('hide');
                        /*location.reload();*/
                        window.location.href = '/User/Users';
                    }
                    else {
                        //$('#EmailError').text("");
                    }
                },
                error: function (err) {
                    debugger
                    console.log(err.message)
                }
            });

            // and callback
            callback(true);
        });

        $('#delUserConfirmationModal button.cancel').off().on('click', function () {
            // close window
            $('#modal-custom-confirmation').modal('hide');
            // callback
            callback(false);
        });
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
        $('#userId').val(0); // Reset to 0 for adding a new user
        resetFields();
        $('#exampleModal').modal('show'); // Show the modal if needed
    });
    function resetFields() {
        $('#nametxt').val('');
        $('#emailtxt').val('');
        $('#phonetxt').val('');
        $('#roletxt').val('');
        $('#passwordtxt').val('');
        $('#passwordtxt').prop("disabled", false);
        $('#statetxt').val('');
        $("#userImage").attr("src", '/assets/blank-profile-picture.png');
        $('#pswdNoteMsg').text('');
    }


});
