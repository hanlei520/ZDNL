using DataModel;
using EntityModel;
using EntityModel.RepairInfoDTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PropertyManagesSystem.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PropertyManagesSystem.Controllers
{
    [MyUserActionFilter]
    public class User_RepairInfoController : BaseController
    {
        // GET: User_RepairInfo
        /// <summary>
        /// 业主报修信息主页
        /// </summary>
        /// <returns></returns>
        public ActionResult User_RepairIndex()
        {
            return View();
        }
        /// <summary>
        /// 报修-->添加视图
        /// </summary>
        /// <returns></returns>
        public ActionResult User_RepairAdd()
        {
            return View();
        }
        /// <summary>
        /// 业主修改报修试图
        /// </summary>
        /// <param name="RepairID"></param>
        /// <returns></returns>
        public ActionResult User_RepairUpdate(string RepairID)
        {
            //根据传过来的id获取选中账号的信息,并把查到的数据赋值给viewbag，以便页面绑定数据
            RepairInfoOutput model = RepairInfoService.GetModelByID(RepairID);

            if (model != null)
            {
                ViewBag.model = model;
            }

            return View();
        }
        /// <summary>
        /// 基于业主的分页查询
        /// </summary>
        /// <returns></returns>
        public string GetRepairInfo()
        {
            //拿到前端传来的数据-->第几页，每页显示多少条
            int pageIndex = Convert.ToInt32(Request["page"]);
            int pageSize = Convert.ToInt32(Request["limit"]);
            string State = Request["State"];
            string UserID = Session["UserID"].ToString();
            string RsubmitDate = Request["RsubmitDate"];
            string EndDate = Request["EndDate"];
            DateTime DRsubmitDate = new DateTime();
            DateTime DEndDate = new DateTime();
            int Istate = -1;
            if (!string.IsNullOrEmpty(RsubmitDate) && !string.IsNullOrEmpty(EndDate))
            {
                DRsubmitDate = Convert.ToDateTime(RsubmitDate);
                DEndDate = Convert.ToDateTime(EndDate).AddDays(1).AddSeconds(-1);
            }
            if (!string.IsNullOrEmpty(State))
            {
                Istate = Convert.ToInt32(State);
            }
            //设置总条数，默认值为0
            int count = 0;


            List<RepairInfoOutput> list = RepairInfoService.GetPageList(pageIndex, pageSize, out count, UserID,Istate,DRsubmitDate,DEndDate);
            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy'-'MM'-'dd"//格式化时间，默认是ISO8601格式
            };
            string strjson = JsonConvert.SerializeObject(list, Formatting.Indented, timeConverter);

            //字符串拼接，构造与layui规则相同的json对象数组字符串
            string str = "{\"code\": 0 ,\"msg\": \"\",\"count\": " + count + " ,\"data\":";
            str += strjson + "}";
            return str;
        }
        /// <summary>
        /// 基于业主的数据更新
        /// </summary>
        /// <param name="User_RepairInfoJson">前端返回的JSON数据</param>
        /// <returns></returns>
        public string User_RepairInfoUpdate(String User_RepairInfoJson)
        {
            RepairInfo repairInfo = JsonConvert.DeserializeObject<RepairInfo>( User_RepairInfoJson );
            //添加数据
            if (RepairInfoService.User_Update(repairInfo))
            {
                return "ok";
            }
            else
            {
                return "no";
            }
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="User_RepairInfoJson">前端返回的JSON数据</param>
        /// <returns></returns>
        public string User_RepairInfoAdd(string User_RepairInfoJson)
        {
            RepairInfoInputDTO  repairInfoInput= JsonConvert.DeserializeObject<RepairInfoInputDTO>(User_RepairInfoJson);
            string HousID = Session["HousID"].ToString();
            repairInfoInput.HousID = HousID;
            //添加数据
            if (RepairInfoService.DTOAdd(repairInfoInput))
            {
                return "ok";
            }
            else
            {
                return "no";
            }
        }
        /// <summary>
        /// 根据ID取消报修信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public string Cancel(RepairInfo model, int ID)
        {
            if (RepairInfoService.Cancel(model, ID))
            {
                return "ok";
            }
            else
            {
                return "on";
            }
        }
    }
}