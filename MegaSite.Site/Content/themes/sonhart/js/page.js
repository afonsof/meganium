var isLoaded = false;
var isMobile = false;
var $fullsize = jQuery('#fullsize');
var $fullsizetimer = jQuery('#fullsizeTimer');
var $superbgimage = jQuery('#superbgimage');
var $superbgimageplayer = jQuery('#superbgimageplayer');

if (navigator.platform == 'iPad' || navigator.platform == 'iPhone' || navigator.platform == 'iPod') {
    var isMobile = true;
}

jQuery(function ($) {

    // Options for SuperBGImage
    jQuery.fn.superbgimage.options = {
        transition: 1,
        vertical_center: 1,
        slideshow: 1,
        speed: 'normal', // animation speed
        randomimage: 0,
        preload: 1,
        slide_interval: 0, // invervall of animation
        onClick: false, // function-callback click image
        onHide: superbgimageHide, // function-callback hide image
        onShow: superbgimageShow // function-callback show image
    };

    // Show thumnails if option is activated
    jQuery('#fullsize a' + "[rel='" + jQuery.superbg_imgActual + "']").livequery(function () {
        var dataUrl = jQuery(this).attr('data-url');
        window.videoUrl = jQuery.parseJSON(dataUrl);

        if (window.videoUrl.type != "selfhosted" || window.videoUrl.type != "youtube_embed" || window.videoUrl.type != "vimeo_embed") {
            jQuery('#fullsize a' + "[rel='" + jQuery.superbg_imgActual + "']").expire();
        }
    });

    function superbgimageHide() {
        jQuery('#scanlines').css({ zIndex: 15 });
        jQuery('#fsg_playbutton').fadeOut();
        jQuery('#main, #page').addClass('zIndex').unbind('click');
        jQuery('#superbgimageplayer').removeClass();
        jQuery('#fsg_playbutton').add(jQuery('#main')).add(jQuery('#page')).unbind('click touchstart touchend');

        $fullsizetimer.stopTimer();
        jQuery('#fullsize a.activeslide').animate({ opacity: 0.5, top: 0 });

        jQuery('#superbgimageplayer, #superbgplayer').fadeOut(250);
        jQuery('#vimeoplayer, #selfhostedplayer').html('');
        jQuery('#superbgimage img.activeslide').fadeIn(250);

        // hide title
        jQuery('#showtitle').stop(false, true).animate({ marginBottom: 50, opacity: 0 }, 250, function () {
            jQuery(this).css({ marginBottom: 1 });
        });
    }

    function superbgimageShow(img) {

        jQuery('#superbgimage').css({ zIndex: 5 }).show();
        jQuery('#main, #page').addClass('zIndex').unbind('click');

        var dataUrl = "";
        window.videoUrl = {};

        // Show scanlines only if not in fullscreen mode
        if (jQuery('#expander').hasClass('slide-up')) {
            if (isMobile === false) {
                jQuery('#scanlines').show().stop(false, true).animate({ opacity: 1 }, 450);
            }
        }

        jQuery('#fullsize a' + "[rel='" + jQuery.superbg_imgActual + "']").livequery(function () {

            dataUrl = jQuery(this).attr('data-url');
            window.videoUrl = jQuery.parseJSON($('<div/>').html(dataUrl).text());

            // change the background color of the body
            if (window.videoUrl.backgroundcolor != "") {
                jQuery('body, #superbgimage, #superbgimageplayer ').stop().animate({ backgroundColor: window.videoUrl.backgroundcolor });
            } else {
                if (jQuery('body.white-theme').length) {
                    jQuery('body, #superbgimage, #superbgimageplayer').stop().animate({ backgroundColor: "#f5f5f5" });
                }
                if (jQuery('body.black-theme').length) {
                    jQuery('body, #superbgimage, #superbgimageplayer').stop().animate({ backgroundColor: "#222" });
                }
            }

            // add alt tag and ken burns to current fullsize gallery image
            jQuery('#superbgimage img.activeslide')
                        .attr('alt', window.videoUrl.excerpt);

            if (window.videoUrl.type == "selfhosted" || window.videoUrl.type == "youtube_embed" || window.videoUrl.type == "vimeo_embed") {

                jQuery('#fullsize').stopSlideShow();

                //jQuery('#superbgimageplayer').html('');

                $.getScript("js/post-video.js", function () {
                    jQuery('#superbgimageplayer, #superbgplayer').css({ display: 'block' });
                });

            } else {

                jQuery("#my-loading").add(jQuery('#fsg_playbutton')).fadeOut(150);

                if (jQuery('#expander').hasClass('slide-up')) {
                    if (isMobile === false) {
                        jQuery('#scanlines').show().stop(false, true).animate({ opacity: 1 }, 450);
                    }
                }

                if (jQuery.fn.superbgimage.options.slideshow == 1) {
                    jQuery.fn.superbgimage.options.slide_interval = 5000;
                    $fullsizetimer.startTimer(5000);
                    $fullsize.startSlideShow();
                }
            }

            jQuery('#fullsize a.activeslide').animate({ opacity: 1 });

            function checkTitleLeftMargin() {

                var sbHeight = jQuery('#branding').height() + jQuery('#sidebar').height();
                var bHeight = jQuery(window).height() - jQuery('#thumbnails').outerHeight() - jQuery('#showtitle').outerHeight() - jQuery('#colophon').outerHeight();

                if (sbHeight >= bHeight) {
                    jQuery('#showtitle').stop().animate({ left: 275 }, 200, 'easeOutQuad');
                } else {
                    jQuery('#showtitle').stop().animate({ left: 20 }, 200, 'easeOutQuad');
                }
            }
            checkTitleLeftMargin();

            jQuery(window).resize(function () {
                clearTimeout(this.id);
                this.id = setTimeout(checkTitleLeftMargin, 500);
            });

            // change title and show
            jQuery('#showtitle span.imagetitle').html(window.videoUrl.title);

            if (window.videoUrl.excerpt != "") {
                jQuery('#showtitle span.imagecaption')
                            .html(window.videoUrl.excerpt).show();
            } else {
                jQuery('#showtitle span.imagecaption').hide();
            }
            jQuery('#showtitle div a, #responsiveTitle a').attr('href', window.videoUrl.permalink).attr('target', window.videoUrl.target);
            jQuery('#showtitle .imagecount').html(img + '/' + jQuery.superbg_imgIndex);

            if (jQuery(window).width() >= 481) {
                jQuery('#showtitle').stop(false, true).show().animate({ opacity: 1 });
            }
        });
    }
    jQuery('body').addClass('fullsize-gallery');
});

