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

        public ValuesController(IAdvertisementServices Advertisement) 
        { 
            this.Advertisement = Advertisement;
        }

        /// <summary>
        /// 获取所有信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IEnumerable<Advertisement>> Get()
        {
            return await Advertisement.Query(x => x.Id > 1);
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
