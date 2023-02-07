using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
        }

        public void Configure(WebApplication app)
        {
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
