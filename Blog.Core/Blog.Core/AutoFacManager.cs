using Autofac;
using Blog.Core.Interface.IRepository;
using Blog.Core.Interface.IServices;
using System.Reflection;

namespace Blog.Core.Api
{
    public class AutoFacManager:Autofac.Module
    {
        // 重写加载方法
        protected override void Load(ContainerBuilder builder)
        {
            // 直接注入
            //builder.RegisterType<AdvertisementRepository>().As<IAdvertisementRepository>();
            //builder.RegisterType<AdvertisementServices>().As<IAdvertisementServices>();


            // 根据项目名，扫描其下所有类后反推继承、实现了哪些类、接口并进行注入
            var assemblysServices = Assembly.Load("Blog.Core.Services");
            builder.RegisterAssemblyTypes(assemblysServices).AsImplementedInterfaces();

            var assemblysRepository = Assembly.Load("Blog.Core.Repository");
            builder.RegisterAssemblyTypes(assemblysRepository).AsImplementedInterfaces();

            // 根据项目名，扫描其下所有类并注入
            var assemblysCommon = Assembly.Load("Blog.Core.Common");
            builder.RegisterAssemblyTypes(assemblysCommon);
        }
    }
}
