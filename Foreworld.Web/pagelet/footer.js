/**
 * 作者：黄鑫
 * 日期：2013-09-17
 * 描述：页脚
 */
define(["jquery",
	"util",
	"text!../pagelet/footer.html"], function($, $util, $text) {

	var _render = function(){
		$("footer").append($text);
	};

	var _appendCss = function(){
		$util.appendCss("../pagelet/footer.css");
	};

	var _d = function(){
		_appendCss();
		_render();
	};
	return _d;
});