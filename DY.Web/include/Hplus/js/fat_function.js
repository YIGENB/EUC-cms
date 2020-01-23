$(function () {
    $(".fat_class").click(function (e) {
        if ($(".tree_class").is(":hidden"))
            $(".tree_class").show();
        else
            $(".tree_class").hide();

        e.stopPropagation();
    });
    $(document).click(function (event) {

        $(".tree_class").hide();
    });
});
