using DataModel;
using EntityModel;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PropertyManagesSystem.Filters;
using EntityModel.VillageInfoDTO;

namespace PropertyManagesSystem.Controllers
{
    [MyActionFilter]
    public class VillageInfoController : BaseController
    {
        // GET: VillageInfo
        /// <summary>
        /// 主页
        /// </summary>
        /// <returns></returns>
        public ActionResult VillageIndex()
        {
            return View();
        }
        /// <summary>
        /// 添加页面
        /// </summary>
        /// <returns></returns>
        public ActionResult VillageAdd()
        {
            return View();
        }
        /// <summary>
        /// 修改页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult VillageUpdate(int id)
        {
            //根据传过来的id获取选中账号的信息,并把查到的数据赋值给viewbag，以便页面绑定数据
            VillageInfo model = VillageInfoService.GetModelByID(id);
            if (model != null)
            {
                ViewBag.model = model;
            }
            return View();
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <returns></returns>
        public string GetVillageInfo()
        {
            int pageIndex = Convert.ToInt32(Request["page"]);
            int pageSize = Convert.ToInt32(Request["limit"]);
            //获取文本框的值:有可能为null  ，有可能为"" ，有可能为其他字符串
            string selectInfo = Request["selectInfo"];

            //设置总条数，默认值为0
            int count = 0;

            //使用json.net序列化数据
            List<VillageInfo> list = VillageInfoService.GetPageList(pageIndex, pageSize, out count, selectInfo);
            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy'-'MM'-'dd hh':'mm':'ss"//格式化时间，默认是ISO8601格式
            };
            string strjson = JsonConvert.SerializeObject(list, Formatting.Indented, timeConverter);

            //字符串拼接，构造与layui规则相同的json对象数组字符串
            string str = "{\"code\": 0 ,\"msg\": \"\",\"count\": " + count + " ,\"data\":";
            str += strjson + "}";

            return str;
        }
        /// <summary>
        /// 添加方法
        /// </summary>
        /// <param name="VillageInfoJson">前端返回的JSON数据</param>
        /// <returns></returns>
        public string VillageInfoAdd(string VillageInfoJson)
        {
            //添加数据 
            //Json转实体类
            VillageInfoInputDTO villageInfo =  JsonConvert.DeserializeObject<VillageInfoInputDTO>(VillageInfoJson);
            if (VillageInfoService.DTOAdd(villageInfo))
            {
                return "ok";
            }
            else
            {
                return "no";
            }
        }
        /// <summary>
        /// 修改方法
        /// </summary>
        /// <param name="VillageInfoJson">前端返回的JSON数据</param>
        /// <returns></returns>
        public string VillageInfoUpdate(string VillageInfoJson)
        {
            int outID;
            //Json转实体类
            VillageInfoInputDTO villageInfo = JsonConvert.DeserializeObject<VillageInfoInputDTO>(VillageInfoJson);
            if (VillageInfoService.DTOUpdate(villageInfo, out outID)>0)
            {
                return "ok";
            }
            else
            {
                return "no";
            }
        }
        /// <summary>
        /// 检查是否已存在
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string CheckRepeat(string value)
        {
            if (VillageInfoService.CheckRepeat(value))
            {
                //说明小区信息已存在--告诉前端，不合法
                return "no";
            }
            else
            {
                return "ok";
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public string Delete(Array id)
        {
            if (VillageInfoService.Delete(id))
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