using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PropertyManagesSystem.Filters
{
    public class MyActionFilterAttribute : ActionFilterAttribute//继承行为结果过滤器
    {
        public string RoleID { get; set; }
        /// <summary>
        /// 重写行为前过滤器：session统一校验和角色权限验证 2，5
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            //判断是否跳过授权过滤器[AllowAnonymous]
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)//获取方法上的特性名字，如果特性名字为AllowAnonymous，返回true
               || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))//获取控制器上的特性名字，如果特性名字为AllowAnonymous，返回true
            {
                return;
            }
            else
            {
                if (filterContext.HttpContext.Session["AdminID"] == null)
                {
                    //调回登录页
                    filterContext.Result = new RedirectResult("/Login/Index");
                }
                else
                {
                    if (RoleID != null)
                    {
                        //字符串分割，得到数组
                        string[] arrRoleID = RoleID.Split(',');
                        //遍历数组，进行角色权限验证   
                        foreach (var item in arrRoleID)
                        {

                            if (filterContext.HttpContext.Session["AdminRoleID"].ToString().Contains(item))
                            {
                                return;//说明通过验证
                            }
                        }
                        //调回无权限页面
                        filterContext.Result = new RedirectResult("/NoPower.html");
                    }
                }
            }
        }
    }
}