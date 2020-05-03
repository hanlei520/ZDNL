using Common;
using DataModel;
using EntityModel;
using EntityModel.FloorInfoDTO;
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
    public class FloorInfoController : BaseController
    {
        // GET: FloorInfo
        /// <summary>
        /// 楼栋主页
        /// </summary>
        /// <returns></returns>
        public ActionResult FloorIndex()
        {
            return View();
        }
        /// <summary>
        /// 添加页面
        /// </summary>
        /// <returns></returns>
        public ActionResult FloorAdd()
        {
            return View();
        }
        /// <summary>
        /// 修改页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult FloorUpdate(int id)
        {
            //根据传过来的id获取选中账号的信息,并把查到的数据赋值给viewbag，以便页面绑定数据
            FloorInfo model = FloorInfoService.GetModelByID(id);
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
        public string GetFloorInfo()
        {
            int pageIndex = Convert.ToInt32(Request["page"]);
            int pageSize = Convert.ToInt32(Request["limit"]);
            //获取文本框的值:有可能为null  ，有可能为"" ，有可能为其他字符串
            string selectInfo = Request["selectInfo"];

            //设置总条数，默认值为0
            int count = 0;

            //使用json.net序列化数据
            List<FloorInfo> list = FloorInfoService.GetPageList(pageIndex, pageSize, out count, selectInfo);
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
        /// <param name="model"></param>
        /// <returns></returns>
        public string FloorInfoAdd(string FloorInfoJson)
        {
            //添加数据            
            //Json转实体类
            FloorInfoInputDTO floorInfo = JsonConvert.DeserializeObject<FloorInfoInputDTO>(FloorInfoJson);
            if (FloorInfoService.DTOAdd(floorInfo))
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
        /// <param name="model"></param>
        /// <returns></returns>
        public string FloorInfoUpdate(string FloorInfoJson)
        {
            int outID;
            //添加数据
            //Json转实体类
            FloorInfoInputDTO floorInfo = JsonConvert.DeserializeObject<FloorInfoInputDTO>(FloorInfoJson);
            if (FloorInfoService.DTOUpdate(floorInfo, out outID) > 0)
            {
                return "ok";
            }
            else
            {
                return "no";
            }
        }

        /// <summary>
        /// 删除方法
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public string Delete(Array id)
        {
            if (FloorInfoService.Delete(id))
            {
                return "ok";
            }
            else
            {
                return "no";
            }
        }

        /// <summary>
        /// 查询楼栋编号是否存在 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public string CheckRepeat(string value)
        {
            if (FloorInfoService.CheckRepeat(value))
            {
                //说明楼栋编号已存在，告诉前端不合法
                return "no";
            }
            else
            {
                return "ok";
            }
        }
    }
}