using System.Web;
using System.Web.Optimization;

namespace RecruitingPortal
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(
                        new ScriptBundle("~/bundles/js").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/datatables/jquery.dataTables.min.js",
                        "~/Scripts/DataTables/dataTables.bootstrap.js",
                        "~/Scripts/jquery-ui-1.12.1.min.js",
                        "~/Scripts/jquery.ui.core.js",
                        "~/Scripts/jquery.ui.datepicker.js",
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/respond.js",
                        "~/Scripts/DataTables/jquery.dataTables.min.js",
                        "~/Scripts/DataTables/dataTables.buttons.min.js",
                        "~/Scripts/DataTables/buttons.flash.min.js",
                        "~/Scripts/DataTables/buttons.print.min.js",
                        "~/Scripts/jquery.fancybox.js",
                        "~/Scripts/RecruitingPortal.js",
                        "~/Scripts/jQuery.print.min.js",
                        "~/Scripts/moment.min.js",
                        "~/Scripts/daterangepicker.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js"
                        ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            // stylesheet
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/themes/base/jquery-ui.min.css",
                      "~/Content/bootstrap.css",
                      "~/Content/themes/base/datepicker.css",
                      "~/Content/jquery.fancybox.css",
                      "~/Content/ErrorStyles.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/DataTables/css/jquery.dataTables.min.css",
                      "~/Content/DataTables/css/buttons.dataTables.min.css",
                      "~/Content/daterangepicker.css",
                      "~/Content/style.css"
                      
                      ));
        }
    }
}
