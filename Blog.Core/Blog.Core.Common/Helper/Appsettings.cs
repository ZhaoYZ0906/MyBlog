using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Blog.Core.Common.Helper
{
    /// <summary>
    /// 读取appsetting的帮助类
    /// </summary>
    public class Appsettings
    {
        static IConfiguration Configuration { get; set; }
        static string contentPath { get; set; }

        static Appsettings()
        {
            //ReloadOnChange = true 当appsettings.json被修改时重新加载
            Configuration = new ConfigurationBuilder()
            .Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true })
            .Build();
        }

        /// <summary>
        /// 封装要操作的字符
        /// </summary>
        /// <param name="sections"></param>
        /// <returns></returns>
        public static string read(params string[] sections)
        {
            try
            {
                var val = string.Empty;
                val=String.Join(":", sections);

                return Configuration[val.TrimEnd(':')];
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static List<T> read<T>(params string[] session)
        {
            List<T> list = new List<T>();
            Configuration.Bind(string.Join(":", session), list);
            return list;
        }


    }
}
