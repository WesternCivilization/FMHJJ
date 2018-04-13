using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Optimization;

namespace FMHJJService
{
    public class BundleConfig
    {        
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            //// 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            //// 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));
                       
            bundles.Add(new StyleBundle("~/Content/bootstrap-table/css").Include(
                "~/Content/bootstrap-table/bootstrap-table.min.css"));
            bundles.Add(new StyleBundle("~/Content/bootstrap-fileinput/css/css").Include(
                "~/Content/bootstrap-fileinput/css/fileinput.css"));
            bundles.Add(new StyleBundle("~/Content/ztree/css").Include(
                "~/Content/ztree/demo.css",
                "~/Content/ztree/zTreeStyle.css",
                "~/Content/ztree/zTree.theme.metro.css"));
            bundles.Add(new StyleBundle("~/Scripts/showLoading/css/css").Include(
                "~/Scripts/showLoading/css/showLoading.css"));            
            bundles.Add(new ScriptBundle("~/script").Include(
                "~/Scripts/jquery-1.9.1.min.js",
                "~/Content/jquery-tablesort/jquery-latest.min.js",
                "~/Content/jquery-tablesort/jquery.tablesort.js",
                "~/Scripts/jquery.unobtrusive-ajax.min.js",
                "~/Content/bootstrap/js/bootstrap.min.js",
                "~/Content/bootstrap-table/bootstrap-table.min.js",
                "~/Content/bootstrap-table/locale/bootstrap-table-zh-CN.js",
                "~/Content/bootstrap-fileinput/js/fileinput.js",
                "~/Content/bootstrap-fileinput/js/fileinput_locale_zh.js",
                "~/Content/ztree/jquery.ztree.all.min.js",
                "~/Content/ztree/jquery.ztree.core.js",
                "~/Content/ztree/jquery.ztree.excheck.js",
                "~/Scripts/ZtreeNote.js",
                "~/Scripts/jquery.base64.js",
                "~/Scripts/tableExport.js",
                "~/Content/bootstrap-loading/PerfectLoad.js",                
                "~/Scripts/Home/index.js",
                "~/Scripts/jquery-migrate-1.1.0.js",
                "~/Scripts/jquery.jqprint-0.3.js",
                "~/Scripts/jquery.metadata.js",
                "~/Scripts/jquery.validate.min.js",
                "~/Scripts/localization/messages_zh.min.js",
                "~/Scripts/showLoading/js/jquery.showLoading.min.js"));

            bundles.Add(new ScriptBundle("~/script/IE9").Include(
                "~/Scripts/html5shiv.min.js",
                "~/Scripts/excanvas.js",
                "~/Scripts/respond.min.js",
                "~/Scripts/es5-shim.min.js"));

            bundles.Add(new StyleBundle("~/Content/ace/css/css").Include(
                "~/Content/ace/css/ace-rtl.min.css",
                "~/Content/ace/css/ace-skins.min.css"));
            bundles.Add(new StyleBundle("~/Content/sidebar-menu/css").Include(
                "~/Content/sidebar-menu/sidebar-menu.css"));

            bundles.Add(new ScriptBundle("~/Index/script").Include(
                "~/Content/sidebar-menu/sidebar-menu.js",
                "~/Scripts/bootstrap-tab.js",
                "~/Content/bootstrap/js/menuData.js",
                "~/Content/bootstrap/js/bootstrap-dropdown.js",
                "~/Content/Chart.js-master/Chart.js"));

            //React服务端渲染未实现只预留文件夹
            //bundles.Add(new BabelBundle("~/Jsx").IncludeDirectory(
            //    "~/Jsx", "*.jsx", true));

            BundleTable.EnableOptimizations = true;
        }
    }
}
