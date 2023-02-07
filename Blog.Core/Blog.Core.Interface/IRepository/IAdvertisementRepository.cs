using Blog.Core.Interface.IRepository.Base;
using Blog.Core.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Interface.IRepository
{
    public interface IAdvertisementRepository //: IBaseRepository<Advertisement>
    {
        int Sum(int i, int j);
    }
}
