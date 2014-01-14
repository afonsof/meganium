/*-----------------------------------------------------------------------------------
 	My Custom JS for SuperBgImage Script Wordpress Theme
-----------------------------------------------------------------------------------*/
/*
	1. =General Settings
*/

// Init jQuery on page load
jQuery(document).ready(function($) {

	// check for touchable device
	var isTouch =  false,
		click_event = 'click';

	if( jQuery('html').hasClass('touch') ){
		isTouch = true,
		click_event = 'click'; // change click event for play button to touchstart
	}

	//defining some global variables
	$fullsize      = jQuery('#fullsize');
	$scrollerItems = jQuery('#fullsize .item');
	$cntItems      = $scrollerItems.length;
	$fullsizeTimer = jQuery('#fullsizeTimer');
	$fullsizeStart = jQuery('#fullsizeStart');
	$fullsizeStop  = jQuery('#fullsizeStop');
	$togthumbs     = jQuery('#toggleThumbnails');

/*-----------------------------------------------------------------------------------*/
/*	1. =General Settings
/*-----------------------------------------------------------------------------------*/

	jQuery.fn.stopTimer = function() {
		jQuery(this).stop().width(0);
		$fullsizeStart.show();
		$fullsizeStop.hide();
	}

	jQuery.fn.startTimer = function( timer, resume ) {

		var elem = jQuery(this);

		if(resume){

			var left_duration = timer * ( jQuery(window).width() - elem.width() ) / jQuery(window).width();

			jQuery.fn.superbgimage.options.slide_interval = left_duration;

			elem.stop(true).animate({
				width:  jQuery(window).width()
			}, {
				duration: left_duration,
				specialEasing: {
					width: "linear"
				}
			});

			$fullsizeStart.hide();
			$fullsizeStop.show();

		}else{

			if( jQuery.fn.superbgimage.options.slideshow == 1 || ( jQuery('#superbgimageplayer').hasClass('jwplayer_init') && jwplayer('superbgimageplayer').getState() == "PLAYING" ) ) {

				$actItem = jQuery('#fullsize .item.activeslide');
				$ind = 	jQuery('a.activeslide').index("#fullsize .item") + 1;

				if( $ind == 1){
					$fullsize.animate({ marginLeft: 0 });
				}

				elem.css({ width: 0 }).stop(true).animate({
					width:  jQuery(window).width()
				}, {
					duration: timer,
					specialEasing: {
						width: "linear"
					}
				});
				$fullsizeStart.hide();
				$fullsizeStop.show();

			}

		}

	};

	jQuery.fn.pauseTimer = function( timer ){
		jQuery(this).stop(true);
		$fullsizeStart.show();
		$fullsizeStop.hide();
	}

	// set opacity of thumbnails
	jQuery( '#fullsize a' ).animate({ opacity: 0.5, top: 0 });

	// set position of controls
	if(jQuery('#thumbnails').hasClass('centered')){
		jQuery('#thumbnails .controls').css({ right: 50 + "%", marginRight: -jQuery('#thumbnails .controls').outerWidth() / 2 });
	}

	// add hover effect to the thumbnails
	jQuery('#fullsize').on('hover', 'a', function(e){

		var thumbnail_link = jQuery(this);

		if(e.type == "mouseenter") {

			if( thumbnail_link.hasClass('greyscaled') ){
				thumbnail_link
					.find('img.img-color')
						.stop(false,true)
						.fadeIn(250, function(){
							jQuery(this)
								.find('img.img-grey')
								.css({ visibility: 'hidden' })
						})
			}

			thumbnail_link
				.find('.overlay')
					.show()
					.stop(false,true)
					.animate({ opacity: 0.7},250)
				.end()
				.not('.activeslide')
					.stop()
					.animate({ opacity: 1 })

		}else if (e.type == "mouseleave") {

			if( thumbnail_link.hasClass('greyscaled') ){
				thumbnail_link
					.find('img.img-grey')
						.css({ visibility: 'visible' })
					.end()
					.find('img.img-color')
						.stop(false,true)
						.fadeOut(250)
			}

			thumbnail_link
				.find('.overlay')
					.stop(false, true)
					.animate({ opacity: 0},250)
				.end()
				.not('.activeslide')
					.stop()
					.animate({ opacity: 0.5 });
		}
	})

	// calculate container width on window load
	$contWidth = 0;

	jQuery(window).load( function() {

		var thumbs_array = jQuery('#thumbnailContainer').find('a');

		thumbs_array.each(function() {
			$contWidth = $contWidth + jQuery(this).outerWidth(true);
		})

 		if( $contWidth > jQuery( '#thumbnails .rel' ).width() ){
			jQuery( '#thumbnails .pulldown-items' ).width( $contWidth );
			jQuery( '#thumbnails .scroll-link' ).fadeIn();
		}

		// reset opacity and visibility
		var anim_height = jQuery('#thumbnails').outerHeight( true ) + jQuery('#colophon').outerHeight( true );

		jQuery( '#thumbnails' ).css('padding-top', jQuery('#thumbnails .controls').outerHeight() );

		//scrollpane parts
		var scrollPane = jQuery( "#thumbnailContainer" ),
			scrollContent = $fullsize;

		// set height of containers
		jQuery('#thumbnails, #thumbnails .rel').css({
  		height: jQuery('#fullsize a:first img').outerHeight() + parseInt(jQuery('#fullsize').css('margin-top')) * 2
		})

		setScrollerWidth();

		var slide_handler = function(e, ui) {
			if ( scrollContent.width() > scrollPane.width() ) {
				scrollContent.css( "margin-left", Math.round(
					ui.value / 100 * ( scrollPane.width() - scrollContent.width() )
				) + "px" );
			} else {
				scrollContent.css( "margin-left", 0 );
			}
		};

		//build slider
		var scrollbar = jQuery( ".scroll-bar" ).slider({
			slide: slide_handler,
			change: slide_handler
		});

		jQuery('.scroll-content-item:last').css({marginRight: 0});

		if(!isTouch){

  		if( !jQuery('#thumbnails').hasClass('mouse-scrub') ){
  			// Mousewheel plugin
  			scrollPane.mousewheel(function(event, delta) {
  				var value = scrollbar.slider('option', 'value');

  				if (delta > 0) { value += 10; }
  				else if (delta < 0) { value -= 10; }

  				// Ensure that its limited between 0 and 100
  				value = Math.max(0, Math.min(100, value));
  				scrollbar.slider('option', 'value', value);
  				event.preventDefault();
  			});
  		}

  		jQuery("#scroll_right").mouseenter(
  			function() {

  				timer = setInterval(function() {

  					jQuery("#scroll_left").removeClass('disabled');

  					var speed = parseInt(8);
  					var slider = jQuery('.scroll-bar');
  					var curSlider = slider.slider("option", "value");
  					curSlider += speed; // += and -= directions of scroling with MouseWheel

  					if (curSlider > slider.slider("option", "max")){
  						jQuery("#scroll_right").addClass('disabled');
  						curSlider = slider.slider("option", "max");
  					} else if (curSlider < slider.slider("option", "min")){
  						curSlider = slider.slider("option", "min");
  					}else{

  					}
  					slider.slider("value", curSlider);

  				}, 100);

  			}
  		);

  		jQuery("#scroll_right").mouseleave(
  			function() {
  				clearInterval(timer);
  			}
  		);

  		jQuery("#scroll_left").mouseenter(
  			function() {
  				timer = setInterval(function() {

  				jQuery("#scroll_right").removeClass('disabled');

  				var speed = parseInt(8);
  				var slider = jQuery('.scroll-bar');;
  				var curSlider = slider.slider("option", "value");
  				curSlider -= speed; // += and -= directions of scroling with MouseWheel

  				if (curSlider > slider.slider("option", "max")){
  					curSlider = slider.slider("option", "max");
  				}else if (curSlider < slider.slider("option", "min")){
  					jQuery("#scroll_left").addClass('disabled');
  						curSlider = slider.slider("option", "min");
  				}

  				slider.slider("value", curSlider);

  			}, 100);

  			}
  		);
  		jQuery("#scroll_left").mouseleave(
  			function() {
  				clearInterval(timer);
  			}
  		);

    }

		function setScrollerWidth(){
			var origWidth = jQuery(".scroll-bar").width();//read the original slider width
			var sliderWidth = origWidth -200;//the width through which the handle can move needs to be the original width minus the handle width
			var sliderMargin =  (origWidth - sliderWidth)*0.5;//so the slider needs to have both top and bottom margins equal to half the difference
			jQuery(".scroll-bar-wrap").css({width:sliderWidth,marginRight: sliderMargin});//set the slider height and margins
		}


	});

	// Show or hide scrolling on window resize
	jQuery(window).smartresize(function(){
 		if( $contWidth > jQuery( '#thumbnails' ).width() ){
			jQuery( '#thumbnails .pulldown-items' ).width( $contWidth );
			jQuery( '#thumbnails .scroll-link' ).fadeIn();
		}else{
			jQuery( '#thumbnails .scroll-link' ).fadeOut();
			jQuery( '#thumbnails .pulldown-items' ).css( "margin-left", 0 );
		}

		// refresh the myScroll script
		if(isTouch){
  		myScrollRefresh();
    }

	});

	// Set bottom of the thumbnail container to the footers height
	// jQuery('#thumbnails').css({ bottom: jQuery('#colophon').outerHeight() });
	// jQuery('#thumbnails').css({ bottom: - jQuery('#thumbnails').outerHeight( true ) + jQuery('#colophon').outerHeight() });

	// initialize SuperBGImage
	$fullsize.superbgimage();

	// prev slide
	var fullsizePrev = jQuery('#thumbnails a.fullsize-prev').livequery(click_event,function() {

		perform_prevAnimation($fullsize, $fullsizeTimer);

		return false;
	});


	// Function to bind the home touchswipe action on the background
	$.fn.addFullsizeTouchswipe = function(){
		jQuery(this).swipe({
			threshold: 50,
			swipe: function(event, direction, distance, duration, fingerCount) {

				if( fingerCount == 1 ){ // one finger touch gestures

					// show next Slideshow Image if swipe left
					if( 'left' === direction ) {
						perform_nextAnimation();
					}

					// show next Slideshow Image if swipe right
					if( 'right' === direction ){
						perform_prevAnimation( $fullsize, $fullsizeTimer );
					}

					// hide Thumbnails
					if( 'down' === direction && true === window.thumsVisible ){
						$togthumbs.toggleThumbnails('hide');
					}

					// show thumbnails
					if( 'up' === direction && false === window.thumsVisible ){
						$togthumbs.toggleThumbnails('show');
					}

				}

				if( fingerCount == 2 ){ // two finger touch gestures

					if( 'up' === direction && true === window.allElementsVisible ) {
						$expander.hideAllElements('hide', 1);
					}

					if( 'down' === direction && false === window.allElementsVisible ) {
						$expander.hideAllElements('show', 1);
					}

				}

			},
			fingers: 'all',
			allowPageScroll: 'none'
		});
	};

	// check for touch devices
	if(isTouch){
		// add the touchswipe events
		jQuery("#superbgimage, #scanlines, #page").swipe("destroy");
		jQuery("#superbgimage, #scanlines, #page").addFullsizeTouchswipe();
  }

	// prev animation function
	function perform_prevAnimation($fullsize, $fullsizeTimer){

		jQuery('#superbgimageplayer').html();

		jQuery('#startInterval').val("start");

		$fullsizeTimer.stopTimer();
		$fullsize.prevSlide();

		if(jQuery.fn.superbgimage.options.slideshow == 1){
			$fullsize.startSlideShow();
		}else{
			$fullsize.stopSlideShow();
		}

	}

	// next slide
	var fullsizeNext = jQuery('#thumbnails a.fullsize-next').livequery(click_event,function() {

		perform_nextAnimation();

		return false;
	});


	// next animation function
	function perform_nextAnimation(){

		jQuery('#superbgimageplayer').html();

		jQuery('#startInterval').val("start");

		$fullsizeTimer.stopTimer();
		$fullsize.nextSlide();

		if(jQuery.fn.superbgimage.options.slideshow == 1){
			$fullsize.startSlideShow();
		}else{
			$fullsize.stopSlideShow();
		}

	}

	// mouse move scroller
	if( jQuery('#thumbnails').hasClass('mouse-scrub') ){

		//Get our elements for faster access and set overlay width
		var div = jQuery('#thumbnailContainer'),
		ul = jQuery('#fullsize'),

		ulPadding = 15;


		//Remove scrollbars
		div.css({overflow: 'hidden'});

		//Find last image container
		var lastLi = jQuery('a',ul).filter(":last");

		//When user move mouse over menu

		div.mousemove(function(e){

			//As images are loaded ul width increases,
			//so we recalculate it each time

  		//Get menu width
  		var divWidth = div.width();

			var ulWidth = lastLi[0].offsetLeft + lastLi.outerWidth() + ulPadding;
			var left = (e.pageX - div.offset().left) * (ulWidth-divWidth) / divWidth;
			div.scrollLeft(left);

		});

	}

	// keypress navigation
	if( jQuery('#thumbnails').hasClass('key-nav') ){
		jQuery(document).keydown(function (e) {

			var keyCode = e.keyCode || e.which,
				arrow = {left: 37, up: 38, space: 32, right: 39, down: 40 };

				switch (keyCode) {
					case arrow.left:

						perform_prevAnimation($fullsize, $fullsizeTimer);

					break;

					case arrow.up:

						$togthumbs.toggleThumbnails('show');

					break;

					case arrow.right:

						perform_nextAnimation();

					break;

					case arrow.down:

						$togthumbs.toggleThumbnails('hide');

					break;

				}

		});
	}

	// Mouse leave/enter Thumbnails
	if( jQuery('#thumbnails').hasClass('mouse-leave') ){

		jQuery('#colophon, #thumbnails').mouseenter(function(e){

			var fotBot = parseInt(jQuery('#colophon').css('bottom'));
			var botAni = jQuery('#colophon').outerHeight();

			if(fotBot < 0){
				botAni = 0;
			}

			jQuery('#thumbnails').stop().animate({ bottom: botAni });
		});

		jQuery('#thumbnails').mouseleave(function(e){

			var thumbnails = jQuery(this),
				fotBot = parseInt(jQuery('#colophon').css('bottom')),
				botAni = jQuery('#colophon').outerHeight();

			if(fotBot < 0){
				botAni = 0;
			}

			thumbnails.stop().animate({ bottom: -thumbnails.outerHeight() + botAni + thumbnails.find('.controls').outerHeight() });
		});

	}

	// new myScroll touch scrolling
	if(isTouch){

    var myScroll;
    function myScrollLoaded() {
    	setTimeout(function () {
  		  myScroll = new iScroll('thumbnailContainer', {
          hScrollbar: false,
          vScrollbar: false,
          onBeforeScrollMove: function() {
            jQuery('#fullsize .overlay').show();
          },
          onTouchEnd : function(){
            jQuery('#fullsize .overlay').hide();
          }
        });
      }, 100);
    }
    document.addEventListener('touchmove', function (e) { e.preventDefault(); }, false);
    window.addEventListener('load', myScrollLoaded, false);

    function myScrollRefresh() {
     	setTimeout(function() {
     		myScroll.refresh();
     	}, 0);
    };

  }

})