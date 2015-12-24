
    window.setTimeout(function () {
        $(".alert-success").fadeTo(400, 0).slideUp(500, function () {
            $(this).remove();
        });
        $(".alert-danger").fadeTo(400, 0).slideUp(500, function () {
            $(this).remove();
        });
    }, 5000);
 