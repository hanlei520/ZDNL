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
    [MyUserActionFilter]
    public class User_CostInfoController : BaseController
    {
        // GET: User_CostInfo

        public ActionResult User_CostIndex()
        {
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
            string State = Request["State"];
            string HousID = Session["HousID"].ToString();
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
            List<CostInfoOutputDTO> list = CostInfoService.User_GetPageList(pageIndex, pageSize, out count, HousID, State,DRsubmitDate,DRsolveDate);
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