using DataModel;
using EntityModel;
using EntityModel.PropertyInfoDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IService
{
  public  interface IPropertyInfoService
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="strHousID"></param>
        /// <param name="State"></param>
        /// <returns></returns>
         List<PropertyInfoOutputDTO> GetPageList(int pageIndex, int pageSize, out int count, string strHousID, string State,string MonthID);

        /// <summary>
        /// 根据ID查询数据
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        PropertyInfoOutput GetModelByID(string PropertyID);
        /// <summary>
        /// DTO添加数据
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <returns></returns>
         bool DTOAdd(PropertyInfoInputDTO inputEntity);

        /// <summary>
        /// 根据ID删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
         bool Delete(Array id);

        /// <summary>
        /// 修改方法
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <param name="AdminID"></param>
        /// <returns></returns>
         int DTOUpdate(PropertyInfoUpdateInputDTO inputEntity, string AdminID);

        /// <summary>
        /// 订单生成方法
        /// </summary>
        /// <returns></returns>
         string CreateOrderCode();

        /// <summary>
        /// 根据房屋编号查询
        /// </summary>
        /// <param name="HousID"></param>
        /// <returns></returns>
         HouseInfo GetPropertyCost(string HousID);

    }
}
