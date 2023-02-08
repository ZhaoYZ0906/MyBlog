using Blog.Core.Interface.IRepository;
using Blog.Core.Model.Models;
using Blog.Core.Repository.Sugar;
using SqlSugar;
using SqlSugar.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
 

namespace Blog.Core.Repository
{
    public class AdvertisementRepository: IAdvertisementRepository
    {
        private DbContext context;
        private SqlSugarClient db;
        private SimpleClient<Advertisement> entityDB;

        internal SqlSugarClient Db
        {
            get { return db; }
            private set { db = value; }
        }
        public DbContext Context
        {
            get { return context; }
            set { context = value; }
        }

        public AdvertisementRepository()
        {
            DbContext.Init(BaseDBConfig.ConnectionString);
            context = DbContext.GetDbContext();
            db = context.Db;
            
            entityDB = context.GetEntityDB<Advertisement>(db);

            //db.Queryable<Advertisement>().
         

            //context.CreateTableByEntity(false, new Type[] { typeof(Advertisement), typeof(BlogArticle), typeof(ModulePermission), typeof(Module), typeof(Permission), typeof(Role), typeof(RoleModulePermission), typeof(sysUserInfo), typeof(Topic), typeof(TopicDetail), typeof(UserRole) });

        }

        public int Add(Advertisement model)
        {
            var i = db.Insertable(model).ExecuteReturnBigIdentity();
            return i.ObjToInt();
        }

        public bool Delete(Advertisement model)
        {
            var i = db.Deleteable(model).ExecuteCommand();
            return i > 0;
        }

        public List<Advertisement> Query(Expression<Func<Advertisement, bool>> whereExpression)
        {
            return entityDB.GetList(whereExpression);
        }

        public int Sum(int i, int j)
        {
            return i + j;
        }

        public bool Update(Advertisement model)
        {
            //这种方式会以主键为条件
            var i = db.Updateable(model).ExecuteCommand();
            return i > 0;
        }
    }
}
