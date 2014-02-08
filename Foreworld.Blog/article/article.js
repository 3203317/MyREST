
$(document).ready(function(){
	console.log("Hello, World!");

	$('.flexslider').flexslider({
		animation: "slide"
	});

	$("#qrcode").pin({
		minWidth: 940
	})
});