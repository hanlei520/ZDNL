using DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    /// <summary>
    /// 工作单元，当需要使用事务的时候调用。
    /// </summary>
    public class UnitOfWork
    {
        protected DbContext Db
        {
            get
            {
                return DBContextFactory.CreateDbContext();
            }
        }
        public int SaveChanges()
        {
            return Db.SaveChanges();
        }
    }
}
