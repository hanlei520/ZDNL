using DataModel;
using EntityModel;
using EntityModel.ComplaInfoDTO;
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
    public class User_ComplaInfoController : BaseController
    {
        // GET: User_ComplaInfo
        public ActionResult User_ComplaInfoIndex()
        {
            return View();
        }

        /// <summary>
        /// 投诉建议视图
        /// </summary>
        /// <returns></returns>
        public ActionResult User_ComplaAdd()
        {
            return View();
        }
        /// <summary>
        /// 修改视图
        /// </summary>
        /// <param name="ComplaID"></param>
        /// <returns></returns>
        public ActionResult User_ComplaUpdate(string ComplaID)
        {
            //根据传过来的id获取选中账号的信息,并把查到的数据赋值给viewbag，以便页面绑定数据
            ComplaInfoOutput model = ComplaInfoService.GetModelByID(ComplaID);

            if (model != null)
            {
                ViewBag.model = model;
            }
            ViewBag.userName = Session["userName"].ToString();
            return View();
        }
        public string UserGetComplaInfo()
        {
            //拿到前端传来的数据-->第几页，每页显示多少条
            int pageIndex = Convert.ToInt32(Request["page"]);
            int pageSize = Convert.ToInt32(Request["limit"]);
            string State = Request["State"];
            string CompType = Request["CompType"];
            string UserID = Session["UserID"].ToString();
            string RsubmitDate = Request["RsubmitDate"];
            string EndDate = Request["EndDate"];
            DateTime DRsubmitDate = new DateTime();
            DateTime DEndDate = new DateTime();
            int Istate = -1;
            if (!string.IsNullOrEmpty(RsubmitDate)&&!string.IsNullOrEmpty(EndDate))
            {
                 DRsubmitDate = Convert.ToDateTime(RsubmitDate);
                 DEndDate = Convert.ToDateTime(EndDate).AddDays(1).AddSeconds(-1);
            }
            if (!string.IsNullOrEmpty(State))
            {
                Istate= Convert.ToInt32(State);
            }
            //设置总条数，默认值为0
            int count = 0;

            List<ComplaInfoOutputDTO> list = ComplaInfoService.UserGetPageList(pageIndex, pageSize, out count, UserID,CompType, Istate, DRsubmitDate, DEndDate);
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
        /// <param name="User_ComplaInfoJson">前端返回的JSON数据</param>
        /// <returns></returns>
        public string User_ComplaInfoUpdate(string User_ComplaInfoJson)
        {
            ComplaInfo complaInfo = JsonConvert.DeserializeObject<ComplaInfo>(User_ComplaInfoJson);
            //添加数据
            if (ComplaInfoService.User_Update(complaInfo))
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
        /// <param name="User_ComplaInfoJson">前端返回的JSON数据</param>
        /// <returns></returns>
        public string User_ComplaInfoAdd(string User_ComplaInfoJson)
        {
            ComplaInfoInputDTO complaInfoInput = JsonConvert.DeserializeObject<ComplaInfoInputDTO>(User_ComplaInfoJson );
            string HousID = Session["HousID"].ToString();
            HouseInfo list = HouseInfoService.GetList(HousID);
            complaInfoInput.HousID = list.HousID;
            //添加数据
            if (ComplaInfoService.DTOAdd(complaInfoInput))
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
        public string Cancel(int ID)
        {
            if (ComplaInfoService.Cancel( ID))
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