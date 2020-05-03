using AutoMapper;
using DataModel;
using EntityModel;
using EntityModel.CostInfoDTO;
using IService;
using Repository;
using Repository.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Service
{
    public class CostInfoService: ICostInfoService
    {
        CostInfoDAL costInfoDAL = DALFactory.CostInfoDAL;
        AdminInfoDAL adminInfoDAL = DALFactory.AdminInfoDAL;
        UserInfoDAL userInfoDAL = DALFactory.UserInfoDAL;
        UnitOfWork unitOfWork = DALFactory.UnitOfWork;

        /// <summary>
        /// 订单生成方法
        /// </summary>
        /// <returns></returns>
        public string CreateOrderCode()
        {
            //订单编号生成方法     
            string ordercode = "SD" + System.DateTime.Now.Date.ToShortDateString();//SD2019/12/10
            ordercode = ordercode.Replace("/", "");//SD20191210
            //List<CostInfo> model = CostInfoService.GetList(ordercode);
            List<CostInfo> list = costInfoDAL.LoadEntities(u => u.CostID.Contains(ordercode)).ToList();
            int num = Convert.ToInt16(list.Count) + 1;//如果查出0条，加1
            ordercode += (num.ToString()).PadLeft(4, '0');//SD20191210001        PadLeft往左补零

            return ordercode;
        }

        /// <summary>
        /// DTO分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<CostInfoOutputDTO> GetPageList(int pageIndex, int pageSize, out int count, string strState, string selectInfo, DateTime DRsubmitDate, DateTime DRsolveDate)
        {
            DateTime Time = new DateTime();//默认时间
            bool bState = false;
            if (strState == "True")
            {
                bState = true;
            }

            IQueryable<CostInfo> iquery;
            if (string.IsNullOrEmpty(strState) && string.IsNullOrEmpty(selectInfo) && DRsubmitDate == Time && DRsolveDate ==Time)
            {
                //说明没有点击搜索按钮
                iquery = costInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.ID > 0, u => u.ID, true);
            }
            else
            {
                if (!string.IsNullOrEmpty(strState) && !string.IsNullOrEmpty(selectInfo) && DRsubmitDate!= Time && DRsolveDate!= Time)//状态+房屋编号+月份
                {
                    iquery = costInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.State == bState && u.HousID.Contains(selectInfo) && u.RsubmitDate>= DRsubmitDate&&u.RsolveDate<=DRsolveDate, u => u.ID, true);
                }
                else if (!string.IsNullOrEmpty(strState) && !string.IsNullOrEmpty(selectInfo))//状态+房屋编号
                {

                    iquery = costInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.State == bState && u.HousID.Contains(selectInfo), u => u.ID, true);
                }
                else if (!string.IsNullOrEmpty(strState) && DRsubmitDate != Time && DRsolveDate != Time)//状态+月份
                {
                    iquery = costInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.State == bState && u.RsubmitDate >= DRsubmitDate && u.RsolveDate <= DRsolveDate, u => u.ID, true);
                }
                else if (!string.IsNullOrEmpty(selectInfo) && DRsubmitDate != Time && DRsolveDate != Time)//房屋编号+月份
                {
                    iquery = costInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.HousID.Contains(selectInfo) && u.RsubmitDate >= DRsubmitDate && u.RsolveDate <= DRsolveDate, u => u.ID, true);
                }
                else if (DRsubmitDate != Time && DRsolveDate != Time)//月份
                {

                    iquery = costInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u =>u.RsubmitDate >= DRsubmitDate && u.RsolveDate <= DRsolveDate, u => u.ID, true);
                }
                else if (!string.IsNullOrEmpty(strState))//状态
                {
                    iquery = costInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.State == bState, u => u.ID, true);
                }
                else//房屋编号
                {
                    iquery = costInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.HousID.Contains(selectInfo), u => u.ID, true);
                }
            }
            #region
            //先拿到所有信息
            IQueryable<AdminInfo> AdIquery = adminInfoDAL.LoadEntities(u => u.ID > 0);//获取管理员信息表
            IQueryable<UserInfo> UsIquery = userInfoDAL.LoadEntities(u => u.ID > 0);//获取业主信息表


            //连表查询
            IQueryable<CostInfoOutputDTO> linq = (from a in iquery
                                               join b in AdIquery on a.AdminID equals b.AdminID into b_join
                                               from c in b_join.DefaultIfEmpty()
                                               join d in UsIquery on a.HousID equals d.HousID into c_join
                                               from e in c_join.DefaultIfEmpty()
                                               select new CostInfoOutputDTO
                                               {
                                                   ID = a.ID,
                                                   CostID = a.CostID,
                                                   CostName = a.CostName,
                                                   HousID = a.HousID,
                                                   RsubmitDate = a.RsubmitDate,
                                                   RsolveDate = a.RsolveDate,
                                                   WaterMeter = a.WaterMeter,
                                                   ElectricMete = a.ElectricMete,
                                                   WaterPrice = a.WaterPrice,
                                                   WirePrice = a.WirePrice,
                                                   SumMoney = a.SumMoney,
                                                   AdminID = a.AdminID,
                                                   Name = c.Name,
                                                   State = a.State,
                                                   UserName = e.Name,
                                               });
            #endregion
            return linq.ToList(); ;
            //}
        }


        /// <summary>
        /// DTO添加水电费信息
        /// </summary>
        /// <param name="inputentity"></param>
        /// <returns></returns>
        public bool DTOAdd(CostInfoInputDTO inputentity)
        {
            //AutoMapper转换
            CostInfo dataModel = Mapper.Map<CostInfoInputDTO, CostInfo>(inputentity);
            dataModel.CostID = CreateOrderCode();
            if (costInfoDAL.AddAndSaveChange(dataModel) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 根据ID查水电费信息
        /// </summary>
        /// <returns></returns>
        public CostInfo GetCostInfoByID(int ID)
        {
            return costInfoDAL.LoadEntities(u => u.ID == ID).FirstOrDefault();
        }

        /// <summary>
        /// DTO更新数据
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <returns></returns>
        public int DTOUpdate(CostInfoUpdateInputDTO inputEntity, out int outID)
        {
            //先查 AsNoTracking() 不追踪
            outID = 0;
            CostInfo checkmodel = costInfoDAL.LoadEntities(u => u.CostID == inputEntity.CostID).AsNoTracking().FirstOrDefault();
            if (checkmodel != null)
            {
                //AutoMap转换
                CostInfo dataModel = Mapper.Map<CostInfoUpdateInputDTO, CostInfo>(inputEntity);
                ////修改数据
                dataModel.ID = checkmodel.ID;//把ID赋值，让EF知道应该修改哪条数据
                dataModel.HousID = checkmodel.HousID;
                outID = dataModel.ID;
                return costInfoDAL.EditAndSaveChange(dataModel);

            }
            else
            {
                return 0;
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
                CostInfo entity = costInfoDAL.LoadEntities(u => u.ID == ID).FirstOrDefault();
                if (entity != null)
                {
                    //打标记
                    costInfoDAL.DeleteFlag(entity);
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
        /// 基于业主的分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<CostInfoOutputDTO> User_GetPageList(int pageIndex, int pageSize, out int count, string HousID, string State, DateTime DRsubmitDate, DateTime DRsolveDate)
        {
            DateTime Time = new DateTime();//默认时间
            IQueryable<CostInfo> iquery;

            if (string.IsNullOrEmpty(State) && DRsubmitDate == Time && DRsolveDate == Time)
            {
                //说明没有点击搜索按钮
                iquery = costInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.HousID==HousID, u => u.ID, true);
            }
            else if (!string.IsNullOrEmpty(State) && DRsubmitDate != Time && DRsolveDate != Time)//状态+时间
            {
                bool bState = false;
                if (State == "1")
                {
                    bState = true;
                }
                iquery = costInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.State == bState && u.HousID == HousID && u.RsubmitDate >= DRsubmitDate && u.RsolveDate <= DRsolveDate, u => u.ID, true);
            }
            else if (DRsubmitDate != Time && DRsolveDate != Time)//时间
            {
                iquery = costInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u =>u.HousID == HousID && u.RsubmitDate >= DRsubmitDate && u.RsolveDate <= DRsolveDate, u => u.ID, true);
            }
            else//状态
            {
                bool bState = false;
                if (State == "1")
                {
                    bState = true;
                }
                iquery = costInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.State == bState && u.HousID == HousID, u => u.ID, true);
            }
            #region
            //先拿到所有信息
            IQueryable<AdminInfo> AdIquery = adminInfoDAL.LoadEntities(u => u.ID > 0);//获取管理员信息表
            IQueryable<UserInfo> UsIquery = userInfoDAL.LoadEntities(u => u.ID > 0);//获取业主信息表


            //连表查询
            IQueryable<CostInfoOutputDTO> linq = (from a in iquery
                                               join b in AdIquery on a.AdminID equals b.AdminID into b_join
                                               from c in b_join.DefaultIfEmpty()
                                               join d in UsIquery on a.HousID equals d.HousID into c_join
                                               from e in c_join.DefaultIfEmpty()
                                               select new CostInfoOutputDTO
                                               {
                                                   ID = a.ID,
                                                   CostID = a.CostID,
                                                   CostName = a.CostName,
                                                   HousID = a.HousID,
                                                   RsubmitDate = a.RsubmitDate,
                                                   RsolveDate = a.RsolveDate,
                                                   WaterMeter = a.WaterMeter,
                                                   ElectricMete = a.ElectricMete,
                                                   WaterPrice = a.WaterPrice,
                                                   WirePrice = a.WirePrice,
                                                   SumMoney = a.SumMoney,
                                                   AdminID = a.AdminID,
                                                   Name = c.Name,
                                                   State = a.State,
                                                   UserName = e.Name,
                                               });
            #endregion
            return linq.ToList(); ;
            //}
        }

    }
}
