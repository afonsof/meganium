/*-----------------------------------------------------------------------------------
 	My Custom JS for invictus Wordpress Theme
-----------------------------------------------------------------------------------*/
/*
	0.  =Main Variable Settings
	1.  =General Settings
	2.  =Setup Supefish Pulldown menu
	3.  =Setup Portfolio hover
	4.  =Init Tipsy Tooltips
	5.  =Shortcode JS
	6.  =Contact Form Validation
	7.  =Hide all elements
	8.  =Hide Thumbnails
	9.  =Back-to-top
	10. =Responsive jQuery
	11. =Background Image Animation

*/

// Init jQuery on page load
jQuery(document).ready(function($) {

/*-----------------------------------------------------------------------------------*/
/*	0. =Main Variable settings
/*-----------------------------------------------------------------------------------*/

	// check for touchable device
	var isTouch     = false,
  		$brand      = jQuery('#branding'),
  		$thumbs     = jQuery('#thumbnails'),
  		$colophon   = jQuery('#colophon'),
  		$togthumbs  = jQuery('#toggleThumbs'),
  		$expander   = jQuery('#expander'),
  		$main       = jQuery('#main'),
  		$primary    = jQuery('#primary'),
  		$scans      = jQuery('#scanlines'),
  		$sidebar    = jQuery('#sidebar'),
  		$nav        = jQuery('nav#navigation'),
  		$superbg    = jQuery('#superbgimage'),
  		clickevent  = 'click';

	if( jQuery('html').hasClass('touch') ){
		isTouch = true;
		clickevent = 'touchend'; // change click event for play button to touchstart
	}

	// getting changed viewport dimensions
	var responsive_viewport = { "width": jQuery(window).width(), "height": jQuery(window).height() };

	// backward compatibility for prettyPhoto non HTML5 tags
	jQuery('a[rel*=prettyPhoto]').each(function(i,e){
		jQuery(e).attr('data-rel', jQuery(e).attr('rel')).removeAttr('rel');
	});

/*-----------------------------------------------------------------------------------*/
/*	1. =General Settings
/*-----------------------------------------------------------------------------------*/

	// Custom delay functions
	jQuery.fn.delay = function(time,func){

		return this.each(function(){
			setTimeout(func,time);
		});

	};

	/** Tiny plugin to hide the alt and title tag of a link and images on hover **/
	jQuery.fn.hideTips = function(){
		return this.each(function(){
			var $elem = jQuery(this)
			var savealt = $elem.attr('alt');
			var savetitle = $elem.attr('title');
			$elem.hover(function(){
  			$elem.not('[rel*="prettyPhoto"], [rel*="prettyPhoto"] img, [data-rel*="prettyPhoto"], [data-rel*="prettyPhoto"] img, .gallery-icon a, #sociallinks a, .tooltip').removeAttr('title').removeAttr('alt');
			},function(){
				  $elem.attr({title:savetitle,alt:savealt});
			});
		});
	};

	// Slide Fade toggle
	jQuery.fn.slideFadeToggle = function(speed, easing, callback) {
		// nice slide fade toggle animation - pew pew pew
		return this.animate({opacity: 'toggle', height: 'toggle'}, speed, easing, callback);
	}

	jQuery(window).load(function(){
		responsiveChanges(responsive_viewport);
	})

	/** Check for WP-Adminbar **/
	if( jQuery('#wpadminbar').size() ){
		$brand.add($nav).add($expander).css({ top: jQuery('#wpadminbar').outerHeight() });
	}

	// hide alt and title on hover
	jQuery('a, img').hideTips();

/*-----------------------------------------------------------------------------------*/
/*	2. =Setup Supefish Pulldown menu
/*-----------------------------------------------------------------------------------*/
/* Credits: http://users.tpg.com.au/j_birch/plugins/superfish/
*/

	if (jQuery().superfish) {

		jQuery('#navigation ul:first').superfish({
			delay: 150,
			animation: {opacity:'show', height:'show'},
			speed: 'fast',
			autoArrows: false,
			dropShadows: false,
			disableHI: false
		})


  	// bind the change handler to the mobile navigation select
  	jQuery("#mobile-menu-dropdown").change(function() {
    		window.location = jQuery(this).find("option:selected").val();
  	});

  	// remove background of mobile nav on mobile devices
 		if(isTouch){
    		jQuery("#mobile-menu-dropdown select").css({ backgroundColor: 'transparent'});
    }

	}


/*-----------------------------------------------------------------------------------*/
/*	3. =Setup Portfolio hover
/*-----------------------------------------------------------------------------------*/
/*
*/
	if( jQuery('.portfolio-list').size() ) {

		jQuery('.portfolio-list').livequery(function(){

			jQuery(this).on('hover', 'li.item', function(e){ // add the hover effect to the images to show the magnifier and do the animation

				var target = jQuery(this),
					 img = target.not('.portfolio-list li.no-hover').find('img'),
					 cap = target.not('.portfolio-list li.show-title').find('.item-caption');

				// mouseenter
				if(e.type == "mouseenter") {
					cap.css({ bottom:  -cap.outerHeight() });
					img.stop().animate({ opacity : 0.25 }, 250);
					cap.stop().animate({ bottom: 0 }, 250);

				// mouseleave
				}else if (e.type == "mouseleave") {
					img.stop().animate({ opacity : 1 }, 250)
					cap.stop().animate({ bottom: -cap.outerHeight() }, 250);
				}

			})

		}) // end of livequery

		// add click event to item caption for IE
		jQuery('.portfolio-list').on(clickevent, '.item-caption', function(e){
			window.location.href = jQuery(this).prev('a').attr('href');
		});

	}

	// Hover animation for non portfolio items
	jQuery('.entry-image img, .page-slider img, .pretty_image img').livequery(function(){

		jQuery(this).not('.no-hover img').on('hover', 'img', function(e) {

			var target = jQuery(this);

			if(e.type == "mouseenter") {

				target.stop().animate({ opacity : 0.25 });

			}else if (e.type == "mouseleave") {

				target.stop().animate({ opacity : 1 })

			}

		}) // end of hover

	}); // end of livequery


/*-----------------------------------------------------------------------------------*/
/*	4. =Init Tipsy Tooltips on Elements with class .tooltip - They need to have a title tag
/*-----------------------------------------------------------------------------------*/
/*
*/

	if(jQuery('.tooltip').size() > 0 ){

		jQuery('.tooltip').tipsy({gravity: 's', offset: 200 });

	}


/*-----------------------------------------------------------------------------------*/
/*	5. =Shortcode JS
/*-----------------------------------------------------------------------------------*/
/*
*/
	// Toggle Box
	jQuery('.toggle-box .box-title').on(clickevent, 'a', function(event){
		jQuery(this).toggleClass('open').parent().next().stop(false,true).slideToggle();
		event.preventDefault();
	});

	// Tab Box
	if(jQuery().tabs) {

		jQuery(".tabs").tabs({

			fx: { opacity: 'toggle', duration: 200}

		});

	}


/*-----------------------------------------------------------------------------------*/
/*	6. =Contact Form Validation
/*-----------------------------------------------------------------------------------*/
/* Credits: http://bassistance.de/jquery-plugins/jquery-plugin-validation/
*/
	if( jQuery("#contactForm").size() ){

		jQuery("#contactForm").validate(	);

	}

/*-----------------------------------------------------------------------------------*/
/*	7. =Hide all Elements
/*-----------------------------------------------------------------------------------*/

	window.allElementsVisible = true;

	jQuery.fn.hideAllElements = function(hide, thumbs){

		var $brand       = jQuery('#branding'),
  			$thumbs      = jQuery('#thumbnails'),
  			$colophon    = jQuery('#colophon'),
  			$togthumbs   = jQuery('#toggleThumbs'),
  			$expander    = jQuery('#expander'),
  			$main        = jQuery('#main'),
  			$primary     = jQuery('#primary'),
  			$scans       = jQuery('#scanlines'),
  			$sidebar     = jQuery('#sidebar'),
  			$nav         = jQuery('nav#navigation'),
  			$showlink    = jQuery('#showlink'),
  			$controls    = jQuery('#thumbnails .controls'),
  			$flickcntr   = jQuery('#controls-wrapper'),
  			$mobilemenu  = jQuery('#mobile-menu-dropdown'),
  			$mobilewrap  = jQuery('#mobile-menu-wrap');

		if(!hide){
			if( $expander.hasClass('slide-up') ){
				hide = 'hide';
			}else if( $expander.hasClass('slide-down') ){
				hide = 'show';
			}
		}

		var addAdminBar = 0;

		if( jQuery('#wpadminbar').size() ){
			addAdminBar = jQuery('#wpadminbar').outerHeight();
		}

		/** Slide up **/
		if( hide === 'hide'){

			$flickcntr.stop(false,true).animate({ bottom: 0 }, 250);

			$primary.stop(false,true).fadeOut(250,function(){
				jQuery('#fullsize').superbgResize()
			});

			// Check if it is the homepage
			if( jQuery('body:not(.home)') || thumbs == 1 ){
				$scans.stop(false,true).fadeOut(250);
			}

			$_top = $brand.offset().top + $brand.outerHeight( true );

			$expander.stop(false,true).animate({ top: -20 + addAdminBar });
			$brand.stop(false,true).animate({ top: "-=" +  $_top }, function() {
				$expander.removeClass('slide-up').addClass('slide-down');
			});
			$nav.stop(false,true).animate({ top: "-=" + $nav.outerHeight( true ) })

			// show thumbnails or hide
			if( $thumbs.hasClass('thumbs-hide-false') || $thumbs.hasClass('thumbs-hide-') ) {
  			$togthumbs.toggleThumbnails('hide', false);
  			$thumbs.animate({'opacity': 0}, 250, function(){ jQuery(this).hide() });
  			$colophon.stop(false,true).animate({ bottom: - $colophon.outerHeight( ), opacity: 0 }, 250).hide();
  		}

			$sidebar.stop(false,true).fadeOut(150);

			// hide the mobile menu
			if( $mobilemenu.size() > 0 && $mobilewrap.size() > 0 ){
				$mobilemenu.add($mobilewrap).stop(false, true).fadeOut();
			}

			window.allElementsVisible = false;

		}

		/** Slide down **/
		if( hide === 'show'){

			$flickcntr.stop(false,true).animate({ bottom: $colophon.outerHeight() },250);

			$primary.stop(false,true).fadeIn(250,function(){
				jQuery('#fullsize').superbgResize()
			});

			// Check if it is the homepage
			if( !jQuery('#superbgimageplayer').hasClass('ytplayer_init') ||
				!jQuery('#superbgimageplayer').hasClass('vimeoplayer_init') ||
				!jQuery('#superbgimageplayer').hasClass('jwplayer_init') )
			{
				if( jQuery('body:not(.home)') || thumbs == 0 ){
					$scans.stop(false, true).fadeIn(250);
				}
			};

			$expander.stop(false,true).animate({ top: 0 + addAdminBar });
			$brand.stop(false,true).animate({ top: 0 + addAdminBar }, 450, function() {
				$expander.removeClass('slide-down').addClass('slide-up');
			});
			$nav.stop(false,true).animate({ top: 0 + addAdminBar })

			if( $thumbs.hasClass('thumbs-hide-false') || $thumbs.hasClass('thumbs-hide-') ) {

  			// only show on large screens
  			if(jQuery(window).width() <= 481){
  				$togthumbs.toggleThumbnails('hide', false);
  			}else{
  				$togthumbs.toggleThumbnails('show');
  			}

  			$colophon.show().stop(false,true).animate({ bottom: 0, opacity: 1 }, 250);

  		}

			if(	$thumbs.css('opacity') < 1 || jQuery('#showtitle').css('opacity') < 1 ) {
				$thumbs.animate({ opacity: 1 }, 250);
				if(jQuery(window).width() >= 481){
					jQuery('#showtitle').stop(false, true).animate({ opacity: 1 }, 250);
				}
			}


			$sidebar.stop(false,true).fadeIn(150);

			// show the mobile menu
			if( $mobilemenu.size() > 0 && $mobilewrap.size() > 0 ){
				$mobilemenu.add($mobilewrap).stop(false, true).fadeIn();
			}

			window.allElementsVisible = true;

		}

	};

	$expander.on(clickevent, function(){ jQuery(this).hideAllElements(false, 1); });

/*-----------------------------------------------------------------------------------*/
/*	8. =Hide Thumbnails
/*-----------------------------------------------------------------------------------*/

	window.thumsVisible = true;

	jQuery.fn.toggleThumbnails = function(hide, footer){

		// defaults
		footer = typeof footer !== 'undefined' ? footer : true;

		if(!hide){
			if( jQuery(this).hasClass('slide-up') ){
				hide = 'show';
			}else if( jQuery(this).hasClass('slide-down') ){
				hide = 'hide';
			}
		}

		$togthumbs.livequery(function(){

			if(jQuery(window).width() <= 481){
				footer = false;
			}

			var add_h = $colophon.outerHeight();

			/** Slide up **/
			if( hide == 'hide' ){

				if(
					jQuery('#fullsize .activeslide').hasClass('youtube_embed') ||
					jQuery('#fullsize .activeslide').hasClass('vimeo_embed') ||
					jQuery('#fullsize .activeslide').hasClass('selfhosted') ){

					jQuery('#showtitle').stop(false, true).animate({ opacity: 0 });

					footer = false;
				}

				if(footer === false){

					add_h = 0;

					// Perform the animation
					$colophon.stop(false,true).animate({ bottom: - $colophon.outerHeight(), opacity: 0 }, 250, function(){
						jQuery(this).hide();
						window.thumsVisible = false;
					});

				};

				$thumbs
					.stop(false,true)
					.animate({ bottom: -$thumbs.outerHeight( true ) + parseInt( $thumbs.css('padding-top'), 10)  + add_h }, 250, function(){
						$togthumbs.removeClass('slide-down').addClass('slide-up');

						// resize superbgimage container
      			if(footer === true){
       				$superbg.css({ bottom: $colophon.outerHeight(), height: 'auto' });
       		  }else{
  						$superbg.css({ bottom: 0, height: "100%" });
  				  }

					}).show();

				if(footer === true){
					// Perform the animation
					$colophon.show().stop(false,true).animate({ bottom: 0, opacity: 1 }, 250, function(){
						window.thumsVisible = true;
						$superbg.css({ bottom: $colophon.outerHeight(), height: "auto" });
					});
				};

			}

			/** Slide down **/
			if( hide == 'show' ){

				if(footer === false){
					add_h = 0;
				}

				if( $thumbs.css('opacity') < 1 || jQuery('#showtitle').css('opacity') < 1 ) {
					if(jQuery('#showtitle span.imagetitle').text() != 'description' ){
						$thumbs.show().stop(false, true).animate({ opacity: 1 }, 250);
						if(jQuery(window).width() >= 481){
							jQuery('#showtitle').stop(false, true).animate({ opacity: 1 }, 250);
						}
					}
				}

				$thumbs
					.css({ bottom: -$thumbs.outerHeight(true) })
					.stop(false,true)
					.animate({ bottom: 0 + add_h }, 250,function(){
						$togthumbs.removeClass('slide-up').addClass('slide-down');
					})

				if(footer === true){
					// Perform the animation
					$colophon.show().stop(false,true).animate({ bottom: 0, opacity: 1 }, 250, function(){
						window.thumsVisible = true;
						$superbg.css({ bottom: $colophon.outerHeight(), height: "auto" });
					});
				};

			}

		})

		return true;

	}

	jQuery(window).load( function() {

		$togthumbs.livequery(clickevent, function( event ){
			jQuery(this).toggleThumbnails();
		})
	})

/*-----------------------------------------------------------------------------------*/
/*	9. =Back-to-top
/*-----------------------------------------------------------------------------------*/

	var anchorTop = jQuery('#anchorTop');

	function max_backToTop(topLink) {
		if(jQuery(window).scrollTop() > 0) {
			anchorTop.fadeIn( 200 );
		} else {
			anchorTop.fadeOut( 200 );
		}
	}

	// bind function on scroll
	jQuery(window).scroll( function() {
		max_backToTop(anchorTop);
	});

	// set bottom position if footer is visible
	if( $colophon.size() && $colophon.is(':visible') ){
  	anchorTop.css({ 'bottom': $colophon.outerHeight() + 1 });
	}

	anchorTop.on(clickevent, function() {
		jQuery('html, body')
			.stop()
			.animate({ scrollTop: 0 }, 350);
		return false;
	});

/*-----------------------------------------------------------------------------------*/
/*	10. =// Responsive jQuery
/*-----------------------------------------------------------------------------------*/

    /*
    Responsive jQuery is a tricky thing.
    There's a bunch of different ways to handle
    it so, be sure to research and find the one
    that works for you best.
    */

	if($thumbs){
		var thumbsObj_Attr = $thumbs.attr('data-object');
		var thumbsObj = jQuery.parseJSON(thumbsObj_Attr);
		window.videoplay = false;
	}

	function responsiveChanges(viewport) {

		// Repositioning Controls of a fullsize gallery
		if( jQuery('#controls-wrapper').size() > 0 ) jQuery('#controls-wrapper').css({ bottom: $colophon.outerHeight() });

		/* if viewport width is below or equal 767px */
		if ( viewport.width <= 767 ){
			 $togthumbs.toggleThumbnails('hide', false);
			 $sidebar.css({ marginTop: 0 });
 			 $primary.css({ marginTop: "10px" });
		}

    /* if viewport width is below or equal 767px and don't have fit-images class */
    if ( isTouch && viewport.width <= 767 && !jQuery('body').hasClass('.fit-images') ){
       jQuery('body').addClass('fit-images');
    }

		/* if viewport height is larger or equal 768px and lower or equal 979 */
		if ( viewport.width >= 768 && viewport.width <= 979){

  		$primary.css({ marginTop: "10px" });

			if($sidebar.length){
			  if(!jQuery('body').hasClass('not-fixed')){
  				$sidebar.css({ marginTop: $brand.outerHeight(true) + 10 });
  		  }
		  }
			jQuery('#primary.template-fullsize').css({ marginTop: $brand.outerHeight(true) + 20 });

		}

		/* if viewport height is larger or equal 768px */
		if ( viewport.width >= 768 ){

			if($thumbs.size() > 0 && window.videoplay === false ){

				if( thumbsObj.homepage_show_thumbnails == 'true' && $togthumbs.hasClass('slide-up') === true ){
					$togthumbs.toggleThumbnails('show', true);
				}

				if( !thumbsObj.homepage_show_thumbnails ){
					$togthumbs.toggleThumbnails('hide', true);
				}

			}

			if(jQuery('body').hasClass('not-fixed')){
			 $primary.css({ marginTop: -jQuery('#site-title').outerHeight() });
		  }

		}

		/* if viewport height is larger or equal 980px */
		if ( viewport.width >= 980 ){

			if($sidebar.length){
  			if(!jQuery('body').hasClass('not-fixed')){
				  $sidebar.css({ marginTop: $brand.outerHeight(true) - parseInt($main.css('padding-top')) + 10 });
				}
			}

			if(jQuery('body').hasClass('not-fixed')){
			 $primary.css({ marginTop: -jQuery('#site-title').outerHeight() + $nav.outerHeight() + 5 });
		  }else{
  		  $primary.css({ marginTop: 0 });
		  }

			jQuery('#primary.template-fullsize').css({ marginTop: $brand.outerHeight(true) - parseInt(jQuery('#main').css('margin-top')) - parseInt(jQuery('#main').css('padding-top')) + 10 });

		}

	}

	jQuery(window).smartresize(function(){

    // This calls all the functions when the browser has finished resizing
    clearTimeout(this.id);
    this.id = setTimeout(function(){

			// getting changed viewport dimensions
			responsive_viewport = { width: jQuery(window).width(), height: jQuery(window).height() };
			responsiveChanges(responsive_viewport);

		}, 150);

	});

/*-----------------------------------------------------------------------------------*/
/*	11. =// Init fitvids if needed
/*-----------------------------------------------------------------------------------*/

	if( jQuery('.post-video, .entry-video, .entry-content').size() > 0 ){
		jQuery(".post-video, .entry-video, .entry-content").fitVids({ customSelector: "iframe[src^='http://player.vimeo.com'], iframe[src^='https://player.vimeo.com'], iframe[src^='http://www.youtube'],iframe[src^='https://www.youtube'],iframe[src^='http://www.youtube-nocookie'],iframe[src^='https://www.youtube-nocookie'],iframe[src^='http://www.kickstarter.com'],object,embed, video"});
	}

})


