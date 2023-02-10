using Autofac;
using Autofac.Extensions.DependencyInjection;
using Blog.Core;
using Blog.Core.Api;
using Blog.Core.Interface.IRepository;
using Blog.Core.Interface.IServices;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();

//var app = builder.Build();

//// Configure the HTTP request pipeline.

//app.UseAuthorization();

//app.MapControllers();

//app.Run();



var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

//Autofac 专用函数注册
//IHostBuilder hostBuilder = builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
//{
//    //这里注册关系
//    builder.RegisterType<AdvertisementRepository>().As<IAdvertisementRepository>();
//builder.RegisterType<AdvertisementServices>().As<IAdvertisementServices>();
//});

IHostBuilder hostBuilder = builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule(new AutoFacManager());
});

var app = builder.Build();
startup.Configure(app);