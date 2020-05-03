using DataModel;
using EntityModel;
using EntityModel.ComplaInfoDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IService
{
   public interface IComplaInfoService
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
         List<ComplaInfoOutputDTO> GetPageList(int pageIndex, int pageSize, out int count, string strHousID, int State, DateTime DRsubmitDate, DateTime DEndDate);
        /// <summary>
        /// 根据ID查询数据
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
         ComplaInfoOutput GetModelByID(string ComplaID);
        /// <summary>
        /// DTO修改方法
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <param name="AdminID"></param>
        /// <returns></returns>
         int DTOUpdate(ComplaInfoUpdateInputDTO inputEntity, string AdminID);
        /// <summary>
        /// 删除
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
        /// DTO添加数据
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <returns></returns>
         bool DTOAdd(ComplaInfoInputDTO inputEntity);


        /// <summary>
        /// DTO基于业主的查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
         List<ComplaInfoOutputDTO> UserGetPageList(int pageIndex, int pageSize, out int count, string UserID, string CompType, int State, DateTime DRsubmitDate, DateTime DEndDate);


        /// <summary>
        /// 修改投诉/建议方法
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <returns></returns>
        bool User_Update(ComplaInfo inputEntity);

        /// <summary>
        /// 业主根据ID取消投诉建议
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
         bool Cancel(int ID);
        /// <summary>
        /// 查询所有投诉订单
        /// </summary>
        /// <returns></returns>
        List<ComplaInfo> GetAllComplaInfo();
    }
}
