using System.Web;
using System.Web.Optimization;

namespace PropertyManagesSystem
{
    public class BundleConfig
    {
        // 有关捆绑的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            //主页CSS
            bundles.Add(new StyleBundle("~/index/css").Include(
                       "~/Content/lib/layui-v2.5.5/css/layui.css",
                       "~/Content/css/layuimini.css",
                       "~/Content/css/themes/default.css",
                       "~/Content/lib/font-awesome-4.7.0/css/font-awesome.min.css"));
            //主页js
            bundles.Add(new ScriptBundle("~/index/js").Include(
                        //"~/Content/lib/jquery-3.4.1/jquery-3.4.1.min.js",
                        "~/Content/lib/layui-v2.5.5/layui.js",
                        "~/Scripts/js/lay-config.js"));


            //welcome-1 css
            bundles.Add(new StyleBundle("~/welcome1/css").Include(
                      "~/Content/lib/layui-v2.5.5/css/layui.css",
                      "~/Content/lib/font-awesome-4.7.0/css/font-awesome.min.css",
                      "~/Content/css/public.css" ));

            //welcome-1 js
            bundles.Add(new ScriptBundle("~/welcome1/js").Include(
                      //"~/Content/lib/jquery-3.4.1/jquery-3.4.1.min.js",
                      "~/Content/lib/layui-v2.5.5/layui.js",
                      "~/Scripts/js/lay-config.js"));

            //用户信息页 CSS
            bundles.Add(new StyleBundle("~/Admin/css").Include(
                       "~/Content/lib/layui-v2.5.5/css/layui.css"));
            //用户信息页js
            bundles.Add(new ScriptBundle("~/Admin/js").Include(
                        "~/Content/lib/jquery-3.4.1/jquery-3.4.1.min.js",
                        "~/Content/lib/layui-v2.5.5/layui.js"));

            //登录js
            bundles.Add(new ScriptBundle("~/Login/js").Include(
                        "~/Content/lib/jquery-3.4.1/jquery-3.4.1.min.js"
                        ));

            //投诉 CSS
            bundles.Add(new StyleBundle("~/Compla/css").Include(
                       "~/Content/lib/layui-v2.5.5/css/layui.css"));
            //投诉 js
            bundles.Add(new ScriptBundle("~/Compla/js").Include(
                        "~/Content/lib/jquery-3.4.1/jquery-3.4.1.min.js",
                        "~/Content/lib/layui-v2.5.5/layui.js"));


            //水电费 CSS
            bundles.Add(new StyleBundle("~/CostInfo/css").Include(
                       "~/Content/lib/layui-v2.5.5/css/layui.css"));
            //水电费 js
            bundles.Add(new ScriptBundle("~/CostInfo/js").Include(
                        "~/Content/lib/jquery-3.4.1/jquery-3.4.1.min.js",
                        "~/Content/lib/layui-v2.5.5/layui.js"));

            //楼栋信息 CSS
            bundles.Add(new StyleBundle("~/FloorInfo/css").Include(
                       "~/Content/lib/layui-v2.5.5/css/layui.css"));
            //楼栋信息 js
            bundles.Add(new ScriptBundle("~/FloorInfo/js").Include(
                        "~/Content/lib/jquery-3.4.1/jquery-3.4.1.min.js",
                        "~/Content/lib/layui-v2.5.5/layui.js"));

            //房产信息 CSS
            bundles.Add(new StyleBundle("~/HouseInfo/css").Include(
                       "~/Content/lib/layui-v2.5.5/css/layui.css"));
            //房产信息 js
            bundles.Add(new ScriptBundle("~/HouseInfo/js").Include(
                        "~/Content/lib/jquery-3.4.1/jquery-3.4.1.min.js",
                        "~/Content/lib/layui-v2.5.5/layui.js"));

            //物业费 CSS
            bundles.Add(new StyleBundle("~/PropertyInfo/css").Include(
                       "~/Content/lib/layui-v2.5.5/css/layui.css"));
            //物业费 js
            bundles.Add(new ScriptBundle("~/PropertyInfo/js").Include(
                        "~/Content/lib/jquery-3.4.1/jquery-3.4.1.min.js",
                        "~/Content/lib/layui-v2.5.5/layui.js"));


            //报修 CSS
            bundles.Add(new StyleBundle("~/RepairInfo/css").Include(
                       "~/Content/lib/layui-v2.5.5/css/layui.css"));
            //报修 js
            bundles.Add(new ScriptBundle("~/RepairInfo/js").Include(
                        "~/Content/lib/jquery-3.4.1/jquery-3.4.1.min.js",
                        "~/Content/lib/layui-v2.5.5/layui.js"));


            //角色信息 CSS
            bundles.Add(new StyleBundle("~/RoleInfo/css").Include(
                       "~/Content/lib/layui-v2.5.5/css/layui.css"));
            //角色信息 js
            bundles.Add(new ScriptBundle("~/RoleInfo/js").Include(
                        "~/Content/lib/jquery-3.4.1/jquery-3.4.1.min.js",
                        "~/Content/lib/layui-v2.5.5/layui.js"));

            //业主信息 CSS
            bundles.Add(new StyleBundle("~/UserInfo/css").Include(
                       "~/Content/lib/layui-v2.5.5/css/layui.css"));
            //业主信息 js
            bundles.Add(new ScriptBundle("~/UserInfo/js").Include(
                        "~/Content/lib/jquery-3.4.1/jquery-3.4.1.min.js",
                        "~/Content/lib/layui-v2.5.5/layui.js"));


            //小区 CSS
            bundles.Add(new StyleBundle("~/VillageInfo/css").Include(
                       "~/Content/lib/layui-v2.5.5/css/layui.css"));
            //小区 js
            bundles.Add(new ScriptBundle("~/VillageInfo/js").Include(
                        "~/Content/lib/jquery-3.4.1/jquery-3.4.1.min.js",
                        "~/Content/lib/layui-v2.5.5/layui.js"));

            //table css
            bundles.Add(new StyleBundle("~/table/css").Include(
                      "~/Content/lib/layui-v2.5.5/css/layui.css",
                      "~/Content/css/public.css"
                      ));

            //table js
            bundles.Add(new ScriptBundle("~/table/js").Include(
                       "~/Content/lib/jquery-3.4.1/jquery-3.4.1.min.js",
                       "~/Content/lib/layui-v2.5.5/layui.js"));


        }
    }
}
