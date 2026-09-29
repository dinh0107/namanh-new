$(document).ready(function () {
    setTimeout(ClearAlert, 2000);

    function ClearAlert() {
        $("#AlertBox").slideUp(500);
        $(".validation-summary-errors").slideUp(500);
    };

    $(".form-check-label input").click(function (e) {
        var check = $(this).is(":checked");
        $(this).val(check);
    });
});


if ($(window).width() > 600) {
    $(".paragraph-justify").click(function () {
        $('body').toggleClass('sidebar-xs');
    });
}
if ($(window).width() <= 600) {
    $(".paragraph-justify").click(function () {
        $('.sidebar').toggleClass('active');
    });

    $(".my-accordion-header").click(function () {
        let content = $(this).next(".my-accordion-content");

        $(".my-accordion-content").not(content).slideUp();
        content.slideToggle();
    });
    $("#searchForm").removeClass("show")
}


$(".nav-item-submenu").click(function (e) {
    if (!$('body').hasClass('sidebar-xs')) {
        $(this).toggleClass('nav-item-open');
        $(this).find(".nav-group-sub").slideToggle();
    }
});
if ($("textarea.ckeditor").length && typeof CKEDITOR !== 'undefined') {
    CKEDITOR.timestamp = 'namanh2026';
    $("textarea.ckeditor").ckeditor();
}

function resetAnyFormById(formId) {
    formId = "#" + formId;
    $(formId).trigger("reset");

    $(formId).find(".field-validation-valid").empty();
}

$("[data-item=root]").on("change", function (data) {
    const id = $(this).val();
    var items = [];
    items.push("<option value>Danh mục cấp 2</option>");

    if (id !== "") {
        $.getJSON("/Product/GetChildCategory", { parentId: id }, function (data) {
            $.each(data,
                function (key, val) {
                    items.push("<option value='" + val.Id + "'>" + val.Name + "</option>");
                });
            $("[data-item=child]").html(items.join(""));
        });
    } else {
        $("[data-item=child]").html(items.join(""));
    }
});

$("[data-item=child]").on("change", function (data) {
    const id = $(this).val();
    var items = [];
    items.push("<option value>Danh mục cấp 3</option>");

    if (id !== "") {
        $.getJSON("/Product/GetThirdCategory", { parentId: id }, function (data) {
            $.each(data,
                function (key, val) {
                    items.push("<option value='" + val.Id + "'>" + val.Name + "</option>");
                });
            $("[data-item=third]").html(items.join(""));
        });
    } else {
        $("[data-item=third]").html(items.join(""));
    }
});

$("[data-item=third]").on("change", function (data) {
    const id = $(this).val();
    var items = [];
    items.push("<option value>Danh mục cấp 4</option>");

    if (id !== "") {
        $.getJSON("/Product/GetLastCategory", { parentId: id }, function (data) {
            $.each(data,
                function (key, val) {
                    items.push("<option value='" + val.Id + "'>" + val.Name + "</option>");
                });
            $("[data-item=last]").html(items.join(""));
        });
    } else {
        $("[data-item=last]").html(items.join(""));
    }
});

$("[data-item=root-article]").on("change", function (data) {
    const id = $(this).val();
    var items = [];
    items.push("<option value>Hãy chọn danh mục</option>");

    if (id !== "") {
        $.getJSON("/Article/GetChildCategory", { parentId: id }, function (data) {
            $.each(data,
                function (key, val) {
                    items.push("<option value='" + val.Id + "'>" + val.Name + "</option>");
                });
            $("[data-item=child-article]").html(items.join(""));
        });
    } else {
        $("[data-item=child-article]").html(items.join(""));
    }
});

$('.expands').click(function () {
    $(this).toggleClass('active');
    var id = $(this).parents('.t_row').data('id');
    $('.full-row-wrap[data-parent=' + id + ']').slideToggle();
});