using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Repository.Sugar
{
    public class DbContext2
    {
        // 数据库连接字符串
        public static string _connectionString { get; set; }
        // 数据库类型
        private static DbType _dbType;
        // 数据库操作实例
        private SqlSugarClient _db;

        /// <summary>
        /// 构造函数：只能通过GetDbContext创建新对象
        /// </summary>
        /// <param name="blnIsAutoCloseConnection"></param>
        /// <exception cref="ArgumentNullException"></exception>
        private DbContext2(bool blnIsAutoCloseConnection=true)
        {
            if (string.IsNullOrEmpty(_connectionString))
                throw new ArgumentNullException("数据库连接字符串为空");
            _db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = _connectionString,
                DbType = _dbType,
                IsAutoCloseConnection = blnIsAutoCloseConnection
            });
        }

        /// <summary>
        /// 创建指定实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sugarClient"></param>
        /// <returns></returns>
        public SimpleClient<T> GetCustomEntityDB<T>(ConnectionConfig config) where T : class, new()
        {
            SqlSugarClient sugarClient = GetSqlSugarClient(config);
            return GetEntityDB<T>(sugarClient);
        }

        /// <summary>
        /// 创建指定实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sugarClient"></param>
        /// <returns></returns>
        public SimpleClient<T> GetEntityDB<T>(SqlSugarClient sugarClient) where T : class, new()
        {
            return new SimpleClient<T>(sugarClient);
        }

        /// <summary>
        /// 创建数据库
        /// </summary>
        public void CreateDataBase()
        {
            _db.DbMaintenance.CreateDatabase();
        }

        /// <summary>
        /// 根据实体类型创建对应的数据库表
        /// </summary>
        /// <param name="blnBackupTable"></param>
        /// <param name="lstEntitys"></param>
        public void CreateTableByEntity(bool blnBackupTable, params Type[] lstEntitys)
        {
            if (blnBackupTable)
            {
                _db.CodeFirst.BackupTable().InitTables(lstEntitys); //change entity backupTable            
            }
            else
            {
                _db.CodeFirst.InitTables(lstEntitys);
            }
        }

        #region 数据库操作对象相关操作

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="strConnectionString"></param>
        /// <param name="enmDbType"></param>
        public static void Init(string strConnectionString, DbType enmDbType = SqlSugar.DbType.SqlServer)
        {
            _connectionString = strConnectionString;
            _dbType = enmDbType;
        }

        /// <summary>
        /// 构建一个上下文
        /// </summary>
        /// <param name="blnIsAutoCloseConnection"></param>
        /// <returns></returns>
        public static DbContext2 GetDbContext(bool blnIsAutoCloseConnection = true)
        {
            return new DbContext2(blnIsAutoCloseConnection);
        }

        /// <summary>
        /// 创建连接配置
        /// </summary>
        /// <param name="blnIsAutoCloseConnection"></param>
        /// <param name="blnIsShardSameThread"></param>
        /// <returns></returns>
        public static ConnectionConfig GetConnectionConfig(bool blnIsAutoCloseConnection = true, bool blnIsShardSameThread = false)
        {
            ConnectionConfig config = new ConnectionConfig()
            {
                ConnectionString = _connectionString,
                DbType = _dbType,
                IsAutoCloseConnection = blnIsAutoCloseConnection,                
            };
            return config;
        }

        /// <summary>
        /// 创建数据库客户端
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static SqlSugarClient GetSqlSugarClient(ConnectionConfig config)
        {
            return new SqlSugarClient(config);
        }        

        #endregion
    }
}