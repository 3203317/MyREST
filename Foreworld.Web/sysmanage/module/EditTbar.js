/**
 * 作者：黄鑫
 * 日期：2013-07-23
 * 描述：编辑工具栏
 */
define(["jquery",
	"dhtmlxtoolbar"], function($) {

	/* 描述：工具栏 */
	var _modTbar = function($params){
		/* 创建Grid */
		var __tbar = $params.placeAt;
		__tbar.setIconsPath("../images/tbar/");
		__tbar.loadXML("module/editbar.xml?etc=" + new Date().getTime(), function(){
			console.log(arguments);
		});
		__tbar.attachEvent("onClick", function($id){
			switch($id){
				case "new":
					__event.clickNew();
					break;
			}
		});

		/* 事件注册及返回对象 */
		var __event = {};
		var __modTbar = {
			showEdit: function($true){
				if($true){
					__tbar.enableItem("edit");
				}else{
					__tbar.disableItem("edit");
				}
			},
			showDel: function($true){
				if($true){
					__tbar.enableItem("del");
				}else{
					__tbar.disableItem("del");
				}
			},
			onClickNew: function($fun){
				__event.clickNew = $fun;
			}
		};
		return __modTbar;
	};
	return _modTbar;
});