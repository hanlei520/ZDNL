using DataModel;
using EntityModel;
using EntityModel.CostInfoDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IService
{
  public  interface ICostInfoService
    {
        /// <summary>
        /// 订单生成方法
        /// </summary>
        /// <returns></returns>
        string CreateOrderCode();

        /// <summary>
        /// DTO分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
         List<CostInfoOutputDTO> GetPageList(int pageIndex, int pageSize, out int count, string strState, string selectInfo, DateTime DRsubmitDate, DateTime DRsolveDate);


        /// <summary>
        /// DTO添加水电费信息
        /// </summary>
        /// <param name="inputentity"></param>
        /// <returns></returns>
         bool DTOAdd(CostInfoInputDTO inputentity);


        /// <summary>
        /// 根据ID查水电费信息
        /// </summary>
        /// <returns></returns>
         CostInfo GetCostInfoByID(int ID);

        /// <summary>
        /// DTO更新数据
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <returns></returns>
         int DTOUpdate(CostInfoUpdateInputDTO inputEntity, out int outID);

        /// <summary>
        /// 根据ID删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
         bool Delete(Array id);

        /// <summary>
        /// DTO基于业主的分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
         List<CostInfoOutputDTO> User_GetPageList(int pageIndex, int pageSize, out int count, string HousID, string State, DateTime DRsubmitDate, DateTime RsolveDate);
    }
}
