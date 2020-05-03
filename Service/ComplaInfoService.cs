using AutoMapper;
using DataModel;
using EntityModel;
using EntityModel.ComplaInfoDTO;
using IService;
using Repository;
using Repository.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ComplaInfoService: IComplaInfoService
    {
        ComplaInfoDAL complaInfoDAL = DALFactory.ComplaInfoDAL;
        UserInfoDAL userInfoDAL = DALFactory.UserInfoDAL;
        AdminInfoDAL adminInfoDAL = DALFactory.AdminInfoDAL;
        HouseInfoDAL houseInfoDAL = DALFactory.HouseInfoDAL;
        UnitOfWork unitOfWork = DALFactory.UnitOfWork;

        /// <summary>
        /// DTO分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="strHousID"></param>
        /// <param name="State"></param>
        /// <returns></returns>
        public List<ComplaInfoOutputDTO> GetPageList(int pageIndex, int pageSize, out int count,string strHousID, int IState, DateTime DRsubmitDate, DateTime DEndDate)
        {
            DateTime Time = new DateTime();//默认时间

            IQueryable<ComplaInfo> iquery;

            if (string.IsNullOrEmpty(strHousID) && IState == -1 && DRsubmitDate==Time && DEndDate==Time)
            {
                //说明没有点击搜索按钮
                iquery = complaInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.ID > 0, u => u.ID, true);
            }
            else
            {
                if (!string.IsNullOrEmpty(strHousID) && IState != -1 && DRsubmitDate!=Time&& DEndDate!=Time)//状态+房屋编号+开始时间+结束时间
                {

                    iquery = complaInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.State == IState && u.HousID.Contains(strHousID) && u.RsubmitData >= DRsubmitDate && u.RsubmitData <= DEndDate, u => u.ID, true);
                }
                else if (!string.IsNullOrEmpty(strHousID) && DRsubmitDate!=Time && DEndDate!=Time)//房屋编号+时间
                {

                    iquery = complaInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.HousID.Contains(strHousID) && u.RsubmitData >= DRsubmitDate && u.RsubmitData <= DEndDate, u => u.ID, true);
                }
                else if (IState != -1 && DRsubmitDate != Time && DEndDate !=Time)//状态+时间
                {

                    iquery = complaInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.State == IState && u.RsubmitData >= DRsubmitDate && u.RsubmitData <= DEndDate, u => u.ID, true);
                }
                else if (IState != -1 && !string.IsNullOrEmpty(strHousID))//状态+房屋编号
                {
                    iquery = complaInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.State == IState && u.HousID.Contains(strHousID), u => u.ID, true);
                }
                else if (DRsubmitDate != Time && DEndDate!= Time)//月份
                {

                    iquery = complaInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.RsubmitData >= DRsubmitDate && u.RsubmitData <= DEndDate, u => u.ID, true);
                }
                else if (IState != -1)
                {
                    iquery = complaInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.State == IState && u.HousID.Contains(strHousID), u => u.ID, true);
                }
                else
                {
                    iquery = complaInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u =>u.HousID.Contains(strHousID), u => u.ID, true);
                }
                
              
            }
            #region 连表


            //先拿到所有信息
            IQueryable<UserInfo> userIquery = userInfoDAL.LoadEntities(u => u.ID > 0);//业主信息表
            IQueryable<AdminInfo> adminIquery = adminInfoDAL.LoadEntities(u => u.ID > 0);//业主信息表
            //连表查询
            IQueryable<ComplaInfoOutputDTO> linq = (from a in iquery
                                                 join b in userIquery on a.UserID equals b.UserID into b_join
                                                 from c in b_join.DefaultIfEmpty()
                                                 join d in adminIquery on a.AdminID equals d.AdminID into c_join
                                                 from g in c_join.DefaultIfEmpty()
                                                 select new ComplaInfoOutputDTO
                                                 {
                                                     ID = a.ID,
                                                     ComplaID = a.ComplaID,
                                                     CompType = a.CompType,
                                                     HousID = a.HousID,
                                                     UserName = c.Name,
                                                     Phone = a.Phone,
                                                     Contents = a.Contents,
                                                     RsubmitData = a.RsubmitData,
                                                     RsolveData = a.RsolveData,
                                                     State = a.State,
                                                     Remarks = a.Remarks,
                                                     AdminName = g.Name,
                                                 });

            #endregion
            return linq.ToList();
        }
        /// <summary>
        /// 根据ID查询数据
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ComplaInfoOutput GetModelByID(string ComplaID)
        {
            //先拿到所有信息
            IQueryable<UserInfo> userIquery = userInfoDAL.LoadEntities(u => u.ID > 0);//业主信息表
            IQueryable<AdminInfo> adminIquery = adminInfoDAL.LoadEntities(u => u.ID > 0);//业主信息表
            IQueryable<ComplaInfo> iquery = complaInfoDAL.LoadEntities(u => u.ComplaID == ComplaID);
            //连表查询
            IQueryable<ComplaInfoOutput> linq = (from a in iquery
                                                 join b in userIquery on a.UserID equals b.UserID into b_join
                                                 from c in b_join.DefaultIfEmpty()
                                                 join d in adminIquery on a.AdminID equals d.AdminID into c_join
                                                 from g in c_join.DefaultIfEmpty()
                                                 select new ComplaInfoOutput
                                                 {
                                                     ID = a.ID,
                                                     ComplaID = a.ComplaID,
                                                     CompType = a.CompType,
                                                     HousID = a.HousID,
                                                     UserName = c.Name,
                                                     Phone = a.Phone,
                                                     Contents = a.Contents,
                                                     RsubmitData = a.RsubmitData,
                                                     RsolveData = a.RsolveData,
                                                     State = a.State,
                                                     Remarks = a.Remarks,
                                                     AdminName = g.Name,
                                                 });

            return linq.FirstOrDefault();
        }
        /// <summary>
        /// DTO修改方法
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <param name="AdminID"></param>
        /// <returns></returns>
        public int DTOUpdate(ComplaInfoUpdateInputDTO inputEntity, string AdminID)
        {
            //先查AsNoTracking() 不追踪
            ComplaInfo checkmodel = complaInfoDAL.LoadEntities(u => u.ComplaID == inputEntity.ComplaID).FirstOrDefault();
            if (checkmodel != null)
            {
                checkmodel.RsolveData = inputEntity.RsolveData;
                checkmodel.State = inputEntity.State;
                checkmodel.AdminID = AdminID;
                checkmodel.Remarks = inputEntity.Remarks;

                return complaInfoDAL.EditAndSaveChange(checkmodel);
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(Array id)
        {
            foreach (var item in id)
            {
                int ID = Convert.ToInt32(item);
                //先查
                ComplaInfo entity = complaInfoDAL.LoadEntities(u => u.ID == ID).FirstOrDefault();
                if (entity != null)
                {
                    //打标记
                    complaInfoDAL.DeleteFlag(entity);
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
            string ordercode = "TS" + System.DateTime.Now.Date.ToShortDateString();//OD2019/12/10
            ordercode = ordercode.Replace("/", "");//OD20191210
            List<ComplaInfo> list = complaInfoDAL.LoadEntities(u => u.ComplaID.Contains(ordercode)).ToList();
            int num = Convert.ToInt16(list.Count) + 1;//如果查出0条，加1
            ordercode += (num.ToString()).PadLeft(4, '0');//OD20191210001        PadLeft往左补零

            return ordercode;
        }
        /// <summary>
        /// DTO添加数据
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <returns></returns>
        public bool DTOAdd(ComplaInfoInputDTO inputEntity)
        {
            //AutoMapper转换
            ComplaInfo dataModel = Mapper.Map<ComplaInfoInputDTO, ComplaInfo>(inputEntity);

            HouseInfo list = houseInfoDAL.LoadEntities(u=>u.HousID==dataModel.HousID).FirstOrDefault();
            dataModel.ComplaID = CreateOrderCode();
            dataModel.State = 2;
            dataModel.RsubmitData = DateTime.Now;
            dataModel.UserID = list.UserID;
            //ComplaInfo entity = complaInfoDAL.LoadEntities(u => u.HousID == HousID).FirstOrDefault();
            if (complaInfoDAL.AddAndSaveChange(dataModel) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 基于业主的查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<ComplaInfoOutputDTO> UserGetPageList(int pageIndex, int pageSize, out int count, string UserID,string CompType,int IState, DateTime DRsubmitDate, DateTime DEndDate)
        {
            DateTime Time = new DateTime();
            IQueryable<ComplaInfo> iquery;
            if (string.IsNullOrEmpty(CompType)&& IState == -1 && DRsubmitDate == Time && DEndDate == Time)
            {
                //说明没有点击搜索按钮
                iquery = complaInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.UserID == UserID, u => u.ID, true);
            }
            else
            {
                if (!string.IsNullOrEmpty(CompType) && IState!=-1&& DRsubmitDate!= Time && DEndDate != Time)//类别+状态+开始时间+结束时间
                {

                    iquery = complaInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.UserID == UserID && u.CompType == CompType && u.State == IState && u.RsubmitData >= DRsubmitDate && u.RsubmitData <= DEndDate, u => u.ID, true);
                }
                else if (!string.IsNullOrEmpty(CompType)&& DRsubmitDate != Time && DEndDate != Time)//类别+开始时间+结束时间
                {
                    iquery = complaInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.UserID == UserID && u.CompType == CompType  && u.RsubmitData >= DRsubmitDate && u.RsubmitData <= DEndDate, u => u.ID, true);
                }
                else if (IState != -1 && DRsubmitDate != Time && DEndDate != Time)//状态+开始时间+结束时间
                {

                    iquery = complaInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.UserID == UserID  && u.State == IState && u.RsubmitData >= DRsubmitDate && u.RsubmitData <= DEndDate, u => u.ID, true);
                }
               else if (!string.IsNullOrEmpty(CompType)&& IState != -1)//类别+状态
                {

                    iquery = complaInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.UserID == UserID&&u.CompType==CompType&&u.State==IState, u => u.ID, true);
                }
                else if (DRsubmitDate != Time && DEndDate != Time)// 开始时间 + 结束时间
                {
                    iquery = complaInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.UserID == UserID  && u.RsubmitData >= DRsubmitDate && u.RsubmitData <= DEndDate, u => u.ID, true);
                }
                else if(!string.IsNullOrEmpty(CompType))//类别
                {
                    iquery = complaInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.UserID == UserID && u.CompType == CompType, u => u.ID, true);
                }
                else
                {
                    iquery = complaInfoDAL.LoadPageEntities(pageIndex, pageSize, out count, u => u.UserID == UserID &&u.State == IState, u => u.ID, true);
                }
                
            }
           


            //先拿到所有信息
            IQueryable<UserInfo> userIquery = userInfoDAL.LoadEntities(u => u.ID > 0);//业主信息表
            IQueryable<AdminInfo> adminIquery = adminInfoDAL.LoadEntities(u => u.ID > 0);//业主信息表
            //连表查询
            IQueryable<ComplaInfoOutputDTO> linq = (from a in iquery
                                                 join b in userIquery on a.UserID equals b.UserID into b_join
                                                 from c in b_join.DefaultIfEmpty()
                                                 join d in adminIquery on a.AdminID equals d.AdminID into c_join
                                                 from g in c_join.DefaultIfEmpty()
                                                 select new ComplaInfoOutputDTO
                                                 {
                                                     ID = a.ID,
                                                     ComplaID = a.ComplaID,
                                                     CompType = a.CompType,
                                                     HousID = a.HousID,
                                                     UserName = c.Name,
                                                     Phone = a.Phone,
                                                     Contents = a.Contents,
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
                               p.ComplaID,
                               p.CompType,
                               p.HousID,
                               p.UserName,
                               p.Phone,
                               p.Contents,
                               p.RsubmitData,
                               p.RsolveData,
                               p.State,
                               p.Remarks,
                               p.AdminName,

                           }).AsEnumerable().Select(grp =>
                         new ComplaInfoOutputDTO
                         {
                             ID = grp.Key.ID,
                             ComplaID = grp.Key.ComplaID,
                             CompType = grp.Key.CompType,
                             UserName = grp.Key.UserName,
                             Phone = grp.Key.Phone,
                             Contents = grp.Key.Contents,
                             RsubmitData = grp.Key.RsubmitData,
                             RsolveData = grp.Key.RsolveData,
                             State = grp.Key.State,
                             Remarks = grp.Key.Remarks,
                             AdminName = grp.Key.AdminName,
                             HousID = grp.Key.HousID,
                         }).OrderBy(u => u.CompType).ToList();

            return grplinq.ToList();
        }

        /// <summary>
        /// 修改投诉/建议方法
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <returns></returns>
        public bool User_Update(ComplaInfo inputEntity)
        {
            ComplaInfo model = complaInfoDAL.LoadEntities(u => u.ComplaID == inputEntity.ComplaID).FirstOrDefault();
            if (model != null)
            {
                //修改数据

                model.Phone = inputEntity.Phone;
                model.CompType = inputEntity.CompType;
                model.Remarks = inputEntity.Remarks;
                model.Contents = inputEntity.Contents;

                if (complaInfoDAL.EditAndSaveChange(model) > 0)
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
        /// 业主根据ID取消投诉建议
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool Cancel(int ID)
        {
            ComplaInfo model = complaInfoDAL.LoadEntities(u => u.ID == ID).FirstOrDefault();
            if (model != null)
            {
                //修改数据
                model.State = 3;

                if (complaInfoDAL.EditAndSaveChange(model) > 0)
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
        /// 查询所有投诉订单
        /// </summary>
        /// <returns></returns>
        public List<ComplaInfo> GetAllComplaInfo()
        {
          List<  ComplaInfo> complaInfos = complaInfoDAL.LoadEntities(u => u.ID>0).ToList();
            return complaInfos;
           
        }
    }

}
