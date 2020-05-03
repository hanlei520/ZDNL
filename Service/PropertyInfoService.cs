using AutoMapper;
using DataModel;
using EntityModel;
using EntityModel.PropertyInfoDTO;
using IService;
using Repository;
using Repository.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class PropertyInfoService: IPropertyInfoService
    {
        PropertyInfoDAL propertyInfoDAL = DALFactory.PropertyInfoDAL;
        UserInfoDAL userInfoDAL = DALFactory.UserInfoDAL;
        AdminInfoDAL adminInfoDAL = DALFactory.AdminInfoDAL;
        UnitOfWork unitOfWork = DALFactory.UnitOfWork;
        HouseInfoDAL houseInfoDAL = DALFactory.HouseInfoDAL;

        /// <summary>
        /// DTO分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="strHousID"></param>
        /// <param name="State"></param>
        /// <returns></returns>
        public List<PropertyInfoOutputDTO> GetPageList(int pageIndex, int pageSize, out int count,string strHousID, string State,string MonthID)
        {
            //说明没有点击搜索按钮
            IQueryable<PropertyInfo> iquery;

            if (string.IsNullOrEmpty(strHousID) && string.IsNullOrEmpty(State)&&string.IsNullOrEmpty(MonthID))
            {
                //说明没有点击搜索按钮
                iquery = propertyInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.ID > 0, u => u.ID, true);
            }
            else
            {
                int iState = 0;
                if (!string.IsNullOrEmpty(State)&&!string.IsNullOrEmpty(strHousID)&&!string.IsNullOrEmpty(MonthID))//状态+房屋编号+月份
                {
                    iState = Convert.ToInt32(State);
                    iquery = propertyInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.State == iState && u.HousID.Contains(strHousID)&&u.Month==MonthID, u => u.ID, true);
                }
                else if (!string.IsNullOrEmpty(State) && !string.IsNullOrEmpty(strHousID))//状态+房屋编号
                {
                    iState = Convert.ToInt32(State);
                    iquery = propertyInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.State == iState && u.HousID.Contains(strHousID), u => u.ID, true);
                }
                else if (!string.IsNullOrEmpty(State) && !string.IsNullOrEmpty(MonthID))//状态+月份
                {
                    iState = Convert.ToInt32(State);
                    iquery = propertyInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.State == iState && u.Month == MonthID, u => u.ID, true);
                }
                else if (!string.IsNullOrEmpty(strHousID)&&!string.IsNullOrEmpty(MonthID))//房屋编号+月份
                {
                    iquery = propertyInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.HousID.Contains(strHousID) && u.Month == MonthID, u => u.ID, true);
                }
                else if (!string.IsNullOrEmpty(MonthID))//月份
                {
                    iquery = propertyInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.Month == MonthID, u => u.ID, true);
                }
                else if (!string.IsNullOrEmpty(State))//状态
                {
                    iState = Convert.ToInt32(State);
                    iquery = propertyInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.State == iState, u => u.ID, true);
                }
                else//房屋编号
                {
                    iquery = propertyInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.HousID.Contains(strHousID), u => u.ID, true);
                }
            }
            //先拿到所有信息
            IQueryable<UserInfo> userIquery = userInfoDAL.LoadEntities(u => u.ID > 0);//业主信息表
            IQueryable<AdminInfo> adminIquery = adminInfoDAL.LoadEntities(u => u.ID > 0);//管理员信息表
            //连表查询
            IQueryable<PropertyInfoOutputDTO> linq = (from a in iquery
                                                   join b in userIquery on a.HousID equals b.HousID into b_join
                                                   from c in b_join.DefaultIfEmpty()
                                                   join d in adminIquery on a.AdminID equals d.AdminID into c_join
                                                   from g in c_join.DefaultIfEmpty()
                                                   select new PropertyInfoOutputDTO
                                                   {
                                                       ID = a.ID,
                                                       PropertyID = a.PropertyID,
                                                       HouseID = a.HousID,
                                                       PropertyCost = a.PropertyCost,
                                                       UserName = c.Name,
                                                       Month = a.Month,
                                                       State = a.State,
                                                       AdminName = g.Name,
                                                   });



            return linq.ToList();
        }

        /// <summary>
        /// 根据ID查询数据
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public PropertyInfoOutput GetModelByID(string PropertyID)
        {
            //先拿到所有信息
            IQueryable<AdminInfo> adminIquery = adminInfoDAL.LoadEntities(u => u.ID > 0);//业主信息表
            IQueryable<PropertyInfo> iquery = propertyInfoDAL.LoadEntities(u => u.PropertyID == PropertyID);
            //连表查询
            IQueryable<PropertyInfoOutput> linq = (from a in iquery
                                                   join d in adminIquery on a.AdminID equals d.AdminID into c_join
                                                   from g in c_join.DefaultIfEmpty()
                                                   select new PropertyInfoOutput
                                                   {
                                                       ID = a.ID,
                                                       PropertyID = a.PropertyID,
                                                       HouseID = a.HousID,
                                                       PropertyCost = a.PropertyCost,
                                                       UserName = g.Name,
                                                       Month = a.Month,
                                                       State = a.State,
                                                       AdminName = g.Name,
                                                   });

            return linq.FirstOrDefault();
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <returns></returns>
        public bool DTOAdd(PropertyInfoInputDTO inputEntity)
        {
            //AutoMapper转换
            PropertyInfo dataModel = Mapper.Map<PropertyInfoInputDTO, PropertyInfo>(inputEntity);
            dataModel.PropertyID = CreateOrderCode();
            if (propertyInfoDAL.AddAndSaveChange(dataModel) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
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
                PropertyInfo entity = propertyInfoDAL.LoadEntities(u => u.ID == ID).FirstOrDefault();
                if (entity != null)
                {
                    //打标记
                    propertyInfoDAL.DeleteFlag(entity);
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
        /// 修改方法
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <param name="AdminID"></param>
        /// <returns></returns>
        public int DTOUpdate(PropertyInfoUpdateInputDTO inputEntity, string AdminID)
        {
            PropertyInfo model = propertyInfoDAL.LoadEntities(u => u.PropertyID == inputEntity.PropertyID).FirstOrDefault();
            if (model != null)
            {
                //修改数据
                model.State = inputEntity.State;
                model.AdminID = AdminID;
                //model.Month = inputEntity.Month;

                return propertyInfoDAL.EditAndSaveChange(model);

            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 订单生成方法
        /// </summary>
        /// <returns></returns>
        public string CreateOrderCode()
        {
            //订单编号生成方法     
            string ordercode = "WY" + System.DateTime.Now.Date.ToShortDateString();//OD2019/12/10
            ordercode = ordercode.Replace("/", "");//OD20191210
            //List<CostInfo> model = CostInfoService.GetList(ordercode);
            List<PropertyInfo> list = propertyInfoDAL.LoadEntities(u => u.PropertyID.Contains(ordercode)).ToList();
            int num = Convert.ToInt16(list.Count) + 1;//如果查出0条，加1
            ordercode += (num.ToString()).PadLeft(4, '0');//OD20191210001        PadLeft往左补零

            return ordercode;
        }

        /// <summary>
        /// 根据房屋编号查询
        /// </summary>
        /// <param name="HousID"></param>
        /// <returns></returns>
        public HouseInfo GetPropertyCost(string HousID)
        {
            return houseInfoDAL.LoadEntities(u => u.HousID == HousID).FirstOrDefault();
        }
    }
}
