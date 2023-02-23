using AutoMapper;
using Blog.Core.Api.AuthHelper;
using Blog.Core.Common.Helper;
using Blog.Core.Model.Dto;
using Blog.Core.Model.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Core.Api.Controllers
{
    [Route("api/Login")]
    public class LoginController : Controller
    {
        //IMapper IMapper;
        //public LoginController(IMapper IMapper)
        //{
        //    this.IMapper = IMapper;
        //}

        [HttpGet]
        [Route("Token")]
        public async Task<object> GetJwtStr(string name, string pass)
        {
            //BlogArticle cs = new BlogArticle() {bID=1,btitle="12313135456" };
            //BlogDto BlogDto = IMapper.Map<BlogDto>(cs);

            var ss = Appsettings.read( new[] { "AppSettings", "catalogue" });

            // 将用户id和角色名，作为单独的自定义变量封装进 token 字符串中。
            TokenModelJwt tokenModel = new TokenModelJwt { Uid = 1, Role = "Admin" };
            var jwtStr = JwtHelper.IssueJwt(tokenModel);//登录，获取到一定规则的 Token 令牌
            var suc = true;
            return Ok(new
            {
                success = suc,
                token = jwtStr
            });
        }
    }

    //public class catalogue {
    //    public string name { get; set; }
    //    public bool enabled { get; set; }
    //}
}
