using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class DBContextFactory
    {
        /// <summary>
        /// 线程数据槽==父类DBContext
        /// </summary>
        /// <returns></returns>
        public static DbContext CreateDbContext()
        {
            DbContext dbContext = (DbContext)CallContext.GetData("dbContext");
            if (dbContext == null)
            {
                dbContext = new EFModel();
                CallContext.SetData("dbContext", dbContext);
            }
            //dbContext.Configuration.ProxyCreationEnabled = false;//如果不使用DTO，json.net会出现联动序列化的bug，需要这句代码
            return dbContext;
        }
    }
}
