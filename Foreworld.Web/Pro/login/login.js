/**
 * 作者：黄鑫
 * 日期：2013-07-15
 * 描述：系统登陆
 */
var console = console||{log:function(){return;}}
require.config(config);
require(["jquery",
	"i18n!nls/login"], function($, $i18n){
	/* 错误提示 */
	var _span_errmsg = $("#span_errmsg");

	/* 定义三个input对象 */
	var _text_username = $("#text_username");
	var _text_password = $("#text_password");
	var _text_verifycode = $("#text_verifycode");

	/* 定义两个btn对象 */
	var _btn_submit = $("#btn_submit");
	var _btn_reset = $("#btn_reset");

	/* 登陆 */
	var _login = function($formobj){
		$formobj.ts = Date.parse(new Date())/1000;
		$.ajax({
			type: "get",
			url: "../Api.ashx?command=login",
			dataType: "json",
			async: true,
			data: $formobj,
			beforeSend: function($xhr){
				$xhr.overrideMimeType("text/plain; charset=utf-8");
			}
		}).done(function($data){
			if($data.status == "failure"){
				_span_errmsg.html($data.msg);
			}else{				
				location.href = "main.html?locale="+ config.locale +"#page/"+ config.page;
			}
		});
	};

	/* 重置 */
	var _reset = function(){
		_text_username.val("");
		_text_password.val("");
		_text_verifycode.val("");
		_span_errmsg.html("");
	};

	/* 表单验证，成功则返回表单参数对象，否则在界面上提示 */
	var _verify = function(){
		var __username = $.trim(_text_username.val());
		if("" === __username){
			_text_username.focus();
			_span_errmsg.html($i18n.err_empty_username);
			return null;
		}

		var __password = $.trim(_text_password.val());
		if("" === __password){
			_text_password.focus();
			_span_errmsg.html($i18n.err_empty_password);
			return null;
		}

		var __verifycode = $.trim(_text_verifycode.val());
		if("" === __verifycode){
			_text_verifycode.focus();
			_span_errmsg.html($i18n.err_empty_verifycode);
			return null;
		}
		var __formobj = {
			username: __username, 
			password: __password,
			verifycode: __verifycode
		};
		return __formobj;
	};

	$(document).ready(function(){
		_btn_submit.click(function(){
			var __formobj = _verify();
			if(__formobj) _login(__formobj);
		});
		_btn_reset.click(_reset);
	});
},function($err){    
	var _failedId = $err.requireModules && $err.requireModules[0];
    if (_failedId === "jquery"){
        requirejs.undef(_failedId);
    }
});