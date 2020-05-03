using AutoMapper;
using Common;
using DataModel;
using EntityModel.RoleInfoDTO;
using IService;
using Repository;
using Repository.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class RoleInfoService: IRoleInfoService
    {
        RoleInfoDAL roleInfoDAL = DALFactory.RoleInfoDAL;
        R_UserInfo_RoleInfoDAL r_UserInfo_RoleInfoDAL = DALFactory.R_UserInfo_RoleInfoDAL;
        UnitOfWork unitOfWork = DALFactory.UnitOfWork;
        /// <summary>
        /// 根据登录的用户id获取他对应未被禁用的角色字符串
        /// </summary>
        /// <param name="Admin"></param>
        /// <returns></returns>
        public string GetRoleID(string AdminID)
        {
            //根据userID查询中间表数据
            var iquery = r_UserInfo_RoleInfoDAL.LoadEntities(u => u.AdminID == AdminID);
            //获取角色信息的所有数据
            var roleIquery = roleInfoDAL.LoadEntities(u => u.ID > 0);

            //连表-并筛选掉被禁用的角色
            var r_roleIquery = (from a in iquery
                                join b in roleIquery on a.RoleID equals b.RoleID into b_join
                                from c in b_join.DefaultIfEmpty()
                                select new
                                {
                                    AdminID = a.AdminID,
                                    RoleID = a.RoleID,
                                    DelFlag = c.DelFlag,
                                }).Where(u => u.DelFlag == 0);//筛选
            #region 方法2，通过linq的分组获取
            //分组
            var grpiquery = (from a in r_roleIquery
                             group a by new
                             {
                                 a.AdminID //根据UserID进行分组
                             }).AsEnumerable().Select(p =>
                                 new
                                 {
                                     UserID = p.Key.AdminID,
                                     RoleID = String.Join(",", p.Select(x => x.RoleID).ToArray()),//把RoleID通过逗号分隔，并实现字符串拼接
                                 }
                        ).ToList();
            if (grpiquery.Count > 0)
            {
                return grpiquery[0].RoleID;
            }
            else
            {
                return "0";
            }
            #endregion



        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="selectInfo"></param>
        /// <returns></returns>
        public List<RoleInfo> GetPageList(int pageIndex, int pageSize, out int count, string selectInfo)
        {

            IQueryable<RoleInfo> iquery;
            if (selectInfo == null)
            {
                //说明没有点击搜索按钮
                iquery = roleInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.ID > 0, u => u.ID, true);
            }
            else
            {
                //根据文本框的值进行模糊查询
                iquery = roleInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.RoleName.Contains(selectInfo), u => u.ID, true);
            }


            return iquery.ToList(); ;
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <returns></returns>
        public bool DTOAdd(RoleInfoInputDTO inputEntity)
        {
            //AutoMapper转换
            RoleInfo dataModel = Mapper.Map<RoleInfoInputDTO, RoleInfo>(inputEntity);
            dataModel.DelFlag = 0;
            dataModel.AddTime = DateTime.Now;
            if (roleInfoDAL.AddAndSaveChange(dataModel) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据ID删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(Array id)
        {
            foreach (var item in id)
            {
                int ID = Convert.ToInt32(item);
                //先查
                RoleInfo entity = roleInfoDAL.LoadEntities(u => u.ID == ID).FirstOrDefault();
                if (entity != null)
                {
                    //打标记
                    roleInfoDAL.DeleteFlag(entity);
                }
            }

            if (unitOfWork.SaveChanges() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }   


        /// <summary>
        /// 根据id获取用户数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RoleInfo GetModelByID(int id)
        {
            return roleInfoDAL.LoadEntities(u => u.ID == id).FirstOrDefault();
        }


        /// <summary>
        /// 检查是否已存在
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool CheckRepeat(string value)
        {
            RoleInfo model = roleInfoDAL.LoadEntities(u => u.RoleID == value).FirstOrDefault();
            if (model != null)
            {
                return true;//说明用户编号已存在
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///DTO 修改
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <param name="outID"></param>
        /// <returns></returns>
        public int DTOUpdate(RoleInfoInputDTO inputEntity, out int outID)
        {
            //先查AsNoTracking()不追踪
            outID = 0;
            RoleInfo checkmodel = roleInfoDAL.LoadEntities(u => u.RoleID == inputEntity.RoleID).AsNoTracking().FirstOrDefault();
            if (checkmodel != null)
            {
                //AutoMapper转换
                RoleInfo dataModel = Mapper.Map<RoleInfoInputDTO, RoleInfo>(inputEntity);
                //修改数据 
                dataModel.ID = checkmodel.ID;
                dataModel.AddTime = checkmodel.AddTime;
                dataModel.DelFlag = checkmodel.DelFlag;
                outID = checkmodel.ID;
                return roleInfoDAL.EditAndSaveChange(dataModel);
               
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 禁用或开启用户--其实就是修改，
        /// </summary>
        /// <param name="id"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public bool ForbidUser(int id, string flag)
        {
            RoleInfo userEntity = roleInfoDAL.LoadEntities(u => u.ID == id).FirstOrDefault();
            if (userEntity != null)
            {
                if (flag == "true")
                {
                    userEntity.DelFlag = 0;//把DelFlag字段修改为0，表示开启
                    if (roleInfoDAL.EditAndSaveChange(userEntity) > 0)
                    {
                        return true;
                    }
                }
                else
                {
                    userEntity.DelFlag = 1;//把DelFlag字段修改为1，表示禁用
                    if (roleInfoDAL.EditAndSaveChange(userEntity) > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 获取除了超级管理员的其他角色信息 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<RoleInfo> GetPageList(int pageIndex, int pageSize, out int count)
        {
            return roleInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.DelFlag == 0 && u.RoleID != "1", u => u.ID, true).ToList();
        }

        /// <summary>
        /// 获取所有角色，先删除再按要求重新添加
        /// </summary>
        /// <param name="arrRoleID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public bool ConfiRole(string arrRoleID, string AdminID)
        {
            //1、先删除所有角色信息，再循环添加
            //1.1、获取用户拥有的所有角色
            List<R_UserInfo_RoleInfo> list = r_UserInfo_RoleInfoDAL.LoadEntities(u => u.AdminID == AdminID).ToList();
            //for 循环打上删除标记
            for (int i = 0; i < list.Count; i++)
            {
                r_UserInfo_RoleInfoDAL.DeleteFlag(list[i]);
            }
            //1.2、添加
            string[] arrStr = arrRoleID.Split(',');  //分割
            List<R_UserInfo_RoleInfo> addlist = new List<R_UserInfo_RoleInfo>();//新建一个list集合，用来装需要添加的所有数据
            for (int i = 0; i < arrStr.Length - 1; i++)
            {
                R_UserInfo_RoleInfo model = new R_UserInfo_RoleInfo
                {
                    AdminID = AdminID,
                    RoleID = arrStr[i],
                };
                addlist.Add(model);
            }
            //for 循环打上添加标记
            for (int i = 0; i < addlist.Count; i++)
            {
                r_UserInfo_RoleInfoDAL.AddFlag(addlist[i]);//打上添加标记
            }

            if (unitOfWork.SaveChanges() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
