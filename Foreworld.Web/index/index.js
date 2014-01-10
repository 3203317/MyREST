/**
 * 作者：黄鑫
 * 日期：2013-08-24
 * 描述：首页
 */
var console = console||{log:function(){return;}}
require.config(config);
require(["jquery",
	"util",
	"footer"], function($, $util, $footer){

	$(document).ready(function(){
		$footer();
	});
},function($err){    
	var _failedId = $err.requireModules && $err.requireModules[0];
    if (_failedId === "jquery"){
        requirejs.undef(_failedId);
    }
});