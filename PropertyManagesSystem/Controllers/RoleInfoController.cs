using DataModel;
using EntityModel.RoleInfoDTO;
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
    [MyActionFilter(RoleID = "1")]//只有超级管理员才能访问此页面
    public class RoleInfoController : BaseController
    {
        // GET: RoleInfo
        /// <summary>
        /// 主页
        /// </summary>
        /// <returns></returns>
        public ActionResult RoleIndex()
        {
            return View();
        }
        /// <summary>
        /// 添加视图
        /// </summary>
        /// <returns></returns>
        public ActionResult RoleAdd()
        {
            return View();
        }
        /// <summary>
        /// 修改视图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult RoleUpdate(int id)
        {
            //根据传过来的id获取选中账号的信息,并把查到的数据赋值给viewbag，以便页面绑定数据
            RoleInfo model = RoleInfoService.GetModelByID(id);
            if (model != null)
            {
                ViewBag.model = model;
            }
            return View();
        }
        /// <summary>
        /// 分配权限
        /// </summary>
        /// <param name="AdminID"></param>
        /// <returns></returns>
        public ActionResult ConfigIndex(string AdminID)
        {
            ViewBag.AdminID = AdminID;
            return View();
        }


        /// <summary>
        /// 分页查询
        /// </summary>
        /// <returns></returns>
        public string GetJsonList()
        {
            //分页查询
            //拿到前端传来的数据-->第几页，每页显示多少条
            int pageIndex = Convert.ToInt32(Request["page"]);
            int pageSize = Convert.ToInt32(Request["limit"]);

            //设置总条数，默认值为0
            int count = 0;

            //使用json.net序列化数据
            List<RoleInfo> list = RoleInfoService.GetPageList(pageIndex, pageSize, out count);
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
        /// 配置用户信息
        /// </summary>
        /// <param name="arrRoleID"></param>
        /// <param name="AdminID"></param>
        /// <returns></returns>
        public string ConfigRole(string arrRoleID, string AdminID)
        {
            if (RoleInfoService.ConfiRole(arrRoleID, AdminID))
            {
                return "ok";
            }
            else
            {
                return "no";
            }
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <returns></returns>
        public string GetRoleInfo()
        {
            int pageIndex = Convert.ToInt32(Request["page"]);
            int pageSize = Convert.ToInt32(Request["limit"]);
            //获取文本框的值:有可能为null  ，有可能为"" ，有可能为其他字符串
            string selectInfo = Request["selectInfo"];

            //设置总条数，默认值为0
            int count = 0;

            //使用json.net序列化数据
            List<RoleInfo> list = RoleInfoService.GetPageList(pageIndex, pageSize, out count, selectInfo);
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
        /// 添加
        /// </summary>
        /// <param name="RoleInfoJson">前端返回的JSON数据</param>
        /// <returns></returns>
        public string RoleInfoAdd(string RoleInfoJson)
        {       
            //Json转实体类
            RoleInfoInputDTO roleInfo = JsonConvert.DeserializeObject<RoleInfoInputDTO>(RoleInfoJson);
            //添加数据
            if (RoleInfoService.DTOAdd(roleInfo))
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
        /// <param name="RoleInfoJson">前端返回的JSON数据</param>
        /// <returns></returns>
        public string RoleInfoUpdate(string RoleInfoJson)
        {
            int outID; 
            //Json转实体类
            RoleInfoInputDTO roleInfo = JsonConvert.DeserializeObject<RoleInfoInputDTO>(RoleInfoJson);
            if (RoleInfoService.DTOUpdate(roleInfo, out outID)>0)
            {
                return "ok";
            }
            else
            {
                return "no";
            }
        }
        /// <summary>
        /// 查看用户编号是否已存在,如果存在，返回no
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string CheckRepeat(string value)
        {
            if (RoleInfoService.CheckRepeat(value))
            {
                //说明用户信息已存在--告诉前端，不合法
                return "no";
            }
            else
            {
                return "ok";
            }
        }

        /// <summary>
        /// 根据ID删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public string Delete(Array id)
        {
            if (RoleInfoService.Delete(id))
            {
                return "ok";
            }
            else
            {
                return "no";
            }
        }

        /// <summary>
        /// 禁用或开启用户--其实就是修改，
        /// </summary>
        /// <param name="id"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public string ForbidUser(int id, string flag)
        {
            //获取前端传过来的id和flag
            if (RoleInfoService.ForbidUser(id, flag))
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