function animUp() {
    $(".arrow").animate({
        top: "0"
    }, "slow", "swing", animDown);
}

function animDown() {

    $(".arrow").animate({

        top: "30px"

    }, "slow", "swing", animUp);

}


$(document).ready(function () {

    animUp();

});