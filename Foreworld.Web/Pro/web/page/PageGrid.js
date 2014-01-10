/**
 * 作者：黄鑫
 * 日期：2013-09-04
 * 描述：右侧Grid
 */
define(["jquery",
	"dhtmlxgrid_excell_cntr"], function($) {
	/* 菜单树 */
	var _grid = function($params){
		/* 创建Grid */
		var __grid = $params.placeAt;
		__grid.setImagePath(cdnPath + dhxPath +"dhtmlxGrid/codebase/imgs/");
		__grid.setHeader("ID,#master_checkbox,数据源描述,数据源类型,SQL语句,测试参数,参数公式,创建时间,创建人,状态",null,[null,"text-align:left;"]);
		__grid.setInitWidths("30,40,150,80,350,120,120,120,100,100");
		__grid.setColAlign("center,center,left,left,left,left,left,left,left,left");
		__grid.setColTypes("cntr,ch,ro,ro,ro,ro,ro,ro,ro,ro");
		__grid.setColSorting("int,intr,str,str,str,str,str,date,str,str");
		__grid.setSkin("dhx_skyblue");
		__grid.setColumnColor("#CCE2FE");
		__grid.init2();

		/* 全选checkbox */
		//var __allChk = $("#"+ __grid.entBox.id +" input:checkbox")[0];
		/* 事件：选中一行时，清除第1列所有checkbox和全选checkbox 
		__grid.attachEvent("onRowSelect",function($id,$col){
			__grid.setCheckedRows(1, 0);
			__allChk.checked = false;
			__grid.cells($id,1).setChecked(1);
		});*/

		/* 选中grid的check回调函数 */
		__grid.attachEvent("onCheckbox",function($id,$col,$state){
			if(__event.checkedGrid){
				var __rows = __grid.getCheckedRows(1);
				var __rowsize = __rows == "" ? 0 : __rows.split(",").length;
				__event.checkedGrid(__rowsize);
			}
		});

		__grid.attachEvent("onRowSelect", function($id){
			__event.selectedGrid({ id: $id });
		});

		/* 载入数据给Grid */
		var __loadData = function($data){
			__grid.clearAll();
			__grid.clearSelection2();
			__grid.parse($data,"json");
		};

		/* 加载Grid数据 */
		var __ajax = function($id){
			if($id == 0){
				__grid.clearAll();
				__grid.clearSelection2();
			}else{
				$.ajax({
					type: "get",
					url: "../../Api.ashx?command=listPageDss",
					dataType: "json",
					async: true,
					data: { format: "dhx", pageid: $id, ts: Date.parse(new Date())/1000 },
					beforeSend: function($xhr){
						$xhr.overrideMimeType("text/plain;charset=utf-8");
					}
				}).done(function($data){
					if($data.status == "failure"){
						console.log($data);
					}else if($data.status == "timeout"){
						top.location.href = "../login.html?locale="+ config.locale;
					}else{
						__loadData($data);
					}
				});
			}
		};

		/* 事件注册及返回对象 */
		var __event = {};
		var __r = {
			loadData: __ajax,
			onCheck: function($fun){
				__event.checkedGrid = $fun;
			},
			onRowSelect: function($fun){
				__event.selectedGrid = $fun;
			},
			getCheckedData: function(){
				var t = __grid;
				var __rownum = t.getCheckedRows(1).split(",")[0];
				var __data = {
					id: __rownum,
					name: t.cells(__rownum,2).getValue(),
					desc: t.cells(__rownum,4).getValue(),
					href: t.cells(__rownum,3).getValue(),
					icon: t.cells(__rownum,5).getValue(),
					sort: t.cells(__rownum,6).getValue()
				};
				return __data;
			}
		};
		return __r;
	};
	return _grid;
});