using AutoMapper;
using DataModel;
using EntityModel;
using EntityModel.RepairInfoDTO;
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
    public class RepairInfoService : IRepairInfoService
    {
        RepairInfoDAL repairInfoDAL = DALFactory.RepairInfoDAL;
        UserInfoDAL userInfoDAL = DALFactory.UserInfoDAL;
        AdminInfoDAL adminInfoDAL = DALFactory.AdminInfoDAL;
        UnitOfWork unitOfWork = DALFactory.UnitOfWork;
        HouseInfoDAL houseInfoDAL = DALFactory.HouseInfoDAL;


        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="strHousID"></param>
        /// <param name="State"></param>
        /// <returns></returns>
        public List<RepairInfoOutputDTO> GetPageList(int pageIndex, int pageSize, out int count, string strHousID, string State,string RsubmitDate,string EndDate)
        {
            IQueryable<RepairInfo> iquery;

            if (string.IsNullOrEmpty(strHousID) && string.IsNullOrEmpty(State)&&string.IsNullOrEmpty(RsubmitDate)&&string.IsNullOrEmpty(EndDate))
            {
                //说明没有点击搜索按钮
                iquery = repairInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.ID > 0, u => u.ID, true);
            }
            else
            {
                int iState = 0;
                
                if (!string.IsNullOrEmpty(State) && !string.IsNullOrEmpty(strHousID) && !string.IsNullOrEmpty(RsubmitDate) && !string.IsNullOrEmpty(EndDate))//状态+房屋编号+开始时间+结束时间
                {
                    iState = Convert.ToInt32(State);
                    DateTime DRsubmitDate = Convert.ToDateTime(RsubmitDate);
                    DateTime DEndDate = Convert.ToDateTime(EndDate).AddDays(1).AddSeconds(-1);
                    iquery = repairInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.State == iState && u.HousID.Contains(strHousID) && u.RsubmitData >= DRsubmitDate && u.RsubmitData <= DEndDate, u => u.ID, true);
                }
                else if (!string.IsNullOrEmpty(State) && !string.IsNullOrEmpty(strHousID))//状态+房屋编号
                {
                    iState = Convert.ToInt32(State);
                    iquery = repairInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.State == iState && u.HousID.Contains(strHousID), u => u.ID, true);
                }
                else if (!string.IsNullOrEmpty(State) && !string.IsNullOrEmpty(RsubmitDate))//状态+月份
                {
                    DateTime DRsubmitDate = Convert.ToDateTime(RsubmitDate);
                    DateTime DEndDate = Convert.ToDateTime(EndDate).AddDays(1).AddSeconds(-1);
                    iquery = repairInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.State == iState && u.RsubmitData >= DRsubmitDate && u.RsubmitData <= DEndDate, u => u.ID, true);
                }
                else if (!string.IsNullOrEmpty(strHousID) && !string.IsNullOrEmpty(RsubmitDate))//房屋编号+月份
                {
                    DateTime DRsubmitDate = Convert.ToDateTime(RsubmitDate);
                    DateTime DEndDate = Convert.ToDateTime(EndDate).AddDays(1).AddSeconds(-1);
                    iquery = repairInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.HousID.Contains(strHousID) && u.RsubmitData >= DRsubmitDate && u.RsubmitData <= DEndDate, u => u.ID, true);
                }
                else if (!string.IsNullOrEmpty(RsubmitDate))//月份
                {
                    DateTime DRsubmitDate = Convert.ToDateTime(RsubmitDate);
                    DateTime DEndDate = Convert.ToDateTime(EndDate).AddDays(1).AddSeconds(-1);
                    ////iquery = costInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.RsubmitDate.ToString().Contains(RsubmitDate), u => u.ID, true);
                    iquery = repairInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.RsubmitData >= DRsubmitDate && u.RsubmitData <= DEndDate, u => u.ID, true);
                }
              else  if (!string.IsNullOrEmpty(State))//状态
                {
                    iState = Convert.ToInt32(State);
                    iquery = repairInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.State == iState && u.HousID.Contains(strHousID), u => u.ID, true);
                }
                else//房屋编号
                {
                    iquery = repairInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.HousID.Contains(strHousID), u => u.ID, true);
                }

            }
            #region   连表  
            //先拿到所有信息
            IQueryable<UserInfo> userIquery = userInfoDAL.LoadEntities(u => u.ID > 0);//业主信息表
            IQueryable<AdminInfo> adminIquery = adminInfoDAL.LoadEntities(u => u.ID > 0);//业主信息表
            //连表查询
            IQueryable<RepairInfoOutputDTO> linq = (from a in iquery
                                                 join b in userIquery on a.UserID equals b.UserID into b_join
                                                 from c in b_join.DefaultIfEmpty()
                                                 join d in adminIquery on a.AdminID equals d.AdminID into c_join
                                                 from g in c_join.DefaultIfEmpty()
                                                 select new RepairInfoOutputDTO
                                                 {
                                                     ID = a.ID,
                                                     RepairID = a.RepairID,
                                                     HousID = a.HousID,
                                                     Rreason = a.Rreason,
                                                     UserName = c.Name,
                                                     Phone = a.Phone,
                                                     RsubmitData = a.RsubmitData,
                                                     RsolveData = a.RsolveData,
                                                     State = a.State,
                                                     Remarks = a.Remarks,
                                                     AdminName = g.Name,
                                                 });

            //分组     
            var grplinq = (from p in linq
                           group p by new
                           {
                               p.ID,
                               p.RepairID,
                               p.Rreason,
                               p.HousID,
                               p.UserName,
                               p.Phone,
                               p.RsubmitData,
                               p.RsolveData,
                               p.State,
                               p.Remarks,
                               p.AdminName,

                           }).AsEnumerable().Select(grp =>
                         new RepairInfoOutputDTO
                         {
                             ID = grp.Key.ID,
                             RepairID = grp.Key.RepairID,
                             Rreason = grp.Key.Rreason,
                             UserName = grp.Key.UserName,
                             Phone = grp.Key.Phone,
                             RsubmitData = grp.Key.RsubmitData,
                             RsolveData = grp.Key.RsolveData,
                             State = grp.Key.State,
                             Remarks = grp.Key.Remarks,
                             AdminName = grp.Key.AdminName,
                             HousID = grp.Key.HousID,
                         }).OrderBy(u => u.RepairID).ToList();
            #endregion  
            return grplinq.ToList();
        }
        /// <summary>
        /// 根据报修单号查询
        /// </summary>
        /// <param name="RepairID"></param>
        /// <returns></returns>
        public RepairInfoOutput GetModelByID(string RepairID)
        {
            //先拿到所有信息
            IQueryable<UserInfo> userIquery = userInfoDAL.LoadEntities(u => u.ID > 0);//业主信息表
            IQueryable<AdminInfo> adminIquery = adminInfoDAL.LoadEntities(u => u.ID > 0);//管理员信息表
            IQueryable<RepairInfo> iquery = repairInfoDAL.LoadEntities(u => u.RepairID == RepairID);
            //连表查询
            IQueryable<RepairInfoOutput> linq = (from a in iquery
                                                 join b in userIquery on a.UserID equals b.UserID into b_join
                                                 from c in b_join.DefaultIfEmpty()
                                                 join d in adminIquery on a.AdminID equals d.AdminID into c_join
                                                 from g in c_join.DefaultIfEmpty()
                                                 select new RepairInfoOutput
                                                 {
                                                     ID = a.ID,
                                                     RepairID = a.RepairID,
                                                     HousID = a.HousID,
                                                     UserName = c.Name,
                                                     Phone = a.Phone,
                                                     Rreason = a.Rreason,
                                                     RsubmitData = a.RsubmitData,
                                                     RsolveData = a.RsolveData,
                                                     State = a.State,
                                                     Remarks = a.Remarks,
                                                     AdminName = g.Name,
                                                 });

            return linq.FirstOrDefault();
        }
        /// <summary>
        /// 修改报修信息
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <param name="AdminID"></param>
        /// <returns></returns>
        public int DTOUpdate(RepairInfoUpdateInputDTO inputEntity, string AdminID)
        {
            //先查
            RepairInfo model = repairInfoDAL.LoadEntities(u => u.RepairID == inputEntity.RepairID).FirstOrDefault();
            if (model != null)
            {

                //修改数据
                model.RsolveData = inputEntity.RsolveData;
                model.State = inputEntity.State;
                model.AdminID = AdminID;
                model.Remarks = inputEntity.Remarks;

                return repairInfoDAL.EditAndSaveChange(model);

            }
            else
            {
                return 0;
            }

        }



        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <returns></returns>
        public bool DTOAdd(RepairInfoInputDTO inputEntity)
        {
            //AutoMapper转换
            RepairInfo dataModel = Mapper.Map<RepairInfoInputDTO, RepairInfo>(inputEntity);
            HouseInfo list = houseInfoDAL.LoadEntities(u=>u.HousID==inputEntity.HousID).FirstOrDefault();
            dataModel.RepairID = CreateOrderCode();
            dataModel.State = 2;
            dataModel.RsubmitData = DateTime.Now;
            dataModel.UserID = list.UserID;
            if (repairInfoDAL.AddAndSaveChange(dataModel) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 删除方法
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(Array id)
        {

            foreach (var item in id)
            {
                int ID = Convert.ToInt32(item);
                //先查
                RepairInfo entity = repairInfoDAL.LoadEntities(u => u.ID == ID).FirstOrDefault();
                if (entity != null)
                {
                    //打标记
                    repairInfoDAL.DeleteFlag(entity);
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
        /// 订单生成方法
        /// </summary>
        /// <returns></returns>
        public string CreateOrderCode()
        {
            //订单编号生成方法     
            string ordercode = "BX" + System.DateTime.Now.Date.ToShortDateString();//OD2019/12/10
            ordercode = ordercode.Replace("/", "");//OD20191210
            List<RepairInfo> list = repairInfoDAL.LoadEntities(u => u.RepairID.Contains(ordercode)).ToList();
            int num = Convert.ToInt16(list.Count) + 1;//如果查出0条，加1
            ordercode += (num.ToString()).PadLeft(4, '0');//OD20191210001        PadLeft往左补零

            return ordercode;
        }

        /// <summary>
        /// 基于业主的查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<RepairInfoOutput> GetPageList(int pageIndex, int pageSize, out int count, string UserID, int State,DateTime DRsubmitDate, DateTime DEndDate)
        {
            DateTime Time = new DateTime();
            IQueryable<RepairInfo> iquery;

            if (State==-1 && DRsubmitDate == Time && DEndDate == Time)
            {
                //说明没有点击搜索按钮
                iquery = repairInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.UserID == UserID, u => u.ID, true);
            }
            else
            {
                if (State != -1 && DRsubmitDate!= Time && DEndDate != Time)//状态+时间
                {
                    iquery = repairInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.UserID == UserID&&u.State== State && u.RsubmitData >= DRsubmitDate && u.RsubmitData <= DEndDate, u => u.ID, true);
                }
                else if (DRsubmitDate != Time && DEndDate != Time)//时间
                {
                    iquery = repairInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.UserID == UserID && u.RsubmitData >= DRsubmitDate && u.RsubmitData <= DEndDate, u => u.ID, true);
                }
                else//状态
                {
                    iquery = repairInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.UserID == UserID && u.State == State, u => u.ID, true);
                }
            }
            #region 连表

           

            //先拿到所有信息
            IQueryable<UserInfo> userIquery = userInfoDAL.LoadEntities(u => u.ID > 0);//业主信息表
            IQueryable<AdminInfo> adminIquery = adminInfoDAL.LoadEntities(u => u.ID > 0);//业主信息表
                                                                                         //连表查询
            IQueryable<RepairInfoOutput> linq = (from a in iquery
                                                 join b in userIquery on a.UserID equals b.UserID into b_join
                                                 from c in b_join.DefaultIfEmpty()
                                                 join d in adminIquery on a.AdminID equals d.AdminID into c_join
                                                 from g in c_join.DefaultIfEmpty()
                                                 select new RepairInfoOutput
                                                 {
                                                     ID = a.ID,
                                                     RepairID = a.RepairID,
                                                     HousID = a.HousID,
                                                     Rreason = a.Rreason,
                                                     UserName = c.Name,
                                                     Phone = a.Phone,
                                                     RsubmitData = a.RsubmitData,
                                                     RsolveData = a.RsolveData,
                                                     State = a.State,
                                                     Remarks = a.Remarks,
                                                     AdminName = g.Name,
                                                 });

            //分组     
            var grplinq = (from p in linq
                           group p by new
                           {
                               p.ID,
                               p.RepairID,
                               p.Rreason,
                               p.HousID,
                               p.UserName,
                               p.Phone,
                               p.RsubmitData,
                               p.RsolveData,
                               p.State,
                               p.Remarks,
                               p.AdminName,

                           }).AsEnumerable().Select(grp =>
                         new RepairInfoOutput
                         {
                             ID = grp.Key.ID,
                             RepairID = grp.Key.RepairID,
                             Rreason = grp.Key.Rreason,
                             UserName = grp.Key.UserName,
                             Phone = grp.Key.Phone,
                             RsubmitData = grp.Key.RsubmitData,
                             RsolveData = grp.Key.RsolveData,
                             State = grp.Key.State,
                             Remarks = grp.Key.Remarks,
                             AdminName = grp.Key.AdminName,
                             HousID = grp.Key.HousID,
                         }).OrderBy(u => u.RepairID).ToList();
            #endregion
            return grplinq.ToList();
        }
        /// <summary>
        /// 基于业主的修改
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <returns></returns>
        public bool User_Update(RepairInfo inputEntity)
        {
            RepairInfo model = repairInfoDAL.LoadEntities(u => u.RepairID == inputEntity.RepairID).FirstOrDefault();
            if (model != null)
            {
                //修改数据
                model.Phone = inputEntity.Phone;
                model.Rreason = inputEntity.Rreason;
                model.Remarks = inputEntity.Remarks;
                model.RsubmitData = DateTime.Now;

                if (repairInfoDAL.EditAndSaveChange(model) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 业主根据ID取消报修
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool Cancel(RepairInfo inputEntity, int ID)
        {
            RepairInfo model = repairInfoDAL.LoadEntities(u => u.ID == ID).FirstOrDefault();
            if (model != null)
            {
                //修改数据
                model.State = inputEntity.State;

                if (repairInfoDAL.EditAndSaveChange(model) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 查询所有报修订单
        /// </summary>
        /// <returns></returns>
        public List<RepairInfo> GetAllRepairInfo()
        {
            List<RepairInfo> repairInfos = repairInfoDAL.LoadEntities(u => u.ID > 0).ToList();
            return repairInfos;
        }
    }
}
