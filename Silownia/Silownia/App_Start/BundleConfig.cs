using System.Web.Optimization;

namespace Silownia
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
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
                      "~/Scripts/bootstrap-datepicker.js",    // ** NEW for Bootstrap Datepicker
                      "~/Scripts/respond.js"));

            //bootstrap datepicker do dat z godzinami
            bundles.Add(new ScriptBundle("~/bundles/datetime").Include(
    "~/Scripts/moment*",
    "~/Scripts/bootstrap-datetimepicker*"));

            bundles.Add(new ScriptBundle("~/bundles/datePicker").Include(
          "~/Scripts/moment.min.js",
          "~/Scripts/bootstrap-datetimepicker.min.js"));

            bundles.Add(new StyleBundle("~/Content/datepicker").Include(
                     "~/Content/bootstrap-datetimepicker.min.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-datepicker.css",  // ** NEW for Bootstrap Datepicker
                      "~/Content/site.css"));




            bundles.IgnoreList.Clear();
        }
    }
}
