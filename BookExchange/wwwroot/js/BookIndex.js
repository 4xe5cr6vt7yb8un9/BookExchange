'script'

$(document).ready(function () {
    $(".disabled").removeAttr("href")
    centerMain()

    $(window).resize(function () {
        centerMain()
    });

    function centerMain() {
        let main = document.querySelector('.main')
        let container = document.querySelector('.container')
        let marg = ((container.offsetHeight - 10) / 2 - main.offsetHeight / 2) + 'px'

        $(".main").css("top", marg)
    }
})