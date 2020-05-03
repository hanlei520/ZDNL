using Common;
using DataModel;
using EntityModel;
using EntityModel.AdminInfoDTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using PropertyManagesSystem.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PropertyManagesSystem.Controllers
{
    [MyActionFilter(RoleID = "1")]//只有超级管理员才能访问此页面
    public class AdminInfoController : BaseController
    {
        // GET: AdminInfo
        /// <summary>
        /// 主页
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminIndex()
        {
            return View();
        }
        /// <summary>
        /// 添加页面
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminAdd()
        {
            return View();
        }

        /// <summary>
        /// ajax JSON添加
        /// </summary>
        /// <param name="AdminInfoJson"></param>
        /// <returns></returns>
        public string AdminInfoAdd(string AdminInfoJson)
        {
            //Json转实体类
            AdminInfoInputDTO adminInfoInputDTO = JsonConvert.DeserializeObject<AdminInfoInputDTO>(AdminInfoJson);
            //添加数据
            if (AdminInfoService.DTOAdd(adminInfoInputDTO))
            {
                return "ok";
            }
            else
            {
                return "no";
            }
        }

        /// <summary>
        /// 添加页面
        /// </summary>
        /// <returns></returns>
        public ActionResult AddAdmin()
        {
            return View();
        }
        /// <summary>
        /// 修改页面
        /// </summary>
        /// <param name="AdminID"></param>
        /// <returns></returns>
        public ActionResult AdminUpdate(string AdminID)
        {
            //根据传过来的id获取选中账号的信息,并把查到的数据赋值给viewbag，以便页面绑定数据
            AdminInfo model = AdminInfoService.GetModelByID(AdminID);
            if (model != null)
            {
                ViewBag.model = model;
            }
            return View();
        }
        [AllowAnonymous]   //跳过验证
        /// <summary>
        /// 基本资料
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminAlter()
        {
            string AdminID = Session["AdminID"].ToString();
            //根据传过来的id获取选中账号的信息,并把查到的数据赋值给viewbag，以便页面绑定数据
            AdminInfo model = AdminInfoService.GetModelByID(AdminID);
            if (model != null)
            {
                ViewBag.model = model;
            }
            return View();
        }
        [AllowAnonymous]   //跳过验证
        /// <summary>
        /// 修改密码页面
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminResetPwd()
        {
            return View();
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <returns></returns>
        public string GetAdminInfo()
        {
            //分页查询
            //拿到前端传来的数据-->第几页，每页显示多少条
            int pageIndex = Convert.ToInt32(Request["page"]);
            int pageSize = Convert.ToInt32(Request["limit"]);
            //获取文本框的值:有可能为null  ，有可能为"" ，有可能为其他字符串
            string selectInfo = Request["selectInfo"];

            //设置总条数，默认值为0
            int count = 0;

            //使用json.net序列化数据
            List<AdminOutputDTO> list = AdminInfoService.GetPageList(pageIndex, pageSize, out count, selectInfo);
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
        /// 根据ID删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public string Delete(Array id)
        {
            if (AdminInfoService.Delete(id))
            {
                return "ok";
            }
            else
            {
                return "on";
            }
        }

        /// <summary>
        /// 查询用户编号是否存在 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public string CheckRepeat(string value)
        {
            if (AdminInfoService.CheckRepeat(value))
            {
                //说明用户编号已存在，告诉前端不合法
                return "no";
            }
            else
            {
                return "ok";
            }
        }

        /// <summary>
        /// 数据更新
        /// </summary>
        /// <param name="AdminInfoJson"></param>
        /// <returns></returns>
        public string AdminInfoUpdate(string AdminInfoJson)
        {
            //Json转实体类
            AdminInfoInputDTO adminInfoInputDTO = JsonConvert.DeserializeObject<AdminInfoInputDTO>(AdminInfoJson);
            //添加数据
            if (AdminInfoService.DTOUpdate(adminInfoInputDTO) > 0)
            {
                return "ok";
            }
            else
            {
                return "no";
            }
        }

        [AllowAnonymous]   //跳过验证
        /// <summary>
        /// 基本信息修改
        /// </summary>
        /// <param name="AdminInfoJson">前端返回的JSON数据</param>
        /// <returns></returns>
        public string AlterUpdate(string AdminInfoJson)
        {
            //Json转实体类
            AdminInfoInputDTO adminInfoInputDTO = new AdminInfoInputDTO { };
            adminInfoInputDTO = JsonConvert.DeserializeObject<AdminInfoInputDTO>(AdminInfoJson);
            if (AdminInfoService.DTOUpdate(adminInfoInputDTO) > 0)
            {
                return "ok";
            }
            else
            {
                return "no";
            }
        }
        [AllowAnonymous]   //跳过验证
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="AdminInfoJson">前端返回的JSON</param>
        /// <returns></returns>
        public string ResetPwd(string AdminInfoJson)
        {

            //JSON字符串转JSON对象
            JObject jo = (JObject)JsonConvert.DeserializeObject(AdminInfoJson);
            string AdminID = Session["AdminID"].ToString();
            if (jo != null)
            {
                //取值
                string PassWord = jo["PassWord"].ToString();
                string NewPwd = jo["NewPwd"].ToString();
                string Pwd = jo["Pwd"].ToString();
                if (AdminInfoService.ResetPwd(PassWord, AdminID, NewPwd, Pwd))
                {
                    return "ok";
                }
                else
                {
                    return "no";
                }
            }
            else
            {
                return "no";
            }

        }

        /// <summary>
        /// 根据ID重置密码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public string RePwd(Array id)
        {
            if (AdminInfoService.RePwd(id))
            {
                return "ok";
            }
            else
            {
                return "no";
            }
        }
        /// <summary>
        /// 禁用或开启
        /// </summary>
        /// <param name="id"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public string ForbidUser(int id, string flag)
        {
            //获取前端传过来的数据 id flag
            if (AdminInfoService.ForbidUser(id, flag))
            {
                return "ok";
            }
            else
            {
                return "no";
            }
        }

        [AllowAnonymous]   //跳过验证
        /// <summary>
        /// 查询当前密码是否正确 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public string CheckPwd(string value)
        {
            if (AdminInfoService.CheckPwd(value))
            {
                //说明密码正确，告诉前端合法
                return "ok";
            }
            else
            {
                return "no";
            }
        }
    }
}