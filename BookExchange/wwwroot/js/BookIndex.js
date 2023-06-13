'script'

$(document).ready(function () {
    $(".disabled").removeAttr("href")

    innerDrop();
})

function loadDefa() {
    var defaSrc = $("#defa").attr('src')
    $("#imgSrc").attr('src', defaSrc)
}

function innerDrop() {
    $(".innerDropdown").hover(function () {
        $(this).children("div").css("display", "block")
    }, function () {
        $(this).children("div").css("display", "none")
    })
};

