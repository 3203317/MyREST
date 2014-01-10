/**
 * 作者：黄鑫
 * 日期：2013-09-09
 * 描述：页面代码生成
 */
var console = console||{log:function(){return;}}
require.config(config);
require(["jquery",
	"backbone",
	"PageTree",
	"dhtmlxlayout",
	"dhtmlxeditor"], function($, $backbone, $Tree){

	var _template_html = ["<!DOCTYPE html>",
						"<html>",
						"  <head>",
						'    <meta http-equiv="Content-type" content="text/html;charset=utf-8" />',
						'    <meta http-equiv="X-UA-Compatible" content="IE=edge" />',
						"    <title>${title}</title>",
						'    <style type="text/css">',
						"    </style>",
						"  </head>",
						'  <body class="fore">',
						"    ${div}",
						'    <!--[if lt IE 10]> <script type="text/javascript" src="http://cdn:8082/cdn/js/html5shiv/3.6.2/html5.js"></script> <![endif]-->',
						"    <script>",
						'      var cdnPath = "http://cdn:8082/cdn/js/";',
						'      var dhxPath = "dhtmlx/3.6/";',
						"      var require = {",
						"        waitSeconds: 3,",
						'        urlArgs: "v="+ (new Date()).getTime()',
						"      };",
						"      var config = {",
						"        waitSeconds: 3,",
						'        locale: top.location.search.match(/locale=([\w\-]+)/) ? RegExp.$1 : "zh",',
						"        paths: {",
						"          ${widget}",
						'          jquery: cdnPath +"jquery/1.10.2/jquery.min",',
						'          i18n: cdnPath +"require/i18n",',
						'          text: cdnPath +"require/text",',
						'          underscore: cdnPath +"underscore/1.5.1/underscore.min",',
						'          backbone: cdnPath +"backbone/1.0.0/backbone.min",',
						'          util: cdnPath +"xlo/0.9/util"',
						"        },",
						"        shim: {",
						"          backbone: {",
                        '            deps: ["underscore"],',
                        '            exports: "Backbone"',
						"          }",
						"        }",
						"      };",
						"    </script>",
						'    <script data-main="${path}" src="http://cdn:8082/cdn/js/require/2.1.6/require.js"></script>',
						"  </body>",
						"</html>"].join("\r\n");

	var _template_js = ["/**",
						" * 作者：黄鑫",
						" * 日期：2013-09-09",
						" * 描述：",
						" */",
						"var console = console||{log:function(){return;}}",
						"require.config(config);",
						'require(["jquery",',
						'\t"util"${widget_r}],function($,$util${widget_r2}){',
						"\t$(document).ready(function(){",
						"\t\t${ds}",
						"\t});",
						"},function($err){",
						"\tvar _failedId = $err.requireModules && $err.requireModules[0];",
						'\tif (_failedId === "jquery"){',
						"\t\trequirejs.undef(_failedId);",
						"\t}",
						"});"].join("\r\n");

	var _loadHTML = function($data,$params){
		var __div = [];
		var __widget = [];
		for(var __p_3 in $data.items){
			var __item_4 = $data.items[__p_3];
			if(__item_4.v0 == "w"){
				__div.push("<div id='div_"+ __item_4.nodename +"'></div>");
				__widget.push(__item_4.v1+"_"+__item_4.v3+": cdnPath +'"+__item_4.v2+__item_4.v1+"',");
			}
		}
		/* div */
		_template_html = _template_html.replace("${div}",__div.join("\r\n    "));
		/* widget */
		_template_html = _template_html.replace("${widget}",_.union(__widget).join("\r\n          "));
		_template_html = _template_html.replace("${path}",$params.v1+"/"+$params.v1);
		_template_html = _template_html.replace("${title}",$params.v2);
		$("#texta_html").val(_template_html);
	};

	var _loadJS = function($data){
		var __ds = [];
		var __widget = [];
		var __widget2 = [];
		for(var __p_3 in $data.items){
			var __item_4 = $data.items[__p_3];
			if(__item_4.v0 == "d"){
				__ds.push('$util.ajjax("../../",{');
				__ds.push('\tcommand: "dataSource",');
				__ds.push('\tname: "'+ __item_4.nodename +'",');
				__ds.push('\tparams: '+ __item_4.v1);
				__ds.push('},function($data){');

				for(var __p_6 in $data.items){
					var __item_7 = $data.items[__p_6];
					if(__item_7.v0 == "w" && __item_7.p_id == __item_4.id){
						__ds.push("\t$"+__item_7.v1+"_"+__item_7.v3+"($data,{");

						var __params = [];
						for(var __p_9 in $data.items){
							var __item_10 = $data.items[__p_9];
							if(__item_10.v0 == "p" && __item_10.p_id == __item_7.id){
								__params.push(__item_10.nodename+': "'+__item_10.v1+'"');
							}
						}
						__ds.push("\t\t"+__params.join(",\r\n\t\t\t\t"));

						__ds.push("\t});");
					}
				}
				__ds.push("});");
			}
			if(__item_4.v0 == "w"){
				__widget.push(',\r\n\t"'+__item_4.v1+"_"+__item_4.v3+'"');
				__widget2.push(',$'+ __item_4.v1+"_"+__item_4.v3);
			}
		}
		_template_js = _template_js.replace("${ds}",__ds.join("\r\n\t\t"));
		_template_js = _template_js.replace("${widget_r}",_.union(__widget).join(""));
		_template_js = _template_js.replace("${widget_r2}",_.union(__widget2).join(""));
		$("#texta_js").val(_template_js);
	};

	var _loadData = function($params){
		if($params.id == 0){
			$("#texta_html").val("");
			$("#texta_js").val("");
		}else{
			$.ajax({
				type: "get",
				url: "../../Api.ashx?command=listPageCodeTree",
				dataType: "json",
				async: true,
				data: { ts: Date.parse(new Date())/1000, pageid: $params.id },
				beforeSend: function($xhr){
					$xhr.overrideMimeType("text/plain;charset=utf-8");
				}
			}).done(function($data){
				if($data.status == "failure"){
					$("#texta_html").val("加载数据异常...");
					$("#texta_js").val("加载数据异常...");
				}else if($data.status == "timeout"){
					top.location.href = "../login.html?locale="+ config.locale;
				}else{		
					_loadHTML($data,$params);
					_loadJS($data);
				}
			});
		}
	};

	$(document).ready(function(){
		/* 主框架布局 */
		var __layout = new dhtmlXLayoutObject({
			parent: "div_center",
			pattern: "3L",
			cells: [{
				id: "a",
				text: "页面树",
				width: 220
			},{
				id: "b",
				text: "HTML"
			},{
				id: "c",
				text: "JS"
			}]
        });
		/* 去掉底部边框，设置为0 */
		$(".dhxcont_global_content_area").css({"border-bottom-width":0});
		__layout.setEffect("collapse",true);
		var __layoutA = __layout.cells("a");
		__layoutA.attachObject("div_tree");
		__layout.setCollapsedText("a","页面树");
		__layout.setCollapsedText("b","HTML");
		__layout.setCollapsedText("c","JS");
		__layout.cells("b").attachObject("texta_html");
		__layout.cells("c").attachObject("texta_js");

		/* 创建树 */
		var __tree = new $Tree({ placeAt: "div_tree" });

		/*<!-- 各种事件 */
		__tree.onClick(function($params){
			_loadData($params);
		});
		/* 点击工具栏的新增按钮 */
		/* 各种事件 -->*/

		/* 页面重绘或调整布局 */
		var __setSizes = function(){
			__layout.setSizes();
		};
		$(window).resize(function(){ setTimeout(__setSizes, 10); });
		__setSizes();
	});
},function($err){    
	var _failedId = $err.requireModules && $err.requireModules[0];
    if (_failedId === "jquery"){
        requirejs.undef(_failedId);
    }
});