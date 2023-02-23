using Blog.Core.Api.AutoMap;
using Blog.Core.Common.Helper;
using Blog.Core.Common.Redis;
using Blog.Core.Interface.IRedis;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Reflection;
using System.Text;

namespace Blog.Core
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // 注册控制器
            services.AddControllers();

            // 注册Swashbuckle
            services.AddSwaggerGen(
                    c =>
                    {
                        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                        {
                            // 配置相关文档相关信息，具体参数请研究OpenApiInfo
                            Version = "v1",
                            Title = "MyBlog"
                        });

                        // 获取XML文件路径
                        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                        c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename), true);
                        // 包含model.xml文件
                        c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Blog.Core.Model.xml"), true);
                    }
                );

            // 编写授权策略
            // 策略的优势在于统一控制，修改一处即可
            services.AddAuthorization(options =>
                {
                    options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());//单独角色
                    options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                    options.AddPolicy("SystemOrAdmin", policy => policy.RequireRole("Admin", "System"));//或的关系
                    options.AddPolicy("SystemAndAdmin", policy => policy.RequireRole("Admin").RequireRole("System"));//且的关系
                }
            );

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("sdfsdfsrty45634kkhllghtdgdfss345t678fs"));

            // 认证服务
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            // 认证策略
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,//参数配置在下边
                    ValidateIssuer = true,
                    ValidIssuer = "zyz",//发行人
                    ValidateAudience = true,
                    ValidAudience = "user1",//订阅人
                    //ValidateLifetime = true,
                    //ClockSkew = TimeSpan.Zero,//这个是缓冲过期时间，也就是说，即使我们配置了过期时间，这里也要考虑进去，过期时间+缓冲，默认好像是7分钟，你可以直接设置为0
                    //RequireExpirationTime = true,
                };

            });
            // 自定义认证方案
            //.AddJwtBearer("", x => { });

            // 自带注入框架
            //services.AddTransient<IAdvertisementRepository, AdvertisementRepository>();

            //services.AddTransient<IAdvertisementServices, AdvertisementServices>();


            
            // 配置启动Redis服务，虽然可能影响项目启动速度，但是不能在运行的时候报错，所以是合理的
            services.AddSingleton<ConnectionMultiplexer>(sp =>
            {
                //获取连接字符串
                string redisConfiguration = Appsettings.read(new string[] { "AppSettings", "RedisCaching", "ConnectionString" });

                var configuration = ConfigurationOptions.Parse(redisConfiguration, true);

                configuration.ResolveDns = true;

                return ConnectionMultiplexer.Connect(configuration);
            });

            #region CORS
            //跨域第一种方法，先注入服务，声明策略，然后再下边app中配置开启中间件
            services.AddCors(c =>
            {
                //↓↓↓↓↓↓↓注意正式环境不要使用这种全开放的处理↓↓↓↓↓↓↓↓↓↓
                c.AddPolicy("AllRequests", policy =>
                {
                    policy
                    //.AllowAnyOrigin()//允许任何源
                    .WithOrigins("*", "*", "*")
                    .AllowAnyMethod()//允许任何方式
                    .AllowAnyHeader()//允许任何头
                    .AllowCredentials();//允许cookie
                });
                //↑↑↑↑↑↑↑注意正式环境不要使用这种全开放的处理↑↑↑↑↑↑↑↑↑↑


                //一般采用这种方法
                c.AddPolicy("LimitRequests", policy =>
                {
                    policy
                    .WithOrigins("http://127.0.0.1:1818", "http://localhost:8080", "http://localhost:8021", "http://localhost:8081", "http://localhost:1818")//支持多个域名端口，注意端口号后不要带/斜杆：比如localhost:8000/，是错的
                    .AllowAnyHeader()//Ensures that the policy allows any header.
                    .AllowAnyMethod();
                });
            });

            // 这是第二种注入跨域服务的方法，这里有歧义，部分读者可能没看懂，请看下边解释
            //services.AddCors();
            #endregion


            services.AddAutoMapper(typeof(AllProfile));

        }

        public void Configure(WebApplication app)
        {
            app.UseCors("LimitRequests");

            // 注册接口文档中间件
            app.UseSwagger();
            app.UseSwaggerUI(
                option =>
                {
                    // 设置SwaggerGen生成地址，并起一个名字
                    option.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");

                    // 设置文档的路由前缀 RoutePrefix默认值为swagger则访问路径为 https://localhost:<port>/swagger/index  将其设置为空后 https://localhost:<port>/index
                    // 可能会与其他路由冲突，合理利用
                    option.RoutePrefix = string.Empty;
                }
            );

            //app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
