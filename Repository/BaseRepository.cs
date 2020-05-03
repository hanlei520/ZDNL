using DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    /// <summary>
    /// 基本的数据仓储，存放最基本的增删查改操作
    /// </summary>
    public class BaseRepository<T> where T : class//泛型约束：调用者确定T的类型时，必须是一个引用类型
    {
        protected DbContext db
        {
            get
            {
                return DBContextFactory.CreateDbContext();
            }
        }


        /// <summary>
        /// 查询过滤
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        /// Func<T, bool>这是一个委托类型，用来修饰方法（该方法必须是参数为T，返回值为bool类型的方法）-->能够把方法当做参数传递
        /// 泛型
        public IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda)
        {
            return db.Set<T>().Where(whereLambda);//
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="s"></typeparam>
        /// <param name="pageIndex">访问的页码数</param>
        /// <param name="pageSize">每页显示数据的条数</param>
        /// <param name="totalCount">数据库中符合条件的数据的总条数</param>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="orderbyLambda">排序字段</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns></returns>
        public IQueryable<T> LoadPageEntities<s>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, s>> orderbyLambda, bool isAsc)
        {
            var temp = db.Set<T>().Where<T>(whereLambda);
            totalCount = temp.Count();
            if (isAsc)//升序
            {
                temp = temp.OrderBy<T, s>(orderbyLambda).Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
            }
            else
            {
                temp = temp.OrderByDescending<T, s>(orderbyLambda).Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
            }
            return temp;
        }



        #region 只打上标记，没有SaveChanges()
        /// <summary>
        /// 删除标记
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int DeleteFlag(T entity)
        {
            //db.Entry<T>(entity).State = System.Data.EntityState.Deleted;
            db.Set<T>().Attach(entity);
            db.Set<T>().Remove(entity);
            return 1;
            //return Db.SaveChanges() > 0;
        }

        /// <summary>
        /// 更新标记
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int EditFlag(T entity)
        {
            //Mark entity as modified： 这里采用entity的主键进行匹配。因此，采用这种方法时，必须确保主键已经设置。       
            DbEntityEntry<T> entry = db.Entry(entity);
            //db.Entry(entity).State = EntityState.Modified;
            entry.State = EntityState.Modified;
            //return Db.SaveChanges() > 0;
            return 1;
        }

        /// <summary>
        /// 添加数据标记
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddFlag(T entity)
        {

            db.Set<T>().Add(entity);
            // Db.SaveChanges();
            return 1;
        }


        #endregion


        #region 调用了SaveChanges()
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int DeleteAndSaveChange(T entity)
        {
            //db.Entry<T>(entity).State = System.Data.EntityState.Deleted;
            db.Set<T>().Attach(entity);
            db.Set<T>().Remove(entity);
            return db.SaveChanges();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int EditAndSaveChange(T entity)
        {

            //解决关于Entity Framework更新的几种方式以及可能遇到的问题（附加类型“Model”的实体失败，因为相同类型的其他实体已具有相同的主键值）在使用 _Attach_ 方法或者将实体的状态设置为 _Unchanged_ 或 _Modified_ 时如果图形中的任何实体具有冲突键值，则可能会发生上述行为 
            //if (db.Entry(entity).State == EntityState.Detached)
            //{
            //    HandleDetached(entity);
            //}

            //Mark entity as modified： 这里采用entity的主键进行匹配。因此，采用这种方法时，必须确保主键已经设置。       
            DbEntityEntry<T> entry = db.Entry(entity);
            //db.Entry(entity).State = EntityState.Modified;
            entry.State = EntityState.Modified;
            return db.SaveChanges();

        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddAndSaveChange(T entity)
        {

            db.Set<T>().Add(entity);
            return db.SaveChanges();

        }


        #endregion
    }
}
