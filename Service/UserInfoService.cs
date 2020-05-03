using AutoMapper;
using Common;
using DataModel;
using EntityModel;
using EntityModel.UserInfoDTO;
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
  public  class UserInfoService: IUserInfoService
    {
        UserInfoDAL userInfoDAL = DALFactory.UserInfoDAL;
        HouseInfoDAL houseInfoDAL = DALFactory.HouseInfoDAL;
        UnitOfWork unitOfWork = DALFactory.UnitOfWork;
        /// <summary>
        /// 查询业主所有信息
        /// </summary>
        /// <returns></returns>
        public List<UserInfo> GetUserInfo()
        {
            List<UserInfo> list = userInfoDAL.LoadEntities(u => u.ID > 0).Distinct().ToList();
            return list;
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="selectInfo"></param>
        /// <returns></returns>
        public List<UserInfo> GetPageList(int pageIndex, int pageSize, out int count, string selectInfo)
        {

            IQueryable<UserInfo> iquery;
            if (selectInfo == null)
            {
                //说明没有点击搜索按钮
                iquery = userInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.ID > 0, u => u.ID, true);
            }
            else
            {
                //根据文本框的值进行模糊查询
                iquery = userInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.Name.Contains(selectInfo), u => u.ID, true);
            }


            return iquery.ToList(); ;
        }

        /// <summary>
        /// DTO添加数据
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <returns></returns>
        public bool DTOAdd(UserInfoInputDTO inputEntity)
        {
            UserInfo dataModel = Mapper.Map<UserInfoInputDTO, UserInfo>(inputEntity);
            dataModel.PassWord = MD5Helper.EncryptString("123");
            //先查房屋信息把状态修改为已售并把业主加上去
            HouseInfo model = houseInfoDAL.LoadEntities(u => u.HousID == dataModel.HousID).FirstOrDefault();
            model.State = false;
            model.UserID = dataModel.UserID;


            //房屋打上修改标记
            houseInfoDAL.EditFlag(model);
            //业主打上添加标记
            userInfoDAL.AddFlag(dataModel);

            if (unitOfWork.SaveChanges()>0)
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
                UserInfo entity = userInfoDAL.LoadEntities(u => u.ID == ID).FirstOrDefault();
                if (entity != null&&string.IsNullOrEmpty(entity.HousID))
                {
                    //打标记
                    userInfoDAL.DeleteFlag(entity);
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
        public UserInfo GetUserByID(int id)
        {
            return userInfoDAL.LoadEntities(u => u.ID == id).FirstOrDefault();
        }

        /// <summary>
        /// 业主基本信息的修改查询
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public UserInfo GetModelByID(string UserID)
        {
            return userInfoDAL.LoadEntities(u => u.UserID == UserID).FirstOrDefault();
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
                UserInfo entity = userInfoDAL.LoadEntities(u => u.ID == ID).FirstOrDefault();

                if (entity!=null)
                {
                    //修改密码,为默认值，并MD5加密
                    entity.PassWord = MD5Helper.EncryptString("123");
                    userInfoDAL.EditFlag(entity);
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
        /// 检查用户编号是否已存在
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool CheckRepeat(string value)
        {
            UserInfo model = userInfoDAL.LoadEntities(u => u.UserID == value).FirstOrDefault();
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
        /// DTO修改
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <param name="outID"></param>
        /// <returns></returns>
        public bool DTOUpdate(UserInfoInputDTO inputEntity,out int outID)
        {
            //先查AsNoTracking() 不追踪
            outID = 0;
            UserInfo checkmodel = userInfoDAL.LoadEntities(u => u.UserID == inputEntity.UserID).AsNoTracking().FirstOrDefault();
            if (checkmodel != null)
            {
                //AutoMap转换
                UserInfo dataModel = Mapper.Map<UserInfoInputDTO, UserInfo>(inputEntity);
                //修改数据
                dataModel.ID = checkmodel.ID;//把ID赋值，让EF知道应该修改哪条数据
                dataModel.HousID = checkmodel.HousID;
                dataModel.PassWord = checkmodel.PassWord;
                if (!string.IsNullOrEmpty(inputEntity.HousID))
                {
                    dataModel.HousID = inputEntity.HousID;
                    //先查房屋信息i
                    HouseInfo entity = houseInfoDAL.LoadEntities(u => u.HousID == inputEntity.HousID).FirstOrDefault();
                    entity.UserID = dataModel.UserID;
                    entity.State = false;
                    //打上标记
                    houseInfoDAL.EditFlag(entity);
                }
                outID = dataModel.ID;//给ID赋值
                 userInfoDAL.EditFlag(dataModel);
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
        /// 业主登录
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <returns></returns>
        public UserInfo UserLogin(UserInfo inputEntity)
        {

            //先给密码进行MD5加密
            string pwd = MD5Helper.EncryptString(inputEntity.PassWord);
            //
            UserInfo model = userInfoDAL.LoadEntities(u => u.UserID == inputEntity.UserID && u.PassWord == pwd).FirstOrDefault();

            return model;

        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="model"></param>
        /// <param name="UserID"></param>
        /// <param name="NewPwd"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        public bool ResetPwd(string PassWord, string UserID, string NewPwd, string Pwd)
        {
            //先查
            UserInfo entity = userInfoDAL.LoadEntities(u => u.UserID == UserID).FirstOrDefault();
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
                    if (userInfoDAL.EditAndSaveChange(entity) > 0)
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
        /// 查询当前密码是否正确
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool CheckPwd(string value)
        {
            string pwd = MD5Helper.EncryptString(value);
            UserInfo model = userInfoDAL.LoadEntities(u => u.PassWord == pwd).FirstOrDefault();
            if (model != null)
            {
                return true;//密码正确
            }
            else
            {
                return false;
            }
        }

        public bool UserUpdate(UserInfo inputEntity)
        {
            UserInfo model = userInfoDAL.LoadEntities(u => u.UserID == inputEntity.UserID).FirstOrDefault();
            if (model != null)
            {
                //修改数据
                model.Phone = inputEntity.Phone;
                model.Name = inputEntity.Name;
                model.Sex = inputEntity.Sex;
                model.IDNumber = inputEntity.IDNumber;
                model.HousID = inputEntity.HousID;
                if (userInfoDAL.EditAndSaveChange(model) > 0)
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
        /// <summary>
        /// 查询用户身份证是否已存在
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool CheckIDnumber(string value)
        {
            UserInfo model = userInfoDAL.LoadEntities(u => u.IDNumber == value).FirstOrDefault();
            if (model != null)
            {
                return true;//说明用户身份证号码已存在
            }
            else
            {
                return false;
            }
        }
    }
}
