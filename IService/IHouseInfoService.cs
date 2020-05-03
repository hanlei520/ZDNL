using DataModel;
using EntityModel;
using EntityModel.HouseInfoDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IService
{
   public interface IHouseInfoService
    {
        /// <summary>
        /// 查询未售的房屋
        /// </summary>
        /// <returns></returns>
         List<HouseInfo> GetTrueList();

        /// <summary>
        /// 查询已售的房屋
        /// </summary>
        /// <returns></returns>
         List<HouseInfo> GetFalseList();
        /// <summary>
        /// 根据房屋编号查询已售的房屋信息
        /// </summary>
        /// <param name="HousID"></param>
        /// <returns></returns>
         HouseInfo GetList(string HousID);
        /// <summary>
        /// 查询所有房屋信息
        /// </summary>
        /// <returns></returns>
         List<HouseInfo> GetAllHouseList();


        /// <summary>
        /// 查询房屋编号是否已存在
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
         bool CheckRepeat(string value);


        /// <summary>
        /// 根据ID查房屋信息
        /// </summary>
        /// <returns></returns>
         HouseInfo GetHouseInfoByID(int ID);

        /// <summary>
        /// 根据ID删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
         bool Delete(Array id);

        /// <summary>
        /// 根据ID置空房间
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
         bool Empty(Array id);

        #region DTO
        /// <summary>
        /// DTO添加房屋信息
        /// </summary>
        /// <param name="inputentity"></param>
        /// <returns></returns>
        bool DTOAdd(HouseInfoInputDTO inputentity);

        /// <summary>
        /// DTO更新数据
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <returns></returns>
        int DTOUpdate(HouseInfoInputDTO inputEntity, out int outID);
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
        List<HouseInfoOutputDTO> DTOGetPageList(int pageIndex, int pageSize, out int count, string VillageID, string selectInfo, string State);
        #endregion
    }
}
