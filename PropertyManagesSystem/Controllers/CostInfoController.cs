using DataModel;
using EntityModel;
using EntityModel.CostInfoDTO;
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
    public class CostInfoController : BaseController
    {
        // GET: CostInfo
        /// <summary>
        /// 水电费主页
        /// </summary>
        /// <returns></returns>
        public ActionResult CostIndex()
        {
            return View();
        }
        /// <summary>
        /// 水电费添加页面
        /// </summary>
        /// <returns></returns>
        public ActionResult CostAdd()
        {
            List<HouseInfo> houseInfos = HouseInfoService.GetFalseList();
            ViewBag.houseInfo = houseInfos;
            return View();
        }
        /// <summary>
        /// 水电费修改页面
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult CostUpdate(int ID)
        {
            CostInfo costInfo = CostInfoService.GetCostInfoByID(ID);
            ViewBag.RsolveDate = costInfo.RsolveDate.ToString("yyyy'-'MM'-'dd");
            ViewBag.RsubmitDate = costInfo.RsubmitDate.ToString("yyyy'-'MM'-'dd");

            ViewBag.costInfo = costInfo;

            return View();
        }
        /// <summary>
        /// 水电费分页查询
        /// </summary>
        /// <returns></returns>
        public string GetCostInfo()
        {
            int pageIndex = Convert.ToInt32(Request["page"]);
            int pageSize = Convert.ToInt32(Request["limit"]);
            //获取文本框的值:有可能为null  ，有可能为"" ，有可能为其他字符串
            string selectInfo = Request["selectInfo"];
            string State = Request["State"];
            string RsubmitDate = Request["RsubmitDate"];
            string RsolveDate = Request["RsolveDate"];
            DateTime DRsubmitDate = new DateTime();
            DateTime DRsolveDate = new DateTime();
            if (!string.IsNullOrEmpty(RsubmitDate) && !string.IsNullOrEmpty(RsolveDate))
            {
                DRsubmitDate = Convert.ToDateTime(RsubmitDate);
                DRsolveDate = Convert.ToDateTime(RsolveDate).AddDays(1).AddSeconds(-1);
            }
            //设置总条数，默认值为0
            int count = 0;

            //使用json.net序列化数据
            List<CostInfoOutputDTO> list = CostInfoService.GetPageList(pageIndex,pageSize,out count,State,selectInfo, DRsubmitDate, DRsolveDate);
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
        /// 添加水电费信息
        /// </summary>
        /// <param name="CostInfoJson">前台返回的Json数据</param>
        /// <returns></returns>
        public string CostInfoAdd(string CostInfoJson)
        {          
            //Json转实体类
            CostInfoInputDTO complaInfo = JsonConvert.DeserializeObject<CostInfoInputDTO>(CostInfoJson);
            complaInfo.AdminID = Session["AdminID"].ToString();
            if (CostInfoService.DTOAdd(complaInfo))
            {
                return "ok";
            }
            else
            {
                return "no";
            }
        }


        /// <summary>
        /// 数据更新
        /// </summary>
        /// <param name="CostInfoJson">前台返回的Json数据</param>
        /// <returns></returns>
        public string CostInfoUpdate(string CostInfoJson)
        {
            int outID;
            //Json转实体类
            CostInfoUpdateInputDTO complaInfo =JsonConvert.DeserializeObject<CostInfoUpdateInputDTO>(CostInfoJson);
            complaInfo.AdminID = Session["AdminID"].ToString();
            if (CostInfoService.DTOUpdate(complaInfo, out outID)>0)
            {
                return "ok";
            }
            else
            {
                return "no";
            }
        }

        /// <summary>
        /// 根据id删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public string Delete(Array id)
        {

            if (CostInfoService.Delete(id))
            {
                return "ok";
            }
            else
            {
                return "no";
            }
        }
    }
}