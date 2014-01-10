/**
 * 作者：黄鑫
 * 日期：2013-09-07
 * 描述：右下侧Grid
 */
define(["jquery",
	"dhtmlxgrid_excell_cntr"], function($) {
	/* 菜单树 */
	var _grid = function($params){
		/* 创建Grid */
		var __grid = $params.placeAt;
		__grid.setImagePath(cdnPath + dhxPath +"dhtmlxGrid/codebase/imgs/");
		__grid.setHeader("ID,,参数名称,参数描述,对应字段",null,[null,"text-align:left;"]);
		__grid.setInitWidths("30,0,150,150,150");
		__grid.setColAlign("center,left,left,left,left");
		__grid.setColTypes("cntr,ro,ro,ro,ro");
		__grid.setColSorting("int,str,str,str,str");
		__grid.setSkin("dhx_skyblue");
		__grid.setColumnColor("#CCE2FE");
		__grid.init2();

		/* 选中grid的check回调函数 */

		/* 载入数据给Grid */
		var __loadData = function($data){
			__grid.clearAll();
			__grid.parse($data,"json");
		};

		/* 加载Grid数据 */
		var __ajax = function($params){
			if($params.id == 0){
				__grid.clearAll();
			}else{
				$.ajax({
					type: "get",
					url: "../../Api.ashx?command=listPageDsWdParams",
					dataType: "json",
					async: true,
					data: { format: "dhx", pagedswgid: $params.id, ts: Date.parse(new Date())/1000 },
					beforeSend: function($xhr){
						$xhr.overrideMimeType("text/plain;charset=utf-8");
					}
				}).done(function($data){
					if($data.status == "failure"){
						__grid.clearAll();
					}else if($data.status == "timeout"){
						top.location.href = "../login.html?locale="+ config.locale;
					}else{
						__loadData($data);
					}
				});
			}
		};

		var __clearAll = function(){
			__grid.clearAll();
		};

		/* 事件注册及返回对象 */
		var __event = {};
		var __r = {
			loadData: __ajax,
			clearAll: __clearAll
		};
		return __r;
	};
	return _grid;
});