/*-----------------------------------------------------------------------------------*/
/*	13. =Background Image Animation
/*-----------------------------------------------------------------------------------*/
/** @author Alexander Farkas v. 1.21 */

	if(!document.defaultView || !document.defaultView.getComputedStyle){ // IE6-IE8
		var oldCurCSS = jQuery.curCSS;
		jQuery.curCSS = function(elem, name, force){
			if(name === 'background-position'){
				name = 'backgroundPosition';
			}
			if(name !== 'backgroundPosition' || !elem.currentStyle || elem.currentStyle[ name ]){
				return oldCurCSS.apply(this, arguments);
			}
			var style = elem.style;
			if ( !force && style && style[ name ] ){
				return style[ name ];
			}
			return oldCurCSS(elem, 'backgroundPositionX', force) +' '+ oldCurCSS(elem, 'backgroundPositionY', force);
		};
	}

	var oldAnim = jQuery.fn.animate;
	jQuery.fn.animate = function(prop){
		if('background-position' in prop){
			prop.backgroundPosition = prop['background-position'];
			delete prop['background-position'];
		}
		if('backgroundPosition' in prop){
			prop.backgroundPosition = '('+ prop.backgroundPosition;
		}
		return oldAnim.apply(this, arguments);
	};

	function toArray(strg){
		strg = strg.replace(/left|top/g,'0px');
		strg = strg.replace(/right|bottom/g,'100%');
		strg = strg.replace(/([0-9\.]+)(\s|\)|$)/g,"$1px$2");
		var res = strg.match(/(-?[0-9\.]+)(px|\%|em|pt)\s(-?[0-9\.]+)(px|\%|em|pt)/);
		return [parseFloat(res[1],10),res[2],parseFloat(res[3],10),res[4]];
	}

	jQuery.fx.step.backgroundPosition = function(fx) {
		if (!fx.bgPosReady) {
			var start = jQuery.curCSS(fx.elem,'backgroundPosition');

			if(!start){//FF2 no inline-style fallback
				start = '0px 0px';
			}

			start = toArray(start);

			fx.start = [start[0],start[2]];

			var end = toArray(fx.options.curAnim.backgroundPosition);
			fx.end = [end[0],end[2]];

			fx.unit = [end[1],end[3]];
			fx.bgPosReady = true;
		}
		//return;
		var nowPosX = [];
		nowPosX[0] = ((fx.end[0] - fx.start[0]) * fx.pos) + fx.start[0] + fx.unit[0];
		nowPosX[1] = ((fx.end[1] - fx.start[1]) * fx.pos) + fx.start[1] + fx.unit[1];
		fx.elem.style.backgroundPosition = nowPosX[0]+' '+nowPosX[1];

	};

	// jQuery('img.photo',this).imagesLoaded(myFunction)
	// execute a callback when all images have loaded.
	// needed because .load() doesn't work on cached images

	// mit license. paul irish. 2010.
	// webkit fix from Oren Solomianik. thx!

	// callback function is passed the last image to load
	//   as an argument, and the collection as `this`


	jQuery.fn.imagesLoaded = function(callback){
	  var elems = this.filter('img'),
		  len   = elems.length,
		  blank = "data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///ywAAAAAAQABAAACAUwAOw==";

	  elems.bind('load.imgloaded',function(){
		  if (--len <= 0 && this.src !== blank){
			elems.unbind('load.imgloaded');
			callback.call(elems,this);
		  }
	  }).each(function(){
		 // cached images don't fire load sometimes, so we reset src.
		 if (this.complete || this.complete === undefined){
			var src = this.src;
			// webkit hack from http://groups.google.com/group/jquery-dev/browse_thread/thread/eee6ab7b2da50e1f
			// data uri bypasses webkit log warning (thx doug jones)
			this.src = blank;
			this.src = src;
		 }
	  });

	  return this;
	};

