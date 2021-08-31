// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    var scroll_start = 0;
    var startchange = $('#startchange');
    var offset = startchange.offset();
    if (startchange.length) {
        $(document).scroll(function () {
            scroll_start = $(document).scrollTop();
            if (scroll_start > offset.top) {
                //console.log('scroll!')
                $(".navbar").css('background-color', '#333');
            } else {
                //console.log('other-scroll!')
                $('.navbar').css('background-color', 'transparent');
            }
        });
    }

    $('#navLinks').find("active").removeClass("active");
    var currentRoute = window.location.href;
    if (/Privacy/.test(currentRoute)) {
        $('#privacylink').addClass('active');
    }
    else {
        $('#homelink').addClass('active');
    }
    console.log("current location is " + currentRoute);

    /*var navBar = document.getElementById("navLinks");
    var navlinks = navBar.getElementsByClassName("nav-link");
    for (var i = 0; i < navlinks.length; i++) {
        console.log("got nav item " + i);
    }
    var currentlyActive = document.getElementsByClassName("active")[0];
    currentlyActive.className = currentlyActive.className.replace(" active", "");
    if (currentRoute == "https://" + window.location.host + "/") {
        //set home as active
    }
    else if (currentroute.split("/")[currentRoute.split("/").length - 1].toLowerCase() == "privacy") {
        //set privacy policy as active
    }*/
});

// Add slideDown animation to Bootstrap dropdown when expanding.
$('.dropdown').on('open.bs.dropdown', function () {
    console.log('dropdown!');
    $(this).find('.dropdown-menu').first().stop(true, true).slideDown();
});

// Add slideUp animation to Bootstrap dropdown when collapsing.
$('.dropdown').on('close.bs.dropdown', function () {
    console.log('dropUp!');
    $(this).find('.dropdown-menu').first().stop(true, true).slideUp();
});

$('.dropdown').on('click', function () {
    $(this).find('.dropdown-menu').first().stop(true, true).slideDown();
    console.log('it was clicked?');
})

$()