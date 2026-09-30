AOS.init({
    once: true,
});
$('.button-wrap').on("click", function () {
    $(this).toggleClass('button-active');
    $(".toDate").toggleClass('input-active');
});

if ($('.slider-thuml').length) {
    $('.slider-thuml').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        arrows: false,
        fade: true,
        asNavFor: '.slider-nav'
    });

    if ($('.slider-nav').length) {
        $('.slider-nav').slick({
            slidesToShow: 5,
            arrows: false,
            slidesToScroll: 1,
            asNavFor: '.slider-thuml',
            focusOnSelect: true,
            responsive: [
                {
                    breakpoint: 900,
                    settings: {
                        dots: false,
                        slidesToShow: 3
                    }
                },
                {
                    breakpoint: 768,
                    settings: {
                        centerPadding: '0',
                        dots: false,
                        slidesToShow: 3
                    }
                },
                {
                    breakpoint: 480,
                    settings: {
                        centerPadding: '0', dots: false,
                        slidesToShow: 3
                    }
                }
            ]
        });
    }
}


if ($('.price-slick').length) {
    $('.price-slick').slick({
        slidesToShow: 3,
        arrows: false,
        slidesToScroll: 1,
        centerMode: true,
        centerPadding: '0',
        responsive: [
            {
                breakpoint: 900,
                settings: {
                    dots: false,
                    slidesToShow: 3
                }
            },
            {
                breakpoint: 768,
                settings: {
                    centerPadding: '0',
                    dots: false,
                    slidesToShow: 3
                }
            },
            {
                breakpoint: 480,
                settings: {
                    centerPadding: '0', dots: false,
                    slidesToShow: 1
                }
            }
        ]
    });
}







function homeJs() {

    $('.service-slide').slick({
        centerMode: true,
        centerPadding: '0',
        slidesToShow: 3,
        infinite: true,
        dots: true,
        autoplay: false,
        autoplaySpeed: 3000,
        prevArrow: "<button type='button' aria-label='bên trái' class='slick-prev pull-left'><i class='fa fa-angle-left' aria-hidden='true'></i></button>",
        nextArrow: "<button type='button'aria-label='bên phải' class='slick-next pull-right'><i class='fa fa-angle-right' aria-hidden='true'></i></button>",

        responsive: [
            {
                breakpoint: 900,
                settings: {
                    arrows: true,
                    centerMode: false,
                    dots: false,
                    slidesToShow: 3
                }
            },
            {
                breakpoint: 768,
                settings: {
                    arrows: true,
                    centerMode: true,
                    centerPadding: '0',
                    dots: false,
                    slidesToShow: 1
                }
            },
            {
                breakpoint: 480,
                settings: {
                    arrows: true,
                    centerMode: true,
                    centerPadding: '0', dots: false,
                    slidesToShow: 1
                }
            }
        ]
    });
    function toggleAccordion(button) {
        button.classList.toggle("active");
        const content = button.nextElementSibling;
        if (content.style.display === "block") {
            content.style.display = "none";
        } else {
            content.style.display = "block";
        }
    }


    $('.fb-slider').slick({
        slidesToShow: 1,
        infinite: true,
        dots: false,
        autoplay: true,
        autoplaySpeed: 3000,
        prevArrow: "<button type='button' aria-label='bên trái' class='slick-prev pull-left'><i class='fa fa-angle-left' aria-hidden='true'></i></button>",
        nextArrow: "<button type='button'aria-label='bên phải' class='slick-next pull-right'><i class='fa fa-angle-right' aria-hidden='true'></i></button>",
    });
    $('.list-car').slick({
        slidesToShow: 3,
        infinite: true,
        dots: true,
        autoplay: true,
        autoplaySpeed: 3000,
        prevArrow: "<button type='button' aria-label='bên trái' class='slick-prev pull-left'><i class='fa fa-angle-left' aria-hidden='true'></i></button>",
        nextArrow: "<button type='button'aria-label='bên phải' class='slick-next pull-right'><i class='fa fa-angle-right' aria-hidden='true'></i></button>",

        responsive: [
            {
                breakpoint: 768,
                settings: {
                    slidesToShow: 2,
                    dots: false,
                    arrows: true,
                }
            },
            {
                breakpoint: 480,
                settings: {
                    slidesToShow: 1,
                    dots: false,
                    arrows: true,
                }
            }
        ]
    });

    var rows = $("#service-tb tbody tr");
    var showCount = 5;
    rows.slice(showCount).hide();
    $("#show-call-table").removeClass("active");
    $("#show-more-table").click(function () {
        rows.show();
        $(this).hide();
        $("#show-call-table").addClass("active");
    });





}

