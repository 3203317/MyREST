/**
 * 作者：黄鑫
 * 日期：2013-09-04
 * 描述：页面管理
 */
var console = console||{log:function(){return;}}
require.config(config);
require(["jquery",
	"PageTree",
	"PageGrid",
	"PageDsWdGrid",
	"PageDsWdParamGrid",
	"PageTbar",
	"PageForm",
	"dhtmlxlayout_pattern4c"], function($, $Tree, $Grid, $Grid2, $Grid3, $Tbar, $Form){

	$(document).ready(function(){
		/* 主框架布局 */
		var __layout = new dhtmlXLayoutObject({
			parent: "div_center",
			pattern: "4C",
			cells: [{
				id: "a",
				text: "页面树",
				width: 220
			},{
				id: "b",
				text: "数据源列表",
				header: false
			},{
				id: "c",
				text: "组件列表",
				header: false
			},{
				id: "d",
				text: "属性关联",
				header: false
			}]
        });
		/* 去掉底部边框，设置为0 */
		$(".dhxcont_global_content_area").css({"border-bottom-width":0});
		__layout.setEffect("collapse",true);
		var __layoutA = __layout.cells("a");
		var __layoutB = __layout.cells("b");
		var __layoutC = __layout.cells("c");
		var __layoutD = __layout.cells("d");
		__layoutA.attachObject("div_tree");
		__layout.setCollapsedText("a","页面树");

		/* 创建树 */
		var __tree = new $Tree({ placeAt: "div_tree" });
		/* 工具栏 */
		var __tbar = new $Tbar({ placeAt: __layoutB.attachToolbar() });
		/* 创建Grid */
		var __grid = new $Grid({ placeAt: __layoutB.attachGrid() });
		/* 创建Form */
		var __form; setTimeout(function(){ __form = new $Form({ placeAt: document.body }); },100);
		/* 创建Grid2 */
		var __grid2 = new $Grid2({ placeAt: __layoutC.attachGrid() });
		/* 创建Grid3 */
		var __grid3 = new $Grid3({ placeAt: __layoutD.attachGrid() });

		/*<!-- 各种事件 */
		__tree.onClick(function($params){
			__tbar.defaultState();
			__grid.loadData($params.id);
			__grid2.clearAll();
			__grid3.clearAll();
		});
		__grid.onCheck(function($rowsize){
			__tbar.showEdit($rowsize == 1);
			__tbar.showDel($rowsize > 0);
		});
		__grid.onRowSelect(function($params){
			__grid2.loadData($params);
			__grid3.clearAll();
		});
		__grid2.onRowSelect(function($params){
			__grid3.loadData($params);
		});
		/* 点击工具栏的新增按钮 */
		__tbar.onClickNew(function(){
			if(__form) __form.showNew();
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