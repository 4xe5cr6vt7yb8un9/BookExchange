'strict'

$(document).ready(function () {
    let windowWidth = window.innerWidth
    let windowHeight = window.innerHeight

    let menu = document.querySelector('#c1')

    positionMenu();

    function positionMenu() {
        const menuWidth = menu.offsetWidth + 4
        //menuHeight = textMenu.offsetHeight + 4

        //textMenu.style.top = windowHeight / 2 - menuHeight / 2 + 'px'
        menu.style.left = windowWidth / 2 - menuWidth / 2 + 'px'
    }

    $(window).resize(function () {
        windowWidth = window.innerWidth
        windowHeight = window.innerHeight

        positionMenu();
    });
})