$(function() {
    var loc2 = location.pathname.split("/")[2];
    var loc3 = location.pathname.split("/")[3];
    if (loc2 == null) {
        $('#Dashboard a').addClass('active');
    } else if (loc2 == "Patients") {
        $('#Patient a').addClass('active');
    } else if (loc2 == "Clinicians") {
        $('#Clinician a').addClass('active');
    } else if (loc2 == "Facility") {
        $('#Facility a').addClass('active');
    } else if (loc2 == "Home") {
        if (loc3 == "MyPatient") {
            $('#Patients a').addClass('active');
        } else if (loc3 == "Enquiry") {
            $('#Support a').addClass('active');
        }
    } else if (loc2 == "Account") {
        if (loc3 == "EditProfile") {
            $('#Profile a').addClass('active');
        } else if (loc3 == "ChangePassword") {
            $('#Settings a').addClass('active');
        }
    }
});



$(document).ready(function(e) {

    //burger bar event
    $('#burger-bar').click(function() {
        $('body').toggleClass('sm-menu');
    });

    var date = new Date();
    var today = new Date(date.getFullYear(), date.getMonth(), date.getDate());


    if ($("#show-calendar").length > 0) {
        $('#show-calendar').datepicker();
        $('#show-calendar').datepicker('setDate', today);
    }


    if ($('#pageWrapper').hasClass('compact-wrapper')) {
        $('.menu-title').click(function() {
            $('.menu-title').removeClass('active');
            $('.menu-content').slideUp('normal');
            if ($(this).next().is(':hidden') == true) {
                $(this).addClass('active');
                $(this).next().slideDown('normal');
            } else {}
        });
        $('.menu-content').hide();
        $('.submenu-title').click(function() {
            $(this).removeClass('active');
            $(this).next().slideUp('normal');
            if ($(this).next().is(':hidden') == true) {
                $(this).addClass('active');
                $(this).next().slideDown('normal');
            } else {}
        });
        $('.submenu-content').hide();
    }

    //jquery added for go to bottom
    $('.filter-sm-bar a').click(function() {
        $('body').toggleClass('nav-sm');
    });
    $('.fmenu-more').click(function() {
        $(this).next('ul').toggleClass('show');
        $('.footer-overlay').toggleClass('show');
    });
    //end

    //jquery added for go to bottom
    var scrollBottom = $(window).scrollTop() + $(document).height();
    $('.scroll-bottom').click(function() {
        $('html, body').animate({
                scrollTop: scrollBottom,
            },
            'slow'
        );
        return false;
    });
    //end

    //handle progress page carousel page each element height
    $(window).on('load resize', jqueryCarousel);
    //end

    // jquery added for nicescroll on left menu, respective functions are out of document.ready
    $(window).on('load resize', getNiceScroll);
    $(window).on('load resize', callCustomScrollbar);

    $('.nav-menu li a').click(function() {
        getNiceScrollResize();
    });
    //end

    $('.show-rightpanel').click(function() {
        $('.card-right').toggleClass('show-panel');
    });


    $('.open-search-box').click(function() {
        $(this).parent('.title').toggleClass('show-textbox');
    });

    $('.close-search').click(function() {
        $(this).parents('.title.show-textbox').removeClass('show-textbox');
    });

});







function getNiceScroll() {
    $('.navbar-scrollbar').getNiceScroll().remove();
    $('.navbar-scrollbar').niceScroll({
        touchbehavior: true,
        cursorcolor: 'rgba(42, 63, 84, 0.35)',
        autohidemode: true,
    });
    $('.navbar-scrollbar').getNiceScroll().resize();
}

function getNiceScrollResize() {
    setTimeout(function() {
        getNiceScroll();
    }, 500);
}


function callCustomScrollbar() {
    $('.custom-scrollbar').getNiceScroll().remove();
    $('.custom-scrollbar').niceScroll({
        touchbehavior: true,
        cursorcolor: 'rgba(42, 63, 84, 0.35)',
        autohidemode: true,
    });
    $('.custom-scrollbar').getNiceScroll().resize();
}





function jqueryCarousel() {
    if ($(".jcarousel").length > 0) {
        var jcarousel = $('.jcarousel');
        jcarousel

            .on('jcarousel:reload jcarousel:create', function() {
                var carousel = $(this),
                    width = carousel.innerWidth();
                if (width >= 600) {
                    width = width / 3;
                } else if (width >= 350) {
                    width = width / 2;
                }
                carousel.jcarousel('items').css('width', Math.ceil(width) + 'px');
            })
            .jcarousel({
                wrap: 'circular'
            })
            .jcarouselSwipe();
        $('.jcarousel-control-prev')
            .jcarouselControl({
                target: '-=1'
            });
        $('.jcarousel-control-next')
            .jcarouselControl({
                target: '+=1'
            });
        $('.jcarousel-pagination')
            .on('jcarouselpagination:active', 'a', function() {
                $(this).addClass('active');
            })
            .on('jcarouselpagination:inactive', 'a', function() {
                $(this).removeClass('active');
            })
            .on('click', function(e) {
                e.preventDefault();
            })
            .jcarouselPagination({
                perPage: 1,
                item: function(page) {
                    return '<a href="#' + page + '">' + page + '</a>';
                }
            });
        $('.jcarousel-control-next.opacity1').click(function() {
            $('.jcarousel-control-prev').addClass('active');
        });
    }
}

$(document).ready(function(e) {
    if ($(".table-sticky").length > 0) {
        getfloatHeight();
        $(window).resize(function() {
            getfloatHeight();
        });
    }
});

function getfloatHeight() {
    var windowHeight = $(window).height();
    var headerHeight = 0;
    var paddingSpace = 0;
    var pageHeading = parseInt($('.page-body-wrapper .page-heading').css('margin-bottom'), 10) + $('.page-body-wrapper .page-heading').outerHeight();
    if ($(window).width() > 991) {
        headerHeight = $('.page-main-header').innerHeight();
        paddingSpace = 70;
    } else {
        paddingSpace = 102;
    }
    $('.page-body-wrapper .table-card').css('height', windowHeight - (headerHeight + pageHeading + paddingSpace));
}