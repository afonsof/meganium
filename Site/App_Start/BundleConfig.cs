using System.Web.Optimization;

namespace Meganium.Site
{
    public static class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/libs").Include(
                "~/Content/admin/js/libs/jquery/jquery-{version}.js",
                "~/Content/admin/js/libs/bootstrap/bootstrap.js",
                "~/Content/admin/js/libs/bootstrap-colorpicker/bootstrap-colorpicker.js",
                "~/Content/admin/js/libs/bootstrap-datetimepicker/bootstrap-datetimepicker.js",
                "~/Content/admin/js/libs/bootstrap-switch/bootstrap-switch.min.js",
                "~/Content/admin/js/libs/chosen/chosen.jquery.js",
                "~/Content/admin/js/libs/jquery-uploadifive/jquery.uploadifive.js",
                "~/Content/admin/js/libs/jquery-multiselect/jquery.multi-select.js",
                "~/Content/admin/js/libs/bootstrap-wysihtml5/wysihtml5-{version}.js",
                "~/Content/admin/js/libs/bootstrap-wysihtml5/bootstrap-wysihtml5.js",
                "~/Content/admin/js/libs/bootstrap-daterangepicker/daterangepicker.js",
                "~/Content/admin/js/libs/bootstrap-daterangepicker/moment.js",
                "~/Content/admin/js/libs/jquery-jsoneditor/jquery.jsoneditor.js",
                "~/Content/admin/js/libs/base64/base64.js",
                "~/Content/admin/js/app.js"));

            bundles.Add(new ScriptBundle("~/bundles/validate").Include(
                "~/Content/admin/js/libs/jquery-validate/jquery.validate.js",
                "~/Content/admin/js/libs/jquery-validate/jquery.validate.unobtrusive.js"));

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/Content/admin/js/libs/bootstrap/bootstrap.css", new CssRewriteUrlTransform())
                .Include("~/Content/admin/js/libs/bootstrap/bootstrap-responsive.css", new CssRewriteUrlTransform())
                .Include("~/Content/admin/js/libs/font-awesome/css/font-awesome.css", new CssRewriteUrlTransform())
                .Include("~/Content/admin/js/libs/bootstrap-colorpicker/bootstrap-colorpicker.css", new CssRewriteUrlTransform())
                .Include("~/Content/admin/js/libs/bootstrap-datetimepicker/bootstrap-datetimepicker.css", new CssRewriteUrlTransform())
                .Include("~/Content/admin/js/libs/bootstrap-switch/bootstrap-switch.css", new CssRewriteUrlTransform())
                .Include("~/Content/admin/js/libs/chosen/chosen.css", new CssRewriteUrlTransform())
                .Include("~/Content/admin/js/libs/jquery-uploadifive/uploadifive.css", new CssRewriteUrlTransform())
                .Include("~/Content/admin/js/libs/jquery-multiselect/css/multi-select.css", new CssRewriteUrlTransform())
                .Include("~/Content/admin/js/libs/bootstrap-wysihtml5/bootstrap-wysihtml5.css", new CssRewriteUrlTransform())
                .Include("~/Content/admin/js/libs/bootstrap-wysihtml5/wysihtml5-color.css", new CssRewriteUrlTransform())
                .Include("~/Content/admin/js/libs/bootstrap-daterangepicker/daterangepicker-bs2.css", new CssRewriteUrlTransform())
                .Include("~/Content/admin/js/libs/jquery-jsoneditor/jsoneditor.css", new CssRewriteUrlTransform())
                .Include("~/Content/admin/css/mediafilemanager.css", new CssRewriteUrlTransform())
                .Include("~/Content/admin/css/app.css", new CssRewriteUrlTransform()));
        }
    }
}