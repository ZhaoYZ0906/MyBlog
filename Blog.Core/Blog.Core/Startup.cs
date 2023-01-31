namespace Blog.Core
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration= configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        public void Configure(WebApplication app)
        {
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
