function bindZtree(json)
{
var setting = {
    data: {
        key: {
            //将treeNode的ItemName属性当做节点名称
            name: "FunctionName"
        },
        simpleData: {
            //是否使用简单数据模式
            enable: true,
            //当前节点id属性  
            //idKey: "DBID",
            idKey: "ID",
            //当前节点的父节点id属性 
            pIdKey: "PID",
            //用于修正根节点父节点数据，即pIdKey指定的属性值
            rootPId: 0
        }
    },
    view: {
        //是否支持同时选中多个节点
        selectedMulti: false
    }, check: {
        enable: true,
        chkStyle: "checkbox"
    }
};
var treeObj = $.fn.zTree.init($("#treeDemo"), setting, $.parseJSON(json));
//默认展开所有节点
treeObj.expandAll(true);
var treeObj = $.fn.zTree.getZTreeObj("treeDemo");
//var nodes = treeObj.getNodes();
//for (var i = 0; i < nodes.length; i++) {
//    nodes[i].nocheck = true;
//    treeObj.updateNode(nodes[i]);
//    var no = treeObj.getNodesByParam("PID", 0, null);
//    for (var j = 0; j < no.length; j++) {
//        no[j].nocheck = true;
//        treeObj.updateNode(no[j]);
//    }
    //}
$.each($.parseJSON(json), function (i, model) {
    if (model.IsAllow == 1)
    {
        treeObj.checkNode(treeObj.getNodesByParam("ID", model.ID, null)[0], true, true);
    }    
});
}