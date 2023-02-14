using Autofac;
using Autofac.Core;
using Autofac.Extras.DynamicProxy;
using Blog.Core.Api.AOP;
using Blog.Core.Interface.IRepository;
using Blog.Core.Interface.IServices;
using Blog.Core.Services;
using Microsoft.Extensions.Caching.Memory;
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

            // 先注册AOP类
            builder.RegisterType<BlogLogAOP>();


            // 注入缓存对象
            builder.RegisterType<MemoryCache>().As<IMemoryCache>().SingleInstance();

            builder.RegisterType<MemoryCaching>().As<ICaching>();

            builder.RegisterType<BlogCacheAOP>();


            // 根据项目名，扫描其下所有类后反推继承、实现了哪些类、接口并进行注入
            var assemblysServices = Assembly.Load("Blog.Core.Services");
            builder.RegisterAssemblyTypes(assemblysServices).AsImplementedInterfaces().InstancePerLifetimeScope()
                      .EnableInterfaceInterceptors()//引用Autofac.Extras.DynamicProxy;
                      .InterceptedBy(typeof(BlogCacheAOP));//可以直接替换拦截器
            

            var assemblysRepository = Assembly.Load("Blog.Core.Repository");
            builder.RegisterAssemblyTypes(assemblysRepository).AsImplementedInterfaces().InstancePerLifetimeScope();

            // 根据项目名，扫描其下所有类并注入
            var assemblysCommon = Assembly.Load("Blog.Core.Common");
            builder.RegisterAssemblyTypes(assemblysCommon);
        }
    }
}
