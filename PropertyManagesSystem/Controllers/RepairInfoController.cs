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
    [MyActionFilter]
    public class RepairInfoController : BaseController
    {
        // GET: RepairInfo
        /// <summary>
        /// 报修主页
        /// </summary>
        /// <returns></returns>
        public ActionResult RepairIndex()
        {
            return View();
        }
        /// <summary>
        /// 报修-->添加视图
        /// </summary>
        /// <returns></returns>
        public ActionResult RepairAdd()
        {
            ViewBag.userName = Session["userName"].ToString();
            //查询房屋信息，并放到viewbag中
            List<HouseInfo> list = HouseInfoService.GetFalseList();
            ViewBag.Hous = list;
            return View();
        }
        /// <summary>
        /// 修改视图
        /// </summary>
        /// <param name="RepairID"></param>
        /// <returns></returns>
        public ActionResult RepairUpdate(string RepairID)
        {
            //根据传过来的id获取选中账号的信息,并把查到的数据赋值给viewbag，以便页面绑定数据
            RepairInfoOutput model = RepairInfoService.GetModelByID(RepairID);
            ViewBag.RsubmitData = model.RsubmitData.ToString("yyyy'-'MM'-'dd");
            //ViewBag.RsolveData = model.RsolveData.ToString();
            if (model != null)
            {
                ViewBag.model = model;
                DateTime? dateTime = model.RsolveData;
                ViewBag.RsolveData = dateTime == null ? "" : dateTime.Value.ToString("yyyy'-'MM'-'dd");
            }
            ViewBag.userName = Session["userName"].ToString();

            return View();
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <returns></returns>
        public string GetRepairInfo()
        {
            //拿到前端传来的数据-->第几页，每页显示多少条
            int pageIndex = Convert.ToInt32(Request["page"]);
            int pageSize = Convert.ToInt32(Request["limit"]);
            string RsubmitDate = Request["RsubmitDate"];
            string EndDate = Request["EndDate"];
            string HousID = Request["HousID"];
            string State = Request["State"];
            //设置总条数，默认值为0
            int count = 0;


            List<RepairInfoOutputDTO> list = RepairInfoService.GetPageList(pageIndex, pageSize, out count,HousID,State, RsubmitDate, EndDate);
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
        /// 数据更新
        /// </summary>
        /// <param name="RepairInfoJson">前端返回的JSON数据</param>
        /// <returns></returns>
        [HttpPost]
        public string RepairInfoUpdate(string RepairInfoJson)
        {       
            //Json转实体类
            RepairInfoUpdateInputDTO repairInfo = JsonConvert.DeserializeObject<RepairInfoUpdateInputDTO>(RepairInfoJson);
            string AdminID = Session["AdminID"].ToString();
            //添加数据
            if (RepairInfoService.DTOUpdate(repairInfo, AdminID)>0)
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
        /// <param name="RepairInfoJson">前端返回的JSON数据</param>
        /// <returns></returns>
        public string RepairInfoAdd(string RepairInfoJson)
        {    
            //Json转实体类
            RepairInfoInputDTO repairInfo = JsonConvert.DeserializeObject<RepairInfoInputDTO>(RepairInfoJson);
            repairInfo.AdminID = Session["AdminID"].ToString();
            //添加数据
            if (RepairInfoService.DTOAdd(repairInfo))
            {
                return "ok";
            }
            else
            {
                return "no";
            }
        }
        /// <summary>
        /// 根据ID删除报修信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public string Delete(Array id)
        {
            if (RepairInfoService.Delete(id))
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