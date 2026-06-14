AOS.init({
    once: true,
});
$('.button-wrap').on("click", function () {
    $(this).toggleClass('button-active');
    $(".toDate").toggleClass('input-active');
});

$('.slider-thuml').slick({
    slidesToShow: 1,
    slidesToScroll: 1,
    arrows: false,
    fade: true,
    asNavFor: '.slider-nav'
});

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



function autoComplate() {
    const API_KEY = "zxjFUlokomoYCcC9EzXHKSwXml4tYSafvdwJ6Qgn";
    const lat = null;
    const lng = null;
    const radius = 50000;
    let fromLatLng = null;
    let toLatLng = null;
    let isOutProvince = false;

    $(document).on("click", ".form-title", function () {
        $(".form-title").removeClass("active");
        $(this).addClass("active");

        if ($(this).find("i").hasClass("fa-road")) {
            isOutProvince = true;
            $("#diemDen").val("");
            toLatLng = null;
            console.log("Chế độ: Đi tỉnh");
        } else {
            isOutProvince = false;
            console.log("Chế độ: Sân bay");
            setDefaultDestination();
        }
    });

    function setDefaultDestination() {
        const defaultDescription = "Sân bay Nội Bài";
        $("#diemDen").val(defaultDescription);

        $.getJSON(`https://rsapi.goong.io/Place/AutoComplete?api_key=${API_KEY}&input=${encodeURIComponent(defaultDescription)}&location=${lat},${lng}`, function (data) {
            if (data.predictions && data.predictions.length > 0) {
                const placeId = data.predictions[0].place_id;
                $.getJSON(`https://rsapi.goong.io/Place/Detail?place_id=${placeId}&api_key=${API_KEY}`, function (res) {
                    const loc = res.result.geometry.location;
                    toLatLng = loc.lat + "," + loc.lng;
                });
            }
        });
    }

    setDefaultDestination();

    let debounceTimer = null;

    $(document).on("input", ".autocomplete-input", function () {
        const $input = $(this);
        const keyword = $input.val().trim();
        const $container = $input.closest(".autocomplete-container");
        const $list = $container.find(".autocomplete-list");

        clearTimeout(debounceTimer);
        $list.empty().hide();

        if (keyword.length < 3) return;

        debounceTimer = setTimeout(() => {
            let url = isOutProvince
                ? `https://rsapi.goong.io/Place/AutoComplete?api_key=${API_KEY}&input=${encodeURIComponent(keyword)}`
                : `https://rsapi.goong.io/Place/AutoComplete?api_key=${API_KEY}&input=${encodeURIComponent(keyword)}&location=${lat},${lng}`;

            $.getJSON(url, function (data) {
                $.each(data.predictions, function (i, item) {
                    const $div = $("<div>", { class: "autocomplete-item" });
                    const $icon = $("<img>", {
                        class: "autocomplete-icon",
                        src: "https://cdn-icons-png.flaticon.com/512/1865/1865269.png"
                    });
                    const $textWrap = $("<div>", { class: "autocomplete-text" });
                    const $primary = $("<div>", { class: "primary" }).text(item.structured_formatting.main_text || item.description);
                    const $secondary = $("<div>", { class: "secondary" }).text(item.structured_formatting.secondary_text || "");
                    $textWrap.append($primary, $secondary);
                    $div.append($icon, $textWrap);

                    $div.on("click", function () {
                        selectPlace(item.place_id, item.description, $input, $list);
                    });

                    $list.append($div);
                });
                $list.show();
            });

        }, 300); // delay 300ms chống spam API
    });

    function selectPlace(placeId, description, $input, $list) {
        $list.empty().hide();
        $input.val(description);

        $.getJSON(`https://rsapi.goong.io/Place/Detail?place_id=${placeId}&api_key=${API_KEY}`, function (data) {
            const loc = data.result.geometry.location;
            const latlng = loc.lat + "," + loc.lng;

            if ($input.attr('id') === 'diemDi') fromLatLng = latlng;
            if ($input.attr('id') === 'diemDen') toLatLng = latlng;

            if (fromLatLng && toLatLng) {
                getDistance(fromLatLng, toLatLng);
            }
        });
    }

    function getDistance(origin, destination) {
        $.getJSON(`https://rsapi.goong.io/Direction?origin=${origin}&destination=${destination}&vehicle=car&api_key=${API_KEY}`, function (res) {
            if (res.routes && res.routes.length > 0) {
                console.log("Khoảng cách:", res.routes[0].legs[0].distance.text);
            }
        });
    }
}

$(document).ready(autoComplate);

//document.addEventListener("DOMContentLoaded", function () {
//    const marquees = document.querySelectorAll(".marquee-item");

//    marquees.forEach((marquee, index) => {
//        marquee.innerHTML += marquee.innerHTML;
//        const height = marquee.scrollHeight / 2;

//        gsap.to(marquee, {
//            y: -height,
//            duration: index === 0 ? 7 : 5,
//            ease: "linear",
//            repeat: -1
//        });
//    });
//});

function homeJs() {
    
    
    //document.addEventListener('DOMContentLoaded', function () {
    //    Fancybox.bind("[data-fancybox]", { Thumbs: { autoStart: true } });
    //});
    //$('.slider-for').slick({
    //    slidesToShow: 1,
    //    autoplay: true,
    //    autoplaySpeed: 2000,
    //    arrows: false,
    //    asNavFor: '.slider-nav'
    //})
    //$('.slider-nav').slick({
    //    slidesToShow: 4,
    //    arrows: false,
    //    slidesToScroll: 1,
    //    asNavFor: '.slider-for',
    //    focusOnSelect: true
    //});
    //$('.list-baner').slick({
    //    slidesToShow: 1,
    //    autoplay: true,
    //    autoplaySpeed: 3000,
    //    arrows: false,
    //})

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


    
 

    initPriceRoutes();
}

function initPriceRoutes() {
    var $section = $('.price-routes-section');
    if (!$section.length) {
        return;
    }

    $section.on('click', '.price-route-tab', function () {
        var tabId = $(this).data('tab');
        $section.find('.price-route-tab').removeClass('active');
        $(this).addClass('active');
        $section.find('.price-route-panel').removeClass('active');
        $section.find('.price-route-panel[data-panel="' + tabId + '"]').addClass('active');
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
    $(window).on("scroll", function () {
        if ($(this).scrollTop() > 200) {
            $("#header-sticky").addClass("active");
        } else {
            $("#header-sticky").removeClass("active");
        }
    });

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
$('[data-fancybox]').fancybox({
    caption: ''
});