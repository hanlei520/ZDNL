using DataModel;
using EntityModel;
using EntityModel.PropertyInfoDTO;
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
    public class PropertyInfoController : BaseController
    {
        // GET: PropertyInfo
        /// <summary>
        /// 物业费主页
        /// </summary>
        /// <returns></returns>
        public ActionResult PropertyIndex()
        {
            return View();
        }
        /// <summary>
        /// 物业费添加 视图
        /// </summary>
        /// <returns></returns>
        public ActionResult PropertyAdd()
        {
            ViewBag.userName = Session["userName"].ToString();
            //查询房屋信息，并放到viewbag中
            List<HouseInfo> list = HouseInfoService.GetFalseList();
            ViewBag.Hous = list;
            return View();
        }
        /// <summary>
        /// 物业费修改 视图
        /// </summary>
        /// <returns></returns>
        public ActionResult PropertyUpdate(string PropertyID)
        {
            //根据传过来的id获取选中账号的信息,并把查到的数据赋值给viewbag，以便页面绑定数据
            PropertyInfoOutput model = PropertyInfoService.GetModelByID(PropertyID);

            if (model != null)
            {
                ViewBag.model = model;
            }
            //查询房屋信息，并放到viewbag中
            List<HouseInfo> list = HouseInfoService.GetAllHouseList();
            ViewBag.Hous = list;
            ViewBag.userName = Session["userName"].ToString();
            return View();
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <returns></returns>
        public string GetPropertyInfo()
        {
            //拿到前端传来的数据-->第几页，每页显示多少条
            int pageIndex = Convert.ToInt32(Request["page"]);
            int pageSize = Convert.ToInt32(Request["limit"]);
            string HouseID = Request["HousID"];
            string State = Request["State"];
            string MonthID = Request["MonthID"];
            //设置总条数，默认值为0
            int count = 0;


            List<PropertyInfoOutputDTO> list = PropertyInfoService.GetPageList(pageIndex, pageSize, out count,HouseID,State,MonthID);
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
        /// 添加数据
        /// </summary>
        /// <param name="PropertyInfoJson">前端返回的JSON数据</param>
        /// <returns></returns>
        public string PropertyInfoAdd(string PropertyInfoJson)
        {            
            //Json转实体类
            PropertyInfoInputDTO propertyInfo = JsonConvert.DeserializeObject<PropertyInfoInputDTO>(PropertyInfoJson);
            propertyInfo.AdminID = Session["AdminID"].ToString();
            //添加数据
            if (PropertyInfoService.DTOAdd(propertyInfo))
            {
                return "ok";
            }
            else
            {
                return "no";
            }
        }
        [HttpPost]
        public string Delete(Array ID)
        {
            //添加数据
            if (PropertyInfoService.Delete(ID))
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
        /// <param name="PropertyInfoJson">前端返回的JSON数据</param>
        /// <returns></returns>
        public string PropertyInfoUpdate(string PropertyInfoJson)
        {    
            //Json转实体类
            PropertyInfoUpdateInputDTO propertyInfo =  JsonConvert.DeserializeObject<PropertyInfoUpdateInputDTO>(PropertyInfoJson);
            string AdminID = Session["AdminID"].ToString();
            //添加数据
            if (PropertyInfoService.DTOUpdate(propertyInfo, AdminID)>0)
            {
                return "ok";
            }
            else
            {
                return "no";
            }
        }
        /// <summary>
        /// 下拉框选定绑定物业费
        /// </summary>
        /// <param name="HousID"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetPropertyCost(string HousID)
        {
            HouseInfo model = PropertyInfoService.GetPropertyCost(HousID);
            if (model!=null)
            {
                return model.PropertyCost.ToString();
            }
            return "";
        }
    }
}