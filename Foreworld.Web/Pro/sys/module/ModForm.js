/**
 * 作者：黄鑫
 * 日期：2013-07-23
 * 描述：表单窗口
 */
define(["jquery",
	"dhtmlxwindows"], function($) {

	/* 菜单树 */
	var _from = function($params){
		var __wins,__w1;

		/* 创建win */
		var __createWin = function(){
			__wins = new dhtmlXWindows();
			__wins.setImagePath(cdnPath + dhxPath +"dhtmlxWindows/codebase/imgs/");
			__w1 = __wins.createWindow("w1", 30, 40, 320, 240); 
			__w1.attachURL("module/form.html");

			/* 添加关闭事件，但不真正关闭，只隐藏 */
			__wins.attachEvent("onClose",function($win){
				$win.setModal(false);
				$win.hide();
				$win.getFrame().contentWindow.closeAndClear();
				return false;
			});

			__w1.hide();
		};
		__createWin();

		/* 事件注册及返回对象 */
		var __event = {};
		var __r = {
			onClick: function($fun){
				__event.clickTree = $fun;
			},
			showNew: function(){
				//if(!__w1) __createWin();
				__w1.setText("新增");
				__w1.setModal(true);
				__w1.center();
				__w1.show();
			},
			showEdit: function($data){
				//if(!__w1) __createWin();
				__w1.setText("编辑");
				__w1.setModal(true);
				__w1.center();
				__w1.show();
				/* 设置表单数据 */
				__w1.getFrame().contentWindow.set($data);
			}
		};
		return __r;
	};
	return _from;
});