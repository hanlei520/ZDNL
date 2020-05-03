using Common;
using DataModel;
using EntityModel;
using EntityModel.UserInfoDTO;
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
    public class UserInfoController : BaseController
    {
        // GET: UserInfo
        /// <summary>
        /// 业主管理主页
        /// </summary>
        /// <returns></returns>
        public ActionResult UserIndex()
        {
            return View();
        }
        /// <summary>
        /// 业主信息添加页面
        /// </summary>
        /// <returns></returns>
        public ActionResult UserAdd()
        {
            List<HouseInfo> houseInfos = HouseInfoService.GetTrueList();
            ViewBag.houseInfo = houseInfos;
            return View();
        }
        /// <summary>
        /// 业主信息修改页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult UserUpdate(int id)
        {
            //根据传过来的id获取选中账号的信息,并把查到的数据赋值给viewbag，以便页面绑定数据
            UserInfo model = UserInfoService.GetUserByID(id);
            List<HouseInfo> houseInfos = HouseInfoService.GetTrueList();
            ViewBag.houseInfo = houseInfos;
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
        public string GetUserInfo()
        {
            int pageIndex = Convert.ToInt32(Request["page"]);
            int pageSize = Convert.ToInt32(Request["limit"]);
            //获取文本框的值:有可能为null  ，有可能为"" ，有可能为其他字符串
            string selectInfo = Request["selectInfo"];

            //设置总条数，默认值为0
            int count = 0;

            //使用json.net序列化数据
            List<UserInfo> list = UserInfoService.GetPageList(pageIndex, pageSize, out count, selectInfo);
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
        /// 根据id重置密码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public string RePwd(Array id)
        {

            if (UserInfoService.RePwd(id))
            {
                return "ok";
            }
            else
            {
                return "no";
            }
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="UserInfoJson">前端返回的JSON数据</param>
        /// <returns></returns>
        public string UserInfoAdd(string UserInfoJson)
        {    
            //Json转实体类
            UserInfoInputDTO userInfo =  JsonConvert.DeserializeObject<UserInfoInputDTO>(UserInfoJson);
            //添加数据
            if (UserInfoService.DTOAdd(userInfo))
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
        /// <param name="UserInfoJson">前端返回的JSON数据</param>
        /// <returns></returns>
        public string UserInfoUpdate(string UserInfoJson)
        {
            int outID = 0;
            //Json转实体类
            UserInfoInputDTO userInfo = JsonConvert.DeserializeObject<UserInfoInputDTO>(UserInfoJson);
            if (UserInfoService.DTOUpdate(userInfo, out outID))
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
            if (UserInfoService.CheckRepeat(value))
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
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpPost]
        public string Delete(Array id)
        {
            if (UserInfoService.Delete(id))
            {
                return "ok";
            }
            else
            {
                return "no";
            }
        }
        /// <summary>
        /// 查询用户身份证是否已存在
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string CheckIDNumber(string value)
        {
            if (UserInfoService.CheckIDnumber(value))
            {
                //说明用户身份证已存在--告诉前端，不合法
                return "no";
            }
            else
            {
                return "ok";
            }
        }

    }
}