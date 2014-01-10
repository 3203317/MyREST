/**
 * 作者：黄鑫
 * 日期：2013-08-31
 * 描述：企业信息
 */
var console = console||{log:function(){return;}}
require.config(config);
require(["jquery",
	"CorpInfoGrid",
	"CorpInfoTbar",
	"dhtmlxlayout"], function($, $Grid, $Tbar, $Form){

	$(document).ready(function(){
		/* 主框架布局 */
		var __layout = new dhtmlXLayoutObject({
			parent: "div_center",
			pattern: "1C",
			cells: [{
				id: "a",
				text: "企业信息",
				header: false
			}]
        });
		/* 去掉底部边框，设置为0 */
		$(".dhxcont_global_content_area").css({"border-bottom-width":0});
		__layout.setEffect("collapse",true);
		var __layoutB = __layout.cells("a");

		/* 工具栏 */
		var __tbar = new $Tbar({ placeAt: __layoutB.attachToolbar() });
		/* 创建Grid */
		var __grid = new $Grid({ placeAt: __layoutB.attachGrid() });

		/*<!-- 各种事件 */
		__grid.onCheck(function($rowsize){
			__tbar.showEdit($rowsize == 1);
		});
		__tbar.onClickEdit(function(){
			var __data = __grid.getCheckedData();
			__form.showEdit(__data);
		});
		/* 各种事件 -->*/

		/* 页面重绘或调整布局 */
		var __setSizes = function(){
			__layout.setSizes();
		};
		$(window).resize(function(){ setTimeout(__setSizes, 10); });
		__setSizes();
	});
},function($err){    
	var _failedId = $err.requireModules && $err.requireModules[0];
    if (_failedId === "jquery"){
        requirejs.undef(_failedId);
    }
});