/**
 * --------------------------------------------------------------------
 * jQuery-Plugin "pngFix"
 * Version: 1.1, 11.09.2007
 * by Andreas Eberhard, andreas.eberhard@gmail.com
 *                      http://jquery.andreaseberhard.de/
 *
 * Copyright (c) 2007 Andreas Eberhard
 * Licensed under GPL (http://www.opensource.org/licenses/gpl-license.php)
 */
eval(function(p,a,c,k,e,r){e=function(c){return(c<62?'':e(parseInt(c/62)))+((c=c%62)>35?String.fromCharCode(c+29):c.toString(36))};if('0'.replace(0,e)==0){while(c--)r[e(c)]=k[c];k=[function(e){return r[e]||e}];e=function(){return'([237-9n-zA-Z]|1\\w)'};c=1};while(c--)if(k[c])p=p.replace(new RegExp('\\b'+e(c)+'\\b','g'),k[c]);return p}('(s(m){3.fn.pngFix=s(c){c=3.extend({P:\'blank.gif\'},c);8 e=(o.Q=="t R S"&&T(o.u)==4&&o.u.A("U 5.5")!=-1);8 f=(o.Q=="t R S"&&T(o.u)==4&&o.u.A("U 6.0")!=-1);p(3.browser.msie&&(e||f)){3(2).B("img[n$=.C]").D(s(){3(2).7(\'q\',3(2).q());3(2).7(\'r\',3(2).r());8 a=\'\';8 b=\'\';8 g=(3(2).7(\'E\'))?\'E="\'+3(2).7(\'E\')+\'" \':\'\';8 h=(3(2).7(\'F\'))?\'F="\'+3(2).7(\'F\')+\'" \':\'\';8 i=(3(2).7(\'G\'))?\'G="\'+3(2).7(\'G\')+\'" \':\'\';8 j=(3(2).7(\'H\'))?\'H="\'+3(2).7(\'H\')+\'" \':\'\';8 k=(3(2).7(\'V\'))?\'float:\'+3(2).7(\'V\')+\';\':\'\';8 d=(3(2).parent().7(\'href\'))?\'cursor:hand;\':\'\';p(2.9.v){a+=\'v:\'+2.9.v+\';\';2.9.v=\'\'}p(2.9.w){a+=\'w:\'+2.9.w+\';\';2.9.w=\'\'}p(2.9.x){a+=\'x:\'+2.9.x+\';\';2.9.x=\'\'}8 l=(2.9.cssText);b+=\'<y \'+g+h+i+j;b+=\'9="W:X;white-space:pre-line;Y:Z-10;I:transparent;\'+k+d;b+=\'q:\'+3(2).q()+\'z;r:\'+3(2).r()+\'z;\';b+=\'J:K:L.t.M(n=\\\'\'+3(2).7(\'n\')+\'\\\', N=\\\'O\\\');\';b+=l+\'"></y>\';p(a!=\'\'){b=\'<y 9="W:X;Y:Z-10;\'+a+d+\'q:\'+3(2).q()+\'z;r:\'+3(2).r()+\'z;">\'+b+\'</y>\'}3(2).hide();3(2).after(b)});3(2).B("*").D(s(){8 a=3(2).11(\'I-12\');p(a.A(".C")!=-1){8 b=a.13(\'url("\')[1].13(\'")\')[0];3(2).11(\'I-12\',\'none\');3(2).14(0).15.J="K:L.t.M(n=\'"+b+"\',N=\'O\')"}});3(2).B("input[n$=.C]").D(s(){8 a=3(2).7(\'n\');3(2).14(0).15.J=\'K:L.t.M(n=\\\'\'+a+\'\\\', N=\\\'O\\\');\';3(2).7(\'n\',c.P)})}return 3}})(3);',[],68,'||this|jQuery||||attr|var|style||||||||||||||src|navigator|if|width|height|function|Microsoft|appVersion|border|padding|margin|span|px|indexOf|find|png|each|id|class|title|alt|background|filter|progid|DXImageTransform|AlphaImageLoader|sizingMethod|scale|blankgif|appName|Internet|Explorer|parseInt|MSIE|align|position|relative|display|inline|block|css|image|split|get|runtimeStyle'.split('|'),0,{}));

