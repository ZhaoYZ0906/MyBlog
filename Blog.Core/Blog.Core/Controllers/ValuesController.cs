using Blog.Core.Common.Helper;
using Blog.Core.Common.Redis;
using Blog.Core.Interface.IRedis;
using Blog.Core.Interface.IServices;
using Blog.Core.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blog.Core.Api.Controllers
{
    /// <summary>
    /// 控制器注释
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Policy = "Admin")]
    public class ValuesController : ControllerBase
    {
        IAdvertisementServices Advertisement;//= new AdvertisementServices();
        IBlogArticleService blogArticle;
        IRedisBasketRepository redisBasketRepository;

        public ValuesController(IAdvertisementServices Advertisement, IBlogArticleService BlogArticle, IRedisBasketRepository RedisBasketRepository) 
        { 
            this.Advertisement = Advertisement;
            blogArticle=BlogArticle;
            redisBasketRepository=RedisBasketRepository;
        }

        /// <summary>
        /// 获取所有信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IEnumerable<BlogArticle>> Get()
        {
            //var connect = Appsettings.app(new string[] { "AppSettings", "RedisCaching", "ConnectionString" });//按照层级的顺序，依次写出来


            List<BlogArticle> blogArticleList = await blogArticle.getBlogs();

            // 直接使用redis
            //blogArticleList = await redisBasketRepository.Get<List<BlogArticle>>("zyz");

            //if (blogArticleList == null)
            //{
            //    blogArticleList = await blogArticle.Query(d => d.bID > 5);
            //    redisBasketRepository.Set("zyz", blogArticleList, TimeSpan.FromHours(2));//缓存2小时
            //}


            return blogArticleList;
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public int Get(int id)
        {
            return 3;
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] Topic value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
