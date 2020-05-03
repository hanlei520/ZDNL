using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PropertyManagesSystem.Filters
{
    public class MyUserActionFilterAttribute : ActionFilterAttribute//继承行为结果过滤器
    {
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
                if (filterContext.HttpContext.Session["UserID"] == null)
                {
                    //调回登录页
                    filterContext.Result = new RedirectResult("/Login/Index");
                }
                //else
                //{
                //    //调回错误页
                //    filterContext.Result = new RedirectResult("/NoPower.html");
                //}
            }
        }
    }
}