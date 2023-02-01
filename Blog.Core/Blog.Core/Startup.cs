using Microsoft.Extensions.Options;
using System.Reflection;

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
                        c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename),true);
                        // 包含model.xml文件
                        c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Blog.Core.Model.xml"), true);
                    }
                );

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

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
