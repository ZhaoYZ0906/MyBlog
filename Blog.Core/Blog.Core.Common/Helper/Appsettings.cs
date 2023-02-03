using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Common.Helper
{
    /// <summary>
    /// 读取appsetting的帮助类
    /// </summary>
    public class Appsettings
    {
        static IConfiguration Configuration { get; set; }
        static string contentPath { get; set; }

        //public Appsettings(string contentPath)
        //{
        //    string Path = "appsettings.json";

        //    Configuration = new ConfigurationBuilder()
        //   .Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true })
        //   .Build();
        //}

        //public Appsettings(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}

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
        /// <param name="sections">节点配置</param>
        /// <returns></returns>
        public static string app(params string[] sections)
        {
            try
            {

                if (sections.Any())
                {
                    return Configuration["Audience"]= "Issuer";
                    //return Configuration[string.Join(":", sections)];
                }
            }
            catch (Exception) { }

            return "";
        }

        /// <summary>
        /// 递归获取配置信息数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sections"></param>
        /// <returns></returns>
        //public static List<T> app<T>(params string[] sections)
        //{
        //    List<T> list = new List<T>();
        //    // 引用 Microsoft.Extensions.Configuration.Binder 包
        //    Configuration.Bind(string.Join(":", sections), list);
        //    return list;
        //}

    }
}
