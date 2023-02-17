using Blog.Core.Interface.IServices.Base;
using Blog.Core.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Interface.IServices
{
    public interface IBlogArticleService:IBaseServices<BlogArticle>
    {
        Task<List<BlogArticle>> getBlogs();
    }
}
