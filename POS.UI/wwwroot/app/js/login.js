$(document).ready(function () {
    $('#toggleIcon').click(function () {
        var passwordField = $('.pswdField').attr("type");
        var icon = $(this);
        debugger
        if (passwordField === 'password') {
            $('.pswdField').attr('type', 'text');
            icon.attr('src', '/assets/eye.png'); // Open eye icon
        } else {
            $('.pswdField').attr('type', 'password');
            icon.attr('src', '/assets/closed_Eye.png'); // Closed eye icon
        }
    });
});