using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using DataModel;
using EntityModel;
using EntityModel.AdminInfoDTO;
using IService;
using Repository;
using Repository.DAL;

namespace Service
{
    public class AdminInfoService: IAdminInfoService
    {
        AdminInfoDAL adminInfoDAL = DALFactory.AdminInfoDAL;
        R_UserInfo_RoleInfoDAL r_UserInfo_RoleInfoDAL = DALFactory.R_UserInfo_RoleInfoDAL;
        RoleInfoDAL roleInfoDAL = DALFactory.RoleInfoDAL;
        UnitOfWork unitOfWork = DALFactory.UnitOfWork;

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <returns></returns>
        public AdminInfo Login(AdminInfo inputEntity)
        {

            //先给密码进行MD5加密
            string pwd = MD5Helper.EncryptString(inputEntity.PassWord);
            //
            AdminInfo model = adminInfoDAL.LoadEntities(u => u.AdminID == inputEntity.AdminID && u.PassWord == pwd&&u.DelFlag!=1).FirstOrDefault();
            
            return model;

        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<AdminOutputDTO> GetPageList(int pageIndex, int pageSize, out int count, string selectInfo)
        {
            IQueryable<AdminInfo> iquery;
            if (selectInfo == null)
            {
                //说明没有点击搜索按钮
                iquery = adminInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.ID > 0, u => u.ID, true);
            }
            else
            {
                //根据文本框的值进行模糊查询
                iquery = adminInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.Name.Contains(selectInfo), u => u.ID, true);
            }
            //先拿到所有信息
            IQueryable<R_UserInfo_RoleInfo> r_u_rIquery = r_UserInfo_RoleInfoDAL.LoadEntities(u => u.ID > 0);//获取用户角色中间表
            IQueryable<RoleInfoOutpt> roleIquery = from a in roleInfoDAL.LoadEntities(u => u.ID > 0)
                                                   select new RoleInfoOutpt
                                                   {
                                                       RoleID = a.RoleID,
                                                       RoleName = a.DelFlag == 0 ? a.RoleName : a.RoleName + "(已被禁用)",
                                                   };
            //连表查询
            IQueryable<AdminOutputDTO> linq = (from a in iquery
                                            join d in r_u_rIquery on a.AdminID equals d.AdminID into c_join
                                            from e in c_join.DefaultIfEmpty()
                                            join f in roleIquery on e.RoleID equals f.RoleID into d_join
                                            from g in d_join.DefaultIfEmpty()
                                            select new AdminOutputDTO
                                            {
                                                ID = a.ID,
                                                AdminID = a.AdminID,
                                                Name = a.Name,
                                                Phone = a.Phone,
                                                DelFlag = a.DelFlag,
                                                Sex = a.Sex,
                                                RoleName = g.RoleName,
                                                Email = a.Email,
                                            });
            //分组
            var grplinq = (from p in linq
                           group p by new
                           {
                               p.ID,
                               p.AdminID,
                               p.Name,
                               p.Phone,
                               p.DelFlag,
                               p.Sex,
                               p.Email,
                           }).AsEnumerable().Select(grp => new AdminOutputDTO
                           {
                               ID = grp.Key.ID,
                               AdminID = grp.Key.AdminID,
                               Name = grp.Key.Name,
                               Phone = grp.Key.Phone,
                               DelFlag = grp.Key.DelFlag,
                               Sex = grp.Key.Sex,
                               Email = grp.Key.Email,
                               RoleName = string.Join(",", grp.Select(x => x.RoleName).ToArray()),//通过逗号隔开，把RoleName合并

                           }).OrderBy(u => u.ID).ToList();

