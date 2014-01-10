/**
 * 作者：黄鑫
 * 日期：2013-07-23
 * 描述：左侧菜单树
 */
define(["jquery",
	"dhtmlxtree_json"], function($) {
	/* 数据转树对象 */
	var _data2TreeObject = function($data,$pIdVal,$treeObj){
		var __treeItem = [];
		for(var __i_3=0,__item_3;__item_3=$data.items[__i_3];__i_3++){
			if(__item_3["PModuleId"] == $pIdVal) {
				var __item_4 = {
					id: __item_3["ModuleId"],
					text: __item_3["ModuleName"]
				};
				__treeItem.push(__item_4);

				/* 用户自定义属性 */
				var __userDataArr_4 = [];
				for(var __item_5 in __item_3){
					var __userDataObj_4 = {};
					__userDataObj_4["name"] = __item_5;
					__userDataObj_4["content"] = __item_3[__item_5];
					__userDataArr_4.push(__userDataObj_4);
				}
				__item_4["userdata"] = __userDataArr_4;

				_data2TreeObject($data,__item_4["id"],__item_4);
			}
		}
		if(__treeItem.length > 0) {
			$treeObj["item"] = __treeItem;
		}else{
			if($treeObj["item"] == null) delete $treeObj.item;
		}
	};

	/* 菜单树 */
	var _tree = function($params){
		var __tree = new dhtmlXTreeObject($params.placeAt,"100%","100%",0);
		__tree.setSkin("dhx_skyblue");
		__tree.setImagePath(cdnPath + dhxPath +"dhtmlxTree/codebase/imgs/csh_vista/");
		/* 为树注册点击事件 */
		__tree.setOnSelectStateChange(function($id){
			var __params = {
				id: $id,
				name: this.getItemText($id),
				href: this.getUserData($id, "ModuleUrl")
			};
			if(__event.clickTree) __event.clickTree(__params);
		});

		/* 载入树数据 */
		var __loadData = function($data){	
			/* 树的根对象 */
			var __treeObj = { id: "root", item: [], text: "根", select: 1 };	
			_data2TreeObject($data,0,__treeObj);
			var __rootObj = { id: 0, item: [] };
			__rootObj.item.push(__treeObj);
			__tree.loadJSONObject(__rootObj, function(){
				__tree.openItem(__tree.getSelectedItemId());
			});		
		};

//		$.ajax({
//			type: "get",
//			url: "../../Api.ashx?command=listModules",
//			dataType: "json",
//			async: true,
//			data: { ts: Date.parse(new Date())/1000 },
//			beforeSend: function($xhr){
//				$xhr.overrideMimeType("text/plain;charset=utf-8");
//			}
//		}).done(function($data){
//			if($data.status == "failure"){
//				console.log($data);
//			}else if($data.status == "timeout"){
//				top.location.href = "../login.html?locale="+ config.locale;
//			}else{			
//				__loadData($data);
//			}
//		});

		/* 事件注册及返回对象 */
		var __event = {};
		var __r = {
			onClick: function($fun){
				__event.clickTree = $fun;
			}
		};
				
        __loadData({ items: moduleTree });
        		
		return __r;
	};
	return _tree;
});