using System.Web.Optimization;

namespace TayanaYacht
{
    public class BundleConfig
    {
        // 如需統合的詳細資訊，請瀏覽 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 使用開發版本的 Modernizr 進行開發並學習。然後，當您
            // 準備好可進行生產時，請使用 https://modernizr.com 的建置工具，只挑選您需要的測試。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            //adminlte
            bundles.Add(new Bundle("~/bundles/adminlte-css").Include(
                 "~/Content/adminlte/css/adminlte.css"));

            bundles.Add(new Bundle("~/bundles/adminlte-bootstrapicon-css").Include(
     "~/Content/adminlte/bootstrap-icons/bootstrap-icons.css"));


            bundles.Add(new Bundle("~/bundles/adminlte-js").Include(
                    "~/Scripts/bootstrap.bundle.min.js", 
                    "~/Content/adminlte/js/adminlte.js"));
            //ckeditor
            bundles.Add(new ScriptBundle("~/bundles/ckeditor").Include(
                "~/Scripts/ckeditor/ckeditor.js"
            ));

            //region elFinder bundles

            bundles.Add(new ScriptBundle("~/Scripts/elfinder").Include(
                             "~/Content/elfinder/js/elfinder.full.js"
                             , "~/Content/elfinder/js/i18n/elfinder.zh_TW.js"
                             ));

            bundles.Add(new StyleBundle("~/Content/elfinder").Include(
                            "~/Content/elfinder/css/elfinder.full.css",
                            "~/Content/elfinder/css/theme.css"));

            bundles.Add(new StyleBundle("~/Content/jquery-ui").Include(
                                        "~/Content/jquery-ui-1.14.2/jquery-ui.css"));

            bundles.Add(new ScriptBundle("~/Scripts/jquery-ui").Include(
                                        "~/Content/jquery-ui-1.14.2/jquery-ui.js"));
        }
    }
}
