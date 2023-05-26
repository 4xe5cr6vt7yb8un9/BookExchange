'strict'

$(document).ready(function () {
    $('tr').hover(function () {
        $(this).find('a').css('color', 'white');
    }, function () {
        $(this).find('a').css('color', 'black');
        $('.rent').css('color', 'mediumblue');
    })
})
