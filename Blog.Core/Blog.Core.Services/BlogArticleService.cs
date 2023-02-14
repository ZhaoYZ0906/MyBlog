using Blog.Core.Interface.IRepository;
using Blog.Core.Interface.IServices;
using Blog.Core.Model.Models;
using Blog.Core.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Services
{
    internal class BlogArticleService:BaseServices<BlogArticle>, IBlogArticleService
    {
        IBlogArticleRepository dal;

        public BlogArticleService(IBlogArticleRepository dal)
        {
            this.dal = dal;
            base.baseDal = dal;
        }

    }
}
