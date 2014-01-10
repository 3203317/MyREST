dijit.byId("main_tab_41").on("unLoad", function(){
	fo._sys._code._addCodeTypeDlg.destroy();
	fo._sys._code._addCodeDlg.destroy();
	fo._sys._code = null;
});

dijit.byId("main_tab_41").on("load", function(){
	require(["dojo/_base/connect",
		"dojox/grid/TreeGrid",
		"fore/dialog/FormDialog",
		"fore/EnhancedTree",
		"dojo/_base/declare",
		"dojo/dom",
		"dijit/tree/ForestStoreModel",
		"dojox/grid/EnhancedGrid",
		"fore/data/ItemFileWriteStore",
		"dojo/i18n!general/_sys/_code/nls/index",
		"dojo/_base/lang",
		"dojo/_base/array",
		"dojox/grid/_CheckBoxSelector",
		"dojox/grid/_RadioSelector",
		"dojo/data/ItemFileWriteStore"],function($connect,$TreeGrid,$FormDialog,$Tree,$declare,$dom,$ForestStoreModel,$EnhancedGrid,$ItemFileWriteStore,$i18n,$lang,$array){

		fo._sys._code = {
			id: "_fo_sys_code",
			_init: function(){
				dijit.byId(this.id +"_codeToolbar_editBtnTip").set("label", $i18n.tip_editItem);
				dijit.byId(this.id +"_codeToolbar_delBtnTip").set("label", $i18n.tip_delItem);
			},
			_delCodeType: function(){
				var __selItem = this._codeTypeGrid.selection.getSelected()[0];
				$connect.publish("/fo/show/delDlg/", [$lang.hitch(this,function($ids){
					this._codeTypeStore.deleteItems($ids);
				}), __selItem.id]);
			},
			_delCode: function(){
				var __selItem = this._codeGrid.selection.getSelected()[0];
				this._codeGrid.selection.clear();
				$connect.publish("/fo/show/delDlg/", [$lang.hitch(this,function($ids){
					this._codeTreeModel.store.deleteItems($ids);
				}), __selItem.id]);
			},
			_loadCodeTypes: function($data){
				if($data == null || $data.opt != null) return;
				this._codeTypeGrid.selection.clear();

				this._codeTypeStore = new $ItemFileWriteStore({ 
					cmdInfo: ["addCodeType","updateCodeType","deleteCodeType"], 
					data: $data 
				});

				$connect.connect(this._codeTypeStore,"onNewItem",this,"_hideAddCodeTypeDlg");
				$connect.connect(this._codeTypeStore,"onSetAllValue",this,"_hideAddCodeTypeDlg");
				$connect.connect(this._codeTypeStore,"onDeletes",this,"_selectFirstOfCodeTypeGrid");

				this._codeTypeGrid.setStore(this._codeTypeStore);
				this._selectFirstOfCodeTypeGrid();
			},
			_selectFirstOfCodeTypeGrid: function(){
				this._codeTypeGrid.selection.setSelected(0,true);
			},
			_createCodeTypeGrid: function(){
				this._codeTypeGrid = new $EnhancedGrid({
					style: { height: "100%", margin: 0, padding: 0 },
					structure: [{
							type: "dojox.grid._RadioSelector"
						}, { 
							cells: [{ name: "代码类型名称", field: "id", width: "80px" }], 
							noscroll: true 
						}, [
							{ name: "代码类型描述", field: "codetypedesc", width: "100px" },
							{ name: "创建时间", field: "addtime", width: "120px" },
							{ name: "创建人", field: "opt_s_user_name", width: "80px" },
							{ name: "状态", field: "isenable_text" }
						]
					]
				},this.id +"_codeTypeGrid");
				this._codeTypeGrid.startup();
				
				$connect.connect(this._codeTypeGrid,"onSelected",$lang.hitch(this,function(){
					this._loadCodes(this._codeTypeGrid.selection.getSelected()[0].id[0]);
				}));
				$connect.connect(this._codeTypeGrid, "onDblClick", this, "_showEditCodeTypeDlg");

				$connect.publish("/fore/ajax/", {
					url: "../Api.ashx?command=listCodeTypes",
					params: {},
					cbe: function(){ return true; },
					cba: $lang.hitch(this,"_loadCodeTypes")
				});
			},
			_loadCodes: function($codetype){	
				$connect.publish("/fore/ajax/", {
					url: "../Api.ashx?command=listCodes",
					params: { tab_p_codetype_id: $codetype },
					cbe: function(){ return true; },
					cba: $lang.hitch(this, "_setStoreOfCodeGrid")
				});
			},
			_resetAddCodeFrm: function(){
				if(this._addCodeFrm != undefined){
					this._addCodeFrm._reset();
				}
			},
			_setStoreOfCodeGrid: function($data){
				this._codeGrid.selection.clear();
				if($data == null || $data.opt != null) {
					this._codeGrid.setStore(null);
					dijit.byId(this.id +"_codeTypeToolbar_delBtn").set("disabled", true);
				}else{
					this._codeTreeModel.destroy();
					this._codeTreeModel.store = null;
					this._codeTreeModel = null;
					var __items = fo.utils.data2TreeObject($data,0,{ 
						identifier: "code", 
						fidentifier: "p_code", 
						label: "codename" 
					}).children;
					this._codeTreeModel = new $ForestStoreModel({
						store: new $ItemFileWriteStore({ 
							cmdInfo: ["addCode","updateCode","deleteCode"], 
							data: {
								identifier: "id",
								label: "codename",
								items: __items == undefined ? [] : __items
							}
						}),
						query: { type: "code" },
						rootId: "0",
						rootLabel: "代码名称",
						childrenAttrs: [ "children" ]
					});

					$connect.connect(this._codeTreeModel.store,"onNewItem",this,"_hideAddCodeDlg");
					$connect.connect(this._codeTreeModel.store,"onNewItem",this,"_setButtonDisable");
					$connect.connect(this._codeTreeModel.store,"onSetAllValue",this,"_hideAddCodeDlg");
					$connect.connect(this._codeTreeModel.store,"onNewItemError",this,"_resetAddCodeFrm");
					$connect.connect(this._codeTreeModel.store,"onSetAllValueError",this,"_resetAddCodeFrm");
					$connect.connect(this._codeTreeModel.store,"onDeletes",this,"_setButtonDisable");
					this._codeGrid.setModel(this._codeTreeModel);
					this._setButtonDisable();
				}
			},
			_createCodeGrid: function(){
				this._codeTreeModel = new $ForestStoreModel({
					store: new $ItemFileWriteStore({
						data: {
							identifier: "id",
							label: "codename",
							items: []
						}
					}),
					query: { type: "code" },
					rootId: "0",
					rootLabel: "代码名称",
					childrenAttrs: [ "children" ]
				});
				this._codeGrid = new $TreeGrid({
					keepSelection: true,
					defaultOpen: true,
					rowSelector: true,
					style: { height: "100%", margin: 0, padding: 0 },
					treeModel: this._codeTreeModel,
					structure: [
						{ name: "代码名称", field: "codename", width: "150px" },
						{ name: "代码描述", field: "codedesc", width: "100px" },
						{ name: "自定义编码", field: "code", width: "100px" },
						{ name: "排序", field: "sort", width: "100px" },
						{ name: "创建时间", field: "addtime", width: "120px" },
						{ name: "创建人", field: "opt_s_user_name", width: "80px" },
						{ name: "状态", field: "isenable_text" }
					]		
				},this.id +"_codeGrid");
				this._codeGrid.startup();

				$connect.connect(this._codeGrid, "onSelected", this, "_setButtonDisable");
				$connect.connect(this._codeGrid, "onDeselected", this, "_setButtonDisable");
				$connect.connect(this._codeGrid, "onDblClick", this, "_showEditCodeDlg");
			},
			_showAddCodeTypeDlg: function(){
				this._addCodeTypeDlg.set("title","新增");  
				this._addCodeTypeDlg.set("model","Create");
				this._addCodeTypeDlg.show();
			},
			_showEditCodeTypeDlg: function(){
				this._addCodeTypeDlg.set("title","编辑");  
				this._addCodeTypeDlg.set("model","Update");
				this._addCodeTypeDlg.show();
			},
			_hideAddCodeTypeDlg: function(){
				this._addCodeTypeDlg.hide();
			},
			_showAddCodeDlg: function(){
				this._addCodeDlg.set("title","新增");  
				this._addCodeDlg.set("model","Create");
				this._addCodeDlg.show();
			},
			_showAddCodeChildDlg: function(){
				this._addCodeDlg.set("title","新增子项");  
				this._addCodeDlg.set("model","CreateChild");
				this._addCodeDlg.show();
			},
			_showEditCodeDlg: function(){
				this._addCodeDlg.set("title","编辑");  
				this._addCodeDlg.set("model","Update");
				this._addCodeDlg.show();
			},
			_hideAddCodeDlg: function(){
				this._addCodeDlg.hide();
			},
			_addCodeTypeDlg: new $FormDialog({
				id: "_fo_sys_code_addCodeTypeDlg",
				href: "_sys/_code/addCodeType.html",
				style: { height: "124px", width: "380px" }
			}),
			_addCodeDlg: new $FormDialog({
				id: "_fo_sys_code_addCodeDlg",
				href: "_sys/_code/addCode.html",
				style: { height: "170px", width: "380px" }
			}),
			_setButtonDisable: function(){
				var __selOne = this._codeGrid.selection.getSelectedCount() != 1;
				dijit.byId(this.id +"_codeToolbar_editBtn").set("disabled", __selOne);
				dijit.byId(this.id +"_codeToolbar_delBtn").set("disabled", __selOne ? true : (this._codeGrid.selection.getSelected()[0].children != undefined));
				dijit.byId(this.id +"_codeToolbar_addChildBtn").set("disabled", __selOne);
				dijit.byId(this.id +"_codeTypeToolbar_delBtn").set("disabled", this._codeGrid.store._getItemsArray().length > 0);
			},
			_complete: function(){
				$connect.connect(dijit.byId(this.id +"_codeTypeToolbar_addBtn"), "onClick", this, "_showAddCodeTypeDlg");
				$connect.connect(dijit.byId(this.id +"_codeTypeToolbar_editBtn"), "onClick", this, "_showEditCodeTypeDlg");
				$connect.connect(dijit.byId(this.id +"_codeTypeToolbar_delBtn"), "onClick", this, "_delCodeType");
				$connect.connect(dijit.byId(this.id +"_codeToolbar_addBtn"), "onClick", this, "_showAddCodeDlg");
				$connect.connect(dijit.byId(this.id +"_codeToolbar_addChildBtn"), "onClick", this, "_showAddCodeChildDlg");
				$connect.connect(dijit.byId(this.id +"_codeToolbar_editBtn"), "onClick", this, "_showEditCodeDlg");
				$connect.connect(dijit.byId(this.id +"_codeToolbar_delBtn"), "onClick", this, "_delCode");
				dijit.byId(this.id).layout();
			}
		};

		$connect.connect(fo._sys._code,"_init",fo._sys._code,"_createCodeTypeGrid");
		$connect.connect(fo._sys._code,"_init",fo._sys._code,"_createCodeGrid");
		$connect.connect(fo._sys._code,"_createCodeGrid",fo._sys._code,"_complete");

		fo._sys._code._init();
	});
});

require(["dijit/form/Form",
	"dijit/form/NumberSpinner"], function(){});