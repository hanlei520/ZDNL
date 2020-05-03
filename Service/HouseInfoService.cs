using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataModel;
using EntityModel;
using EntityModel.HouseInfoDTO;
using IService;
using Repository;
using Repository.DAL;

namespace Service
{
    public class HouseInfoService : IHouseInfoService
    {
        HouseInfoDAL houseInfoDAL = DALFactory.HouseInfoDAL;
        VillageInfoDAL villageInfoDAL = DALFactory.VillageInfoDAL;
        FloorInfoDAL FloorInfoDAL = DALFactory.FloorInfoDAL;
        UserInfoDAL userInfoDAL = DALFactory.UserInfoDAL;
        UnitOfWork unitOfWork = DALFactory.UnitOfWork;

        /// <summary>
        /// 查询未售的房屋
        /// </summary>
        /// <returns></returns>
        public List<HouseInfo> GetTrueList()
        {
            return houseInfoDAL.LoadEntities(u => u.State == true).ToList();
        }

        /// <summary>
        /// 查询已售的房屋
        /// </summary>
        /// <returns></returns>
        public List<HouseInfo> GetFalseList()
        {
            return houseInfoDAL.LoadEntities(u => u.State == false).ToList();
        }
        /// <summary>
        /// 根据房屋编号查询已售的房屋信息
        /// </summary>
        /// <param name="HousID"></param>
        /// <returns></returns>
        public HouseInfo GetList(string HousID)
        {
            return houseInfoDAL.LoadEntities(u => u.HousID == HousID && u.State == false).FirstOrDefault();
        }
        /// <summary>
        /// 查询所有房屋信息
        /// </summary>
        /// <returns></returns>
        public List<HouseInfo> GetAllHouseList()
        {
            return houseInfoDAL.LoadEntities(u => u.ID > 0).ToList();
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>

        /// <summary>
        /// 查询房屋编号是否已存在
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool CheckRepeat(string value)
        {
            HouseInfo list = houseInfoDAL.LoadEntities(u => u.HousID == value).FirstOrDefault();
            if (list != null)
            {
                return true;//说明用户已存在
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 根据ID查房屋信息
        /// </summary>
        /// <returns></returns>
        public HouseInfo GetHouseInfoByID(int ID)
        {
            return houseInfoDAL.LoadEntities(u => u.ID == ID).FirstOrDefault();
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
                HouseInfo entity = houseInfoDAL.LoadEntities(u => u.ID == ID).FirstOrDefault();
                if (entity != null && entity.State == true)
                {
                    //打标记
                    houseInfoDAL.DeleteFlag(entity);
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
        /// 根据ID置空房间
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Empty(Array id)
        {
            foreach (var item in id)
            {
                int ID = Convert.ToInt32(item);
                //先查房屋信息i
                HouseInfo entity = houseInfoDAL.LoadEntities(u => u.ID == ID).FirstOrDefault();
                //先查用户信息
                UserInfo Uentity = userInfoDAL.LoadEntities(u => u.HousID == entity.HousID).FirstOrDefault();
                if (entity != null)
                {
                    entity.UserID = "";
                    entity.State = true;
                    //打上标记
                    houseInfoDAL.EditFlag(entity);
                }
                if (Uentity != null)
                {
                    Uentity.HousID = "";
                    //打上标记
                    userInfoDAL.EditFlag(Uentity);
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
        #region DTO
        /// <summary>
        /// DTO添加数据
        /// </summary>
        /// <param name="inputentity"></param>
        /// <returns></returns>
        public bool DTOAdd(HouseInfoInputDTO inputentity)
        {
            HouseInfo dataModel = Mapper.Map<HouseInfoInputDTO, HouseInfo>(inputentity);
            dataModel.State = true;
            if (houseInfoDAL.AddAndSaveChange(dataModel) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// DTO分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="VillageID"></param>
        /// <param name="selectInfo"></param>
        /// <param name="State"></param>
        /// <returns></returns>
        public List<HouseInfoOutputDTO> DTOGetPageList(int pageIndex, int pageSize, out int count, string VillageID, string selectInfo, string State)
        {

            IQueryable<HouseInfo> iquery;
            if (string.IsNullOrEmpty(selectInfo) && string.IsNullOrEmpty(VillageID) && string.IsNullOrEmpty(State))
            {
                //说明没有点击搜索按钮
                iquery = houseInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.ID > 0, u => u.ID, true);
            }
            else
            {
                bool bState = false;
                if (State == "True")
                {
                    bState = true;
                }

                //三个条件 房屋编号+小区+状态
                if (!string.IsNullOrEmpty(selectInfo) && !string.IsNullOrEmpty(VillageID) && !string.IsNullOrEmpty(State))
                {
                    iquery = houseInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.HousID.Contains(selectInfo) && u.VillageID == VillageID && u.State == bState, u => u.ID, true);
                }
                else if (!string.IsNullOrEmpty(VillageID) && !string.IsNullOrEmpty(State))//小区+状态
                {
                    iquery = houseInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.VillageID == VillageID && u.State == bState, u => u.ID, true);
                }
                else if (!string.IsNullOrEmpty(selectInfo) && !string.IsNullOrEmpty(State))//两个条件 房屋编号+状态
                {
                    iquery = houseInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.HousID.Contains(selectInfo) && u.State == bState, u => u.ID, true);
                }
                else if (!string.IsNullOrEmpty(selectInfo) && !string.IsNullOrEmpty(VillageID))//两个条件 房屋编号+小区
                {
                    iquery = houseInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.HousID.Contains(selectInfo) && u.VillageID == VillageID, u => u.ID, true);
                }
                else if (!string.IsNullOrEmpty(selectInfo))//房屋编号
                {
                    iquery = houseInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.HousID.Contains(selectInfo), u => u.ID, true);
                }
                else if (!string.IsNullOrEmpty(VillageID))//小区
                {
                    iquery = houseInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.VillageID == VillageID, u => u.ID, true);
                }
                else/* if (!string.IsNullOrEmpty(State))*///状态
                {

                    iquery = houseInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.State == bState, u => u.ID, true);
                }
            }
            #region
            //先拿到所有信息
            IQueryable<VillageInfo> ViIquery = villageInfoDAL.LoadEntities(u => u.ID > 0);//获取小区信息表
            IQueryable<FloorInfo> FlIquery = FloorInfoDAL.LoadEntities(u => u.ID > 0);
            IQueryable<UserInfo> UsIquery = userInfoDAL.LoadEntities(u => u.ID > 0);

            //连表查询
            IQueryable<HouseInfoOutputDTO> linq = (from a in iquery
                                            join b in ViIquery on a.VillageID equals b.VillageID into b_join
                                            from c in b_join.DefaultIfEmpty()
                                            join d in FlIquery on a.FloorID equals d.FloorID into c_join
                                            from e in c_join.DefaultIfEmpty()
                                            join f in UsIquery on a.UserID equals f.UserID into d_join
                                            from g in d_join.DefaultIfEmpty()
                                            select new HouseInfoOutputDTO
                                            {
                                                ID = a.ID,
                                                UserID = a.UserID,
                                                FloorID = a.FloorID,
                                                HousID = a.HousID,
                                                HouseArea = a.HouseArea,
                                                HouseType = a.HouseType,
                                                State = a.State,
                                                PropertyCost = a.PropertyCost,
                                                Purpose = a.Purpose,
                                                VillageID = a.VillageID,
                                                VillageName = c.VillageName,
                                                FloorName = e.FloorName,
                                                UserName = g.Name,
                                            });
            #endregion
            return linq.ToList();
            //}
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <returns></returns>
        public int DTOUpdate(HouseInfoInputDTO inputEntity, out int outID)
        {
            //先查房屋信息 AsNoTracking() 不追踪
            outID = 0;
            HouseInfo checkmodel = houseInfoDAL.LoadEntities(u => u.HousID == inputEntity.HousID).AsNoTracking().FirstOrDefault();

            if (checkmodel != null)
            {
                //AutoMap转换
                HouseInfo dataModel = Mapper.Map<HouseInfoInputDTO, HouseInfo>(inputEntity);
                //修改数据
                dataModel.UserID = checkmodel.UserID;
                dataModel.State = checkmodel.State;
                dataModel.ID = checkmodel.ID;//把ID赋值，让EF知道应该修改哪条数据

                outID = dataModel.ID;//给ID赋值
                return houseInfoDAL.EditAndSaveChange(dataModel);
            }
            else
            {
                return 0;
            }
        }
        #endregion

    }
}
