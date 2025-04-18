// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



document.addEventListener('DOMContentLoaded', function () {
    // Handle parent menu clicks
    document.querySelectorAll('.menu-parent').forEach(parent => {
        const parentLink = parent.querySelector('.parent-link');
        const flyoutId = parent.getAttribute('data-menu-id');
        const flyout = document.getElementById(`flyout-${flyoutId}`);

        parentLink.addEventListener('click', function (e) {
            e.preventDefault();

            // Close all other flyouts
            document.querySelectorAll('.submenu-flyout').forEach(f => {
                f.classList.remove('active');
            });
            document.querySelectorAll('.menu-parent').forEach(p => {
                p.classList.remove('active');
            });

            // Toggle current flyout
            flyout.classList.toggle('active');
            parent.classList.toggle('active');
        });
    });

    // Close flyout when clicking outside
    document.addEventListener('click', function (e) {
        if (!e.target.closest('.menu-parent') && !e.target.closest('.submenu-flyout')) {
            document.querySelectorAll('.submenu-flyout').forEach(f => {
                f.classList.remove('active');
            });
            document.querySelectorAll('.menu-parent').forEach(p => {
                p.classList.remove('active');
            });
        }
    });


    document.querySelector('.user-profile').addEventListener('click', function (e) {
        e.stopPropagation();
        const dropdown = document.querySelector('.profile-dropdown');
        dropdown.style.display = dropdown.style.display === 'block' ? 'none' : 'block';
    });

    document.addEventListener('click', function (e) {
        if (!e.target.closest('.user-profile') && !e.target.closest('.profile-dropdown')) {
            document.querySelector('.profile-dropdown').style.display = 'none';
        }
    });
});

