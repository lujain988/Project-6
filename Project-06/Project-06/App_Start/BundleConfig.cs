using System.Web;
using System.Web.Optimization;

namespace Project_06
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/css").Include(
                 "~/Content/vendor/bootstrap/css/bootstrap.min.css",
                 "~/Content/fonts/font-awesome-4.7.0/css/font-awesome.min.css",
                 "~/Content/fonts/iconic/css/material-design-iconic-font.min.css",
                 "~/Content/fonts/linearicons-v1.0.0/icon-font.min.css",
                 "~/Content/vendor/animate/animate.css",
                 "~/Content/vendor/css-hamburgers/hamburgers.min.css",
                 "~/Content/vendor/animsition/css/animsition.min.css",
                 "~/Content/vendor/select2/select2.min.css",
                 "~/Content/vendor/daterangepicker/daterangepicker.css",
                 "~/Content/vendor/slick/slick.css",
                 "~/Content/vendor/MagnificPopup/magnific-popup.css",
                 "~/Content/vendor/perfect-scrollbar/perfect-scrollbar.css",
                 "~/Content/css/util.css",
                 "~/Content/css/main.css"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/vendor/jquery/jquery-3.2.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Content/vendor/bootstrap/js/popper.js",
                      "~/Content/vendor/bootstrap/js/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/plugins").Include(
                      "~/Content/vendor/animsition/js/animsition.min.js",
                      "~/Content/vendor/select2/select2.min.js",
                      "~/Content/vendor/daterangepicker/moment.min.js",
                      "~/Content/vendor/daterangepicker/daterangepicker.js",
                      "~/Content/vendor/slick/slick.min.js",
                      "~/Content/vendor/parallax100/parallax100.js",
                      "~/Content/vendor/MagnificPopup/jquery.magnific-popup.min.js",
                      "~/Content/vendor/isotope/isotope.pkgd.min.js",
                      "~/Content/vendor/sweetalert/sweetalert.min.js",
                      "~/Content/vendor/perfect-scrollbar/perfect-scrollbar.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/main").Include(
                      "~/Content/js/main.js"));
        }
    }
}