using DataModel;
using EntityModel;
using EntityModel.RepairInfoDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IService
{
   public interface IRepairInfoService
    {

        /// <summary>
        /// DTO分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="strHousID"></param>
        /// <param name="State"></param>
        /// <returns></returns>
         List<RepairInfoOutputDTO> GetPageList(int pageIndex, int pageSize, out int count, string strHousID, string State,string RsubmitDate,string EndDate);
        /// <summary>
        /// 根据报修单号查询
        /// </summary>
        /// <param name="RepairID"></param>
        /// <returns></returns>
        RepairInfoOutput GetModelByID(string RepairID);
        /// <summary>
        /// DTO报修 修改
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <param name="AdminID"></param>
        /// <returns></returns>
         int DTOUpdate(RepairInfoUpdateInputDTO inputEntity, string AdminID);
        /// <summary>
        /// DTO添加数据
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <returns></returns>
         bool DTOAdd(RepairInfoInputDTO inputEntity);
        /// <summary>
        /// 删除方法
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
         bool Delete(Array id);

        /// <summary>
        /// 订单生成方法
        /// </summary>
        /// <returns></returns>
         string CreateOrderCode();

        /// <summary>
        /// 基于业主的查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
         List<RepairInfoOutput> GetPageList(int pageIndex, int pageSize, out int count, string UserID,int State, DateTime DRsubmitDate, DateTime DEndDate);
        /// <summary>
        /// 基于业主的修改
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <returns></returns>
         bool User_Update(RepairInfo inputEntity);
        /// <summary>
        /// 业主根据ID取消报修
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
         bool Cancel(RepairInfo inputEntity, int ID);
        /// <summary>
        /// 查询所有报修订单
        /// </summary>
        /// <returns></returns>
        List<RepairInfo> GetAllRepairInfo();
    }
}
