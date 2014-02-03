
$(document).ready(function(){
	console.log("Hello, World!");

	$('.flexslider').flexslider({
		animation: "slide"
	});

	$("#qrcode").pin({
		minWidth: 940
	})
	
	$("#loadMore").click(function(){
        //console.log($("#loadMore").data("current-page"));
        var data = { currentPage: $("#loadMore").data("current-page") + 1 };
        $.ajax({
			url: $().olxUtilRandomUrl("index_loadmore.html"),
			type: "POST",
			dataType: "html",
			data: { data: JSON.stringify(data) }
		}).done(function(responseText) {
		    if("" != $.trim(responseText)){		    
                $("#loadMore").data("current-page", "2");		    
			    $("#articleIntros").append(responseText);
		    }			
		}).complete(function(){
		});
	});
});