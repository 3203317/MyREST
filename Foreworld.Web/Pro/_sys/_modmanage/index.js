dijit.byId("main_tab_5").on("unLoad", function(){
	fo._sys._modmanage._addDlg.destroy();
	fo._sys._modmanage = null;
});

dijit.byId("main_tab_5").on("load", function(){
	require(["dojo/_base/connect",
		"fore/EnhancedTree",
		"dojo/_base/declare",
		"dojo/dom",
		"dijit/tree/ForestStoreModel",
		"dojox/grid/EnhancedGrid",
		"dojo/data/ItemFileWriteStore",
		"fore/data/ItemFileWriteStore",
		"dijit/Toolbar",
		"dijit/form/Button", 
		"dijit/Tooltip",
		"dojo/i18n!general/_sys/_modmanage/nls/index",
		"fore/dialog/FormDialog",
		"dojo/_base/lang",
		"dojo/_base/array",
		"dijit/Dialog",
		"dojox/grid/_CheckBoxSelector"],function($connect,$Tree,$declare,$dom,$ForestStoreModel,$EnhancedGrid,$oItemFileWriteStore,$ItemFileWriteStore,$Toolbar,$Button,$Tooltip,$i18n,$FormDialog,$lang,$array){

		fo._sys._modmanage = {
			id: "_fo_sys_modmanage",
			_init: function(){
				dijit.byId(this.id +"_toolbar_editBtnTip").set("label", $i18n.tip_editItem);
				dijit.byId(this.id +"_toolbar_delBtnTip").set("label", $i18n.tip_delItem);
				$connect.publish("/fore/ajax/", {
					url: "../Api.ashx?command=listModules",
					params: {},
					cbe: function(){ return true; },
					cba: $lang.hitch(this, "_createTreeStore")
				});
			},
			_createTreeStore: function($data){
				if($data == null || $data.opt != null) return;
				$data.items = fo.utils.data2TreeObject($data,0,{ 
					identifier: "id", 
					fidentifier: "p_id", 
					label: "modulename" 
				}).children;
				this._treeStore = new $ItemFileWriteStore({
					cmdInfo: ["addModule","updateModule","deleteModule"], 
					data: $data 
				});
				$connect.connect(this._treeStore,"onNewItem",$lang.hitch(this,function($item){
					this._modGrid.store.newItem($item);
					this._hideAddDlg();
				}));
				$connect.connect(this._treeStore,"onDeletes",$lang.hitch(this,function($ids){
					$array.forEach($ids,$lang.hitch(this,function($id){
						this._modGrid.store.fetchItemByIdentity({
							identity: $id,
							onItem: $lang.hitch(this,function($item){
								this._modGrid.store.deleteItem($item);
							})
						});
					}));
				}));
				$connect.connect(this._treeStore,"onSetAllValue",$lang.hitch(this,function($newValue){
					this._modGrid.store.fetchItemByIdentity({
						identity: $newValue.id,
						onItem: $lang.hitch(this,function($item){
							for(var __prop_3 in $newValue){
								if(__prop_3 != "id"){
									this._modGrid.store.setValue($item, __prop_3, $newValue[__prop_3]);
								}
							}
							this._hideAddDlg();
						})
					});				
				}));
			},
			_createTree: function(){
				if(this._treeStore == null) return;

				this._modTree = new $Tree({
					model: new $ForestStoreModel({
						store: this._treeStore,
						query: { "type": "module" },
						rootId: "0",
						rootLabel: fo.base.corp,
						childrenAttrs: ["children"]
					}), 
					showRoot: true, 
					openOnDblClick: true,
					style: { height: "100%", margin: 0, padding: 0 }
				},this.id +"_modTree");

				this._modTree.set("path", ["0"]);

				$connect.connect(this._modTree,"onSelected",this,function($item){
					this._loadModChild($item.id);
				});
			},
			_loadModChild: function($pModId){	
				$connect.publish("/fore/ajax/", {
					url: "../Api.ashx?command=listModules",
					params: { p_id: $pModId },
					cbe: function(){ return true; },
					cba: $lang.hitch(this, "_loadModChild_")
				});
			},
			_loadModChild_: function($data){
				this._modGrid.selection.clear();
				if($data == null || $data.opt != null) {
					this._modGrid.setStore(null);
				}else{
					this._modGrid.setStore(new $oItemFileWriteStore({ 
						data: $data 
					}));
				}
			},
			_createGrid: function(){
				this._modGrid = new $EnhancedGrid({
					style: { height: "100%", margin: 0, padding: 0 },
					structure: [{
							type: "dojox.grid._CheckBoxSelector"
						}, { 
							//cells: [{ name: "编号", field: "id", width: "35px", datatype: "number", editable: false }], 
							cells: [{ name: "模块名称", field: "modulename", width: "150px" }], 
							noscroll: true 
						}, [
							{ name: "图标", field: "icon", width: "100px" },
							{ name: "排序", field: "sort", width: "80px" },
							{ name: "模块地址", field: "href", width: "300px" },
							{ name: "创建时间", field: "addtime", width: "120px" },
							{ name: "创建人", field: "opt_s_user_name", width: "80px" },
							{ name: "状态", field: "isenable_text" }
						]
					]
				},this.id +"_modGrid");
				this._modGrid.startup();

				$connect.connect(this._modGrid, "onSelected", this, "_setButtonDisable");
				$connect.connect(this._modGrid, "onDeselected", this, "_setButtonDisable");
				$connect.connect(this._modGrid, "onDblClick", this, "_showEditDlg");

				this._loadModChild(0);
			},
			_showAddDlg: function(){
				this._addDlg.set("title","新增");  
				this._addDlg.set("model","Create");
				this._addDlg.show();
			},
			_showEditDlg: function(){
				this._addDlg.set("title","编辑");  
				this._addDlg.set("model","Update");
				this._addDlg.show();
			},
			_hideAddDlg: function(){
				this._addDlg.hide();
			},
			_delModules: function(){
				var __selItems = this._modGrid.selection.getSelected();
				if(__selItems.length > 0){
					var __ids = [];
					dojo.forEach(__selItems, function($item){ 
						__ids.push($item.id[0]-0); 
					});
					$connect.publish("/fo/show/delDlg/", [$lang.hitch(this,function($ids){
						this._treeStore.deleteItems($ids);
					}), __ids]);
				}
			},
			_setButtonDisable: function(){
				dijit.byId(this.id +"_toolbar_editBtn").set("disabled", this._modGrid.selection.getSelectedCount() != 1);
				dijit.byId(this.id +"_toolbar_delBtn").set("disabled", this._modGrid.selection.getSelectedCount() == 0);
			},
			_addDlg: new $FormDialog({
				id: "_fo_sys_modmanage_addDlg",
				href: "_sys/_modmanage/add.html",
				style: {
					height: "170px",
					width: "380px"
				},
				buttons: [
					["test1Btn","测试1"],
					["submitBtn","保存"],
					["resetBtn","重置"]
				]
			}),
			_complete: function(){
				$connect.connect(dijit.byId(this.id +"_toolbar_addBtn"), "onClick", this, "_showAddDlg");
				$connect.connect(dijit.byId(this.id +"_toolbar_editBtn"), "onClick", this, "_showEditDlg");
				$connect.connect(dijit.byId(this.id +"_toolbar_delBtn"), "onClick", this, "_delModules");
				dijit.byId(this.id).layout();
			}
		};

		$connect.connect(fo._sys._modmanage,"_createTreeStore",fo._sys._modmanage,"_createTree");
		$connect.connect(fo._sys._modmanage,"_init",fo._sys._modmanage,"_createGrid");
		$connect.connect(fo._sys._modmanage,"_createGrid",fo._sys._modmanage,"_complete");

		fo._sys._modmanage._init();
	});
});

require(["dijit/form/Form",
	"dijit/form/NumberSpinner"], function(){});