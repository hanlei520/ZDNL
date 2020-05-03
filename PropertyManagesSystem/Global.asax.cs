using log4net;
using PropertyManagesSystem.App_Start;
using PropertyManagesSystem.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PropertyManagesSystem
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            BundleTable.EnableOptimizations = false;
            log4net.Config.XmlConfigurator.Configure();//注册log4net
            AutoMapperConfig.Config();//注册AutoMapper
            AutoFacConfig.MyAutoFacConfig();//注册AutoFac
            FilterConFig.RegisterGlobalFilters(GlobalFilters.Filters);//注册过滤器
            #region Log4net      
            ThreadPool.QueueUserWorkItem(o =>
            {
                while (true)//死循环，代码一直走
                {
                    if (MyErrorFilterAttribute.ExceptionQueue.Count > 0)//判断队列里是否有数据
                    {
                        Exception ex = MyErrorFilterAttribute.ExceptionQueue.Dequeue();//出队
                        if (ex != null)
                        {
                            ILog logger = LogManager.GetLogger("testError");
                            logger.Error(ex.ToString()); //将异常信息写入Log4Net中  
                        }
                        else
                        {
                            Thread.Sleep(3000);//线程休眠3000毫秒
                        }
                    }
                    else
                    {
                        Thread.Sleep(3000);////线程休眠3000毫秒
                    }
                }
            });
            #endregion
        }
    }
}