            return grplinq.ToList();
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
                AdminInfo entity = adminInfoDAL.LoadEntities(u => u.ID == ID).FirstOrDefault();
                //1.1、获取用户拥有的所有角色
                List<R_UserInfo_RoleInfo> list = r_UserInfo_RoleInfoDAL.LoadEntities(u => u.AdminID == entity.AdminID).ToList();
                //for 循环打上删除标记
                for (int i = 0; i < list.Count; i++)
                {
                    r_UserInfo_RoleInfoDAL.DeleteFlag(list[i]);
                }
                if (entity != null)
                {
                    //打标记
                    adminInfoDAL.DeleteFlag(entity);
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
        /// 修改密码
        /// </summary>
        /// <param name="model"></param>
        /// <param name="AdminID"></param>
        /// <param name="NewPwd"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        public bool ResetPwd(string PassWord, string AdminID, string NewPwd, string Pwd)
        {
            //先查
            AdminInfo entity = adminInfoDAL.LoadEntities(u => u.AdminID == AdminID).FirstOrDefault();
            if (entity != null)
            {
                if (PassWord != null)
                {
                    string strPassWord = MD5Helper.EncryptString(PassWord);
                    if (entity.PassWord != strPassWord)
                    {
                        return false;
                    }
                    if (NewPwd != Pwd)
                    {
                        return false;
                    }
                    Pwd = MD5Helper.EncryptString(Pwd);
                    entity.PassWord = Pwd;
                    if (adminInfoDAL.EditAndSaveChange(entity) > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据ID重置密码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool RePwd(Array id)
        {
            foreach (var item in id)
            {
                int ID = Convert.ToInt32(item);
                AdminInfo entity = adminInfoDAL.LoadEntities(u => u.ID == ID).FirstOrDefault();

                if (entity != null)
                {
                    //修改密码,为默认值，并MD5加密
                    entity.PassWord = MD5Helper.EncryptString("123");
                    adminInfoDAL.EditFlag(entity);
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
        /// 添加数据
        /// </summary>
        /// <param name="inpuEntity"></param>
        /// <returns></returns>
        public bool DTOAdd(AdminInfoInputDTO inpuEntity)
        {
            //AutoMapper转换
            AdminInfo dataModel = Mapper.Map<AdminInfoInputDTO, AdminInfo>(inpuEntity);
            dataModel.PassWord = MD5Helper.EncryptString("123");
            dataModel.DelFlag = 0;
            //打上添加标记
            adminInfoDAL.AddFlag(dataModel);
            //添加默认数据 员工
            R_UserInfo_RoleInfo model = new R_UserInfo_RoleInfo
            {
                AdminID = dataModel.AdminID,
                RoleID = "3",
            };
            r_UserInfo_RoleInfoDAL.AddFlag(model);
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
        /// 根据value查询用户编号是否存在
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool CheckRepeat(string value)
        {
            AdminInfo model = adminInfoDAL.LoadEntities(u => u.AdminID == value).FirstOrDefault();
            if (model != null)
            {
                return true;//用户编号已存在
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 查询当前用户的信息
        /// </summary>
        /// <param name="AdminID"></param>
        /// <returns></returns>
        public AdminInfo GetModelByID(string AdminID)
        {
            return adminInfoDAL.LoadEntities(u => u.AdminID == AdminID).FirstOrDefault();
        }

        /// <summary>
        /// DTO用户信息修改
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <returns></returns>
        public int DTOUpdate(AdminInfoInputDTO inputEntity)
        {
            //先查  AsNoTracking()不追踪
            AdminInfo checkmodel = adminInfoDAL.LoadEntities(u => u.AdminID == inputEntity.AdminID).AsNoTracking().FirstOrDefault();

            if (checkmodel != null)
            {
                //AutoMapper转换
                AdminInfo dataModel = Mapper.Map<AdminInfoInputDTO, AdminInfo>(inputEntity);
                dataModel.ID = checkmodel.ID;
                dataModel.PassWord = checkmodel.PassWord;
                dataModel.DelFlag = checkmodel.DelFlag;
                return adminInfoDAL.EditAndSaveChange(dataModel);
               
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
            AdminInfo adminEntity = adminInfoDAL.LoadEntities(u => u.ID == id).FirstOrDefault();
            if (adminEntity != null)
            {
                if (flag == "true")
                {
                    adminEntity.DelFlag = 0;//把DelFlag字段修改为0，表示开启
                    if (adminInfoDAL.EditAndSaveChange(adminEntity) > 0)
                    {
                        return true;
                    }
                }
                else
                {
                    adminEntity.DelFlag = 1;//把DelFlag字段修改为1，表示禁用
                    if (adminInfoDAL.EditAndSaveChange(adminEntity) > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 查询当前密码是否正确
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool CheckPwd(string value)
        {
            string pwd = MD5Helper.EncryptString(value);
            AdminInfo model = adminInfoDAL.LoadEntities(u => u.PassWord == pwd).FirstOrDefault();
            if (model != null)
            {
                return true;//密码正确
            }
            else
            {
                return false;
            }
        }
    }
}
