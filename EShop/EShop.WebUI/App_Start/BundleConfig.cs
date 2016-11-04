using System.Web;
using System.Web.Optimization;

namespace EShop.WebUI
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/content/manager/js").Include(
                      //基础框架js
                      "~/content/mgr/js/jquery-1.10.2.min.js",
                      "~/content/mgr/js/jquery.cookie.js",
                      "~/content/mgr/js/jquery-migrate-1.2.1.min.js",
                      "~/content/mgr/js/jquery-ui.js",
                      "~/content/mgr/vendors/bootstrap/js/bootstrap.min.js",
                      "~/content/mgr/vendors/bootstrap-hover-dropdown/bootstrap-hover-dropdown.js",
                      "~/content/mgr/vendors/metisMenu/jquery.metisMenu.js",
                      "~/content/mgr/js/jquery.menu.js",
                      "~/content/mgr/vendors/jquery-pace/pace.min.js",

                      //功能插件js
                      "~/content/mgr/vendors/holder/holder.js",
                      "~/content/mgr/vendors/x-editable/bootstrap3-editable/js/bootstrap-editable.min.js",

                      "~/content/mgr/vendors/bootstrap-progressbar/bootstrap-progressbar.min.js",

                      "~/content/mgr/vendors/jquery-validate/jquery.validate.min.js",
                      "~/content/mgr/vendors/jquery-validate/message_cn.js",
                      "~/content/mgr/vendors/jquery-validate/jquery.form.js",
                      "~/content/mgr/vendors/jquery-validate/jquery-me.validate.js",

                      "~/content/mgr/vendors/iCheck/icheck.min.js",
                      "~/content/mgr/vendors/iCheck/custom.min.js",

                      "~/content/mgr/vendors/select2/select2.min.js",
                      "~/content/mgr/vendors/bootstrap-select/bootstrap-select.min.js",
                      "~/content/mgr/vendors/multi-select/js/jquery.multi-select.js",

                      "~/content/mgr/vendors/jquery-nestable/jquery.nestable.js",

                      "~/content/mgr/vendors/jquery-notific8/jquery.notific8.min.js",
                      "~/content/mgr/vendors/sco.message/sco.message.js",
                      "~/content/mgr/vendors/jquery-toastr/toastr.min.js",

                      "~/content/mgr/vendors/moment/moment.js",
                      "~/content/mgr/vendors/bootstrap-datetimepicker/js/bootstrap-datetimepicker.min.js",

                      "~/content/mgr/vendors/bootstrap-switch/js/bootstrap-switch.min.js",

                      "~/content/mgr/vendors/ueditor/ueditor.config.js",
                      "~/content/mgr/vendors/ueditor/ueditor.all.js",
                      "~/content/mgr/vendors/ueditor/lang/zh-cn/zh-cn.js",

                      "~/content/mgr/vendors/charts/echarts.min.js",


                      //基础框架js
                      "~/content/mgr/js/amy.default.js"));

            bundles.Add(new StyleBundle("~/content/manager/css").Include(
                      //基础框架css
                      "~/content/mgr/vendors/bootstrap/css/bootstrap.min.css",

                      //功能插件css
                      "~/content/mgr/vendors/x-editable/bootstrap3-editable/css/bootstrap-editable.css",

                      "~/content/mgr/vendors/bootstrap-progressbar/css/bootstrap-progressbar-3.1.1.min.css",

                      "~/content/mgr/vendors/iCheck/skins/minimal/all.css",

                      "~/content/mgr/vendors/select2/select2.css",
                      "~/content/mgr/vendors/bootstrap-select/bootstrap-select.min.css",
                      "~/content/mgr/vendors/multi-select/css/multi-select-madmin.css",

                      "~/content/mgr/vendors/pageloader/pageloader.css",

                      "~/content/mgr/vendors/jquery-nestable/nestable.css",

                      "~/content/mgr/vendors/jquery-notific8/jquery.notific8.min.css",
                      "~/content/mgr/vendors/sco.message/sco.message.css",
                      "~/content/mgr/vendors/jquery-toastr/toastr.min.css",

                      "~/content/mgr/vendors/bootstrap-datetimepicker/css/bootstrap-datetimepicker.css",

                      "~/content/mgr/vendors/bootstrap-switch/css/bootstrap-switch.css",

                      //基础框架css
                      "~/content/mgr/vendors/jquery-pace/pace.css"));

            bundles.Add(new StyleBundle("~/content/manager/css/themes").Include(
                      "~/content/mgr/css/themes/style1/orange-blue.css",
                      "~/content/mgr/css/style-responsive.css",
                      "~/content/mgr/css/amy.plugins.css"));
        }
    }
}