function show() {
    $(".menu-drawer").addClass("open");
    $(".menu-overlay").addClass("active");
}
function Close() {
    toggleMenu();
}
function toggleMenu() {
    $(".menu-overlay").removeClass("active");
    $(".menu-drawer").removeClass("open");
}

$(document).ready(function () {
    $(".menu-btn").on("click", function () {
        $(".menu-drawer").addClass("open");
        $(".menu-overlay").addClass("active");
    });

    $(".menu-overlay").on("click", function () {
        toggleMenu();
    });

    $(".expand-bar").on("click", function () {
        $(this).toggleClass("open");
        $(this).parent().toggleClass("open");
    });
});
function productDetail() {
    $('.list-car').slick({
        slidesToShow: 3,
        infinite: true,
        dots: true,
        autoplay: true,
        autoplaySpeed: 3000,
        prevArrow: "<button type='button' aria-label='bên trái' class='slick-prev pull-left'><i class='fa fa-angle-left' aria-hidden='true'></i></button>",
        nextArrow: "<button type='button'aria-label='bên phải' class='slick-next pull-right'><i class='fa fa-angle-right' aria-hidden='true'></i></button>",
        responsive: [
            {
                breakpoint: 768,
                settings: {
                    slidesToShow: 2,
                    dots: false,
                    arrows: true,
                }
            },
            {
                breakpoint: 480,
                settings: {
                    slidesToShow: 1,
                    dots: false,
                    arrows: true,
                }
            }
        ]
    });
}
$(".view-all").click(function () {
    let $carBody = $(".car-body");
    let $btn = $(this);

    $carBody.toggleClass("active");

    if ($carBody.hasClass("active")) {
        $btn.text("Ẩn bớt");
    } else {
        $btn.text("Xem thêm");
    }
});

$(function () {
    $(".contact-part").on("submit", function (e) {
        e.preventDefault();
        if ($(this).valid()) {
            $.post("/Home/ContactForm", $(this).serialize(), function (data) {
                if (data.status) {
                    $.toast({
                        heading: 'Liên hệ đặt xe thành công',
                        text: data.msg,
                        icon: 'success',
                        position: "bottom-right"
                    })
                    $(".contact-part").trigger("reset");
                } else {
                    $.toast({
                        heading: 'Liên hệ không thành công',
                        text: data.msg,
                        icon: 'error',
                        position: "bottom-right"
                    })
                }
            });
        }
    });
});


(function () {
    const names = ["Ng Trang", "Minh Khoa", "Bảo Vy", "Tuấn Anh", "Hồng Nhung", "Thanh Hà"];
    const phones = ["039", "086", "090", "097", "089", "032", "034"];
    const destinations = ["Nội Bài", "Mỹ Đình", "Hải Phòng", "Hạ Long", "Ninh Bình"];

    const item = document.getElementById("seedItem");
    if (!item) return;
    let idx = 0;
    const SHOW_TIME = 6000; // thời gian hiển thị mỗi item
    const GAP_TIME = 4000;   // khoảng nghỉ giữa 2 item (ms)

    function randomPhone() {
        const head = phones[Math.floor(Math.random() * phones.length)];
        const end = Math.floor(100 + Math.random() * 900);
        return `${head}.xxx.${end}`;
    }

    function showOnce() {
        item.classList.remove("show"); // ẩn

        setTimeout(() => { // CHỜ 400ms rồi mới hiện item mới
            const name = names[Math.floor(Math.random() * names.length)];
            const dest = destinations[Math.floor(Math.random() * destinations.length)];
            const phone = randomPhone();

            item.innerHTML = `Khách hàng <b>${name}</b> (${phone}) vừa đặt xe đi <b>${dest}</b>`;
            requestAnimationFrame(() => item.classList.add("show"));
        }, GAP_TIME);

    }

    showOnce();
    setInterval(showOnce, SHOW_TIME + GAP_TIME);
})();
if ($.fn.fancybox) {
    $('[data-fancybox]').fancybox({
        caption: ''
    });
}