fo._sys._modmanage._addFrm = {
	id: fo._sys._modmanage.id +"_addFrm",
	parent: fo._sys._modmanage,
	pid: fo._sys._modmanage.id +"_addDlg_",
	_init: function(){
		this._set();
	},
	_reset: function(){
		if(this.parent._addDlg.get("model") == "Create"){
			dijit.byId(this.id).reset();
		}else{
			this._set();
		}
	},
	_set: function(){
		if(this.parent._addDlg.get("model") == "Create"){
			this._reset();
		}else{
			var __selItems = this.parent._modGrid.selection.getSelected();
			if(__selItems.length != 1) return;
			var __selItem = __selItems[0];
			dijit.byId(this.id +"_modulenameText").set("value",__selItem.modulename[0]);
			dijit.byId(this.id +"_iconText").set("value",__selItem.icon[0]);
			dijit.byId(this.id +"_hrefText").set("value",__selItem.href[0]);
			dijit.byId(this.id +"_sortText").set("value",__selItem.sort[0]);
		}
		this._valid();
	},
	_submit: function(){
		dijit.byId(this.pid +"submitBtn").set("disabled", true);
		var __newItem = dijit.byId(this.id).getValues();
		__newItem.p_id = this.parent._modTree.selectedItem.id;
		__newItem.type = "module";
		if(this.parent._addDlg.get("model") == "Create"){
			var __selItem_3 = this.parent._modTree.selectedItem;
			var __pItem_3 = __selItem_3.id == 0 ? null : { parent: __selItem_3, attribute: "children" };
			this.parent._modTree.model.store.newItem(__newItem,__pItem_3);
			if(!this.parent._modTree.selectedNode.isExpanded) this.parent._modTree._expandNode(this.parent._modTree.selectedNode);
		}else{
			var __selItem_3 = this.parent._modGrid.selection.getSelected()[0];
			__newItem.id = __selItem_3.id[0];
			this.parent._modTree.model.store.setAllValue(__newItem);
		}
	},
	_valid: function(){
		var __btn_submit = dijit.byId(this.pid +"submitBtn");
		var __frm = dijit.byId(this.id);
		var __valid = __frm.isValid();
		__btn_submit.set("disabled",!__valid);
	}
};

require(["dojo/_base/connect"],function($connect){
	$connect.connect(dijit.byId(fo._sys._modmanage._addFrm.pid),"onUnload",function(){});

	$connect.connect(dijit.byId(fo._sys._modmanage._addFrm.pid),"onLoad",function(){
		$connect.connect(dijit.byId(fo._sys._modmanage._addFrm.id), "onValidStateChange", fo._sys._modmanage._addFrm, "_valid");
		$connect.connect(dijit.byId(fo._sys._modmanage._addFrm.pid +"submitBtn"), "onClick", fo._sys._modmanage._addFrm, "_submit");

		$connect.connect(dijit.byId(fo._sys._modmanage._addFrm.pid +"resetBtn"), "onClick", fo._sys._modmanage._addFrm, "_reset");
		$connect.connect(fo._sys._modmanage._addDlg, "onShow", fo._sys._modmanage._addFrm, "_set");

		$connect.connect(fo._sys._modmanage._treeStore, "onNewItemError", fo._sys._modmanage._addFrm, "_reset");
		$connect.connect(fo._sys._modmanage._treeStore, "onSetAllValueError", fo._sys._modmanage._addFrm, "_reset");

		fo._sys._modmanage._addFrm._init();
	});
});