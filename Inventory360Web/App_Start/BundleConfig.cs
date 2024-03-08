using System.Web.Optimization;

namespace Inventory360Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/dataTables.bootstrap.min.js",
                      "~/Scripts/buttons.bootstrap.min.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/others").Include(
                      "~/Scripts/nprogress.js",
                      "~/Scripts/moment.min.js",
                      "~/Scripts/custom.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                      "~/Scripts/angular.min.js",
                      "~/Scripts/angular-route.min.js",
                      "~/Scripts/loading-bar.min.js",
                      "~/Scripts/textAngular-rangy.min.js",
                      "~/Scripts/textAngular-sanitize.min.js",
                      "~/Scripts/textAngular.min.js",
                      "~/Scripts/angular-confirm.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/dataTables.bootstrap.min.css",
                      "~/Content/buttons.bootstrap.min.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/nprogress.css",
                      "~/Content/animate.min.css",
                      "~/Content/angular-confirm.css",
                      "~/Content/loading-bar.css",
                      "~/Content/custom.min.css"));
        }
    }
}
