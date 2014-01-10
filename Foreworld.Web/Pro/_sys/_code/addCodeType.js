fo._sys._code._addCodeTypeFrm = {
	id: fo._sys._code.id +"_addCodeTypeFrm",
	parent: fo._sys._code,
	pid: fo._sys._code.id +"_addCodeTypeDlg_",
	_init: function(){
		this._set();
	},
	_reset: function(){
		if(this.parent._addCodeTypeDlg.get("model") == "Create"){
			dijit.byId(this.id).reset();
		}else{
			this._set();
		}
	},
	_set: function(){
		if(this.parent._addCodeTypeDlg.get("model") == "Create"){
			dijit.byId(this.id +"_idText").set("disabled",false);
			this._reset();
		}else{
			var __selItems = this.parent._codeTypeGrid.selection.getSelected();
			if(__selItems.length != 1) return;
			var __selItem = __selItems[0];
			dijit.byId(this.id +"_idText").set("disabled",true);
			dijit.byId(this.id +"_idText").set("value",__selItem.id[0]);
			dijit.byId(this.id +"_codetypedescText").set("value",__selItem.codetypedesc[0]);
		}
		this._valid();
	},
	_submit: function(){
		dijit.byId(this.id +"_idText").set("disabled",false);
		dijit.byId(this.pid +"submitBtn").set("disabled", true);
		var __newItem = dijit.byId(this.id).getValues();
		dijit.byId(this.id +"_idText").set("disabled",true);
		if(this.parent._addCodeTypeDlg.get("model") == "Create"){
			this.parent._codeTypeGrid.store.newItem(__newItem);
		}else{
			this.parent._codeTypeGrid.store.setAllValue(__newItem);
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
	$connect.connect(dijit.byId(fo._sys._code._addCodeTypeFrm.pid),"onUnload",function(){});

	$connect.connect(dijit.byId(fo._sys._code._addCodeTypeFrm.pid),"onLoad",function(){
		$connect.connect(dijit.byId(fo._sys._code._addCodeTypeFrm.id), "onValidStateChange", fo._sys._code._addCodeTypeFrm, "_valid");
		$connect.connect(dijit.byId(fo._sys._code._addCodeTypeFrm.pid +"submitBtn"), "onClick", fo._sys._code._addCodeTypeFrm, "_submit");

		$connect.connect(dijit.byId(fo._sys._code._addCodeTypeFrm.pid +"resetBtn"), "onClick", fo._sys._code._addCodeTypeFrm, "_reset");
		$connect.connect(fo._sys._code._addCodeTypeDlg, "onShow", fo._sys._code._addCodeTypeFrm, "_set");

		$connect.connect(fo._sys._code._codeTypeStore, "onNewItemError", fo._sys._code._addCodeTypeFrm, "_reset");
		$connect.connect(fo._sys._code._codeTypeStore, "onSetAllValueError", fo._sys._code._addCodeTypeFrm, "_reset");

		fo._sys._code._addCodeTypeFrm._init();
	}); 
});