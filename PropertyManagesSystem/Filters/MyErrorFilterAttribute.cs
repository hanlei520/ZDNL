using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PropertyManagesSystem.Filters
{
    public class MyErrorFilterAttribute : HandleErrorAttribute
    {
        public static Queue<Exception> ExceptionQueue = new Queue<Exception>();//创建一个队列
        /// <summary>
        /// 异常处理过滤器
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            if (!filterContext.ExceptionHandled)//判断该异常是否被处理
            {
                //如果异常未被处理，跳转到错误页面
                //存到文本文件中，把异常信息写到队列中
                ExceptionQueue.Enqueue(filterContext.Exception);//入队
                filterContext.Result = new RedirectResult("/Error.html");
                //异常处理后，要将ExceptionHandled设置为true,否则仍然会继续抛出错误
                filterContext.ExceptionHandled = true;
            }
        }
    }
}