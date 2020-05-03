using DataModel;
using EntityModel;
using EntityModel.ComplaInfoDTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PropertyManagesSystem.Filters;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PropertyManagesSystem.Controllers
{
    [MyActionFilter]
    public class ComplaInfoController : BaseController
    {
        // GET: ComplaInfo

            /// <summary>
            /// 投诉主页
            /// </summary>
            /// <returns></returns>
        public ActionResult ComplaIndex()
        {
            return View();
        }
        /// <summary>
        /// 添加视图
        /// </summary>
        /// <returns></returns>
        public ActionResult ComplaAdd()
        {
            ViewBag.userName = Session["userName"].ToString();
            //查询房屋信息，并放到viewbag中
            List<HouseInfo> list = HouseInfoService.GetFalseList();
            ViewBag.Hous = list;
            return View();
        }

        /// <summary>
        /// 修改页面
        /// </summary>
        /// <param name="ComplaID"></param>
        /// <returns></returns>
        public ActionResult ComplaUpdate(string ComplaID)
        {
            //根据传过来的id获取选中账号的信息,并把查到的数据赋值给viewbag，以便页面绑定数据
            ComplaInfoOutput model = ComplaInfoService.GetModelByID(ComplaID);
            if (model != null)
            {
                ViewBag.model = model; 
                DateTime? dateTime = model.RsolveData;
                ViewBag.RsolveData = dateTime == null ? "": dateTime.Value.ToString("yyyy'-'MM'-'dd");
                ViewBag.RsubmitData = model.RsubmitData.ToString("yyyy'-'MM'-'dd");
            }
            ViewBag.userName = Session["userName"].ToString();
            return View();
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <returns></returns>
        public string GetComplaInfo()
        {
            //拿到前端传来的数据-->第几页，每页显示多少条
            int pageIndex = Convert.ToInt32(Request["page"]);
            int pageSize = Convert.ToInt32(Request["limit"]);
            string RsubmitDate = Request["RsubmitDate"];
            string EndDate = Request["EndDate"];
            string HousID = Request["HousID"];
            string State = Request["State"];
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


            List<ComplaInfoOutputDTO> list = ComplaInfoService.GetPageList(pageIndex, pageSize, out count,HousID,Istate,DRsubmitDate,DEndDate);
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
        /// <param name="ComplaInfoJson"></param>
        /// <returns></returns>
        public string ComplaInfoUpdate(string ComplaInfoJson)
        {
            //JSON转实体类
            ComplaInfoUpdateInputDTO complaInfo = JsonConvert.DeserializeObject<ComplaInfoUpdateInputDTO>(ComplaInfoJson);
            string Admin = Session["AdminID"].ToString();
            if (ComplaInfoService.DTOUpdate(complaInfo, Admin)>0)
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
        /// <param name="ComplaInfoJson"></param>
        /// <returns></returns>
        public string ComplaInfoAdd(string ComplaInfoJson)
        {

            //Json转实体类
            ComplaInfoInputDTO complaInfo =JsonConvert.DeserializeObject<ComplaInfoInputDTO>(ComplaInfoJson);
            complaInfo.AdminID = Session["AdminID"].ToString();
            //添加数据
            if (ComplaInfoService.DTOAdd(complaInfo))
            {
                return "ok";
            }
            else
            {
                return "no";
            }
        }

        /// <summary>
        /// 根据ID删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public string Delete(Array id)
        {
            if (ComplaInfoService.Delete(id))
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