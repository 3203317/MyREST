/**
 * 作者：黄鑫
 * 日期：2013-07-23
 * 描述：右侧Grid
 */
define(["jquery",
	"dhtmlxgrid_excell_cntr"], function($) {
	/* 测试数据 */
	var __testData = {
		rows: [{
			id: 1001, 
			data: [
				"100",
				"A Time to Kill",
				"John Grisham",
				"12.99",
				"1",
				"05/01/1998"
			]
		},{
			id: 1002, 
			data: [
				"1000",
				"Blood and Smoke",
				"Stephen King",
				"0",
				"1",
				"01/01/2000"
			] 
		}]
	};

	/* 菜单树 */
	var _grid = function($params){
		/* 创建Grid */
		var __grid = $params.placeAt;
		__grid.setImagePath(cdnPath + dhxPath +"dhtmlxGrid/codebase/imgs/");
		__grid.setHeader("ID,#master_checkbox,模块名称,模块地址,模块描述,图标,排序,创建时间,创建人,状态",null,[null,"text-align:left;"]);
		__grid.setInitWidths("30,40,150,200,100,100,100,120,100,100");
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

		/* 载入数据给Grid */
		var __loadData = function($data){
			__grid.clearAll();
			__grid.clearSelection2();
			__grid.parse($data,"json");
		};

		/* 加载Grid数据 */
		var __ajax = function($id){
			$.ajax({
				type: "get",
				url: "../../Api.ashx?command=listModules",
				dataType: "json",
				async: true,
				data: { format: "dhx", p_id: ($id == "root" ? 0 : $id), ts: Date.parse(new Date())/1000 },
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
		};

		/* 事件注册及返回对象 */
		var __event = {};
		var __r = {
			loadData: __ajax,
			onCheck: function($fun){
				__event.checkedGrid = $fun;
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