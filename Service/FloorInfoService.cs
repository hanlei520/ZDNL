using AutoMapper;
using DataModel;
using EntityModel;
using EntityModel.FloorInfoDTO;
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
    public class FloorInfoService: IFloorInfoService
    {
        FloorInfoDAL floorInfoDAL = DALFactory.FloorInfoDAL;
        UnitOfWork unitOfWork = DALFactory.UnitOfWork;

        /// <summary>
        /// 查询全部楼栋信息
        /// </summary>
        /// <returns></returns>
        public List<FloorInfo> GetFloorInfo()
        {
            List<FloorInfo> list = floorInfoDAL.LoadEntities(u => u.ID > 0).Distinct().ToList();
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
        public List<FloorInfo> GetPageList(int pageIndex, int pageSize, out int count, string selectInfo)
        {

            IQueryable<FloorInfo> iquery;
            if (selectInfo == null)
            {
                //说明没有点击搜索按钮
                iquery = floorInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.ID > 0, u => u.ID, true);
            }
            else
            {
                //根据文本框的值进行模糊查询
                iquery = floorInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.FloorName.Contains(selectInfo), u => u.ID, true);
            }


            return iquery.ToList(); 
        }
        /// <summary>
        /// 根据ID查询楼栋信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FloorInfo GetModelByID(int id)
        {
            return floorInfoDAL.LoadEntities(u => u.ID == id).FirstOrDefault();
        }

        /// <summary>
        /// 楼栋信息添加
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <returns></returns>
        public bool DTOAdd(FloorInfoInputDTO inputEntity)
        {
            FloorInfo dataModel = Mapper.Map<FloorInfoInputDTO,FloorInfo>(inputEntity);
            if (floorInfoDAL.AddAndSaveChange(dataModel) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 楼栋信息修改
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <param name="outID"></param>
        /// <returns></returns>
        public int DTOUpdate(FloorInfoInputDTO inputEntity, out int outID)
        {
            //先查
            outID = 0;
            FloorInfo model = floorInfoDAL.LoadEntities(u => u.FloorID == inputEntity.FloorID).AsNoTracking().FirstOrDefault();
            if (model != null)
            {
               // AutoMapper转换
                FloorInfo dataModel = Mapper.Map<FloorInfoInputDTO, FloorInfo>(inputEntity);
                //修改数据
                dataModel.ID = model.ID;
                outID = model.ID;
                return floorInfoDAL.EditAndSaveChange(dataModel);
               
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 检查楼栋编号是否已存在
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool CheckRepeat(string value)
        {
            FloorInfo model = floorInfoDAL.LoadEntities(u => u.FloorID == value).FirstOrDefault();
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
        /// 根据ID 楼栋信息删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(Array id)
        {
            foreach (var item in id)
            {
                int ID = Convert.ToInt32(item);
                //先查
                FloorInfo entity = floorInfoDAL.LoadEntities(u => u.ID == ID).FirstOrDefault();
                if (entity != null)
                {
                    //打标记
                    floorInfoDAL.DeleteFlag(entity);
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
