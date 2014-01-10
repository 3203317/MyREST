fo._sys._code._addCodeFrm = {
	id: fo._sys._code.id +"_addCodeFrm",
	parent: fo._sys._code,
	pid: fo._sys._code.id +"_addCodeDlg_",
	_init: function(){
		this._set();
	},
	_reset: function(){
		if(this.parent._addCodeDlg.get("model") == "Create"){
			dijit.byId(this.id +"_codeText").set("disabled",false);
			dijit.byId(this.id).reset();
		}else if(this.parent._addCodeDlg.get("model") == "CreateChild"){
			dijit.byId(this.id +"_codeText").set("disabled",false);
			dijit.byId(this.id).reset();
		}else{
			this._set();
		}
	},
	_set: function(){
		if(this.parent._addCodeDlg.get("model") == "Create"){
			dijit.byId(this.id +"_codeText").set("disabled",false);
			this._reset();
		}else if(this.parent._addCodeDlg.get("model") == "CreateChild"){
			dijit.byId(this.id +"_codeText").set("disabled",false);
			this._reset();
		}else{
			var __selItems = this.parent._codeGrid.selection.getSelected();
			if(__selItems.length != 1) return;
			var __selItem = __selItems[0];
			dijit.byId(this.id +"_codeText").set("disabled",true);
			dijit.byId(this.id +"_codenameText").set("value",__selItem.codename[0]);
			dijit.byId(this.id +"_codedescText").set("value",__selItem.codedesc[0]);
			dijit.byId(this.id +"_codeText").set("value",__selItem.code[0]);
			dijit.byId(this.id +"_sortText").set("value",__selItem.sort[0]);
		}
		this._valid();
	},
	_submit: function(){
		dijit.byId(this.id +"_codeText").set("disabled",false);
		dijit.byId(this.pid +"submitBtn").set("disabled", true);
		var __newItem = dijit.byId(this.id).getValues();
		dijit.byId(this.id +"_codeText").set("disabled",true);
		__newItem.tab_p_codetype_id = this.parent._codeTypeGrid.selection.getSelected()[0].id[0];
		__newItem.type = "code";
		
		var __codeTreeGrid = this.parent._codeGrid;

		if(this.parent._addCodeDlg.get("model") == "Create"){
			__newItem.p_code = "0";
			__codeTreeGrid.store.newItem(__newItem);
		}else if(this.parent._addCodeDlg.get("model") == "CreateChild"){
			var __selItem_3 = __codeTreeGrid.selection.getSelected()[0];
			__newItem.p_code = __selItem_3.code[0];
			var __pItem_3 = { parent: __selItem_3, attribute: "children" };
			__codeTreeGrid.store.newItem(__newItem, __pItem_3);
		}else{
			var __selItem_3 = __codeTreeGrid.selection.getSelected()[0];
			__newItem.id = __selItem_3.id[0];
			__codeTreeGrid.store.setAllValue(__newItem);
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
	$connect.connect(dijit.byId(fo._sys._code._addCodeFrm.pid),"onUnload",function(){});

	$connect.connect(dijit.byId(fo._sys._code._addCodeFrm.pid),"onLoad",function(){
		$connect.connect(dijit.byId(fo._sys._code._addCodeFrm.id), "onValidStateChange", fo._sys._code._addCodeFrm, "_valid");
		$connect.connect(dijit.byId(fo._sys._code._addCodeFrm.pid +"submitBtn"), "onClick", fo._sys._code._addCodeFrm, "_submit");

		$connect.connect(dijit.byId(fo._sys._code._addCodeFrm.pid +"resetBtn"), "onClick", fo._sys._code._addCodeFrm, "_reset");
		$connect.connect(fo._sys._code._addCodeDlg, "onShow", fo._sys._code._addCodeFrm, "_set");

		fo._sys._code._addCodeFrm._init();
	});
});