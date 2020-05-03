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
    [MyUserActionFilter]
    public class User_PropertyInfoController : BaseController
    {
        // GET: User_PropertyInfo
        /// <summary>
        /// 业主物业费
        /// </summary>
        /// <returns></returns>
        public ActionResult User_PropertyIndex()
        {
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
            string State = Request["State"];
            string MonthID = Request["MonthID"];
            //设置总条数，默认值为0
            int count = 0;
            string HousID = Session["HousID"].ToString();

            List<PropertyInfoOutputDTO> list = PropertyInfoService.GetPageList(pageIndex, pageSize, out count, HousID, State,MonthID);
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
    }
}