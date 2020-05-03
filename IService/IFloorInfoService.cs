using DataModel;
using EntityModel.FloorInfoDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IService
{
  public  interface IFloorInfoService
    {
        /// <summary>
        /// 查询全部楼栋信息
        /// </summary>
        /// <returns></returns>
         List<FloorInfo> GetFloorInfo();
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="selectInfo"></param>
        /// <returns></returns>
         List<FloorInfo> GetPageList(int pageIndex, int pageSize, out int count, string selectInfo);
        /// <summary>
        /// 根据ID查询楼栋信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
         FloorInfo GetModelByID(int id);

        /// <summary>
        /// DTO楼栋信息添加
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <returns></returns>
         bool DTOAdd(FloorInfoInputDTO inputEntity);
        /// <summary>
        /// 楼栋信息修改
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <param name="outID"></param>
        /// <returns></returns>
         int DTOUpdate(FloorInfoInputDTO inputEntity, out int outID);
        /// <summary>
        /// 检查楼栋编号是否已存在
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
         bool CheckRepeat(string value);
        /// <summary>
        /// 根据ID 楼栋信息删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(Array id);
    }
}
