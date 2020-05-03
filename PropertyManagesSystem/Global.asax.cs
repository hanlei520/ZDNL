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
            log4net.Config.XmlConfigurator.Configure();//ע��log4net
            AutoMapperConfig.Config();//ע��AutoMapper
            AutoFacConfig.MyAutoFacConfig();//ע��AutoFac
            FilterConFig.RegisterGlobalFilters(GlobalFilters.Filters);//ע�������
            #region Log4net      
            ThreadPool.QueueUserWorkItem(o =>
            {
                while (true)//��ѭ��������һֱ��
                {
                    if (MyErrorFilterAttribute.ExceptionQueue.Count > 0)//�ж϶������Ƿ�������
                    {
                        Exception ex = MyErrorFilterAttribute.ExceptionQueue.Dequeue();//����
                        if (ex != null)
                        {
                            ILog logger = LogManager.GetLogger("testError");
                            logger.Error(ex.ToString()); //���쳣��Ϣд��Log4Net��  
                        }
                        else
                        {
                            Thread.Sleep(3000);//�߳�����3000����
                        }
                    }
                    else
                    {
                        Thread.Sleep(3000);////�߳�����3000����
                    }
                }
            });
            #endregion
        }
    }
}
