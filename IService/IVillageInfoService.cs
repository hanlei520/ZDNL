using DataModel;
using EntityModel.VillageInfoDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IService
{
  public  interface IVillageInfoService
    {
        /// <summary>
        /// 查询小区信息
        /// </summary>
        /// <returns></returns>
         List<VillageInfo> GetVillageInfo();
        /// <summary>
        /// 根据ID查询小区信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
         VillageInfo GetModelByID(int id);
        /// <summary>
        /// 小区分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="selectInfo"></param>
        /// <returns></returns>
         List<VillageInfo> GetPageList(int pageIndex, int pageSize, out int count, string selectInfo);
        /// <summary>
        ///DTO 小区添加方法
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <returns></returns>
         bool DTOAdd(VillageInfoInputDTO inputEntity);
        /// <summary>
        /// 小区修改方法
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <param name="outID"></param>
        /// <returns></returns>
         int DTOUpdate(VillageInfoInputDTO inputEntity, out int outID);
        /// <summary>
        /// 检查小区编号是否已存在
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
         bool CheckRepeat(string value);
        /// <summary>
        /// 小区删除方法
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
         bool Delete(Array id);
    }
}