jQuery(document).ready(function () {

    var isTouch = false;
    if (jQuery('html').hasClass('touch')) {
        isTouch = true;
    }

    if (jQuery().prettyPhoto) { // only load prettyPhoto if script file is included

        jQuery("a[data-rel^='prettyPhoto'], .gallery-icon a[href$='.jpg'], .gallery-icon a[href$='.png'], .gallery-icon a[href$='.gif']").livequery(function () {
            jQuery("a[data-rel^='prettyPhoto'], .gallery-icon a[href$='.jpg'], .gallery-icon a[href$='.png'], .gallery-icon a[href$='.gif']").prettyPhoto({
                hook: 'data-rel',
                allow_resize: true, /* Resize the photos bigger than viewport. true/false */
                allow_expand: true, /* Allow the user to expand a resized image. true/false */
                animationSpeed: 'normal',
                slideshow: 8000,
                theme: 'dark_rounded',
                deeplinking: false,
                callback: function () {
                    if (jQuery('.scroll-pane').size() > 0) {
                        jQuery('#scroll_left').not('#scroll_left.disabled').show();
                        jQuery('#scroll_right').not('#scroll_right.disabled').show();
                    }
                },
                show_title: true,
                overlay_gallery: true,
                social_tools: false
            });
        });

    }

    // fade in content parts
    var $hideParts = jQuery('#primary, #sidebar').not('.page-template-template-grid-fullsize-php #primary, .page-template-template-grid-fullsize-php #sidebar, .page-template-template-sortable-php #primary, .page-template-template-sortable-php #sidebar, .page-template-template-scroller-php #primary, .single-gallery #primary, .single-gallery #sidebar');

    if (!isTouch) {
        $hideParts.hide();
        $hideParts.fadeIn(450);
    } else {
        $hideParts.show();
    }

});