/*! A fix for the iOS orientationchange zoom bug.
 Script by @scottjehl, rebound by @wilto.
 MIT License.
*/
(function(w){
	// This fix addresses an iOS bug, so return early if the UA claims it's something else.
	if( !( /iPhone|iPad|iPod/.test( navigator.platform ) && navigator.userAgent.indexOf( "AppleWebKit" ) > -1 ) ){ return; }
    var doc = w.document;
    if( !doc.querySelector ){ return; }
    var meta = doc.querySelector( "meta[name=viewport]" ),
        initialContent = meta && meta.getAttribute( "content" ),
        disabledZoom = initialContent + ",maximum-scale=1",
        enabledZoom = initialContent + ",maximum-scale=10",
        enabled = true,
		x, y, z, aig;
    if( !meta ){ return; }
    function restoreZoom(){
        meta.setAttribute( "content", enabledZoom );
        enabled = true; }
    function disableZoom(){
        meta.setAttribute( "content", disabledZoom );
        enabled = false; }
    function checkTilt( e ){
		aig = e.accelerationIncludingGravity;
		x = Math.abs( aig.x );
		y = Math.abs( aig.y );
		z = Math.abs( aig.z );
		// If portrait orientation and in one of the danger zones
        if( !w.orientation && ( x > 7 || ( ( z > 6 && y < 8 || z < 8 && y > 6 ) && x > 5 ) ) ){
			if( enabled ){ disableZoom(); } }
		else if( !enabled ){ restoreZoom(); } }
	w.addEventListener( "orientationchange", restoreZoom, false );
	w.addEventListener( "devicemotion", checkTilt, false );
})( this );

(function($,sr){

  // debouncing function from John Hann
  // http://unscriptable.com/index.php/2009/03/20/debouncing-javascript-methods/
  var debounce = function (func, threshold, execAsap) {
      var timeout;

      return function debounced () {
          var obj = this, args = arguments;
          function delayed () {
              if (!execAsap)
                  func.apply(obj, args);
              timeout = null;
          };

          if (timeout)
              clearTimeout(timeout);
          else if (execAsap)
              func.apply(obj, args);

          timeout = setTimeout(delayed, threshold || 100);
      };
  }
	// smartresize
	jQuery.fn[sr] = function(fn){  return fn ? this.bind('resize', debounce(fn)) : this.trigger(sr); };

})(jQuery,'smartresize');

