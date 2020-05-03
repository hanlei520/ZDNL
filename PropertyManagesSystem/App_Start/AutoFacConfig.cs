using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace PropertyManagesSystem.App_Start
{
    public class AutoFacConfig
    {
        public static void MyAutoFacConfig()
        {
            //构造一个AutoFac的builder容器
            ContainerBuilder builder = new ContainerBuilder();
            //注册当前程序集的所有Controllers
            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();

            //加载业务逻辑层这个程序集        
            string AutoFacName = ConfigurationManager.AppSettings["AutoFac"];//从配置文档获取业务逻辑层的名字
            Assembly Service = Assembly.Load(AutoFacName);

            //已接口的形式保存被创建的对象实例
            //PropertiesAutowired表示：如果实现类中也使用了的接口，实现类中的接口也会被注入（前提是，该对象必须的Autofac创建的）
            builder.RegisterAssemblyTypes(Service).Where(u => !u.IsAbstract).AsImplementedInterfaces().PropertiesAutowired();
            //创建一个真正的AutoFac的工作容器
            var container = builder.Build();
            //移除原本的MVC的容器使用AutoFac的容器
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}