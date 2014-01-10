$(function(){
	$("#btn_submit").click(function(){
		var __formObj = $("#loginForm").serializeObjectForm();
		console.log(JSON.stringify(__formObj));

		var __data = { data: JSON.stringify(__formObj) };

		$.ajax({
			type: "post",
			url: "login.do",
			dataType: "json",
			async: true,
			data: __data,
			beforeSend: function($xhr){
				$xhr.overrideMimeType("text/plain; charset=utf-8");
			}
		}).done(function($data){
			console.log($data);
			if($data.Success){
				location.href = "../main/indexUI.html?locale="+ (location.search.match(/locale=([\w\-]+)/) ? RegExp.$1 : "zh") +"#page/welcomeUI.html";
			}else{		
				$("#span_errmsg").html($data.Msg);		
			}
		});
	});					
	
	$("#btn_reset").click(function(){
		$("#loginForm")[0].reset();
	});
})