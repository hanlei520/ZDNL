using AutoMapper;
using DataModel;
using EntityModel;
using EntityModel.VillageInfoDTO;
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
    public class VillageInfoService: IVillageInfoService
    {
        VillageInfoDAL villageInfoDAL = DALFactory.VillageInfoDAL;
        UnitOfWork unitOfWork = DALFactory.UnitOfWork;
        /// <summary>
        /// 查询小区信息
        /// </summary>
        /// <returns></returns>
        public List<VillageInfo> GetVillageInfo()
        {
            List<VillageInfo> list = villageInfoDAL.LoadEntities(u => u.ID > 0).Distinct().ToList();
            return list;
        }
        /// <summary>
        /// 根据ID查询小区信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public VillageInfo GetModelByID(int id)
        {
            return villageInfoDAL.LoadEntities(u => u.ID == id).FirstOrDefault();
        }
        /// <summary>
        /// 小区分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="selectInfo"></param>
        /// <returns></returns>
        public List<VillageInfo> GetPageList(int pageIndex, int pageSize, out int count, string selectInfo)
        {

            IQueryable<VillageInfo> iquery;
            if (selectInfo == null)
            {
                //说明没有点击搜索按钮
                iquery = villageInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.ID > 0, u => u.ID, true);
            }
            else
            {
                //根据文本框的值进行模糊查询
                iquery = villageInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.VillageName.Contains(selectInfo), u => u.ID, true);
            }
         
            return iquery.ToList();
        }
        /// <summary>
        /// 小区添加方法
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <returns></returns>
        public bool DTOAdd(VillageInfoInputDTO inputEntity)
        {
            VillageInfo dataModel = Mapper.Map<VillageInfoInputDTO, VillageInfo>(inputEntity);
            if (villageInfoDAL.AddAndSaveChange(dataModel) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 小区修改方法
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <param name="outID"></param>
        /// <returns></returns>
        public int DTOUpdate(VillageInfoInputDTO inputEntity, out int outID)
        {
            //先查AsNoTracking()不追踪
            outID = 0;
            VillageInfo model = villageInfoDAL.LoadEntities(u => u.VillageID == inputEntity.VillageID).AsNoTracking().FirstOrDefault();
            if (model != null)
            {
                //修改数据
                //model.VillageName = inputEntity.VillageName;
                VillageInfo dataModel = Mapper.Map<VillageInfoInputDTO, VillageInfo>(inputEntity);
                dataModel.ID = model.ID;

                outID = model.ID;
                return villageInfoDAL.EditAndSaveChange(dataModel);
               
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 检查小区编号是否已存在
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool CheckRepeat(string value)
        {
            VillageInfo model = villageInfoDAL.LoadEntities(u => u.VillageID == value).FirstOrDefault();
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
        /// 小区删除方法
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(Array id)
        {
            foreach (var item in id)
            {
                int ID = Convert.ToInt32(item);
                //先查
                VillageInfo entity = villageInfoDAL.LoadEntities(u => u.ID == ID).FirstOrDefault();
                if (entity != null)
                {
                    //打标记
                    villageInfoDAL.DeleteFlag(entity);
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
    }
}
