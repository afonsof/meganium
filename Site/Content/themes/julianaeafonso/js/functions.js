

// If JavaScript is enabled remove 'no-js' class and give 'js' class
jQuery('html').removeClass('no-js').addClass('js');

// Add .osx class to html if on Os/x
if (navigator.appVersion.indexOf("Mac") !== -1) {
	jQuery('html').addClass('osx');
}
// When DOM is fully loaded
jQuery(document).ready(function($) {

var newYear = new Date(); 
newYear = new Date(2014,1,8,18,30,0); 
$('#defaultCountdown').countdown({until: newYear}); 
  


/* 	PRETTYPHOTO */
$('a[data-rel]').each(function() {
			$(this).attr('rel', $(this).data('rel'));
		});
$("a[rel^='prettyPhoto']").prettyPhoto({animation_speed: 'normal', slideshow: 5000, autoplay_slideshow: false, social_tools: false}); 



/* FITTEXT */
 
		
    $(".fittext1").fitText(1, { minFontSize: '15px', maxFontSize: '30px' });
    $(".fittext2").fitText(0.4, { minFontSize: '30px', maxFontSize: '80px' });
    $(".fittext3").fitText(0.4, { minFontSize: '30px', maxFontSize: '80px' });
    $(".fittext4").fitText(1.5, { minFontSize: '15px', maxFontSize: '24px' });

/* 	External Links	*/	

	(function() {
	    $(window).load(function() {
			$('a[rel=external]').attr('target','_blank');	
		});                                            
	})();  


/* 	Flex Initialize	*/	

$(window).load(function() {
 
  $('.slider1').flexslider({
    animation: "slide",
    animationLoop: false,
    itemWidth: 300, 
    itemMargin: 0,
    slideshow: false,
    directionNav: false
  });


  $('.slider2').flexslider({
    animation: "slide",
    directionNav: true,
    slideshow: false,
    animationLoop: false    
  });

  
  $('.flexslider').flexslider({ 
    animation: "slide",
    slideshow: false,
    directionNav: false,
    start: function(slider){
      $('body').removeClass('loading');
    } 
  });

});


/*  STICKY 	*/
$("nav").sticky({topSpacing:0});


/* SCROLL 	*/

$('nav').localScroll({duration:600, offset : { top:0, left:0 }});
  
$(".select-menu").change(function() {
	
		$('html, body').animate({
	        scrollTop: $($(this).find("option:selected").val()).offset().top
	    }, 1000, function(){
	    	window.location.hash = $(this).find("option:selected").val();
	    });
	});
  
  

/* 	MOBILE MENU	*/
	$("<option />", {
	   "selected": "selected",
	   "value"   : "",
	   "text"    : "Navegação"
	}).appendTo(".select-menu");


	$(".navi a").each(function() {
	 var select = $(this);
	 $("<option />", {
	     "value"   : select.attr("href"),
	     "text"    : select.attr("title")
	 }).appendTo(".select-menu");
	});


/* SCROLL NAVIGATION */
 
$(window).scroll(function() {
     var scrollTop = $(window).scrollTop(); 
      
      $('.navi a[href*="home"]').addClass('active');
      $('.navi a[href*="about"]').removeClass('active');
			$('.navi a[href*="location"]').removeClass('active');
      $('.navi a[href*="gifts"]').removeClass('active');
		  $('.navi a[href*="tableware"]').removeClass('active');
      $('.navi a[href*="gallery"]').removeClass('active');
		  $('.navi a[href*="contact"]').removeClass('active');
     
                                        
		if ( scrollTop >= $('#home').height() + $('#slide').height() - 60 ) {
      $('.navi a[href*="home"]').removeClass('active');
			$('.navi a[href*="about"]').addClass('active');
		} 
		
    if (scrollTop >= $('#home').height() + $('#slide').height() + $('#about').height()  ) {
      $('.navi a[href*="home"]').removeClass('active');
      $('.navi a[href*="about"]').removeClass('active');
			$('.navi a[href*="location"]').addClass('active');
		}
    
    
    if (scrollTop >= $('#home').height() + $('#slide').height() + $('#about').height() + $('#location').height()  ) {
      
      $('.navi a[href*="home"]').removeClass('active');
      $('.navi a[href*="about"]').removeClass('active');
			$('.navi a[href*="location"]').removeClass('active');
      $('.navi a[href*="gifts"]').addClass('active');
		}
        
      if (scrollTop >= $('#home').height() + $('#slide').height() + $('#about').height()  + $('#location').height() + $('#gifts').height() ) {
      
      $('.navi a[href*="home"]').removeClass('active');
      $('.navi a[href*="about"]').removeClass('active');
			$('.navi a[href*="location"]').removeClass('active');
      $('.navi a[href*="gifts"]').removeClass('active');
      $('.navi a[href*="tableware"]').addClass('active');
		} 
    
    
      if (scrollTop >= $('#home').height() + $('#slide').height() + $('#about').height()  + $('#location').height() + $('#gifts').height() + $('#tableware').height() ) {
      
      $('.navi a[href*="home"]').removeClass('active');
      $('.navi a[href*="about"]').removeClass('active');
			$('.navi a[href*="location"]').removeClass('active');
      $('.navi a[href*="gifts"]').removeClass('active');
      $('.navi a[href*="tableware"]').removeClass('active');
      $('.navi a[href*="gallery"]').addClass('active');
		}
    
    
      if (scrollTop >= $('#home').height() + $('#slide').height() + $('#about').height()  + $('#location').height() + $('#gifts').height() + $('#tableware').height() + $('#gallery').height()  ) {
      
      $('.navi a[href*="home"]').removeClass('active');
      $('.navi a[href*="about"]').removeClass('active');
			$('.navi a[href*="location"]').removeClass('active');
      $('.navi a[href*="gifts"]').removeClass('active');
      $('.navi a[href*="tableware"]').removeClass('active');
      $('.navi a[href*="gallery"]').removeClass('active');
      $('.navi a[href*="contact"]').addClass('active');
		}  





});

}); 